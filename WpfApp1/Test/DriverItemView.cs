using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace WpfApp1.Test
{
    class DriverItemView
    {

        private DriverItemController dv;
        private string GoToUrl = string.Empty;
        private string BodyString = string.Empty;
        private string[] reviewiteminfo = { string.Empty, string.Empty };
        private string[] nextPageLocate = { string.Empty, string.Empty };
        private string[] moreInfoLocate = { string.Empty, string.Empty };
        private List<string[]> fields = new List<string[]>();
        private List<string[]> infos = new List<string[]>();
        public List<string[]> Infos { get => infos; set => infos = value; }
        public string[] Reviewiteminfo { get => reviewiteminfo; set => reviewiteminfo = value; }
        public string[] NextPageLocate { get => nextPageLocate; set => nextPageLocate = value; }
        public string[] MoreInfoLocate { get => moreInfoLocate; set => moreInfoLocate = value; }

        public DriverItemView()
        {

        }

        public DriverItemView(string GoToUrl)
        {
            this.GoToUrl = GoToUrl;
            GetInfos(GoToUrl);
            dv = new DriverItemController();
        }

        public void GetInfos(string GoToUrl)
        {
            infos.Clear();
            infos = null;
            infos = new List<string[]>();
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(GoToUrl);
                OpenQA.Selenium.Support.UI.WebDriverWait webDriverWait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(13));
                webDriverWait.Until(d => { return driver; });
                new Actions(driver).SendKeys(Keys.Control + Keys.End).Perform();
                webDriverWait.Until(d => {
                    try
                    {
                        (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                        return driver.FindElement(By.TagName("*"));
                    }
                    catch (Exception)
                    {
                        (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                        return null;
                    }
                });
                (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                webDriverWait.Until(d => {
                    try
                    {
                        (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform(); 
                        driver.FindElement(By.TagName("*"));
                        return true;
                    }
                    catch (Exception)
                    {
                        (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                        return false;
                    }
                });

                this.BodyString = driver.FindElement(By.TagName("body")).Text;

                try
                {
                    while (true)
                    {
                        try
                        {
                            driver.FindElements(MyDetermineBy(moreInfoLocate));
                        }
                        catch (Exception) { }
                        webDriverWait.Until(d => {
                            try
                            {
                                (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                                return driver.FindElement(MyDetermineBy(reviewiteminfo));
                            }
                            catch (Exception)
                            {
                                (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                                return null;
                            }
                        });
                        foreach (var item in driver.FindElements(MyDetermineBy(reviewiteminfo)))
                        {
                            string[] l = new string[fields.Count];
                            for (int i = 0; i < fields.Count; i++)
                            {
                                try
                                {
                                    l[i] = item.FindElement(MyDetermineBy(fields[i])).Text;
                                    //case "R":
                                    //    l[i] = System.Text.RegularExpressions.Regex.Match(item.Text, fields[i][1]).Value;
                                    //    break;
                                }
                                catch (Exception) { l[i] = "-"; }
                            }
                            infos.Add(l);
                        }
                        if (driver.FindElement(MyDetermineBy(nextPageLocate)).GetAttribute("href")!=""&& driver.FindElement(MyDetermineBy(nextPageLocate)).GetAttribute("href")!=string.Empty&& driver.FindElement(MyDetermineBy(nextPageLocate)).GetAttribute("href")!=null)
                        {
                            driver.FindElement(MyDetermineBy(nextPageLocate)).Click();
                        }
                        else
                        {
                            break;
                        }
                        try
                        {
                            webDriverWait.Until(d => {
                                try
                                {
                                    (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                                    return driver.FindElement(MyDetermineBy(reviewiteminfo));
                                }
                                catch (Exception) {
                                    (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                                    return null;
                                }
                            });
                        }
                        catch (Exception){ break; }
                    }
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// 判断类型
        /// </summary>
        /// <param name="vs">需要判断的数据源</param>
        /// <returns></returns>
        private By MyDetermineBy(string[] vs)
        {
            switch (vs[0])
            {
                case "L":
                    return By.TagName(vs[1]);
                case "X":
                    return By.XPath(vs[1]);
                case "CS":
                    return By.CssSelector(vs[1]);
                case "CL":
                    return By.ClassName(vs[1]);
                case "I":
                    return By.Id(vs[1]);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="items">创建的字段组</param>
        public void GetFields(ItemCollection items)
        {
            fields.Clear();
            fields = null;
            fields = new List<string[]>();
            for (int i = 0; i < items.Count; i++)
            {
                fields.Add(new string[] { (items[i] as TextBlock).Tag.ToString(), (items[i] as TextBlock).Text });
            }
        }

        public string ToString(int a)
        {
            return BodyString;
        }
    }
}
