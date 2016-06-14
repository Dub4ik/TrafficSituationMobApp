using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficSituationMobApp.Data;

namespace TrafficSituationMobApp.Statistics
{
    class StatisticsManager
    {
        DataProvider dataProvider = new DataProvider();
        public async Task<ObservableCollection<TrafficData>> GetStatistics(string country,
            string city, string street,DateTime time, string requestType)
        {

            var result = dataProvider.GetStatistics(
                new DeviceLocation() { Country = country, City = city, Street = street },
                time, requestType);
            var list = await result;

            return list;
        }
    }
}
