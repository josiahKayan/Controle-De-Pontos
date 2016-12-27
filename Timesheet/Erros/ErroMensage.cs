using Apassos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.Erros
{
    public class ErroMensage
    {
        static private TimesheetContext db = new TimesheetContext();


        public List<TeamworkLogTraces> RetornaErros()
        {

            List<TeamworkLogTraces> listLogs = db.TeamworkLogTraces.ToList();
            return listLogs;

        }

        public static void MarcarNaoEnviarEmail(TeamworkLogTraces currentLog)
        {

            TeamworkLogTraces log = db.TeamworkLogTraces.Where(x => x.Id == currentLog.Id).FirstOrDefault();

            if (log != null)
            {

                log.IsFixed = currentLog.IsFixed;
                db.SaveChanges();
            }

        }
       

    }
}


