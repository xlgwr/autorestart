using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using Quartz;
using System.Reflection;

namespace ReStartServer.job
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class restartJob : IJob
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            logger.Debug("执行重始任务!!!!!!!!!!!!!!!");
            if (Service1.isStop >= 1)
            {
                try
                {
                    var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var root = System.IO.Path.GetDirectoryName(currentAssembly);
                    var cmdPath = System.IO.Path.Combine(root, "_reStartEPMCSService.cmd");
                    //执行批处理进行恢复
                    logger.DebugFormat("开始任务:{0}", cmdPath);

                    System.Diagnostics.Process.Start("cmd.exe", "/c \"" + cmdPath + "\"").WaitForExit();

                    logger.Debug("********************执行重始任务完成。!!!!!!!!!!!!!!!");
                }
                catch (Exception ex)
                {

                    logger.ErrorFormat("执行重始任务失败，{0}", ex.Message);

                }
            }
            else
            {
                logger.DebugFormat("***********************停止执行重始任务!!!!!!!!!!!!!!!,isStop: flag {0},restart:{1} ", Service1.isStop, Service1.restart); ;
             }

            //throw new NotImplementedException();
        }
    }
}
