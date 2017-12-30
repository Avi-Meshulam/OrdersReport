using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace Orders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DAL.Orders _orders;

        public MainWindow()
        {
            InitializeComponent();
            string _connectionString = ConfigurationManager.ConnectionStrings["AppDB"].ConnectionString;
            _orders = new DAL.Orders(_connectionString);
        }

        private void grdAnnualReport_Loaded(object sender, RoutedEventArgs e)
        {
            grdAnnualReport.DataContext = _orders.GetAnnualReport().DefaultView;
            grdAnnualReport.SelectedIndex = 0;
        }

        private void grdAnnualReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string data = GetSelectedRowData((DataGrid)sender, "Year");

            int year;
            int.TryParse(data, out year);

            grdOrders.DataContext = _orders.GetOrdersByYear(year).DefaultView;
            grdOrders.SelectedIndex = 0;
        }

        private void grdOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = (DataGrid)sender;

            if (grid.SelectedItem == null)
                return;

            string data = GetSelectedRowData(grid, "OrderID");

            int orderID;
            int.TryParse(data, out orderID);

            grdOrderItems.DataContext = _orders.GetOrderItems(orderID).DefaultView;
            grdOrderItems.SelectedIndex = 0;
        }

        private static string GetSelectedRowData(DataGrid grid, string colName)
        {
            var row = (DataRowView)grid.SelectedItem;
            return row[colName].ToString();
        }

        private void btnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            var productWindow = new ProductWindow();
            productWindow.ShowDialog();
        }
    }
}
