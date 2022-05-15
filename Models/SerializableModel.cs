using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerWpf.Models
{
    public class SerializableModel<T> where T : class
    {
        public static T? FromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string ToJson(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
