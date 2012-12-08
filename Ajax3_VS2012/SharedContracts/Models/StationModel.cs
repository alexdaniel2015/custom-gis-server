using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SharedContracts
{   
    [DataContract]
    public class StationModel : IDataModel
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public int CompanyId { get; set; }
        
        [DataMember]
        public string StreetName { get; set; }
        
        [DataMember]
        public int HouseNo { get; set; }
        
        [DataMember]
        public string City { get; set; }
        
        [DataMember]
        public string Phone { get; set; }
        
        [DataMember]
        public string Coordinates { get; set; }

        [DataMember]
        public IList<PersonModel> Persons { get; set; }

        [DataMember]
        public IList<ServiceModel> Services { get; set; }


    }
}