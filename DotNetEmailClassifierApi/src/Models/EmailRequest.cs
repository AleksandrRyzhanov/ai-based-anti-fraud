using Newtonsoft.Json;
namespace DotNetEmailClassifierApi.Models
{
    public class EmailRequest
    {
        [JsonProperty("message")]
        public string? Message { get; set; }
        [JsonProperty("model")]
        public string? Model { get; set; }
        [JsonProperty("topic_id")]
        public string? TopicId { get; set; }
    }
}