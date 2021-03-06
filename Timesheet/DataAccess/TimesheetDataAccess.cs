﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Apassos.Common;
using Apassos.Models;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Text;
using System.Data;
using TeamWorkNet.Model;
using System.Data.Entity;
using System.Diagnostics;

namespace Apassos.DataAccess
{
    public class TimesheetDataAccess
    {

        /**
         * Retorna o apontamento do consultor(partner.isuser=S), do periodo.
         */
        public TimesheetHeader GetApontamentoCabecalhoPorPeriodo(Partners partner, Period periodo)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                TimesheetHeader lista = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == env && th.Partner.PARTNERID == partner.PARTNERID && th.Period.PERIODID == periodo.PERIODID).FirstOrDefault();
                if (lista != null)
                {
                    return lista;
                }
                return null;
            }
        }


        public bool GetStatus(string _id, string tipoaprovacao)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                int id = int.Parse(_id);
                int tipo = int.Parse(tipoaprovacao);

                try
                {
                    var z = db.TimesheetItems.First(x => x.TIMESHEETITEMID == id && x.STATUS == tipo);

                    if (z != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /**
       * Retorna os  itens de apontamentos do cabecalho (header).
       */

        //Aqui adiciona o include do projeto
        public List<TimesheetItem> GetiTensApontamentoPorCabecalho(TimesheetHeader header)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                db.Configuration.LazyLoadingEnabled = true;

                if (header != null)
                {
                    var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                    List<TimesheetItem> listaReturn = db.TimesheetItems.Include("project").Where(thi => thi.ENVIRONMENT == env && thi.TimesheetHeader.TIMESHEETHEADERID == header.TIMESHEETHEADERID)
                        .OrderBy(thi2 => thi2.DATE).ThenBy(thi3 => thi3.IN).ToList();
                    return listaReturn;
                }
                return new List<TimesheetItem>();
            }
        }

        public void SalvarItemApontamento(string _id, string _data, string _projectid, string _type, string _in, string _out, string _break, string _description, string _timesheetheaderid, string _consultorid, string _periodoid)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                //valida as entradas
                if (_data == "" || _projectid == "" || _type == "" || _in == "" || _out == "")
                {
                    return;
                }

                if (_in.Trim() == "")
                {
                    _in = "00:00";
                }
                if (_out.Trim() == "")
                {
                    _out = "00:00";
                }
                if (_break.Trim() == "")
                {
                    _break = "00:00";
                }
                if (_description.Trim() == "")
                {
                    _description = ".";
                }


                if (_id == null) //inserir
                {
                    var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

                    //verifica se tem header
                    TimesheetHeader timesheetHeader = null;
                    if (_timesheetheaderid == null || _timesheetheaderid == "")
                    {

                        //busca o header dos apontamentos
                        var consultorheader = db.Partners.Find(int.Parse(_consultorid));
                        var periodoheader = db.Periods.Find(int.Parse(_periodoid));
                        var headerEncontrado = GetApontamentoCabecalhoPorPeriodo(consultorheader, periodoheader);
                        if (headerEncontrado == null)
                        {
                            timesheetHeader = new TimesheetHeader
                            {
                                ENVIRONMENT = env,
                                Partner = db.Partners.Find(int.Parse(_consultorid)),
                                Period = db.Periods.Find(int.Parse(_periodoid))
                            };
                        }
                        else
                        {
                            timesheetHeader = headerEncontrado;
                        }

                    }
                    else
                    {
                        timesheetHeader = db.TimesheetHeaders.Find(int.Parse(_timesheetheaderid));
                    }

                    TimesheetItem itemSalvar = new TimesheetItem
                    {
                        ENVIRONMENT = env,
                        COUNTER = 1,
                        DATE = Convert.ToDateTime(_data),
                        project = db.Projects.Find(int.Parse(_projectid)),
                        TYPE = _type,
                        IN = TimeSpan.Parse(_in),
                        OUT = TimeSpan.Parse(_out),
                        BREAK = TimeSpan.Parse(_break),
                        DESCRIPTION = _description,
                        NOTE = ".",
                        STATUS = 0,
                        TimesheetHeader = timesheetHeader
                    };
                    db.TimesheetItems.Add(itemSalvar);
                    db.SaveChanges();
                }
                else //atualizar
                {

                    TimesheetItem itemSalvar = db.TimesheetItems.Find(int.Parse(_id));
                    itemSalvar.DATE = Convert.ToDateTime(_data);
                    itemSalvar.project = db.Projects.Find(int.Parse(_projectid));
                    itemSalvar.TYPE = _type;
                    itemSalvar.IN = TimeSpan.Parse(_in);
                    itemSalvar.OUT = TimeSpan.Parse(_out);
                    itemSalvar.BREAK = TimeSpan.Parse(_break);
                    itemSalvar.DESCRIPTION = _description;
                    db.SaveChanges();
                }

            }
        }

        public void SalvarItemApontamentoAprovacao(string _id, string tipoaprovacao, string anotacao)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                TimesheetItem itemSalvar = db.TimesheetItems.Find(int.Parse(_id));
                itemSalvar.project = db.Projects.Find(itemSalvar.project.PROJECTID);
                itemSalvar.STATUS = int.Parse(tipoaprovacao);
                itemSalvar.NOTE = anotacao;
                db.SaveChanges();
            }

        }

        public List<TimeSpan> GetTotalHorasApontamentoDia(string _periodoid, string _data, string _idPartners)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                PeriodDataAccess peri = new PeriodDataAccess();
                PartnerDataAccess part = new PartnerDataAccess();
                Period p = peri.GetPeriodo(_periodoid);
                Partners parceiro = part.GetParceiroId(_idPartners);
                DateTime date = Convert.ToDateTime(_data);
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                List<TimesheetItem> item = db.TimesheetItems.Where(t => t.ENVIRONMENT == env && parceiro.PARTNERID == t.TimesheetHeader.Partner.PARTNERID &&
                t.TimesheetHeader.Period.MONTH == p.MONTH && t.TimesheetHeader.Period.YEAR == p.YEAR && date == t.DATE).ToList();

                List<TimeSpan> m = new List<TimeSpan>();
                foreach (var x in item)
                {
                    m.Add(x.OUT - x.IN - x.BREAK);
                }

                return m;
            }
        }


        public void SaveTimesheetItems(List<TimesheetTeamWorkItem> items)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                int mes = DateTime.Now.Month;

                foreach (var item in items)
                {

                    if (item.TimesheetItem.DATE.Month == mes)
                    {
                        TimesheetTeamWorkItem foundItem =
                            db.TimesheetTeamWorkItems.Include("TimesheetItem").Where(x => x.TeamWorkTimeEntryId == item.TeamWorkTimeEntryId).SingleOrDefault();
                        if (foundItem != null)
                        {
                            try
                            {
                                foundItem.TeamWorkTimeEntryId = item.TeamWorkTimeEntryId;
                                foundItem.TeamWorkTodoItemId = item.TeamWorkTodoItemId;
                                foundItem.TimesheetItem.BREAK = item.TimesheetItem.BREAK;
                                foundItem.TimesheetItem.COUNTER = item.TimesheetItem.COUNTER;
                                foundItem.TimesheetItem.DATE = item.TimesheetItem.DATE;

                                if (item.TeamWorkTimeDescription != null)
                                {
                                    foundItem.TimesheetItem.DESCRIPTION = "TW- " + item.TimesheetItem.DESCRIPTION + ":" + item.TeamWorkTimeDescription;
                                }
                                else
                                {
                                    foundItem.TimesheetItem.DESCRIPTION = item.TimesheetItem.DESCRIPTION;
                                }
                                foundItem.TimesheetItem.IN = item.TimesheetItem.IN;
                                foundItem.TimesheetItem.OUT = item.TimesheetItem.OUT;
                                foundItem.TimesheetItem.project = item.TimesheetItem.project;
                                foundItem.TimesheetItem.TimesheetHeader = item.TimesheetItem.TimesheetHeader;
                                db.Entry(foundItem).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                Util.EscreverLog(e.Message, e.Message);
                            }
                        }

                        else
                        {
                            if (item.TimesheetItem.DATE.Month == mes)
                            {
                                try
                                {
                                    db.TimesheetTeamWorkItems.Add(item);
                                    db.SaveChanges();
                                }
                                catch (Exception e)
                                {
                                    Util.EscreverLog(e.Message, e.Message);
                                }
                            }
                        }

                    }
                }
            }
            //}
            //catch (DbEntityValidationException ex)
            //{

            //    List<string> errorMessages = new List<string>();
            //    foreach (DbEntityValidationResult validationResult in ex.EntityValidationErrors)
            //    {
            //        string entityName = validationResult.Entry.Entity.GetType().Name;
            //        foreach (DbValidationError error in validationResult.ValidationErrors)
            //        {
            //            errorMessages.Add(entityName + "." + error.PropertyName + ": " + error.ErrorMessage);
            //        }
            //    }
            //    var fullErrorMessage = string.Join("; ", errorMessages);
            //    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
            //    Util.EscreverLog(exceptionMessage, fullErrorMessage);
            //}
            //catch (Exception dbuex)
            //{
            //StringBuilder builder = new StringBuilder("");

            //try
            //{
            //    foreach (var result in dbuex.Entries)
            //    {
            //        if (result.Entity != null)
            //        {
            //            var entityType = result.Entity.GetType();

            //            var id = entityType.GetProperty("Id").GetValue(result.Entity, null);

            //            builder.Append(id);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    builder.Append("Error parsing DbUpdateException: " + e.ToString());
            //}

            //string message = builder.ToString();
            //    //var exceptionMessage = string.Concat(dbuex.Message, " The validation errors are: ", message);
            //    Util.EscreverLog(dbuex, message);
            //}
            //catch (Exception ex)
            //{
            //    Util.EscreverLog(ex.Message, "");
            //}
        }


        public void SalvarHeader(Partners consultor, Period periodo)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                TimesheetHeader itemSalvar = new TimesheetHeader
                {
                    ENVIRONMENT = env,
                    Partner = db.Partners.Find(consultor.PARTNERID),
                    Period = db.Periods.Find(periodo.PERIODID)
                };
                db.TimesheetHeaders.Add(itemSalvar);
                db.SaveChanges();
            }
        }

        public void ExcluirItemApontamento(string _id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                if (_id != null && _id != "") //excluir
                {
                    var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();


                    TimesheetItem itemExcluir = db.TimesheetItems.Find(int.Parse(_id));
                    TimesheetTeamWorkItem exclui = db.TimesheetTeamWorkItems.Where(x => x.TimesheetItem.TIMESHEETITEMID == itemExcluir.TIMESHEETITEMID).FirstOrDefault();

                    if (exclui != null)
                    {
                        db.TimesheetTeamWorkItems.Remove(exclui);
                        db.TimesheetItems.Remove(itemExcluir);
                    }
                    else
                    {
                        db.TimesheetItems.Remove(itemExcluir);
                    }

                    db.SaveChanges();
                }
            }
        }

        public bool ApontamentosFechado(TimesheetHeader th)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                if (th != null)
                {
                    var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();

                    //busca todos os items do header
                    var lista = db.TimesheetItems.Where(ti => ti.ENVIRONMENT == env && ti.TimesheetHeader.TIMESHEETHEADERID == th.TIMESHEETHEADERID).ToList();

                    int totalEncerrado = lista.Where(ti => ti.STATUS == ((int)Constants.StatusAprovacaoConstant.Encerrado)).Count();

                    return (totalEncerrado == lista.Count() && lista.Count() > 0);
                }
                return false;
            }
        }

        public void TimesheetItemAtualizar(int id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                db.TimesheetItems.Find(id);
                db.SaveChanges();
            }
        }

        public TimesheetItem Get(int id)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                return db.TimesheetItems.Find(id);
            }
        }

        /**
     * Retorna os  itens de apontamentos do cabecalho (header).
     */
        public bool isApontamentoByStartTS(string login)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                PartnerDataAccess part = new PartnerDataAccess();
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                Partners consultorAtual = part.GetPartnerByLogin(login);
                DateTime dataAtual = DateTime.Now;
                List<TimesheetItem> listaReturn = db.TimesheetItems.Where(thi => thi.ENVIRONMENT == env).ToList();

                foreach (TimesheetItem it in listaReturn)
                {
                    if (it.TimesheetHeader.Partner.PARTNERID == consultorAtual.PARTNERID && it.DATE.ToString("dd/MM/yyyy").Equals(dataAtual.ToString("dd/MM/yyyy")))   // && it.DESCRIPTION.ToUpper().Equals("A DEFINIR."))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /**
          * Retorna os  itens de apontamentos do cabecalho (header).
          */
        public TimesheetItem GetApontamentoByStartTS(string login)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                PartnerDataAccess partner = new PartnerDataAccess();
                var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                Partners consultorAtual = partner.GetPartnerByLogin(login);
                DateTime dataAtual = DateTime.Now;
                List<TimesheetItem> listaReturn = db.TimesheetItems.Where(thi => thi.ENVIRONMENT == env).ToList();

                TimesheetItem itReturn = null;

                foreach (TimesheetItem it in listaReturn)
                {
                    if (it.TimesheetHeader.Partner.PARTNERID == consultorAtual.PARTNERID && it.DATE.ToString("dd/MM/yyyy").Equals(dataAtual.ToString("dd/MM/yyyy")))   // && it.DESCRIPTION.ToUpper().Equals("A DEFINIR."))
                    {
                        itReturn = it;
                    }
                }

                return itReturn;
            }
        }

        public List<Models.Project> GetAllProjects()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                List<Models.Project> projects = db.Projects.ToList();
                return projects;
            }
        }

        public List<Period> GetAllPeriods()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                List<Period> periods = db.Periods.ToList();
                return periods;
            }
        }

        public List<Partners> GetAllPartners()
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                List<Partners> partners = db.Partners.ToList();
                return partners;
            }
        }

        public TeamworkRootCauseProblems GetRootCauseProblem(string abbreviation)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                return db.TeamworkRootCauseProblems.Where(root => root.Abbreviation == abbreviation).SingleOrDefault();
            }
        }

        public void AddTeamworkLogTrace(List<TeamworkLogTraces> logTraceList)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                foreach (var item in logTraceList.ToList())
                {
                    try
                    {
                        TeamworkLogTraces foundLogTrace = db.TeamworkLogTraces.Where(x => x.TeamWorkTodoItemId == item.TeamWorkTodoItemId).SingleOrDefault();
                        if (foundLogTrace == null)
                        {
                            db.TeamworkLogTraces.Add(item);
                            db.SaveChanges();
                        }
                        else
                        {
                            foundLogTrace.IsFixed = false;
                            foundLogTrace.Partner = item.Partner;
                            foundLogTrace.Creator = item.Creator;
                            foundLogTrace.Description = item.Description;
                            foundLogTrace.Project = item.Project;
                            //foundLogTrace.RootCauses = item.RootCauses;
                            foundLogTrace.TeamWorkTodoItemId = item.TeamWorkTodoItemId;
                            db.Entry(foundLogTrace).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    catch (Exception e)
                    {

                    }

                }
            }
        }

        public void SaveTimesheetItemsInTs(List<TimesheetTeamWorkItem> items)
        {
            using (TimesheetContext db = new TimesheetContext())
            {
                int mes = DateTime.Now.Month;

                foreach (var item in items)
                {

                    TimesheetTeamWorkItem foundItem =
                        db.TimesheetTeamWorkItems.Include("TimesheetItem").Where(x => x.TeamWorkTimeEntryId == item.TeamWorkTimeEntryId).SingleOrDefault();
                    if (foundItem != null)
                    {
                        try
                        {
                            foundItem.TeamWorkTimeEntryId = item.TeamWorkTimeEntryId;
                            foundItem.TeamWorkTodoItemId = item.TeamWorkTodoItemId;
                            foundItem.TimesheetItem.BREAK = item.TimesheetItem.BREAK;
                            foundItem.TimesheetItem.COUNTER = item.TimesheetItem.COUNTER;
                            foundItem.TimesheetItem.DATE = item.TimesheetItem.DATE;

                            if (item.TeamWorkTimeDescription != null)
                            {
                                foundItem.TimesheetItem.DESCRIPTION = "" + item.TeamWorkTimeDescription;
                            }
                            else
                            {
                                foundItem.TimesheetItem.DESCRIPTION = item.TimesheetItem.DESCRIPTION;
                            }
                            foundItem.TimesheetItem.IN = item.TimesheetItem.IN;
                            foundItem.TimesheetItem.OUT = item.TimesheetItem.OUT;
                            foundItem.TimesheetItem.project = item.TimesheetItem.project;
                            foundItem.TimesheetItem.TimesheetHeader = item.TimesheetItem.TimesheetHeader;
                            db.Entry(foundItem).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Util.EscreverLog(e.Message, e.Message);
                        }
                    }

                    else
                    {
                        if (item.TimesheetItem.DATE.Month == mes)
                        {
                            try
                            {

                                //db.Entry(item.TimesheetHeader.Partner).State = EntityState.Unchanged;
                                //db.Entry(item.project).State = EntityState.Unchanged;
                                //db.Entry(item.TimesheetHeader.Period).State = EntityState.Unchanged;

                                //db.Entry(item.TimesheetHeader).State = EntityState.Unchanged;
                                //db.TimesheetItems.Add(item);


                                //db.Entry(item.TimesheetItem.TimesheetHeader.Partner).State = EntityState.Unchanged;
                                //db.Entry(item.TimesheetItem.TimesheetHeader).State = EntityState.Unchanged;
                                //db.Entry(item.TimesheetItem.TimesheetHeader.Period).State = EntityState.Unchanged;
                                //db.Entry(item.TimesheetItem.project).State = EntityState.Unchanged;

                                db.TimesheetTeamWorkItems.Add(item);
                                db.SaveChanges();


                                //child.Parent = parent;
                                //context.Childs.Add(child); // both child and parent are marked as inserted!!!
                                //context.Entry(parent).State = EntityState.Unchanged; // set parent as unchanged
                                //context.SaveChanges();

                            }
                            catch (Exception e)
                            {
                                Util.EscreverLog(e.Message, e.Message);
                            }
                        }
                    }
                }

            }
        }


    }
}