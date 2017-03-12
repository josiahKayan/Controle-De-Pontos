﻿using Apassos.DataAccess;
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
                //header.Period = period;
                //header.Partner = partner;
                header.CHANGEDATE = DateTime.Now.Date;
                header.CHANGEDBY = "TimesheetService";
                header.CREATEDBY = "TimesheetService";
                header.CREATIONDATE = DateTime.Now.Date;
                header.Period = period;
                header.Partner = partner;
            }

            return header;
        }

    }
}