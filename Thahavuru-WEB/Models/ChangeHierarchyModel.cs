using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class ChangeHierarchyModel
    {
        public ChangeHierarchyModel() 
        {
            AttributesInHierarchy = new List<AttributeModel>();
            AttributesNotInHierarchy = new List<AttributeModel>();
        }

        [DataMember]
        public List<AttributeModel> AttributesNotInHierarchy { get; set; }

        [DataMember]
        public List<AttributeModel> AttributesInHierarchy { get; set; }

        
    }
}