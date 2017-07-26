using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWorkDomain.Entities
{
    public class TodoItemResponse
    {
        [JsonProperty("todo-item")]

        public TodoItem __invalid_name__todo_item { get; set; }

}
}
