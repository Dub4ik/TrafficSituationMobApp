using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TrafficSituationMobApp.Data
{
    public class RoadInformation
    {
        public int TrafficLevel { get; set; }
        public string Geoposition { get; set; }
    }
}
