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
    /// Class Perfil.
    /// </summary>
    public class Perfil
    {
        /// <summary>
        /// Gets or sets the perfilid.
        /// </summary>
        /// <value>The perfilid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PERFILID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        [StringLength(20)]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [StringLength(20)]
        public string NAME { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        [Required]
        [StringLength(10)]
        [Column("CODE")]
        public string CODE { get; set; }

        /// <summary>
        /// Gets or sets the creationdate.
        /// </summary>
        /// <value>The creationdate.</value>
        [Required]
        public DateTime CREATIONDATE { get; set; }

        /// <summary>
        /// Gets or sets the createdby.
        /// </summary>
        /// <value>The createdby.</value>
        [Required]
        [StringLength(60)]
        public string CREATEDBY { get; set; }

        /// <summary>
        /// Gets or sets the changedate.
        /// </summary>
        /// <value>The changedate.</value>
        [Required]
        public DateTime CHANGEDATE { get; set; }

        /// <summary>
        /// Gets or sets the changedby.
        /// </summary>
        /// <value>The changedby.</value>
        [Required]
        [StringLength(60)]
        public string CHANGEDBY { get; set; }

        /// <summary>
        /// Gets the permissions.
        /// </summary>
        /// <returns>List&lt;Permission&gt;.</returns>
        public List<Permission> GetPermissions()  {
                return PartnerDataAccess.GetPermissions(this);
        }


    }
}