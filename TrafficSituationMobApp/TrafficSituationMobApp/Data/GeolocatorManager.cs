using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TrafficSituationMobApp.Data
{
    class GeolocatorManager
    {
        Geolocator geolocator;
        public GeolocatorManager()
        {
            geolocator = new Geolocator();

            geolocator.ReportInterval = 1000;
        }

        public async Task<Geopoint> GetCurrentGeopoint()
        {
            var geoposition = await geolocator.GetGeopositionAsync();
            Geopoint geopoint = geoposition.Coordinate.Point;
            return geopoint;
        }
    }
}
