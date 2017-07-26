using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamWorkDomain.Entities
{
    public class Project
    {

        public bool replyByEmailEnabled { get; set; }
        public bool starred { get; set; }
        public bool __invalid_name__show_announcement { get; set; }
        public bool __invalid_name__harvest_timers_enabled { get; set; }
        public string status { get; set; }
        public string subStatus { get; set; }
        public string defaultPrivacy { get; set; }
        public Integrations integrations { get; set; }
        public string __invalid_name__created_on { get; set; }
        public Category category { get; set; }
        public bool filesAutoNewVersion { get; set; }
        public string __invalid_name__overview_start_page { get; set; }
        public List<object> tags { get; set; }
        public string logo { get; set; }
        public string startDate { get; set; }
        public string id { get; set; }
        public string __invalid_name__last_changed_on { get; set; }
        public string endDate { get; set; }
        public Defaults defaults { get; set; }
        public Company company { get; set; }
        public string __invalid_name__tasks_start_page { get; set; }
        public string name { get; set; }
        public bool privacyEnabled { get; set; }
        public string description { get; set; }
        public bool logoFromCompany { get; set; }
        public bool isProjectAdmin { get; set; }
        [JsonProperty("__invalid_name__start-page")]
        public string __invalid_name__start_page { get; set; }
        public bool notifyeveryone { get; set; }


    }
}
