using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Apassos.Common.Extensions;
using Apassos.DataAccess;

namespace Apassos.Models
{
    //classe usuario
    /// <summary>
    /// Class Users.
    /// </summary>
    [Table("Users")]
    public class Users
    {
        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        /// <value>The userid.</value>
        [Key, Column("USERID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int USERID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        [StringLength(10)]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        [Required]
        [StringLength(30)]
        public string USERNAME { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Required]
        [StringLength(200)]
        public string PASSWORD { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>The profile.</value>
        [Required]
        [StringLength(2)]
        public string PROFILE { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        /// <value>The locked.</value>
        [StringLength(1)]
        public string LOCKED { get; set; }

        //public int PARTNERID { get; set; }

        /// <summary>
        /// Gets or sets the isalterpwd.
        /// </summary>
        /// <value>The isalterpwd.</value>
        [StringLength(1)]
        public string ISALTERPWD { get; set; }

        /// <summary>
        /// Gets or sets the validfrom.
        /// </summary>
        /// <value>The validfrom.</value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? VALIDFROM { get; set; }

        /// <summary>
        /// Gets or sets the validto.
        /// </summary>
        /// <value>The validto.</value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? VALIDTO { get; set; }

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
        [Column("CHANGEDATE")]
        public DateTime? CHANGEDATE { get; set; }


        /// <summary>
        /// Gets or sets the changedby.
        /// </summary>
        /// <value>The changedby.</value>
        public string CHANGEDBY { get; set; }
        /// <summary>
        /// Gets or sets the lastlogondate.
        /// </summary>
        /// <value>The lastlogondate.</value>
        public DateTime? LASTLOGONDATE { get; set; }

        /// <summary>
        /// Gets or sets the partnerid.
        /// </summary>
        /// <value>The partnerid.</value>
        public int PARTNERID { get; set; }

        /// <summary>
        /// Gets the partner.
        /// </summary>
        /// <value>The partner.</value>
        [NotMapped]
        public Partners Partner
        {
            get
            {
                if (PARTNERID > 0)
                {
                    PartnerDataAccess p = new PartnerDataAccess();
                    return p.GetParceiro(PARTNERID);
                }
                return null;
            }
        }


        /// <summary>
        /// Gets the login pass crypt.
        /// </summary>
        /// <value>The login pass crypt.</value>
        [NotMapped]
        public string LoginPassCrypt
        {
            get
            {
                if (PASSWORD != null && USERNAME != null)
                {
                    return (PASSWORD.ToLower() + ":" + USERNAME.ToLower()).sysPassEncrypt();
                }
                return "";
            }
        }

        /// <summary>
        /// Gets the login pass decrypt.
        /// </summary>
        /// <value>The login pass decrypt.</value>
        [NotMapped]
        public string LoginPassDecrypt
        {
            get
            {
                if (PASSWORD != null && USERNAME != null)
                {
                    return (PASSWORD.ToLower() + ":" + USERNAME.ToLower()).sysPassDecrypt();
                }
                return "";
            }
        }


        /// <summary>
        /// Users the perfils list.
        /// </summary>
        /// <returns>List&lt;UserPerfil&gt;.</returns>
        public List<UserPerfil> UserPerfilsList()
        {
            return UsersDataAccess.GetUserPerfisUsuario(this);
        }


        /// <summary>
        /// Perfilses the list.
        /// </summary>
        /// <returns>List&lt;Perfil&gt;.</returns>
        public List<Perfil> PerfilsList()
        {
            return PartnerDataAccess.GetPerfisUsuario(this);
        }


        /// <summary>
        /// Permissionses the list.
        /// </summary>
        /// <returns>List&lt;Permission&gt;.</returns>
        public List<Permission> PermissionsList()
        {
            return PartnerDataAccess.GetPermissoesUsuario(this);
        }

    }



}