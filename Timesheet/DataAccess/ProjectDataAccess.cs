using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Apassos.Models;
using System.Data.Entity.Validation;
using Apassos.Common;
using Apassos.Common.Extensions;
using System.Diagnostics;

namespace Apassos.DataAccess
{
    public class ProjectDataAccess
    {
        static private TimesheetContext db = new TimesheetContext();

        /**
         * Retorna lista de projetos do consultor.
         */
        public List<ProjectUser> GetProjetosPorConsultor(Partners consultor, bool ignoreEncerrados = true)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<ProjectUser> lista = new List<ProjectUser>();
            if (!ignoreEncerrados)
            {
                lista = db.ProjectUsers.Where(p => p.ENVIRONMENT == env).ToList();
            }
            else
            {
                var listaAll = db.ProjectUsers.Where(p => p.ENVIRONMENT == env).ToList();
                foreach (var pu in listaAll)
                {
                    if (int.Parse(pu.project.STATUS).In((int)Constants.StatusProjetoConstant.Aberto, (int)Constants.StatusProjetoConstant.Iniciado))
                    {
                        lista.Add(pu);
                    }
                }

            }

            if (lista != null)
            {
                List<ProjectUser> listaReturn = new List<ProjectUser>();
                foreach (ProjectUser item in lista)
                {
                    if (item.partner.PARTNERID == consultor.PARTNERID)
                    {
                        listaReturn.Add(item);
                    }
                }
                return listaReturn;
            }
            return new List<ProjectUser>();
        }

        /**
         * Retorna lista de projetos do consultor.
         */
        public List<Project> GetProjetosPorGestor(Partners gestor)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Projects.Where(p => p.ENVIRONMENT == env).ToList();

