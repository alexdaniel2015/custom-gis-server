using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SharedContracts
{
    [DataContract]
    public class PriceModel:IDataModel
    {
        [DataMember]
        public int StationId { get; set; }
        
        [DataMember]
        public int FuelId { get; set; }
        
        [DataMember]
        public float Price { get; set; }
    }
}