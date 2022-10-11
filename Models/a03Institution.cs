using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class a03Institution
    {
        public int a03ID { get; set; }
        public int a09ID { get; set; }
        public int a05ID { get; set; }
        public int a06ID { get; set; }
        public int a03ID_Founder { get; set; }
        public int a03ID_Supervisory { get; set; }
        public int a21ID { get; set; }
        public int a28ID { get; set; }
        public int a70ID { get; set; }
        public bool a03IsTestRecord { get; set; }

        public string a03REDIZO { get; set; }
        public string a03ICO { get; set; }
        public string a03Name { get; set; }
        public string a03ShortName { get; set; }
        public string a03City { get; set; }
        public string a03Street { get; set; }
        public string a03PostCode { get; set; }
        public string a03Phone { get; set; }
        public string a03Mobile { get; set; }
        public string a03Fax { get; set; }
        public string a03Email { get; set; }
        public string a03Web { get; set; }
        public string a03FounderCode { get; set; }
        public string a03DirectorFullName { get; set; }
        public string a03Slug { get; set; }
        public double a03Latitude { get; set; }
        public double a03Longitude { get; set; }

        
        
        public string a05Name { get; set; }//combo
        public string a05UIVCode;
        public string a06Name;
        public string a21Name { get; set; }//combo
        public string a09Name { get; set; } //combo
        public string a09UIVCode;

        public bool isclosed { get; set; }
    }
}