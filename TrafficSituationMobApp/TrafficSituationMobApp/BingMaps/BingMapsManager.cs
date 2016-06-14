using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using TrafficSituationMobApp.Data;

namespace TrafficSituationMobApp.BingMaps
{
    public class BingMapsManager
    {



        public async Task<DeviceLocation> FindALocationByPoint(string latitude, string longtitude)
        {
            string requestUrl = CreateRequest(latitude + "," + longtitude);
            Response response = await MakeRequest(requestUrl);
            DeviceLocation address = GetFullAddress(response);

            return address;

        }

        //Create the request URL
        static string CreateRequest(string queryString)
        {
            string UrlRequest = "http://dev.virtualearth.net/REST/v1/Locations/" +
                                           queryString +
                                           "?key=" + "Aii2AKhWB1TCCgQflyCBHGoV6I-GnYZx1RwUy4RDrODsFNVcysWxugM-YFLdvGAd";
            return (UrlRequest);
        }
        static async Task<Response> MakeRequest(string requestUrl)
        {
            try
            {
                HttpClient client = new HttpClient();
                Task<string> responseTask = client.GetStringAsync(new Uri(requestUrl));
                string responseString = await responseTask;
                Response jsonResponse = JsonConvert.DeserializeObject<Response>(responseString);
                return jsonResponse;
                
            }
            catch (Exception e)
            {
                //дописать обработчик
                return null;
            }
        }

        public DeviceLocation GetFullAddress(Response locationsResponse)
        {
            Location location = (Location)locationsResponse.ResourceSets[0].Resources[0];
            DeviceLocation deviceLocation = new DeviceLocation()
            {
                Country = location.Address.CountryRegion,
                City = location.Address.Locality,
                Street = location.Address.AddressLine
            };

            return deviceLocation;

        }
    }
}
