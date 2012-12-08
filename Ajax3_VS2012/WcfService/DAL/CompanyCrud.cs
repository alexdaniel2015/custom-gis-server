using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharedContracts;
using WcfService;

namespace Service.DAL
{
    public class CompanyCrud:ICrud
    {
        public  CompanyCrud()
        {
            Mapper.CreateMap<Company, CompanyModel>()
                .ForMember(cm => cm.Id, m => m.MapFrom(s => s.ID))
                .ForMember(cm => cm.CompanyName, m => m.MapFrom(s => s.Name));
        }

        public IEnumerable<IDataModel> Read()
        {
            var companies = new List<CompanyModel>();
         
            using (var db = new GisDataBase())
            {
                companies.AddRange(db.Companies.Select(company => Mapper.Map<Company, CompanyModel>(company)));
            }

            return companies;
        }

        public IDataModel Read(int id)
        {
            using(var db = new GisDataBase())
            {
                return Mapper.Map<Company,CompanyModel>(db.Companies.FirstOrDefault(x => x.ID == id));
            }
        }
    }
}