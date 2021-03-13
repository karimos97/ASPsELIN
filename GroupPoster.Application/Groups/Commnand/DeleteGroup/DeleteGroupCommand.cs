using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;

namespace GroupPoster.ApplicationLayer.Groups.Commnand.DeleteGroup
{
    public class DeleteGroupCommand
    {
        public int Id { get; set; }
    }

    public class DeleteGroupCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public DeleteGroupCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task Handle(DeleteGroupCommand request)
        {
            return persistence.DeleteGroup(request.Id);
        }
    }
}
