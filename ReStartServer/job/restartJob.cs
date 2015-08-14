using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using Quartz;
using System.Reflection;
using ReStartServer.DAL;

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
            if (Program.isStop >= 1)
            {
                try
                {
                    var tmplastCollectTime = new DateTime();
                    using (MysqlDbContext db = new MysqlDbContext())
                    {
                        var tmpcount = db.Datas.Count();
                        if (tmpcount >= 1)
                        {
                            tmplastCollectTime = db.Datas.Max(d => d.PowerDate);
                        }
                        else
                        {
                            tmplastCollectTime = DateTime.Now.AddMinutes(-60);
                        }
                    }
                    var tmpdiffMin = DateTime.Now - tmplastCollectTime;
                    logger.DebugFormat("***************数据库最后时间：{0}，当前时间：{1}，时间差（分）：{2}", tmplastCollectTime, DateTime.Now, tmpdiffMin.TotalMinutes);
                    if (tmpdiffMin.TotalMinutes >= Program._diffMin)
                    {
                        var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        var root = System.IO.Path.GetDirectoryName(currentAssembly);
                        var cmdPath = System.IO.Path.Combine(root, "_reStartEPMCSService.cmd");
                        //执行批处理进行恢复
                        logger.DebugFormat("开始任务:{0}", cmdPath);

                        System.Diagnostics.Process.Start("cmd.exe", "/c \"" + cmdPath + "\"").WaitForExit();

                        logger.Debug("********************执行重始任务完成。!!!!!!!!!!!!!!!");
                    }
                    else
                    {
                        logger.DebugFormat("********************时间小于【{0}】,无需重始采集服务。!!!!!!!!!!!!!!!", Program._diffMin);
                    }


                }
                catch (Exception ex)
                {

                    logger.ErrorFormat("执行重始任务失败，{0}", ex.Message);

                }
            }
            else
            {
                logger.DebugFormat("***********************停止执行重始任务!!!!!!!!!!!!!!!,isStop: flag {0},restart:{1} ", Program.isStop, Program.restart); ;
            }

            //throw new NotImplementedException();
        }
    }
}
