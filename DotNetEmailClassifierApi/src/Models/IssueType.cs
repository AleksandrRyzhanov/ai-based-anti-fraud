using Newtonsoft.Json;
using System.Collections.Generic;

namespace DotNetEmailClassifierApi.Models
{
    public class IssueType
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        [JsonProperty("example")]
        public string Example { get; set; } = string.Empty;
        [JsonProperty("relevant")]
        public string RelevantMessage { get; set; } = string.Empty;
        [JsonProperty("irrelevant")]
        public string IrrelevantMessage { get; set; } = string.Empty;
        [JsonProperty("partially_relevant")]
        public string PartiallyRelevantMessage { get; set; } = string.Empty;
    }

    public class IssueTypes
    {
        [JsonProperty("issue_types")]
        public List<IssueType> IssueTypeList { get; set; } = new();
    }
}