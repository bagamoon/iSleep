using System;
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
using Visifire.Commons;
using iSleep.Model;
using System.Windows.Navigation;

namespace iSleep.DataChart
{
    public partial class QuantifyChart : PhoneApplicationPage
    {
        private SettingService _settingService = new SettingService();
        private SleepService _sleepService = new SleepService();
        private DateTime _currentViewDate = DateTime.Now;

        public QuantifyChart()
        {
            InitializeComponent();
            CreateChart(false);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _currentViewDate = _currentViewDate.AddMonths(-1);
            CreateChart(false);
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            CreateChart(true);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentViewDate = _currentViewDate.AddMonths(1);
            CreateChart(false);
        }

        public void CreateChart(bool isRenderAll)
        {
            Chart chart = new Chart();

            chart.AxesX.Add(new Axis { Title = "睡眠量" });
            chart.AxesY.Add(new Axis { Title = "Day" });

            DataSeries dataSeries = new DataSeries();
            dataSeries.RenderAs = RenderAs.Bar;

            IList<SleepModel> data = new List<SleepModel>();

            Title title = new Title();
            if (isRenderAll)
            {
                title.Text = "睡眠量統計 - All";
                data = _sleepService.GetSleepDataAll();
            }
            else
            {
                title.Text = "睡眠量統計 - " + _currentViewDate.ToString("yyyy/MM");
                data = _sleepService.GetSleepDataByDate(_currentViewDate);
            }
            chart.Titles.Add(title);

            var dataGroup = data.GroupBy(d => d.SleepQuantify)
                                .OrderBy(g => (SleepQuantify)g.Key);

            foreach (var group in dataGroup)
            {
                DataPoint dataPoint = new DataPoint();                
                dataPoint.YValue = group.Count();
                dataPoint.ToolTipText = group.Count().ToString();
                dataPoint.AxisXLabel = CommonService.GetEnumDescription(group.Key);
                                
                dataPoint.Color = GetColor(group.Key);

                dataSeries.DataPoints.Add(dataPoint);
            }

            chart.Series.Add(dataSeries);
            ContentPanel.Children.Add(chart);            
        }

        private Brush GetColor(SleepQuantify quantify )
        {            
            switch (quantify)
            { 
                case SleepQuantify.Lacked:
                    return new SolidColorBrush(Colors.Red);
                    
                case SleepQuantify.Tolerable:
                    return new SolidColorBrush(Colors.Orange);
                    
                case SleepQuantify.Medium:
                    return new SolidColorBrush(Colors.Yellow);
                    
                case SleepQuantify.Enough:
                    return new SolidColorBrush(Colors.Blue);
                    
                case SleepQuantify.Satisfied:
                    return new SolidColorBrush(Colors.Green);
                    
                default:
                    return new SolidColorBrush(Colors.Green);
            }

        }
    }
}