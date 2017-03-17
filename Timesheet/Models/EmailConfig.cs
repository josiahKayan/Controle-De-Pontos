using Apassos.TeamWork.JsonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class EmailConfig

    {

        public InfoObjects Log { get; set; }
        public string EmailUser { get; set; }

        public string Pendencia { get; set; }

        public string Date { get; set; }
        public string Name { get; set; }
        public string Projeto { get; set; }


        public EmailConfig(InfoObjects log)
        {
            this.Log = log;
        }

        public void GetUserNameMail(InfoObjects log)
        {

            this.EmailUser = log.Erro.EmailPartner;
            this.Pendencia = log.Erro.ProblemDescription;
            this.Date = log.Erro.Date;
            this.Name = log.Erro.Consultor;
            this.Projeto = log.Erro.TWProject;
        }


        
    }
}