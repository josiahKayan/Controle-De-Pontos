using Apassos.Models;
using Apassos.TeamWork.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apassos.TeamWork.WebServices
{
    public interface ITimeEntryService
    {

        List<EntryTime> GetItems();


    }
}
