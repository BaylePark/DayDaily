using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayDaily.Model
{
    public class DataService : IDataService
    {
        Task _loadingTask;

        IList<User> _users = new List<User>();

        public async Task LoadAsync()
        {
            _loadingTask = Task.Run(() =>
            {
                using (var driver = new ChromeDriver())
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                    driver.Manage().Window.FullScreen();
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
                                User user = new User(userName);
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
                                _users.Add(user);
                            }
                            break;
                        }
                        catch (StaleElementReferenceException) { }
                        attempt++;
                    }
                }
            });
            await _loadingTask;
        }

        public async Task<IList<User>> GetUsers()
        {
            await Task.Run(() => { });

            return null;
        }
    }
}