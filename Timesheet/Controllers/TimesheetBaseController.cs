using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Models;
using Apassos.Common;
using Apassos.DataAccess;

namespace Apassos.Controllers
{
    public class TimesheetBaseController : Controller
    {

        protected TimesheetContext db = new TimesheetContext();
        protected string env;
        protected Users usuarioLogado;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            
            try
            {
               base.Initialize(requestContext);
               usuarioLogado = (Users)Session["_USUARIO_LOGADO"];
                this.env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            if (usuarioLogado == null)
            {

               //Response.Redirect("",true);

               //  throw new System.ArgumentException("Sessão expirada. Faça o login novamente.", "Login");
               Response.Redirect(ConfigurationManager.AppSettings["RELATIVEPATH"].ToString() + "Login");

            }
            else
            {
               Session["_USUARIO_LOGADO"] = usuarioLogado;
               db.Configuration.ProxyCreationEnabled = false;
               db.Configuration.ValidateOnSaveEnabled = false;
            }

            }
            catch (Exception ex)
            {
                Util.EscreverLog(ex.Message, usuarioLogado);
                Response.Redirect(ConfigurationManager.AppSettings["RELATIVEPATH"].ToString() + "Login");
                //throw new System.ArgumentException("Falha crítica. Faça o login novamente.", "Login");
            }
        }

     
        protected List<Project> getListaProjects(Period periodoAtual)
        {
            List<Project> listaProjetos;

            if (periodoAtual == null)
            {
                listaProjetos = ProjectDataAccess.GetProjetosNomeNoPeriodoAll();
            }
            else
            {
                listaProjetos = ProjectDataAccess.GetProjetosNomeNoPeriodo(periodoAtual);
            }
            return listaProjetos;
        }  
           
    }
}
