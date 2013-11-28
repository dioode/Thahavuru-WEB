using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Thahavuru_WEB.Models
{
    [DataContract]
    public class PersonVM
    {
        public PersonVM()
        {
            FaceofP = new Face();
            SearchTrakKeeper = new List<List<List<int>>>();
            MatchedFaceIdSet = new Dictionary<int, List<int>>();

        }
        [DataMember]
        public Face FaceofP; //{ get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Id { get; set; }//Guid Id { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public Dictionary<int,List<int>>  MatchedFaceIdSet { get; set; }
       
        [DataMember]
        public List<List<List<int>>> SearchTrakKeeper { get; set; }
       
    }
}
