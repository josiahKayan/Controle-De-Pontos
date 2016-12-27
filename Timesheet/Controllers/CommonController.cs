using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.Models;
using Apassos.DataAccess;

namespace Apassos.Controllers
{
    public class CommonController : Controller
    {
        private static CommonController instance;

        private CommonController() { }

        public static CommonController Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new CommonController();
                }
                return instance;
            }
        }

        public bool AccessValidate(ControllerContext context, Constants.ModulesConstant module)
        {
            this.ControllerContext = context;
            Users usuarioLogado = ((Users)Session["_USUARIO_LOGADO"]);
            var validaPerfil = PartnerDataAccess.ValidaAcessoModulo(usuarioLogado, module.ToString());
            Util.EscreverLog("Validando acesso BD: " + usuarioLogado.USERNAME + " / " + module.ToString() + " / " + validaPerfil, usuarioLogado);
            if ( !validaPerfil ) {
                validaPerfil = AccessRules.ModuleValidate(module, usuarioLogado);
                Util.EscreverLog("Validando acesso HC: " + usuarioLogado.USERNAME + " / " + module.ToString() + " / " + validaPerfil, usuarioLogado);
            }
            return validaPerfil;
        }
        
        public bool AccessValidateRedirect(ControllerContext context, Constants.ModulesConstant module)
        {
            var virtualDir = ConfigurationManager.AppSettings["RELATIVEPATH"].ToString();
            this.ControllerContext = context;
            Users usuarioLogado = ((Users)Session["_USUARIO_LOGADO"]);
            if (!AccessValidate( context, module)) {
                ViewBag.loginerror = "true";
                Session["_USUARIO_LOGADO"] = null;
                Session["_SUCCESS_"] = "false";
                Session["_MENSAGEM_"] = "Você não tem permissão para acessar este módulo !";
                Util.EscreverLog("Redirect apos a validacao ocorrer com erro.", usuarioLogado);
                Response.Redirect(virtualDir);
                
                return false;
            }

            return true;
        }

        public ActionResult ReturnToLoginPage(ControllerContext context)
        {
            this.ControllerContext = context;
            ViewBag.loginerror = "true";
            Session["_SUCCESS_"] = "false";
            Session["_MENSAGEM_"] = "Você não tem permissão para acessar este módulo !";
            return RedirectToAction("Index", "Login", new { erro = true });
        }

        public List<string[]> buttonsProfile(ControllerContext context)
        {
            this.ControllerContext = context;
            Users usuarioLogado = ((Users)Session["_USUARIO_LOGADO"]);
            return AccessRules.ButtonsProfile(usuarioLogado);
        }
    }
}
