using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;

namespace WpfApp1.Test
{
    public class Class1
    {
        private List<MyInfos_Untreated> strs = new List<MyInfos_Untreated>();
        private List<ListViewShowInfo> infos = new List<ListViewShowInfo>();
        public List<ListViewShowInfo> Infos { get => infos; set => infos = value; }

        public Class1()
        {

        }

        public Class1(string GoToUrl = "https://movie.douban.com/tag/")
        {
            GetInfos();
            MyClassify();
        }



        /// <summary>
        /// 整理信息
        /// </summary>
        /// <returns></returns>
        public void MyClassify()
        {
            for (int i = 0; i < strs.Count; i++)
            {
                string[] strs0 = strs[i].Str0.Split('\n');
                string[] strs1 = strs[i].Str1.Split('\n');

                ListViewShowInfo showInfo = new ListViewShowInfo();
                showInfo.Str00 = strs[i].Str3;
                showInfo.Str01 = strs[i].Str2;
                showInfo.Str02 = strs0[0].Split(':')[1];
                showInfo.Str03 = strs0[1].Split(':')[1];
                showInfo.Str04 = strs0[2].Split(':')[1];
                showInfo.Str05 = strs0[3].Split(':')[1];
                System.Text.RegularExpressions.Match reMatch = System.Text.RegularExpressions.Regex.Match(strs[i].Str0, "制片国家/地区:.*\\r");
                showInfo.Str06 = reMatch.Value.Split(':')[1];
                reMatch = System.Text.RegularExpressions.Regex.Match(strs[i].Str0, "语言:.*\\r");
                showInfo.Str07 = reMatch.Value.Split(':')[1];
                reMatch = System.Text.RegularExpressions.Regex.Match(strs[i].Str0, "首播:.*\\r");
                try
                {
                    showInfo.Str08 = reMatch.Value.Split(':')[1];
                }
                catch (Exception)
                {
                    reMatch = System.Text.RegularExpressions.Regex.Match(strs[i].Str0, "上映日期:.*\\r");
                    showInfo.Str08 = reMatch.Value.Split(':')[1];
                }
                reMatch = System.Text.RegularExpressions.Regex.Match(strs[i].Str0, "又名:.*\\r");
                showInfo.Str09 = reMatch.Value.Split(':')[1];
                showInfo.Str10 = strs0.Last().Split(':')[1];

                showInfo.Str11 = strs1[2];
                showInfo.Str12 = strs1[1];
                showInfo.Str13 = strs1[4];
                showInfo.Str14 = strs1[6];
                showInfo.Str15 = strs1[8];
                showInfo.Str16 = strs1[10];
                showInfo.Str17 = strs1[12];

                infos.Add(showInfo);
                string asdasd = "";
            }
        }

        /// <summary>
        /// 爬取信息
        /// </summary>
        /// <param name="GoToUrl"></param>
        private void GetInfos(string GoToUrl= "https://movie.douban.com/tag/")
        {
            string str0 = "";
            string str1 = "";
            string str2 = "";
            string str3 = "";

            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl(GoToUrl);  //driver.Url = "http://www.baidu.com"是一样的

                System.Threading.Thread.Sleep(2000);
                (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                System.Threading.Thread.Sleep(1500);
                (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
                System.Threading.Thread.Sleep(1500);

                var tableCol = driver.FindElements(By.XPath("//*[@id='app']/div/div[1]/div[3]/a"));
                //var tableCol = driver.FindElements(By.XPath("//a[@class='item' and @target='_blank']"));
                for (int i = 0; i < tableCol.Count; i++)
                {
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                    tableCol[i].Click();
                }
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

                //FirefoxDriver driver1 = new FirefoxDriver();
                for (int i = 1; i <= tableCol.Count; i++)
                {
                    //var headers = driver.WindowHandles;
                    //tableCol = driver.FindElements(By.XPath("//a[@class='item' and @target='_blank']"));
                    //(new Actions(driver)).SendKeys(Keys.Control + Keys.Tab).Perform();
                    //System.Threading.Thread.Sleep(new Random().Next(100, 200));

                    driver.SwitchTo().Window(driver.WindowHandles[i]);
                    try
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                        driver.FindElement(By.XPath("//a[@class='more-actor' and @href='javascript:;']")).Click();
                        str0 = driver.FindElement(By.XPath("//div[@id='info']")).Text;
                        str1 = driver.FindElement(By.XPath("//div[@id='interest_sectl']")).Text;
                        str2 = driver.FindElement(By.XPath("//span[@property='v:summary']")).Text;
                        str3 = driver.FindElement(By.XPath("//span[@property='v:itemreviewed']")).Text;
                    }
                    catch (Exception)
                    {
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                        driver.FindElement(By.XPath("//a[@class='more-actor' and @href='javascript:;']")).Click();
                        str0 = driver.FindElement(By.XPath("//div[@id='info']")).Text;
                        str1 = driver.FindElement(By.XPath("//div[@id='interest_sectl']")).Text;
                        str2 = driver.FindElement(By.XPath("//span[@property='v:summary']")).Text;
                        str3 = driver.FindElement(By.XPath("//span[@property='v:itemreviewed']")).Text;
                    }
                    strs.Add(new MyInfos_Untreated(str0, str1, str2,str3));
                }

                driver.SwitchTo().Window(driver.WindowHandles[0]);

            }
        }

