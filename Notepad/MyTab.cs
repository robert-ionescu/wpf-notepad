﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad
{
    public class MyTab
    {
        public string Header { get; set; }

        public ObservableCollection<MyTabData> Data { get; } = new ObservableCollection<MyTabData>();
    }
    public class MyTabData
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
    }
}
