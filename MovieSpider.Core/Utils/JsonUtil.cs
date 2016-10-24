using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public static class JsonUtil
    {
        public static T GetJsonData<T>(string fileName)
        {
            var json = File.ReadAllText(fileName);

            JsonConvert.DeserializeObject<T>(json);

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
