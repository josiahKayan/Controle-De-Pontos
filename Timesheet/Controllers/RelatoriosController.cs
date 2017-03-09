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
            PeriodDataAccess period = new PeriodDataAccess();
            PartnerDataAccess partner = new PartnerDataAccess();
            ProjectDataAccess projectData = new ProjectDataAccess();
            List<Period> listaPeriodoInicial = new List<Period>();
            Period periodoInicial = null;
            listaPeriodoInicial = period.GetPeriodoAll();
            List<Period> listaPeriodoFinal = new List<Period>();
            listaPeriodoFinal = period.GetPeriodoAll();
            Period periodoFinal = null;

            var _idPeriodoInicial = Request.Form["selectperiodoinicial"];
            if (_idPeriodoInicial != null && _idPeriodoInicial != null)
            {
                periodoInicial = period.GetPeriodo(_idPeriodoInicial);
            }
            else
            {
                periodoInicial = period.GetPeriodoAtual();
            }


            var _idPeriodoFinal = Request.Form["selectperiodoFinal"];
            if (_idPeriodoFinal != null && _idPeriodoFinal != null)
            {
                periodoFinal = period.GetPeriodo(_idPeriodoFinal);
            }
            else
            {
                periodoFinal = period.GetPeriodoAtual();
            }





            Period periodoAtual = null;
            var _periodid = Request.Form["selectperiod"];

            if (_periodid != null && _periodid != "")
            {
                periodoAtual = period.GetPeriodo(_periodid);
            }
            else
            {
                periodoAtual = period.GetPeriodoAtual();
            }

            List<Project> listaProjetos = new List<Project>();

            var _projectId = Request.Form["selectprojeto"];
            listaProjetos.AddRange(projectData.GetProjetosAll());

            listaProjetos.OrderBy(x => x.NAME);

            List<Period> listaPeriodos = period.GetPeriodoAll();

            List<Partners> consultoresDisponiveis = partner.GetAllParceiros();

            Project projetoAtual = null;

            if (_projectId != null && _projectId != string.Empty)
            {
                projetoAtual = projectData.GetProjeto(_projectId);
            }
            if (projetoAtual != null)
            {
                consultoresDisponiveis = projectData.GetConsultoresProjeto(projetoAtual);
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
            ProjectDataAccess projectData = new ProjectDataAccess();

            Debug.WriteLine("O id recebido foi:" + id);
            List<Partners> listPartners = projectData.GetConsultoresIdProjeto(id);
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
                PeriodDataAccess period = new PeriodDataAccess();

                Project projeto = null;
                
                if (projectID != null && !projectID.Equals(string.Empty))
                {
                    projeto = listaProjetos.Where(p => p.PROJECTID == int.Parse(projectID)).SingleOrDefault();
                }

                Period periodoInicial = period.GetPeriodo(periodInicialId);
                Period periodoFinal = period.GetPeriodo(periodFinalId);

                Partners partner = consultoresDisponiveis.Where(p => p.NAME == consultor).SingleOrDefault();

                RelatorioProjetosXLS xls = new RelatorioProjetosXLS(periodoInicial, periodoFinal, projeto, partner);
                xls.Execute(this.HttpContext);
            }
        }

    }
}
