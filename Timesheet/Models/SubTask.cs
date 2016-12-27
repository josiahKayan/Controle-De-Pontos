using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class SubTask
    {
        [JsonProperty("project_id")]
        public string project_id { get; set; }
        public bool hasTickets { get; set; }
        public string order { get; set; }
        [JsonProperty("comments-count")]
        public string comments_count { get; set; }
        [JsonProperty("created-on")]
        public string created_on { get; set; }
        public bool canEdit { get; set; }
        [JsonProperty("has-predecessors")]
        public string has_predecessors { get; set; }
        public string id { get; set; }
        public bool completed { get; set; }
        public string position { get; set; }
        [JsonProperty("estimated-minutes")]
        public string estimated_minutes { get; set; }
        public string description { get; set; }
        public string progress { get; set; }
        [JsonProperty("harvest-enabled")]
        public bool harvest_enabled { get; set; }
        public string parentTaskId { get; set; }
        [JsonProperty("responsible-party-lastname")]
        public string responsible_party_lastname { get; set; }
        [JsonProperty("company-id")]
        public string company_id { get; set; }
        [JsonProperty("creator-avatar-url")]
        public string creator_avatar_url { get; set; }
        [JsonProperty("creator-id")]
        public string creator_id { get; set; }
        [JsonProperty("project-name")]
        public string project_name { get; set; }
        [JsonProperty("start-date")]
        public string start_date { get; set; }
        [JsonProperty("tasklist-private")]
        public bool tasklist_private { get; set; }
        public string lockdownId { get; set; }
        public bool canComplete { get; set; }
        [JsonProperty("responsible-party-id")]
        public string responsible_party_id { get; set; }
        [JsonProperty("creator-lastname")]
        public string creator_lastname { get; set; }
        [JsonProperty("has-reminders")]
        public bool has_reminders { get; set; }
        [JsonProperty("has-unread-comments")]
        public bool has_unread_comments { get; set; }
        [JsonProperty("todo-list-name")]
        public string todo_list_name { get; set; }
        [JsonProperty("due-date-base")]
        public string due_date_base { get; set; }
        [JsonProperty("private")]
        public string @private { get; set; }
        public bool userFollowingComments { get; set; }
        [JsonProperty("responsible-party-summary")]
        public string responsible_party_summary { get; set; }
        public string status { get; set; }
        [JsonProperty("todo-list-id")]
        public string todo_list_id { get; set; }
        public List<object> predecessors { get; set; }
        public List<object> tags { get; set; }
        public string content { get; set; }
        [JsonProperty("responsible-party-type")]
        public string responsible_party_type { get; set; }
        [JsonProperty("company-name")]
        public string company_name { get; set; }
        [JsonProperty("creator-firstname")]
        public string creator_firstname { get; set; }
        [JsonProperty("last-changed-on")]
        public string last_changed_on { get; set; }
        [JsonProperty("due-date")]
        public string due_date { get; set; }
        [JsonProperty("has-dependencies")]
        public string has_dependencies { get; set; }
        [JsonProperty("attachments-count")]
        public string attachments_count { get; set; }
        public bool userFollowingChanges { get; set; }
        public string priority { get; set; }
        [JsonProperty("responsible-party-firstname")]
        public string responsible_party_firstname { get; set; }
        public bool viewEstimatedTime { get; set; }
        [JsonProperty("responsible-party-ids")]
        public string responsible_party_ids { get; set; }
        [JsonProperty("responsible-party-names")]
        public string responsible_party_names { get; set; }
        [JsonProperty("tasklist-lockdownId")]
        public string tasklist_lockdownId { get; set; }
        public bool canLogTime { get; set; }
        public string timeIsLogged { get; set; }

    }
}