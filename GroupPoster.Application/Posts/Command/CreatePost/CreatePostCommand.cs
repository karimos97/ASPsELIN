using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Posts.Command.CreatePost
{
    public class CreatePostCommand
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class CreatePostCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public CreatePostCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task<int> Handle(CreatePostCommand request)
        {
            Post post = new()
            {
                Title = request.Title,
                Content = request.Content
            };

            return persistence.AddPost(post);
        }
    }
}
