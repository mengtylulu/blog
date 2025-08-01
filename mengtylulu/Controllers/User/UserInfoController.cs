using mengtylulu.ApiModel;
using mengtylulu.Model.DB.Repositories;
using mengtylulu.Model.DB.Interfaces;
using mengtylulu.Model.DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace mengtylulu.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfoController : ControllerBase
    {
        [HttpGet]
        public void InsertUser([FromQuery] InsertUserModel input)
        {
            UserInfo userInfo = new UserInfo()
            {
                UserId = Guid.NewGuid(),
                Email = input.Email,
                Name = input.Name,
                Password = input.Password,
            };
            IUserInfoRepository userInfoRepository = new UserInfoRepository();
            var res = userInfoRepository.CreateAsync(userInfo);
        }
    }
}
