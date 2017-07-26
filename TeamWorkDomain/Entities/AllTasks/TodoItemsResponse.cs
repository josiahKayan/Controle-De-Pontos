using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWorkDomain.Entities.AllTasks
{
    public class TodoItemsResponse
    {
        [JsonProperty("todo-items")]
        public TodoItems[] todoitems { get; set; }
        public string STATUS { get; set; }

    }
}
