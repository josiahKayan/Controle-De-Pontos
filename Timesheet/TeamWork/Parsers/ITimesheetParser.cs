using Apassos.Models;
using System.Collections.Generic;

namespace Apassos.TeamWork.Parsers
{
  public interface ITimesheetParser
  {
    List<TimesheetTeamWorkItem> GetItems();

  }
}
  