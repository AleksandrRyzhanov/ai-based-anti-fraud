using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DotNetEmailClassifierApi.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace DotNetEmailClassifierApi.Services
{
    public class AiServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SchemaProvider _schemaProvider;
        private readonly IssueTypesProvider _issueTypesProvider;

        public AiServiceClient(HttpClient httpClient,
            IConfiguration configuration,
            SchemaProvider schemaProvider,
            IssueTypesProvider issueTypesProvider)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _schemaProvider = schemaProvider;
            _issueTypesProvider = issueTypesProvider;
        }

        public async Task<ClassificationResponse> SendEmailForClassification(EmailRequest emailRequest)
        {
            var modelPrompt = await GetModelPrompt(emailRequest);
            var jsonContent = JsonConvert.SerializeObject(modelPrompt, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore });
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            Console.Out.WriteLine($"Sending request to AI service at {GetServiceUrl()} with model {modelPrompt.Model}");
            Console.Out.WriteLine($"Request content: {jsonContent}");

            var response = await _httpClient.PostAsync(GetServiceUrl(), content);

            if (response.IsSuccessStatusCode)
            {
                if (response.Content == null)
                {
                    throw new HttpRequestException("Response content is null");
                }
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ModelResponse>(jsonResponse);
                
                if (result == null)
                {
                    throw new HttpRequestException("Failed to deserialize response from AI service.");
                }

                if (result.Choices == null || result.Choices.Count == 0)
                {
                    throw new HttpRequestException("No choices returned from AI service.");
                }
                var choicesContent = result.Choices[0].Message?.Content;

                if (choicesContent == null)
                {
                    throw new HttpRequestException("No content provided by AI service");
                }
                
                var classificationResponse = JsonConvert.DeserializeObject<ClassificationResponse>(choicesContent);

                if (classificationResponse == null)
                {
                    throw new HttpRequestException("Failed to deserialize classification response from AI service.");
                }

                return classificationResponse;
            }

            throw new HttpRequestException("Error calling AI service: " + response.ReasonPhrase);
        }

        private string GetPromptTemplate()
        {
            return _configuration["AiService:PromptTemplate"] ?? "Please classify the following input: {message} on topic {topic}.";
        }
        private string GetServiceUrl()
        {
            return _configuration["AiService:Url"] ?? "http://localhost:1234/v1/chat/completions";
        }   
        private string GetSystemMessage()
        {
            return _configuration["AiService:SystemMessage"] ?? "You are an email classifier.";
        }
        private async Task<ModelPrompt> GetModelPrompt(EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                throw new ArgumentException("Email message cannot be null or empty.", nameof(request.Message));
            }
            if (string.IsNullOrWhiteSpace(request.Model))
            {
                throw new ArgumentException("Model cannot be null or empty.", nameof(request.Model));
            }
            int topicId;
            if (!int.TryParse(request.TopicId, out topicId) || topicId <= 0)
            {
                throw new ArgumentException("TopicId must be a positive integer.", nameof(request.TopicId));
            }
            var issueTypes = await _issueTypesProvider.GetIssueTypesJsonAsync();
            if (issueTypes.IssueTypeList == null || issueTypes.IssueTypeList.Count == 0)
            {
                throw new InvalidOperationException("Issue types list is empty.");
            }
            var topic = issueTypes.IssueTypeList.Find(t => t.Id == topicId);
            if (topic == null)
            {
                throw new ArgumentException($"No issue type found for TopicId {request.TopicId}.", nameof(request.TopicId));
            }

            var schemaJson = await _schemaProvider.GetSchemaJsonAsync();
            var schemaObj = JsonConvert.DeserializeObject<JsonSchemaObject>(schemaJson);
            if (schemaObj == null)
            {
                throw new HttpRequestException("Failed to deserialize JSON schema.");
            }

            var prompt = new ModelPrompt
            {
                Model = request.Model,
                Messages = new[]
                {
                    new ModelMessage { role = "system", content = GetSystemMessage() },
                    new ModelMessage { role = "user", content = GetPromptTemplate()
                        .Replace("{message}", request.Message)
                        .Replace("{topic}", topic.Type)
                        .Replace("{example}", topic.Example) }
                },
                Temperature = 0,
                MaxTokens = 1000,
                Stream = false,
                ResponseFormat = new ResponseFormat
                {
                    Type = "json_schema",
                    JsonSchema = schemaObj
                }
            };
            return prompt;
        }
    }
}