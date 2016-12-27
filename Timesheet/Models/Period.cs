using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    /// <summary>
    /// Class Period.
    /// </summary>
    public class Period
  {

        /// <summary>
        /// Gets or sets the periodid.
        /// </summary>
        /// <value>The periodid.</value>
        [Key, Column(Order = 0)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? PERIODID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
    public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>The month.</value>
        [Required]
    public int MONTH { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        [Required]
    public int YEAR { get; set; }

        /// <summary>
        /// Gets or sets the timesheetperiodstart.
        /// </summary>
        /// <value>The timesheetperiodstart.</value>
        [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? TIMESHEETPERIODSTART { get; set; }

        /// <summary>
        /// Gets or sets the timesheetperiodfinish.
        /// </summary>
        /// <value>The timesheetperiodfinish.</value>
        [Required]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime? TIMESHEETPERIODFINISH { get; set; }

        /// <summary>
        /// Gets or sets the creationdate.
        /// </summary>
        /// <value>The creationdate.</value>
        public DateTime CREATIONDATE { get; set; }
        /// <summary>
        /// Gets or sets the createdby.
        /// </summary>
        /// <value>The createdby.</value>
        public string CREATEDBY { get; set; }
        /// <summary>
        /// Gets or sets the changedate.
        /// </summary>
        /// <value>The changedate.</value>
        public DateTime CHANGEDATE { get; set; }
        /// <summary>
        /// Gets or sets the changedby.
        /// </summary>
        /// <value>The changedby.</value>
        public string CHANGEDBY { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string STATUS { get; set; }

        /// <summary>
        /// Gets or sets the totalhours.
        /// </summary>
        /// <value>The totalhours.</value>
        public int TOTALHOURS { get; set; }

        /// <summary>
        /// Formats the start.
        /// </summary>
        /// <returns>String.</returns>
        public String FormatStart()
    {
      if (this.TIMESHEETPERIODSTART == null)
      {
        return "";
      }
      return Convert.ToDateTime(this.TIMESHEETPERIODSTART).ToString("yyyy/MM/dd");
    }

        /// <summary>
        /// Formats the finish.
        /// </summary>
        /// <returns>String.</returns>
        public String FormatFinish()
    {
      if (this.TIMESHEETPERIODFINISH == null)
      {
        return "";
      }
      return Convert.ToDateTime(this.TIMESHEETPERIODFINISH).ToString("yyyy/MM/dd");
    }

  }
}