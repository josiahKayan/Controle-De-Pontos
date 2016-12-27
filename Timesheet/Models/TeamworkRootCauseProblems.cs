using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class TeamworkRootCauseProblems
    {
        public enum EnumRootCauseProblems
        {
            NO_TAG,
            NO_ENTRIES,
            NO_USER,
            NO_USER_ENTRIES
        };

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [Column("rootcause_description")]
        public virtual string Description { get; set; }

        [Column("abbreviation")]
        public virtual string Abbreviation { get; set; }

    }
}