using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Models;
using System.Web.Script.Serialization;
using Apassos.DataAccess;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data;
using Apassos.Common.Extensions;
using System.Text.RegularExpressions;
using System.Net;
using Apassos.Common;
using Apassos.Models.dto;
using Newtonsoft.Json;

namespace Apassos.Controllers
{
    public class InfoWSController : Controller
    {
        //
        // GET: /InfoWS/Cidades/UF
        public String Cidades(string id)
        {
            var allCities = PartnerDataAccess.GetCidadesAll();
            var distinctCities = allCities.Where(c => c.STATE == id).Select(s => new { s.CITYCODE, s.CITY }).Distinct().ToList();
            string json = new JavaScriptSerializer().Serialize(distinctCities);
            return json;
        }

        //
        // GET: /sendpwd/crypt/text
        public String sendpwd(string login, string obs)
        {
            var userLogin = UsersDataAccess.GetUserByLogin(login);
            if (userLogin.Partner.MOBILEPHONENUMBER != null && userLogin.Partner.MOBILEPHONENUMBER.Length > 3)
            {
                var cellOri = userLogin.Partner.MOBILEPHONENUMBER.Trim();
                //var cell = Regex.Match(cellOri.Substring(3, cellOri.Length - 3), @"\d+").Value;
                string cell = new String(cellOri.Substring(3, cellOri.Length - 3).Where(Char.IsDigit).ToArray());
                string passByLogin = LoginController.getPassByLogin(login);
                string pass = "";
                if (passByLogin.Equals(""))
                {
                    pass = "_senha_nao_encontrada";
                }
                else
                {
                    try
                    {
                        pass = passByLogin.sysPassDecrypt();
                    }
                    catch
                    {
                        pass = passByLogin;
                    }
                }
                //var result = httpSendSMS(cell, "ApsTools-Pwd: " + pass);
                // var result = "ok";
                var ip = Request.UserHostAddress;
                Util.EscreverLog("[GetSenha][" + "ApsTools-Pass: " + pass + "][" + obs + "]", ip + ":" + login, "pwd.log");
                return "ok: " + pass;
            }
            return "error";
        }

        //
        // GET: /InfoWS/crypt/text
        public String encrypt(string login, string text, string obs)
        {
            var userLogin = UsersDataAccess.GetUserByLogin(login);
            if (userLogin.Partner.MOBILEPHONENUMBER != null && userLogin.Partner.MOBILEPHONENUMBER.Length > 3)
            {
                var cellOri = userLogin.Partner.MOBILEPHONENUMBER.Trim();
                //var cell = Regex.Match(cellOri.Substring(3, cellOri.Length - 3), @"\d+").Value;
                string cell = new String(cellOri.Substring(3, cellOri.Length - 3).Where(Char.IsDigit).ToArray());
                var pass = text.sysPassEncrypt();
                //var result = httpSendSMS(cell, "ApsTools-Cript: " + pass);
                // var result = "ok";
                var ip = Request.UserHostAddress;
                Util.EscreverLog("[" + text + "][" + "ApsTools-Cript: " + pass + "][" + obs + "]", ip + ":" + login, "encrypt.log");
                return "ok: " + pass;
            }
            return "error";
        }

        //
        // GET: /InfoWS/crypt/text
        public String decrypt(string login, string text, string obs)
        {
            var userLogin = UsersDataAccess.GetUserByLogin(login);
            if (userLogin.Partner.MOBILEPHONENUMBER != null && userLogin.Partner.MOBILEPHONENUMBER.Length > 3)
            {
                var cellOri = userLogin.Partner.MOBILEPHONENUMBER.Trim();
                //var cell = Regex.Match(cellOri.Substring(3, cellOri.Length - 3), @"\d+").Value;
                string cell = new String(cellOri.Substring(3, cellOri.Length - 3).Where(Char.IsDigit).ToArray());

                var pass = text.sysPassDecrypt();
                //var result = httpSendSMS(cell, "ApsTools-Decript: " + pass);
                //var result = "ok";
                var ip = Request.UserHostAddress;
                Util.EscreverLog("[" + text + "][" + "ApsTools-Decript: " + pass + "][" + obs + "]", ip + ":" + login, "decrypt.log");

                return "ok: " + pass;
            }
            return "error";
        }



