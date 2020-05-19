﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {

        private DataTable d1 = new DataTable();
        private Test.DriverItemView _myView;
        private Test.DriverItemController _myCtrler;

        public UserControl1()
        {
            InitializeComponent();
            _myView = new Test.DriverItemView();
            _myCtrler = new Test.DriverItemController();
            //Messenger.Default.Send(ListInfo, Test.MessageToken.SetDataGrid);
            //Test2();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBlock t1 = new TextBlock();
            t1.Text = LableContent.Text;
            foreach (var item in MyLableTypes.Children)
            {
                switch ((item as RadioButton).IsChecked)
                {
                    case true:
                        t1.Tag = (item as RadioButton).Tag;
                        MyLable.Items.Add(t1);
                        break;
                    case false:
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Start(object sender, RoutedEventArgs e)
        {
            if (SetUrl.Text != "")
            {
                //防止空数据
                if (reviewitem.IsChecked == true && (reviewitemLocate.Text == string.Empty || ((reviewitem.Content as DockPanel).Children[3] as ComboBox).SelectedItem == null))
                {
                    MessageBox.Show("非法数值");
                }
                else if (NextPage.IsChecked == true && (NextPageLocate.Text == string.Empty || ((NextPage.Content as DockPanel).Children[3] as ComboBox).SelectedItem == null))
                {
                    MessageBox.Show("非法数值");
                }
                else if (MoreInfo.IsChecked == true && (MoreInfoLocate.Text == string.Empty || ((MoreInfo.Content as DockPanel).Children[3] as ComboBox).SelectedItem == null))
                {
                    MessageBox.Show("非法数值");
                }
                else
                {
                    if (reviewitem.IsChecked == true)
                    {
                        string asd = (((reviewitem.Content as DockPanel).Children[3] as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString();
                        _myView.Reviewiteminfo = new string[] { asd, reviewitemLocate.Text };
                    }
                    if (NextPage.IsChecked == true)
                    {
                        _myView.NextPageLocate = new string[] { (((NextPage.Content as DockPanel).Children[3] as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString(), NextPageLocate.Text };
                    }
                    if (MoreInfo.IsChecked == true)
                    {
                        _myView.MoreInfoLocate = new string[] { (((MoreInfo.Content as DockPanel).Children[3] as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString(), MoreInfoLocate.Text };
                    }
                    _myView.GetInfos(SetUrl.Text);
                    List<string[]> vs = _myView.Infos;
                    int dataColumns = vs.Count;
                    for (int i = 0; i < dataColumns; i++)
                    {
                        DataRow row1 = d1.NewRow();
                        //循环添加行数据
                        for (int j = 0; j < ListInfo.Columns.Count - 1; j++)
                        {
                            row1[d1.Columns[j + 1].ColumnName] = vs[i][j];
                        }
                        d1.Rows.Add(row1);
                    }
                }
            }
            else
            {
                MessageBox.Show("Url链接不能为空");
            }
        }

        private void Button_Click_Del(object sender, RoutedEventArgs e)
        {
            MyLable.Items.Remove(MyLable.SelectedItem);
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            MyLable.Items.Clear();
        }

        private void Button_Click_Create(object sender, RoutedEventArgs e)
        {
            if (MyLable.Items.Count != 0)
            {
                ListInfo.Columns.Clear();
                d1 = null;
                d1 = new DataTable();
                DataColumn _columnHead = new DataColumn("序号", typeof(int));
                _columnHead.AutoIncrement = true;
                _columnHead.AutoIncrementSeed = 1;
                d1.Columns.Add(_columnHead);
                for (int i = 0; i < MyLable.Items.Count; i++)
                {
                    DataColumn _columns = new DataColumn("字段" + i, typeof(string))
                    {
                        ReadOnly = false
                    };
                    d1.Columns.Add(_columns);
                }
                ListInfo.ItemsSource = d1.DefaultView;

                _myView.GetFields(MyLable.Items);
            }
            else
            {
                MessageBox.Show("可创建表为空");
            }

        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if (ListInfo.Items.Count != 0)
            {
                //防止控数据

                Microsoft.Win32.SaveFileDialog op = new Microsoft.Win32.SaveFileDialog()
                {

                    Title = "保存为:",
                    RestoreDirectory = true,
                    Filter = "Excel 97 - 2003 工作薄(*.xls)|*.xls|Excel 工作薄(*.xlsx)|*.xlsx|保存本页文本(*.txt)|*.txt|所有文件(*.*)|*.*"
                };
                //op.Multiselect = false;
                op.Title = "请选择文件夹";
                if (op.ShowDialog() == true)
                {
                    if (op.FilterIndex == 3)
                    {
                        _myCtrler.FileToString(_myView.ToString(1), op);
                    }
                    else
                    {
                        _myCtrler.TableToExcel(d1, op.FileName);
                    }
                }
                else
                {
                    MessageBox.Show("没有选择文件");
                }
            }
            else
            {
                MessageBox.Show("导出项目为空");
            }
        }

        private void reviewitem_Checked(object sender, RoutedEventArgs e)
        {
            //reviewitemLocate.SetBinding(TextBox.TextProperty, new Binding() { Source= new ReviewItem(), Path = new PropertyPath("ReviewContent"), Mode = BindingMode.TwoWay });
            if (((reviewitem.Content as DockPanel).Children[3] as ComboBox).SelectedItem != null)
            {
                string asd = (((reviewitem.Content as DockPanel).Children[3] as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString();
                _myView.Reviewiteminfo = new string[] { asd, reviewitemLocate.Text };
            }
        }

        private void reviewitem_Unchecked(object sender, RoutedEventArgs e)
        {
            _myView.Reviewiteminfo = new string[] { string.Empty, string.Empty };
        }

        private void NextPage_Checked(object sender, RoutedEventArgs e)
        {
            if (((NextPage.Content as DockPanel).Children[3] as ComboBox).SelectedItem != null)
            {
                _myView.NextPageLocate = new string[] { (((NextPage.Content as DockPanel).Children[3] as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString(), NextPageLocate.Text };
            }
        }

        private void NextPage_Unchecked(object sender, RoutedEventArgs e)
        {
            _myView.NextPageLocate = new string[] { string.Empty, string.Empty };
        }

        private void MoreInfo_Checked(object sender, RoutedEventArgs e)
        {
            if (((MoreInfo.Content as DockPanel).Children[3] as ComboBox).SelectedItem != null)
            {
                _myView.MoreInfoLocate = new string[] { (((MoreInfo.Content as DockPanel).Children[3] as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString(), MoreInfoLocate.Text };
            }
        }

        private void MoreInfo_Unchecked(object sender, RoutedEventArgs e)
        {
            _myView.MoreInfoLocate = new string[] { string.Empty, string.Empty };
        }
    }

    //public class ReviewItem
    //{
    //    public string ReviewContent { get; set; }
    //    public string NextPageContent { get; set; }
    //    public string MoreContent { get; set; }
    //}
}