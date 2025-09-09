using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NewstonTools.DataBase.sqlite;
using NewstonTools.FunnyTool;
using System.Configuration;
using System.Data.SQLite;
using System.Diagnostics;
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
    /// <summary>
    /// 前机保存数据到本地，后机去查询前机去查询数据。
    /// </summary>
    [TestMethod]
    public void InsertTestMethod2()
    {
        var db = new SQLiteTool1("Data Source=D:\\Test\\Test.Dbfile\\SQLiteTool1Test1.db;Version=3;");
        //创建第一台
        db.ExecuteNonQuery(
            @"CREATE TABLE IF NOT EXISTS ""FirstMachine"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Message1""	TEXT,
	            ""Message2""	TEXT,
	            ""Message3""	TEXT,
	            ""Message4""	TEXT,
	            ""Message5""	TEXT,
	            ""Message6""	TEXT,
	            ""Message7""	TEXT,
	            ""Message8""	TEXT,
                ""Message9""	TEXT,
                ""Message10""	TEXT,
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );");

        try
        {
            string InsertString = "INSERT INTO FirstMachine (Message1, Message2, Message3, Message4, Message5, Message6, Message7, Message8, Message9, Message10) VALUES (@message1, @message2, @message3, @message4, @message5, @message6, @message7, @message8, @message9, @message10)";
            SQLiteParameter[] parameter = new SQLiteParameter[10];
            for (int i = 0; i < 10; i++)
            {
                if (i < 3)
                {
                    parameter[i] = new SQLiteParameter($"@message{i + 1}", ChineseTextGenerator.Generate());
                }
                else
                {
                    parameter[i] = new SQLiteParameter($"@message{i + 1}", null);
                }

            }
            db.BeginTransaction();
            db.ExecuteNonQuery(InsertString, parameter);
            db.CommitTransaction();
        }
        catch (Exception)
        {
            db.RollbackTransaction();
            throw;
        }

        void InsertMulString(string[] strings)
        {
            string InsertString = "INSERT INTO FirstMachine (Message1, Message2, Message3, Message4, Message5, Message6, Message7, Message8, Message9, Message10) VALUES (@message1, @message2, @message3, @message4, @message5, @message6, @message7, @message8, @message9, @message10)";
            SQLiteParameter parameter = new SQLiteParameter("@message1", strings[0]);
        }
    }
    /// <summary>
    /// 插入测试,23万条记录，没崩，查询速度也是正常的。
    /// </summary>
    [TestMethod]
    public void InsertTestMethod3()
    {
        var db = new SQLiteTool1("Data Source=SQLiteTool1Test1.db;Version=3;");
        // 创建计时表
        db.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS ""ElapsedMilliseconds"" 
                           (""Id""	INTEGER NOT NULL UNIQUE,
                            ""Time""	INTEGER,
	                        PRIMARY KEY(""Id"" AUTOINCREMENT));");
        int times = 0;
        Stopwatch sw_InsertTime = Stopwatch.StartNew();
        Stopwatch sw_InsertAllTime = Stopwatch.StartNew();
        while (times < 1000)
        {
            string name = ChineseNameGenerator.Generate();
            sw_InsertTime.Restart();
            db.ExecuteNonQuery("INSERT INTO Users (Name, Age) VALUES (@name, @age)",
                new SQLiteParameter("@name", name),
                new SQLiteParameter("@age", 25));
            long temptime = sw_InsertTime.ElapsedMilliseconds;

            db.ExecuteNonQuery("INSERT INTO ElapsedMilliseconds (Time) VALUES (@time)",
                new SQLiteParameter("@time", temptime));
            //Thread.Sleep(0);
            times++;
        }
        Debug.WriteLine($"总耗时：{sw_InsertAllTime.ElapsedMilliseconds}ms");
    }

    /// <summary>
    /// 开启多个线程使用类库去进行插入操作。
    /// </summary>
    [TestMethod]
    public void InsertTestMethod4()
    {
        try
        {
            var db = new SQLiteTool1("Data Source=D:\\Test\\Test.Dbfile\\SQLiteTool1Test1.db;Version=3;");
            //创建第一台
            db.ExecuteNonQuery(
                @"CREATE TABLE IF NOT EXISTS ""MutiThread"" (
	            ""Id""	INTEGER NOT NULL,
	            ""Message1""	TEXT,
	            PRIMARY KEY(""Id"" AUTOINCREMENT)
            );");

            Thread t1 = new Thread(t =>
            {
                string InsertStr = $"INSERT INTO MutiThread(Message1) VALUES(@message1)";
                SQLiteParameter parameter = new SQLiteParameter("@message1", "这是t1写入的");
                while (true)
                {
                    db.ExecuteNonQuery(InsertStr, parameter);
                    Thread.Sleep(10);
                }
            });

            Thread t2 = new Thread(t =>
            {
                string InsertStr = $"INSERT INTO MutiThread(Message1) VALUES(@message1)";
                SQLiteParameter parameter = new SQLiteParameter("@message1", "这是t2写入的");
                while (true)
                {
                    db.ExecuteNonQuery(InsertStr, parameter);
                    Thread.Sleep(10);
                }
            });

            t1.Start();
            t2.Start();
            t2.Join();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
