using System.Collections.Generic;
using Apassos.TeamWork.Services;
using Ninject;
using Apassos.IoC;
using TeamWorkNet.Model;
using Apassos.Models;
using System.Configuration;
using System.Diagnostics;
using System;
using Apassos.DataAccess;
using System.Globalization;
using TeamWorkNet.Response;
using TeamWorkNet.Base.Model;
using static Apassos.Models.TeamworkRootCauseProblems;

namespace Apassos.TeamWork.Parsers
{
    /***
     * 
     */
    public class TimesheetParser : ITimesheetParser
    {

        private readonly string DEFAULT_BREAK_TIME = "00:00:00";
        private readonly string DEFAULT_NOTE = ".";
        private readonly string DEFAULT_TYPE = "R";
        private readonly string DEFAULT_TAG_PREFIX = "#:";
        private readonly string DEFAULT_TIME_PATTERN = "HH:mm:ss";

        private readonly int MINUTES_OF_A_HOUR = 60;
        private readonly string STATUS_COMPLETED = "completed";

        [Inject]
        private ITeamWorkService _teamWorkService { set; get; }

        private IKernel _kernel;

        private string _enviroment;

        private List<Period> _periods;

        private List<Partners> _partners;

        private List<Models.Project> _projects;

        private List<TeamworkLogTraces> _logTraceList;

        public TimesheetParser(ITeamWorkService service)
        {
            _kernel = new StandardKernel(new TimesheetNinjectModule());
            _teamWorkService = _kernel.Get<ITeamWorkService>();

            _enviroment = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            _periods = GetAllPeriods();
            _partners = GetAllPartners();
            _projects = GetAllProjects();
        }

