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
using iSleep.Dao;
using System.Linq;
using iSleep.Config;

namespace iSleep.Service
{
    public class SettingService
    {
        private ISXmlDao _xmlDao = new ISXmlDao();

        public SettingModel GetCurrentSetting()
        {
            var setting = _xmlDao.Read<SettingModel>(FilePath.settingPath);

            if (setting == null)
            {
                setting = new SettingModel
                {
                    NowStatus = SleepStatus.Waking,
                    AppInitialDate = DateTime.Now,
                    TargetSleepTime = new DateTime(2012, 1, 1, 0, 0, 0),
                    TargetWakeTime = new DateTime(2012, 1, 1, 8, 0, 0),
                    LastSleepTime = null,
                    LastWakeTime = null
                };

                _xmlDao.Write(setting, (FilePath.settingPath));
            }

            return setting;
        }

        public void SetToSleeping(DateTime dateTime)
        {
            var setting = GetCurrentSetting();
            setting.LastSleepTime = dateTime;
            setting.LastWakeTime = null;
            setting.NowStatus = SleepStatus.Sleeping;
            UpdateSetting(setting);
        }

        public void SetToWaking(DateTime dateTime)
        {
            var setting = GetCurrentSetting();
            setting.LastWakeTime = dateTime;
            setting.NowStatus = SleepStatus.Waking;
            UpdateSetting(setting);
        }

        public void UpdateSetting(SettingModel setting)
        {
            _xmlDao.Write(setting, (FilePath.settingPath));
        }
    }
}
