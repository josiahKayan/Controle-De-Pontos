using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeamWorkNet.Model;

namespace Apassos.Models
{
    public class TodoListWithSubTask
    {
        [JsonProperty("project-id")]
        public string project_id { get; set; }
        [JsonProperty("todo-items")]
        public List<TodoItem> todo_items { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        [JsonProperty("milestone-id")]
        public string milestone_id { get; set; }
        [JsonProperty("uncompleted-count")]
        public string uncompleted_count { get; set; }
        public bool complete { get; set; }
        [JsonProperty("private")]
        public string @private { get; set; }
        [JsonProperty("todo-items")]
        public string overdue_count { get; set; }
        [JsonProperty("project-name")]
        public string project_name { get; set; }
        public bool pinned { get; set; }
        public bool tracked { get; set; }
        public string id { get; set; }
        public string position { get; set; }
        [JsonProperty("completed-count")]
        public string completed_count { get; set; }

    }
}