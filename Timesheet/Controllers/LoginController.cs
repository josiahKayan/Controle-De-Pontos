using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.Models;
using Apassos.Common.Extensions;
using Apassos.DataAccess;
using System.Diagnostics;

namespace Apassos.Controllers
{
  public class LoginController : Controller
  {


    // GET: /Login/
    public ActionResult Index(bool erro = false)
    {
      Session["_USUARIO_LOGADO"] = "";
      if (!erro)
      {
        Session["_SUCCESS_"] = "";
        Session["_MENSAGEM_"] = "";
      }
      return View();
    }

    // Post: /Login/Acessar?loginid=x&pass=y
    [HttpPost]
    public ActionResult Acessar(string loginid, string password)
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                try
                {
                    //causa excecao se falhar
                    this.ValidaLogin(loginid, password);
                    Users usuarioLogado = getUserByLogin(loginid, password);

                    //db.Entry(usuarioLogado).Reload();

                    //causa excecao se falhar
                    ValidaDatasAcesso(usuarioLogado);
                    //causa excecao se falhar
                    ValidaAcessoBloqueado(usuarioLogado);

                    usuarioLogado = db.Users.Find(usuarioLogado.USERID);
                    Session["_USUARIO_LOGADO"] = usuarioLogado;
                    usuarioLogado.LASTLOGONDATE = DateTime.Now;
                    db.SaveChanges();
                    ViewBag.loginerror = "false";
                    Session["_SUCCESS_"] = "";
                    Session["_MENSAGEM_"] = "";
                    var defaultProfile = AccessRules.DefaultProfile(usuarioLogado);
                    return RedirectToAction(defaultProfile[0][3], defaultProfile[0][2]);

                }
                catch (AcessosException aex)
                {
                    ViewBag.loginerror = "true";
                    Session["_SUCCESS_"] = "false";
                    Session["_MENSAGEM_"] = aex.Message;
                    return RedirectToAction("Index", "Login", new { erro = true });
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
                    Session["_MENSAGEM_"] = "Falha na transação. Tente novamente.";
                    return RedirectToAction("Index", "Login", new { erro = true });
                }
            }
    }

    //
    // Post: /Login/Acessar?loginid=x&pass=y
    [HttpPost]
    public ActionResult Sair()
    {
      ViewBag.loginerror = "false";
      Session["_USUARIO_LOGADO"] = "";
      return RedirectToAction("Index", "Login");
    }


    private bool ValidaLogin(string loginid, string password)
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var userLoginPass = (loginid.ToLower() + ":" + password.ToLower()).sysPassEncrypt();
                var user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                    Equals(loginid.Trim().ToLower()) && u.PASSWORD.Trim().ToLower().Equals(userLoginPass.Trim().ToLower())).FirstOrDefault();
                if (user == null)
                {
                    user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                        Equals(loginid.Trim().ToLower()) && u.PASSWORD.Trim().ToLower().Equals(password.Trim().ToLower())).FirstOrDefault();
                }
                if (user == null)
                {


                    throw new AcessosException("login e senha não encontrados. Verifique os dados informados e tente novamente!");
                    //Debug.WriteLine("Lançou a exception!!");

                }
                return true;
            }
    }

    private bool ValidaDatasAcesso(Users usuarioLogado)
    {
      List<DateTime> listD = new List<DateTime>();
      listD.Add(DateTime.Now);
      if (!Util.DatesInRange(listD, Convert.ToDateTime(usuarioLogado.VALIDFROM), Convert.ToDateTime(usuarioLogado.VALIDTO)))
      {
        throw new AcessosException("A data atual está fora das datas permitidas para o usuário acessar o sistema!");
      }
      return true;
    }

    private bool ValidaAcessoBloqueado(Users usuarioLogado)
    {
      if (usuarioLogado.LOCKED == "S")
      {
        throw new AcessosException("Acesso negado!");
      }
      return true;
    }

    public Users getUserByLogin(string loginid, string password)
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var userLoginPass = (loginid.ToLower() + ":" + password.ToLower()).sysPassEncrypt();
                var user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                    Equals(loginid.Trim().ToLower()) && u.PASSWORD.Trim().ToLower().Equals(userLoginPass.Trim().ToLower())).FirstOrDefault();
                if (user == null)
                {
                    user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                        Equals(loginid.Trim().ToLower()) && u.PASSWORD.Trim().ToLower().Equals(password.Trim().ToLower())).FirstOrDefault();
                }
                //resolvendo problema de lazy
                Partners partner = user.Partner;
                return user;
            }
    }

    public static String getPassByLogin(string loginid)
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                var userLogin = loginid.ToLower();
                var user = db.Users.Where(u => u.ENVIRONMENT == env && u.USERNAME.Trim().ToLower().
                    Equals(loginid.Trim().ToLower())).FirstOrDefault();
                if (user == null)
                {
                    return "";
                }
                //resolvendo problema de lazy
                Partners partner = user.Partner;
                return user.PASSWORD;
            }
    }

    //
    // GET: /Login/
    public ActionResult Senha()
    {
      var usuarioLogado = Session["_USUARIO_LOGADO"];
      return View(usuarioLogado);
    }
    //

    // POST: /Period/Edit/5
    [HttpPost]
    public ActionResult Senha(Users user)
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                UsersDataAccess userData = new UsersDataAccess();
                var usuarioLogado = Session["_USUARIO_LOGADO"];
                var senhaAnterior = Request.Form["senha_anterior"];
                var senhaNova = Request.Form["nova_senha"];
                var senhaConfirma = Request.Form["confirma_senha"];

                var userPersist = db.Users.Find(user.USERID);

                var validaLogin = ValidaLogin(userPersist.USERNAME, senhaAnterior);

                if (validaLogin)
                {
                    userPersist.PASSWORD = userData.ConcatSenhaAcesso(userPersist.USERNAME, senhaNova, true);
                    db.Entry(userPersist).State = EntityState.Modified;
                    db.SaveChanges();

                    Session["_SUCCESS_"] = "true";
                    Session["_MENSAGEM_"] = "A senha foi alterada com sucesso!";
                    return RedirectToAction("Index", "Login");
                    //var usuarioLogado = Session["_USUARIO_LOGADO"];
                    //return View(user);
                }
                else
                {
                    Session["_SUCCESS_"] = "false";
                    Session["_MENSAGEM_"] = "A senha anterior não confere!";
                }

                return View(user);
            }
    }


    /**
     * Criptografa todas as senhas dos usuarios.
     */
    public ActionResult Cript()
    {
      return View();
    }

    /**
     * Criptografa todas as senhas dos usuarios.
     */
    [HttpPost]
    public ActionResult Cript(string _confirmed)
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                try
                {
                    var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

                    List<Users> listaUsuarios = db.Users.ToList();
                    //verifica se o admin esta criptografado
                    Users uAdmin = listaUsuarios.Where(u => u.ENVIRONMENT == env && u.USERNAME == "admin").FirstOrDefault();
                    if (uAdmin.PASSWORD.Trim().Equals("admin@passos".sysPassEncrypt()))
                    {
                        Session["_SUCCESS_"] = "true";
                        Session["_MENSAGEM_"] = "As senhas ja foram criptografadas anteriormente. Faça o login novamente.";
                        return RedirectToAction("Index", "Login", new { erro = true });
                    }

                    foreach (Users u in listaUsuarios)
                    {
                        //verifica se ja foi criptografado
                        string pass = u.PASSWORD;
                        string passCrypt = (u.USERNAME.ToLower() + ":" + pass.ToLower()).sysPassEncrypt();
                        u.PASSWORD = passCrypt;
                        var partner = u.Partner;
                        db.Entry(u).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    Session["_SUCCESS_"] = "true";
                    Session["_MENSAGEM_"] = "As senhas foram criptografadas. Faça o login novamente.";
                    return Sair();
                }
                catch (Exception ex)
                {
                    Util.EscreverLog(ex.Message, "SystemCript");
                    Session["_SUCCESS_"] = "false";
                    Session["_MENSAGEM_"] = "Falha na transação. Tente novamente.";
                    return RedirectToAction("Index", "Login", new { erro = true });
                }
            }
    }


    /**
     * Criptografa todas as senhas dos usuarios.
     */
    [HttpPost]
    public String ResetaSenha()
    {
            using (TimesheetContext db = new TimesheetContext())
            {
                UsersDataAccess userData = new UsersDataAccess();

                try
                {
                    var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                    string senhaAdmin = Request.Form["senhaAdmin"];
                    string login = Request.Form["login"];
                    string novaSenha = Request.Form["novaSenha"];


                    var validaLoginAdmin = ValidaLogin("admin", senhaAdmin);

                    if (validaLoginAdmin)
                    {
                        var userPersist = userData.GetUserByLogin(login);
                        userPersist.PASSWORD = (novaSenha).sysPassEncrypt();
                        db.Entry(userPersist).State = EntityState.Modified;
                        db.SaveChanges();
                        Util.EscreverLog("Alteracao de senha pelo admin WEB:" + login + " novaSenha: " + novaSenha, "SystemWEB");
                        return "OK";
                    }
                    else
                    {
                        Util.EscreverLog("Falha Alteracao de senha pelo admin WEB:" + login + " novaSenha: " + novaSenha, "SystemWEB");
                        return "NOK";
                    }
                }
                catch (Exception ex)
                {
                    Util.EscreverLog(ex.Message, "SystemWEB");
                    return "NOKex";
                }
            }
    }





  }
}
