using Apassos.DataAccess;
using Apassos.Models;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.reports.classes
{
   public class RelatoriosXLS : BaseXLS
   {

      private Partners gestorAtual;
      private List<PartnersTimesheetHeaderAccess> consultoresApontamentos;

      public RelatoriosXLS(string periodid, Partners gestorAtual)
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
      public RelatoriosXLS(string periodid, Project projeto, Partners partner, string periodoInicialId, string periodoFinalId)
      {


         //Para todos os campos cheios...
         if (projeto == null && partner == null && periodoInicialId != null && periodoFinalId != null)
         {
            this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
            this.periodoFinal = PeriodDataAccess.GetPeriodo(periodoFinalId);
            this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + "_" + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, periodoInicial, periodoFinal);
         }

         //Para todos os campos cheios...
         else if (projeto != null && partner != null && periodoInicialId != null && periodoFinalId != null)
         {
            this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
            this.periodoFinal = PeriodDataAccess.GetPeriodo(periodoFinalId);
            this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + "_" + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), PeriodDataAccess.GetPeriodo(periodoFinalId));
         }

         //Fazendo o periodoInicial como vazio
         else if (projeto != null && partner != null && periodoFinalId != null && periodoInicialId == null)
         {
            this.periodoFinal = PeriodDataAccess.GetPeriodo(periodoFinalId);
            this.filename = "apontamentos_" + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, null, PeriodDataAccess.GetPeriodo(periodoFinalId));
         }

         //Fazendo o periodoFinal como vazio
         else if (projeto != null && partner != null && periodoFinalId == null && periodoInicialId != null)
         {
            this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
            this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), null);
         }

         //Fazendo os dois periodos como nulo
         else if (projeto != null && partner != null && periodoFinalId == null && periodoInicialId == null)
         {
            this.filename = "apontamentos_" + projeto.NAME + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto);
         }

         //Fazendo o projeto como vazio e com qualquer periodo nulo
         else if (projeto == null && partner != null && (periodoFinalId == null || periodoInicialId == null) || (periodoFinalId != null || periodoInicialId != null))
         {
            if (partner != null && periodoFinalId != null && periodoInicialId != null)
            {
               this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
               this.periodoFinal = PeriodDataAccess.GetPeriodo(periodoFinalId);
               this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + "_" + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), PeriodDataAccess.GetPeriodo(periodoFinalId));
            }
            else if (partner != null && periodoInicialId != null)
            {
               this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
               this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), null);
            }
            else if (partner != null && periodoFinalId != null)
            {
               this.periodoFinal = PeriodDataAccess.GetPeriodo(periodoFinalId);
               this.filename = "apontamentos_" + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, null, PeriodDataAccess.GetPeriodo(periodoFinalId));
            }
            else if (partner != null && periodoFinalId == null && periodoInicialId == null)
            {
               this.filename = "apontamentos_" + partner.SHORTNAME + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(partner, projeto, null, null);
            }
         }

         //Caso o Partner seja nulo
         if (partner == null && projeto != null)
         {
            if (projeto != null && periodoFinalId != null && periodoInicialId != null)
            {
               this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
               this.periodoFinal = PeriodDataAccess.GetPeriodo(periodoFinalId);
               this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + "_" + periodoFinal.YEAR + "_" + periodoFinal.MONTH + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), PeriodDataAccess.GetPeriodo(periodoFinalId));
            }
            else if (projeto != null && periodoInicialId != null)
            {
               this.periodoInicial = PeriodDataAccess.GetPeriodo(periodoInicialId);
               this.filename = "apontamentos_" + periodoInicial.YEAR + "_" + periodoInicial.MONTH + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), null);
            }
            else if (projeto != null && periodoFinalId == null && periodoInicialId == null)
            {
               this.filename = "apontamentos_" + projeto.NAME + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(projeto, null, null);
            }
            else if (projeto != null && periodoInicialId != null)
            {
               this.filename = "apontamentos_" + projeto.NAME + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(projeto, PeriodDataAccess.GetPeriodo(periodoInicialId), null);
            }
            else if (projeto != null && periodoFinalId != null)
            {
               this.filename = "apontamentos_" + projeto.NAME + ".xlsx";
               this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(projeto, null, PeriodDataAccess.GetPeriodo(periodoFinalId));
            }

         }
         //Caso qualquer campo seja branco
         if (partner == null && projeto == null && periodoFinalId == null && periodoInicialId == null)
         {
            this.filename = "apontamentos_" + ".xlsx";
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(null, null, null);
         }




         this.wb = new XSSFWorkbook();
         this.CriaGeral();
         this.CriaAbas();
      }


      //Fazendo a sobrecarga de método aqui, a variável irrelevante serve para poder passar user o construtor
      public RelatoriosXLS(string periodid, Project projeto, Partners partner)
      {
         // Se todos forem nulos isso resolve
         this.periodoAtual = PeriodDataAccess.GetPeriodo(periodid);
         this.filename = "apontamentos_" + periodoAtual.YEAR + "_" + periodoAtual.MONTH + ".xlsx";

         if (projeto == null && partner == null)
         {
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(PeriodDataAccess.GetPeriodo(periodid));
         }
         //Se o projeto for diferente de null 
         else if (projeto != null && partner == null)
         {
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorProjeto(PeriodDataAccess.GetPeriodo(periodid), projeto);
         }
         else if (projeto != null && partner != null)
         {
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoProjeto(PeriodDataAccess.GetPeriodo(periodid), partner, projeto);
         }
         else if (projeto == null && partner != null)
         {
            this.consultoresApontamentos = ProjectDataAccess.GetConsultoresApontamentosPorPeriodoPartner(PeriodDataAccess.GetPeriodo(periodid), partner);
         }

         this.wb = new XSSFWorkbook();
         this.CriaGeral();
         this.CriaAbas();
      }


      public void CriaAbas()
      {
         foreach (var consultorApontamentos in this.consultoresApontamentos)
         {
            this.CriaAbaConsultorRelatorio(consultorApontamentos);
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