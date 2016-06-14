using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using TrafficSituationMobApp.Data;
using TrafficSituationMobApp.Statistics;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TrafficSituationMobApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CurrentSituationPage : Page
    {
        GeolocatorManager geolocatorManager;
        DataProvider dataProvider;
        private RoadInformationManager roadInformationManager;
        public CurrentSituationPage()
        {
            this.InitializeComponent();
            geolocatorManager = new GeolocatorManager();
            dataProvider = new DataProvider();
            roadInformationManager = new RoadInformationManager();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            MainMap.Center = await geolocatorManager.GetCurrentGeopoint();
        }

        private void TrackingButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TrackingPage));
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(StatisticsPage));
        }

        private async void CurrentPositionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var geopoint = await geolocatorManager.GetCurrentGeopoint();
                MainMap.ZoomLevel = 15;

                MapIcon mapIcon = new MapIcon();
                mapIcon.Location = geopoint;
                mapIcon.Title = "You are here";
                MainMap.MapElements.Add(mapIcon);
                MainMap.Center = geopoint;
            }
            catch (Exception)
            {
                //Запилить обработку
            }
            
        }

        private async void CurrentSituationButton_Click(object sender, RoutedEventArgs e)
        {
            //MainMap.TrafficFlowVisible = true; // Тест
            var roadInformationList = await roadInformationManager.GetActualRoadInformation();

            MainMap.MapElements.Clear();

            foreach (var item in roadInformationList)
            {
                MapPolyline line = new MapPolyline();
                line.StrokeColor = GetRoadColor(item.TrafficLevel);
                line.StrokeThickness = 5;
                line.Path = GetGeopath(item.Geoposition);

                MainMap.MapElements.Add(line);
            }

            
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
            if (trafficLevel >= 0 && trafficLevel <= 3)
            {
                return Colors.Green;
            }
            if (trafficLevel >= 4 && trafficLevel <= 6)
            {
                return Colors.Orange;
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

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            MainMap.ZoomLevel = MainMap.ZoomLevel < 20 ? ++MainMap.ZoomLevel : MainMap.ZoomLevel;
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            
            MainMap.ZoomLevel = MainMap.ZoomLevel>1? --MainMap.ZoomLevel :MainMap.ZoomLevel;
        }
    }
}
