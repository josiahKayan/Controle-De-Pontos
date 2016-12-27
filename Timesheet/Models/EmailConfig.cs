using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apassos.Models
{
    public class EmailConfig

    {

        static private TimesheetContext db = new TimesheetContext();

        public TeamworkLogTraces Log { get; set; }
        public string EmailUser { get; set; }

        public string Pendencia { get; set; }

        public string Titulo { get; set; }

        public EmailConfig(TeamworkLogTraces log)
        {
            this.Log = log;

        }

        public void GetUserNameMail(TeamworkLogTraces log)
        {
            Partners parceiro;
            parceiro = db.Partners.Where(p => p.FIRSTNAME == log.Creator).FirstOrDefault();

            if (parceiro != null)
            {
                this.EmailUser = parceiro.EMAIL;
            }
            else
            {
                try
                {
                    this.EmailUser = parceiro.EMAIL;
                }
                catch (Exception e)
                {
                    this.EmailUser = "paulo.palmeira@apassos.com.br";
                }
            }



            this.Titulo = log.Titulo;
            foreach (var item in log.RootCauses)
            {
                if (item.TeamworkRootCauseProblems.Description != null)
                {
                    Pendencia = item.TeamworkRootCauseProblems.Description;
                }
                else
                {
                    Pendencia = "A descrição esta nula";
                }
               
            }
            
        }

        public void SetaParaNaoEnviarEmail(TeamworkLogTraces log)
        {
            log.IsFixed = true;
            Erros.ErroMensage.MarcarNaoEnviarEmail(log);

        }

        
    }
}