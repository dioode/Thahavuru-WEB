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
        public List<KeyValuePair<string, string>> attributeValues { get; set; }

        [DataMember]
        public ImageModel AdvancedSearchResult { get; set; }

        public string AdvancedSearchUserName { get; set; }

        [DataMember]
        public bool Next { get; set; }

        [DataMember]
        public bool Back { get; set; }
    }
}