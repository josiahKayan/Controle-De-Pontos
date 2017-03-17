using Apassos.Models;
using Apassos.TeamWork.JsonObject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
namespace Apassos.Emails
{
    public class Email
    {

        public async void EnviaMensagemEmail(List<InfoObjects> listLogs)
        {
            #region
            //string destino = "niege.costa@apassos.com.br";
            //string remetente = "niege.costa@apassos.com.br";
            //string corpo = "Esta mensagem foi enviado do código C#";
            //string porta = "587";
            //string smtp = "mail.ita.locamail.com.br";

            #region
            string email = "jkayanlima@gmail.com";
            string secret = "danca12017";

            #endregion
            #region
            //SmtpClient client = new SmtpClient();
            //client.Host = smtp;
            //client.Port = 465;
            //client.EnableSsl = true;
            //client.Credentials = new NetworkCredential(remetente, senha);
            //MailMessage mail = new MailMessage();
            //mail.Sender = new MailAddress(remetente, "ENVIADOR");
            //mail.From = new MailAddress(remetente, "ENVIADOR");
            //mail.To.Add(new MailAddress(destino, "RECEBEDOR"));
            //mail.Subject = "Contato";
            //mail.Body = corpo;
            //mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;
            #endregion

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(email, secret);

            //SmtpClient client = new SmtpClient();
            //client.Host = "mail.ita.locamail.com.br";
            //client.Port = 587;
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = true;
            //client.Credentials = new NetworkCredential("envio-nfe@apassos.com.br", "@P@ssos123");

            //Montando a mensagem que será enviada
            #endregion

            List<EmailConfig> listEmail = new List<EmailConfig>();

            foreach (var log in listLogs)
            {
                EmailConfig emailConfig = new EmailConfig(log);
                emailConfig.GetUserNameMail(log);
                listEmail.Add(emailConfig);
            }



            #region MyRegion
            //SmtpClient client = new SmtpClient();
            //client.Host = "email-ssl.com.br";
            //client.Port = 465;
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = true;
            //client.Credentials = new NetworkCredential(remetente, senha);
            #endregion

            foreach (var mensagem in listEmail)
            {
                try
                {
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(email);
                        mail.To.Add(new MailAddress("" + mensagem.EmailUser));
                        mail.Subject = "Aviso de Pendências Timesheet";
                        mail.Body = CreateBodyEmail(mensagem);
                        await client.SendMailAsync(mail);
                    }

                }
                catch (Exception ex)
                {
                    string erro = ex.Message;
                    Debug.WriteLine("" + erro);
                }

            }
        }
        public string CreateBodyEmail(EmailConfig mensagem)
        {

            string body =
                "Olá " + mensagem.Name + " tudo bem?" +
                "\n" +
                "\n" + "Notamos que existe um log de tempo no projeto: " + mensagem.Projeto  +
                "\n" +
                "\n" + "Na data:" + mensagem.Date +
                "\n" +
                "\n" + "Contém a seguinte pendência(s):" + mensagem.Pendencia +
                "\n" +
                "\n" + "Por favor solucione isso." +
                "\n" +
                "\n" + "Qualquer dúvida entre em contato pelo E-mail:" + "josias.lima@apassos.com.br  ou pelo skype: josiaslimacarvalho" +
                "\n" +
                "\n" + "Caro parceiro a Apassos agradece a sua colaboração!!!" +
                "\n" +
                "\n" + "God bless you!!";
            return body;
        }

    }
}


