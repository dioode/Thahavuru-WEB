using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class AttributesListModel
    {
        [DataMember]
        public List<AttributeModel> AttributeList { get; set; }
    }
}