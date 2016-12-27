using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Apassos.DataAccess;


namespace Apassos.Models
{
    //[Table("Partners")]
    /// <summary>
    /// Class Partners.
    /// </summary>
    public class Partners
    {
        /// <summary>
        /// Gets or sets the partnerid.
        /// </summary>
        /// <value>The partnerid.</value>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int PARTNERID { get; set; }
        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        [StringLength(60)] 
        public string NAME { get; set; }

        /// <summary>
        /// Gets or sets the shortname.
        /// </summary>
        /// <value>The shortname.</value>
        [Required]
        [StringLength(40)]
        public string SHORTNAME { get; set; }

        /// <summary>
        /// Gets or sets the firstname.
        /// </summary>
        /// <value>The firstname.</value>
        [StringLength(50)]
        public string FIRSTNAME { get; set; }

        /// <summary>
        /// Gets or sets the lastname.
        /// </summary>
        /// <value>The lastname.</value>
        [StringLength(50)]
        public string LASTNAME { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [Required]
        [StringLength(150)]
        public string ADDRESS { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>The number.</value>
        [Required]
        [StringLength(10)]
        public string NUMBER { get; set; }

        /// <summary>
        /// Gets or sets the complement.
        /// </summary>
        /// <value>The complement.</value>
        [StringLength(100)]
        public string COMPLEMENT { get; set; }

        /// <summary>
        /// Gets or sets the district.
        /// </summary>
        /// <value>The district.</value>
        [Required]
        [StringLength(60)]
        public string DISTRICT { get; set; }

        /// <summary>
        /// Gets or sets the cityid.
        /// </summary>
        /// <value>The cityid.</value>
        [StringLength(10)]
        public string CITYID { get; set; }

        /// <summary>
        /// Gets or sets the countryid.
        /// </summary>
        /// <value>The countryid.</value>
        [Required]
        [StringLength(10)]
        public string COUNTRYID { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required]
        [StringLength(150)]
        public string EMAIL { get; set; }

        /// <summary>
        /// Gets or sets the telephonenumber.
        /// </summary>
        /// <value>The telephonenumber.</value>
        [Required]
        [StringLength(20)]
        public string TELEPHONENUMBER { get; set; }

        /// <summary>
        /// Gets or sets the telephoneextension.
        /// </summary>
        /// <value>The telephoneextension.</value>
        [StringLength(20)]
        public string TELEPHONEEXTENSION { get; set; }

        /// <summary>
        /// Gets or sets the mobilephonenumber.
        /// </summary>
        /// <value>The mobilephonenumber.</value>
        [StringLength(20)]
        public string MOBILEPHONENUMBER { get; set; }

        /// <summary>
        /// Gets or sets the CPFCNPJ.
        /// </summary>
        /// <value>The CPFCNPJ.</value>
        [StringLength(20)]
        public string CPFCNPJ { get; set; }

        /// <summary>
        /// Gets or sets the inscricaoestadual.
        /// </summary>
        /// <value>The inscricaoestadual.</value>
        [StringLength(20)]
        public string INSCRICAOESTADUAL { get; set; }

        /// <summary>
        /// Gets or sets the inscricaomunicipal.
        /// </summary>
        /// <value>The inscricaomunicipal.</value>
        [StringLength(20)]
        public string INSCRICAOMUNICIPAL { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        [StringLength(10)]
        public string ZIP { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [StringLength(1)]
        public string TYPE { get; set; }

        /// <summary>
        /// Gets or sets the isuser.
        /// </summary>
        /// <value>The isuser.</value>
        [Required]
        [StringLength(10)]
        public string ISUSER { get; set; }

        /// <summary>
        /// Gets or sets the usergroup.
        /// </summary>
        /// <value>The usergroup.</value>
        [Required]
        [StringLength(20)]
        public string USERGROUP { get; set; }


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
        /// Gets the country.
        /// </summary>
        /// <value>The country.</value>
        [NotMapped]
        public Country country
        {
            get
            {

                return PartnerDataAccess.GetPais(COUNTRYID);
            }

        }

        /// <summary>
        /// Gets the city.
        /// </summary>
        /// <value>The city.</value>
        [NotMapped]
        public BrazilCity city
        {
            get
            {

                return PartnerDataAccess.GetCidade(CITYID);
            }

        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <value>The user.</value>
        [NotMapped]
        public Users user
        {
            get
            {
                if (ISUSER == "S")
                {
                    return PartnerDataAccess.GetUsuario(this);
                }
                else
                {
                    return new Users();
                }
            }

        }

        /// <summary>
        /// Gets the user forced.
        /// </summary>
        /// <value>The user forced.</value>
        [NotMapped]
        public Users UserForced
        {
            get
            {
                var usuario = PartnerDataAccess.GetUsuario(this);
                if ( usuario != null ) {
                    return usuario;
                }
                return new Users();
            }

        }

        /// <summary>
        /// Gets a value indicating whether this instance is user forced.
        /// </summary>
        /// <value><c>true</c> if this instance is user forced; otherwise, <c>false</c>.</value>
        [NotMapped]
        public bool IsUserForced
        {
            get
            {
                var usuario = PartnerDataAccess.GetUsuario(this);
                return (usuario != null);
            }

        }


        /// <summary>
        /// Gets the CSS class users.
        /// </summary>
        /// <value>The CSS class users.</value>
        [NotMapped]
        public String cssClassUsers
        {
            get
            {
                if (ISUSER == "S")
                {
                    return "classUsers";
                }
                else
                {
                    return "";
                }
            }
        }

    }


}

