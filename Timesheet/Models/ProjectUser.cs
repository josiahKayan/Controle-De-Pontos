using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Apassos.DataAccess;

namespace Apassos.Models
{
    /**
     *Consultores do projeto. 
     */
    /// <summary>
    /// Class ProjectUser.
    /// </summary>
    public class ProjectUser
    {
        /// <summary>
        /// Gets or sets the projectuserid.
        /// </summary>
        /// <value>The projectuserid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PROJECTUSERID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        [Required]
        [Column("PROJECTID")]
        public virtual Project project { get; set; }

        /// <summary>
        /// Gets or sets the partner.
        /// </summary>
        /// <value>The partner.</value>
        [Required]
        [Column("PARTNERID")]
        public virtual Partners partner { get; set; }

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

        
    }
}