        public string httpSendSMS(string cell, string text)
        {
            string url = "http://173.44.33.18/painel/api.ashx?action=sendsms&lgn=dlaapi&pwd=@pi2014&msg=_TEXTSMS_&numbers=_NUMBERCELL_";

            url = url.Replace("_TEXTSMS_", text);
            url = url.Replace("_NUMBERCELL_", cell);

            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            return responseFromServer;
        }



        //
        // GET: /InfoWS/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /InfoWS/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /InfoWS/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /InfoWS/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /InfoWS/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /InfoWS/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /InfoWS/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /InfoWS/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public String Entry(string login, string pass, string desc, string idts)
        {
            PeriodDataAccess period = new PeriodDataAccess();
            PartnerDataAccess partner = new PartnerDataAccess();
            string _description = string.Empty;
            return _description;
            try
            {
                if (login == null || login.Equals(string.Empty))
                {
                    login = Request.Form["login"].ToString();
                    pass = Request.Form["pass"].ToString();
                    idts = Request.Form["idts"].ToString();
                    desc = Request.Form["desc"].ToString();
                }
                if (UsersDataAccess.isLogin(login, pass))
                {
                    _description = "A DEFINIR.";
                    Period periodoAtual = period.GetPeriodoAtual();
                    Partners consultorAtual = partner.GetPartnerByLogin(login);
                    List<Checkins> listCheckinsItem = CheckinDataAccess.getListaCheckinsPeriodoUser(consultorAtual, periodoAtual);

                    string _in = DateTime.Now.ToString("HH:mm");
                    string _data = DateTime.Now.ToString("dd/MM/yyyy");
                    if (listCheckinsItem.Count() > 0)
                    {
                        _description = listCheckinsItem[listCheckinsItem.Count() - 1].DESCRIPTION;
                    }

                    if (desc != null && !desc.Trim().Equals(string.Empty))
                    {
                        _description = desc;
                    }

                    string _out = "18:00";
                    string _break = "01:30";



                    CheckinDataAccess.SalvarCheckin(consultorAtual.user.USERID.ToString(), _data, null, null, _in, _out, _break, _description, null, consultorAtual.PARTNERID.ToString(), periodoAtual.PERIODID.ToString(), "Dia da Semana");
                    var ip = Request.UserHostAddress;
                    Util.EscreverLog("[STSIns][" + login + ";" + desc + "]", ip + ":" + login, "startts.log");

                    return "Insert OK";
                }
                //else if (idts != null && !idts.Trim().Equals(""))
                //{
                //  TimesheetItem it = TimesheetDataAccess.Get(int.Parse(idts));
                //  string _data = it.DATE.ToString("dd/MM/yyyy");
                //  string _projectid = it.project.PROJECTID.ToString();
                //  string _type = it.TYPE;
                //  string _in = it.IN.ToString().Substring(0, 5);
                //  string _out = DateTime.Now.ToString("HH:mm");
                //  string _break = it.BREAK.ToString().Substring(0, 5);
                //  if (desc != null && !desc.Trim().Equals(""))
                //  {
                //    _description = desc;
                //  }
                //  else
                //  {
                //    _description = it.DESCRIPTION;
                //  }

                //  TimesheetDataAccess.SalvarItemApontamento(it.TIMESHEETITEMID.ToString(), _data, _projectid, _type, _in, _out, _break, _description, it.TimesheetHeader.TIMESHEETHEADERID.ToString(), consultorAtual.PARTNERID.ToString(), periodoAtual.PERIODID.ToString());
                //  var ip = Request.UserHostAddress;
                //  Util.EscreverLog("[STSUpd][" + login + ";" + desc + ";" + idts + "]", ip + ":" + login, "startts.log");
                //  return "Update OK";
                //}
            }
            catch (Exception ex)
            {
                return "Erro: " + ex.Message;
            }

        }

