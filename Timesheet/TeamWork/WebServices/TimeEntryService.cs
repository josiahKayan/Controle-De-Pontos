using Apassos.IoC;
using Apassos.Models;
using Apassos.TeamWork.Response;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.WebServices
{
    public class TimeEntryService : ITimeEntryService
    {


        [Inject]
        private ITeamWorkWebService _teamWorkService { set; get; }

        private IKernel _kernel;

        private string _enviroment;

        //private List<TeamworkLogTraces> _logTraceList;

        public TimeEntryService()
        {
            _kernel = new StandardKernel(new TimesheetNinjectModule());
            _teamWorkService = _kernel.Get<ITeamWorkWebService>();

            _enviroment = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
        }

        public List<EntryTime> GetItems()
        {
            List<EntryTime> timeEntries = _teamWorkService.GetAllTimeEntries();
            return timeEntries;
        }
    }
}