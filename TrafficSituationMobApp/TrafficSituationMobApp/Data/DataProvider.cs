using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSituationMobApp.Data
{
    public class DataProvider
    {
        const string uri = "http://localhost:20350//api/Values";
        const string statisticsUrl = "http://localhost:20350//api/Archives/?";
        const string roadInformationUrl = "http://localhost:20350//api/RoadInformation/";

        public async void SendAutomaticallyGeneratedData(DeviceLocation address, double speed, string acceleration)
        {
            AutomaticallyGeneratedMessage autoGenData = new AutomaticallyGeneratedMessage()
            {
                Address = address,
                Acceleration = acceleration,
                Speed = speed,
                DateOfCreation = DateTime.Now
            };

            var jsonData = JsonConvert.SerializeObject(autoGenData);

            HttpClient client = new HttpClient();

            await client.PostAsync(new Uri(uri), new StringContent(jsonData, Encoding.UTF8, "text/json"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="fromDate">Start date</param>
        /// <param name="resultType">Per hour or per current time</param>
        /// <returns>Collections of TrafficData</returns>
        public async Task<ObservableCollection<TrafficData>> GetStatistics(DeviceLocation address, DateTime fromDate, string resultType)
        {
            //TODO сделать контроллер Get

            var requestUrl = CombineStatisticsUrl(address, fromDate, resultType);

            HttpClient client = new HttpClient();
            Task<string> responseTask = client.GetStringAsync(new Uri(requestUrl));
            string responseString = await responseTask;
            ObservableCollection<TrafficData> jsonResponse = JsonConvert.DeserializeObject<ObservableCollection<TrafficData>>(responseString);

            return jsonResponse;
        }

        public string CombineStatisticsUrl(DeviceLocation address, DateTime fromDate, string resultType)
        {
            string requestUrl = String.Format("{0}Country={1}&City={2}&Street={3}&Date={4}&TypeRequest={5}", statisticsUrl,
                address.Country, address.City, address.Street, fromDate, resultType);

            requestUrl = requestUrl.Replace(" ", "%20");

            return requestUrl;
        }

        public async Task<List<RoadInformation>> GetActualRoadInformation()
        {
            HttpClient client = new HttpClient();
            Task<string> responseTask = client.GetStringAsync(new Uri(roadInformationUrl));

            string responseString = await responseTask;
            List<RoadInformation> jsonResponse = JsonConvert.DeserializeObject<List<RoadInformation>>(responseString);

            return jsonResponse;
        }
    }
}
