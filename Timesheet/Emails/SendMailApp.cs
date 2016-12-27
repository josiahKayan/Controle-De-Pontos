using System;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;

namespace App.SendMail
{
    public class SendMailApp
    {
        //public string nomeRemetente = "APSM - APS Monitor";
        //public string emailRemetente = "apsmonitor@apassos.com.br";
        //public string msgReturn = "";

        //MailMessage mailMessage = new MailMessage();
        //SmtpClient smtpClient = new SmtpClient();

        //public bool sendNotificationMail()
        //{
        //    bool _return = true;

        //    //Define os dados do e-mail
        //    string nomeRemetente = this.nomeRemetente;
        //    string emailRemetente = this.emailRemetente;

        //    //Host da porta SMTP
        //    string SMTP = Ini.IniFile.getValue("mail.ita.locamail.com.br");        //"mail.ita.locamail.com.br";
        //    string emailAutenticacao = Ini.IniFile.getValue("josias.lima@apassos.com.br"); //"willian.lira@apassos.com.br";
        //    string senha = Ini.IniFile.getValue("Senha");             //"*********";

        //    //string emailDestinatario = "arilson.silva@apassos.com.br";
        //    //string emailComCopia = "willian.lira@apassos.com.br";
        //    //emailComCopia = "etzel.santos@apassos.com.br";
        //    //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

        //    string assuntoMensagem = "APSM Monitor / Cliente: " + Ini.IniFile.getValue("NomeCliente");
        //    //string conteudoMensagem = "Teste de envio de emails";
        //    string conteudoMensagem = msgServicoIniciadoHTML();

        //    //Define o Campo From e ReplyTo do e-mail.
        //    mailMessage.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

        //    //Define os destinatários do e-mail.
        //    foreach (string email in Ini.IniFile.getValue("EmailTo").Split(';'))
        //        if (!string.IsNullOrEmpty(email))
        //            mailMessage.To.Add(email.Trim());

        //    //Enviar cópia para.
        //    //objEmail.CC.Add(emailComCopia);

        //    //Enviar cópia oculta para.
        //    //objEmail.Bcc.Add(emailComCopiaOculta);

        //    //Define a prioridade do e-mail.
        //    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

        //    //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
        //    mailMessage.IsBodyHtml = true;

        //    //Define título do e-mail.
        //    mailMessage.Subject = assuntoMensagem;

        //    //Define o corpo do e-mail.
        //    mailMessage.Body = conteudoMensagem;


        //    //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
        //    mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
        //    mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");


        //    // Caso queira enviar um arquivo anexo
        //    //Caminho do arquivo a ser enviado como anexo
        //    //string arquivo = Server.MapPath("arquivo.jpg");

        //    // Ou especifique o caminho manualmente
        //    //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";

        //    // Cria o anexo para o e-mail
        //    //Attachment anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);

        //    // Anexa o arquivo a mensagem
        //    //objEmail.Attachments.Add(anexo);

        //    //Cria objeto com os dados do SMTP
        //    System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();
        //    //System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient(



        //    //Alocamos o endereço do host para enviar os e-mails  
        //    objSmtp.Credentials = new System.Net.NetworkCredential(emailAutenticacao, senha);
        //    objSmtp.Host = SMTP;
        //    objSmtp.Port = Convert.ToInt32(Ini.IniFile.getValue("Porta")); //587;
        //    //Caso utilize conta de email do exchange da locaweb deve habilitar o SSL
        //    //objEmail.EnableSsl = true;

        //    //Enviamos o e-mail através do método .send()
        //    try
        //    {
        //        objSmtp.Send(mailMessage);
        //        //ASM.App.Libs.Util.writeLog("E-mail via SMTP enviado com sucesso !");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        ASM.App.Libs.Util.writeLog("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        _return = false;
        //    }
        //    finally
        //    {
        //        //excluímos o objeto de e-mail da memória
        //        mailMessage.Dispose();
        //        //anexo.Dispose();
        //    }

        //    return _return;
        //}

        //public bool sendNotificationMailFailure()
        //{
        //    bool _return = true;

        //    //Define os dados do e-mail
        //    string nomeRemetente = this.nomeRemetente;
        //    string emailRemetente = this.emailRemetente;

