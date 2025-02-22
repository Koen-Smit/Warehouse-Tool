using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;


namespace WMT.Classes
{
    internal class Articles
    {
        public class Dimensions
        {
            [JsonProperty("length")]
            public int Length { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
            [JsonProperty("weight")]
            public int Weight { get; set; }
            [JsonProperty("volume")]
            public int Volume { get; set; }
        }

        public class Identifiers
        {
            [JsonProperty("label")]
            public string Label { get; set; }
        }

        public class Root
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("identifiers")]
            public Identifiers Identifiers { get; set; }
            [JsonProperty("description")]
            public string Description { get; set; }
            [JsonProperty("dimensions")]
            public Dimensions Dimensions { get; set; }
        }
    }
}
