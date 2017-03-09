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
    /// Class Project.
    /// </summary>
    public class Project
    {

        /// <summary>
        /// Gets or sets the projectid.
        /// </summary>
        /// <value>The projectid.</value>
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PROJECTID { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Required]
        public string ENVIRONMENT { get; set; }

        /// <summary>
        /// Gets or sets the partnerid.
        /// </summary>
        /// <value>The partnerid.</value>
        [Required]
        [Column("PARTNERID")]
        public int PARTNERID { get; set; }

        /// <summary>
        /// Gets or sets the gestorid.
        /// </summary>
        /// <value>The gestorid.</value>
        [Required]
        [Column("GESTORID")]
        public int GESTORID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        public string NAME { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [Required]
        public string STATUS { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Required]
        public string DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the plannedstartdate.
        /// </summary>
        /// <value>The plannedstartdate.</value>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PLANNEDSTARTDATE { get; set; }

        /// <summary>
        /// Gets or sets the actualstartdate.
        /// </summary>
        /// <value>The actualstartdate.</value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ACTUALSTARTDATE { get; set; }

        /// <summary>
        /// Gets or sets the plannedfinishdate.
        /// </summary>
        /// <value>The plannedfinishdate.</value>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime PLANNEDFINISHDATE { get; set; }

        /// <summary>
        /// Gets or sets the actualfinishdate.
        /// </summary>
        /// <value>The actualfinishdate.</value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ACTUALFINISHDATE { get; set; }

        /// <summary>
        /// Gets or sets the customerwbs.
        /// </summary>
        /// <value>The customerwbs.</value>
        public string CUSTOMERWBS { get; set; }



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


        /// <summary>
        /// The partner
        /// </summary>
        [NotMapped]
        private Partners _partner;
        /// <summary>
        /// Gets the partner.
        /// </summary>
        /// <value>The partner.</value>
        public Partners Partner
        {
            get
            {
                if (PARTNERID > 0)
                {
                    PartnerDataAccess p = new PartnerDataAccess();
                    _partner = p.GetParceiro(PARTNERID);
                    return _partner;
                }
                return Partner;
            }
        }

        /// <summary>
        /// Gets the gestor.
        /// </summary>
        /// <value>The gestor.</value>
        [NotMapped]
        public Partners Gestor
        {
            get
            {
                if (GESTORID > 0)
                {
                    PartnerDataAccess p = new PartnerDataAccess();

                    return p.GetParceiro(GESTORID);
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the consultores.
        /// </summary>
        /// <value>The consultores.</value>
        [NotMapped]
        public List<Partners> Consultores
        {
            get
            {
                ProjectDataAccess p = new ProjectDataAccess();
                return p.GetConsultoresProjeto(this);
            }
        }

        /// <summary>
        /// Gets the get date start to string.
        /// </summary>
        /// <value>The get date start to string.</value>
        [NotMapped]
        public String GetDateStartToString
        {
            get
            {
                if (ACTUALSTARTDATE != null)
                {
                    return Convert.ToDateTime(ACTUALSTARTDATE).ToShortDateString();
                }
                return " Não definido";
            }
        }
        /// <summary>
        /// Gets the get date finish to string.
        /// </summary>
        /// <value>The get date finish to string.</value>
        [NotMapped]
        public String GetDateFinishToString
        {
            get
            {
                if (ACTUALFINISHDATE != null)
                {
                    return Convert.ToDateTime(ACTUALFINISHDATE).ToShortDateString();
                }
                return " Não definido";
            }
        }

        /// <summary>
        /// Gets the CSS class partners.
        /// </summary>
        /// <value>The CSS class partners.</value>
        [NotMapped]
        public String cssClassPartners
        {
            get
            {
                if (Consultores != null && Consultores.Count() > 0)
                {
                    return "classPartners";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the name with status prefix.
        /// </summary>
        /// <value>The name with status prefix.</value>
        [NotMapped]
        public String NameWithStatusPrefix
        {
            get
            {
                if (Consultores != null && Consultores.Count() > 0)
                {
                    return "classPartners";
                }
                else
                {
                    return "";
                }
            }
        }
        
    }
}