using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SharedContracts
{
    [DataContract]
    public class FuelTypeModel:IDataModel
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string FuelName { get; set; }
    }
}