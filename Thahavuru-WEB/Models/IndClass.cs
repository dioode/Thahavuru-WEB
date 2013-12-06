using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class IndClass
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ClassNumber { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}