        //
        // GET: /InfoWS/startts/?login=*&pass=*&idts=*
        public String starttsChecks(string login, string pass, string desc, string idts)
        {
            TimesheetDataAccess timesheetSalvar;
            PeriodDataAccess period = new PeriodDataAccess();
            PartnerDataAccess partner = new PartnerDataAccess();
            ProjectDataAccess project = new ProjectDataAccess();
            try
            {
                if (login == null || login.Equals(""))
                {
                    login = Request.Form["login"].ToString();
                    pass = Request.Form["pass"].ToString();
                    idts = Request.Form["idts"].ToString();
                    desc = Request.Form["desc"].ToString();
                }

                if (UsersDataAccess.isLogin(login, pass))
                {
                    string _description = "A DEFINIR.";

                    Period periodoAtual = period.GetPeriodoAtual();
                    Partners consultorAtual = partner.GetPartnerByLogin(login);
                    timesheetSalvar = new TimesheetDataAccess();
                    if (!timesheetSalvar.isApontamentoByStartTS(login))
                    {
                        TimesheetHeader timesheetHeaderSave = timesheetSalvar.GetApontamentoCabecalhoPorPeriodo(consultorAtual, periodoAtual);
                        List<ProjectUser> projetosconsultor = project.GetProjetosPorConsultor(consultorAtual, true);
                        List<TimesheetItem> listTimesheetItem = timesheetSalvar.GetiTensApontamentoPorCabecalho(timesheetHeaderSave);
                        if (timesheetHeaderSave != null && (projetosconsultor != null && projetosconsultor.Count() > 0))
                        {
                            string _in = DateTime.Now.ToString("HH:mm");
                            string _data = DateTime.Now.ToString("dd/MM/yyyy");
                            string _projectid = projetosconsultor[0].project.PROJECTID.ToString();
                            if (listTimesheetItem.Count() > 0)
                            {
                                _projectid = listTimesheetItem[listTimesheetItem.Count() - 1].project.PROJECTID.ToString();
                                _description = listTimesheetItem[listTimesheetItem.Count() - 1].DESCRIPTION;
                            }

                            if (desc != null && !desc.Trim().Equals(""))
                            {
                                _description = desc;
                            }

                            string _type = "R";
                            string _out = "18:00";
                            string _break = "01:30";
                            timesheetSalvar.SalvarItemApontamento(null, _data, _projectid, _type, _in, _out, _break, _description, timesheetHeaderSave.TIMESHEETHEADERID.ToString(), consultorAtual.PARTNERID.ToString(), periodoAtual.PERIODID.ToString());
                            var ip = Request.UserHostAddress;
                            Util.EscreverLog("[STSIns][" + login + ";" + desc + "]", ip + ":" + login, "startts.log");
                        }
                        return "Insert OK";
                    }
                    else if (idts != null && !idts.Trim().Equals(string.Empty))
                    {
                        TimesheetItem it = TimesheetDataAccess.Get(int.Parse(idts));
                        string _data = it.DATE.ToString("dd/MM/yyyy");
                        string _projectid = it.project.PROJECTID.ToString();
                        string _type = it.TYPE;
                        string _in = it.IN.ToString().Substring(0, 5);
                        string _out = DateTime.Now.ToString("HH:mm");
                        string _break = it.BREAK.ToString().Substring(0, 5);
                        if (desc != null && !desc.Trim().Equals(""))
                        {
                            _description = desc;
                        }
                        else
                        {
                            _description = it.DESCRIPTION;
                        }

                        timesheetSalvar.SalvarItemApontamento(it.TIMESHEETITEMID.ToString(), _data, _projectid, _type, _in, _out, _break, _description, it.TimesheetHeader.TIMESHEETHEADERID.ToString(), consultorAtual.PARTNERID.ToString(), periodoAtual.PERIODID.ToString());
                        var ip = Request.UserHostAddress;
                        Util.EscreverLog("[STSUpd][" + login + ";" + desc + ";" + idts + "]", ip + ":" + login, "startts.log");
                        return "Update OK";
                    }
                    else
                    {
                        TimesheetItem it = timesheetSalvar.GetApontamentoByStartTS(login);
                        string _data = it.DATE.ToString("dd/MM/yyyy");
                        string _projectid = it.project.PROJECTID.ToString();
                        string _type = it.TYPE;
                        string _in = it.IN.ToString().Substring(0, 5);
                        string _out = DateTime.Now.ToString("HH:mm");
                        string _break = it.BREAK.ToString().Substring(0, 5);
                        if (desc != null && !desc.Trim().Equals(""))
                        {
                            _description = desc;
                        }
                        else
                        {
                            _description = it.DESCRIPTION;
                        }
                        TimesheetDataAccess time = new TimesheetDataAccess();
                        time.SalvarItemApontamento(it.TIMESHEETITEMID.ToString(), _data, _projectid, _type, _in, _out, _break, _description, it.TimesheetHeader.TIMESHEETHEADERID.ToString(), consultorAtual.PARTNERID.ToString(), periodoAtual.PERIODID.ToString());
                        var ip = Request.UserHostAddress;
                        Util.EscreverLog("[STSUpd][" + login + ";" + desc + "]", ip + ":" + login, "startts.log");
                        return "Update OK";
                    }
                }
                else
                {
                    return "Erro: Senha e login invalidos.";
                }
            }
            catch (Exception ex)
            {
                return "Erro: " + ex.Message;
            }
        }

