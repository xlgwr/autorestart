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
            Restart();
            if (isStop <= 0)
            {
                logger.Debug("**********************停止执行重始任务 EPMCS.Service");
                logger.Error("**********************停止执行重始任务 EPMCS.Service");
                return;
            }
            else
            {
                logger.DebugFormat("开始重始服务 EPMCS.Service，每【{0}】分钟", restart);

            }
            logger.Debug("================================================================");

            scheduler.Start();
            logger.Info("Quartz服务成功启动");

            DateTimeOffset runTime = DateBuilder.EvenSecondDate(DateTimeOffset.Now);

            #region "Restart"

            int RestartInterval = restart;
            IJobDetail restart_Job = JobBuilder.Create<restartJob>()
                .WithIdentity("ReStart_job", "ReStart_Group")
                 .Build();

            ITrigger restart_trigger = TriggerBuilder.Create()
                .WithIdentity("ReStart_Trigger","ReStart_Group")
                .StartAt(runTime)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(RestartInterval).RepeatForever())
                .Build();
            scheduler.ScheduleJob(restart_Job, restart_trigger);

            #endregion "Restart"

        }

        public void OnStop()
        {
            try
            {
                scheduler.Shutdown();
                logger.Info("Quartz服务成功终止");
            }
            finally { }           
        }

        public void OnPause()
        {
            scheduler.PauseAll();
        }

        public void OnContinue()
        {
            scheduler.ResumeAll();
        }

        public static int restart = 0;
        public static int isStop = 0;
        public static int Restart()
        {
            if (restart <= 0)
            {
                string txt = System.Configuration.ConfigurationSettings.AppSettings.Get("restart");
                if (!int.TryParse(txt, out restart))
                {
                    restart = 60; //默认60分钟
                }
                if (restart <= 0)
                {
                    isStop = 0;
                    restart = 5;
                }
                else
                {
                    isStop = 1;
                }
            }
            return restart;
        }
    }
}
