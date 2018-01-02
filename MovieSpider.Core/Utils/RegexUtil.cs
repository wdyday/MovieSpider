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
            return Regex.Replace(val, @"\s", "").Replace("&nbsp;", "");
            //return Regex.Replace(val, @"\r|\n", ""); 
        }

        public static string ReplaceNewline(string val)
        {
            return Regex.Replace(val, @"\r|\n", "");
        }

        /// <summary>
        /// 正则 替换空格 tab字符 换行符 新行 , &nbsp;
        /// </summary>
        public static string ReplaceBr(string val)
        {
            return Regex.Replace(val, @"<br>", "").Replace("<br/>", "").Replace("<br />", "");
        }

        /// <summary>
        /// 判断字符串str是否以val开头
        /// str的字符之间可包含空格, 如:  [译　　名　不良少妇/不良女从夫传]  以 [译名] 开头
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool StartsWith(string str, string val)
        {
            var pattern = string.Empty;
            for(var i = 0; i < val.Length; i++)
            {
                pattern += val.Substring(i, 1) + "\\s*";
            }

            return Regex.IsMatch(str, pattern);
        }

        public static string GetValue(string str, string val)
        {
            var pattern = string.Empty;
            for (var i = 0; i < val.Length; i++)
            {
                pattern += val.Substring(i, 1) + "\\s*";
            }

            return Regex.Match(str, pattern).Value;
        }
    }
}
