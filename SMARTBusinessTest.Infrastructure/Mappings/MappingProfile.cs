using AutoMapper;
using SMARTBusinessTest.Domain.DTOs;
using SMARTBusinessTest.Domain.Entities;

namespace SMARTBusinessTest.Domain.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductionFacility, ProductionFacilityDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area));

            CreateMap<EquipmentUnit, EquipmentUnitDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Equipment.Code))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Equipment.Name))
                .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Equipment.Area))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));

            CreateMap<PlacementContract, PlacementContractDTO>()
                .ForMember(dest => dest.ContractId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EquipmentUnits, opt => opt.MapFrom(src => src.EquipmentUnits))
                .ForMember(dest => dest.ProductionFacility, opt => opt.MapFrom(src => src.ProductionFacility));
        }
    }
}
