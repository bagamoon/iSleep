﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace iSleep.ViewModel
{
    public class SettingsViewModel
    {
        public SettingsCategory Category { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }

    public enum SettingsCategory
    {
        TargetTime = 1,
        ClearAllData = 2
    }
}
