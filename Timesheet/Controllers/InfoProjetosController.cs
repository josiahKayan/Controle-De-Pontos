using Apassos.DataAccess;
using Apassos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Apassos.Controllers
{
    public class InfoProjetosController : TimesheetBaseController
    {
        public ActionResult InfoProjetos(List<ProjetoAuxiliar> x = null)
        {
            ProjectDataAccess project = new ProjectDataAccess();
            Session["LISTA_PROJETOS"] = project.GetProjetosAllTimesheet();
            Session["PROJETO"] = x;
            return View();
        }


        public JsonResult AutoCompleteCountry(string term)
        {
            ProjectDataAccess project = new ProjectDataAccess();
            var result = project.GetListaProjetosPorNome(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}