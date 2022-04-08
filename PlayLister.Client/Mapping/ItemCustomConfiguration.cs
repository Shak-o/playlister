using AutoMapper;
using PlayLister.Infrastructure.Models;
using PlayLister.Services.Models;

namespace PlayLister.Client.Mapping
{
    public class ItemCustomConfiguration : IValueResolver<PlaylistData, PlaylistRepoModel, List<ItemRepoModel>>
    {
        public List<ItemRepoModel> Resolve(PlaylistData source, PlaylistRepoModel destination, List<ItemRepoModel> destMember, ResolutionContext context)
        {
            var list = new List<ItemRepoModel>();
            foreach (var data in source.Items)
            {
                if (data != null && data.Snippet.Thumbnails.ThumbnailDefault != null)
                {
                    var converted = new ItemRepoModel()
                    {
                        Description = data.Snippet.Description,
                        Height = data.Snippet.Thumbnails.ThumbnailDefault.Height,
                        Title = data.Snippet.Title, Url = data.Snippet.Thumbnails.ThumbnailDefault.Url,
                        Width = data.Snippet.Thumbnails.ThumbnailDefault.Width
                    };
                    list.Add(converted);
                }
            }

            return list;
        }
    }
}
