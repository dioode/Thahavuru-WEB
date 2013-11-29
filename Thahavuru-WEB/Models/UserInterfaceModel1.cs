using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class UserInterfaceModel1
    {
        public UserInterfaceModel1()
        {
            SearchingPerson = new PersonVM1();
        }

        [DataMember]
        public PersonVM1 SearchingPerson;

        [DataMember]
        public int PageNumber{ get; set; }

        [DataMember]
        public int MaxLeaves
        {
            get;
            set;
        }

        [DataMember]
        public bool Next{ get; set; }

        [DataMember]
        public bool Back{ get; set; }
    }
}
