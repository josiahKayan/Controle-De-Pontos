using Apassos.DataAccess;
using Apassos.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Handler
{
    public class TimesheetManagerHeader
    {
        private string _enviroment = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

        public TimesheetHeader GetHeader(Period period, Partners partner)
        {
            TimesheetDataAccess timesheetSalvar = new TimesheetDataAccess();
            TimesheetHeader header = timesheetSalvar.GetApontamentoCabecalhoPorPeriodo(partner, period);
            if (header == null)
            {
                header = new TimesheetHeader();
                header.ENVIRONMENT = _enviroment;
                header.CHANGEDATE = DateTime.Now.Date;
                header.CHANGEDBY = "TimesheetService";
                header.CREATEDBY = "TimesheetService";
                header.CREATIONDATE = DateTime.Now.Date;
                header.Period = period;
                header.Partner = partner;
            }

            return header;
        }


        public int AlgumNaoAprovado(TimesheetContext db, TimesheetTeamWorkItem foundItem)
        {

            var total = db.TimesheetItems.Where(x => x.TimesheetHeader.TIMESHEETHEADERID == foundItem.TimesheetItem.TimesheetHeader.TIMESHEETHEADERID && x.STATUS == 0).FirstOrDefault();

            if (total != null)
            {
                return 1;

            }
            else
            {

                int quantidadeAprovados = 0;
                quantidadeAprovados = db.TimesheetItems.Where(x => x.TimesheetHeader.TIMESHEETHEADERID == foundItem.TimesheetItem.TimesheetHeader.TIMESHEETHEADERID && x.STATUS == 1).ToList().Count();

                if (quantidadeAprovados > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

        public int AlgumNaoAprovadoComNovoApontamento(TimesheetContext db, TimesheetHeader header)
        {

            var total = db.TimesheetItems.Where(x => x.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID && x.STATUS == 0).FirstOrDefault();

            if (total != null)
            {
                return 1;

            }
            else
            {

                int quantidadeAprovados = 0;
                quantidadeAprovados = db.TimesheetItems.Where(x => x.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID && x.STATUS == 1).ToList().Count();

                if (quantidadeAprovados > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
        }

    }
}