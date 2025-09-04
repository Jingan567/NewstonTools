using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewstonTools.DataBase.sqlite
{
    /// <summary>
    /// 使用SQLite数据库的工具类
    /// 通过最基础的SqliteConnection, SQLiteCommand, SQLiteDataAdapter等实现
    /// </summary>
    public class SQLiteTool1 : IDisposable
    {
        private readonly string _connectionString;
        private SQLiteConnection _connection;
        private SQLiteTransaction _transaction;
        private bool _disposed;

        /// <summary>
        /// 初始化SQLiteTool1
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        public SQLiteTool1(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// 打开数据库连接（自动管理连接生命周期）
        /// </summary>
        private void EnsureConnectionOpen()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        #region 基础操作方法

        /// <summary>
        /// 执行非查询SQL（INSERT/UPDATE/DELETE）
        /// </summary>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sql, parameters))
            {
                return cmd.ExecuteNonQuery();
            }
          
        }

        /// <summary>
        /// 执行非查询SQL（异步版本）
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string sql, params SQLiteParameter[] parameters)
        {
            EnsureConnectionOpen();
            using (var cmd = CreateCommand(sql, parameters))
            {
                return await cmd.ExecuteNonQueryAsync();
            }     
        }

        /// <summary>
        /// 执行查询并返回DataReader
        /// </summary>
        public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parameters)
        {
            EnsureConnectionOpen();
            var cmd = CreateCommand(sql, parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行查询并返回对象列表
        /// </summary>
        public List<T> Query<T>(string sql, Func<SQLiteDataReader, T> map, params SQLiteParameter[] parameters)
        {
            using (var reader = ExecuteReader(sql, parameters))
            {
                var list = new List<T>();
                while (reader.Read())
                {
                    list.Add(map(reader));
                }
                return list;
            }
        }

        /// <summary>
        /// 执行查询并返回单个对象
        /// </summary>
        public T QuerySingle<T>(string sql, Func<SQLiteDataReader, T> map, params SQLiteParameter[] parameters)
        {
            using (var reader = ExecuteReader(sql, parameters))
            {
                return reader.Read() ? map(reader) : default;
            }     
        }

        #endregion

        #region 事务处理

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            EnsureConnectionOpen();
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        #endregion

        #region 辅助方法

        private SQLiteCommand CreateCommand(string sql, IEnumerable<SQLiteParameter> parameters)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.Transaction = _transaction;

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            return cmd;
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }

        ~SQLiteTool1() => Dispose(false);

        #endregion
    }
}

