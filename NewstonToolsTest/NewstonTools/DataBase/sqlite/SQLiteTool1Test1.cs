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

    [TestMethod]
    public void InsertTestMethod2()
    {
        //ʹ��IP�������ݿ�
        sql
    }
}
