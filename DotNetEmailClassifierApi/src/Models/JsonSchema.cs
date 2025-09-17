using Newtonsoft.Json;
using System.Collections.Generic;

namespace DotNetEmailClassifierApi.Models
{
    public class JsonSchemaObject
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("schema")]
        public SchemaDetail? Schema { get; set; }
    }

    public class SchemaDetail
    {
        [JsonProperty("$schema")]
        public string? SchemaVersion { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, PropertyDetail>? Properties { get; set; }

        [JsonProperty("required")]
        public List<string>? Required { get; set; }
    }

    public class PropertyDetail
    {
        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("minimum")]
        public double? Minimum { get; set; }

        [JsonProperty("maximum")]
        public double? Maximum { get; set; }

        [JsonProperty("items")]
        public PropertyDetail? Items { get; set; }
    }
}