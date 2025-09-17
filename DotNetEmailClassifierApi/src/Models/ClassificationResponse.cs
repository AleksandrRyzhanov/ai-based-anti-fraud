using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNetEmailClassifierApi.Models
{   
    public class ClassificationResponse
    {
        [JsonProperty("relevant_score")]
        public double RelevantScore { get; set; }
        [JsonProperty("irrelevancy_reasons")]
        public List<string> IrrelevancyReasons { get; set; } = new();
        [JsonProperty("elapsed_time")]
        public double ElapsedTime { get; set; } // in seconds
    }
}