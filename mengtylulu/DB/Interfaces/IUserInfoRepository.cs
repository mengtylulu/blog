using mengtylulu.DB.Models;

namespace mengtylulu.DB.Interfaces
{
    public interface IUserInfoRepository
    {
        public Task<int> CreateAsync(UserInfo userInfo);

        public Task<int> UpdateAsync(long id, UserInfo userInfo);

        public Task<int> DeleteAsync(long id);

        public Task<UserInfo?> GetAsync(long id);

        public Task<IEnumerable<UserInfo>> GetAllAsync(int skip, int take);
    }
}
