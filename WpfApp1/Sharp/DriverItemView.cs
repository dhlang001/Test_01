using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;

namespace WpfApp1.Sharp
{
    class DriverItemView
    {

        private DriverItemController dv;
        private string GoToUrl = string.Empty;
        private string BodyString = string.Empty;
        private string[] reviewiteminfo = null;//{ string.Empty, string.Empty };
        private string[] nextPageLocate = null;
        private string[] moreInfoLocate = null;
        private List<string[]> fields = new List<string[]>();
        private List<string[]> infos = new List<string[]>();

        public List<string[]> Infos { get => infos; set => infos = value; }
        public string[] Reviewiteminfo { get => reviewiteminfo; set => reviewiteminfo = value; }
        public string[] NextPageLocate { get => nextPageLocate; set => nextPageLocate = value; }
        public string[] MoreInfoLocate { get => moreInfoLocate; set => moreInfoLocate = value; }

        //public DriverItemView()
        //{

        //}

        public DriverItemView(string GoToUrl="")
        {
            this.GoToUrl = GoToUrl;
            //GetInfos(GoToUrl);
            dv = new DriverItemController();

            //初始化
            BodyString = string.Empty;
            reviewiteminfo = new string[] { string.Empty, string.Empty };
            nextPageLocate = new string[] { string.Empty, string.Empty };
            moreInfoLocate = new string[] { string.Empty, string.Empty };
            fields = new List<string[]>();
            infos = new List<string[]>();
        }

        public void GetInfos(string GoToUrl)
        {
            infos.Clear();
            infos = null;
            infos = new List<string[]>();
            try
            {
                using (IWebDriver driver = new FirefoxDriver())
                {
                    driver.Navigate().GoToUrl(GoToUrl);
                    OpenQA.Selenium.Support.UI.WebDriverWait webDriverWait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(6));
                    webDriverWait.Until(d => { return true; });
                    new Actions(driver).SendKeys(Keys.Control + Keys.End).Perform();
                    webDriverWait.Until(d => {
                        try
                        {
                            (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                            Thread.Sleep(new TimeSpan(0, 0, 1));
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
                            Thread.Sleep(new TimeSpan(0, 0, 1));
                            driver.FindElement(By.TagName("*"));
                            return true;
                        }
                        catch (Exception)
                        {
                            (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                            return false;
                        }
                    });

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                if (moreInfoLocate[0]!="")
                                {
                                    webDriverWait.Until(d => {
                                        try
                                        {
                                            (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                                            Thread.Sleep(new TimeSpan(0, 0, 1));
                                            driver.FindElement(MyDetermineBy(moreInfoLocate)).Click();
                                            return true;
                                        }
                                        catch (Exception)
                                        {
                                            (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                                            return false;
                                        }
                                    });
                                }
                                //driver.FindElement(MyDetermineBy(moreInfoLocate)).Click();
                            }
                            catch (Exception) { }
                            if (reviewiteminfo!=null)
                            {
                                try
                                {
                                    webDriverWait.Until(d => {
                                        try
                                        {
                                            (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                                            Thread.Sleep(new TimeSpan(0, 0, 1));
                                            return driver.FindElement(MyDetermineBy(reviewiteminfo));
                                        }
                                        catch (Exception)
                                        {
                                            (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                                            return null;
                                        }
                                    });
                                }
                                catch (Exception) { }
                                foreach (var item in driver.FindElements(MyDetermineBy(reviewiteminfo[0]!=""?reviewiteminfo:new string[] {"L","body" })))
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
                            }
                            else
                            {
                                string[] l = new string[fields.Count];
                                l[0] = webDriverWait.Until(d => {
                                    try
                                    {
                                        (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                                        Thread.Sleep(new TimeSpan(0, 0, 1));
                                        return driver.FindElement(MyDetermineBy(fields[0]));
                                    }
                                    catch (Exception)
                                    {
                                        (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                                        return null;
                                    }
                                }).Text;
                                infos.Add(l);
                            }
                            if (driver.FindElement(MyDetermineBy(nextPageLocate)).GetAttribute("href") != "" && driver.FindElement(MyDetermineBy(nextPageLocate)).GetAttribute("href") != string.Empty && driver.FindElement(MyDetermineBy(nextPageLocate)).GetAttribute("href") != null && !nextPageLocate.Equals(new string[] { string.Empty, string.Empty }))
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
                                        Thread.Sleep(new TimeSpan(0, 0, 1));
                                        return driver.FindElement(MyDetermineBy(reviewiteminfo));
                                    }
                                    catch (Exception)
                                    {
                                        (new Actions(driver)).SendKeys(Keys.Control + Keys.Home).Perform();
                                        return null;
                                    }
                                });
                            }
                            catch (Exception) { break; }
                        }
                    }
                    catch (Exception e) {
                        //MessageBox.Show(e.Message); 
                    }

                    try
                    {
                        this.BodyString = driver.FindElement(By.TagName("body")).Text;
                    }
                    catch (Exception)
                    {
                        this.BodyString = "";
                    }
                }

            }
            catch (Exception) { }
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
