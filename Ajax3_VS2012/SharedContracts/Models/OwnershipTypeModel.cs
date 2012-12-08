using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedContracts.Models
{
    public class OwnershipTypeModel:IDataModel
    {
        public int ID { get; set; }

        public string OwnershipType { get; set; } 
    }
}
