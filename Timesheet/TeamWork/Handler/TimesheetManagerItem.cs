using Apassos.Common;
using Apassos.Models;
using Apassos.Observer;
using Apassos.TeamWork.Exceptions;
using Apassos.TeamWork.JsonObject;
using Apassos.TeamWork.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Handler
{
    public class TimesheetManagerItem
    {

        private readonly string DEFAULT_BREAK_TIME = "00:00:00";
        private readonly string DEFAULT_NOTE = ".";
        private readonly string DEFAULT_TYPE = "R";
        private readonly string DEFAULT_TIME_PATTERN = "HH:mm:ss";
        private readonly string ENVIRONMENT = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
        private readonly int MINUTES_OF_A_HOUR = 60;
        private readonly string TAG_PROBLEM = "Não";
        private readonly string DESCRIPTION_PROBLEM = "Não";
        private readonly string TAG_PROBLEM_IS_HAPPENING = "Sim";
        private readonly string DESCRIPTION_PROBLEM_IS_HAPPENING = "Sim";
        private readonly string[] DESCRIPTIONSTATUS = {
            "Atividade atualizada e migrada com sucesso",
            "Atividade inserida e migrada com sucesso",
            "Erro ao atualizar atividade",
            "Erro ao inserir atividade",
            "Erro ,pois não contém TAG ou Descrição" };
        private readonly string APPROVEDSTATUS = "Atividade aprovada pelo Gestor";

        public void CreateTimesheetItem(Period period, Partners partner, Project projectTodoItem, EntryTime entry, Logs log)
        {

            TimesheetItem item;
            List<InfoObjects> teamWorkInfoObject = new List<InfoObjects>();

            TimesheetTeamWorkItem teamWorkItem ;

            GenericExceptions exceptions;

            using (TimesheetContext db = new TimesheetContext())
            {

                teamWorkItem = new TimesheetTeamWorkItem();

                if (projectTodoItem != null && entry.Description != null && entry.Description != string.Empty)
                {
                    #region
                    TimesheetHeader header = db.TimesheetHeaders.Where(th => th.ENVIRONMENT == ENVIRONMENT && th.Partner.PARTNERID == partner.PARTNERID && th.Period.PERIODID == period.PERIODID).FirstOrDefault();

                    #region
                    if (header == null)
                    {

                        header = new TimesheetHeader();
                        var env = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
                        header.ENVIRONMENT = env;
                        //header.CHANGEDATE = DateTime.Now.Date;
                        //header.CHANGEDBY = "TimesheetService";
                        //header.CREATEDBY = "TimesheetService";
                        //header.CREATIONDATE = DateTime.Now.Date;
                        header.Period = db.Periods.Where(p => p.PERIODID == period.PERIODID).FirstOrDefault();
                        header.Partner = db.Partners.Where(p => p.PARTNERID == partner.PARTNERID).FirstOrDefault();
                        //db.TimesheetHeaders.Add(header);
                        //db.SaveChanges();

                    }

                    DateTime initialDate = DateTime.Parse(entry.Date);
                    int totalMinutes = (int.Parse(entry.Hours) * MINUTES_OF_A_HOUR) + int.Parse(entry.Minutes);
                    DateTime finalDate = initialDate.Add(new TimeSpan(0, totalMinutes, 0));
                    string initialDateAsString = initialDate.ToString(DEFAULT_TIME_PATTERN);
                    string finalDateAsString = finalDate.ToString(DEFAULT_TIME_PATTERN);

                    item = new TimesheetItem();
                    item.ENVIRONMENT = ENVIRONMENT;

                    item.project = db.Projects.Where(x => x.PROJECTID == projectTodoItem.PROJECTID).FirstOrDefault();

                    item.COUNTER = 1;
                    item.DESCRIPTION = "TW: " + entry.Description;
                    item.TYPE = DEFAULT_TYPE;
                    item.NOTE = DEFAULT_NOTE;
                    item.BREAK = TimeSpan.Parse(DEFAULT_BREAK_TIME);
                    item.STATUS = 0;
                    item.DATE = DateTime.Parse(entry.Date);
                    item.IN = TimeSpan.Parse(initialDateAsString);
                    item.OUT = TimeSpan.Parse(finalDateAsString);
                    item.CreationDate = DateTime.Now.Date;
                    item.TimesheetHeader = db.TimesheetHeaders.Find(header.TIMESHEETHEADERID) ?? header;

                    teamWorkItem = new TimesheetTeamWorkItem();
                    if (item != null)
                    {
                        #region
                        teamWorkItem.TimesheetItem = item;
                        teamWorkItem.TeamWorkTodoItemId = entry.Id;
                        teamWorkItem.TeamWorkTimeEntryId = entry.Id;
                        #endregion
                    }
                    #endregion
                    //Inicializa os dados do Log
                    log.ActivityDate = ConvertTime(entry.Date);
                    log.ActivityDescription = entry.Description;
                    log.TagProblem = TAG_PROBLEM;
                    log.DescriptionProblem = DESCRIPTION_PROBLEM;
                    log.Data = DateTime.UtcNow.Date;
                    log.ProjectTW = entry.ProjectName;
                    log.TeamWorkTimeEntryId = entry.Id;
                    log.ConsultorId = partner.PARTNERID;
                    log.PeriodoId = period.PERIODID;
                    //Se o projeto for diferente nulo, ele insere os itens
                    //Investiga se o item já existe 
                    #region
                    TimesheetTeamWorkItem foundItem =
                            db.TimesheetTeamWorkItems.Include("TimesheetItem").Where(x => x.TeamWorkTimeEntryId == teamWorkItem.TeamWorkTimeEntryId).SingleOrDefault();

                    exceptions = new GenericExceptions();

                    int actualMonth = DateTime.Now.Month;

                    if (foundItem != null)
                    {

                        try
                        {


                            if ( (period.STATUS.Equals("a") || (period.STATUS.Equals("c"))) && (foundItem.TimesheetItem.STATUS != 1)  )
                            {
                                exceptions.PeriodIsClosed(header.Period.STATUS);
                                exceptions.SheetIsApproved(foundItem.TimesheetItem.STATUS);


                                TimesheetManagerHeader manager = new TimesheetManagerHeader();

                                int contaApontamentosAprovados = manager.AlgumNaoAprovado(db, foundItem);

                                if (contaApontamentosAprovados == 1)
                                {



                                    foundItem.TeamWorkTimeEntryId = teamWorkItem.TeamWorkTimeEntryId;
                                    foundItem.TeamWorkTodoItemId = teamWorkItem.TeamWorkTodoItemId;
                                    foundItem.TimesheetItem.BREAK = teamWorkItem.TimesheetItem.BREAK;
                                    foundItem.TimesheetItem.COUNTER = teamWorkItem.TimesheetItem.COUNTER;
                                    foundItem.TimesheetItem.DATE = teamWorkItem.TimesheetItem.DATE;

                                    if (teamWorkItem.TeamWorkTimeDescription != null)
                                    {
                                        foundItem.TimesheetItem.DESCRIPTION = "" + teamWorkItem.TeamWorkTimeDescription;
                                    }
                                    else
                                    {
                                        foundItem.TimesheetItem.DESCRIPTION = teamWorkItem.TimesheetItem.DESCRIPTION;
                                    }
                                    foundItem.TimesheetItem.IN = teamWorkItem.TimesheetItem.IN;
                                    foundItem.TimesheetItem.OUT = teamWorkItem.TimesheetItem.OUT;
                                    foundItem.TimesheetItem.project = teamWorkItem.TimesheetItem.project;
                                    foundItem.TimesheetItem.TimesheetHeader = teamWorkItem.TimesheetItem.TimesheetHeader;
                                    log.TimesheetItem = foundItem.TimesheetItem.TIMESHEETITEMID;
                                    db.Entry(foundItem).State = EntityState.Modified;
                                    db.SaveChanges();
                                    log.Description = DESCRIPTIONSTATUS[0];
                                }
                                else
                                {
                                    log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                                }
                            }
                            else
                            {
                                log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                            }
                            
                        }
                        catch (Exception e)
                        {
                            //Util.EscreverLog(e.Message, e.Message);
                            log.Description =  e.Message;

                        }
                    }
                    #endregion
                    //Se não existe ele insere no banco
                    #region
                    else
                    {
                        try
                        {
                            if ((period.STATUS.Equals("a")) || (period.STATUS.Equals("c")))
                            {


                                TimesheetManagerHeader manager2 = new TimesheetManagerHeader();

                                int contaApontamentosAprovados = manager2.AlgumNaoAprovadoComNovoApontamento(db, header);

                                if (contaApontamentosAprovados == 1)
                                {

                                    db.TimesheetTeamWorkItems.Add(teamWorkItem);
                                    db.SaveChanges();
                                    log.Description = DESCRIPTIONSTATUS[1];
                                    log.TimesheetItem = item.TIMESHEETITEMID;
                                }
                                else
                                {
                                    log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                                    log.TimesheetItem = item.TIMESHEETITEMID;
                                }
                            }
                            else
                            {
                                log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                                log.TimesheetItem = item.TIMESHEETITEMID;
                            }

                        }
                        catch (Exception e)
                        {
                            exceptions.PeriodIsClosed(header.Period.STATUS);
                            log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                            log.TimesheetItem = item.TIMESHEETITEMID;
                            Util.EscreverLog(e.Message, e.Message);
                            log.Description =  e.Message;
                        }
                    }
                    #endregion

                    #endregion
                }

                if (projectTodoItem == null)
                {
                    #region
                    //Inicializa os dados do Log caso a tag não exista

                    log.ActivityDate = ConvertTime(entry.Date);
                    if (entry.Description.Equals(string.Empty) || entry.Description == null)
                    {
                        log.ActivityDescription = "NÃO EXISTE DESCRIÇÃO NO LOG DE TEMPO";
                        log.DescriptionProblem = DESCRIPTION_PROBLEM_IS_HAPPENING;
                    }
                    else
                    {
                        log.ActivityDescription = entry.Description;
                        log.DescriptionProblem = DESCRIPTION_PROBLEM;
                    }
                    log.Description = DESCRIPTIONSTATUS[4];
                    log.Data = DateTime.UtcNow.Date;
                    log.ProjectTW = entry.ProjectName;
                    log.TagProblem = TAG_PROBLEM_IS_HAPPENING;
                    log.TeamWorkTimeEntryId = entry.Id;
                    log.TimesheetItem = null;
                    log.PeriodoId = period.PERIODID;
                    log.ConsultorId = partner.PARTNERID;
                    #endregion
                }

                if (entry.Description.Equals(""))
                {
                    #region
                    //Inicializa os dados do Log caso a tag não exista

                    log.ActivityDate = ConvertTime(entry.Date);
                    if (entry.Description.Equals(string.Empty) || entry.Description == null)
                    {
                        log.ActivityDescription = "NÃO EXISTE DESCRIÇÃO NO LOG DE TEMPO";
                        log.DescriptionProblem = DESCRIPTION_PROBLEM_IS_HAPPENING;
                    }
                    else
                    {
                        log.ActivityDescription = entry.Description;
                        log.DescriptionProblem = DESCRIPTION_PROBLEM;
                    }
                    log.Description = DESCRIPTIONSTATUS[4];
                    log.Data = DateTime.UtcNow.Date;
                    log.ProjectTW = entry.ProjectName;
                    if (projectTodoItem != null)
                    {
                        log.TagProblem = TAG_PROBLEM;
                    }
                    else
                    {
                        log.TagProblem = TAG_PROBLEM_IS_HAPPENING;

                    }
                    log.TeamWorkTimeEntryId = entry.Id;
                    log.TimesheetItem = null;
                    log.PeriodoId = period.PERIODID;
                    log.ConsultorId = partner.PARTNERID;
                    #endregion
                }

                InsertOrUpdateLog(log, db, projectTodoItem);

            }

        }

        public void InsertOrUpdateLog(Logs log, TimesheetContext db, Project project)
        {

            Logs foundItem =
                     db.Logs.Where(x => x.TeamWorkTimeEntryId == log.TeamWorkTimeEntryId).SingleOrDefault();
            #region
            if (foundItem != null)
            {
                try
                {
                    foundItem.ActivityDescription = log.ActivityDescription;
                    foundItem.ActivityDate = log.ActivityDate;
                    foundItem.Data = log.Data;
                    if (log.Description == null)
                    {
                        log.Description = APPROVEDSTATUS;
                    }
                    foundItem.Description = log.Description;
                    foundItem.DescriptionProblem = log.DescriptionProblem;
                    foundItem.TagProblem = log.TagProblem;
                    foundItem.TeamWorkTimeEntryId = log.TeamWorkTimeEntryId;
                    foundItem.ProjectTW = log.ProjectTW;
                    foundItem.ConsultorId = log.ConsultorId;
                    foundItem.PeriodoId = log.PeriodoId;
                    if (project == null)
                    {
                        db.Entry(foundItem).State = EntityState.Modified;
                    }
                    else
                    {
                        foundItem.TimesheetItem = log.TimesheetItem;
                        db.Entry(foundItem).State = EntityState.Modified;
                    }

                    if (foundItem.TagProblem.Equals("Não") && foundItem.DescriptionProblem.Equals("Não"))
                    {
                        foundItem.Status = 1;
                    }
                    else
                    {
                        log.Status = 2;
                    }

                    if (log.Description == null)
                    {
                        log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                    }

                    if (log.Description.Contains("O período está Fechado. Entre em contato com o Administrador") || log.Description.Contains("Atividade não migrada. O apontamento já foi aprovado pelo Gestor"))
                    {
                        log.Status = 2;
                    }

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Util.EscreverLog(e.Message, e.Message);
                }
            }
            #endregion

            //Se não existe ele insere no banco
            #region
            else
            {
                try
                {

                    if (log.TagProblem == null)
                    {
                        log.TagProblem = "Sim";
                        log.Status = 2;

                    }
                    if (log.DescriptionProblem == null)
                    {
                        log.DescriptionProblem = "Sim";
                        log.Status = 2;

                    }

                    if (log.TagProblem.Equals("Não") && log.DescriptionProblem.Equals("Não"))
                    {
                        log.Status = 1;
                    }
                    else
                    {
                        log.Status = 2;
                    }

                    if (log.Description == null)
                    {
                        log.Description = "Atividade não migrada. O apontamento já foi aprovado pelo Gestor";
                    }


                    if (log.Description.Contains("O período está Fechado. Entre em contato com o Administrador") || log.Description.Contains("Atividade não migrada. O apontamento já foi aprovado pelo Gestor"))
                    {
                        log.Status = 2;
                    }

                    db.Logs.Add(log);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Util.EscreverLog(e.Message, e.Message);
                }
            }
            #endregion

        }


        public DateTime ConvertTime(string entryDate)
        {
            DateTime datePort = DateTime.ParseExact(entryDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            return datePort;
        }
    }
}