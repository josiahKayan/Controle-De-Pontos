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
    public class ConsultoresApontamentosXLS : BaseXLS
    {
        private Partners gestorAtual;
        private List<PartnersTimesheetHeaderAccess> consultoresApontamentos;

        public ConsultoresApontamentosXLS(string periodid, Partners gestorAtual)
        {
            this.periodoAtual = PeriodDataAccess.GetPeriodo(periodid);
            this.gestorAtual = gestorAtual;
            this.filename = "apontamentos_" + periodoAtual.YEAR + "_" + periodoAtual.MONTH + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(PeriodDataAccess.GetPeriodo(periodid));
            this.wb = new XSSFWorkbook();
            this.CriaGeral();
            this.CriaAbas();

        }

        //Fazendo a sobrecarga de método aqui, a variável irrelevante serve para poder passar user o construtor
        public ConsultoresApontamentosXLS(string periodid, Partners gestorAtual, bool irrelevante)
        {

            this.periodoAtual = PeriodDataAccess.GetPeriodo(periodid);
            this.gestorAtual = gestorAtual;
            this.filename = "apontamentos_" + periodoAtual.YEAR + "_" + periodoAtual.MONTH + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(PeriodDataAccess.GetPeriodo(periodid));
            this.wb = new XSSFWorkbook();
            this.CriaGeral();
            this.CriaAbas();
        }

        public void CriaAbas()
        {
            foreach (var consultorApontamentos in this.consultoresApontamentos)
            {
                this.CriaAbaConsultor(consultorApontamentos);
                this.AutoSizeColumn();
            }
        }

        public void CriaGeral()
        {
            this.CriaAbaGeral(this.consultoresApontamentos);
            this.AutoSizeColumn();
        }

    }
}