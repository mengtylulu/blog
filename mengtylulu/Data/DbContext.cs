using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace mengtylulu.Data
{
    public class DbContext : ControllerBase
    {
        //数据库连接字符串
        private readonly string _connectionString;
        public DbContext(string connectionString)
        {
            this._connectionString = connectionString;
        }


        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        public async void excuteSql(string sql)
        {
            //DirectoryRepository directoryRepository = new DirectoryRepository();
            await using var dataSource = NpgsqlDataSource.Create(this._connectionString);

            await using (var cmd = dataSource.CreateCommand($"{sql}"))
            {
            }
        }
    }
}
