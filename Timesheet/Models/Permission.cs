using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apassos.Models
{
    /// <summary>
    /// Class Permission.
    /// </summary>
    public class Permission
    {

        /// <summary>
        /// Gets or sets the permissionid.
        /// </summary>
        /// <value>The permissionid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PERMISSIONID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        [StringLength(10)]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Required]
        [StringLength(50)]
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [Required]
        [StringLength(30)]
        public string CODE { get; set; }
    }
}