using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Posts.Command.UpdatePost
{
    public class UpdatePostCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UpdatePostCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public UpdatePostCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task Handle(UpdatePostCommand request)
        {
            Post post = new()
            {
                Id = request.Id,
                Title = request.Title,
                Content = request.Content
            };

            return persistence.UpdatePost(post);
        }
    }
}
