using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.IO;


using ReStartServer.job;
using System.ServiceProcess;

namespace ReStartServer
{
    public partial class Service1 : ServiceBase
    {
        private readonly ILog logger;
        public static IScheduler scheduler;
        public Service1()
        {
            InitializeComponent();
            logger = LogManager.GetLogger(GetType());
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        protected override void OnStart(string[] args)
        {
            logger.Debug("====================以下参数修改后需重启服务生效===================");
            logger.DebugFormat("开始重始服务 EPMCS.Service，【{0}】", Restart());
            logger.Debug("================================================================");


            scheduler.Start();
            logger.Info("Quartz服务成功启动");

            DateTimeOffset runTime = DateBuilder.EvenSecondDate(DateTimeOffset.Now);

            #region "Restart"
            logger.Debug("===============================Restart=================================");
            int RestartInterval = Restart();
            IJobDetail restart_Job = JobBuilder.Create<restartJob>()
                .WithIdentity("ReStart_job", "ReStart_Group")
                 .Build();

            ITrigger restart_trigger = TriggerBuilder.Create()
                .WithIdentity("ReStart_Trigger", "ReStart_Group")
                .StartAt(runTime)
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(RestartInterval).RepeatForever())
                .Build();
            scheduler.ScheduleJob(restart_Job, restart_trigger);

            #endregion "Restart"
        }

        protected override void OnStop()
        {
            try
            {
                scheduler.Shutdown();
                logger.Info("Quartz服务成功终止");
            }
            finally { }    
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
