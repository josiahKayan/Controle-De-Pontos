using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class TeamworkLogTraces
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Column("teamwork_todoitemid")]
        public virtual string TeamWorkTodoItemId { get; set; }

        [Column("description")]
        public virtual string Description { get; set; }

        [Column("is_fixed")]
        public virtual bool IsFixed { get; set; }

        [Column("project_PROJECTID")]
        public virtual Project Project { get; set; }

        [Column("Partner_PARTNERID")]
        public virtual int Partner { get; set; }

        [Column("Creator")]
        public virtual string Creator { get; set; }

        [Column("Titulo")]
        public virtual string Titulo { get; set; }

        public virtual List<TeamworkLogTraceRootCauses> RootCauses { get; set; } 

    }
}