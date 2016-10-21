using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class HtmlUtil
    {
        private const string HTML_JAVASCRIPT_PATTERN = @"<script(.|\n)*?>(.|\n)*?<\/script>";
        private const string HTML_TAG_PATTERN = @"<(.|\n)*?>";

        public static string RemoveHTMLTag(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = Regex.Replace(content, HTML_JAVASCRIPT_PATTERN, string.Empty, RegexOptions.IgnoreCase);
                content = Regex.Replace(content, HTML_TAG_PATTERN, string.Empty, RegexOptions.IgnoreCase);

                return content;
            }
            else
            {
                return "";
            }
        }
        public static string GetFirstMatchValue(MatchType matchType, string content)
        {
            string result = string.Empty;

            string pattern = string.Empty;
            switch (matchType)
            {
                case MatchType.Image: pattern = "<img[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>"; break;
                case MatchType.Hyperlink: pattern = "<a[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>"; break;
                default: pattern = "<img[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>"; break;
            }

            Regex regex = new Regex(pattern);

            Match match = regex.Match(content);

            if (match.Success)
            {
                result = match.Groups[1].Value;
            }

            return result;
        }

        public static Match GetHtmlMatch(MatchType matchType, string content)
        {
            string pattern = string.Empty;
            switch (matchType)
            {
                case MatchType.Image: pattern = "<img[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>"; break;
                case MatchType.Hyperlink: pattern = "<a[^>]+href\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>"; break;
                default: pattern = "<img[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*>"; break;
            }

            Regex regex = new Regex(pattern);

            Match match = regex.Match(content);


            return match;
        }
    }

    public enum MatchType
    {
        Image,
        Hyperlink
    }
}
