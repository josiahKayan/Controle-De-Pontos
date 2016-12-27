using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using Apassos.reports.classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Apassos.Controllers
{
    public class RelatoriosController : TimesheetBaseController
    {

        public ActionResult Relatorios()
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTSCONSULTANT))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }

            List<Period> listaPeriodoInicial = new List<Period>();
            Period periodoInicial = null;
            listaPeriodoInicial = PeriodDataAccess.GetPeriodoAll();
            List<Period> listaPeriodoFinal = new List<Period>();
            listaPeriodoFinal = PeriodDataAccess.GetPeriodoAll();
            Period periodoFinal = null;

            var _idPeriodoInicial = Request.Form["selectperiodoinicial"];
            if (_idPeriodoInicial != null && _idPeriodoInicial != null)
            {
                periodoInicial = PeriodDataAccess.GetPeriodo(_idPeriodoInicial);
            }
            else
            {
                periodoInicial = PeriodDataAccess.GetPeriodoAtual();
            }


            var _idPeriodoFinal = Request.Form["selectperiodoFinal"];
            if (_idPeriodoFinal != null && _idPeriodoFinal != null)
            {
                periodoFinal = PeriodDataAccess.GetPeriodo(_idPeriodoFinal);
            }
            else
            {
                periodoFinal = PeriodDataAccess.GetPeriodoAtual();
            }





            Period periodoAtual = null;
            var _periodid = Request.Form["selectperiod"];

            if (_periodid != null && _periodid != "")
            {
                periodoAtual = PeriodDataAccess.GetPeriodo(_periodid);
            }
            else
            {
                periodoAtual = PeriodDataAccess.GetPeriodoAtual();
            }

            List<Project> listaProjetos = new List<Project>();

            var _projectId = Request.Form["selectprojeto"];
            listaProjetos.AddRange(ProjectDataAccess.GetProjetosAll());

            listaProjetos.OrderBy(x => x.NAME);

            List<Period> listaPeriodos = PeriodDataAccess.GetPeriodoAll();

            List<Partners> consultoresDisponiveis = PartnerDataAccess.GetAllParceiros();

            Project projetoAtual = null;

            if (_projectId != null && _projectId != string.Empty)
            {
                projetoAtual = ProjectDataAccess.GetProjeto(_projectId);
            }
            if (projetoAtual != null)
            {
                consultoresDisponiveis = ProjectDataAccess.GetConsultoresProjeto(projetoAtual);
            }

            //Partners parceiroAtual = null ;
            //var _partnersId = Request.Form["selectconsultor"];

            //if (_partnersId != null && _partnersId != "")
            //{
            //    parceiroAtual = PartnerDataAccess.GetParceiroId(_partnersId);
            //}

            Session["PROJETO_ATUAL"] = projetoAtual;
            Session["periodoAtual"] = periodoAtual;
            Session["TODOS_PERIODOS"] = listaPeriodos;
            Session["CONSULTORES_DISPONIVEIS"] = consultoresDisponiveis;
            Session["TODOS_PROJETOS"] = listaProjetos;
            Session["LISTA_PERIODOS_INICIAL"] = listaPeriodoInicial;
            Session["LISTA_PERIODOS_FINAL"] = listaPeriodoFinal;
            Session["PERIODO_INCIAL"] = periodoInicial;
            Session["PERIODO_FINAL"] = periodoFinal;
            //Session["PARCEIRO_ATUAL"] = parceiroAtual;

            return View();
        }

        // GET: /InfoWS/ExportToExcel/1 => periodid
        public void ExportToExcelRelatorioMensalProjetos()
        {
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT))
            {
                var periodid = Request.Form["selectperiod"];
                var periodInicialId = Request.Form["selectperiodoinicial"];
                var periodFinalId = Request.Form["selectperiodoFinal"];
                var gestorAtual = usuarioLogado.Partner;
                var consultor = Request.Form["selectconsultor"];
                var projectID = Request.Form["selectprojeto"];
                List<Partners> consultoresDisponiveis = (List<Partners>)Session["CONSULTORES_DISPONIVEIS"];
                List<Project> listaProjetos = (List<Project>)Session["TODOS_PROJETOS"];

                Project projeto = null;
                if (projectID == "")
                {
                    projectID = null;
                }
                else if (projectID != null)
                {
                    projeto = listaProjetos.Where(p => p.PROJECTID == int.Parse(projectID)).SingleOrDefault();
                }

                if (periodInicialId == string.Empty)
                {
                    periodInicialId = null;
                }

                if (periodFinalId == string.Empty)
                {
                    periodFinalId = null;
                }

                Partners partner = consultoresDisponiveis.Where(p => p.NAME == consultor).SingleOrDefault();
                RelatoriosXLS xls = new RelatoriosXLS(periodid, projeto, partner, periodInicialId, periodFinalId);
                xls.Execute(this.HttpContext);
            }
        }

        public ActionResult getListPartersSelected(int id)
        {
            Debug.WriteLine("O id recebido foi:" + id);
            List<Partners> listPartners = ProjectDataAccess.GetConsultoresIdProjeto(id);
            Session["CONSULTORES_DISPONIVEIS"] = listPartners;
            return View();
        }

        public void ExportToExcelHourForProject()
        {
            //valida se tem permissao para entrar nessa tela
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT))
            {
                var periodid = Request.Form["selectperiod"];
                var periodInicialId = Request.Form["selectperiodoinicial"];
                var periodFinalId = Request.Form["selectperiodoFinal"];
                var gestorAtual = usuarioLogado.Partner;
                var consultor = Request.Form["selectconsultor"];
                var projectID = Request.Form["selectprojeto"];
                List<Partners> consultoresDisponiveis = (List<Partners>)Session["CONSULTORES_DISPONIVEIS"];
                List<Project> listaProjetos = (List<Project>)Session["TODOS_PROJETOS"];


                Project projeto = null;
                
                if (projectID != null && !projectID.Equals(string.Empty))
                {
                    projeto = listaProjetos.Where(p => p.PROJECTID == int.Parse(projectID)).SingleOrDefault();
                }

                Period periodoInicial = PeriodDataAccess.GetPeriodo(periodInicialId);
                Period periodoFinal = PeriodDataAccess.GetPeriodo(periodFinalId);

                Partners partner = consultoresDisponiveis.Where(p => p.NAME == consultor).SingleOrDefault();

                RelatorioProjetosXLS xls = new RelatorioProjetosXLS(periodoInicial, periodoFinal, projeto, partner);
                xls.Execute(this.HttpContext);
            }
        }

    }
}
