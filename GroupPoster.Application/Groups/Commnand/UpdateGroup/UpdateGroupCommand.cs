using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;

namespace GroupPoster.ApplicationLayer.Groups.Commnand.UpdateGroup
{
    public class UpdateGroupCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
    }

    public class UpdatedGroupCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public UpdatedGroupCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task Handle(UpdateGroupCommand request)
        {
            Group group = new()
            {
                Id = request.Id,
                Name = request.Name,
                Link = request.Link,
                //Status = Enum.Parse<GroupStatus>(request.Status)
            };

            return persistence.UpdateGroup(group);
        }
    }
}
