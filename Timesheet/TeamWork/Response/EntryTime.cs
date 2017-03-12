using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Response
{
    public class EntryTime
    {
        [JsonProperty("project-id")]
        public string Projectid { get; set; }

        [JsonProperty("isbillable")]
        public string Isbillable { get; set; }

        [JsonProperty("todo-list-name")]
        public string TodoListName { get; set; }

        [JsonProperty("todo-item-name")]
        public string TodoItemName { get; set; }

        [JsonProperty("isbilled")]
        public string Isbilled { get; set; }

        [JsonProperty("updated-date")]
        public string UpdatedDate { get; set; }

        [JsonProperty("todo-list-id")]
        public string TodoListId { get; set; }

        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [JsonProperty("canEdit")]
        public string CanEdit { get; set; }

        [JsonProperty("taskEstimatedTime")]
        public string TaskEstimatedTime { get; set; }

        [JsonProperty("company-name")]
        public string CompanyName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("invoiceNo")]
        public string InvoiceNo { get; set; }

        [JsonProperty("person-last-name")]
        public string PersonLastName { get; set; }

        [JsonProperty("parentTaskName")]
        public string ParentTaskName { get; set; }

        [JsonProperty("dateUserPerspective")]
        public string DateUserPerspective { get; set; }

        [JsonProperty("minutes")]
        public string Minutes { get; set; }

        [JsonProperty("person-first-name")]
        public string PersonFirstName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("ticket-id")]
        public string TicketId { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("taskIsPrivate")]
        public string TaskIsPrivate { get; set; }

        [JsonProperty("parentTaskId")]
        public string ParentTaskId { get; set; }

        [JsonProperty("company-id")]
        public string CompanyId { get; set; }

        [JsonProperty("project-status")]
        public string ProjectStatus { get; set; }

        [JsonProperty("person-id")]
        public string PersonId { get; set; }

        [JsonProperty("project-name")]
        public string ProjectName { get; set; }

        [JsonProperty("taskIsSubTask")]
        public string TaskIsSubTask { get; set; }

        [JsonProperty("todo-item-id")]
        public string TodoItemId { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("has-start-time")]
        public string HasStartTime { get; set; }

        [JsonProperty("hours")]
        public string Hours { get; set; }


        public EntryTime()
        {
            new Tag();
        }
    }
}