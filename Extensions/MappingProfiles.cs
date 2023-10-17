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
            CreateMap<UpdateProductMediaDto, Product>();
            CreateMap<CreateFeatureDto, Feature>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<CreateBrandDto, Brand>();
            CreateMap<CreateBrokerDto, Broker>();
            CreateMap<CreateSettingsDto, Setting>();
            CreateMap<CeoOptimizationDto, CeoOptimization>();
            CreateMap<UpdateCeoOptimizationDto, CeoOptimization>();
            CreateMap<SocialNetworkDto,SocialNetwork > ();
            CreateMap<UpdateSocialNetworkDto, SocialNetwork>();
            CreateMap<MainMenuDto, MainMenu>();
            CreateMap<UpdateMainMenuDto, MainMenu>();           
            CreateMap<QuickAccessDto, QuickAccess > ();
            CreateMap<UpdateQuickAccessDto, QuickAccess>();            
            CreateMap<LogoDto, Logo>();
            CreateMap<UpdateLogoDto, Logo>();
            CreateMap<CreateSlideDto, Slide>();
            CreateMap<UpdateSlideDto, Slide>();
            CreateMap<PartnerDto, Partner>();
            CreateMap<UpdatePartnerDto, Partner>();
            CreateMap<CreateArtistDto, Artist>();
            CreateMap<UpdateArtistDto, Artist>();
            CreateMap<CreatePageItemDto, PageItem>();
            CreateMap<UpdatePageItemDto, PageItem>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<CreateMessageDto, Message>();
            CreateMap<CreateArticleDto, Article>();
            CreateMap<UpdateArticleDto, Article>();
            CreateMap<CreateUsageDto, Usage>();
            CreateMap<CreateSizeDto, Size>();


        }
    }
}
