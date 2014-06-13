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
using System.Windows.Navigation;

namespace iSleep
{
    public partial class EditingPage : PhoneApplicationPage
    {
        private SettingService _settingService = new SettingService();
        private SleepService _sleepService = new SleepService();
        private string _guId = "";
        private DateTime _currentViewDate = DateTime.Now;

        public EditingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                BindInitialValue();
            }
        }

        private void BindInitialValue()
        {
            if (this.NavigationContext.QueryString.ContainsKey("id"))
            {
                _guId = this.NavigationContext.QueryString["id"];
            }

            if (this.NavigationContext.QueryString.ContainsKey("date"))
            {
                _currentViewDate = Convert.ToDateTime(this.NavigationContext.QueryString["date"]);
            }

            var data = _sleepService.GetSleepDataByDate(_currentViewDate).FirstOrDefault(d => d.Id == new Guid(_guId));

            if (data != null)
            {
                datePickerSleep.Value = data.SleepTime;
                timePickerSleep.Value = data.SleepTime;
                datePickerWake.Value = data.WakeTime;
                timePickerWake.Value = data.WakeTime;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DateTime sleepTime = new DateTime(datePickerSleep.Value.Value.Year,
                                              datePickerSleep.Value.Value.Month,
                                              datePickerSleep.Value.Value.Day,
                                              timePickerSleep.Value.Value.Hour,
                                              timePickerSleep.Value.Value.Minute,
                                              timePickerSleep.Value.Value.Second);


            DateTime wakeTime = new DateTime(datePickerWake.Value.Value.Year,
                                             datePickerWake.Value.Value.Month,
                                             datePickerWake.Value.Value.Day,
                                             timePickerWake.Value.Value.Hour,
                                             timePickerWake.Value.Value.Minute,
                                             timePickerWake.Value.Value.Second);

            if (wakeTime > sleepTime)
            {
                var sleepData = _sleepService.GetSleepDataByDate(_currentViewDate);
                var data = sleepData.FirstOrDefault(d => d.Id == new Guid(_guId));

                data.SleepTime = sleepTime;
                data.WakeTime = wakeTime;
                _sleepService.UpdateData(sleepData, _currentViewDate);

                MessageBox.Show("更新成功!",
                                "編輯",
                                 MessageBoxButton.OK);

                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("起床時間必須晚於就寢時間，請重新輸入!",
                                "編輯",
                                 MessageBoxButton.OK);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


    }
}