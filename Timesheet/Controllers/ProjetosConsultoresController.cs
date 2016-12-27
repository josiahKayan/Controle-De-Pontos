using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Apassos.Common;
using Apassos.DataAccess;
using Apassos.Models;

namespace Apassos.Controllers
{
   public class ProjetosConsultoresController : TimesheetBaseController
   {

      public ActionResult SelectionProjectConsultant( )
      {
         if (!CommonController.Instance.AccessValidate(this.ControllerContext, Constants.ModulesConstant.PROJECTSCONSULTANT))
         {
            return CommonController.Instance.ReturnToLoginPage(this.ControllerContext);
         }

         var gestorAtual = usuarioLogado.Partner;

         var _projectid = Request.Form["selectprojeto"];

         var _periodid = Request.Form["selectperiodo"];

         var status = Request.Form["STATUS"];

         if( status == null )
         {
            status = "disabled";
         } 

         Project projetoAtual = null;
         if (_projectid != null && _projectid != "")
         {
            projetoAtual = ProjectDataAccess.GetProjeto(_projectid);
         }
         else
         {
            var projetoAtualP = TempData["projetoAtualP"];

            if (projetoAtualP != null)
            {
               projetoAtual = db.Projects.Find(((Project)projetoAtualP).PROJECTID);
            }
            else
            {
               projetoAtual = ProjectDataAccess.GetProjetoAtual();
            }
         }
         //db.Entry(projetoAtual).Reload();

         Period periodoAtual = null;
         if (_periodid != null && _periodid != "")
         {
            periodoAtual = PeriodDataAccess.GetPeriodo(_periodid);
         }

         //Tirei o if daqui
         List<Project> listaProjetos = getListaProjects(periodoAtual);

         var listaConsultores = db.Partners.Where(p => p.ENVIRONMENT == env && p.ISUSER == "S").ToList();
         listaConsultores.Sort((x, y) => string.Compare(x.NAME, y.NAME));

         var listaConsultoresProjeto = ProjectDataAccess.GetConsultoresProjeto(projetoAtual);
         var listaConsultoresComApontamentos = ProjectDataAccess.GetConsultoresComApontamentos(projetoAtual);

         List<Partners> consultoresDisponiveis = new List<Partners>();

         if (listaConsultoresProjeto.Count() > 0)
         {
            foreach (Partners consult in listaConsultores)
            {
               //nao pode alocar o root
               if (int.Parse(consult.user.PROFILE) != ((int)Constants.ProfileConstant.ROOT))
               {
                  var countConsult = listaConsultoresProjeto.Where(p => p.PARTNERID == consult.PARTNERID).Count();
                  if (countConsult == 0)
                  {
                     consultoresDisponiveis.Add(consult);
                  }
               }

            }
         }
         else
         {
            consultoresDisponiveis.AddRange(listaConsultores);
         }



         Session["GESTOR_ATUAL"] = gestorAtual;
         Session["PROJETO_ATUAL"] = projetoAtual;
         Session["TODOS_PROJETOS"] = listaProjetos;
         Session["STATUS"] = status;
         Session["CONSULTORES_DISPONIVEIS"] = consultoresDisponiveis;
         Session["CONSULTORES_PROJETO"] = listaConsultoresProjeto;
         Session["CONSULTORES_PROJETO_APONTAMENTOS"] = listaConsultoresComApontamentos;
         Session["periodoAtual"] = periodoAtual;

         return View();
      }




      // Post: /ProjetosConsultores/Acessar?loginid=x&pass=y
      [HttpPost]
      public ActionResult Save()
      {
         var _projectid = Request.Form["selectprojeto"];
         var projetoAtual = ProjectDataAccess.GetProjeto(_projectid);
         var _consultoreselecionados = Request.Form["idsconsultoresselecionados"];

         //remove os consultores nao selecionados, que antes estavam alocados no projeto.
         ProjectDataAccess.ExcluiRelacaoConsultoresProjetos(projetoAtual, _consultoreselecionados);

         //insere os novos consultores no projeto
         string consultoresFalha = "";
         if (_consultoreselecionados != "")
         {
            var consultoresArray = _consultoreselecionados.Split(',');
            foreach (string _idPartner in consultoresArray)
            {
               if (_idPartner != "")
               {
                  if (!ProjectDataAccess.SaveProjectPartner(projetoAtual.PROJECTID, int.Parse(_idPartner), usuarioLogado))
                  {
                     Partners consultor = PartnerDataAccess.GetParceiro(int.Parse(_idPartner));
                     consultoresFalha = consultoresFalha + consultor.SHORTNAME + ",";
                  }
               }
            }
         }

         if (consultoresFalha == "")
         {
            Session["_SUCCESS_"] = "true";
            Session["_MENSAGEM_"] = "A relação dos consultores nos projetos foi atualizada com sucesso!";
         }
         else
         {
            Session["_SUCCESS_"] = "false";
            Session["_MENSAGEM_"] = "Ocorreram falhas ao atualizar os dados dos consultores: " + consultoresFalha;
         }
         // Pertistir dados até o próximo request.
         TempData["projetoAtualP"] = projetoAtual;
         return RedirectToAction("SelectionProjectConsultant", "ProjetosConsultores");
      }


      protected override void Dispose(bool disposing)
      {
         db.Dispose();
         base.Dispose(disposing);
      }

   }



}