using mengtylulu.DB;
using mengtylulu.DB.Interfaces;
using mengtylulu.DB.Models;
using mengtylulu.DB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace mengtylulu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAnimal _IAnimal;


        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAnimal animal)
        {
            _logger = logger;
            _IAnimal = animal;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            UserInfo userInfo = new UserInfo()
            {
                UserId = Guid.NewGuid(),
                Email = "2324064873@qq.com",
                Name = "tjt",
                Password = "123456789",
            };
            IUserInfoRepository userInfoRepository = new UserInfoRepository();
            var res = userInfoRepository.CreateAsync(userInfo);

            var test = _IAnimal.say();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}