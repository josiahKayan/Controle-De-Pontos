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
            Session["LISTA_PROJETOS"] = ProjectDataAccess.GetProjetosAllTimesheet();
            Session["PROJETO"] = x;
            return View();
        }


        public JsonResult AutoCompleteCountry(string term)
        {
            var result = ProjectDataAccess.GetListaProjetosPorNome(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}