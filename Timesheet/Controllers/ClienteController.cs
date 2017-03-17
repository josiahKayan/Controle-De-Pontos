using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using Apassos.Common.Extensions;

namespace Apassos.Controllers
{
  public class ClienteController : TimesheetBaseController
  {

    //
    // GET: /Cliente/

    public ActionResult Index()
    {
      this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
      usuarioLogado = (Users)Session["_USUARIO_LOGADO"];

      if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PARTNERS))
      {
        return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
      }

      PartnerDataAccess parceiro = new PartnerDataAccess();

      var listaParceiros = parceiro.GetParceirosSistema();

      return View(listaParceiros);
    }


    //
    // GET: /Cliente/Create
    public ActionResult Create()
    {
      this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
      usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
      if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PARTNERS))
      {
        return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
      }
      Session["TODOS_PAISES"] = PartnerDataAccess.GetPaisesAll();
      Session["TODOS_ESTADOS"] = PartnerDataAccess.GetEstadosAll();
      //Session["TODOS_CIDADES"] = PartnerDataAccess.GetCidadesAll();
      return View();
    }

    //
    // POST: /Cliente/Create
    [HttpPost]
    public ActionResult Create(Partners partners)
    {
      this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
      usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
      if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PARTNERS))
      {
        return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
      }

      try
      {
        string message = "O parceiro foi salvo com sucesso!";

        var pais = Request.Form["selectcountry"];
        var cidade = Request.Form["selectcity"];
        var isuser = Request.Form["checkisusuario"];
        var type = Request.Form["selecttipo"];
        var perfil = Request.Form["selectperfil"];
        var loginusuario = Request.Form["loginusuario"];
        var grupo = Request.Form["grupo"];


        if (type == "1")  ///"J"
        {
          partners.FIRSTNAME = "";
          partners.LASTNAME = "";
        }

        partners.ENVIRONMENT = env;
        partners.COUNTRYID = pais;
        partners.CITYID = cidade;
        partners.CREATEDBY = usuarioLogado.USERNAME;
        partners.CREATIONDATE = DateTime.Now;
        partners.CHANGEDBY = usuarioLogado.USERNAME;
        partners.CHANGEDATE = DateTime.Now;
        partners.TYPE = Constants.GetTipoPessoaFJ(type);

        if (grupo == null || grupo == string.Empty)
        {


          partners.USERGROUP = ".";
        }
        else
        {
          partners.USERGROUP = grupo;
        }
        var isUserSN = "S";
        if (isuser != "S")
        {
          isUserSN = "N";
        }
        partners.ISUSER = isUserSN;
        db.Partners.Add(partners);
        db.SaveChanges();

        if (isUserSN == "S")
        {
                    UsersDataAccess userData = new UsersDataAccess();
          //grava novo usuario
          loginusuario = partners.SHORTNAME.Replace(" ", "").Replace("'", "").Replace("\"\"", "") + DateTime.Now.ToString("yyyyMMdd");
          string loginNovo = userData.CreateLogin(partners);
          Users itemSalvar = new Users
          {
            //inicialmente o usuario sera consultor, com um login criado automaticamente
            CHANGEDATE = DateTime.Now,
            CHANGEDBY = usuarioLogado.USERNAME,
            CREATEDBY = usuarioLogado.USERNAME,
            CREATIONDATE = DateTime.Now,
            ENVIRONMENT = env,
            LASTLOGONDATE = null,
            LOCKED = "N",
            PARTNERID = partners.PARTNERID,
            PASSWORD = userData.SenhaDefault(loginNovo),
            PROFILE = ((int)Constants.ProfileConstant.CONSULTOR).ToString(),
            USERNAME = loginNovo,      //partners.SHORTNAME.Replace(" ", "").Replace("'", "").Replace("\"\"", ""),   // + DateTime.Now.ToString("yyyyMMdd"),
            VALIDFROM = DateTime.Now,
            VALIDTO = new DateTime(2020, 12, 31)
          };
          db.Users.Add(itemSalvar);
          db.SaveChanges();
          message = message + " Login criado: " + itemSalvar.USERNAME;
        }

        Session["_SUCCESS_"] = "true";
        Session["_MENSAGEM_"] = message;

        return RedirectToAction("Index");

      }
      catch (DbEntityValidationException ex)
      {
        List<string> errorMessages = new List<string>();
        foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
        {
          string entityName = validationResult.Entry.Entity.GetType().Name;
          foreach (DbValidationError error in validationResult.ValidationErrors)
          {
            errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
          }
        }

        var fullErrorMessage = string.Join("; ", errorMessages);

        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        Console.WriteLine(exceptionMessage);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Falha na transação. Verifique os campos obrigatórios ou se o parceiro já foi cadastrado.";
        Util.EscreverLog(exceptionMessage, usuarioLogado);
        return RedirectToAction("Index", "Login", new { erro = true });
      }
      catch (Exception e)
      {
        Util.EscreverLog(e.Message, usuarioLogado);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Aconteceu uma falha ao salvar o parceiro Tente novamente. Se o problema persistir, informe ao suporte do sistema!";
      }
      return View();

    }




    //
    // GET: /Cliente/Edit/5
    public ActionResult Edit(string id)
    {
      this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
      usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
      if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PARTNERS))
      {
        return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
      }
      Partners partners = db.Partners.Find(int.Parse(id));
      Session["TODOS_PAISES"] = PartnerDataAccess.GetPaisesAll();
      Session["TODOS_ESTADOS"] = PartnerDataAccess.GetEstadosAll();

      //Session["TODOS_CIDADES"] = PartnerDataAccess.GetCidadesAll();
      return View(partners);
    }

    //
    // POST: /Cliente/Edit/5
    [HttpPost]
    public ActionResult Edit(Partners partners)
    {

      this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
      usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
      if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PARTNERS))
      {
        return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
      }

      try
      {
        string message = "O parceiro foi salvo com sucesso!";

        var pais = Request.Form["selectcountry"];
        var cidade = Request.Form["selectcity"];
        var isuser = Request.Form["checkisusuario"];
        var type = Request.Form["selecttipo"];
        var perfil = Request.Form["selectperfil"];
        var loginusuario = Request.Form["loginusuario"];

        partners.ENVIRONMENT = env;
        partners.COUNTRYID = pais;
        partners.CITYID = cidade;
        partners.CREATEDBY = usuarioLogado.USERNAME;
        partners.CREATIONDATE = DateTime.Now;
        partners.CHANGEDBY = usuarioLogado.USERNAME;
        partners.CHANGEDATE = DateTime.Now;
        partners.TYPE = Constants.GetTipoPessoaFJ(type);
        if (partners.TYPE == "1")  ///J"
        {
          partners.FIRSTNAME = partners.SHORTNAME;
          partners.LASTNAME = ".";
        }

        db.Entry(partners).State = EntityState.Modified;

        var isUserSN = "S";
        if (isuser != "S")
        {
          isUserSN = "N";
        }
        partners.ISUSER = isUserSN;
        db.SaveChanges();

        if (isUserSN == "S")
        {

          if (partners.user == null || partners.user.USERID <= 0)
          {
                        UsersDataAccess userData = new UsersDataAccess();
            loginusuario = partners.SHORTNAME.Replace(" ", "").Replace("'", "").Replace("\"\"", "") + DateTime.Now.ToString("yyyyMMdd");
            string loginNovo = userData.CreateLogin(partners);
            //grava novo usuario
            Users itemSalvar = new Users
            {
              //inicialmente o usuario sera consultor, com um login criado automaticamente
              CHANGEDATE = DateTime.Now,
              CHANGEDBY = usuarioLogado.USERNAME,
              CREATEDBY = usuarioLogado.USERNAME,
              CREATIONDATE = DateTime.Now,
              ENVIRONMENT = env,
              LASTLOGONDATE = null,
              LOCKED = "N",
              PARTNERID = partners.PARTNERID,
              PASSWORD = userData.SenhaDefault(loginNovo),
              PROFILE = ((int)Constants.ProfileConstant.CONSULTOR).ToString(),
              USERNAME = loginNovo,   //partners.SHORTNAME.Replace(" ", "").Replace("'", "").Replace("\"\"", ""),  // + DateTime.Now.ToString("yyyyMMdd"),
              VALIDFROM = DateTime.Now,
              VALIDTO = new DateTime(2020, 12, 31)
            };
            Util.EscreverLog("User: " + loginusuario + " / CreatedBy: " + usuarioLogado.USERNAME, usuarioLogado);
            db.Users.Add(itemSalvar);
            db.SaveChanges();
            message = message + " Login criado: " + itemSalvar.USERNAME;
          }

        }
        else
        {
          if (partners.UserForced != null && partners.UserForced.USERID > 0)
          {
            //atualiza
            Users itemAlterar = db.Users.Find(partners.UserForced.USERID);
            itemAlterar.CHANGEDATE = DateTime.Now;
            itemAlterar.CHANGEDBY = usuarioLogado.USERNAME;
            itemAlterar.LOCKED = "S";
            db.Entry(itemAlterar).State = EntityState.Modified;
            Util.EscreverLog("User: " + itemAlterar.USERNAME + " / locked: " + itemAlterar.LOCKED + " ChangedBy: " + itemAlterar.CHANGEDBY, usuarioLogado);
            db.SaveChanges();
          }
        }


        Session["_SUCCESS_"] = "true";
        Session["_MENSAGEM_"] = message;
        return RedirectToAction("Index");
      }
      catch (DbEntityValidationException ex)
      {
        List<string> errorMessages = new List<string>();
        foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
        {
          string entityName = validationResult.Entry.Entity.GetType().Name;
          foreach (DbValidationError error in validationResult.ValidationErrors)
          {
            errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
          }
        }

        var fullErrorMessage = string.Join("; ", errorMessages);

        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        Console.WriteLine(exceptionMessage);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Falha na transação. Verifique os campos obrigatórios ou se o parceiro já foi cadastrado.";
      }
      catch (Exception e)
      {
        Util.EscreverLog(e.Message, usuarioLogado);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Aconteceu uma falha ao salvar o parceiro. Tente novamente. Se o problema persistir, informe ao suporte do sistema!";
      }

      return View(partners);

    }

    [HttpPost]
    public ActionResult Excluir()
    {
      this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
      usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
      try
      {
        var checados = Request.Form["checados"];
        string[] idsX = checados.Split(',');
        foreach (string id in idsX)
        {
          Partners cliente = db.Partners.Find(int.Parse(id));
          Util.EscreverLog("Exclusao: " + cliente.NAME + " id: " + cliente.PARTNERID, usuarioLogado);
          if (cliente.ISUSER == "S")
          {
            //se for usuario, exclui o usuario antes
            if (cliente.user != null)
            {
              Users userX = db.Users.Find(cliente.user.USERID);
              Util.EscreverLog("Exclusao: " + cliente.NAME + "nmu: " + userX.USERNAME + " idu: " + userX.USERID, usuarioLogado);
              db.Users.Remove(userX);
            }
          }
          Util.EscreverLog("Concluindo Exclusao: " + cliente.NAME + " id: " + cliente.PARTNERID, usuarioLogado);
          db.Partners.Remove(cliente);
        }
        db.SaveChanges();
        Session["_SUCCESS_"] = "true";
        Session["_MENSAGEM_"] = "Exclusao realizada com sucesso.";
        Util.EscreverLog("Sucesso Exclusao.", usuarioLogado);
      }
      catch (Exception e)
      {
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Ocorreu uma falha durante o processo de exclusão. Verifique se alguns desses parceiros estão sendo usados.";
        Util.EscreverLog("Erro exclusao:" + e.Message, usuarioLogado);
      }
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}