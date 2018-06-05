using MovieSpider.Data;
using MovieSpider.Data.IContentEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services.IContentService
{
    public class AppConfigService
    {
        public List<AppConfig> GetAppConfigs()
        {
            using (var db = new IContentDbContext())
            {
                var configs = db.AppConfig.AsNoTracking().ToList();
                return configs;
            }
        }
    }
}
