using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SharedContracts
{
    [DataContract]
    public class PersonModel
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string SurName { get; set; }
        
        [DataMember]
        public int SpecialityId { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]

        public IList<StationModel> Stations { get; set; }

    }
}