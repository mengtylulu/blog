using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;

namespace mengtylulu.ApiModel.DotNet.DependencyInjection.Model
{
    public class StudentService : IStudentService
    {
        public void Show()
        {
            Console.WriteLine("StudentService");
        }
    }

    public class StudentServiceEx : IStudentService
    {
        public void Show()
        {
            Console.WriteLine("StudentServiceEx");
        }
    }
}
