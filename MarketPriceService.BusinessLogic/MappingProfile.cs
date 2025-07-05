using AutoMapper;
using MarketPriceService.BusinessLogic.Models;
using MarketPriceService.DataAccess.Entities;

namespace MarketPriceService.DataAccess
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InstrumentEntity, Instrument>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InstrumentId))
                .ForMember(dest => dest.Mappings, opt => opt.MapFrom(src => src.Mappings))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile));

            CreateMap<ICollection<InstrumentMappingEntity>, Dictionary<string, InstrumentMapping>>()
                .ConvertUsing((src, _, context) =>
                    src
                    .Where(x => !string.IsNullOrWhiteSpace(x.Provider))
                    .GroupBy(x => x.Provider)
                    .ToDictionary(
                        g => g.Key,
                        g => context.Mapper.Map<InstrumentMapping>(g.First())
                    )
                );

            CreateMap<InstrumentMappingEntity, InstrumentMapping>();
            CreateMap<InstrumentProfileEntity, InstrumentProfile>().ReverseMap();
            CreateMap<GicsInfoEntity, GicsInfo>().ReverseMap();
            CreateMap<TradingHoursEntity, TradingHours>().ReverseMap();

            CreateMap<Instrument, InstrumentEntity>()
                .ForMember(dest => dest.InstrumentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.Mappings, opt => opt.MapFrom((src, _, _, context) =>
                    src.Mappings.Select(kvp =>
                    {
                        var entity = context.Mapper.Map<InstrumentMappingEntity>(kvp.Value);
                        entity.Provider = kvp.Key;
                        entity.InstrumentEntityId = Guid.Parse(src.Id);
                        return entity;
                    }).ToList()
                ));

            CreateMap<Dictionary<string, InstrumentMapping>, ICollection<InstrumentMappingEntity>>()
                .ConvertUsing((src, _, context) =>
                    src.Select(kvp =>
                    {
                        var entity = context.Mapper.Map<InstrumentMappingEntity>(kvp.Value);
                        entity.Provider = kvp.Key ?? throw new InvalidOperationException("Provider is null");
                        return entity;
                    }).ToList()
                );

            CreateMap<InstrumentMapping, InstrumentMappingEntity>();
        }
    }
}