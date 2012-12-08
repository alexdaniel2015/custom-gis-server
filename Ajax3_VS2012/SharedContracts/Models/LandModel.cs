using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContracts.Models
{
    public class LandModel:IDataModel
    {
        public int ID { get; set; }
        public int StationID { get; set; }
        public int OwnerShipTypeID { get; set; }
        public int OwnerID { get; set; }
        public string Description { get; set; }

    }
}