        //    //Host da porta SMTP
        //    string SMTP = Ini.IniFile.getValue("ServidorSMTP");        //"mail.ita.locamail.com.br";
        //    string emailAutenticacao = Ini.IniFile.getValue("EmailAutenticacao"); //"willian.lira@apassos.com.br";
        //    string senha = Ini.IniFile.getValue("Senha");             //"*********";

        //    //string emailDestinatario = "arilson.silva@apassos.com.br";
        //    //string emailComCopia = "willian.lira@apassos.com.br";
        //    //emailComCopia = "etzel.santos@apassos.com.br";
        //    //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

        //    string assuntoMensagem = "Serviço " + Ini.IniFile.getValue("APSServico") + " Parou / Cliente: " + Ini.IniFile.getValue("NomeCliente");
        //    //string conteudoMensagem = "Teste de envio de emails";
        //    string conteudoMensagem = msgServicoFalhaHTML();

        //    //Cria objeto com dados do e-mail.


        //    //Define o Campo From e ReplyTo do e-mail.
        //    mailMessage.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

        //    //Define os destinatários do e-mail.
        //    foreach (string email in Ini.IniFile.getValue("EmailTo").Split(';'))
        //        if (!string.IsNullOrEmpty(email))
        //            mailMessage.To.Add(email.Trim());

        //    //Enviar cópia para.
        //    //objEmail.CC.Add(emailComCopia);

        //    //Enviar cópia oculta para.
        //    //objEmail.Bcc.Add(emailComCopiaOculta);

        //    //Define a prioridade do e-mail.
        //    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

        //    //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
        //    mailMessage.IsBodyHtml = true;

        //    //Define título do e-mail.
        //    mailMessage.Subject = assuntoMensagem;

        //    //Define o corpo do e-mail.
        //    mailMessage.Body = conteudoMensagem;


        //    //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
        //    mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
        //    mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");


        //    // Caso queira enviar um arquivo anexo
        //    //Caminho do arquivo a ser enviado como anexo
        //    //string arquivo = Server.MapPath("arquivo.jpg");

        //    // Ou especifique o caminho manualmente
        //    //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";

        //    // Cria o anexo para o e-mail
        //    //Attachment anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);

        //    // Anexa o arquivo a mensagem
        //    //objEmail.Attachments.Add(anexo);

        //    //Alocamos o endereço do host para enviar os e-mails  
        //    smtpClient.Credentials = new System.Net.NetworkCredential(emailAutenticacao, senha);
        //    smtpClient.Host = SMTP;
        //    smtpClient.Port = Convert.ToInt32(Ini.IniFile.getValue("Porta")); //587;
        //    //Caso utilize conta de email do exchange da locaweb deve habilitar o SSL
        //    //objEmail.EnableSsl = true;

        //    //Enviamos o e-mail através do método .send()
        //    try
        //    {
        //        smtpClient.Send(mailMessage);
        //        ASM.App.Libs.Util.writeLog("E-mail via SMTP enviado com sucesso !");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        ASM.App.Libs.Util.writeLog("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        _return = false;
        //    }
        //    finally
        //    {
        //        //excluímos o objeto de e-mail da memória
        //        mailMessage.Dispose();
        //        //anexo.Dispose();
        //    }

        //    return _return;
        //}

        //public void sendNotificationMailVBS()
        //{
        //    //EmailTest.vbs "1" "mail.ita.locamail.com.br" "587" "willian.lira@apassos.com.br" "blablabla" "C:\temp\emailtesteresult.txt"

        //    string fileVbs = "APSMSendMail.vbs";
        //    string x0Times = "1";
        //    string x1Smtp = Ini.IniFile.getValue("ServidorSMTP");
        //    string x2Port = Ini.IniFile.getValue("Porta");
        //    string x3User = Ini.IniFile.getValue("EmailAutenticacao");
        //    string x4Pass = Ini.IniFile.getValue("Senha");
        //    string x5logFile = Ini.IniFile.appPath + "\\ASMSendMailLog.txt";
        //    string x6Assunto = "APSM - APS Monitor / Cliente: " + Ini.IniFile.getValue("NomeCliente");
        //    string x7From = this.emailRemetente;
        //    string x8To = Ini.IniFile.getValue("EmailTo");

        //    string x9TextBody = msgServicoIniciadoTXT();

