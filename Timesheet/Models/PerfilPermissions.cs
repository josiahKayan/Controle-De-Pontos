using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Apassos.DataAccess;

namespace Apassos.Models
{
    /// <summary>
    /// Class PerfilPermissions.
    /// </summary>
    public class PerfilPermissions
    {
        /// <summary>
        /// Gets or sets the perfilpermissionid.
        /// </summary>
        /// <value>The perfilpermissionid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PERFILPERMISSIONID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the perfilid.
        /// </summary>
        /// <value>The perfilid.</value>
        [Required]
        [Column("PERFILID")]
        public int PERFILID { get; set; }

        /// <summary>
        /// Gets or sets the permissionid.
        /// </summary>
        /// <value>The permissionid.</value>
        [Required]
        [Column("PERMISSIONID")]
        public int PERMISSIONID { get; set; }

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
        /// Gets the perfil.
        /// </summary>
        /// <value>The perfil.</value>
        [NotMapped]
        public Perfil perfil
        {
            get
            {
                if (PERFILID > 0)
                {
                    return PartnerDataAccess.GetPerfil(PERFILID);
                }
                return new Perfil();
            }
        }

        /// <summary>
        /// Gets the permission.
        /// </summary>
        /// <value>The permission.</value>
        [NotMapped]
        public Permission permission
        {
            get
            {
                if (PERFILID > 0)
                {
                    return PartnerDataAccess.GetPermission(PERMISSIONID);
                }
                return new Permission();
            }
        }
    }
}