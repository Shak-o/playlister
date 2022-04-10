using AutoMapper;
using PlayLister.Infrastructure.Models;
using PlayLister.Services.Models;
using PlayLister.Services.Models.ServiceModels;

namespace PlayLister.Client.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<PlaylistData, PlaylistRepoModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.PlaylistId))
                .ForMember(x => x.Items, opt => opt.MapFrom<ItemCustomConfiguration>())
                .ForMember(x => x.TotalResults, opt => opt.MapFrom(x => x.PageInfo.TotalResults))
                .ForMember(x => x.ResultsPerPage, opt => opt.MapFrom(x => x.PageInfo.ResultsPerPage));
            CreateMap<PlaylistRepoModel, PlaylistData>();
            CreateMap<ItemRepoModel, YoutubeItem>();
            CreateMap<YoutubeItem, ItemRepoModel>();

            CreateMap<PlaylistServiceModel, PlaylistRepoModel>();
            CreateMap<PlaylistRepoModel, PlaylistServiceModel>();
            CreateMap<ItemServiceModel, ItemRepoModel>();
            CreateMap<ItemRepoModel, ItemServiceModel>();
        }
    }
}
