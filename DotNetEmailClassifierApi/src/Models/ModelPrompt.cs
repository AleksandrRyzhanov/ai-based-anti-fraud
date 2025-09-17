using Newtonsoft.Json;

namespace DotNetEmailClassifierApi.Models
{
    public class ModelPrompt
    {
        [JsonProperty("model")]
        public string? Model { get; set; }

        [JsonProperty("messages")]
        public ModelMessage[] Messages { get; set; } = new ModelMessage[] { };

        [JsonProperty("temperature")]
        public int Temperature { get; set; } = 0;

        [JsonProperty("max_tokens")]
        public int MaxTokens { get; set; } = 1000;

        [JsonProperty("stream")]
        public bool Stream { get; set; } = false;

        [JsonProperty("response_format")]
        public ResponseFormat? ResponseFormat { get; set; }
    }

    public class ResponseFormat
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "json_schema";

        [JsonProperty("json_schema")]
        public JsonSchemaObject? JsonSchema { get; set; }
    }
}



public class ModelMessage
{
    public string role { get; set; } = "user";
    public string content { get; set; } = "";
}