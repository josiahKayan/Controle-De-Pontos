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
    /// Class BrazilCity.
    /// </summary>
    [Table("BRAZILCITY")]
    public class BrazilCity
    {
        /// <summary>
        /// Gets or sets the citycode.
        /// </summary>
        /// <value>The citycode.</value>
        [Key, Column(Order=1)]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CITYCODE { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        [Required]
        public string CITY { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [Required]
        [Column("STATE")]
        public string STATE { get; set; }

        /// <summary>
        /// Gets the state of the brazil.
        /// </summary>
        /// <value>The state of the brazil.</value>
        [NotMapped]
        public BrazilState brazilState
        {
            get
            {

                return PartnerDataAccess.GetEstado(STATE);
            }

        }

    }
}