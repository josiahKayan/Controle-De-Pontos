using Apassos.Models;
using Apassos.Observer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Apassos.DataAccess
{
    public class LogDataAccess
    {

        public List<Logs> GetLogByPeriod(int? periodId)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                List<Logs> Logs = db.Logs.Where(p => p.PeriodoId == periodId).OrderByDescending(date => date.ActivityDate).ToList();
                return Logs;
            }
        }

        public List<Logs> GetLogByPeriodAndPartner(int? periodId, int? partnerId)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                List<Logs> Logs = db.Logs.Where(p => p.PeriodoId == periodId && p.ConsultorId == partnerId ).OrderByDescending(date => date.ActivityDate).ToList();
                return Logs;
            }
        }

    }
}