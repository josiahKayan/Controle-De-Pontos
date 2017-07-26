using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class Status
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id;

        [Column("PARCEIROID")]
        public int ParceiroId;

        [Column("PERIODID")]

        public int PeriodoId;

        [Column("BLOCKED")]

        public int Blocked;

        [Column("MONTH")]

        public int Month ;

        [Column("YEAR")]

        public int Year;

    }
}