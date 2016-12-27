using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Apassos.reports.classes
{
    public class ApontamentosXLS : BaseXLS
    {
        private Partners consultorAtual;
        private PartnersTimesheetHeaderAccess partnerPeriod;

        public ApontamentosXLS(string periodid, Partners consultorAtual)
        {
            this.periodoAtual = PeriodDataAccess.GetPeriodo(periodid);
            this.consultorAtual = consultorAtual;
            this.filename = "apontamentos_" + periodoAtual.YEAR + "_" + periodoAtual.MONTH + ".xlsx";
            this.partnerPeriod = new PartnersTimesheetHeaderAccess(consultorAtual, periodoAtual, periodoAtual);
            this.wb = new XSSFWorkbook();
          this.CriaAbaConsultor();
        }

        public void CriaAbaConsultor()
        {
            this.CriaAbaConsultor(this.partnerPeriod);
            this.AutoSizeColumn();
        }

        public void CriaAbas()
        {
            this.CriaAbaConsultor(this.partnerPeriod);
        }

    }
}