        public List<TimesheetTeamWorkItem> GetItems()
        {
            _logTraceList = new List<TeamworkLogTraces>();
            int diaDoMes = DateTime.Now.Month;
            List<TodoItem> todoItems = _teamWorkService.GetAllTodoItems();
            int mesInicialTarefa;

            List<TimesheetTeamWorkItem> items = new List<TimesheetTeamWorkItem>();

            foreach (var todoItem in todoItems)
            {
            
                try
                {

                    //Filtro para não salvar tarefas antigas, essas tarefas serão filtradas a partir da data que está corrente
                    mesInicialTarefa = todoItem.StartDate.Month;

                    if (mesInicialTarefa >= (diaDoMes))
                    {
                        if (todoItem.ResponsiblePartyNames.Contains("Danilo S.") || todoItem.ResponsiblePartyNames.Contains("Willian L.") || todoItem.ResponsiblePartyNames.Contains("Josias L.") || todoItem.ResponsiblePartyNames.Contains("Jorge L.") || todoItem.ResponsiblePartyNames.Contains("Andreson M.") || todoItem.ResponsiblePartyNames.Contains("Wanderson H.") || todoItem.ResponsiblePartyNames.Contains("Evandro C.") || todoItem.ResponsiblePartyNames.Contains("Djanildes A.") || todoItem.ResponsiblePartyNames.Contains("Jaime N.") || todoItem.ResponsiblePartyNames.Contains("Carlos X.")
                        || todoItem.ResponsiblePartyNames.Contains("Paulo P.") || todoItem.ResponsiblePartyNames.Contains("Waldir T.") || todoItem.ResponsiblePartyNames.Contains("Sandra G.") || todoItem.ResponsiblePartyNames.Contains("Dennis C.") || todoItem.ResponsiblePartyNames.Contains("Rodrigo L.") || todoItem.ResponsiblePartyNames.Contains("Carlos R.") || todoItem.ResponsiblePartyNames.Contains("Bruno R.") || todoItem.ResponsiblePartyNames.Contains("Eduardo C.") || todoItem.ResponsiblePartyNames.Contains("Matheus F.") || todoItem.ResponsiblePartyNames.Contains("Rayden F.")
                        || todoItem.ResponsiblePartyNames.Contains("Stella W.") || todoItem.ResponsiblePartyNames.Contains("Nonato O.") || todoItem.ResponsiblePartyNames.Contains("Fernando P.") || todoItem.ResponsiblePartyNames.Contains("Fábio D.") || todoItem.ResponsiblePartyNames.Contains("Fábio A."))

                        {
                            try
                            {
                                if (!todoItem.Status.Equals(STATUS_COMPLETED))
                                {
                                    List<TimesheetTeamWorkItem> nonCompletedItems = NonCompletedTodoItem(todoItem);
                                    if (nonCompletedItems.Count > 0)
                                    {
                                        items.AddRange(nonCompletedItems);
                                    }
                                }
                                else
                                {
                                    List<TimesheetTeamWorkItem> completedItems = CompletedTodoItem(todoItem);
                                    if (completedItems.Count > 0)
                                    {
                                        items.AddRange(completedItems);
                                    }
                                }
                            }
                            catch (System.NullReferenceException e)
                            {
                                Console.WriteLine(e.Message);
                            }

                        }
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine("" + e.Message);
                }
            }
            //TimesheetDataAccess.AddTeamworkLogTrace(_logTraceList);
            return items;
        }


        private List<TimesheetTeamWorkItem> CompletedTodoItem(TodoItem todoItem)
        {
            int diaDoMes = DateTime.Now.Month;
            List<TimesheetTeamWorkItem> items = new List<TimesheetTeamWorkItem>();

            TeamworkLogTraces logTrace = new TeamworkLogTraces();
            logTrace.RootCauses = new List<TeamworkLogTraceRootCauses>();
            logTrace.TeamWorkTodoItemId = todoItem.Id;
            logTrace.Description = todoItem.Description;
            logTrace.Creator = todoItem.CreatorFirstname;
            logTrace.Titulo = todoItem.Content;

            Period period = GetPeriodByTodoItem(todoItem);
            Partners partner = GetPartnerByTodoItem(todoItem);
            int idByTag = GetTagByTodoItem(todoItem);
            Models.Project projectTodoItem = GetProjectByTags(idByTag);

            if (ValidObjects(period, partner, projectTodoItem))
            {
                TimeEntriesResponse response = _teamWorkService.GetSingleTimeEntry(int.Parse(todoItem.Id));
                if (response.TimeEntries.Length > 0)
                {

                    foreach (var entry in response.TimeEntries)
                    {
                        string[] mes = entry.Date.Split(('-'));

                        //if (int.Parse(mes[1]) >= (diaDoMes-1))
                        if (int.Parse(mes[1]) == 12 || int.Parse(mes[1]) == 1)

                        {
                            TimesheetItem item = CreateTimesheetItem(todoItem, period, partner, projectTodoItem, entry);

                            TimesheetTeamWorkItem teamWorkItem = new TimesheetTeamWorkItem();
                            if (item != null)
                            {
                                teamWorkItem.TimesheetItem = item;
                                teamWorkItem.TeamWorkTodoItemId = todoItem.Id;
                                teamWorkItem.TeamWorkTimeEntryId = entry.Id;
                                if (entry.Description != null)
                                {
                                    teamWorkItem.TeamWorkTimeDescription = entry.Description;
                                }
                                //teamWorkItem.TeamWorkTimeUser = entry.PersonFirstName;
                                items.Add(teamWorkItem);
                            }
                        }
                    }
                }
                else
                {
                    NoEntriesRootCause(logTrace);
                }
            }
            else
            {
                if (idByTag == -1)
                {
                    NoTagRootCause(logTrace);
                }
                if (partner == null)
                {
                    NoUserRootCause(logTrace);
                }
            }

            if (logTrace.RootCauses.Count > 0)
            {
                _logTraceList.Add(logTrace);
            }

            return items;
        }

        private void NoTagRootCause(TeamworkLogTraces logTrace)
        {
            TeamworkRootCauseProblems rootCause = GetRootCauseProblem(EnumRootCauseProblems.NO_TAG);
            AddLogTraceRootCause(logTrace, rootCause);
        }


        private void NoUserRootCause(TeamworkLogTraces logTrace)
        {
            TeamworkRootCauseProblems rootCause = GetRootCauseProblem(EnumRootCauseProblems.NO_USER);
            AddLogTraceRootCause(logTrace, rootCause);
        }

        private void NoEntriesRootCause(TeamworkLogTraces logTrace)
        {
            TeamworkRootCauseProblems rootCause = GetRootCauseProblem(EnumRootCauseProblems.NO_ENTRIES);
            AddLogTraceRootCause(logTrace, rootCause);
        }

        private static void AddLogTraceRootCause(TeamworkLogTraces logTrace, TeamworkRootCauseProblems rootCause)
        {
            TeamworkLogTraceRootCauses logTraceRootCause = new TeamworkLogTraceRootCauses();
            logTraceRootCause.TeamworkRootCauseProblems = rootCause;
            logTrace.RootCauses.Add(logTraceRootCause);
        }

        private TeamworkRootCauseProblems GetRootCauseProblem(EnumRootCauseProblems rootCause)
        {
            return TimesheetDataAccess.GetRootCauseProblem(rootCause.ToString());
        }

        private List<TimesheetTeamWorkItem> NonCompletedTodoItem(TodoItem todoItem)
        {
            List<TimesheetTeamWorkItem> items = new List<TimesheetTeamWorkItem>();
            TimeEntriesResponse response = _teamWorkService.GetSingleTimeEntry(int.Parse(todoItem.Id));
            int diaDoMes = DateTime.Now.Month;
            if (response.TimeEntries.Length > 0)
            {
                foreach (var entry in response.TimeEntries)
                {
                    string[] mes = entry.Date.Split(('-'));

                    //if (int.Parse(mes[1]) >= (diaDoMes-1))

                    if (int.Parse(mes[1]) == 1 || int.Parse(mes[1]) == 12)
                    {
                        Period period = GetPeriodByEntry(entry);
                        Partners partner = GetPartnerByEntry(entry);
                        int idByTag = GetTagByTodoItem(todoItem);
                        Models.Project projectTodoItem = GetProjectByTags(idByTag);
                        if (ValidObjects(period, partner, projectTodoItem))
                        {
                            TimesheetItem item = CreateTimesheetItem(todoItem, period, partner, projectTodoItem, entry);

                            TimesheetTeamWorkItem teamWorkItem = new TimesheetTeamWorkItem();
                            if (item != null)
                            {
                                teamWorkItem.TimesheetItem = item;
                                teamWorkItem.TeamWorkTodoItemId = todoItem.Id;
                                teamWorkItem.TeamWorkTimeEntryId = entry.Id;
                                if (entry.Description != null)
                                {
                                    teamWorkItem.TeamWorkTimeDescription = entry.Description;
                                }
                                else
                                {
                                    teamWorkItem.TeamWorkTimeDescription = "A";
                                }
                                items.Add(teamWorkItem);
                            }
                        }
                    }
                }
            }
            else
            {
                TeamworkLogTraces logTrace = new TeamworkLogTraces();
                logTrace.RootCauses = new List<TeamworkLogTraceRootCauses>();
                logTrace.TeamWorkTodoItemId = todoItem.Id;
                if (logTrace.Description != null || logTrace.Description != string.Empty)
                {
                    logTrace.Description = todoItem.Description;
                }
                else
                {
                    logTrace.Description = "Descrição de atividade gerada automaticamente";
                }
                Partners parceiro;
                if (todoItem.ResponsiblePartyFirstname.Equals(string.Empty))
                {
                    parceiro = PartnerDataAccess.GetParceiroName(todoItem.ResponsiblePartyFirstname.ToString());
                    logTrace.Partner = parceiro.PARTNERID;
                }
                else
                {
                    parceiro = PartnerDataAccess.GetParceiroName("josias");
                    logTrace.Partner = parceiro.PARTNERID;
                }

                logTrace.Creator = todoItem.CreatorFirstname;
                logTrace.Titulo = todoItem.Content;
                int idByTag = GetTagByTodoItem(todoItem);
                if (idByTag == -1)
                {
                    NoTagRootCause(logTrace);
                    _logTraceList.Add(logTrace);
                }

            }
            return items;
        }

        private TimeSpan GetTotalHoursAndMinutes(TimeEntriesResponse response)
        {
            TimeEntry[] entries = response.TimeEntries;
            int fullTotalMinutes = 0;
            foreach (var item in entries)
            {
                int hoursToMinutes = int.Parse(item.Hours) * MINUTES_OF_A_HOUR;
                int totalMinutes = hoursToMinutes + int.Parse(item.Minutes);
                fullTotalMinutes += totalMinutes;
            }
            TimeSpan countedTime = new TimeSpan(0, fullTotalMinutes, 0);
            return countedTime;
        }

        private TimeSpan GetTotalHoursAndMinutesByEntries(TimeEntry entry)
        {
            int fullTotalMinutes = 0;
            int hoursToMinutes = int.Parse(entry.Hours) * MINUTES_OF_A_HOUR;
            fullTotalMinutes += hoursToMinutes + int.Parse(entry.Minutes); ;

            TimeSpan countedTime = new TimeSpan(0, fullTotalMinutes, 0);
            return countedTime;
        }

        private TimesheetItem CreateTimesheetItem(TodoItem todoItem, Period period, Partners partner, Models.Project projectTodoItem, TimeEntry entry)
        {

            TimesheetHeader header = GetHeader(period, partner);


            //DateTime endDate = DateTime.Parse(entry.Date);
            //DateTime initialDate = endDate.Subtract(new TimeSpan(0, totalMinutes, 0));
            //int totalMinutes = (int.Parse(entry.Hours) * MINUTES_OF_A_HOUR) + int.Parse(entry.Minutes);
            //initialDate = initialDate.ToUniversalTime();
            //string initialDateAsString = initialDate.ToString(DEFAULT_TIME_PATTERN, CultureInfo.InvariantCulture);
            //string endDateAsString = endDate.ToString(DEFAULT_TIME_PATTERN, CultureInfo.InvariantCulture);

            DateTime initialDate = DateTime.Parse(entry.Date);
            int totalMinutes = (int.Parse(entry.Hours) * MINUTES_OF_A_HOUR) + int.Parse(entry.Minutes);
            DateTime finalDate = initialDate.Add(new TimeSpan(0, totalMinutes, 0));
            string initialDateAsString = initialDate.ToString(DEFAULT_TIME_PATTERN);
            string finalDateAsString = finalDate.ToString(DEFAULT_TIME_PATTERN);

            TimesheetItem item = new TimesheetItem();
            item.ENVIRONMENT = _enviroment;
            item.TimesheetHeader = header;
            item.project = projectTodoItem;
            item.COUNTER = 1;
            item.DESCRIPTION = todoItem.Content;
            item.TYPE = DEFAULT_TYPE;
            item.NOTE = DEFAULT_NOTE;
            item.BREAK = TimeSpan.Parse(DEFAULT_BREAK_TIME);
            item.STATUS = 0;
            item.DATE = DateTime.Parse(entry.Date);
            item.IN = TimeSpan.Parse(initialDateAsString);
            item.OUT = TimeSpan.Parse(finalDateAsString);
            item.CreationDate = DateTime.Now;
            return item;
        }

        private TimesheetHeader GetHeader(Period period, Partners partner)
        {
            TimesheetHeader header = TimesheetDataAccess.GetApontamentoCabecalhoPorPeriodo(partner, period);
            if (header == null)
            {
                header = new TimesheetHeader();
                header.ENVIRONMENT = _enviroment;
                header.Period = period;
                header.Partner = partner;
            }

            return header;
        }


        private List<Period> GetAllPeriods()
        {
            List<Period> periods = TimesheetDataAccess.GetAllPeriods();
            return periods;
        }

        private List<Models.Project> GetAllProjects()
        {
            List<Models.Project> projects = TimesheetDataAccess.GetAllProjects();
            return projects;
        }

        private List<Partners> GetAllPartners()
        {
            List<Partners> partners = TimesheetDataAccess.GetAllPartners();
            return partners;
        }

        private Partners GetPartnerByTodoItem(TodoItem todoItem)
        {
            //return _partners.Find(x => x.FIRSTNAME.ToUpper() == todoItem.CompleterFirstname.ToUpper()
            //                                && x.LASTNAME.ToUpper() == todoItem.CompleterLastname.ToUpper());


            todoItem.ResponsiblePartyFirstname = todoItem.ResponsiblePartyFirstname.Replace('á', 'a');

            List<Partners> p = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(todoItem.ResponsiblePartyFirstname.ToUpper()));

            if (p.Count > 1)
            {
                if (todoItem.ResponsiblePartyFirstname.Equals("Fabio"))
                {
                    if (todoItem.ResponsiblePartyLastname.ToUpper().Trim().Equals("DEALMEIDA"))
                    {
                        return _partners.Find(x => x.LASTNAME.ToUpper().Trim().Equals("ALMEIDA"));
                    }
                    else
                    {
                        return _partners.Find(x => x.LASTNAME.ToUpper().Trim().Equals("ALVES"));
                    }
                }
                //return _partners.Find(x => x.NAME.Replace('á', 'a').ToUpper().Trim().Equals(entry.PersonFirstName.ToUpper().Trim()+entry.PersonLastName.ToUpper().Trim()));
            }
            return _partners.Find(x => x.FIRSTNAME.ToUpper().Trim().Equals(todoItem.ResponsiblePartyFirstname.ToUpper()));

        }

