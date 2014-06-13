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
using System.ComponentModel;

namespace iSleep.Model
{
    public enum SleepQuantify
    {
        [Description("危在旦夕")]
        Lacked = 1,
        
        [Description("勉強續命")]
        Tolerable =2,

        [Description("普普通通")]
        Medium = 3,

        [Description("休生養息")]
        Enough = 4,

        [Description("心滿意足")]
        Satisfied = 5
    }

    public class SleepModel
    {
        public Guid Id { get; set; }

        public DateTime SleepTime { get; set; }

        public DateTime WakeTime { get; set; }

        public TimeSpan SleepInterval
        {
            get
            {
                return WakeTime - SleepTime;
            }
        }

        public SleepQuantify SleepQuantify
        {
            get
            {
                if (SleepInterval.TotalHours > 0 && SleepInterval.TotalHours < 2)
                {
                    return SleepQuantify.Lacked;
                }
                else if (SleepInterval.TotalHours > 2 && SleepInterval.TotalHours < 4)
                {
                    return SleepQuantify.Tolerable;
                }
                else if (SleepInterval.TotalHours > 4 && SleepInterval.TotalHours < 6)
                {
                    return SleepQuantify.Medium;
                }
                else if (SleepInterval.TotalHours > 6 && SleepInterval.TotalHours < 8)
                {
                    return SleepQuantify.Enough;
                }
                else
                {
                    return SleepQuantify.Satisfied;
                }
            }
        }

        public string SleepImageUrl
        {
            get 
            {
                return "/iSleep;component/Images/Quantify_" + this.SleepQuantify.ToString() + ".png";
            }
        }

        public string SleepTimeString
        {
            get
            {
                return SleepTime.ToString("MM/dd HH:mm");
            }
        }

        public string WakeTimeString
        {
            get
            {
                return WakeTime.ToString("MM/dd HH:mm");
            }
        }

        public string SleepIntervalHour
        {
            get
            {
                return Math.Round(SleepInterval.TotalHours, 1) + " hr";
            }
        }
    }
}
