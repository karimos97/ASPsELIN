using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;

namespace GroupPoster.ApplicationLayer.Posts.Command.DeletePost
{
    public class DeletePostCommand
    {
        public int Id { get; set; }
    }

    public class DeletePostCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public DeletePostCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task Handle(DeletePostCommand request)
        {
            return persistence.DeletePost(request.Id);
        }
    }
}
