﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using iSleep.Service;
using Visifire.Charts;
using System.Windows.Navigation;

namespace iSleep.DataChart
{
    public partial class WakeTrend : PhoneApplicationPage
    {
        public WakeTrend()
        {
            InitializeComponent();
            CreateChart();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private SettingService _settingService = new SettingService();
        private SleepService _sleepService = new SleepService();
        private ReportService _reportService = new ReportService();
        private DateTime _currentViewDate = DateTime.Now;

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _currentViewDate = _currentViewDate.AddMonths(-1);
            CreateChart();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentViewDate = _currentViewDate.AddMonths(1);
            CreateChart();
        }

        public void CreateChart()
        {
            var setting = _settingService.GetCurrentSetting();

            Chart chart = new Chart();

            chart.AxesY.Add(new Axis { Title = "目標起床時間：" + setting.TargetWakeTime.ToString("HH:mm") });

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Line;
            dataSeries.Color = new SolidColorBrush(Colors.Green);
            chart.Series.Add(dataSeries);

            var trendData = _reportService.GetWakeTrend(_currentViewDate);

            Title title = new Title();

            title.Text = "起床趨勢 - " + _currentViewDate.ToString("yyyy/MM");

            chart.Titles.Add(title);

            foreach (var data in trendData)
            {
                DataPoint dataPoint = new DataPoint();
                dataPoint.YValue = data.YValue;
                dataPoint.ToolTipText = data.ToolTipText;

                dataSeries.DataPoints.Add(dataPoint);
            }

            ContentPanel.Children.Add(chart);
        }
    }
}