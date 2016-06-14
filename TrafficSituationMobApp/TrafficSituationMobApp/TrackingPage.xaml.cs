using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using TrafficSituationMobApp.BingMaps;
using TrafficSituationMobApp.Data;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        public sealed partial class TrackingPage : Page
    {
        private Geolocator _geolocator = null;
        private BingMapsManager mapsManager;
        private DataProvider dataProvider;

        public TrackingPage()
        {
            this.InitializeComponent();
            mapsManager = new BingMapsManager();
            dataProvider = new DataProvider();
            _geolocator = new Geolocator();

            _geolocator.ReportInterval = 1000*30; //30sec

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }
            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StartTrackingButton.IsEnabled = true;
            StopTrackingButton.IsEnabled = false;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (StopTrackingButton.IsEnabled)
            {
               // _geolocator.PositionChanged-=
            }
            base.OnNavigatingFrom(e);
        }
        


        /// <summary>
        /// This is the event handler for PositionChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {

            try
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Geoposition pos = e.Position;

                    string latitude = pos.Coordinate.Point.Position.Latitude.ToString();
                    string longtitude = pos.Coordinate.Point.Position.Longitude.ToString();
                    LatitudeTextBlock.Text = latitude;
                    LongtitudeTextBlock.Text = longtitude;
                    //AddressTextBlock.Text = pos.CivicAddress.City;
                    SpeedTextBlock.Text = pos.Coordinate.Speed.ToString();
                    //TODO Пофиксить

                    DeviceLocation address = await mapsManager.FindALocationByPoint(latitude, longtitude);
                    AddressTextBlock.Text = address.ToString();

                    dataProvider.SendAutomaticallyGeneratedData(address, (double)pos.Coordinate.Speed * 3.6, "null");
                });
            }
            catch (Exception)
            {

               //Костыль
            }
            
        }

        

        private void StartTrackingButton_Click(object sender, RoutedEventArgs e)
        {
            _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            mapsManager = new BingMapsManager();

            StartTrackingButton.IsEnabled = false;
            StopTrackingButton.IsEnabled = true;
        }

        private void StopTrackingButton_Click(object sender, RoutedEventArgs e)
        {
            _geolocator.PositionChanged -= new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            

            StartTrackingButton.IsEnabled = true;
            StopTrackingButton.IsEnabled = false;
        }
    }
}
