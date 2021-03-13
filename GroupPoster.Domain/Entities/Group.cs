using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.Domain.Enums;

namespace GroupPoster.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        //public GroupStatus Status { get; set; }
    }
}
