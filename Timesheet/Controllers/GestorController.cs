using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;
using Apassos.reports.classes;
using System.Text;

namespace Apassos.Controllers
{
  public class GestorController : TimesheetBaseController
  {

    //
    // GET: /Gestor/

    public ActionResult Aprovacao()
    {
      if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.APPROVAL))
      {
        return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
      }

      var gestorAtual = usuarioLogado.Partner;

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

      Session["_USUARIO_LOGADO"] = usuarioLogado;
      Session["GESTOR_ATUAL"] = gestorAtual;
      Session["PERIODO_ATUAL"] = periodoAtual;
      Session["TODOS_PERIODOS"] = PeriodDataAccess.GetPeriodoAll();

      Session["TODOS_CONSULTORES_APONTAMENTOS"] = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(periodoAtual);

      return View();
    }

    // Post: /Login/Acessar?loginid=x&pass=y
    [HttpPost]
    public ActionResult AprovacaoSalvar()
    {
      Period periodoAtual = db.Periods.Find(int.Parse(Request.Form["selectperiodo"]));
      List<PartnersTimesheetHeaderAccess> listaApont = ProjectDataAccess.GetConsultoresApontamentosPorPeriodo(periodoAtual);

      foreach (PartnersTimesheetHeaderAccess consultApont in listaApont)
      {
        //verifica se tem status para atualizar no formulario
        foreach (TimesheetItem itemTimesheet in consultApont.items)
        {
          var iditem = Request.Form["hidden_id_timesheetitem_" + itemTimesheet.TIMESHEETITEMID];
          if (iditem != null)
          {
            var tipoaprovacao = Request.Form["selectstatusitem_id_" + itemTimesheet.TIMESHEETITEMID];
            var anotacao = Request.Form["text_note_" + itemTimesheet.TIMESHEETITEMID];
            var hash = Request.Form["hash_" + itemTimesheet.TIMESHEETITEMID];

            StringBuilder newObjectHash = new StringBuilder(tipoaprovacao);
            newObjectHash.Append(anotacao);

            int newHash = newObjectHash.GetHashCode();
            int oldHash = int.Parse(hash);
            if (!oldHash.Equals(newHash))
            {
              TimesheetDataAccess.SalvarItemApontamentoAprovacao(iditem, tipoaprovacao, anotacao);
            }
          }
        }

      }

      Session["_SUCCESS_"] = "true";
      Session["_MENSAGEM_"] = "Os apontamentos foram atualizados com sucesso!";

      return RedirectToAction("Aprovacao", "Gestor");
    }

    // GET: /InfoWS/ExportToExcel/1 => periodid
    public void ExportToExcel()
    {
      //valida se tem permissao para entrar nessa tela
      if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.ALLTIMESHEETINPERIODREPORT))
      {
        var periodid = Request.Form["selectperiodo"];
        var gestorAtual = usuarioLogado.Partner;
        ConsultoresApontamentosXLS xls = new ConsultoresApontamentosXLS(periodid, gestorAtual,true);
        xls.Execute(this.HttpContext);
      }
      
    }
 
    public void ExportToExcelHours()
    {
      //valida se tem permissao para entrar nessa tela
      if (CommonController.Instance.AccessValidateRedirect(this.ControllerContext, Constants.ModulesConstant.TIMESHEETINPERIODREPORT))
      {
        var periodid = Request.Form["selectperiodo"];
        RelatorioProjetosXLS xls = new RelatorioProjetosXLS(periodid);
        xls.Execute(this.HttpContext);
      }
    }

    
    
  }
}
