using mengtylulu.Model.DB.Constans;
using mengtylulu.Model.DB.Models;
using Npgsql;

namespace mengtylulu.Model.DB.BlogApi
{
    public class BlogDB
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstans.CONNECTION_STRING);

        public async Task<int> InsertAsync(Blog model)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $@" insert into ""Blog"" (""BlogId"",""Title"",""Content"",""IpAddress"",""CreateTime"",""TypeId"",""UpdateTime"") ";
                query += " values (@BlogId,@Title,@Content,@IpAddress,@CreateTime,@TypeId,@UpdateTime) ";
                NpgsqlCommand cmd = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new NpgsqlParameter("@BlogId",model.BlogId),
                        new NpgsqlParameter("@Title",model.Title),
                        new NpgsqlParameter("@Content",model.Content),
                        new NpgsqlParameter("@IpAddress",model.IpAddress),
                        new NpgsqlParameter("@CreateTime",model.CreateTime),
                        new NpgsqlParameter("@TypeId",model.TypeId),
                        new NpgsqlParameter("@UpdateTime",model.UpdateTime),
                    }
                };
                int result = await cmd.ExecuteNonQueryAsync();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
