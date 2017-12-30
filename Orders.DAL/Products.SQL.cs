using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class Products
    {
        private const string PRODUCTS_SQL =
@"SELECT ProductID, ProductName FROM Products";

        private const string UPDATE_PRODUCT_SQL =
@"SELECT
    ProductID
    ,ProductName
    ,SupplierID
    ,CategoryID
    ,QuantityPerUnit
    ,UnitPrice
    ,UnitsInStock
    ,UnitsOnOrder
    ,ReorderLevel
FROM
    Products
";

        private const string SELECT_PRODUCT_SQL =
            UPDATE_PRODUCT_SQL + 
@"WHERE
    ProductID = @ProductID";

        private const string CATEGORIES_SQL =
@"SELECT CategoryID, CategoryName FROM Categories";

        private const string SUPPLIERS_SQL =
@"SELECT SupplierID, CompanyName FROM Suppliers";

    }
}
