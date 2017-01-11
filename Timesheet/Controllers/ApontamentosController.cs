using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using Apassos.reports.classes;
using System.Diagnostics;
using System.Text;

namespace Apassos.Controllers
{
    public class ApontamentosController : TimesheetBaseController
    {


        // GET: /Apontamentos/
        public TimesheetHeader apontamentocabecalho;

        public ActionResult Index()
        {
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.TIMESHEET))
            {
                var consultorAtual = usuarioLogado.Partner;



                var _periodid = Request.Form["selectperiodo"];
                Period periodoAtual = null;
                if (_periodid != null && _periodid != "")
                {
                    periodoAtual = PeriodDataAccess.GetPeriodo(_periodid);
                }
                else
                {
                    periodoAtual = PeriodDataAccess.GetPeriodoAtual();
                }

                apontamentocabecalho = TimesheetDataAccess.GetApontamentoCabecalhoPorPeriodo(consultorAtual, periodoAtual);
                List<TimesheetItem> apontamentoitens = null;
                if (apontamentocabecalho != null)
                {
                    apontamentoitens = TimesheetDataAccess.GetiTensApontamentoPorCabecalho(apontamentocabecalho);
                }
                else
                { //salva o novo cabecalho
                    apontamentocabecalho = new TimesheetHeader();
                    apontamentocabecalho.ENVIRONMENT = env;
                    apontamentocabecalho.Partner = consultorAtual;
                    apontamentocabecalho.Period = periodoAtual;
                    apontamentoitens = new List<TimesheetItem>();
                }

                var grupo = consultorAtual.USERGROUP;

                var projetosconsultor = ProjectDataAccess.GetProjetosPorConsultor(consultorAtual, true);

                Session["CONSULTOR_ATUAL"] = consultorAtual;
                Session["PERIODO_ATUAL"] = periodoAtual;
                Session["PERIODO_ATUAL_DATAS"] = PeriodDataAccess.GetListDate(periodoAtual);
                Session["TODOS_PERIODOS"] = PeriodDataAccess.GetPeriodoAll();

                Session["CONSULTOR_APONTAMENTO_CABECALHO_ATUAL"] = apontamentocabecalho;
                Session["CONSULTOR_APONTAMENTOS_ITENS_ATUAL"] = apontamentoitens;
                Session["CONSULTOR_PROJETOS"] = projetosconsultor;
                Session["GRUPO"] = grupo;
                Session["TIPOS_APONTAMENTOS"] = Enum.GetValues(typeof(Apassos.Common.Constants.TipoApontamentoConstant));
                Session["INFO_APONTAMENTOS_PERIODO"] = new PartnersTimesheetHeaderAccess(consultorAtual, periodoAtual, periodoAtual);
                Session["_USUARIO_LOGADO"] = usuarioLogado;

            }

            return View();
        }

        // Post: /Login/Acessar?loginid=x&pass=y
        [HttpPost]
        public ActionResult Salvar()
        {
            //TimeSpan almoco = new TimeSpan(1, 30, 0);
            Session["_MENSAGEM_"] = "";
            var totalupdates = int.Parse(Request.Form["totalupdates"]);
            string timesheetheaderid = Request.Form["timesheetheaderid"];
            string idsexcluir = Request.Form["idsexcluir"];
            string consultorid = Request.Form["consultorid"];
            string periodoid = Request.Form["selectperiodo"];

            Period periodoComp = PeriodDataAccess.GetPeriodo(periodoid);




            if (periodoComp.STATUS == "f")
            {

                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = " Mês de Apontamento já fechado !!!";
                return RedirectToAction("Index");

            }
            else
            {



                for (int contup = 1; contup <= totalupdates; contup++)
                {

                    //valida se o item para update esta marcado para exclusao
                    string _id = Request.Form["idapont_" + contup];

                    if (idsexcluir.IndexOf("," + _id + ",") < 0)
                    {
                        string _isread = Request.Form["idapont_mode_" + contup];
                        //altera somente os apontamentos que estejam habilitados para edicao
                        if (!_isread.ToLower().Equals("true"))
                        {

                            string _data = Request.Form[contup + "_selectdata"];
                            string _projectid = Request.Form[contup + "_selectprojeto"];
                            string _type = Request.Form[contup + "_selecttipoentrada"];
                            string _in = Request.Form[contup + "entrada_"];
                            string _out = Request.Form[contup + "saida_"];
                            //string _break = almoco.ToString();
                            string _break = Request.Form[contup + "intervalo_"];
                            string _description = Request.Form[contup + "observacao_"];
                            string _hash = Request.Form[contup + "_hash"];

                            StringBuilder stringHashObject = new StringBuilder(_data);
                            stringHashObject.Append(_projectid);
                            stringHashObject.Append(_type);
                            stringHashObject.Append(_in);
                            stringHashObject.Append(_out);
                            stringHashObject.Append(_break);
                            stringHashObject.Append(_description);

                            int newHash = stringHashObject.GetHashCode();
                            int oldHash = int.Parse(_hash);
                            if (!oldHash.Equals(newHash))
                            {
                                TimesheetDataAccess.SalvarItemApontamento(_id, _data, _projectid, _type, _in, _out, _break, _description, timesheetheaderid, consultorid, periodoid);
                            }
                        }
                    }

                }

                var totalinsert = int.Parse(Request.Form["containsert"]);

                for (int contins = 1; contins <= totalinsert; contins++)
                {


                    string _data = Request.Form["_selectdata_insert_" + contins];
                    string _projectid = Request.Form["_selectprojeto_insert_" + contins];
                    string _type = Request.Form["_selecttipoentrada_insert_" + contins];
                    string _in = Request.Form["entrada_insert_" + contins];
                    string _out = Request.Form["saida_insert_" + contins];
                    string _break = Request.Form["intervalo_insert_" + contins];
                    string _description = Request.Form["observacao_insert_" + contins];

                    TimesheetDataAccess.SalvarItemApontamento(null, _data, _projectid, _type, _in, _out, _break, _description, timesheetheaderid, consultorid, periodoid);

                }

                //exlclui os apontamentos marcados
                if (idsexcluir.Trim().Length > 0)
                {
                    string[] arrayIdsExcluir = idsexcluir.Split(',');
                    for (int contidx = 0; contidx < arrayIdsExcluir.Length; contidx++)
                    {
                        TimesheetDataAccess.ExcluirItemApontamento(arrayIdsExcluir[contidx]);
                    }
                }

                Session["_SUCCESS_"] = "true";
                Session["_MENSAGEM_"] = "Os apontamentos foram salvos com sucesso!";

                return RedirectToAction("Index", "Apontamentos");
            }
        }

        [HttpPost]
        public ActionResult Atualizar(int id)
        {
            TimesheetDataAccess.TimesheetItemAtualizar(id);
            return RedirectToAction("Index", "Apontamentos");
        }

        // GET: /InfoWS/ExportToExcel/1 => periodid

        public void ExportToExcel()
        {
            //valida se tem permissao para entrar nessa tela
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.TIMESHEETINPERIODREPORT))
            {
                var periodid = Request.Form["selectperiodo"];
                var consultorAtual = usuarioLogado.Partner;
                ApontamentosXLS xls = new ApontamentosXLS(periodid, consultorAtual);
                xls.Execute(this.HttpContext);
            }
        }


    }
}
