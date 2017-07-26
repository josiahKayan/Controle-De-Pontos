using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWorkDomain.Entities
{
    public class TodoItem
    {
        
        public string content { get; set; }
        public bool notify { get; set; }
        public string description { get; set; }
        [JsonProperty("due-date")]
        public string __invalid_name__due_date { get; set; }
        [JsonProperty("start-date")]
        public string __invalid_name__start_date { get; set; }
        [JsonProperty("estimated-minutes")]
        public string __invalid_name__estimated_minutes { get; set; }
        [JsonProperty("private")]
        public int @private { get; set; }
        [JsonProperty("grant-access-to")]
        public string __invalid_name__grant_access_to { get; set; }
        [JsonProperty("priority")]
        public string priority { get; set; }
        [JsonProperty("progress")]
        public string progress { get; set; }
        [JsonProperty("attachments")]

        public List<object> attachments { get; set; }
        [JsonProperty("pendingFileAttachments")]

        public string pendingFileAttachments { get; set; }
        [JsonProperty("responsible-party-id")]

        public int __invalid_name__responsible_party_id { get; set; }
        [JsonProperty("predecessors")]

        public List<Predecessor> predecessors { get; set; }
        [JsonProperty("tags")]

        public string tags { get; set; }
        [JsonProperty("positionAfterTask")]

        public int positionAfterTask { get; set; }

    }
}
