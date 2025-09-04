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
        // ��ʼ��������
        var db = new SQLiteTool1("Data Source=SQLiteTool1Test1.db;Version=3;");

        // �����û���
        db.ExecuteNonQuery(@"
    CREATE TABLE IF NOT EXISTS Users (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Name TEXT NOT NULL,
        Age INT NOT NULL
    );");

        //// ��������
        //db.ExecuteNonQuery("INSERT INTO Users (Name, Age) VALUES (@name, @age)",
        //    new SQLiteParameter("@name", "����"),
        //    new SQLiteParameter("@age", 25));

        //// ��ѯ���ݣ����ض����б�
        //var users = db.Query("SELECT * FROM Users", reader =>
        //    new
        //    {
        //        Id = reader.GetInt32(0),
        //        Name = reader.GetString(1),
        //        Age = reader.GetInt32(2)
        //    });

        // �������ʾ��
        db.BeginTransaction();
        try
        {
            db.ExecuteNonQuery("UPDATE Users SET Age = 51 WHERE Name = '����'");//�õ��Եķ�ʽ���������ڲ������������ʧЧ�ˡ�
            //throw new Exception("�ж�����");
            db.ExecuteNonQuery("INSERT INTO Users (Name, Age) VALUES ('����', 30)");
            db.CommitTransaction();
        }
        catch
        {
            db.RollbackTransaction();
            throw;//�ع�����Ž������׳�
        }
    }
    /// <summary>
    /// ǰ���������ݵ����أ����ȥ��ѯǰ��ȥ��ѯ���ݡ�
    /// </summary>
    [TestMethod]
    public void InsertTestMethod2()
    {
        var db = new SQLiteTool1("Data Source=SQLiteTool1Test1.db;Version=3;");
        //������һ̨
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
    /// �������,23������¼��û������ѯ�ٶ�Ҳ�������ġ�
    /// </summary>
    [TestMethod]
    public void InsertTestMethod3()
    {
        var db = new SQLiteTool1("Data Source=SQLiteTool1Test1.db;Version=3;");
        // ������ʱ��
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
        Debug.WriteLine($"�ܺ�ʱ��{sw_InsertAllTime.ElapsedMilliseconds}ms");
    }
}
