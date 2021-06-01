using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;

namespace MayDiseaseClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
            DataContext = this;
        }

        private System.Timers.Timer MyTimer { get; set; }
        private DateTime CachedDate { get; set; }

        private void Init()
        {
            MyTimer = new System.Timers.Timer
            {
                AutoReset = true,
                Enabled = true,
                Interval = 1000
            };
            MyTimer.Elapsed += MyTimer_Elapsed;
            MyTimer.Start();
        }

        private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var current = DateTime.Now;
            SetTime(current);
            SetDate(current);
        }

        private void SetDate(DateTime current)
        {
            if (current.Day == CachedDate.Day && current.Month == CachedDate.Month &&
                current.Year == CachedDate.Year) return;
            var day = GetDay(current);
            var month = current.Month > 4 ? 4 : current.Month;
            DateString = $"{month}月{day}日";
            WeekDayString = _weekDayDict[current.DayOfWeek];
            CachedDate = current;
        }

        private object GetDay(DateTime current)
        {
            if (current.Month <= 4) return current.Day;
            var ret = current.Day;
            for (var i = 4; i < current.Month; i++)
            {
                ret += DateTime.DaysInMonth(current.Year, i);
            }
            return ret;
        }

        private readonly IDictionary<DayOfWeek, string> _weekDayDict = new Dictionary<DayOfWeek, string>()
        {
            {DayOfWeek.Monday, "星期一"},
            {DayOfWeek.Tuesday, "星期二"},
            {DayOfWeek.Wednesday, "星期三"},
            {DayOfWeek.Thursday, "星期四"},
            {DayOfWeek.Friday, "星期五"},
            {DayOfWeek.Saturday, "星期六"},
            {DayOfWeek.Sunday, "星期日"}
        };

        private void SetTime(DateTime current)
        {
            Hour = current.Hour.ToString("D2");
            Minute = current.Minute.ToString("D2");
        }

        private string _hour;
        public string Hour
        {
            get => _hour;
            set
            {
                _hour = value;
                OnPropertyChanged("Hour");
            }
        }
        private string _minute;
        public string Minute
        {
            get => _minute;
            set
            {
                _minute = value;
                OnPropertyChanged("Minute");

            }
        }
        private string _dateString;
        public string DateString
        {
            get => _dateString;
            set
            {
                _dateString = value;
                OnPropertyChanged("DateString");
            }
        }
        private string _weekDayString;
        public string WeekDayString
        {
            get => _weekDayString;
            set
            {
                _weekDayString = value;
                OnPropertyChanged("WeekDayString");

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
