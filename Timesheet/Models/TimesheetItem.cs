using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Apassos.Common;
using Apassos.Common.Extensions;

namespace Apassos.Models
{
    /// <summary>
    /// Class TimesheetItem.
    /// </summary>
    public class TimesheetItem
    {
        /// <summary>
        /// Gets or sets the timesheetitemid.
        /// </summary>
        /// <value>The timesheetitemid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TIMESHEETITEMID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the timesheet header.
        /// </summary>
        /// <value>The timesheet header.</value>
        [Column("TIMESHEETHEADERID")]
        public virtual TimesheetHeader TimesheetHeader { get; set; }

        /// <summary>
        /// Gets or sets the counter.
        /// </summary>
        /// <value>The counter.</value>
        [Required]
        public int COUNTER { get; set; }
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        [Required]
        public DateTime DATE { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [Required]
        [Column("PROJECTID")]
        public virtual Project project { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [Required]
        public string TYPE { get; set; }

        /// <summary>
        /// Gets or sets the in.
        /// </summary>
        /// <value>The in.</value>
        [Required]
        public TimeSpan IN { get; set; }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        /// <value>The out.</value>
        [Required]
        public TimeSpan OUT { get; set; }

        /// <summary>
        /// Gets or sets the break.
        /// </summary>
        /// <value>The break.</value>
        [Required]
        public TimeSpan BREAK { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Required]
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>The note.</value>
        [Required]
        public string NOTE { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Required]
        [Column("STATUS")]
        public int STATUS { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        [Column("CREATIONDATE")]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Gets the total hours.
        /// </summary>
        /// <value>The total hours.</value>
        [NotMapped]
        public TimeSpan TotalHours
        {
            get
            {
                return Util.GetTotalHoras(IN, OUT, BREAK);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read.
        /// </summary>
        /// <value><c>true</c> if this instance is read; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool IsRead
        {
            get
            {
                return STATUS.In((int)Constants.StatusAprovacaoConstant.Encerrado, (int)Constants.StatusAprovacaoConstant.Aprovado);

            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is delete.
        /// </summary>
        /// <value><c>true</c> if this instance is delete; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool IsDelete
        {
            get
            {
                return !STATUS.In((int)Constants.StatusAprovacaoConstant.Encerrado, (int)Constants.StatusAprovacaoConstant.Aprovado);
            }
        }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [NotMapped]
        public int HASH { get; set; }

        /// <summary>
        /// Gets or sets the total days.
        /// </summary>
        /// <value>The total days.</value>
        [NotMapped]
        public int TotalDays { get; set; }

        /// <summary>
        /// Gets or sets the total hours day.
        /// </summary>
        /// <value>The total hours day.</value>
        [NotMapped]
        public TimeSpan TotalHoursDay { get; set; }

        /// <summary>
        /// Gets or sets the type of the style.
        /// </summary>
        /// <value>The type of the style.</value>
        [NotMapped]
        public string StyleType { get; set; }

    }
}