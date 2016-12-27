using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apassos.DataAccess;


namespace Apassos.Models
{
    /// <summary>
    /// Class TimesheetHeader.
    /// </summary>
    public class TimesheetHeader
    {

        /// <summary>
        /// Gets or sets the timesheetheaderid.
        /// </summary>
        /// <value>The timesheetheaderid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TIMESHEETHEADERID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>The period.</value>
        [Column("PERIODID")]
        public virtual Period Period { get; set; }

        /// <summary>
        /// Gets or sets the partner.
        /// </summary>
        /// <value>The partner.</value>
        [Column("PARTNERID")]
        public virtual Partners Partner { get; set; }

        /// <summary>
        /// Gets or sets the creationdate.
        /// </summary>
        /// <value>The creationdate.</value>
        public DateTime? CREATIONDATE { get; set; }
        /// <summary>
        /// Gets or sets the createdby.
        /// </summary>
        /// <value>The createdby.</value>
        public string CREATEDBY { get; set; }
        /// <summary>
        /// Gets or sets the changedate.
        /// </summary>
        /// <value>The changedate.</value>
        public DateTime? CHANGEDATE { get; set; }
        /// <summary>
        /// Gets or sets the changedby.
        /// </summary>
        /// <value>The changedby.</value>
        public string CHANGEDBY { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is encerrado.
        /// </summary>
        /// <value><c>true</c> if this instance is encerrado; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool IsEncerrado
        {
            get
            {
                return TimesheetDataAccess.ApontamentosFechado(this);
            }
        }

    }
}