using DayDaily.Model.VO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public class SecDataService : IDataService
    {
        IList<DeveloperInfo> _developers = new List<DeveloperInfo>();

        Task _loadingTask;
        DeveloperInfo _selectedDeveloper = null;

        public async Task LoadAsync()
        {
            _loadingTask = Task.Run(() =>
            {
                var alertWaitCondition = new Func<IWebDriver, IAlert>((d) =>
                {
                    try
                    {
                        return d.SwitchTo().Alert();
                    }
                    catch (NoAlertPresentException)
                    {
                        return null;
                    }
                });

                string tokenUrl;
                using (var service = InternetExplorerDriverService.CreateDefaultService())
                {
                    var options = new InternetExplorerOptions();
                    service.HideCommandPromptWindow = true;
                    options.InitialBrowserUrl = "http://sso.vd.sec.samsung.net/";
                    options.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
                    options.IgnoreZoomLevel = true;
                    options.EnableNativeEvents = false;

                    using (var driver = new InternetExplorerDriver(service, options))
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                        driver.ExecuteScript("window.onbeforeunload = function() {};");
                        driver.ExecuteScript("window.alert = function() {};");
                        driver.ExecuteScript("window.confirm = function() {};");


                        //wait.Until(d => d.FindElement(By.XPath("//input[@type='button' and @value='Jira URL 생성']"))).Click();
                        driver.ExecuteScript("getUrlToken()", new object[] { "jira" });
                        wait.Until(alertWaitCondition).Accept();
                        wait.Until(alertWaitCondition).Accept();
                        tokenUrl = wait.Until(d => d.FindElement(By.XPath("//input[@name='tokenUrl']"))).GetAttribute("value");
                    }
                }

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

                        driver.Url = tokenUrl;
                        driver.Url = "http://jira.vd.sec.samsung.net/secure/RapidBoard.jspa?rapidView=3332";

                        var issues = wait.Until(d => d.FindElements(By.ClassName("ghx-issue")));
                        foreach (var issue in issues)
                        {
                            var type = issue.FindElement(By.ClassName("ghx-type")).GetAttribute("title");
                            var key = issue.FindElement(By.ClassName("ghx-key")).FindElement(By.TagName("a")).GetAttribute("title");
                            var title = issue.FindElement(By.ClassName("ghx-summary")).GetAttribute("title");
                            var assignee = issue.FindElement(By.ClassName("ghx-avatar-img")).GetAttribute("data-tooltip");
                            string status = null;
                            foreach (var extrafield in issue.FindElements(By.ClassName("ghx-extra-field")))
                            {
                                if (extrafield.GetAttribute("data-tooltip").Contains("Status"))
                                {
                                    status = extrafield.GetAttribute("data-tooltip");
                                    break;
                                }
                            }
                            System.Diagnostics.Debug.WriteLine("{0} / {1} / {2} / {3} / {4}", new object[] { type, key, title, assignee, status });
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