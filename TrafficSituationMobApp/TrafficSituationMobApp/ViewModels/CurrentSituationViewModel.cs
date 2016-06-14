using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using System.Text.RegularExpressions;
using TrafficSituationMobApp.Statistics;

namespace TrafficSituationMobApp.ViewModels
{

    //Уже бесполезный класс
    class CurrentSituationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MapPolyline> polylinesCollection;
        private RoadInformationManager roadInformationManager = new RoadInformationManager();

        public ObservableCollection<MapPolyline> PolylinesCollection
        {
            get
            {
                return polylinesCollection;
            }

            set
            {
                polylinesCollection = value;
                RaisePropertyChanged("PolylinesCollection");
            }
        }

        private DelegateCommand getActualRoadInformationCommand;
        public ICommand GetActualRoadInformationButtonClick
        {
            get
            {
                if (getActualRoadInformationCommand == null)
                {
                    getActualRoadInformationCommand = new DelegateCommand(GetActualRoadInformation);
                }

                return getActualRoadInformationCommand;
            }
        }

        private async void GetActualRoadInformation(object obj)
        {
            var roadInformationList = await roadInformationManager.GetActualRoadInformation();

            var bufPolylinesCollection = new ObservableCollection<MapPolyline>();

            foreach (var item in roadInformationList)
            {
                MapPolyline line = new MapPolyline();
                line.StrokeColor = GetRoadColor(item.TrafficLevel);
                line.StrokeThickness = 5;
                line.Path = GetGeopath(item.Geoposition);
                bufPolylinesCollection.Add(line);
            }

            PolylinesCollection = bufPolylinesCollection;
        }

        private Geopath GetGeopath(string inputString)
        {
            var geopositionList = new List<BasicGeoposition>();
            string pattern = @"\[(\d+.\d+)[^\d]\s(\d+.\d+)\]";
            foreach (Match item in Regex.Matches(inputString, pattern))
            {
                geopositionList.Add(new BasicGeoposition
                {
                    Latitude = Convert.ToDouble(item.Groups[1].Value),
                    Longitude = Convert.ToDouble(item.Groups[2].Value),
                });
            }

            return new Geopath(geopositionList);
        }

        private Color GetRoadColor(int trafficLevel)
        {
            if (trafficLevel>=0 && trafficLevel<=3)
            {
                return Colors.Green;
            }
            if (trafficLevel >= 4 && trafficLevel <= 6)
            {
                return Colors.Yellow;
            }
            if (trafficLevel >= 7 && trafficLevel <= 10)
            {
                return Colors.Red;
            }
            else
            {
                return Colors.Black;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName]string propertyName = "") // волшебство .NET 4.5
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
