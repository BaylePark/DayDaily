using DayDaily.Model.VO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DayDaily.Model
{
    public class SecDataService : IDataService
    {
        IDictionary<string, UserInfo> _users = new ConcurrentDictionary<string, UserInfo>();
        ConcurrentBag<JiraItem> _jiraItems = new ConcurrentBag<JiraItem>();
        IList<UserInfo> _orderedUsers = new List<UserInfo>();

        public UserInfo CurrentUser { get; set; }

        public void AddOrderedUser(UserInfo user)
        {
            _orderedUsers.Add(user);
        }

        public IList<UserInfo> GetAllUserInfos() => new List<UserInfo>(from keyValuePair in _users select keyValuePair.Value);

        public IList<JiraItem> GetJiraItemsByUserName(string name) => new List<JiraItem>(from jiraitem in _jiraItems where jiraitem.User.Name == name select jiraitem);

        readonly object _syncPrimitive = new object();

        public async Task LoadAsync()
        {
            await Task.Run(async () =>
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

                string tokenUrl = "";
                /*
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
                */
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    System.Windows.Forms.WebBrowser wb = new System.Windows.Forms.WebBrowser();
                    wb.Url = new System.Uri("http://sso.vd.sec.samsung.net/", System.UriKind.Absolute);
                    bool firstRun = true;
                    wb.DocumentCompleted += (s, e) =>
                    {
                        if (firstRun)
                        {
                            wb.Document.InvokeScript("getUrlToken", new object[] { "wiki" });
                            firstRun = false;
                        }
                        else if (wb.Url.ToString().ToLower().Contains("connect"))
                        {
                            System.Windows.Forms.HtmlElement scriptElem = wb.Document.CreateElement("script");
                            scriptElem.SetAttribute("type", "text/javascript");
                            scriptElem.SetAttribute("text", "window.alert = function () { }; window.confirm=function () { }; ");
                            wb.Document.GetElementsByTagName("head")[0].AppendChild(scriptElem);

                            if (wb.Document.Forms.Count == 0)
                            {
                                return;
                            }

                            System.Windows.Forms.HtmlElement elem = wb.Document.Forms[0];
                            System.Windows.Forms.HtmlElementCollection elemColl = elem.GetElementsByTagName("input").GetElementsByName("tokenUrl");
                            if (elemColl == null || elemColl.Count == 0)
                            {
                                return;
                            }

                            tokenUrl = elemColl[0].GetAttribute("value");
                            lock (_syncPrimitive)
                            {
                                Monitor.Pulse(_syncPrimitive);
                            }
                        }
                    };
                }), null);

                lock (_syncPrimitive)
                {
                    Monitor.Wait(_syncPrimitive);
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
                        IEnumerable<Task> parseTask = from issue in issues select Parse(issue);

                        await Task.WhenAll(parseTask);
                    }
                }
            });
        }

        private async Task Parse(IWebElement issue)
        {
            await Task.Run(() =>
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

                var userinfoStrs = assignee.Split(new char[] { '/', ':' }, 4);
                var userName = userinfoStrs[1].Trim();
                UserInfo userInfo = null;
                if (_users.ContainsKey(userName) == false)
                {
                    userInfo = new UserInfo()
                    {
                        Name = userName,
                        SingleID = userinfoStrs[2].Trim(),
                        Belong = userinfoStrs[3].Trim()
                    };
                    _users.Add(userInfo.Name, userInfo);
                }
                else
                {
                    userInfo = _users[userName];
                }

                var jiraItem = new JiraItem(key, title, userInfo)
                {
                    Status = ParseJiraItemStatus(status.Split(':')[1]),
                    Type = ParseJiraItemType(type),
                };

                _jiraItems.Add(jiraItem);
            });
        }

        private JiraItemType ParseJiraItemType(string type)
        {
            JiraItemType ret = JiraItemType.Task;
            switch (type.ToLower().Trim())
            {
                case "epic":
                    ret = JiraItemType.Epic;
                    break;
                case "story":
                    ret = JiraItemType.Story;
                    break;
                case "task":
                    ret = JiraItemType.Task;
                    break;
                case "sub-task":
                    ret = JiraItemType.SubTask;
                    break;
                case "bug":
                    ret = JiraItemType.Bug;
                    break;
            }
            return ret;
        }

        private JiraItemStatus ParseJiraItemStatus(string status)
        {
            JiraItemStatus ret = JiraItemStatus.Backlog;
            switch (status.ToLower().Trim())
            {
                case "backlog":
                    ret = JiraItemStatus.Backlog;
                    break;
                case "to do":
                    ret = JiraItemStatus.ToDo;
                    break;
                case "in progress":
                    ret = JiraItemStatus.InProgress;
                    break;
                case "pending":
                    ret = JiraItemStatus.Pending;
                    break;
                case "done":
                    ret = JiraItemStatus.Done;
                    break;
            }
            return ret;
        }

        public void SetUserByOrder(int index)
        {
            CurrentUser = _orderedUsers[index];
        }
    }
}