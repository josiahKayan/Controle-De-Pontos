//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using Quartz;
//using Apassos.Common;
//namespace Apassos.Quartz
//{
//    public class QuartzClass : IJob
//    {
//        public void Execute(IJobExecutionContext context)
//        {

//            try
//            {
//                long idTicks = DateTime.Now.Ticks;
//                DateTimeOffset startTime = DateBuilder.FutureDate(45, IntervalUnit.Second);
//                IJobDetail job2 = JobBuilder.Create<QuartzClass>()
//                                           .WithIdentity("job" + idTicks)   ///exception happens when use name job only
//                                           .Build();

//                ITrigger trigger2 = TriggerBuilder.Create()
//                                                        .WithIdentity("trigger" + idTicks)
//                                                        .StartAt(startTime)
//                                                        ///.WithSimpleSchedule(x => x.WithIntervalInSeconds(15))   //.WithSimpleSchedule(x => x.WithIntervalInSeconds(15).WithRepeatCount(2))
//                                                        .Build();
//                MvcApplication.scApplication.ScheduleJob(job2, trigger2);

//                Util.EscreverLog("[QuartzClass]" + context.Trigger.Key.Name, "quartz", "quartz.log");

//                var a = 1 / int.Parse("0");
//            }
//            catch (Exception ex)
//            {
//                Util.EscreverLog("[QuartzClass-erro]" + ex.Message + " : " + context.Trigger.Key.Name, "quartz", "quartz.log");
//            }


//        }
//    }
//}