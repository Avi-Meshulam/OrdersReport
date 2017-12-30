using System;
using System.Collections;
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
using System.Windows.Shapes;

namespace Orders
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private DAL.Products _products;
        private DataTable _productsData;
        private DataRow _productData;

        public ProductWindow()
        {
            InitializeComponent();
            string _connectionString = ConfigurationManager.ConnectionStrings["AppDB"].ConnectionString;
            _products = new DAL.Products(_connectionString);
        }

        private void grdMain_Loaded(object sender, RoutedEventArgs e)
        {
            InitCombos();
            ResetControls();
        }

        private void InitCombos()
        {
            try
            {
                _productsData = _products.GetProducts();
                cboProducts.ItemsSource = _productsData.DefaultView;
                cboSuppliers.ItemsSource = _products.GetSuppliers().DefaultView;
                cboCategories.ItemsSource = _products.GetCategories().DefaultView;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cboProducts_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();
            if (key.Length > 1)
            {
                switch (key)
                {
                    case "Up":
                    case "Down":
                        cboProducts.IsDropDownOpen = true;
                        return;
                    case "Back":
                    case "Delete":
                        break;
                    default:
                        return;
                }
            }

            ResetControls();

            if (cboProducts.Text == string.Empty)
            {
                cboProducts.ItemsSource = _productsData.DefaultView;
                cboProducts.SelectedIndex = -1;
                cboProducts.IsDropDownOpen = false;
                return;
            }

            var productsData =
                _productsData.AsEnumerable()
                .Where(row =>
                    row["ProductName"].ToString().ToUpper()
                    .Contains(cboProducts.Text.ToUpper()));

            string cboProducts_Text = cboProducts.Text;
            int selectionStart = cboProducts.SelectionStart;

            if (productsData.Any())
                cboProducts.ItemsSource = productsData.CopyToDataTable().DefaultView;
            else
                cboProducts.ItemsSource = null;

            if (cboProducts.Text == string.Empty)
            {
                cboProducts.Text = cboProducts_Text;
            }

            cboProducts.IsDropDownOpen = true;
            cboProducts.SetCaret(selectionStart);
            cboProducts.SelectedIndex = -1;
        }

        private void cboProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboProducts.SelectedIndex == -1)
                return;

            int productID = (int)cboProducts.SelectedValue;

            try
            {
                DataTable productDataTable = _products.GetProductById(productID);
                _productData = productDataTable.Rows[0];
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            SetWindowData();
        }

        private void SetWindowData()
        {
            if (_productData == null)
                return;

            EnableAllControls();

            txtProductID.Text = _productData["ProductID"].ToString();
            txtProductName.Text = _productData["ProductName"].ToString();
            cboSuppliers.SelectedValue = _productData["SupplierID"];
            cboCategories.SelectedValue = _productData["CategoryID"];
            txtQuantityPerUnit.Text = _productData["QuantityPerUnit"].ToString();
            txtUnitPrice.Text = $"{_productData["UnitPrice"]:N2}";
            txtUnitsInStock.Text = _productData["UnitsInStock"].ToString();
            txtUnitsOnOrder.Text = _productData["UnitsOnOrder"].ToString();
            txtReorderLevel.Text = _productData["ReorderLevel"].ToString();
        }

        private void ResetControls()
        {
            _productData = null;

            foreach (var child in Utils.FindLogicalChildren(this))
            {
                if (child is TextBox)
                {
                    (child as TextBox).Text = string.Empty;
                    (child as TextBox).IsEnabled = false;
                }
                else if (child is ComboBox && (child as ComboBox).Name != "cboProducts")
                {
                    (child as ComboBox).SelectedIndex = -1;
                    (child as ComboBox).IsEnabled = false;
                }
                else if (child is Button)
                    (child as Button).IsEnabled = false;
            }
        }

        private void EnableAllControls()
        {
            foreach (dynamic child in Utils.FindLogicalChildren(this))
            {
                if (child.Name == "btnSave")
                {
                    child.IsEnabled = false;
                    continue;
                }

                try
                {
                    child.IsEnabled = true;
                }
                catch (Exception)
                { }
            }
        }
        private void cboSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboSuppliers.SelectedValue == null)
                return;

            _productData["SupplierID"] = cboSuppliers.SelectedValue;
            btnSave.IsEnabled = true;
        }

        private void cboCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboCategories.SelectedValue == null)
                return;

            _productData["CategoryID"] = cboCategories.SelectedValue;
            btnSave.IsEnabled = true;
        }

        private void txtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            string fieldName = txtBox.Name.Substring(3);

            try
            {
                if(_productData[fieldName].ToString() != txtBox.Text)
                    _productData[fieldName] = txtBox.Text;
            }
            catch (Exception ex)
            {
                txtBox.Text = fieldName == "UnitPrice" ? 
                    $"{_productData["UnitPrice"]:N2}" : _productData[fieldName].ToString();
                HandleException(ex);
            }
        }

        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            string fieldName = txtBox.Name.Substring(3);

            if (_productData?[fieldName].ToString() != txtBox.Text)
                btnSave.IsEnabled = true;
            else
                btnSave.IsEnabled = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_productData == null)
                return;

            try
            {
                int affected = _products.UpdateProduct(_productData);
                if (affected > 0)
                {
                    MessageBox.Show("Product has been saved successfully", "Confirmation");
                    btnSave.IsEnabled = false;
                }
                else
                    MessageBox.Show("No changes have been tracked", "Information");
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void HandleException(Exception ex)
        {
            MessageBox.Show(ex.GetInnerExceptions(), "Error");
        }
    }
}
