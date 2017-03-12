using Apassos.DataAccess;
using Apassos.Models;
using Apassos.TeamWork.JsonObject;
using Apassos.TeamWork.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Handler
{
    public class TimesheetManager
    {

        private readonly string DEFAULT_BREAK_TIME = "00:00:00";
        private readonly string DEFAULT_NOTE = ".";
        private readonly string DEFAULT_TYPE = "R";
        private readonly string DEFAULT_TAG_PREFIX = "#:";
        private readonly string DEFAULT_TIME_PATTERN = "HH:mm:ss";

        private readonly int MINUTES_OF_A_HOUR = 60;
        private readonly string STATUS_COMPLETED = "completed";

        public List<TimesheetTeamWorkItem> InsertData( List<EntryTime> entryTimeList)
        {
            TimesheetManagerItem tsManagerItem = new TimesheetManagerItem();
            TimesheetItem tsItem;

            Period period ;
            Project project ;
            Partners partner ;
            List<TimesheetTeamWorkItem> listInfoObject = new List<TimesheetTeamWorkItem>();

            foreach (var item in entryTimeList)
            {
                project = new Project();
                period = new Period();
                partner = new Partners();

                //Pega a lista de Tags
                project = GetProject(item.Tags);

                //Pega o período
                period = GetPeriodByEntry(item.Date);

                //Pega o Parceiro
                partner = GetPartnerByEntry(item.PersonFirstName,item.PersonLastName);


                if (project != null)
                {
                    tsItem = tsManagerItem.CreateTimesheetItem(period, partner, project, item);
                    TimesheetTeamWorkItem teamWorkItem = new TimesheetTeamWorkItem();
                    if (tsItem != null)
                    {
                        teamWorkItem.TimesheetItem = tsItem;
                        teamWorkItem.TeamWorkTodoItemId = item.TodoItemId;
                        teamWorkItem.TeamWorkTimeEntryId = item.Id;
                        if (item.Description != null)
                        {
                            teamWorkItem.TeamWorkTimeDescription =  item.Description;
                        }
                        //teamWorkItem.TeamWorkTimeUser = entry.PersonFirstName;
                        listInfoObject.Add(teamWorkItem);
                    }
                }
            }

            return listInfoObject;
        }

        private Project GetProject(List<Tag> listTag)
        {
            int idByTag = GetTagById(listTag);

            ProjectDataAccess projectDataAccess = new ProjectDataAccess();

            Project project = projectDataAccess.GetProjeto(idByTag.ToString());

            return project;
        }

        private int GetTagById(List<Tag> listTag)
        {
            int idByTag = -1;

            foreach (var tag in listTag)
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

        private Period GetPeriodByEntry(string entryDate)
        {
            DateTime date = DateTime.Parse(entryDate);
            PeriodDataAccess _periods = new PeriodDataAccess();
            return _periods.GetPeriodoAtual();
        }

        private Partners GetPartnerByEntry(string FirstName, string LastName)
        {
            PartnerDataAccess p = new PartnerDataAccess();
            List<Partners> _partners = p.GetAllParceiros();

            if (FirstName.Contains('á'))
            {
                FirstName.Replace('á', 'a');
                int number = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).Count();
                if (number > 1)
                {
                    return _partners.Find(x => x.FIRSTNAME.Replace('á', 'a').ToUpper().Trim().Equals(FirstName.ToUpper().Trim()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper()));
                }
            }

            int numberPartner = _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).Count();


            if (numberPartner > 1)
            {
                return _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).FirstOrDefault();

            }

            return _partners.FindAll(x => x.FIRSTNAME.ToUpper().Trim().Equals(FirstName.ToUpper()) && x.LASTNAME.ToUpper().Trim().Equals(LastName.ToUpper())).FirstOrDefault();

        }

        private string SerializeObject(TimesheetItem tsItem)
        {
            string tsJson = JsonConvert.SerializeObject(tsItem);
            return tsJson;
        }

        private void InsertObjectJson(List<InfoObjects> listInfoObject)
        {
            InfoObjectsDataAccess infoDataAccess = new InfoObjectsDataAccess();
            infoDataAccess.InsertObjects(listInfoObject);
        }

    }
}