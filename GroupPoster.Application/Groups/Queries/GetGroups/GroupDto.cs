using System;
using GroupPoster.Domain.Entities;

namespace GroupPoster.ApplicationLayer.Groups.Queries.GetGroups
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }

        public static GroupDto From(Group x) => new()
        {
            Id = x.Id,
            Name = x.Name,
            Link = x.Link,
            //Status = x.Status.ToString()
        };
    }
}
