﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class a19DomainToInstitutionDepartment
    {
        public int a19ID { get; set; }
        public int a37ID { get; set; }
        public int a18ID { get; set; }

        public bool a19IsShallEnd { get; set; }
        public int a19StudyCapacity { get; set; }
        public int a19StudyDuration { get; set; }
        public int a19StudyLanguage { get; set; }
        public int a19StudyPlatform { get; set; }

        public string a18Code;
        public string a18Name;
        public string a18CodePlusName { get; set; }
        public string a37Name;
        public string a37IZO;
        public int a03ID;

        public string a37IzoPlusName { get; set; }
    }
}