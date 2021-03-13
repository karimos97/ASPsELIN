using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors;
using OpenQA.Selenium;

namespace GroupPoster.Infrastructure.BrowserAccess
{
    public class LoginHandler
    {
        private readonly BrowserInteractors.BrowserInteractor interactor;

        public LoginHandler(BrowserInteractors.BrowserInteractor interactor)
        {
            this.interactor = interactor;
        }

        public async Task LogIntoFacebook(string email, string password)
        {
            await interactor.Navigate("https://www.facebook.com").ConfigureAwait(false);

            ICollection<IWebElement> emailTextFields = interactor.FindElements(By.Id("email"));
            ICollection<IWebElement> passwordTextFields = interactor.FindElements(By.Id("pass"));
            ICollection<IWebElement> submitButtons = interactor.FindElements(By.CssSelector("button[type=\"submit\"]"));

            if (emailTextFields.Count > 0 && passwordTextFields.Count > 0 && submitButtons.Count > 0)
            {
                IWebElement emailField = emailTextFields.First();
                IWebElement passwordField = passwordTextFields.First();
                IWebElement submitButton = submitButtons.First();

                emailField.SendKeys(email);
                passwordField.SendKeys(password);
                await interactor.Click(submitButton);

                interactor.WaitUntilElementUnLoads(submitButton);
            }
        }

        public bool CheckIfLogInSuccessful()
        {
            return !interactor.CurrentUrl.Contains("/login/?");
        }
    }
}
