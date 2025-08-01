using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;

namespace mengtylulu.ApiModel.DotNet.DependencyInjection.Model
{
    public class UserService : IUserService
    {
        public void Show()
        {
            Console.WriteLine("UserService");
        }
    }

    public class UserServiceEx : IUserService
    {
        public void Show()
        {
            Console.WriteLine("UserServiceEx");
        }
    }
}
