using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Posts.Query.GetPosts
{
    public class GetPostsQuery
    {
        
    }

    public class GetPostsQueryHandler
    {
        private readonly ISystemPersistence persistence;

        public GetPostsQueryHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request)
        {
            IEnumerable<Post> posts = await persistence.GetAllPosts();
            return posts.Select(x => PostDto.From(x));
        }
    }
}
