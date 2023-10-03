using AutoMapper;
using ecommerceApi.DTOs;
using ecommerceApi.Entities;

namespace ecommerceApi.Extensions
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateBrandDto, Brand>();
            CreateMap<CreateBrokerDto, Broker>();
            CreateMap<CreateSettingsDto, Setting>();
            CreateMap<CeoOptimizationDto, CeoOptimization>();
            CreateMap<SocialNetworkDto,SocialNetwork > ();
            CreateMap<QuickAccessDto, QuickAccess > ();
            CreateMap<LogoDto, Logo>();
            CreateMap<UpdateLogoDto, Logo>();
            CreateMap<CreateSlideDto, Slide>();
            CreateMap<UpdateSlideDto, Slide>();
            CreateMap<PartnerDto, Partner>();
            CreateMap<UpdatePartnerDto, Partner>();

            

        }
    }
}
