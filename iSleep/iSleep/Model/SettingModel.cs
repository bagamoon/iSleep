using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace iSleep.Model
{
    public enum SleepStatus
    {
        Waking = 1,
        Sleeping = 2
    }

    public class SettingModel
    {
        public SleepStatus NowStatus { get; set; }

        public DateTime AppInitialDate { get; set; }

        public DateTime TargetWakeTime { get; set; }

        public DateTime TargetSleepTime { get; set; }

        public DateTime? LastWakeTime { get; set; }

        public DateTime? LastSleepTime { get; set; }
    }
}
