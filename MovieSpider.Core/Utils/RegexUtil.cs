using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class RegexUtil
    {
        /// <summary>
        /// 正则 替换空格 tab字符 换行符 新行 , &nbsp;
        /// </summary>
        public static string ReplaceSpaceTabNewline(string val)
        {
            return Regex.Replace(val, @"\s", "").Replace("&nbsp;", "").Replace(" ", "");
        }

        /// <summary>
        /// 正则 替换空格 tab字符 换行符 新行 , &nbsp;
        /// </summary>
        public static string ReplaceBr(string val)
        {
            return Regex.Replace(val, @"<br>", "").Replace("<br/>", "").Replace("<br />", "");
        }
    }
}
