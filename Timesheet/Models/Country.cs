using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apassos.Models
{
    /// <summary>
    /// Class Country.
    /// </summary>
    [Table("COUNTRY")]
    public class Country
    {
        /// <summary>
        /// Gets or sets the countrycode.
        /// </summary>
        /// <value>The countrycode.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string COUNTRYCODE { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
        public string NAME { get; set; }
    }
}