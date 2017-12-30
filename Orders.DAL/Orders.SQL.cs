using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class Orders
    {
        private const string ANNUAL_REPORT_SQL = 
@"SELECT
    YEAR(OrderDate) AS 'Year'
	, COUNT(DISTINCT Orders.OrderID) AS 'NumberOfOrders'
	, FORMAT(ROUND(SUM([Order Details].Quantity * [Order Details].UnitPrice 
        * (1 - Discount)), 1), 'N2') AS 'TotalCost'
FROM
    Orders
        INNER JOIN[Order Details]
            ON Orders.OrderID = [Order Details].OrderID
GROUP BY
    YEAR(OrderDate)
ORDER BY
    [Year]";

        private const string ORDERS_SQL = 
@"SELECT
	Orders.OrderID
	, OrderDate
	, ContactName AS 'CustomerName'
	, FirstName + ' ' + LastName AS 'EmployeeName'
	, CAST(ROUND(SUM([Order Details].Quantity * [Order Details].UnitPrice 
        * (1 - Discount)), 1) AS NUMERIC(18, 2)) AS 'TotalPrice'
FROM
	Orders
		INNER JOIN [Order Details]
			ON [Order Details].OrderID = Orders.OrderID
		INNER JOIN Customers
			ON Customers.CustomerID = Orders.CustomerID
		INNER JOIN Employees
			ON Employees.EmployeeID = Orders.EmployeeID
WHERE 
	YEAR(OrderDate) = @Year
GROUP BY
	Orders.OrderID
	, OrderDate
	, ContactName
	, FirstName + ' ' + LastName";

        private const string ORDER_ITEMS_SQL = 
@"SELECT
	[Order Details].ProductID
	, ProductName
	, CAST([Order Details].UnitPrice AS NUMERIC(18, 2)) AS 'UnitPrice'
	, Quantity
	, CAST(ROUND(SUM([Order Details].Quantity * [Order Details].UnitPrice 
        * (1 - Discount)), 1) AS NUMERIC(18, 2)) AS 'LineTotal'
FROM
	[Order Details]
		INNER JOIN Products
			ON Products.ProductID = [Order Details].ProductID
WHERE
	OrderID = @OrderID
GROUP BY
	[Order Details].ProductID
	, ProductName
	, [Order Details].UnitPrice
	, Quantity";
    }
}
