using Apassos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Observer
{
    public class Logs
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("TSID")]
        public int? TimesheetItem { get; set; }

        [Column("teamwork_timeentryid")]
        public string TeamWorkTimeEntryId { get; set; }

        [Column("Date")]
        public DateTime Data { get; set; }

        [Column("TagProblem")]
        public string TagProblem { get; set; }

        [Column("DescriptionProblem")]
        public string DescriptionProblem { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("ActivityDescription")]
        public string ActivityDescription { get; set; }

        [Column("ProjectTW")]
        public string ProjectTW { get; set; }

        [Column("ActivityDate")]
        public DateTime ActivityDate { get; set; }

        [Column("ConsultorID")]
        public int? ConsultorId { get; set; }

        [Column("PeriodoID")]
        public int? PeriodoId { get; set; }


        [Column("STATUS")]
        public int? Status { get; set; }
    }
}