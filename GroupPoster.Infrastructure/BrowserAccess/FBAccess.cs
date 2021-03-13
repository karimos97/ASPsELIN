using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.ApplicationLayer.Common.Interfaces;
using GroupPoster.Domain.Entities;
using GroupPoster.Domain.Enums;
using GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors;

namespace GroupPoster.Infrastructure.BrowserAccess
{
    public class FBAccess : IFBAccess
    {
        private readonly BrowserInteractor interactor;
        private readonly GroupHandler groupHandler;
        private readonly LoginHandler loginHandler;

        public FBAccess(BrowserInteractor interactor)
        {
            groupHandler = new(interactor);
            loginHandler = new(interactor);
            this.interactor = interactor;
        }

        public async Task<bool> LogIntoFb(string email, string password)
        {
            await loginHandler.LogIntoFacebook(email, password);
            return loginHandler.CheckIfLogInSuccessful();
        }

        public GroupMemberStatus CheckGroupMemberStatus() => groupHandler.GetMemberStatus();

        //public GroupStatus CheckGroupStatus() => groupHandler.CheckGroupStatus();

        public void Dispose() => interactor.Dispose();

        public async Task Post(string email, string passowrd, string group, string content)
        {
            if (await LogIntoFb(email, passowrd))
            {
                await interactor.Navigate(group);

                switch (CheckGroupMemberStatus())
                {
                    case GroupMemberStatus.NotJoined:
                        await groupHandler.JoinGroup();
                        break;
                    case GroupMemberStatus.Joined:
                        await groupHandler.Post(content);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Reset()
        {
            interactor.Reset();
        }
    }
}
