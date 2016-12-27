using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    /// <summary>
    /// Class RelatorioHoras.
    /// </summary>
    public class RelatorioHoras
  {

        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Required]
    public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the name of the consultor.
        /// </summary>
        /// <value>The name of the consultor.</value>
        [Required]
    public string ConsultorName { get; set; }

        /// <summary>
        /// Gets or sets the total hours.
        /// </summary>
        /// <value>The total hours.</value>
        [Required]
    public string TotalHours { get; set; }
    
  }
}