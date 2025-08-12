using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using mengtylulu.Infrastructure;
using mengtylulu.Model.BlogApi;
using mengtylulu.DTOs.Blog;
namespace mengtylulu.Controllers.BlogApi
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlogController : ControllerBase
    {
        private readonly IRepository<BlogDirectory> _directory;
        public BlogController(IRepository<BlogDirectory> directory)
        {
            this._directory = directory;
        }

        [HttpPost]
        public async Task<int> InsertDirectoryAsync(InsertDirectoryDto insertDirectoryDto)
        {
            var directory = await _directory.InsertAsync(new BlogDirectory
            {
                id = Guid.NewGuid(),
                introduction = insertDirectoryDto.Introduction,
                timecreate = insertDirectoryDto.TimeCreate,
                time_update = insertDirectoryDto.TimeUpdate,
                title = insertDirectoryDto.Title,
            });
            await Task.CompletedTask;
            return 0;
        }

        [HttpPost]
        public async Task<IActionResult> GetDirectoryByIdAsync(BlogDirectoryPostDto postDto)
        {
            var directory = await _directory.GetByIdAsync(Guid.Parse("268cdb3b-6014-4291-8131-dc3d6e0bf95d"));
            await Task.CompletedTask;
            return Ok(directory);
        }

        //[HttpPost]
        //public void TestDB(getBlogInput input)
        //{
        //}
    }
}
