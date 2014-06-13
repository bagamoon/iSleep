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
using iSleep.ViewModel;
using iSleep.Service;
using System.Windows.Navigation;

namespace iSleep
{
    public partial class Settings : PhoneApplicationPage
    {
        private SettingService _settingService = new SettingService();
        private SleepService _sleepService = new SleepService();

        public Settings()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BindSettingsListBox();
        }

        public void BindSettingsListBox()
        {
            var setting = _settingService.GetCurrentSetting();
            
            IList<SettingsViewModel> list = new List<SettingsViewModel>();

            list.Add(new SettingsViewModel
                    {
                        Category = SettingsCategory.TargetTime,
                        Title = "目標就寢/起床時間",
                        Description = string.Format("就寢: {0}  起床: {1}",
                                                    setting.TargetSleepTime.ToString("HH:mm"),
                                                    setting.TargetWakeTime.ToString("HH:mm"))
                    });            

            list.Add(new SettingsViewModel
                    {
                        Category = SettingsCategory.ClearAllData,
                        Title = "清除所有資料",
                        Description = "清除所有睡眠記錄及恢復初始設定"
                    });

            
            listBoxSettings.ItemsSource = list;

        }

        private void listBoxSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                SettingsViewModel model = e.AddedItems[0] as SettingsViewModel;

                if (model != null)
                {
                    switch (model.Category)
                    {
                        case SettingsCategory.TargetTime:
                            this.NavigationService.Navigate(new Uri("/SettingTargetTime.xaml", UriKind.RelativeOrAbsolute));
                            break;

                        case SettingsCategory.ClearAllData:
                            if (MessageBox.Show("即將刪除所有睡眠記錄，此動作不可回復，確定繼續？", "刪除所有資料", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                            {
                                _sleepService.RemoveAllData();
                            }                            
                            break;
                    }

                    listBoxSettings.SelectedIndex = -1;
                }
            }
            
        }
    }
}