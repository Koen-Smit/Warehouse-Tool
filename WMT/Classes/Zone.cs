using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMT.Classes
{

    public class Zones
    {
        public string? Id { get; set; }
        public Identifiers? Identifiers { get; set; }
        public List<LocationRange> Locations { get; set; }
    }

    public class Identifiers
    {
        public string? Label { get; set; }
    }


    public class LocationRange
    {
        public Location? Start { get; set; }
        public Location? End { get; set; }
    }

    public class Location
    {
        public string? Label { get; set; }
        public string? Type { get; set; }
    }

}

