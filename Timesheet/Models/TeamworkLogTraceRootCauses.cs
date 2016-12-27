using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class TeamworkLogTraceRootCauses
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Column("TeamworkLogTraces_id")]
        public virtual TeamworkLogTraces TeamworkLogTraces { get; set; }

        [Column("TeamworkRootCauseProblems_id")]
        public virtual TeamworkRootCauseProblems TeamworkRootCauseProblems { get; set; }

    }
}