        //    string _cmd = "{0} \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\"";
        //    string cmd = string.Format(_cmd, x0Times, x1Smtp, x2Port, x3User, x4Pass, x5logFile, x6Assunto, x7From, x8To, x9TextBody);

        //    System.Diagnostics.Process proc1 = new System.Diagnostics.Process();
        //    proc1.StartInfo.FileName = fileVbs;
        //    proc1.StartInfo.WorkingDirectory = Ini.IniFile.appPath;
        //    proc1.StartInfo.Arguments = cmd;
        //    proc1.Start();
        //}

        //public void sendNotificationMailVBSFailure()
        //{
        //    //EmailTest.vbs "1" "mail.ita.locamail.com.br" "587" "willian.lira@apassos.com.br" "blablabla" "C:\temp\emailtesteresult.txt"

        //    string fileVbs = "APSMSendMail.vbs";
        //    string x0Times = "1";
        //    string x1Smtp = Ini.IniFile.getValue("ServidorSMTP");
        //    string x2Port = Ini.IniFile.getValue("Porta");
        //    string x3User = Ini.IniFile.getValue("EmailAutenticacao");
        //    string x4Pass = Ini.IniFile.getValue("Senha");
        //    string x5logFile = Ini.IniFile.appPath + "\\ASMSendMailLog.txt";
        //    string x6Assunto = "Servico APS Parado / Cliente: " + Ini.IniFile.getValue("NomeCliente");
        //    string x7From = this.emailRemetente;
        //    string x8To = Ini.IniFile.getValue("EmailTo");

        //    string x9TextBody = msgServicoFalhaTXT();

        //    string _cmd = "{0} \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\"";
        //    string cmd = string.Format(_cmd, x0Times, x1Smtp, x2Port, x3User, x4Pass, x5logFile, x6Assunto, x7From, x8To, x9TextBody);

        //    System.Diagnostics.Process proc1 = new System.Diagnostics.Process();
        //    proc1.StartInfo.FileName = fileVbs;
        //    proc1.StartInfo.WorkingDirectory = Ini.IniFile.appPath;
        //    proc1.StartInfo.Arguments = cmd;
        //    proc1.Start();
        //}

        //public bool sendMailTest(string addressList)
        //{
        //    bool _return = true;

        //    //Define os dados do e-mail
        //    string nomeRemetente = this.nomeRemetente;
        //    string emailRemetente = this.emailRemetente;

        //    //Host da porta SMTP
        //    string SMTP = Ini.IniFile.getValue("ServidorSMTP");        //"mail.ita.locamail.com.br";
        //    string emailAutenticacao = Ini.IniFile.getValue("EmailAutenticacao"); //"willian.lira@apassos.com.br";
        //    string senha = Ini.IniFile.getValue("Senha");             //"*********";

        //    //string emailDestinatario = "arilson.silva@apassos.com.br";
        //    //string emailComCopia = "willian.lira@apassos.com.br";
        //    //emailComCopia = "etzel.santos@apassos.com.br";
        //    //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

        //    string assuntoMensagem = "APSM Monitor Teste";
        //    //string conteudoMensagem = "Teste de envio de emails";
        //    string conteudoMensagem = msgTesteHTML();

        //    //Define o Campo From e ReplyTo do e-mail.
        //    mailMessage.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

        //    //Define os destinatários do e-mail.
        //    foreach (string email in addressList.Split(';'))
        //        if (!string.IsNullOrEmpty(email))
        //            mailMessage.To.Add(email.Trim());

        //    //Enviar cópia para.
        //    //objEmail.CC.Add(emailComCopia);

        //    //Enviar cópia oculta para.
        //    //objEmail.Bcc.Add(emailComCopiaOculta);

        //    //Define a prioridade do e-mail.
        //    mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

        //    //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
        //    mailMessage.IsBodyHtml = true;

        //    //Define título do e-mail.
        //    mailMessage.Subject = assuntoMensagem;

        //    //Define o corpo do e-mail.
        //    mailMessage.Body = conteudoMensagem;


        //    //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
        //    mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
        //    mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");


        //    // Caso queira enviar um arquivo anexo
        //    //Caminho do arquivo a ser enviado como anexo
        //    //string arquivo = Server.MapPath("arquivo.jpg");

        //    // Ou especifique o caminho manualmente
        //    //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";

        //    // Cria o anexo para o e-mail
        //    //Attachment anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);

