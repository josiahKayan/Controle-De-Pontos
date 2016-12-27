using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Apassos.Models
{
    /// <summary>
    /// Class BrazilState.
    /// </summary>
    [Table("BRAZILSTATE")] 
    public class BrazilState
    {
        /// <summary>
        /// Gets or sets the uf.
        /// </summary>
        /// <value>The uf.</value>
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UF { get; set; }
        /// <summary>
        /// Gets or sets the statename.
        /// </summary>
        /// <value>The statename.</value>
        [Required]
        public string STATENAME { get; set; }
        /// <summary>
        /// Gets or sets the ufcode.
        /// </summary>
        /// <value>The ufcode.</value>
        [Required]
        public string UFCODE { get; set; }
    }
}