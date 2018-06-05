using MovieSpider.Core.Utils;
using MovieSpider.Data.IContentEntities;
using MovieSpider.Services.IContentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Services.IContentCache
{
    public class AppConfigCache
    {
        private const string Cache_Key = "APP_Config_Cache_Key";

        public static void Init()
        {
            var data = new AppConfigService().GetAppConfigs();
            CacheUtil.Add(Cache_Key, data);
        }

        public static List<AppConfig> Data
        {
            get
            {
                if (CacheUtil.Get<List<AppConfig>>(Cache_Key) == null)
                {
                    AppConfigCache.Init();
                }
                return CacheUtil.Get<List<AppConfig>>(Cache_Key);
            }
        }

        public static List<AppConfig> GetAllAppConfigs()
        {
            return Data;
        }

        public static AppConfig GetAppConfig(string name)
        {
            var appConfig = Data.Where(a => a.Name == name).FirstOrDefault();

            return appConfig;
        }

        public static void Update(object obj)
        {
            var newData = obj as AppConfig;
            foreach (var data in Data)
            {
                if (data.AppConfigId == newData.AppConfigId)
                {
                    data.Name = newData.Name;
                    data.Value = newData.Value;
                    break;
                }
            }
        }

        #region Static

        //public static int Homepage_Posts_Board_ID
        //{
        //    get { return GetAppConfig("Homepage_Posts_Board_ID") != null ? int.Parse(GetAppConfig("Homepage_Posts_Board_ID").Value) : 0; }
        //}
        public static int Movie_Board_ID
        {
            get { return GetAppConfig("Movie_Board_ID") != null ? int.Parse(GetAppConfig("Movie_Board_ID").Value) : 0; }
        }

        public static int AboutUs_Board_ID
        {
            get { return GetAppConfig("AboutUs_Board_ID") != null ? int.Parse(GetAppConfig("AboutUs_Board_ID").Value) : 0; }
        }

        public static int ContactUs_Board_ID
        {
            get { return GetAppConfig("ContactUs_Board_ID") != null ? int.Parse(GetAppConfig("ContactUs_Board_ID").Value) : 0; }
        }

        public static int Anonymous_User_ID
        {
            get { return GetAppConfig("Anonymous_User_ID") != null ? int.Parse(GetAppConfig("Anonymous_User_ID").Value) : 0; }
        }

        public static int Admin_User_ID
        {
            get { return GetAppConfig("Admin_User_ID") != null ? int.Parse(GetAppConfig("Admin_User_ID").Value) : 0; }
        }

        public static string User_Publish_Board_IDs
        {
            get { return GetAppConfig("User_Publish_Board_IDs") != null ? GetAppConfig("User_Publish_Board_IDs").Value : ""; }
        }

        public static int Headshot_Big_Width
        {
            get { return GetAppConfig("Headshot_Big_Width") != null ? int.Parse(GetAppConfig("Headshot_Big_Width").Value) : 120; }
        }

        public static int Headshot_Big_Height
        {
            get { return GetAppConfig("Headshot_Big_Height") != null ? int.Parse(GetAppConfig("Headshot_Big_Height").Value) : 120; }
        }

        public static int Headshot_Small_Height
        {
            get { return GetAppConfig("Headshot_Small_Height") != null ? int.Parse(GetAppConfig("Headshot_Small_Height").Value) : 30; }
        }

        public static int Headshot_Small_Width
        {
            get { return GetAppConfig("Headshot_Small_Width") != null ? int.Parse(GetAppConfig("Headshot_Small_Width").Value) : 30; }
        }

        public static string File_Upload_Folder
        {
            get { return GetAppConfig("File_Upload_Folder") != null ? GetAppConfig("File_Upload_Folder").Value : "/content/upload"; }
        }

        public static int Admin_Role_ID
        {
            get { return GetAppConfig("Admin_Role_ID") != null ? int.Parse(GetAppConfig("Admin_Role_ID").Value) : 0; }
        }

        #endregion
    }
}
