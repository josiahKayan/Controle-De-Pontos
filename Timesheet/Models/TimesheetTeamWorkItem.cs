using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apassos.Models
{
    /// <summary>
    /// Class TimesheetTeamWorkItem.
    /// </summary>
    public class TimesheetTeamWorkItem
    {
        /// <summary>
        /// Gets or sets the timesheet team work items identifier.
        /// </summary>
        /// <value>The timesheet team work items identifier.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? TimesheetTeamWorkItemsId { get; set; }

        /// <summary>
        /// Gets or sets the timesheet item.
        /// </summary>
        /// <value>The timesheet item.</value>
        [Column("TimesheetItem_TIMESHEETITEMID")]
        public virtual TimesheetItem TimesheetItem { get; set; }

        /// <summary>
        /// Gets or sets the team work todo item identifier.
        /// </summary>
        /// <value>The team work todo item identifier.</value>
        [Column("teamwork_todoitemid")]
        public virtual string TeamWorkTodoItemId { get; set; }

        /// <summary>
        /// Gets or sets the team work time entry identifier.
        /// </summary>
        /// <value>The team work time entry identifier.</value>
        [Column("teamwork_timeentryid")]
        public virtual string TeamWorkTimeEntryId { get; set; }

        //[Column("teamwork_timeuser")]
        //public virtual string TeamWorkTimeUser { get; set; }

        /// <summary>
        /// Gets or sets the team work time description.
        /// </summary>
        /// <value>The team work time description.</value>
        [Column("Description")]
        public virtual string TeamWorkTimeDescription { get; set; }
    }
}