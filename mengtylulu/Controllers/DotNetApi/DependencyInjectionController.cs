using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;
using Microsoft.AspNetCore.Mvc;

namespace mengtylulu.Controllers.DotNetApi
{
    /// <summary>
    /// 依赖注入
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class DependencyInjectionController : ControllerBase
    {
        [HttpGet]
        public void Test(
            [FromServices] ITestTransient testTransient1,
            [FromServices] ITestTransient testTransient2,
            [FromServices] ITestScoped testScoped1,
            [FromServices] ITestScoped testScoped2,
            [FromServices] ITestSingleton testSingleton1,
            [FromServices] ITestSingleton testSingleton2
            )
        {
            Console.WriteLine($"==========Request Start==========");
            Console.WriteLine($"Transient1 HashCode:{testTransient1.GetHashCode()},Transient2 HashCode:{testTransient2.GetHashCode()}");
            Console.WriteLine($"testScoped1 HashCode:{testScoped1.GetHashCode()},testScoped2 HashCode:{testScoped2.GetHashCode()}");
            Console.WriteLine($"testSingleton1 HashCode:{testSingleton1.GetHashCode()},testSingleton2 HashCode:{testSingleton2.GetHashCode()}");
        }
    }
}
