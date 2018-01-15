using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MovieSpider.Core.Utils
{
    public static class FileUtil
    {
        public static T GetJson<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                var jsonStr = File.ReadAllText(fileName);

                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>("");
            }
        }

        public static void Save(string fileName, object contents)
        {
            var filePath = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var jsonStr = JsonConvert.SerializeObject(contents);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllText(fileName, jsonStr);
        }
    }
}
