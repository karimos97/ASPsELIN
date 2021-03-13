using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupPoster.Domain.Enums;
using GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors;
using OpenQA.Selenium;

namespace GroupPoster.Infrastructure.BrowserAccess
{
    public class GroupHandler
    {
        private readonly BrowserInteractor interactor;

        public GroupHandler(BrowserInteractor interactor)
        {
            this.interactor = interactor;
        }

        public Task NavigateToGroup(string groupLink)
        {
            return interactor.Navigate(groupLink);
        }

        public async Task Post(string content)
        {
            IWebElement textBox = interactor.WaitUntilElementLoad(By.CssSelector("div.b3i9ofy5"));

            await interactor.Click(textBox);

            IWebElement contentTextBox = interactor.WaitUntilElementLoad(By.CssSelector("div[class=\"l9j0dhe7 tkr6xdv7\"] form div[role=\"textbox\"]"));

            await interactor.Click(contentTextBox);

            contentTextBox.SendKeys(content);

            ICollection<IWebElement> postButtons = interactor.FindElements(By.CssSelector("div[class=\"l9j0dhe7 tkr6xdv7\"] form .ihqw7lf3 > div:nth-child(2) > div > div[role=\"button\"]"));

            if (postButtons.Count > 0)
            {
                await interactor.Click(postButtons.First());
            }
        }

        public Task JoinGroup()
        {
            var button = interactor.FindElement(By.CssSelector(".f4c7eilb > .s1i5eluu"));
            return interactor.Click(button);
        }
        //public GroupStatus CheckGroupStatus()
        //{
        //    return interactor
        //        .FindElements(
        //        By.CssSelector
        //        ("div.cddn0xzi i[class=\"hu5pjgll m6k467ps sp_IrYbSX_CP9K sx_d4d1c5\"]"))
        //        .Count > 0 
        //        ? GroupStatus.Public
        //        : GroupStatus.Private;
        //}

        public GroupMemberStatus GetMemberStatus()
        {
            if (interactor.FindElements(By.CssSelector(".f4c7eilb > .s1i5eluu")).Count > 0)
            {
                //Yet To Join
                return GroupMemberStatus.NotJoined;
            }
            else if (interactor.FindElements(By.CssSelector(".rq0escxv > .s1i5eluu")).Count > 0)
            {
                //Joined
                return GroupMemberStatus.Joined;
            }
            else
            {
                //Pending
                return GroupMemberStatus.Pending;
            }
        }
    }
}
