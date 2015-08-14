//#define Dev

using System.ServiceProcess;
namespace ReStartServer
{
#if Dev

    internal class Program
    {
        private static void Main(string[] args)
        {
            Test test = new Test();
            test.OnStart();
        }
    }

#else
    static class Program
    {

        public static int restart = 0;
        public static int isStop = 0;
        public static int _diffMin = 0;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
#endif
}