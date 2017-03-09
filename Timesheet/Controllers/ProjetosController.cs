using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using System.Data.Objects;

namespace Apassos.Controllers
{
    public class ProjetosController : TimesheetBaseController
    {

        //
        // GET: /Projetos/

        public ActionResult Index()
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            ProjectDataAccess project = new ProjectDataAccess();
            var lista = project.GetProjetosNomeAll();  // db.Projects.Where(p => p.ENVIRONMENT == env).ToList();
            
            return View(lista);
        }

        //
        // GET: /Projetos/Create

        public ActionResult Create()
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            PartnerDataAccess partner = new PartnerDataAccess();
            Session["TODOS_CLIENTES"] = partner.GetEmpresas();
            Session["TODOS_GESTORES"] = partner.GetGestores();

            return View();
        } 

        //
        // POST: /Projetos/Create

        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            try
            {
                var idEmpresa = Request.Form["selectempresa"];
                var idGestor = Request.Form["selectgestor"];
                var selectstatus = Request.Form["selectstatus"];

                //project.PROJECTID = ProjectDataAccess.calculateNextProjectNumber(DateTime.Now.Year,DateTime.Now.Month);
                project.ENVIRONMENT = env;
                project.PARTNERID = int.Parse(idEmpresa);
                project.GESTORID = int.Parse(idGestor);
                project.CREATEDBY = usuarioLogado.USERNAME;
                project.CREATIONDATE = DateTime.Now;
                project.CHANGEDBY = usuarioLogado.USERNAME;
                project.CHANGEDATE = DateTime.Now;
                project.STATUS = selectstatus;

                db.Projects.Add(project);
                db.SaveChanges();

                ViewBag.loginerror = "true";
                Session["_SUCCESS_"] = "true";
                Session["_MENSAGEM_"] = "Projeto criado com sucesso!";
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
                Session["_MENSAGEM_"] = "Falha na transação. Tente novamente.";
                return RedirectToAction("Index", "Login", new { erro = true });
            }
            catch (Exception ex)
            {
                Util.EscreverLog(ex.Message,usuarioLogado);
                ViewBag.loginerror = "true";
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Falha na transação!";
                return RedirectToAction("Index");
            }
        }
        
        //
        // GET: /Projetos/Edit/5
 
        public ActionResult Edit(string id)
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            PartnerDataAccess partner = new PartnerDataAccess();

            Session["TODOS_CLIENTES"] = partner.GetEmpresas();
            Session["TODOS_GESTORES"] = partner.GetGestores();
            Project project = db.Projects.Find(int.Parse(id));
            return View(project);
        }

        //
        // POST: /Projetos/Edit/5

        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            try
            {

                var idEmpresa = Request.Form["selectempresa"];
                var idGestor = Request.Form["selectgestor"];
                var selectstatus = Request.Form["selectstatus"];

                project.ENVIRONMENT = env;
                project.PARTNERID = int.Parse(idEmpresa);
                project.GESTORID = int.Parse(idGestor);
                project.STATUS = selectstatus;
                project.CHANGEDBY = usuarioLogado.USERNAME;
                project.CHANGEDATE = DateTime.Now;
                
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.loginerror = "false";
                Session["_SUCCESS_"] = "true";
                Session["_MENSAGEM_"] = "Projeto atualizado com sucesso!";
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
                Session["_MENSAGEM_"] = "Falha na transação. Tente novamente.";

                return RedirectToAction("Index", "Login", new { erro = true });


            }
            catch (Exception ex)
            {
                PartnerDataAccess partner = new PartnerDataAccess();

                Util.EscreverLog(ex.Message, usuarioLogado);
                ViewBag.loginerror = "true";
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Falha na transação!";
                Session["TODOS_CLIENTES"] = partner.GetEmpresas();
                Session["TODOS_GESTORES"] = partner.GetGestores();
                return View(project);
            }
        }

        [HttpPost]
        public ActionResult Excluir()
        {

            if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTS))
            {
                return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
            }
            try
            {
                var checados = Request.Form["checados"];
                string[] idsX = checados.Split(',');
                foreach (string id in idsX)
                {
                    Project project = db.Projects.Find(int.Parse(id));
                    db.Projects.Remove(project);
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Util.EscreverLog(ex.Message, usuarioLogado);
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Ocorreu uma falha durante o processo de exclusão. Verifique se alguns desses projetos estão sendo usados.";
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