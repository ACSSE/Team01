using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace IcebreakServices
{
    [DataContract]
    public class Image
    {
        [DataMember]
        public string base64
        {
            get
            {
                return base64;
            }

            set
            {
                base64 = value;
            }
        }
    }
}