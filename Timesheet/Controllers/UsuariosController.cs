using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using System.Diagnostics;

namespace Apassos.Controllers
{
    public class UsuariosController : TimesheetBaseController
    {

        private const string DEFAULT_PASSWORD = "@passos";
        //
        // GET: /Usuarios/
        public ActionResult Index()
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.USERS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            string profileRoot = ((int)Constants.ProfileConstant.ROOT).ToString();
            var listaUsuarios = db.Users.Where(c => c.ENVIRONMENT == env && c.PROFILE != profileRoot).OrderBy(c => c.USERNAME).ToList();

            listaUsuarios.Sort((x, y) => string.Compare(x.Partner.NAME, y.Partner.NAME));

            return View(listaUsuarios);
        }


        //
        // GET: /Cliente/Create
        public ActionResult Create()
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.USERS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            Session["PARCEIROS_NAO_USUARIOS"] = PartnerDataAccess.GetParceirosNaoUsuarios();
            Session["TODOS_PERFIS"] = PartnerDataAccess.GetPerfisAll();
            return View();
        }

        //
        // POST: /Cliente/Create
        [HttpPost]
        public ActionResult Create(Users usuario)
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.USERS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            try
            {
                UsersDataAccess.SalvarUsuario(usuario, Request.Form["checkperfil"], Request.Form["selectparceiro"], usuarioLogado, "1", Request.Form["selectisalterpwd"]);
                var msg = "O usuário foi salvo com sucesso!";
                Session["_SUCCESS_"] = "true";
                Session["_MENSAGEM_"] = msg;
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
                Util.EscreverLog(exceptionMessage, usuario);
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Falha na transação. Verifique os campos obrigatórios ou se o usuário já foi cadastrado.";
                //return RedirectToAction("Index", "Usuarios", new { erro = true });
            }
            catch (Exception e)
            {
                var msg = "Aconteceu uma falha ao salvar o usuário. Tente novamente. Se o problema persistir, informe ao suporte do sistema!";
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = msg;
                Util.EscreverLog(e.Message, usuarioLogado);
            }
            return View();

        }

        //
        // GET: /Cliente/Edit/5
        public ActionResult Edit(string id)
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PARTNERS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            Users usuario = db.Users.Find(int.Parse(id));
            Session["TODOS_PERFIS"] = PartnerDataAccess.GetPerfisAll();
            return View(usuario);
        }

        //
        // POST: /Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(Users usuario)
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.USERS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            try
            {
                string locked = Request.Form["selectlock"];
                string isalterpwd = Request.Form["selectisalterpwd"];
                UsersDataAccess.SalvarUsuario(usuario, Request.Form["checkperfil"], Request.Form["selectparceiro"], usuarioLogado, locked, isalterpwd);
                var msg = "O usuário foi salvo com sucesso!";
                Session["_SUCCESS_"] = "true";
                Session["_MENSAGEM_"] = msg;
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
                Util.EscreverLog(exceptionMessage, usuarioLogado);
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Falha na transação. Verifique os campos obrigatórios ou se o usuário já foi cadastrado.";
                //return RedirectToAction("Index", "Usuarios", new { erro = true });
            }
            catch (Exception e)
            {
                var msg = "Aconteceu uma falha ao salvar o usuário. Tente novamente. Se o problema persistir, informe ao suporte do sistema!";
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = msg;
                Util.EscreverLog(e.Message, usuarioLogado);
            }
            return View(UsersDataAccess.GetUsuario(usuario.USERID));
        }

        [HttpPost]
        public ActionResult Excluir()
        {
            try
            {
                var checados = Request.Form["checados"];
                string[] idsX = checados.Split(',');
                foreach (string id in idsX)
                {
                    Users consultor = db.Users.Find(int.Parse(id));
                    //remove antes o usuario correspondente
                    db.Users.Remove(consultor);
                    //consultor.LOCKED = "S";
                    //db.us
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Util.EscreverLog(e.Message, usuarioLogado);
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Ocorreu uma falha durante o processo de exclusão. Verifique se alguns desses usuários estão sendo usados.";
            }
            return RedirectToAction("Index");
        }

        /**
      * Criptografa todas as senhas dos usuarios.
      */
        [HttpPost]
        public string Resetar(string username)
        {
            try
            {
                usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
                Session["_USUARIO_LOGADO"] = usuarioLogado;
                if (username.Contains(','))
                {
                    List<Users> listUserd = new List<Users>();
                    string[] userId = username.Split(',');
                    for (int i = 0; i < userId.Length; i++)
                    {
                        listUserd.Add(UsersDataAccess.GetUsuario(int.Parse(userId[i])));
                    }
                    foreach (var u in listUserd)
                    {

                        var userPersist = UsersDataAccess.GetUserByLogin(u.USERNAME);
                        userPersist.PASSWORD = DEFAULT_PASSWORD;
                        db.Entry(userPersist).State = EntityState.Modified;
                        db.SaveChanges();
                        Util.EscreverLog("Alteracao de senha pelo admin WEB:" + u.USERNAME + " novaSenha: " + DEFAULT_PASSWORD, "SystemWEB");
                    }
                }
                else
                {
                    Users u = UsersDataAccess.GetUsuario(int.Parse(username));
                    var userPersist = UsersDataAccess.GetUserByLogin(u.USERNAME);
                    userPersist.PASSWORD = DEFAULT_PASSWORD;
                    db.Entry(userPersist).State = EntityState.Modified;
                    db.SaveChanges();
                    Util.EscreverLog("Alteracao de senha pelo admin WEB:" + u.USERNAME + " novaSenha: " + DEFAULT_PASSWORD, "SystemWEB");
                }

            }
            catch (Exception ex)
            {
                Debug.Write(ex.StackTrace);
            }

            return "ok";

        }


        // POST: /sendpwd/crypt/text
        public String sendpwd(string login, string obs)
        {

            return "error";
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

}