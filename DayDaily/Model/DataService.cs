using DayDaily.Model.VO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public class DataService : IDataService
    {
        IList<DeveloperInfo> _developers = new List<DeveloperInfo>();

        Task _loadingTask;
        DeveloperInfo _selectedDeveloper = null;

        public async Task LoadAsync()
        {
            _loadingTask = Task.Run(() =>
            {
                using (var service = ChromeDriverService.CreateDefaultService())
                {
                    var options = new ChromeOptions();
#if !DEBUG
                    service.HideCommandPromptWindow = true;
                    options.AddArgument("headless");
#endif
                    using (var driver = new ChromeDriver(service, options))
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                        //driver.Manage().Window.FullScreen();
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                        driver.Url = "https://bayle.atlassian.net/secure/RapidBoard.jspa?rapidView=1";

                        wait.Until(d => d.FindElement(By.Id("google-signin-button"))).Click();

                    wait.Until(d => d.FindElement(By.Id("identifierId"))).SendKeys("ID");
                        driver.FindElement(By.Id("identifierNext")).Click();

                    wait.Until(d => d.FindElement(By.Name("password"))).SendKeys("PW");
                        driver.FindElement(By.Id("passwordNext")).Click();

                        int attempt = 0;
                        while (attempt < 2)
                        {
                            try
                            {
                                var swimlanes = wait.Until(d => d.FindElements(By.ClassName("ghx-swimlane")));
                                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(100);
                                foreach (var swimlane in swimlanes)
                                {
                                    var userName = swimlane.FindElement(By.ClassName("ghx-heading")).FindElement(By.XPath("//span[@role='button']")).Text;
                                    DeveloperInfo user = new DeveloperInfo(new UserInfo() { Name = userName });
                                    var issueColumns = swimlane.FindElements(By.ClassName("ghx-wrap-issue"));
                                    foreach (var issueColumn in issueColumns)
                                    {
                                        var issues = issueColumn.FindElements(By.ClassName("ghx-issue"));
                                        foreach (var issue in issues)
                                        {
                                            var summary = issue.FindElement(By.ClassName("ghx-summary"));
                                            var title = summary.GetAttribute("data-tooltip");
                                            user.JiraItems.Add(new JiraItem(title));
                                        }
                                    }
                                    _developers.Add(user);
                                }
                                break;
                            }
                            catch (StaleElementReferenceException) { }
                            attempt++;
                        }
                    }
                }
            });
            await _loadingTask;
        }

        public IList<UserInfo> GetAllUserInfos() => new List<UserInfo>(from developer in _developers select developer.UserInfo);

        public DeveloperInfo GetCurrentDeveloperInfo() => _selectedDeveloper;

        public void SetCurrentDeveloper(string name)
        {
            var selctedDeveloper = from developer in _developers
                                   where developer.UserInfo.Name == name
                                   select developer;
            _selectedDeveloper = selctedDeveloper.ElementAt(0);
        }

        public string GetWelcomeMessage()
        {
            throw new NotImplementedException();
        }
    }
}