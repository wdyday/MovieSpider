using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class ExceptionUtil
    {
        public static string GetExceptionMessage(Exception ex)
        {
            string message =
                   "Exception type " + ex.GetType() + Environment.NewLine +
                   "Exception message: " + ex.Message + Environment.NewLine +
                   "Stack trace: " + ex.StackTrace + Environment.NewLine;
            if (ex.InnerException != null)
            {
                message += "---BEGIN InnerException--- " + Environment.NewLine +
                           "Exception type " + ex.InnerException.GetType() + Environment.NewLine +
                           "Exception message: " + ex.InnerException.Message + Environment.NewLine +
                           "Stack trace: " + ex.InnerException.StackTrace + Environment.NewLine +
                           "---END Inner Exception";
            }

            return message;
        }
    }
}
