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
using iSleep.Model;
using System.Collections.Generic;
using System.Linq;

namespace iSleep.Service
{
    public class ReportService
    {
        private SleepService _sleepService = new SleepService();
        private SettingService _settingService = new SettingService();

        public IList<TrendModel> GetSleepTrend(DateTime date)
        {
            var setting = _settingService.GetCurrentSetting();

            var data = _sleepService.GetSleepDataByDate(date)
                                    .OrderBy(d => d.SleepTime)
                                    .ToList();

            IList<TrendModel> result = new List<TrendModel>();

            if (data.Count > 0)
            {
                DateTime targetTime = setting.TargetSleepTime;

                foreach (var item in data)
                {
                    double totalHours = (item.SleepTime - targetTime).TotalHours;
                    int intHours = Convert.ToInt32(Math.Floor(totalHours));
                    double decimailHours = totalHours - intHours;

                    totalHours = (intHours % 24) + decimailHours;
                    double yValue = totalHours > 12 ? totalHours - 24 : totalHours;

                    TrendModel trend = new TrendModel
                                            {
                                                YValue = yValue,
                                                AxisXLabel = item.SleepTime.ToString("MM/dd"),
                                                ToolTipText = item.SleepTime.ToString("HH:mm")
                                            };

                    result.Add(trend);
                }
            }

            return result;
        }


        public IList<TrendModel> GetWakeTrend(DateTime date)
        {
            var setting = _settingService.GetCurrentSetting();

            var data = _sleepService.GetSleepDataByDate(date)
                                    .OrderBy(d => d.SleepTime)
                                    .ToList();

            IList<TrendModel> result = new List<TrendModel>();

            if (data.Count > 0)
            {
                DateTime targetTime = setting.TargetWakeTime;

                foreach (var item in data)
                {
                    double totalHours = (item.WakeTime - targetTime).TotalHours;
                    int intHours = Convert.ToInt32(Math.Floor(totalHours));
                    double decimailHours = totalHours - intHours;

                    totalHours = (intHours % 24) + decimailHours;
                    double yValue = totalHours > 12 ? totalHours - 24 : totalHours;

                    TrendModel trend = new TrendModel
                    {
                        YValue = yValue,
                        AxisXLabel = item.WakeTime.ToString("MM/dd"),
                        ToolTipText = item.WakeTime.ToString("HH:mm")
                    };

                    result.Add(trend);
                }
            }

            return result;
        }
    }
}
