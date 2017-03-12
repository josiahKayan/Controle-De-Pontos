using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.Response
{
    public class TimeResponse
    {

        public string STATUS { get; set; }
        [JsonProperty("time-entries")]
        public EntryTime[] TimeEntries { get; set; }
    }
}