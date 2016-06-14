using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrafficSituationMobApp.Data;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace TrafficSituationMobApp.Statistics
{
    public class RoadInformationManager
    {
        private DataProvider dataProvider;
        public RoadInformationManager()
        {
            dataProvider = new DataProvider();
        }

        public async Task<List<RoadInformation>> GetActualRoadInformation()
        {
            
            var roadInformationList = await dataProvider.GetActualRoadInformation();
            return roadInformationList;
        }


    }
}
