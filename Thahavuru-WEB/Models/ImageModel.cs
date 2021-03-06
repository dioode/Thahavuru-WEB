﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class ImageModel
    {
        [DataMember]
        public String MatchedImageURI { get; set; }

        [DataMember]
        public String UserName { get; set; }

        [DataMember]
        public String Address { get; set; }
        
    }
}