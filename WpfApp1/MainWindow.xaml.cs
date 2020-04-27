using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Test.MyInfos_Untreated> strs = new List<Test.MyInfos_Untreated>();
        private DataTable d1 = new DataTable();
        private Test.DriverItemView _myView;
        private Test.DriverItemController _myCtrler;

        public MainWindow()
        {
            InitializeComponent();

            _myView = new Test.DriverItemView();
            _myCtrler = new Test.DriverItemController();
            //Messenger.Default.Send(ListInfo, Test.MessageToken.SetDataGrid);
            //Test2();

        }

        private void Test1()
        {
            Test.Class1 c1 = new Test.Class1("https://movie.douban.com/tag/");
            List<Test.ListViewShowInfo> asd = new List<Test.ListViewShowInfo>();
            asd = c1.Infos;
            for (int i = 0; i < asd.Count; i++)
            {
                ListInfo.Items.Add(asd[i]);
            }

            TreeView LB = new TreeView();
            TreeViewItem LBI = new TreeViewItem();
            LBI.Header = "筛选";
            for (int i = 0; i < ListInfo.Columns.Count; i++)
            {
                CheckBox ck = new CheckBox();
                ck.Content = "第 " + i + " 列";
                ck.IsChecked = true;
                ck.Tag = i;
                ck.Checked += CheckBox_Checked;
                ck.Unchecked += CheckBox_UnChecked;
                LBI.Items.Add(ck);
            }
            LB.Items.Add(8);
            (ListInfo.Parent as Grid).Children.Add(LB);
            LB.SetValue(Grid.ColumnProperty, 0);
        }

        private void Test2()
        {
            //this.test1.Text= new Test.Class2("https://movie.douban.com/subject/34805219/").ToString(1);//https://www.jianshu.com/p/ea0022394140
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ListInfo.Columns[Convert.ToInt32((sender as CheckBox).Tag.ToString())].Visibility = Visibility.Visible;
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            ListInfo.Columns[Convert.ToInt32((sender as CheckBox).Tag.ToString())].Visibility = Visibility.Hidden;
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
                        break;
                    case false:
                        break;
                    default:
                        break;
                }
            }
            MyLable.Items.Add(t1);
        }

        private void Btn_Start(object sender, RoutedEventArgs e)
        {
            if (SetUrl.Text!="")
            {
                //防止空数据
                _myView.GetInfos(SetUrl.Text);
                List<string[]> vs = _myView.Infos;
                int dataColumns = vs.Count;
                for (int i = 0; i < dataColumns; i++)
                {
                    DataRow row1 = d1.NewRow();
                    //循环添加行数据
                    for (int j = 0; j < ListInfo.Columns.Count - 1; j++)
                    {
                        row1[d1.Columns[j + 1].ColumnName] = vs[i][j];//d1.Columns[j].ColumnName + " " + "strtest_" + j;
                    }
                    d1.Rows.Add(row1);
                }
            }
            else
            {
                MessageBox.Show("Url链接不能为空");
            }
            //MessageBox.Show(((ListInfo.Columns[2] as DataGridTextColumn).Binding as Binding).Path.Path);//获取列名
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

            /*
            ListInfo.Columns[0].Header = null;
            DockPanel dock = new DockPanel();
            TextBlock t1 = new TextBlock();
            Button b1 = new Button();
            t1.Text = "序号";
            t1.VerticalAlignment = VerticalAlignment.Center;
            b1.Content = "删除";
            dock.Children.Add(t1);
            dock.Children.Add(b1);
            ListInfo.Columns[0].Header = dock;
            for (int i = 1; i < ListInfo.Columns.Count; i++)
            {
                DockPanel docks = new DockPanel();
                TextBlock ts = new TextBlock();
                Button bs = new Button();
                ts = new TextBlock();
                ts.Text = "字段" + i;
                ts.VerticalAlignment = VerticalAlignment.Center;
                bs.Content = "删除";
                docks.Children.Add(ts);
                docks.Children.Add(bs);
                ListInfo.Columns[i].Header = docks;
            }
            */


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

            _myView.GetFields(MyLable.Items);
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            if (ListInfo.Items.Count!=0)
            {
                //防止控数据

                Microsoft.Win32.SaveFileDialog op = new Microsoft.Win32.SaveFileDialog()
                {

                    Title = "保存为:",
                    RestoreDirectory = true,
                    Filter = "Excel 97 - 2003 工作薄(*.xls)|*.xls|Excel 工作薄(*.*)|*.xlsx|保存本页文本(*.txt)|*.txt|所有文件(*.*)|*.*"
                };
                //op.Multiselect = false;
                op.Title = "请选择文件夹";
                if (op.ShowDialog()==true)
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
       
    }
}
