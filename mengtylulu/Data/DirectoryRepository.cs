namespace mengtylulu.Data;

using mengtylulu.Infrastructure;
using mengtylulu.Model;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

public class DirectoryRepository : IDirectoryRepository
{
    //private readonly ConnectionSetting? _connection;
    //public DirectoryRepository(IOptions<ConnectionSetting> connection)
    //{
    //    _connection = connection.Value;
    //}

    //public async Task<Directory> GetById(Guid id)
    //{
    //    Directory directory = null;
    //    await Task.CompletedTask;
    //    return null;
    //}

     Task<Directory> IRepository<Directory>.GetById(int id)
    {
        throw new NotImplementedException();
    }

    Task<Directory> IRepository<Directory>.Create(Directory Entity)
    {
        throw new NotImplementedException();
    }

    Task<Directory> IRepository<Directory>.Delete(int id)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Directory>> IRepository<Directory>.GetAll()
    {
        throw new NotImplementedException();
    }


    Task<Directory> IRepository<Directory>.Update(Directory Entity)
    {
        throw new NotImplementedException();
    }
}
