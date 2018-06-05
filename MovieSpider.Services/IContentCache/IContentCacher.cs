using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services.IContentCache
{
    public static class IContentCacher
    {
        public static void Init()
        {
            try
            {
                AppConfigCache.Init();
            }
            catch(Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(ex);
            }
        }
    }
}