        //Verificar esse trecho de código que pode trazer futuros erros
        private Partners GetPartnerByEntry(TimeEntry entry)
        {
            //return _partners.Find(x => x.FIRSTNAME.ToUpper() == entry.PersonFirstName.ToUpper()
            //                                && x.LASTNAME.ToUpper() == entry.PersonLastName.ToUpper());

            entry.PersonFirstName = entry.PersonFirstName.Replace('á', 'a');

            List<Partners> p = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(entry.PersonFirstName.ToUpper()));

            if (p.Count > 1)
            {
                if (entry.PersonFirstName.Equals("Fabio"))
                {
                    if (entry.PersonLastName.ToUpper().Trim().Equals("DEALMEIDA"))
                    {
                        return _partners.Find(x => x.LASTNAME.ToUpper().Trim().Equals("ALMEIDA"));
                    }
                    else
                    {
                        return _partners.Find(x => x.LASTNAME.ToUpper().Trim().Equals("ALVES"));
                    }
                }
                //return _partners.Find(x => x.NAME.Replace('á', 'a').ToUpper().Trim().Equals(entry.PersonFirstName.ToUpper().Trim()+entry.PersonLastName.ToUpper().Trim()));
            }
            return _partners.Find(x => x.FIRSTNAME.ToUpper().Trim().Equals(entry.PersonFirstName.ToUpper()));
        }

