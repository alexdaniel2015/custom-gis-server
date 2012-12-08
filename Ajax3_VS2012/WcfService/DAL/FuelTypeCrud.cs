using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Service.DAL;
using SharedContracts;

namespace WcfService.DAL
{
    class FuelTypeCrud:ICrud
    {
        public FuelTypeCrud()
        {
            Mapper.CreateMap<FuelType, FuelTypeModel>()
                .ForMember(ftm => ftm.FuelName, m => m.MapFrom(s => s.ID))
                .ForMember(ftm => ftm.ID, m => m.MapFrom(s => s.ID));
        }

        public IEnumerable<IDataModel> Read()
        {
            var fuelTypes = new List<FuelTypeModel>();

            using (var db = new GisDataBase())
            {
                fuelTypes.AddRange(db.FuelTypes.Select(company => Mapper.Map<FuelType, FuelTypeModel>(company)));
            }

            return fuelTypes;
        }

        public IDataModel Read(int id)
        {
            using (var db = new GisDataBase())
            {
                return Mapper.Map<FuelType, FuelTypeModel>(db.FuelTypes.FirstOrDefault(x => x.ID == id));
            }
        }
    }
}
