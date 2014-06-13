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

namespace iSleep
{
    public partial class SettingTargetTime : PhoneApplicationPage
    {
        private SettingService _settingService = new SettingService();

        public SettingTargetTime()
        {
            InitializeComponent();
            InitialTimePicker();
        }

        private void InitialTimePicker()
        {
            var setting = _settingService.GetCurrentSetting();
            timePickerSleep.Value = setting.TargetSleepTime;
            timePickerWake.Value = setting.TargetWakeTime;
        }

        private void timePickerSleep_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (timePickerSleep != null)
            {
                var setting = _settingService.GetCurrentSetting();
                setting.TargetSleepTime = new DateTime(2012, 1, 1, e.NewDateTime.Value.Hour, e.NewDateTime.Value.Minute, 0);
                _settingService.UpdateSetting(setting);
            }
        }

        private void timePickerWake_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (timePickerWake != null)
            {
                var setting = _settingService.GetCurrentSetting();
                setting.TargetWakeTime = new DateTime(2012, 1, 1, e.NewDateTime.Value.Hour, e.NewDateTime.Value.Minute, 0);
                _settingService.UpdateSetting(setting);
            }
        }
    }
}