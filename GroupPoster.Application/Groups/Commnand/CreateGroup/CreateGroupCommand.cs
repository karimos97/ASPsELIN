using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;

namespace GroupPoster.ApplicationLayer.Groups.Commnand.CreateGroup
{
    public class CreateGroupCommand
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
    }

    public class CreateGroupCommandHandler
    {
        private readonly ISystemPersistence persistence;

        public CreateGroupCommandHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public Task<int> Handle(CreateGroupCommand request)
        {
            Group group = new()
            {
                Name = request.Name,
                Link = request.Link,
                //Status = Enum.Parse<GroupStatus>(request.Status)
            };

            return persistence.AddGroup(group);
        }
    }
}
