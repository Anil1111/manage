using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manage.Core.Json
{
    public class JsonUtil
    {
        public static string SerializerObject(object obj, bool formatjson)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            IsoDateTimeConverter idtc = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(idtc);
            JsonWriter jw = new JsonTextWriter(sw);

            if (formatjson)
                jw.Formatting = Formatting.Indented;

            serializer.Serialize(jw, obj);

            return sb.ToString();
        }

        public static string SerializerObject(object obj)
        {
            return SerializerObject(obj, false);
        }

        public static T DeserializeJsonToObject<T>(string json)
        {
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static List<T> DeserializeJsonToList<T>(string json)
        {
            List<T> obj = JsonConvert.DeserializeObject<List<T>>(json);
            return obj;
        }
    }
}
