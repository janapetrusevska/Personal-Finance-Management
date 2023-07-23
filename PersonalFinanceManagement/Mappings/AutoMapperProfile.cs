using AutoMapper;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Models;
using System;

namespace PersonalFinanceManagement.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TransactionEntity, Transaction>()
                .ForMember(t => t.Id, e => e.MapFrom(x => x.id))
                .ForMember(t => t.BeneficiaryName, e => e.MapFrom(x => x.name))
                .ForMember(t => t.Amount, e => e.MapFrom(x => x.amount))
                .ForMember(t => t.Date, e => e.MapFrom(x => x.date))
                .ForMember(t => t.Description, e => e.MapFrom(x => x.description))
                .ForMember(t => t.Direction, e => e.MapFrom(x => x.direction))
                .ForMember(t => t.Kind, e => e.MapFrom(x => x.kind))
                .ForMember(t => t.Currency, e => e.MapFrom(x => x.currency))
                .ForMember(t => t.MccCode, e => e.MapFrom(x => x.mcc))
                .ForMember(t => t.CatCode, e => e.MapFrom(x => x.catCode));

            CreateMap<Transaction, TransactionEntity>()
                .ForMember(t => t.id, e => e.MapFrom(x => x.Id))
                .ForMember(t => t.name, e => e.MapFrom(x => x.BeneficiaryName))
                .ForMember(t => t.amount, e => e.MapFrom(x => x.Amount))
                .ForMember(t => t.date, e => e.MapFrom(x => x.Date))
                .ForMember(t => t.description, e => e.MapFrom(x => x.Description))
                .ForMember(t => t.direction, e => e.MapFrom(x => x.Direction))
                .ForMember(t => t.kind, e => e.MapFrom(x => x.Kind))
                .ForMember(t => t.currency, e => e.MapFrom(x => x.Currency))
                .ForMember(t => t.mcc, e => e.MapFrom(x => x.MccCode))
                .ForMember(t => t.catCode, e => e.MapFrom(x => x.CatCode));
            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>();

            CreateMap<CategoryEntity, Category>()
                .ForMember(t => t.Code, e => e.MapFrom(x => x.code))
                .ForMember(t => t.Name, e => e.MapFrom(x => x.name))
                .ForMember(t => t.ParentCode, e => e.MapFrom(x => x.parentCode));

            CreateMap<Category, CategoryEntity>()
                .ForMember(t => t.code, e => e.MapFrom(x => x.Code))
                .ForMember(t => t.name, e => e.MapFrom(x => x.Name))
                .ForMember(t => t.parentCode, e => e.MapFrom(x => x.ParentCode));
        }

        protected internal AutoMapperProfile(string profileName, Action<IProfileExpression> configurationAction) : base(profileName, configurationAction)
        {
        }
    }
}
