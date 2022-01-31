using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi.Helpers
{
    public class ModelSerializer
    {
        public static string Model2String<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T String2Model<T>(string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
