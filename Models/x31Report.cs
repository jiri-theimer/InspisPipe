using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class x31Report
    {
        public int x31ID { get; set; }
        public int x29ID { get; set; }
        public string x31Name { get; set; }
        public string x31Description { get; set; }
        public string x31PID { get; set; }

        public int x31ReportFormat { get; set; }

        public bool x31Is4SingleRecord { get; set; }

        public string x31MSReporting_ReportServerUrl { get; set; }
        public string x31MSReporting_ReportPath { get; set; }

        public string x29Name { get; set; } //combo
        public string x31DocSqlSource { get; set; }
        public string x31Translate { get; set; }
        public string x31DocSqlSourceTabs { get; set; }
    }
}