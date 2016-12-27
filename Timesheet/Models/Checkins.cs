using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Apassos.Models
{
    // Table Checkin
    /// <summary>
    /// Class Checkins.
    /// </summary>
    public class Checkins
   {

        /// <summary>
        /// Gets or sets the checkinid.
        /// </summary>
        /// <value>The checkinid.</value>
        [Key, Column(Order = 0)]
      [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public int? CHECKINID { get; set; }

        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        [Required]
      public TimeSpan ENTRY { get; set; }

        /// <summary>
        /// Gets or sets the saida.
        /// </summary>
        /// <value>The saida.</value>
        [Required]
      public TimeSpan SAIDA { get; set; }

        /// <summary>
        /// Gets or sets the lunch.
        /// </summary>
        /// <value>The lunch.</value>
        [Required]
      public TimeSpan LUNCH { get; set; }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        [Required]
      public TimeSpan TOTAL { get; set; }


        /// <summary>
        /// Gets or sets the isferiado.
        /// </summary>
        /// <value>The isferiado.</value>
        public string ISFERIADO { get; set; }

        /// <summary>
        /// Gets or sets the horasnaoapontadas.
        /// </summary>
        /// <value>The horasnaoapontadas.</value>
        [Required]
      public TimeSpan HORASNAOAPONTADAS { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [Required]
      public DateTime DATA { get; set; }

        /// <summary>
        /// Gets or sets the apontamento.
        /// </summary>
        /// <value>The apontamento.</value>
        [Required]
      public TimeSpan APONTAMENTO { get; set; }


        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Required]
      public string DESCRIPTION { get; set; }

        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        /// <value>The userid.</value>
        [Required]
      public int USERID { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [NotMapped]
      public int HASH { get; set; }


   }
}