using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    /// <summary>
    /// Class Predecessor.
    /// </summary>
    public class Predecessor
    {
        /// <summary>
        /// Gets or sets the parent task identifier.
        /// </summary>
        /// <value>The parent task identifier.</value>
        [JsonProperty("parent-task-id")]
        public string parent_task_id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string id { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string type { get; set; }

    }
}