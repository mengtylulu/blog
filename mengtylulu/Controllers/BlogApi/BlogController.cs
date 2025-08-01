using mengtylulu.ApiModel.BlogApi;
using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;
using mengtylulu.Model.DB.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using mengtylulu.ADO.NET;
using mengtylulu.Model.DB.BlogApi;
using mengtylulu.Model.DB.Models;
namespace mengtylulu.Controllers.BlogApi
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlogController : ControllerBase
    {

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
        public void GetBlog(getBlogInput input)
        {
        }

        [HttpPost]
        public void TestDB(getBlogInput input)
        {
            Connect connect = new Connect();
            connect.testConnect();
        }
    }
}
