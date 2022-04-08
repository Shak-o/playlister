using AutoMapper;
using PlayLister.Infrastructure.Models;
using PlayLister.Services.Models;

namespace PlayLister.Client.Mapping
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<PlaylistData, PlaylistRepoModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.PlaylistId))
                .ForMember(x => x.Items, opt => opt.MapFrom<ItemCustomConfiguration>());
            CreateMap<PlaylistRepoModel, PlaylistData>();
            CreateMap<ItemRepoModel, YoutubeItem>();
            CreateMap<YoutubeItem, ItemRepoModel>();
        }
    }
}
