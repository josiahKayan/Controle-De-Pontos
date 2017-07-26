using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Apassos.Models;
using Apassos.Common;
using System.Configuration;

namespace Apassos.DataAccess
{
    public class PeriodDataAccess
    {
        /**
         * Retorna o periodo atual cadastrado em banco.Em ordem descrescente de entrada.
         */
        public List<Period> GetPeriodoAll()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Periods.Where(p => p.ENVIRONMENT == env).ToList();
            lista = lista.OrderByDescending(p => p.YEAR).ThenByDescending(p2 => p2.MONTH).ToList();
            return lista;
        }
        }

        /**
         * Retorna o periodo atual cadastrado em banco. 
         */





        /**
         * Retorna o periodo atual cadastrado em banco. 
         */
        public Period GetPeriodoAtual()
        {
            var lista = GetPeriodoAll();
            return lista.FirstOrDefault();
        }

        public Period GetPeriodoActivity(DateTime date)
        {
            Period period;
            using ( TimesheetContext db = new TimesheetContext())
            {
                period = db.Periods.Where(x => x.MONTH == date.Month && x.YEAR == date.Year).FirstOrDefault();
            }

            return period;
        }


        /**
         * Retorna o periodo pelo id. 
         */
        public Period GetPeriodo(string id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var period = db.Periods.Find(int.Parse(id));
                return period;
            }
        }




    /**
     * Retorna uma lista de datas, baseada no periodo atual cadastrado no sistema.
     */
    public List<DateTime> GetListDate()
        {
            return GetListDate(GetPeriodoAtual());
        }



        /**
         * Retorna uma lista de datas, baseada no periodo passado como parametro.
         */
        public List<DateTime> GetListDate(Period period)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                if (period.TIMESHEETPERIODSTART != null && period.TIMESHEETPERIODFINISH != null)
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var lista = db.Projects.Where(p => p.ENVIRONMENT == env && p.ENVIRONMENT == env).ToList();
                List<DateTime> datas = Util.GetDateListByRange(period.TIMESHEETPERIODSTART.GetValueOrDefault(), period.TIMESHEETPERIODFINISH.GetValueOrDefault());
                return datas;
            }
            return null;
        }
        }
    }
}





































