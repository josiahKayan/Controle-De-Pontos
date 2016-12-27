using Apassos.DataAccess;
using Apassos.reports.classes;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;

namespace Apassos.Models
{
    public class RelatorioProjetosXLS : BaseXLS
    {

        private List<RelatorioHoras> listaRelatorios;
        private Project project;
        private Partners partner;
        private Period periodoatual;

        public RelatorioProjetosXLS(string periodid)
        {

            this.periodoatual = PeriodDataAccess.GetPeriodo(periodid);

            this.filename = "apontamentos_projeto" + periodoatual.YEAR + "_" + periodoatual.MONTH + ".xlsx";

            this.wb = new XSSFWorkbook();

            if (project == null && partner == null)
            {
                this.CriaAbaProjetoConsultorHorasNovo(WITH_PARTNER_CONST,periodoatual, periodoatual);
                this.CriaAbaProjetoConsultorHorasNovo(NO_PARTNER_CONST, periodoatual, periodoatual);
            }
        }


        public RelatorioProjetosXLS(Period periodoInicial, Period periodoFinal, Project project, Partners partner)
        {
            this.project = project;
            this.partner = partner;
            this.wb = new XSSFWorkbook();

            if ((project != null && partner != null))
            {
                this.filename = "apontamentos_projeto" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
                this.CriaAbaProjetoConsultorHorasNovo(BOTH_ITEMS_CONST, periodoInicial, periodoFinal);
            }
            else if (project != null && partner == null)
            {
                this.filename = "apontamentos_projeto" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
                this.CriaAbaProjetoConsultorHorasNovo(ONLY_PROJECT_CONST, periodoInicial, periodoFinal);
            }
            else if (project == null && partner != null)
            {
                this.filename = "apontamentos_projeto" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
                this.CriaAbaProjetoConsultorHorasNovo(ONLY_PARTNER_CONST, periodoInicial, periodoFinal);
            }
            else if ( (project == null && partner == null))
            {
                this.filename = "apontamentos_projeto" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
                this.CriaAbaProjetoConsultorHorasNovo(WITH_PARTNER_CONST, periodoInicial, periodoFinal);
                this.CriaAbaProjetoConsultorHorasNovo(NO_PARTNER_CONST, periodoInicial, periodoFinal);
            }
        }

        public void CriaAbaProjetoConsultorHorasNovo(string exportToExcel, Period periodoInicial, Period periodoFinal)
        {
            if (exportToExcel.Equals(WITH_PARTNER_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoConsultorHoras(periodoInicial, periodoFinal);
            }
            else if (exportToExcel.Equals(NO_PARTNER_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoInicial, periodoFinal);
            }
            else if (exportToExcel.Equals(ONLY_PROJECT_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoInicial, periodoFinal, project.PROJECTID);
            }
            else if (exportToExcel.Equals(BOTH_ITEMS_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoInicial, periodoFinal, project.PROJECTID, partner.PARTNERID );
            }
            else if (exportToExcel.Equals(ONLY_PARTNER_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoInicial, periodoFinal, 0, partner.PARTNERID);
            }
            else if (exportToExcel.Equals(NO_PROJECT_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoInicial, periodoFinal, 0, 0);
            }

            this.CriaAbaProjetoConsultorHoras(listaRelatorios, exportToExcel);
            this.AutoSizeColumn();
        }



        public void criaabaprojetoconsultorhoras(string exporttoexcel)
        {
            if (exporttoexcel.Equals(WITH_PARTNER_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoConsultorHoras(periodoatual.YEAR, periodoatual.MONTH);
            }
            else if (exporttoexcel.Equals(NO_PARTNER_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoatual.YEAR, periodoatual.MONTH);
            }
            else if (exporttoexcel.Equals(ONLY_PROJECT_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoatual.YEAR, periodoatual.MONTH, project.PROJECTID);
            }
            else if (exporttoexcel.Equals(BOTH_ITEMS_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoatual.YEAR, periodoatual.MONTH,
                        project.PROJECTID, partner.PARTNERID);
            }
            else if (exporttoexcel.Equals(ONLY_PARTNER_CONST))
            {
                listaRelatorios = RelatorioAccess.GetListaRelatorioProjetoHoras(periodoatual.YEAR, periodoatual.MONTH, 0, partner.PARTNERID);
            }
            this.CriaAbaProjetoConsultorHoras(listaRelatorios, exporttoexcel);
            this.AutoSizeColumn();
        }


    }
}