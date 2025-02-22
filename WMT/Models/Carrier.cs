using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMT.Classes
{
    internal class Carriers
    {
        public class Dimensions
        {
            public int Length { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }

        public class Identifiers
        {
            public string Label { get; set; }
        }

        public class Limitations
        {
            public int MaxWeight { get; set; }
            public int MaxVolume { get; set; }
        }

        public class Position
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string Type { get; set; }
        }

        public class Root
        {
            public string Id { get; set; }
            public Identifiers Identifiers { get; set; }
            public Type Type { get; set; }
            public List<Position> Positions { get; set; }
        }

        public class Type
        {
            public string Id { get; set; }
            public string LabelPattern { get; set; }
            public Dimensions Dimensions { get; set; }
            public Limitations Limitations { get; set; }
            public List<string> PositionLabels { get; set; }
        }
    }
}
