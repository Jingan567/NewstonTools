using NewstonTools.FileHelpers.Implement;
using System.Data;

namespace NewstonToolsTest;

[TestClass]
public class EPPlusSaveXlsxHelperTest
{
    EPPlusSaveXlsxHelper helper = new EPPlusSaveXlsxHelper("C:\\Temp\\Sample.xlsx");
    [TestMethod]
    public void TestWriteXlsx()
    {
        DataSet set = CreateSampleDataSet();
        helper.WriteXlsx(set);
    }

    public DataSet CreateSampleDataSet()
    {
        // 创建 DataSet
        DataSet dataSet = new DataSet("Northwind");

        // 创建 Customers 表
        DataTable customersTable = new DataTable("Customers");
        customersTable.Columns.Add("CustomerID", typeof(string)).Unique = true;
        customersTable.Columns.Add("CompanyName", typeof(string));
        customersTable.Columns.Add("ContactName", typeof(string));
        customersTable.Columns.Add("Country", typeof(string));
        customersTable.PrimaryKey = new DataColumn[] { customersTable.Columns["CustomerID"] };

        // 添加数据到 Customers 表
        customersTable.Rows.Add("ALFKI", "Alfreds Futterkiste", "Maria Anders", "Germany");
        customersTable.Rows.Add("ANATR", "Ana Trujillo Emparedados y helados", "Ana Trujillo", "Mexico");
        customersTable.Rows.Add("ANTON", "Antonio Moreno Taquería", "Antonio Moreno", "Mexico");

        // 创建 Orders 表
        DataTable ordersTable = new DataTable("Orders");
        ordersTable.Columns.Add("OrderID", typeof(int)).Unique = true;
        ordersTable.Columns.Add("CustomerID", typeof(string));
        ordersTable.Columns.Add("OrderDate", typeof(DateTime));
        ordersTable.Columns.Add("TotalAmount", typeof(decimal));
        ordersTable.PrimaryKey =  new DataColumn[] { ordersTable.Columns["OrderID"] };

        // 添加数据到 Orders 表
        ordersTable.Rows.Add(10643, "ALFKI", new DateTime(2023, 1, 1), 32.38m);
        ordersTable.Rows.Add(10692, "ALFKI", new DateTime(2023, 1, 16), 11.61m);
        ordersTable.Rows.Add(10308, "ANATR", new DateTime(2023, 3, 16), 87.8m);

        // 添加表到 DataSet
        dataSet.Tables.Add(customersTable);
        dataSet.Tables.Add(ordersTable);

        // 创建关系
        DataRelation relation = new DataRelation(
            "FK_Customers_Orders",
            customersTable.Columns["CustomerID"],
            ordersTable.Columns["CustomerID"]
        );
        dataSet.Relations.Add(relation);

        return dataSet;
    }

}
