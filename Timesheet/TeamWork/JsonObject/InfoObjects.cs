using Apassos.Erros;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Apassos.TeamWork.JsonObject
{
    public class InfoObjects
    {
        public DateTime Date { get; set; }
        public string Content { get; set; }
        public Erros.Erros Erro { get; set; }

    }
}