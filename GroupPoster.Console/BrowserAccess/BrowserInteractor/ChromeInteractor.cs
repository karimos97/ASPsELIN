using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace GroupPoster.Infrastructure.BrowserAccess.BrowserInteractors
{
    public class ChromeInteractor
    {
        private readonly IWebDriver webDriver;
        private readonly WebDriverWait webWaiter;
        private readonly IJavaScriptExecutor jsExecuter;

        public ChromeInteractor()
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-notifications");

            this.webWaiter = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
            this.jsExecuter = (IJavaScriptExecutor)webDriver;
        }

        public IWebElement FindElement(By target)
        {
            return webDriver.FindElement(target);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By target)
        {
            return webDriver.FindElements(target);
        }

        public IWebElement WaitUntilElementLoad(By target)
        {
            try
            {
                return webWaiter.Until(ExpectedConditions.ElementToBeClickable(target));
            }
            catch
            {
                return null;
            }
        }

        public Task Navigate(string url)
        {
            webDriver.Navigate().GoToUrl(url);
            return Task.Delay(1000);
        }

        public Task Click(IWebElement element)
        {
            jsExecuter.ExecuteScript("arguments[0].click()", element);
            return Task.Delay(1000);
        }

        public void WaitUntilElementUnLoads(IWebElement element)
        {
            webWaiter.Until(ExpectedConditions.StalenessOf(element));
        }

        public Task ScrollTo(IWebElement element)
        {
            jsExecuter.ExecuteScript("arguments[0].scrollIntoView()", element);
            return Task.Delay(1000);
        }
    }
}
