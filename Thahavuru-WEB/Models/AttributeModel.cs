using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class AttributeModel
    {
        
        public AttributeModel()
        {
            IndClasses = new List<IndClass>();
        }
        [DataMember]
        public string ClassificationTechnique { get; set; } //{ get; set; }

        [DataMember]
        public int AttributeId { get; set; } //{ get; set; }

        [DataMember]
        public string Name { get; set; }//{ get; set; }

        [DataMember]
        public bool IsBinary { get { return NumberOfClasses == 2 ? true: false;} }

        [DataMember]
        public bool IsBiometric { get; set; }

        [DataMember]
        public int NumberOfClasses { get; set; }

        [DataMember]
        public List<IndClass> IndClasses { get; set; }
    }
}