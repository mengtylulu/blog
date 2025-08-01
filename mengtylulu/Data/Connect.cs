using Npgsql;

namespace mengtylulu.ADO.NET
{
    public class Connect
    {
        public Connect()
        {
                
        }

        /// <summary>
        /// 数据库测试
        /// </summary>
        public async void testConnect()
        {
            //connect to pgsql
            var connectionString = "Host=localhost;Username=postgres;password=admin;Database=blog";
            await using var dataSource = NpgsqlDataSource.Create(connectionString);

            //Insert some data
            await using (var cmd = dataSource.CreateCommand($"INSERT INTO directory VALUES ('{Guid.NewGuid()}','我的标题','简介','{DateTime.Now}','{DateTime.Now}')"))
            {
                cmd.Parameters.AddWithValue("Hello world");
                await cmd.ExecuteNonQueryAsync();
            }

            //Retrieve all rows
            await using (var cmd = dataSource.CreateCommand("SELECT * FROM directory"))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var test = reader.GetGuid(0);
                    //Console.WriteLine(reader.GetString(0));
                }
            }
        }
    }
}
