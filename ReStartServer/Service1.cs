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
            Restart();
            if (Program.isStop <= 0)
            {
                logger.Debug("**********************时间设为：0，则停止执行重始任务 EPMCS.Service");
                logger.Error("**********************时间设为：0，则停止执行重始任务 EPMCS.Service");
                return;
            }
            else
            {
                logger.DebugFormat("检查服务 EPMCS.Service，每【{0}】分钟", Program.restart);
                logger.DebugFormat("开始重始服务 EPMCS.Service，每时间差 >=【{0}】分钟", Program._diffMin);

            }
            logger.Debug("================================================================");


            scheduler.Start();
            logger.Info("Quartz服务成功启动");

            DateTimeOffset runTime = DateBuilder.EvenSecondDate(DateTimeOffset.Now);

            #region "Restart"
            logger.Debug("===============================Restart=================================");
            int RestartInterval = Program.restart;
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

        public static int Restart()
        {
            if (Program.restart <= 0)
            {
                string txt = System.Configuration.ConfigurationSettings.AppSettings.Get("restartFind");
                string diffMin = System.Configuration.ConfigurationSettings.AppSettings.Get("restartMinDiff");

                if (!int.TryParse(txt, out Program.restart))
                {
                    Program.restart = 5; //默认5分钟
                }
                if (Program.restart <= 0)
                {
                    Program.isStop = 0;
                    Program.restart = 5;
                }
                else
                {
                    if (!int.TryParse(diffMin, out Program._diffMin))
                    {
                        Program._diffMin = 5; //默认5分钟
                    }
                    Program.isStop = 1;
                }
            }

            return Program.restart;
        }
    }
}
