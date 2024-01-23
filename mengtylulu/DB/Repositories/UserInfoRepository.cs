using mengtylulu.DB.Constans;
using mengtylulu.DB.Interfaces;
using mengtylulu.DB.Models;
using Npgsql;

namespace mengtylulu.DB.Repositories
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstans.CONNECTION_STRING);

        public async Task<int> CreateAsync(UserInfo userInfo)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $@"insert into ""user_info"" (""userId"",""name"",""email"",""password"") values(@userId,@name,@email,@password)";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new NpgsqlParameter("@userId",userInfo.UserId),
                        new NpgsqlParameter("@name",userInfo.Name),
                        new NpgsqlParameter("@email",userInfo.Email),
                        new NpgsqlParameter("@password",userInfo.Password),
                    }
                };
                int result = await command.ExecuteNonQueryAsync();
                int i = 4;
                return result;
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("测试");
                throw new Exception(e.Message);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public Task<int> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserInfo>> GetAllAsync(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo?> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(long id, UserInfo userInfo)
        {
            throw new NotImplementedException();
        }
    }
}
