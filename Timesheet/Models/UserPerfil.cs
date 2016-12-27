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
    /// Class UserPerfil.
    /// </summary>
    public class UserPerfil
    {
        /// <summary>
        /// Gets or sets the userperfilid.
        /// </summary>
        /// <value>The userperfilid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int USERPERFILID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        /// <value>The userid.</value>
        [Required]
        [Column("USERID")]
        public int USERID { get; set; }

        /// <summary>
        /// Gets or sets the perfilid.
        /// </summary>
        /// <value>The perfilid.</value>
        [Required]
        [Column("PERFILID")]
        public int PERFILID { get; set; }

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
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        [NotMapped]
        public Users user
        {
            get
            {
                if (USERID > 0)
                {
                    return PartnerDataAccess.GetUsuario(USERID);
                }
                return null;
            }
        }

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
                return null;
            }
        }

    }
}