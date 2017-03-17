using Quartz;
using Quartz.Impl;

namespace Apassos.TeamWork.Jobs
{
    public class TeamWorkScheduler
    {

        private IScheduler _scheduler;

        private IJobDetail _job;

        private ITrigger _trigger;

        private const string TRIGGER_NAME = "teamwork_trigger_getitems";

        private const string GROUP_NAME = "teamwork_group";

        private const int MINUTES = 4;

        private const int SECONDS = 60;

        private const int INTERVAL_IN_SECONDS = MINUTES * SECONDS;

        public TeamWorkScheduler()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
            _job = JobBuilder.Create<TeamWorkJob>().Build();
        }

        public void Start()
        {
            _scheduler.Start();

            _trigger = TriggerBuilder.Create().WithIdentity(TRIGGER_NAME, GROUP_NAME).StartNow().
                  WithSimpleSchedule(s => s.WithIntervalInSeconds(INTERVAL_IN_SECONDS).RepeatForever()).Build();

            _scheduler.ScheduleJob(_job, _trigger);
        }
    }
}