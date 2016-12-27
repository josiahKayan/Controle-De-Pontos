using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Models;
using Apassos.Common;

namespace Apassos.Controllers
{
  public class PeriodController : TimesheetBaseController
  {
    //Session["ATUAL_CONSULTOR"] = consultorAtual;
    //
    // GET: /Period/

    public ViewResult Index()
    {
      List<Period> lista = db.Periods.Where(p => p.ENVIRONMENT == env).OrderBy(p=>p.YEAR).ThenBy(p1=>p1.MONTH).ToList();

      Session["LISTA_TODOS_PERIODOS"] = lista;

      return View();
    }


    //
    // GET: /Period/Create
    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Period/Create

    [HttpPost]
    public ActionResult Create(Period period)
    {
      try
      {

        period.ENVIRONMENT = env;
        period.CREATIONDATE = DateTime.Now;
        period.CREATEDBY = usuarioLogado.USERNAME;
        period.CHANGEDATE = DateTime.Now;

        if (Request.Form["selectstatus"].Equals("aberto"))
        {
          period.STATUS = "a";
        }
        else if (Request.Form["selectstatus"].Equals("corrente"))
        {
          period.STATUS = "c";
        }
        else if (Request.Form["selectstatus"].Equals("fechado"))
        {
          period.STATUS = "f";
        }

        period.CHANGEDBY = usuarioLogado.USERNAME;
        //period.TOTALHOURS = 0;
        db.Periods.Add(period);
        db.SaveChanges();
        Session["_SUCCESS_"] = "true";
        Session["_MENSAGEM_"] = "O período foi salvo com sucesso!";
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
        Apassos.Common.Util.EscreverLog(exceptionMessage, usuarioLogado);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Falha na transação. Verifique se o período já foi cadastrado.";
        return RedirectToAction("Index", "Login", new { erro = true });
      }
      catch (Exception e)
      {
        Util.EscreverLog(e.Message, usuarioLogado);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Aconteceu uma falha ao salvar o período. Tente novamente. Se o problema persistir, informe ao suporte do sistema!";
      }
      return View();
    }

    //
    // GET: /Period/Edit/5

    public ActionResult Edit(string id)
    {
      Period period = db.Periods.Find(int.Parse(id));

      Session["STATUS_ATUAL_PERIODO"] = period.STATUS;

      return View(period);
    }

    //
    // POST: /Period/Edit/5

    [HttpPost]
    public ActionResult Edit(Period period)
    {
      try
      {
        period.CHANGEDATE = Convert.ToDateTime(DateTime.Now.ToShortDateString());

        var status = Request.Form["selectstatus"];

        if (status.Equals("Aberto"))
        {
          period.STATUS = "a";
        }
        else if (status.Equals("Corrente"))
        {
          period.STATUS = "c";
        }
        else if (status.Equals("Fechado"))
        {
          period.STATUS = "f";
        }

        period.CHANGEDBY = usuarioLogado.USERNAME;
        db.Entry(period).State = EntityState.Modified;
        db.SaveChanges();
        Session["_SUCCESS_"] = "true";
        Session["_MENSAGEM_"] = "O período foi salvo com sucesso!";
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
        Session["_MENSAGEM_"] = "Falha na transação. Verifique se o período já foi cadastrado.";
        return RedirectToAction("Index", "Login", new { erro = true });
      }
      catch (Exception e)
      {
        Util.EscreverLog(e.Message, usuarioLogado);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Aconteceu uma falha ao salvar o período. Tente novamente. Se o problema persistir, informe ao suporte do sistema!";
      }

      return View(period);
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
          Period period = db.Periods.Find(int.Parse(id));
          db.Periods.Remove(period);
        }
        db.SaveChanges();
      }
      catch (Exception e)
      {
        Util.EscreverLog(e.Message, usuarioLogado);
        Session["_SUCCESS_"] = "false";
        Session["_MENSAGEM_"] = "Ocorreu uma falha durante o processo de exclusão. Verifique se alguns desses períodos estão sendo usados.";
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