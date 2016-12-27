using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    /// <summary>
    /// Class Cities.
    /// </summary>
    public class Cities
   {
        /// <summary>
        /// Gets or sets the cityid.
        /// </summary>
        /// <value>The cityid.</value>
        [Key, Column(Order = 0)]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int CITYID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [Required]
      public string NAME { get; set; }

   }
}