using NewstonTools.DataBase.sqlite;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;

namespace NewstonToolsTest.NewstonTools.DataBase.sqlite;

[TestClass]
public class SQLiteTool1Test1
{
    [TestMethod]
    public void TestMethod1()
    {
        // 初始化工具类
        var db = new SQLiteTool1("Data Source=SQLiteTool1Test1.db;Version=3;");

        // 创建用户表
        db.ExecuteNonQuery(@"
    CREATE TABLE IF NOT EXISTS Users (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL,
        Age INT NOT NULL
    );");

        //// 插入数据
        //db.ExecuteNonQuery("INSERT INTO Users (Name, Age) VALUES (@name, @age)",
        //    new SQLiteParameter("@name", "张三"),
        //    new SQLiteParameter("@age", 25));

        //// 查询数据（返回对象列表）
        //var users = db.Query("SELECT * FROM Users", reader =>
        //    new
        //    {
        //        Id = reader.GetInt32(0),
        //        Name = reader.GetString(1),
        //        Age = reader.GetInt32(2)
        //    });

        // 事务操作示例
        db.BeginTransaction();
        try
        {
            db.ExecuteNonQuery("UPDATE Users SET Age = 51 WHERE Name = '张三'");//用调试的方式，在事务内部跳步，事务就失效了。
            //throw new Exception("中断事务");
            db.ExecuteNonQuery("INSERT INTO Users (Name, Age) VALUES ('李四', 30)");
            db.CommitTransaction();
        }
        catch
        {
            db.RollbackTransaction();
            throw;//回滚完接着将错误抛出
        }
    }

    [TestMethod]
    public void InsertTestMethod2()
    {
        //使用IP连接数据库
        sql
    }
}
