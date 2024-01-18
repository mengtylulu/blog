using Npgsql;
using System.Data;

namespace mengtylulu.DB
{
    public class PgConn
    {
        /// <summary>
        /// 链接数据库
        /// </summary>
        public void Connect()
        {
            string connString = "Host=localhost;Port=5432;Database=blog_mty;Username=admin;Password=admin";
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string sql = "select * from \"BG_user\" ";
                    NpgsqlDataAdapter sda = new NpgsqlDataAdapter(sql, conn);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    sda.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception)
            {
                throw new Exception("查询错误");
            }
        }
    }
}
