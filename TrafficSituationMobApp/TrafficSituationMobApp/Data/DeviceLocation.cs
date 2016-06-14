using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSituationMobApp.Data
{
    public class DeviceLocation
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public override string ToString()
        {
            return Country + ", " + City + ", " + Street;
        }
    }
}
