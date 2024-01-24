using mengtylulu.ApiModel.BlogApi;
using mengtylulu.DB.BlogApi;
using mengtylulu.DB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace mengtylulu.Controllers.BlogApi
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        [HttpPost]
        public async Task<int> InsertBlog(InsertBlogInput input)
        {
            BlogDB blogDB = new BlogDB();
            Blog blog = new Blog()
            {
                BlogId = input.BlogId,
                Content = input.Content,
                CreateTime = input.CreateTime,
                IpAddress = IPAddress.Parse(input.IpAddress),
                Title = input.Title,
                TypeId = input.TypeId,
                UpdateTime = input.UpdateTime,
            };
            var res = await blogDB.InsertAsync(blog);
            return res;
        }
    }
}
