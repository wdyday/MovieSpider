using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Consts
{
    public class AppSetting
    {
        public static readonly string Dy2018Domain = ConfigurationManager.AppSettings["Dy2018Domain"];
        public static readonly string Dy2018Encode = ConfigurationManager.AppSettings["Dy2018Encode"];
    }
}
