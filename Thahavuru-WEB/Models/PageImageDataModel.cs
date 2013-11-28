using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class PageImageDataModel
    {
        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public List<ImageModel> ImageList { get; set; }

        [DataMember]
        public bool Next { get; set; }

        [DataMember]
        public bool Back { get; set; }
    }
}