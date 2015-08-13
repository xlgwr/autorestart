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
           

            //throw new NotImplementedException();
        }
    }
}
