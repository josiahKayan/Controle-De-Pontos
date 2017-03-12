using System.Collections.Generic;
using Apassos.Models;
using TeamWorkNet.Base;
using TeamWorkNet.Handler;
using TeamWorkNet.Model;
using TeamWorkNet.Response;
using Apassos.TeamWork.Handler;
using System;
using System.IO;
using System.Diagnostics;

namespace Apassos.TeamWork.Services
{
    public class TeamWorkService : ITeamWorkService
    {
        //Paulo
        private readonly string APIKEY = "dog671silk";
        //Josias
        //private readonly string APIKEY = "memory897elbow";
        //DJ
        //private readonly string APIKEY = "crimson218frog";
        private readonly string DOMAIN = "apassosconsultingsoftware";
        private readonly string DEFAULT_PATTERN_DATETIME = "yyyyMMdd";
        private readonly int MAX_PAGES = 100000000;
        private readonly int DEFAULT_DAYS = -1;


        private readonly TeamWorkClient _teamWorkClient;
        private readonly TaskHandler _taskHandler;
        private readonly PeopleHandler _peopleHandler;

        private readonly TSProjectHandler _tsProjectHandler;
        private readonly TSTimeHandler _tsTimeHandler;


        public TeamWorkService()
        {
            _teamWorkClient = new TeamWorkClient();
            _teamWorkClient.Init(APIKEY, DOMAIN);
            _taskHandler = new TaskHandler(_teamWorkClient);
            _peopleHandler = new PeopleHandler(_teamWorkClient);

            _tsTimeHandler = new TSTimeHandler(_teamWorkClient);
            _tsProjectHandler = new TSProjectHandler(_teamWorkClient);
        }

        public TodoItemResponse GetTag(int tagId)
        {
            TodoItemResponse response = _taskHandler.GetTagAsync(tagId).Result;
            return response;
        }

        public TimeTotalsResponse GetTotalHoursProject(int projectId)
        {
            TimeTotalsResponse response = _tsTimeHandler.GetTotals_Project(projectId).Result;
            return response;
        }

        public PeopleResponse GetAllPeople()
        {
            PeopleResponse response = _peopleHandler.GetAllPeople(MAX_PAGES).Result;
            return response;
        }

        public List<TodoItem> GetAllTodoItems()
        {
            List<TodoItem> todoItems = new List<TodoItem>();
            ProjectsResponse projectsResponse = GetAllProjects();

            foreach (var projectItem in projectsResponse.projects)
            {
                int projectId = int.Parse(projectItem.id);
                TeamWorkNet.Model.Project project = GetProjectData(projectId).project;

                List<TodoList> todoList = GetProjectTodoList(project);

                foreach (var item in todoList)
                {
                    todoItems.AddRange(item.TodoItems);
                }
            }

            return todoItems;
        }

        private ProjectsResponse GetAllProjects()
        {
            ProjectsResponse response = _tsProjectHandler.GetAllProjects();
            return response;
        }

        private ProjectResponse GetProjectData(int projectId)
        {
            DateTime now = DateTime.UtcNow;
            string startDate = now.AddDays(DEFAULT_DAYS).ToString(DEFAULT_PATTERN_DATETIME);

            ProjectResponse response = _tsProjectHandler.GetProject(projectId, startDate).Result;

            return response;
        }

        private List<TodoList> GetProjectTodoList(TeamWorkNet.Model.Project project)
        {
            return project.Tasklists;
        }

        public TimeEntriesResponse GetSingleTimeEntry(int id)
        {
            //return _tsTimeHandler.GetSingleTimeEntry(id).Result;
            return null;
        }


        public void salvaProjeto(string texto)
        {
            string strPathFile = @"C:\TimesheetService\PublishTimesheetService\arq.txt";
            try
            {

                using (StreamWriter sw = File.AppendText(strPathFile))
                {
                    sw.Write("\r\n" + texto);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}