            if (lista != null)
            {
                List<Project> listaReturn = new List<Project>();
                foreach (Project item in lista)
                {
                    if (item.Gestor.PARTNERID == gestor.PARTNERID)
                    {
                        listaReturn.Add(item);
                    }
                }
                return listaReturn;
            }
            return new List<Project>();
        }


        public List<ProjetoAuxiliar> GetListaProjetosPorNome(string name)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Projects.Where(p => p.ENVIRONMENT == env && (p.STATUS == "1" || p.STATUS == "0") && p.NAME.ToUpper().Contains(name.ToUpper())).ToList();

            List<ProjetoAuxiliar> listProjetos = new List<ProjetoAuxiliar>();
            ProjetoAuxiliar projetoAuxiliar = new ProjetoAuxiliar();

            try
            {

                foreach (var item in lista)
                {
                    projetoAuxiliar.id = item.PROJECTID;
                    projetoAuxiliar.name = item.NAME;
                    projetoAuxiliar.description = item.DESCRIPTION;
                    listProjetos.Add(projetoAuxiliar);
                }

                if (listProjetos != null)
                {
                    return listProjetos;
                }
            }
            catch ( Exception e)
            {
                Debug.WriteLine("Aquiiii");
            }
            return null;
        }

        /**
        * Retorna lista de consultores que sao dos projetos.
        */
        public List<TimesheetHeader> GetConsultoresApontamentosPorGestor(Partners gestor)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.TimesheetHeaders.Where(p => p.ENVIRONMENT == env).ToList();

            if (lista != null)
            {
                List<TimesheetHeader> listaReturn = new List<TimesheetHeader>();
                foreach (TimesheetHeader item in lista)
                {
                    //    if (item.poL.PARTNERID == gestor.PARTNERID)
                    //    {
                    //        listaReturn.Add(item);
                    //    }
                }
                return listaReturn;
            }
            return new List<TimesheetHeader>();
        }

        /**
       * Retorna lista de consultores que tem apontamentos no periodo.
       */
        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorPeriodo(Period periodo)
        {

            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            //List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.Period.PERIODID == periodo.PERIODID).
            //    Select(x => x.TimesheetHeader.Partner).OrderBy(u1=>u1.USERGROUP).ThenBy(u2=>u2.NAME).Distinct().ToList();
            List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.Period.PERIODID == periodo.PERIODID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();

            listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
            foreach (Partners partner in listPartners)
            {
                list.Add(new PartnersTimesheetHeaderAccess(partner, periodo, periodo));
            }
            return list;
        }


        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorPeriodo(Project projeto = null , Period periodoInicial = null, Period periodoFinal = null)
        {

            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();

            if (projeto == null && periodoInicial == null && periodoFinal == null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env ).Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, true, periodoInicial, periodoFinal, projeto));
                }

            }
            else
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.project.PROJECTID ==
                projeto.PROJECTID).Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, true, periodoInicial, periodoFinal, projeto));
                }
            }
            return list;
        }

        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorPeriodoProjeto(Period periodo, Partners partners, Project project)
        {


            Debug.WriteLine("Aqui");

            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();


            List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && project.PROJECTID == ti.project.PROJECTID
            && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();

            listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
            foreach (Partners partner in listPartners)
            {
                list.Add(new PartnersTimesheetHeaderAccess(partner, periodo, project));
            }
            return list;
        }


        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorPeriodoProjeto(Partners partners , Project project , Period periodoInicial , Period periodoFinal )
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();



            if (partners == null && project == null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env &&
                    ti.TimesheetHeader.Period.TIMESHEETPERIODSTART >= periodoInicial.TIMESHEETPERIODSTART && ti.TimesheetHeader.Period.TIMESHEETPERIODFINISH <= periodoFinal.TIMESHEETPERIODFINISH
                        ).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, periodoInicial, periodoFinal, project));
                }

            }
            else if(partners == null && project != null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && project.PROJECTID == ti.project.PROJECTID
                        ).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, periodoInicial, periodoFinal, project));
                }

            }

            else if (project == null && partners != null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env 
                        && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, periodoInicial, periodoFinal, project));
                }
            }

            else if (project != null && partners != null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && project.PROJECTID == ti.project.PROJECTID
                        && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, periodoInicial, periodoFinal, project));
                }
            }
            return list;
        }


        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorPeriodoProjeto(Partners partners, Project project )
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();


            if (project == null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env
                        && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, project));
                }
            }

            else if (project != null)
            {
                List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && project.PROJECTID == ti.project.PROJECTID
                        && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

                listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
                foreach (Partners partner in listPartners)
                {
                    list.Add(new PartnersTimesheetHeaderAccess(partner, project));
                }
            }
            return list;
        }


        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorPeriodoPartner(Period periodo, Partners partners)
        {

            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();


            List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID
            && partners.PARTNERID == ti.TimesheetHeader.Partner.PARTNERID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();

            listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
            foreach (Partners partner in listPartners)
            {
                list.Add(new PartnersTimesheetHeaderAccess(partner, periodo, null));
            }
            return list;
        }

        public List<PartnersTimesheetHeaderAccess> GetConsultoresApontamentosPorProjeto(Period periodo, Project project)
        {


            Debug.WriteLine("Caso onde o parceiro é nulo e o projeto foi escolhido");

            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

            //Comparar o ID do projeto com o do TimesheetItems
            List<Partners> listPartners = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && project.PROJECTID == ti.project.PROJECTID).
                        Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

            List<PartnersTimesheetHeaderAccess> list = new List<PartnersTimesheetHeaderAccess>();

            //listPartners.Sort((x, y) => string.Compare(x.USERGROUP + ":" + x.NAME, y.USERGROUP + ":" + y.NAME));
            foreach (Partners partner in listPartners)
            {
                list.Add(new PartnersTimesheetHeaderAccess(partner, periodo, project));
            }
            return list;
        }

        /**
        * Retorna os projetos cadastrados em banco.
        */
        public List<Project> GetProjetosNomeAll()
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var listaX = db.Projects.Where(p => p.ENVIRONMENT == env).OrderBy(p => p.NAME).ToList();
            List<Project> lista = new List<Project>();
            foreach (Project pX in listaX)
            {
                var p = db.Projects.Find(pX.PROJECTID);
                db.Entry(p).Reload();
                lista.Add(p);

            }
            return lista;
        }

        /**
       * Retorna todos os projetos cadastrados.
       */
        public List<Project> GetProjetosNomeNoPeriodoAll()
        {
            DateTime dataHoje = DateTime.Now;

            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Projects.Where(p => p.ENVIRONMENT == env).ToList();
            lista = lista.OrderBy(p => p.NAME).ToList();
            return lista;
        }

        /**
      * Retorna os projetos cadastrados, que estejam dentro do periodo estipulado de inicio e final.
      */
        public List<Project> GetProjetosNomeNoPeriodo(Period period)
        {
            DateTime dataHoje = DateTime.Now;
            PeriodDataAccess periodo = new PeriodDataAccess();
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var listaAll = db.Projects.Where(p => p.ENVIRONMENT == env).ToList();

            List<Project> lista = new List<Project>();
            List<DateTime> listaDatasPeriodo = periodo.GetListDate(period);

            foreach (var item in listaAll)
            {
                List<DateTime> listaDatasProj = new List<DateTime>() { item.PLANNEDSTARTDATE, Convert.ToDateTime(item.ACTUALSTARTDATE), item.PLANNEDFINISHDATE, Convert.ToDateTime(item.ACTUALFINISHDATE) };
                if (Util.DatesInRange(listaDatasProj, listaDatasPeriodo[0], listaDatasPeriodo[listaDatasPeriodo.Count() - 1]))
                {
                    lista.Add(item);
                }
            }


            lista = lista.OrderBy(p => p.NAME).ToList();
            return lista;
        }

        /**
        * Retorna os projetos cadastrados em banco.
        */
        public  List<Project> GetProjetosAll()
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Projects.Where(p => p.ENVIRONMENT == env).ToList();
            lista = lista.OrderBy(p => p.NAME).ToList();
            return lista;
        }


        public  List<Project> GetProjetosAllTimesheet()
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            var lista = db.Projects.Where(p => p.ENVIRONMENT == env).ToList();
            lista = lista.OrderBy(p => p.PROJECTID).ToList();
            return lista;
        }


        /**
        * Retorna o projeto pelo id. 
        */

        //Tratar esse bug
        public  Project GetProjeto(string id)
        {
            var projeto = db.Projects.Find(int.Parse(id));
            return projeto;
        }


        public  bool IsProjectExistInThisPeriod(Period period)
        {
            var exist = db.Projects.Where(p => p.PLANNEDSTARTDATE.Month >= period.MONTH && p.PLANNEDFINISHDATE.Month <= period.MONTH).ToString();
            if( exist != null || exist != string.Empty )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
        * Retorna o ultimo projeto cadastrado.
        */
        public Project GetProjetoAtual()
        {
            var lista = GetProjetosAll();
            return lista.FirstOrDefault();
        }

        public Project GetProjetoAtualPorPeriodo(Period periodo, Project project)
        {
            var lista = db.Projects.Where(p => p.ACTUALSTARTDATE >= periodo.TIMESHEETPERIODSTART && p.ACTUALFINISHDATE <= periodo.TIMESHEETPERIODFINISH).ToList();
            return lista.FirstOrDefault();
        }


        /**
        * Retorna os consultores do projeto selecionado.
        */
        public List<Partners> GetConsultoresProjeto(Project project)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<Partners> listPartners = db.ProjectUsers.Where(p => p.ENVIRONMENT == env && p.project.PROJECTID == project.PROJECTID).
                Select(x => x.partner).Distinct().ToList();

            var lista = listPartners.OrderBy(p => p.NAME).ToList();
            return lista;
        }

        public List<Partners> GetConsultoresIdProjeto(int id)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<Partners> listPartners = db.ProjectUsers.Where(p => p.ENVIRONMENT == env && p.project.PROJECTID == id).
                Select(x => x.partner).Distinct().ToList();

            var lista = listPartners.OrderBy(p => p.NAME).ToList();


            return lista;
        }


        /**
        * Retorna as relacoes dos consultores/projeto selecionado.
        */
        public List<ProjectUser> GetRelacaoConsultoresProjeto(Project project)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<ProjectUser> lista = db.ProjectUsers.Where(p => p.ENVIRONMENT == env && p.project.PROJECTID == project.PROJECTID).ToList();
            return lista;
        }

        /**
        * Retorna os consultores do projeto selecionado, com apontamentos.
        */
        public List<Partners> GetConsultoresComApontamentos(Project project)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            List<Partners> listPartners = db.TimesheetItems.Where(t => t.ENVIRONMENT == env && t.project.PROJECTID == project.PROJECTID).
                Select(x => x.TimesheetHeader.Partner).Distinct().ToList();

            var lista = listPartners.OrderBy(p => p.NAME).ToList();
            return lista;
        }

        /**
       * Verifica se o consultor do projeto tem apontamentos.
       */
        public int TotalConsultorApontamentos(Project project, Partners partner)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            int totalApontamentos = db.TimesheetItems.Where(t => t.ENVIRONMENT == env && t.project.PROJECTID == project.PROJECTID
                && t.TimesheetHeader.Partner.PARTNERID == partner.PARTNERID).Count();
            return totalApontamentos;
        }


        public static int TotalConsultorApontamentosFilter(Project project, Partners partner)
        {
            var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            int totalApontamentos = db.TimesheetItems.Where(t => t.ENVIRONMENT == env && t.project.PROJECTID == project.PROJECTID
                && t.TimesheetHeader.Partner.PARTNERID == partner.PARTNERID).Count();
            return totalApontamentos;
        }

        /**
       * Salva o relacionamento do projeto e consultor.
       */
        public bool SaveProjectPartner(int ProjectID, int PartnerID, Users usuarioLogado)
        {
            try
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                ProjectUser pu = new ProjectUser
                {
                    ENVIRONMENT = env,
                    partner = db.Partners.Find(PartnerID),
                    project = db.Projects.Find(ProjectID),
                    CREATIONDATE = (DateTime?)DateTime.Now,
                    CREATEDBY = usuarioLogado.USERNAME,
                    CHANGEDATE = (DateTime?)DateTime.Now,
                    CHANGEDBY = usuarioLogado.USERNAME
                };
                db.ProjectUsers.Add(pu);
                db.SaveChanges();
                return true;
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
                Util.EscreverLog(exceptionMessage, usuarioLogado);
            }
            catch (Exception ex)
            {
                Util.EscreverLog(ex.Message, usuarioLogado);
            }
            return false;
        }

        /**
         * Remove as relacoes entre projetos e consultores que sairam da selecao atual.
         */
        public void ExcluiRelacaoConsultoresProjetos(Project projetoAtual, string _consultoreselecionados)
        {
            ProjectDataAccess project = new ProjectDataAccess();
            //corre todos os consultores ja cadastrados, para verificar quais serao excluidos
            List<ProjectUser> listaConsultoresProjetoExcluir = new List<ProjectUser>();
            List<ProjectUser> listaRelacaoConsultoresProjeto = project.GetRelacaoConsultoresProjeto(projetoAtual);
            foreach (ProjectUser item in listaRelacaoConsultoresProjeto)
            {
                //se nao tiver apontamentos, nao pode excluir
                if (project.TotalConsultorApontamentos(item.project, item.partner) == 0)
                {
                    //se não estiver selecionado, marca para exclusao
                    if (!_consultoreselecionados.Contains("," + item.PROJECTUSERID + ","))
                    {
                        listaConsultoresProjetoExcluir.Add(item);
                    }
                }
            }
            foreach (ProjectUser item in listaConsultoresProjetoExcluir)
            {
                var result = db.ProjectUsers.Remove(item);
            }
            db.SaveChanges();
        }

        public  int calculateNextProjectNumber(int year, int month)
        {
            int nextNumber = 0;
            int yearMonthStarts = int.Parse(year.ToString() + month.ToString() + "0000");
            int yearMonthEnds = int.Parse(year.ToString() + month.ToString() + "9999");
            int currentNumber = 0;
            try
            {
                currentNumber = db.Projects.Where(p => p.PROJECTID >= yearMonthStarts && p.PROJECTID <= yearMonthEnds).Max(p1 => p1.PROJECTID);
                if (currentNumber == 0)
                {
                    currentNumber = yearMonthStarts;
                }
            }
            catch
            {
                currentNumber = yearMonthStarts;
            }

            nextNumber = currentNumber + 1;

            return nextNumber;
        }

    }
}

