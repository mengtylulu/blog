namespace mengtylulu.Data;

using mengtylulu.Infrastructure;
using mengtylulu.Model;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DirectoryRepository : IDirectoryRepository
{
    private readonly ConnectionSetting? _connection;
    public DirectoryRepository(IOptions<ConnectionSetting> connection)
    {
        _connection = connection.Value;
    }

    public Task<Directory> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Directory>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Directory> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Directory> InsertAsync(Directory Entity)
    {
        throw new NotImplementedException();
    }

    public Task<Directory> UpdateAsync(Directory Entity)
    {
        throw new NotImplementedException();
    }

    //public async Task<Directory> GetById(Guid id)
    //{
    //    Directory directory = null;
    //    await Task.CompletedTask;
    //    return null;
    //}
}