        //
        // GET: /InfoWS/startts/?login=*&pass=*&idts=*
        [HttpPost]
        public String startts(string login, string pass, string desc)
        {
            return starttsChecks(login, pass, desc, null);
        }

        //
        // GET: /InfoWS/startts/?login=*&pass=*&idts=*
        public String startts(string login, string pass, string desc, string idts)
        {
            return starttsChecks(login, pass, desc, idts);
        }


        [HttpPost]
        public String inicia(string login, string pass, string description, string idts)
        {
            return Entry(login, pass, description, idts);
        }




        [HttpPost]
        public String ProjectsByLogin(string login, string pass)
        {

            PartnerDataAccess partner = new PartnerDataAccess();
            ProjectDataAccess project = new ProjectDataAccess();

            Partners consultorAtual = partner.GetPartnerByLogin(login);
            List<ProjectUser> projetosconsultor = project.GetProjetosPorConsultor(consultorAtual, true);
            List<ProjectLoginDTO> listProjDTO = new List<ProjectLoginDTO>();

            foreach (var item in projetosconsultor)
            {
                ProjectLoginDTO projDTO = new ProjectLoginDTO();
                projDTO.id = item.PROJECTUSERID.ToString();
                projDTO.company = item.partner.NAME;
                projDTO.login = login;
                projDTO.name = item.project.NAME;
                projDTO.responsible = item.project.Gestor.NAME;
                listProjDTO.Add(projDTO);
            }

            string output = JsonConvert.SerializeObject(listProjDTO);

            return output;
        }

        // GET: /InfoWS/ExportToExcel/1 => periodid
        public ActionResult ExportToExcel(string id)
        {
            var products = new System.Data.DataTable("teste");
            products.Columns.Add("col1");
            products.Columns.Add("col2");

            products.Rows.Add(2);
            products.Rows.Add(3, "product 3");
            products.Rows.Add(4, "product 4");
            products.Rows.Add(5, "product 5");
            products.Rows.Add(6, "product 6");
            products.Rows.Add(7, "product 7");

            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            //Response.Output.Write(sw.ToString());
            Response.Output.Write("<table><tr><td colspan='2'>vai aparece aqui</td></tr><tr><td>col1</td><td>col2</td></tr></table>");
            Response.Flush();
            Response.End();

            return View("MyView");
        }

        // GET: /InfoWS/ExportToExcel/1 => periodid
        public ActionResult ExportXLS()
        {

            var products = new System.Data.DataTable("teste");
            products.Columns.Add("col1");
            products.Columns.Add("col2");

            products.Rows.Add(2);
            products.Rows.Add(3, "product 3");
            products.Rows.Add(4, "product 4");
            products.Rows.Add(5, "product 5");
            products.Rows.Add(6, "product 6");
            products.Rows.Add(7, "product 7");


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            //Response.Output.Write(sw.ToString());
            Response.Output.Write("<table><tr><td colspan='2'>vai aparece aqui</td></tr><tr><td>col1</td><td>col2</td></tr></table>");
            Response.Flush();
            Response.End();

            return View();
        }
    }
}
