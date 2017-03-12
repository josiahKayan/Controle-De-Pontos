using Apassos.TeamWork.Parsers;
using Apassos.TeamWork.Services;
using Apassos.TeamWork.WebServices;
using Ninject.Modules;

namespace Apassos.IoC
{
  public class TimesheetNinjectModule : NinjectModule
  {
    public override void Load()
    {

            ////SERVICES
            //Bind<ITeamWorkService>().To<TeamWorkService>();

            ////PARSERS
            //Bind<ITimesheetParser>().To<TimesheetParser>();

            //PARSERS
            Bind<ITimeEntryService>().To<TimeEntryService>();

            //SERVICES
            Bind<ITeamWorkWebService>().To<TeamWorkWebService>();

          


        }
  }
}