using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.IO;
using System.Text;

namespace ReStartServer
{
    class Test
    {
        private readonly ILog logger;
        public static IScheduler scheduler;
        public Test()
        {
            logger = LogManager.GetLogger(GetType());

            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }
        public void OnStart()
        {
            logger.Debug("====================以下参数修改后需重启服务生效===================");
            logger.DebugFormat("开始重始服务，【{0}】",restart );
            logger.Debug("================================================================");
        }

        private static int restart = 0;

        public static int Restart()
        {
            if (restart <= 0)
            {
                string txt = System.Configuration.ConfigurationSettings.AppSettings.Get("restart");
                if (!int.TryParse(txt, out restart))
                {
                    restart = 60; //默认60分钟
                }
                if (restart < 5) restart = 5;
            }
            return restart;
        }
    }
}
