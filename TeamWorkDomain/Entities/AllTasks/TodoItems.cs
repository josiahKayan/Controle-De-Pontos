using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWorkDomain.Entities.AllTasks
{
    public class TodoItems
    {

        public TodoItems()
        {
            tags = new Tag[5] ;
        }

        [JsonProperty("project-id")]
        public string projectid { get; set; }
        public string order { get; set; }
        [JsonProperty("comments-count")]
        public string commentscount { get; set; }
        public DateTime createdon { get; set; }
        public bool canEdit { get; set; }
        [JsonProperty("has-predecessors")]
        public string haspredecessors { get; set; }
        public string id { get; set; }
        public bool completed { get; set; }
        public string position { get; set; }
        [JsonProperty("estimated-minutes")]
        public string estimatedminutes { get; set; }
        public string description { get; set; }
        public string progress { get; set; }
        [JsonProperty("harvest-enabled")]
        public bool harvestenabled { get; set; }
        [JsonProperty("responsible-party-lastname")]
        public string responsiblepartylastname { get; set; }
        public string parentTaskId { get; set; }
        [JsonProperty("company-id")]
        public string companyid { get; set; }
        [JsonProperty("creator-avatar-url")]
        public string creatoravatarurl { get; set; }
        [JsonProperty("creator-id")]
        public string creatorid { get; set; }
        [JsonProperty("project-name")]
        public string projectname { get; set; }
        [JsonProperty("start-date")]
        public string startdate { get; set; }
        [JsonProperty("tasklist-private")]
        public string tasklistprivate { get; set; }
        public string lockdownId { get; set; }
        public bool canComplete { get; set; }
        [JsonProperty("responsible-party-id")]
        public string responsiblepartyid { get; set; }
        [JsonProperty("creator-lastname")]
        public string creatorlastname { get; set; }
        [JsonProperty("has-reminders")]
        public bool hasreminders { get; set; }
        [JsonProperty("todo-list-name")]
        public string todolistname { get; set; }
        [JsonProperty("has-unread-comments")]
        public bool hasunreadcomments { get; set; }
        [JsonProperty("due-date-base")]
        public string duedatebase { get; set; }
        [JsonProperty("private")]
        public string Private { get; set; }
        [JsonProperty("responsible-party-summary")]
        public string responsiblepartysummary { get; set; }
        public string status { get; set; }
        [JsonProperty("todo-list-id")]
        public string todolistid { get; set; }
        public Predecessor[] predecessors { get; set; }
        [JsonProperty("parent-task")]
        public ParentTask parenttask { get; set; }
        public string content { get; set; }
        public Boardcolumn[] boardColumn { get; set; }
        [JsonProperty("responsible-party-type")]
        public string responsiblepartytype { get; set; }
        [JsonProperty("company-name")]
        public string companyname { get; set; }
        [JsonProperty("creator-firstname")]
        public string creatorfirstname { get; set; }
        [JsonProperty("last-changed-on")]
        public DateTime lastchangedon { get; set; }
        [JsonProperty("due-date")]
        public string duedate { get; set; }
        [JsonProperty("has-dependencies")]
        public string hasdependencies { get; set; }
        [JsonProperty("attachments-count")]
        public string attachmentscount { get; set; }
        public string priority { get; set; }
        [JsonProperty("responsible-party-firstname")]
        public string responsiblepartyfirstname { get; set; }
        public bool viewEstimatedTime { get; set; }
        [JsonProperty("responsible-party-ids")]
        public string responsiblepartyids { get; set; }
        [JsonProperty("responsible-party-names")]
        public string responsiblepartynames { get; set; }
        [JsonProperty("tasklist-lockdownId")]
        public string tasklistlockdownId { get; set; }
        public bool canLogTime { get; set; }
        public string timeIsLogged { get; set; }
        public Tag[] tags { get; set; }

    }
}
