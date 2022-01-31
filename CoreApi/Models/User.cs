using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApi.Models
{
    public class User
    {
        public int ID { get; set; }
        public string connectionID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public byte[] password { get; set; }


        public static User EmptyPlayer()
        {
            return new User
            {
                connectionID = "",
                userName = ""
            };
        }
    }
}
