using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharedContracts;

namespace Service.DAL
{
    public interface ICrud
    {
        IEnumerable<IDataModel> Read();
        IDataModel Read(int id);
    }
}