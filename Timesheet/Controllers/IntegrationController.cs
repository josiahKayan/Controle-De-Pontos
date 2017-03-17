using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Log;
using Apassos.Models;
using Apassos.Observer;
using Apassos.reports;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Apassos.Controllers
{
    public class IntegrationController : TimesheetBaseController
    {

        // GET: /Apontamentos/
        public TimesheetHeader apontamentocabecalho;

        public ActionResult Integration()
        {
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.INTEGRATION))
            {
                var consultorAtual = usuarioLogado.Partner;
                List<Logs> logs;
                //Parte para selecionar o Período
                PeriodDataAccess period = new PeriodDataAccess();

                var _status = Request.Form["selectStatus"];
                if (_status == null)
                {
                    _status = "-1";
                }

                var _periodid = Request.Form["selectperiodo"];
                Period periodoAtual = null;

                int statusInt = int.Parse(_status);

                if (_periodid != null && _periodid != "")
                {
                    periodoAtual = period.GetPeriodo(_periodid);
                    LogDataAccess logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriod(int.Parse(_periodid));
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }
                else
                {
                    periodoAtual = period.GetPeriodoAtual();
                    LogDataAccess logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriod(periodoAtual.PERIODID);
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }

                PartnerDataAccess partnerDataAccess = new PartnerDataAccess();
                List<Partners> partnersList = new List<Partners>();
                foreach (var item in logs)
                {
                    partnersList.Add(partnerDataAccess.GetParceirosById(item.ConsultorId));
                }
                //

                //Selecionar o parceiro
                PartnerDataAccess partnerData = new PartnerDataAccess();

                List<Partners> partnersListFilter = new List<Partners>();

                string _parceiroid = Request.Form["selectParceiro"];

                Partners parceiroAtual = null;

                if (_parceiroid != null && _parceiroid != "")
                {
                    parceiroAtual = partnerData.GetParceiroId(_parceiroid);
                    LogDataAccess logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriodAndPartner(int.Parse(_periodid), int.Parse(_parceiroid));
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }
                else
                {
                    parceiroAtual = null;
                    LogDataAccess logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriod(periodoAtual.PERIODID);
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }
                //
                partnersListFilter = partnerData.GetParceirosSistemaSemRepeticao();




                Session["PERIODO_ATUAL"] = periodoAtual;
                Session["PERIODO_ATUAL_DATAS"] = period.GetListDate(periodoAtual);
                Session["TODOS_PERIODOS"] = period.GetPeriodoAll();
                Session["_USUARIO_LOGADO"] = usuarioLogado;
                Session["LISTA_LOGS"] = logs;
                Session["PARCEIROS_LOGS"] = partnersList;
                Session["PARCEIROS_FILTRO"] = partnersListFilter;
                Session["PARCEIRO_ATUAL"] = parceiroAtual;
                Session["STATUS_ATUAL"] = _status;
            }

            return View();
        }

        public ActionResult MudarPeriodo()
        {
            var _periodid = Request.Form["selectperiodo"];
            return RedirectToAction("Integration", "Integration");
        }

        public void ExportToExcel()
        {
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT))
            {

                var consultorAtual = usuarioLogado.Partner;
                List<Logs> logs;
                //Parte para selecionar o Período
                PeriodDataAccess period = new PeriodDataAccess();
                LogDataAccess logDataAccess;

                var _status = Request.Form["selectStatus"];
                if (_status == null)
                {
                    _status = "-1";
                }

                var _periodid = Request.Form["selectperiodo"];
                Period periodoAtual = null;

                int statusInt = int.Parse(_status);

                if (_periodid != null && _periodid != "")
                {
                    periodoAtual = period.GetPeriodo(_periodid);
                    logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriod(int.Parse(_periodid));
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }
                else
                {
                    periodoAtual = period.GetPeriodoAtual();
                    logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriod(periodoAtual.PERIODID);
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }

                PartnerDataAccess partnerDataAccess = new PartnerDataAccess();
                List<Partners> partnersList = new List<Partners>();
                foreach (var item in logs)
                {
                    partnersList.Add(partnerDataAccess.GetParceirosById(item.ConsultorId));
                }
                //

                //Selecionar o parceiro
                PartnerDataAccess partnerData = new PartnerDataAccess();

                List<Partners> partnersListFilter = new List<Partners>();

                string _parceiroid = Request.Form["selectParceiro"];

                Partners parceiroAtual = null;

                if (_parceiroid != null && _parceiroid != "")
                {
                    parceiroAtual = partnerData.GetParceiroId(_parceiroid);
                    logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriodAndPartner(int.Parse(_periodid), int.Parse(_parceiroid));
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }
                else
                {
                    parceiroAtual = null;
                    logDataAccess = new LogDataAccess();
                    logs = logDataAccess.GetLogByPeriod(periodoAtual.PERIODID);
                    if (statusInt > 0)
                    {
                        logs = logs.Where(sa => sa.Status == statusInt).ToList();
                    }
                }
                //
                partnersListFilter = partnerData.GetParceirosSistemaSemRepeticao();



                string fileName = Path.GetTempPath()+"\\Integration.xls";


                List<EndLog> endLog ;

                endLog = GetEndLog(logs);

                Export.ToExcel(endLog, true, fileName, false);
                

                FileInfo fileInfo = new FileInfo(fileName);

                if (fileInfo.Exists)
                {
                    HttpResponseBase response = HttpContext.Response;

                    response.Clear();
                    response.ClearHeaders();
                    response.ClearContent();
                    response.AddHeader("content-disposition", "attachment; filename=" + fileInfo.Name);
                    response.AddHeader("Content-Type", "application/Excel");
                    response.ContentType = "application/vnd.ms-excel";
                    response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    response.WriteFile(fileInfo.FullName);
                    response.End();
                }

            }
        }

        public List<EndLog> GetEndLog(List<Logs> log)
        {
            List<EndLog> endLogs = new List<EndLog>();
            EndLog endLog;

            PartnerDataAccess partnerData = new PartnerDataAccess();

            foreach (var item in log)
            {
                endLog = new EndLog();

                endLog.Partner = partnerData.GetParceiroId(""+item.ConsultorId).NAME;
                endLog.ActivityDate = item.ActivityDate;
                endLog.ProjectTW = item.ProjectTW;
                endLog.Description = item.ActivityDescription;

                if (item.Status == 1)
                {
                    endLog.Status = "Ok";
                }
                else
                {
                    endLog.Status = "Erro";
                }

                endLog.Log = item.Description;
                endLog.NoTag = item.TagProblem;
                endLog.NoDescription = item.DescriptionProblem;

                endLogs.Add(endLog);
            }

            return endLogs;
        }


    }
}