        //    // Anexa o arquivo a mensagem
        //    //objEmail.Attachments.Add(anexo);


        //    //Alocamos o endereço do host para enviar os e-mails  
        //    smtpClient.Credentials = new System.Net.NetworkCredential(emailAutenticacao, senha);
        //    smtpClient.Host = SMTP;
        //    smtpClient.Port = Convert.ToInt32(Ini.IniFile.getValue("Porta")); //587;
        //    //Caso utilize conta de email do exchange da locaweb deve habilitar o SSL
        //    //objEmail.EnableSsl = true;

        //    //Enviamos o e-mail através do método .send()
        //    try
        //    {
        //        smtpClient.Send(mailMessage);
        //        msgReturn = "E-mail via SMTP enviado com sucesso !";
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        msgReturn = "Ocorreram problemas no envio do e-mail. Erro = " + ex.Message;
        //        _return = false;
        //    }
        //    finally
        //    {
        //        //excluímos o objeto de e-mail da memória
        //        mailMessage.Dispose();
        //        //anexo.Dispose();
        //    }

        //    return _return;
        //}

        ////TODO: Corpo Mensagem HTML E-mail Servico Iniciado
        //private static string msgServicoIniciadoHTML()
        //{
        //    string conteudoMensagem = "Cliente: " + Ini.IniFile.getValue("NomeCliente") + "<br>";
        //    conteudoMensagem += "Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "<br>";
        //    conteudoMensagem += "Notificação: O serviço APS Server teve " + Ini.IniFile.getValue("Tentativas") + " paradas consecutivas.";
        //    conteudoMensagem += "<br><br>Att: <b>APS Service Monitor</b>";
        //    return conteudoMensagem;
        //}

        ////TODO: Corpo Mensagem HTML E-mail Servico Falha
        //private static string msgServicoFalhaHTML()
        //{
        //    string conteudoMensagem = "Cliente: " + Ini.IniFile.getValue("NomeCliente") + "<br>";
        //    conteudoMensagem += "Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "<br>";
        //    conteudoMensagem += "Notificação: O serviço " + Ini.IniFile.getValue("APSServico") + " encontra-se parado após " + Ini.IniFile.getValue("Tentativas") + " tentativas de inicia-lo.";
        //    conteudoMensagem += "<br><br>Att: <b>APS Service Monitor</b>";
        //    return conteudoMensagem;
        //}

        ////TODO: Corpo Mensagem TXT E-mail Servico Iniciado
        //private static string msgServicoIniciadoTXT()
        //{
        //    string x9TextBody = "Cliente: " + Ini.IniFile.getValue("NomeCliente") + "\n";
        //    x9TextBody += "Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
        //    x9TextBody += "Notificação: O serviço APS Server teve " + Ini.IniFile.getValue("Tentativas") + " paradas consecutivas.";
        //    x9TextBody += "\n\nAtt: APS Service Monitor";
        //    return x9TextBody;
        //}

        ////TODO: Corpo Mensagem TXT E-mail Servico Falha
        //private static string msgServicoFalhaTXT()
        //{
        //    string x9TextBody = "Cliente: " + Ini.IniFile.getValue("NomeCliente") + "\n";
        //    x9TextBody += "Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "\n";
        //    x9TextBody += "Notificação: O serviço " + Ini.IniFile.getValue("APSServico") + " encontra-se parado apos " + Ini.IniFile.getValue("Tentativas") + " tentativas de inicia-lo.";
        //    x9TextBody += "\n\nAtt: APS Service Monitor";
        //    return x9TextBody;
        //}

        ////TODO: Corpo Mensagem HTML E-mail Servico Iniciado
        //private static string msgTesteHTML()
        //{
        //    string conteudoMensagem = "Cliente: " + Ini.IniFile.getValue("NomeCliente") + "<br>";
        //    conteudoMensagem += "Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "<br>";
        //    conteudoMensagem += "E-mail de Teste";
        //    conteudoMensagem += "<br><br>Att: <b>APS Monitor</b>";
        //    return conteudoMensagem;
        //}

        //public bool sendMailProxy()
        //{
        //    bool _return = true;

        //    //WebProxy proxy = new WebProxy("address:port");
        //    //proxy.Credentials = CredentialCache.DefaultCredentials;
        //    //proxy.Credentials = new NetworkCredential("PROXYUSER", "PROXYPASS");

