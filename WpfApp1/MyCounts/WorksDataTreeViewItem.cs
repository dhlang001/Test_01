using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1.MyCounts
{
    class WorksDataTreeViewItem : TreeViewItem
    {
        DataTable data = new DataTable();

        public WorksDataTreeViewItem(DataTable data)
        {
            this.data = data;
            this.PreviewMouseDoubleClick += TreeViewItem_MouseDoubleClick;
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Page a = (sender as UserControl).FindName("ShowPosition") as Page;
            a.Content = null;
            a.Content = new WorksDataGrid(data);
        }
    }

    class WorksDataGrid : DataGrid
    {
        private DataTable data;

        public WorksDataGrid(DataTable data)
        {
            this.MaxColumnWidth = 240;
            this.data = data;
            this.ItemsSource = this.data.DefaultView;
        }
    }
}
