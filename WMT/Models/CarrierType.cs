using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace WMT.Classes
{
    internal class CarrierTypes
    {
        public class Dimensions
        {
            [JsonProperty("length")]
            public int Length { get; set; }
            [JsonProperty("width")]
            public int Width { get; set; }
            [JsonProperty("height")]
            public int Height { get; set; }
        }

        public class Limitations
        {
            [JsonProperty("maxWeight")]
            public int MaxWeight { get; set; }
            [JsonProperty("maxVolume")]
            public int MaxVolume { get; set; }
        }

        public class Position
        {
            //
        }


        public class Root
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("dimensions")]
            public Dimensions Dimensions { get; set; }
            [JsonProperty("limitations")]
            public Limitations Limitations { get; set; }
            [JsonProperty("positions")]
            public List<Position> Positions { get; set; }
        }
    }
}
