using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeamWorkDomain.Entities;
using TeamWorkDomain.Entities.AllTasks;

namespace Apassos.TeamWork.AutomaticTasks
{
    public class TeamWorkTasks
    {

        private readonly string TAGNAME = "Automatic Copy";


        public async Task<List<TodoItems>> GetAllTasks()
        {

            //Paulo Administrator Key
            //private readonly string APIKEY = "dog671silk";
            //Josias
            //private readonly string APIKEY = "memory897elbow";

            const string apiKey = "memory897elbow";
            const string domain = "apassosconsultingsoftware"; //.teamwork.com
            const string endpoint = "projects"; //eg projects.json , milestones.json etc
            const int id = 266147;

            var client = new HttpClient { BaseAddress = new Uri("https://" + domain + ".teamwork.com") };

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiKey, "x"))));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            List<TodoItems> todoItems = new List<TodoItems>();

            int Month = DateTime.Now.Month;
            string startDate = (new DateTime(DateTime.UtcNow.Year, Month, 1).Date).ToString("yyyMMdd");

            var data = await client.GetAsync("/tasks.json?startdate="+ startDate);
            using (Stream responseStream = await data.Content.ReadAsStreamAsync())
            {

                if (data.StatusCode == HttpStatusCode.OK)
                {

                    var result = await data.Content.ReadAsStringAsync();

                    try
                    {
                        var todoList = JsonConvert.DeserializeObject<TodoItemsResponse>(result);
                        todoItems.AddRange(todoList.todoitems);
                    }
                    catch (Exception e)
                    {

                    }

                }
                try
                {
                    todoItems = Filters(todoItems);
                }
                catch (Exception e)
                {

                }
                return todoItems;
            }
        }


        public List<TodoItems> Filters(List<TodoItems> todoItems)
        {

            List<TodoItems> newTodoItems = new List<TodoItems>();

            foreach (var item in todoItems)
            {

                if (item.tags != null)
                {
                    foreach (var tag in item.tags)
                    {
                        if (tag != null)
                        {
                            if (tag.name.Contains(TAGNAME))
                            {
                                newTodoItems.Add(item);

                            }
                        }
                    }
                }
            }

            return newTodoItems;

        }


        public async Task<int> CreateTaks(List<TodoItems> listTodoItems)
        {
            //Paulo Administrator Key
            //private readonly string APIKEY = "dog671silk";
            //Josias
            //private readonly string APIKEY = "memory897elbow";

            const string apiKey = "memory897elbow";
            const string domain = "apassosconsultingsoftware"; //.teamwork.com
            const string endpoint = "tasklists"; //eg projects.json , milestones.json etc
            const int id = 266147;
            const int idTaskList = 976981;

            var client = new HttpClient { BaseAddress = new Uri("https://" + domain + ".teamwork.com") };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
              "Basic", Convert.ToBase64String(
                UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiKey, "x"))
              )
            );

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TodoItem todoItem ;

            foreach (var item in listTodoItems)
            {

                todoItem = new TodoItem();

                todoItem.content = item.content;

                int posMonth, posYear;
                string month = "", year = "";
                if (item.content.Contains("["))
                {
                    var splitContent = item.content.Split('[');
                    var splitContentAux = splitContent[1].Split(',');
                    posMonth = splitContentAux[0].IndexOf(':');
                    month = splitContentAux[0].Substring(posMonth+1);
                    posYear = splitContentAux[1].IndexOf(':');
                    year = splitContentAux[1].Substring(posYear + 1).Replace("]","");
                }

                int actuallyMonth = DateTime.Now.Month;
                int actuallyYear = DateTime.Now.Year;



                todoItem.description = item.description + String.Format( " [Mês:{0},Ano:{1}]", actuallyMonth, actuallyYear);
                todoItem.progress = item.progress;
                todoItem.notify = false;
                todoItem.__invalid_name__responsible_party_id = -1;
                todoItem.__invalid_name__start_date = "";
                todoItem.__invalid_name__due_date = "";
                todoItem.priority = item.priority;
                todoItem.attachments = new List<object>();
                todoItem.pendingFileAttachments = "";
                todoItem.predecessors = null;
                todoItem.__invalid_name__estimated_minutes = item.estimatedminutes;
                todoItem.positionAfterTask = int.Parse(item.position);

                todoItem.tags = "";
                todoItem.@private = int.Parse(item.Private);
                todoItem.__invalid_name__grant_access_to = "";


                TodoItemResponse todoResponse = new TodoItemResponse();

                todoResponse.__invalid_name__todo_item = todoItem;

                string newTask = JsonConvert.SerializeObject(todoResponse);

                if ((actuallyMonth > int.Parse(month) && actuallyYear == int.Parse(year)))
                {
                    var data = await client.PostAsync(endpoint + "/" + item.todolistid + "/" + "tasks.json", new StringContent(newTask, Encoding.UTF8));
                }
            }

            return 1;
            //TodoItem todoItem = new TodoItem();

            //todoItem.content = string.Format("Tarefa criada via C# Mês:{0},Ano:{1}", DateTime.UtcNow.Month, DateTime.Now.Year);
            //todoItem.content = string.Format("Tarefa criada via C# Mês:{0},Ano:{1}", 4, 2017);

            //todoItem.description = "Tarefa teste de criação para o início do denvolvimento ";
            //todoItem.progress = "0";
            //todoItem.notify = false;
            //todoItem.__invalid_name__responsible_party_id = -1;
            //todoItem.__invalid_name__start_date = "";
            //todoItem.__invalid_name__due_date = "";
            //todoItem.priority = "low";
            //todoItem.attachments = new List<object>();
            //todoItem.pendingFileAttachments = "";
            //todoItem.predecessors = null;
            //todoItem.__invalid_name__estimated_minutes = "0";
            //todoItem.positionAfterTask = -1;
            //todoItem.tags = "";
            //todoItem.@private = 0;
            //todoItem.__invalid_name__grant_access_to = "";

            //TodoItemResponse todoResponse = new TodoItemResponse();

            //todoResponse.__invalid_name__todo_item = todoItem;

            //string newTask = JsonConvert.SerializeObject(todoResponse);

            //var data = await client.PostAsync(endpoint + "/" + idTaskList + "/" + "tasks.json", new StringContent(newTask, Encoding.UTF8));

            //using (Stream responseStream = await data.Content.ReadAsStreamAsync())
            //{
            //    return new StreamReader(responseStream).ReadToEnd();
            //}
        }


    }
}