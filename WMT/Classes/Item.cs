using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMT.Classes
{
    internal class Items
    {
        public class Article
        {
            public string Id { get; set; }
            public Identifiers Identifiers { get; set; }
            public string Description { get; set; }
            public Dimensions Dimensions { get; set; }
            public List<Part> Parts { get; set; }
        }

        public class Dimensions
        {
            public int Length { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int Weight { get; set; }
            public int Volume { get; set; }
        }

        public class Identifiers
        {
            public string AdditionalProp1 { get; set; }
            public string AdditionalProp2 { get; set; }
            public string AdditionalProp3 { get; set; }
        }

        public class Part
        {
            public string Article { get; set; }
            public int Quantity { get; set; }
        }

        public class Reservation
        {
            public string Id { get; set; }
            public string Type { get; set; }
        }

        public class Root
        {
            public string Id { get; set; }
            public Article Article { get; set; }
            public Reservation Reservation { get; set; }
        }

    }
}
