using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi.Models
{
    public class Requests
    {
        public int ID { get; set; }
        public string ReqTypes { get; set; }
        public string Url { get; set; } 
    }
}
public enum ReqTypes
{
    login,
    registry
}

