using Apassos.TeamWork.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamWorkNet.Model;

namespace Apassos.TeamWork.WebServices
{
    public interface ITeamWorkWebService
    {

        List<EntryTime> GetAllTimeEntries();

    }
}
