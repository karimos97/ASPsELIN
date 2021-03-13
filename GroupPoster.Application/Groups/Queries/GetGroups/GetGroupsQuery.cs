using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Groups.Queries.GetGroups
{
    public class GetGroupsQuery
    {

    }

    public class GetGroupsQueryHandler
    {
        private readonly ISystemPersistence persistence;

        public GetGroupsQueryHandler(ISystemPersistence persistence)
        {
            this.persistence = persistence;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsQuery request)
        {
            IEnumerable<Group> groups = await persistence.GetAllGroups();
            return groups.Select(x => GroupDto.From(x));
        }
    }
}
