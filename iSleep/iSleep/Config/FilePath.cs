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

namespace iSleep.Config
{
    public class FilePath
    {
        public const string settingPath = "Setting.xml";

        /// <summary>
        /// SleepData_yyyyMM.xml
        /// </summary>
        public const string sleepDataPathTemplate = "SleepData_{0}.xml";
    }
}
