using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrafficSituationMobApp.Data;
using TrafficSituationMobApp.Statistics;
using Windows.UI.Xaml;

namespace TrafficSituationMobApp.ViewModels
{
    class StatisticsViewModel : INotifyPropertyChanged
    {

        private string country = String.Empty;
        private string city = String.Empty;
        private string street = String.Empty;
        private TimeSpan currentTime = DateTime.Now.TimeOfDay;
        private DateTimeOffset currentDate = DateTimeOffset.Now;
        private DateTime currentDateTime;

        private ObservableCollection<TrafficData> trafficDataCollection = new ObservableCollection<TrafficData>();

        private bool isStatisticsPerCurrentTimeChecked = true; //default value
        private bool isStatisticsPerHourChecked = false;
         

        private StatisticsManager statisticsModel = new StatisticsManager();
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                RaisePropertyChanged();
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                RaisePropertyChanged();
            }
        }

        public string Street
        {
            get { return street; }
            set
            {
                street = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TrafficData> TrafficDataCollection
        {
            get
            {
                return trafficDataCollection;
            }

            set
            {
                trafficDataCollection = value;
                RaisePropertyChanged("TrafficDataCollection");
            }
        }

        private DelegateCommand getTrafficDataCommand;

        public ICommand GetTrafficDataButtonClick
        {
            get
            {
                if (getTrafficDataCommand==null)
                {
                    getTrafficDataCommand = new DelegateCommand(GetTrafficData);
                }

                return getTrafficDataCommand;
            }
        }

        public bool IsStatisticsPerCurrentTimeChecked
        {
            get
            {
                return isStatisticsPerCurrentTimeChecked;
            }

            set
            {
                isStatisticsPerCurrentTimeChecked = value;
                RaisePropertyChanged("IsStatisticsPerCurrentTimeChecked");
            }
        }

        public bool IsStatisticsPerHourChecked
        {
            get
            {
                return isStatisticsPerHourChecked;
            }

            set
            {
                isStatisticsPerHourChecked = value;
                RaisePropertyChanged("IsStatisticsPerHourChecked");
            }
        }

        public DateTime CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }

            set
            {
                currentDateTime = value;
                RaisePropertyChanged("CurrentDateTime");
            }
        }

        public TimeSpan CurrentTime
        {
            get
            {
                return currentTime;
            }

            set
            {
                currentTime = value;
                RaisePropertyChanged("CurrentTime");
                UpdateDateTime();
            }
        }

        public DateTimeOffset CurrentDate
        {
            get
            {
                return currentDate;
            }

            set
            {
                currentDate = value;
                RaisePropertyChanged("CurrentDate");
                UpdateDateTime();
            }
        }

        private void UpdateDateTime()
        {
            CurrentDateTime = currentDate.Date + CurrentTime;
            
            
        }
        private async void GetTrafficData(object o)
        {
            UpdateDateTime();
            if (statisticsModel != null)
            {
                
                TrafficDataCollection = new ObservableCollection<TrafficData>(await statisticsModel.GetStatistics(Country, City, Street, CurrentDateTime,IsStatisticsPerCurrentTimeChecked?"PerCurrentTime":"PerHour"));

            }
        }

        

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "") // волшебство .NET 4.5
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
