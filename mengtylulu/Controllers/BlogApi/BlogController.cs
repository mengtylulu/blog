using mengtylulu.ApiModel.BlogApi;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;
using mengtylulu.Model.DB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using mengtylulu.ADO.NET;
using mengtylulu.Model.DB.BlogApi;
using mengtylulu.Model.DB.Models;
using mengtylulu.Data;
using mengtylulu.Model;
namespace mengtylulu.Controllers.BlogApi
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlogController : ControllerBase
    {
        private readonly IRepository<mengtylulu.Model.Directory> _directory;
        public BlogController(IRepository<mengtylulu.Model.Directory> directory)
        {
                this._directory = directory;
        }
        [HttpPost]
        public async Task<int> InsertBlog(InsertBlogInput input)
        {
            BlogDB blogDB = new BlogDB();
            Blog blog = new Blog()
            {
                BlogId = new Guid(),
                Content = input.Content,
                CreateTime = new DateTime(),
                IpAddress = IPAddress.Parse(input.IpAddress),
                Title = input.Title,
                TypeId = new Guid(),
                UpdateTime = new DateTime(),
            };
            var res = await blogDB.InsertAsync(blog);
            return res;
        }

        [HttpPost]
        public async Task<IActionResult> GetBlog(getBlogInput input)
        {
            var directory =await _directory.GetByIdAsync(Guid.Parse("268cdb3b-6014-4291-8131-dc3d6e0bf95d"));
            await Task.CompletedTask;
            return Ok(directory);
        }

        [HttpPost]
        public void TestDB(getBlogInput input)
        {
            Connect connect = new Connect();
            connect.testConnect();
        }
    }
}
