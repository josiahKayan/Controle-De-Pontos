using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Apassos.Models;
using Apassos.Common;
using Apassos.DataAccess;
using System.Diagnostics;
using System.Text;

namespace Apassos.Controllers
{
    public class CheckinController : TimesheetBaseController
    {


        // GET: /Apontamentos/
        public TimesheetHeader apontamentocabecalho;
        private List<Checkins> listaCheckins;

        public ActionResult Checkin()
        {
            if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.CHECKIN))
            {



                Partners consultorAtual = usuarioLogado.Partner;

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



                listaCheckins = CheckinDataAccess.getListaCheckinsPeriodoUser(consultorAtual, periodoAtual);



                Session["ATUAL_CONSULTOR"] = consultorAtual;
                Session["ATUAL_PERIODO"] = periodoAtual;
                Session["DATAS_PERIODO_ATUAL"] = PeriodDataAccess.GetListDate(periodoAtual);
                Session["PERIODOS_TODOS"] = PeriodDataAccess.GetPeriodoAll();
                Session["INFO_APONTAMENTOS_NO_PERIODO"] = new PartnersTimesheetHeaderAccess(consultorAtual, periodoAtual, periodoAtual);
                Session["TODOS_CHECKINS"] = listaCheckins;

                Session["_USUARIO_LOGADO"] = usuarioLogado;
            }

            return View();
        }


        [HttpPost]
        public ActionResult Salvar()
        {

            Session["_MENSAGEM_"] = "";
            TimeSpan almoco = new TimeSpan(1, 30, 0);
            var totalupdates = int.Parse(Request.Form["totalupdates"]);
            string timesheetheaderid = Request.Form["timesheetheaderid"];
            string idsexcluir = Request.Form["idsexcluir"];
            string consultorid = Request.Form["consultorid"];
            string periodoid = Request.Form["selectperiodo"];

            Period periodoComp = PeriodDataAccess.GetPeriodo(periodoid);




            if ( periodoComp.STATUS == "f")
            {

                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = " Mês de Check-In já fechado !!!";
                return RedirectToAction("Checkin", "Checkin");

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
                        //if (!_isread.ToLower().Equals("true"))
                        //{

                        string _data = Request.Form[contup + "_selectdata"];
                        string _projectid = Request.Form[contup + "_selectprojeto"];
                        string _type = Request.Form[contup + "_selecttipoentrada"];
                        string _in = Request.Form[contup + "entrada_"];
                        string _out = Request.Form[contup + "saida_"];
                        string _break = almoco.ToString();
                        string _description = Request.Form[contup + "observacao_"];
                        string _hash = Request.Form[contup + "_hash"];
                        string _dataDescription = Request.Form[contup + "_selectDescription"];


                        StringBuilder stringHashObject = new StringBuilder(_data);
                        stringHashObject.Append(_projectid);
                        stringHashObject.Append(_type);
                        stringHashObject.Append(_in);
                        stringHashObject.Append(_out);
                        stringHashObject.Append(_break);
                        stringHashObject.Append(_description);

                        int newHash = stringHashObject.GetHashCode();
                        int oldHash = int.Parse(_hash);
                        //me perdi aqui!!
                        if (!oldHash.Equals(newHash))
                        {
                            //TimesheetDataAccess.SalvarItemApontamento(_id, _data, _projectid, _type, _in, _out, _break, _description, timesheetheaderid, consultorid, periodoid);
                            CheckinDataAccess.SalvarCheckin(_id, _data, _projectid, _type, _in, _out, _break, _description, timesheetheaderid, consultorid, periodoid, _dataDescription);
                        }
                        //}
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
                    string _break = almoco.ToString();
                    string _description = Request.Form["observacao_insert_" + contins];
                    string _dataDescription = Request.Form["selectDescricaoData_insert_" + contins];

               CheckinDataAccess.SalvarCheckin(null, _data, _projectid, _type, _in, _out, _break, _description, timesheetheaderid, consultorid, periodoid, _dataDescription);

                }

                //exlclui os apontamentos marcados
                if (idsexcluir.Trim().Length > 0)
                {
                    string[] arrayIdsExcluir = idsexcluir.Split(',');
                    for (int contidx = 0; contidx < arrayIdsExcluir.Length; contidx++)
                    {
                        //TimesheetDataAccess.ExcluirItemApontamento(arrayIdsExcluir[contidx]);
                        CheckinDataAccess.ExcluirCheckin(arrayIdsExcluir[contidx]);
                    }
                }

                Session["_SUCCESS_"] = "true";
                Session["_MENSAGEM_"] = "Check-in realizado com sucesso!";
                return RedirectToAction("Checkin", "Checkin");
            }



        }


    }
}
