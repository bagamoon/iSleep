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
using iSleep.Dao;
using iSleep.Service;
using System.Windows.Threading;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using iSleep.Helper;

namespace iSleep
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SettingService _settingService = new SettingService();
        private SleepService _sleepService = new SleepService();
        private DateTime _currentViewDate = DateTime.Now;
        private int _secretCount = 0;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            InitialTimer();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowSleepButton();
            BindSleepDetailData();
        }

        private void btnSleep_Click(object sender, RoutedEventArgs e)
        {
            _sleepService.Sleep();
            txtNowSleepInterval.Text = "";
            ShowSleepButton();
        }

        private void btnAwake_Click(object sender, RoutedEventArgs e)
        {
            var sleep = _sleepService.Wake();
            ShowSleepButton();

            MessageBox.Show(string.Format("就寢時間: {0} \n起床時間: {1} \n總計: {2} \n評價: {3}",
                                            sleep.SleepTimeString,
                                            sleep.WakeTimeString,
                                            sleep.SleepIntervalHour,
                                            CommonService.GetEnumDescription(sleep.SleepQuantify),
                            "記錄成功!", 
                            MessageBoxButton.OK));
        }

        private void btnTestData_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("此動作會產生今年初至今天的測試資料並覆蓋現有資料，確定繼續？", "產生測試資料", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _sleepService.GenerateTestData();
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _currentViewDate = _currentViewDate.AddMonths(-1);
            BindSleepDetailData();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentViewDate = _currentViewDate.AddMonths(1);
            BindSleepDetailData();
        }

        private void btnSleepTrend_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/DataChart/SleepTrend.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnWakeTrend_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/DataChart/WakeTrend.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnQuantifyChart_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/DataChart/QuantifyChart.xaml", UriKind.RelativeOrAbsolute));
        }       

        private void pivotMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem item = e.AddedItems[0] as PivotItem;
            
            if(item != null)
            {
                switch (item.Name)
                {
                    case "pivotItemOperation":
                        ShowSleepButton();
                        this.ApplicationBar.IsVisible = true;
                        break;
                    
                    case "pivotItemDetails":
                        BindSleepDetailData();
                        this.ApplicationBar.IsVisible = false;
                        break;
                    
                    case "pivotItemStatistic":
                        this.ApplicationBar.IsVisible = false;
                        break;
                }
            }
        }

        private void InitialTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(RefreshSleepInterval);
            timer.Start();
        }

        private void RefreshSleepInterval(object sender, EventArgs e)
        {
            var setting = _settingService.GetCurrentSetting();

            if (setting.LastSleepTime != null)
            {
                DateTime lastSleepTime = (DateTime)setting.LastSleepTime;
                txtLastSleep.Text = lastSleepTime.ToString("MM/dd HH:mm");

                TimeSpan nowSleepInterval = DateTime.Now - lastSleepTime;
                txtNowSleepInterval.Text = nowSleepInterval.Days + " 天 " +
                                           nowSleepInterval.Hours + " 時 " +
                                           nowSleepInterval.Minutes + " 分 " +
                                           nowSleepInterval.Seconds + " 秒 ";
            }
        }

        private void ShowSleepButton()
        {
            var setting = _settingService.GetCurrentSetting();

            if (setting.NowStatus == Model.SleepStatus.Waking)
            {
                btnSleep.Visibility = System.Windows.Visibility.Visible;
                btnAwake.Visibility = System.Windows.Visibility.Collapsed;
                panelSleep.Visibility = System.Windows.Visibility.Collapsed;                
            }
            else
            {
                btnSleep.Visibility = System.Windows.Visibility.Collapsed;
                btnAwake.Visibility = System.Windows.Visibility.Visible;
                panelSleep.Visibility = System.Windows.Visibility.Visible;                
            }

        }

        private void BindSleepDetailData()
        {
            txtCurrentDate.Text = _currentViewDate.ToString("yyyy/MM");
            var sleepData = _sleepService.GetSleepDataByDate(_currentViewDate)
                            .OrderByDescending(d => d.SleepTime);

            listBoxDetail.ItemsSource = sleepData.ToList();  
        }

        private void SecretTextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _secretCount = _secretCount + 1;

            if (_secretCount >= 10)
            {
                btnTestData.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void MenuItem_Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;

            if (item != null)
            {
                Guid id = new Guid(item.CommandParameter.ToString());
                NavigationService.Navigate(new Uri(string.Format("/EditingPage.xaml?id={0}&date={1}", id, _currentViewDate.ToString()), UriKind.Relative));
            }
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;

            if (item != null)
            {
                Guid id = new Guid(item.CommandParameter.ToString());
                _sleepService.RemoveData(id, _currentViewDate);
                BindSleepDetailData();
                MessageBox.Show("刪除成功!", "刪除", MessageBoxButton.OK);
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            var btn = sender as ApplicationBarIconButton;

            if (btn != null)
            {
                switch (btn.Text)
                { 
                    case "About":
                        this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
                        break;
                    
                    case "Settings":
                        this.NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.RelativeOrAbsolute));
                        break;
                }
            }
        }

    }
}