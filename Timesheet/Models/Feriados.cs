using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    /// <summary>
    /// Class Feriados.
    /// </summary>
    public class Feriados
   {

        /// <summary>
        /// Gets or sets the feriadoid.
        /// </summary>
        /// <value>The feriadoid.</value>
        [Key, Column(Order = 0)]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int FERIADOID { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        [Required]
      public int YEAR { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>The month.</value>
        [Required]
      public int MONTH { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>The day.</value>
        [Required]
      public int DAY { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Required]
      public string DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the citycode.
        /// </summary>
        /// <value>The citycode.</value>
        public string CITYCODE { get; set; }

        /// <summary>
        /// Gets or sets the cepfrom.
        /// </summary>
        /// <value>The cepfrom.</value>
        public string CEPFROM { get; set; }


        /// <summary>
        /// Gets or sets the cepto.
        /// </summary>
        /// <value>The cepto.</value>
        public string CEPTO { get; set; }

   }
}