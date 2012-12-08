using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SharedContracts
{
    [DataContract]
    public class CompanyModel:IDataModel
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public string CompanyName { get; set; }
    }
}