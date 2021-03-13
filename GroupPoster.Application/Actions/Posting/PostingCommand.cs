using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;

namespace GroupPoster.ApplicationLayer.Actions.Posting
{
    public class PostingCommand
    {
        public List<string> Emails { get; set; } = new();
        public List<string> Passwords { get; set; } = new();
        public string Content { get; set; }
        public string Link { get; set; }
    }

    public class PostingCommandHandler
    {
        private readonly IFBAccess access;

        public PostingCommandHandler(IFBAccess access)
        {
            this.access = access;
        }

        public async Task Handle(PostingCommand command)
        {
            string link = command.Link;
            string content = command.Content;

            for (int i = 0; i < command.Emails.Count; i++)
            {
                await access.Post(command.Emails[i], command.Passwords[i], link, content);
                access.Reset();
            }
        }
    }
}