        private Period GetPeriodByTodoItem(TodoItem todoItem)
        {
            DateTime endDate = DateTime.Parse(todoItem.completedOn);
            return _periods.Find(x => x.YEAR == endDate.Year && x.MONTH == endDate.Month);
        }

        private Period GetPeriodByEntry(TimeEntry entry)
        {
            DateTime date = DateTime.Parse(entry.Date);
            return _periods.Find(x => x.YEAR == date.Year && x.MONTH == date.Month);
        }

        private int GetTagByTodoItem(TodoItem todoItem)
        {

            int idByTag = -1;

            foreach (var tag in todoItem.Tags)
            {
                if (tag.Name.Contains(DEFAULT_TAG_PREFIX))
                {
                    int indexTag = tag.Name.IndexOf(':');
                    string tagName = tag.Name.Substring(indexTag + 1);

                    int index = tagName.IndexOf('-');

                    if (index >= 0)
                    {
                        string newTag = tagName.Substring(0, index);
                        try
                        {
                            idByTag = int.Parse(newTag.Trim());
                        }
                        catch (Exception e)
                        {
                            Debug.Write(e.StackTrace);
                        }
                    }
                }
            }

            return idByTag;
        }

        private Models.Project GetProjectByTags(int idByTag)
        {
            Models.Project project = new Models.Project();
            try
            {
                project = _projects.Find(x => x.PROJECTID == idByTag);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            return project;
        }

        private bool ValidObjects(Period period, Partners partner, Models.Project projectTodoItem)
        {
            bool valid = true;

            if (period == null)
            {
                valid = false;
            }

            if (partner == null)
            {
                valid = false;
            }

            if (projectTodoItem == null)
            {
                valid = false;
            }

            return valid;
        }

    }
}