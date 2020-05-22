using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using Org.BouncyCastle.Bcpg;

namespace WpfApp1.Test
{
    class SettingItemCtrl
    {
        private XmlDocument xmldoc;

        public SettingItemCtrl()
        {
            xmldoc = new XmlDocument();
            LoadXml();
        }

        public int LoadXml(string path= @".\Data\Setting.xml")
        {
            try
            {
                xmldoc.Load(path);
            }
            catch (Exception)
            {
                System.IO.Directory.CreateDirectory(@".\Data");
                XmlNode node = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
                XmlNode st = xmldoc.CreateElement("Settings");
                xmldoc.AppendChild(node);
                xmldoc.AppendChild(st);
                xmldoc.Save(path);
            }
            return xmldoc.SelectSingleNode("Settings").ChildNodes.Count;
        }

        public List<Settings> ReadXml()
        {
            List<Settings> sts = new List<Settings>();
            for (int i = 0; i < xmldoc.SelectSingleNode("Settings").ChildNodes.Count; i++)
            {
                Settings st = new Settings();
                st.UrlST=xmldoc.SelectSingleNode("Settings").ChildNodes[i].FirstChild.InnerText;
                st.ReviewItemST = new string[] {
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[1].ChildNodes[0].InnerText,
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[1].ChildNodes[1].InnerText,
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[1].ChildNodes[2].InnerText
                };
                st.NextPageST = new string[] {
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[2].ChildNodes[0].InnerText,
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[2].ChildNodes[1].InnerText,
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[2].ChildNodes[2].InnerText
                };
                st.MoreInfoST = new string[] {
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[3].ChildNodes[0].InnerText,
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[3].ChildNodes[1].InnerText,
                    xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[3].ChildNodes[2].InnerText
                };
                for (int j = 0; j < xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[4].ChildNodes.Count; j++)
                {
                    TextBlock text = new TextBlock();
                    text.Text = xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[4].ChildNodes[j].ChildNodes[0].InnerText;
                    text.Tag = xmldoc.SelectSingleNode("Settings").ChildNodes[i].ChildNodes[4].ChildNodes[j].ChildNodes[1].InnerText;
                    st.LableST.Add(text);
                }
                sts.Add(st);
            }

            return sts;
        }

        public void WriteXml(Settings st)
        {
            XmlElement element= xmldoc.CreateElement("Setting"+(xmldoc.SelectSingleNode("Settings").ChildNodes.Count+1));
            XmlElement UrlST = xmldoc.CreateElement("Url");
            XmlElement ReviewItem = xmldoc.CreateElement("ReviewItem");
            XmlElement NextPage = xmldoc.CreateElement("NextPage");
            XmlElement MoreInfo = xmldoc.CreateElement("MoreInfo");
            XmlElement Labs = xmldoc.CreateElement("Lable");


            UrlST.InnerText = st.UrlST;

            for (int i = 0; i < 3; i++)
            {
                XmlElement a = xmldoc.CreateElement("ReviewItemST_" + i);
                a.InnerText = st.ReviewItemST[i];
                ReviewItem.AppendChild(a);
            }

            for (int i = 0; i < 3; i++)
            {
                XmlElement a = xmldoc.CreateElement("NextPageST_" + i);
                a.InnerText = st.NextPageST[i];
                NextPage.AppendChild(a);
            }

            for (int i = 0; i < 3; i++)
            {
                XmlElement a = xmldoc.CreateElement("MoreInfoST_" + i);
                a.InnerText = st.MoreInfoST[i];
                MoreInfo.AppendChild(a);
            }

            for (int i = 0; i < st.LableST.Count; i++)
            {
                XmlElement lab = xmldoc.CreateElement("Lable_"+(i+1));
                XmlElement a = xmldoc.CreateElement("Text");
                XmlElement b = xmldoc.CreateElement("Tag");
                a.InnerText = st.LableST[i].Text;
                b.InnerText = st.LableST[i].Tag.ToString();
                lab.AppendChild(a);
                lab.AppendChild(b);
                Labs.AppendChild(lab);
            }

            element.AppendChild(UrlST);
            element.AppendChild(ReviewItem);
            element.AppendChild(NextPage);
            element.AppendChild(MoreInfo);
            element.AppendChild(Labs);
            xmldoc.SelectSingleNode("Settings").AppendChild(element);

            xmldoc.Save(@".\Data\Setting.xml");
        }

        public void DelXmlData(int itemCount)
        {
            xmldoc.SelectSingleNode("Settings").RemoveChild(xmldoc.SelectSingleNode("Settings").ChildNodes.Item(itemCount));
            xmldoc.Save(@".\Data\Setting.xml");
        }

        public void LoadData(string urlST, string[] reviewItemST, string[] NextPageST, string[] MoreInfoST, List<TextBlock> LableST)
        {
            Settings sts = new Settings();
            sts.UrlST = urlST;
            sts.ReviewItemST = reviewItemST;
            sts.NextPageST = NextPageST;
            sts.MoreInfoST = MoreInfoST;
            sts.LableST = LableST;
        }

        public void CloseXml()
        {

        }
    }

    public class Settings
    {
        private string urlST;
        private string[] reviewItemST = new string[] { };
        private string[] nextPageST = new string[] { };
        private string[] moreInfoST = new string[] { };
        private List<TextBlock> lableST = new List<TextBlock>();

        public string UrlST { get => urlST; set => urlST = value; }
        public string[] ReviewItemST { get => reviewItemST; set => reviewItemST = value; }
        public string[] NextPageST { get => nextPageST; set => nextPageST = value; }
        public string[] MoreInfoST { get => moreInfoST; set => moreInfoST = value; }
        public List<TextBlock> LableST { get => lableST; set => lableST = value; }
    }
}
