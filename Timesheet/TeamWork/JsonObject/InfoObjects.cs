using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.JsonObject
{
    public class InfoObjects
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime DATE { get; set; }
        public string CONTENT { get; set; }
        public string HASH { get; set; }
        public int PARTNERID { get; set; }

    }
}