        public string ToString(int a)
        {
            return "一共爬取了：" + strs.Count() + "\n\n最后一个的信息：\n" + strs[0].Str0 + "\n\n" + strs[0].Str1 + "\n\n" + strs[0].Str2;
        }

    }
}

//基于必应的
            //using (IWebDriver driver = new FirefoxDriver())
            //{
            //    driver.Navigate().GoToUrl(GoToUrl);  //driver.Url = "http://www.baidu.com"是一样的
            //    str = driver.PageSource;
            //    try
            //    {
            //        System.Threading.Thread.Sleep(2000);
            //        (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
            //        System.Threading.Thread.Sleep(1500);
            //        (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
            //        System.Threading.Thread.Sleep(1500);
            //        (new Actions(driver)).SendKeys(Keys.Control + Keys.End).Perform();
            //        System.Threading.Thread.Sleep(1500);

            //        var imgs = driver.FindElements(By.TagName("li"));
            //        for (int i = 0; i < imgs.Count; i++)
            //        {
            //            try
            //            {
            //                //基于必应的
            //                string imgUrl = imgs[i].FindElement(By.TagName("img")).GetAttribute("src");
            //                if (imgUrl.Length > 300) continue;

            //                HttpClient client = new HttpClient();
            //                client.Credentials = CredentialCache.DefaultCredentials;
            //                client.Headers.Add(HttpRequestHeader.Cookie, driver.WindowHandles[0]);
            //                client.Headers.Add(HttpRequestHeader.Referer, "https://www.bing.com/");//需要必应的url
            //                string a = "./img/" + imgUrl.ToString().Split('/').Last().Split('.').First() + i + ".jpg";
            //                client.DownloadFile(imgUrl, a);
            //                client.Dispose();
            //            }
            //            catch (Exception e)
            //            {
            //                //MessageBox.Show(e.Message);
            //            }
            //        }
            //        str = driver.FindElement(By.TagName("body")).Text;
            //        str1 = str.Split(new char[] {'\r','\n' },StringSplitOptions.RemoveEmptyEntries);
            //    }
            //    catch (Exception e) { MessageBox.Show(e.Message); }
            //

//基于百度的爬虫
//                        try
//                        {

//                            string imgUrl = imgs[i].GetAttribute("src");
//                            if (imgUrl.Length > 300) continue;
//                            HttpClient client = new HttpClient();
//client.Credentials = CredentialCache.DefaultCredentials;
//                            client.Headers.Add(HttpRequestHeader.Cookie, driver.WindowHandles.ToString());
//                            client.Headers.Add(HttpRequestHeader.Referer, imgUrl);
//                            string a = "./img/" + imgUrl.ToString().Split('/').Last().Split('.').First() + ".jpg";
//client.DownloadFile(imgUrl, a);
//                            client.Dispose();
//                        }
//                        catch (Exception) {
//                            try
//                            {
//                                //基于百度爬取
//                                string imgUrl = imgs[i].GetAttribute("data-thumburl");
//                                if (imgUrl.Length > 300) continue;

//                                HttpClient client = new HttpClient();
//client.Credentials = CredentialCache.DefaultCredentials;
//                                string asdasd = driver.WindowHandles.ToString();
//client.Headers.Add(HttpRequestHeader.Cookie, driver.WindowHandles[0]);
//                                client.Headers.Add(HttpRequestHeader.Referer, "https://.bing.com/");//需要百度的url
//                                string a = "./img/" + imgUrl.ToString().Split('/').Last().Split('.').First() + ".jpg";
//client.DownloadFile(imgUrl, a);
//                                client.Dispose();
//                            }
//                            catch (Exception e)
//                            {
//                                //MessageBox.Show(e.Message);
//                            }
//                        }