        //    //Define os dados do e-mail
        //    string nomeRemetente = this.nomeRemetente;
        //    string emailRemetente = this.emailRemetente;

        //    //Host da porta SMTP
        //    string SMTP = Ini.IniFile.getValue("ServidorSMTP");        //"mail.ita.locamail.com.br";
        //    string emailAutenticacao = Ini.IniFile.getValue("EmailAutenticacao"); //"willian.lira@apassos.com.br";
        //    string senha = Ini.IniFile.getValue("Senha");             //"*********";

        //    //string emailDestinatario = "arilson.silva@apassos.com.br";
        //    //string emailComCopia = "willian.lira@apassos.com.br";
        //    //emailComCopia = "etzel.santos@apassos.com.br";
        //    //string emailComCopiaOculta  = "email@comcopiaoculta.com.br";

        //    string assuntoMensagem = "APSM Monitor / Cliente: " + Ini.IniFile.getValue("NomeCliente");
        //    //string conteudoMensagem = "Teste de envio de emails";
        //    string conteudoMensagem = msgServicoIniciadoHTML();

        //    //Cria objeto com dados do e-mail.
        //    MailMessage objEmail = new MailMessage();

        //    //Define o Campo From e ReplyTo do e-mail.
        //    objEmail.From = new System.Net.Mail.MailAddress(nomeRemetente + "<" + emailRemetente + ">");

        //    //Define os destinatários do e-mail.
        //    foreach (string email in Ini.IniFile.getValue("EmailTo").Split(';'))
        //        if (!string.IsNullOrEmpty(email))
        //            objEmail.To.Add(email.Trim());

        //    //Enviar cópia para.
        //    //objEmail.CC.Add(emailComCopia);

        //    //Enviar cópia oculta para.
        //    //objEmail.Bcc.Add(emailComCopiaOculta);

        //    //Define a prioridade do e-mail.
        //    objEmail.Priority = System.Net.Mail.MailPriority.Normal;

        //    //Define o formato do e-mail HTML (caso não queira HTML alocar valor false)
        //    objEmail.IsBodyHtml = true;

        //    //Define título do e-mail.
        //    objEmail.Subject = assuntoMensagem;

        //    //Define o corpo do e-mail.
        //    objEmail.Body = conteudoMensagem;


        //    //Para evitar problemas de caracteres "estranhos", configuramos o charset para "ISO-8859-1"
        //    objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
        //    objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");


        //    // Caso queira enviar um arquivo anexo
        //    //Caminho do arquivo a ser enviado como anexo
        //    //string arquivo = Server.MapPath("arquivo.jpg");

        //    // Ou especifique o caminho manualmente
        //    //string arquivo = @"e:\home\LoginFTP\Web\arquivo.jpg";

        //    // Cria o anexo para o e-mail
        //    //Attachment anexo = new Attachment(arquivo, System.Net.Mime.MediaTypeNames.Application.Octet);

        //    // Anexa o arquivo a mensagem
        //    //objEmail.Attachments.Add(anexo);

        //    //Cria objeto com os dados do SMTP
        //    System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient();
        //    //System.Net.Mail.SmtpClient objSmtp = new System.Net.Mail.SmtpClient(



        //    //Alocamos o endereço do host para enviar os e-mails  
        //    objSmtp.Credentials = new System.Net.NetworkCredential(emailAutenticacao, senha);
        //    objSmtp.Host = SMTP;
        //    objSmtp.Port = Convert.ToInt32(Ini.IniFile.getValue("Porta")); //587;
        //    //Caso utilize conta de email do exchange da locaweb deve habilitar o SSL
        //    //objEmail.EnableSsl = true;

        //    //Enviamos o e-mail através do método .send()
        //    try
        //    {
        //        objSmtp.Send(objEmail);
        //        ASM.App.Libs.Util.writeLog("E-mail via SMTP enviado com sucesso !");
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Write("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        ASM.App.Libs.Util.writeLog("Ocorreram problemas no envio do e-mail. Erro = " + ex.Message);
        //        _return = false;
        //    }
        //    finally
        //    {
        //        //excluímos o objeto de e-mail da memória
        //        objEmail.Dispose();
        //        //anexo.Dispose();
        //    }

        //    return _return;
        //}


    }
}