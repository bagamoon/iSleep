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
using iSleep.Dao;
using iSleep.Model;
using iSleep.Config;
using System.Collections.Generic;
using System.Linq;

namespace iSleep.Service
{
    public class SleepService
    {
        private ISXmlDao _xmlDao = new ISXmlDao();
        private SettingService _settingService = new SettingService();

        public IList<SleepModel> GetSleepDataCurrent()
        {
            var data = _xmlDao.Read<List<SleepModel>>(GetFilePathByDate(DateTime.Now));

            return data ?? new List<SleepModel>();
        }

        public IList<SleepModel> GetSleepDataByDate(DateTime date)
        {
            var data = _xmlDao.Read<List<SleepModel>>(GetFilePathByDate(date));

            return data ?? new List<SleepModel>();
        }

        public IList<SleepModel> GetSleepDataAll()
        {
            var allData = new List<SleepModel>();

            DateTime initialDate = _settingService.GetCurrentSetting().AppInitialDate;

            while (initialDate <= DateTime.Now)
            {
                var data = _xmlDao.Read<List<SleepModel>>(GetFilePathByDate(initialDate));

                if (data != null)
                {
                    allData.AddRange(data);
                }

                initialDate = initialDate.AddMonths(1);
            }

            return allData;
        }

        public void Sleep()
        {            
            DateTime now = DateTime.Now;

            _settingService.SetToSleeping(now);
        }

        public SleepModel Wake()
        {
            var setting = _settingService.GetCurrentSetting();

            DateTime lastSleepTime = (DateTime)setting.LastSleepTime;

            var sleepData = GetSleepDataByDate(lastSleepTime);

            if (sleepData == null)
            {
                sleepData = new List<SleepModel>();
            }

            DateTime now = DateTime.Now;

            SleepModel sleep = new SleepModel
                                    {
                                        Id = Guid.NewGuid(),
                                        SleepTime = lastSleepTime,
                                        WakeTime = now
                                    };

            sleepData.Add(sleep);

            _xmlDao.Write<List<SleepModel>>(sleepData.ToList(), GetFilePathByDate(lastSleepTime));

            _settingService.SetToWaking(now);

            return sleep;
        }

        public void RemoveData(Guid id, DateTime date)
        {
            var sleepData = GetSleepDataByDate(date);

            if (sleepData != null)
            {
                var data = sleepData.FirstOrDefault(d => d.Id == id);

                if (data != null)
                {
                    sleepData.Remove(data);

                    _xmlDao.Write<List<SleepModel>>(sleepData.ToList(), GetFilePathByDate(date));
                }
            }
        }

        public void RemoveAllData()
        {
            _xmlDao.RemoveAll();
        }


        public void UpdateData(IList<SleepModel> sleepData, DateTime month)
        {
            _xmlDao.Write<List<SleepModel>>(sleepData.ToList(), GetFilePathByDate(month));
        }


        private string GetFilePathByDate(DateTime date)
        {
            return string.Format(FilePath.sleepDataPathTemplate, date.ToString("yyyyMM"));
        }


        public void GenerateTestData()
        {
            int year = DateTime.Now.Year;
            for (var i = 1; i <= DateTime.Now.Month; i++)
            {
                DateTime date = new DateTime(year, i, 1);

                var sleepData = new List<SleepModel>();

                DateTime nextMonth = date.AddMonths(1);

                Random rand = new Random(DateTime.Now.Millisecond);

                while (date < nextMonth)
                {
                    sleepData.Add(new SleepModel
                    {
                        Id = Guid.NewGuid(),
                        SleepTime = date.AddHours(rand.NextDouble() - 1),
                        WakeTime = date.AddHours(rand.NextDouble() * 10)
                    });

                    date = date.AddDays(1);
                }

                _xmlDao.Write<List<SleepModel>>(sleepData.ToList(), GetFilePathByDate(date.AddMonths(-1)));
            }
        }        
    }
}
