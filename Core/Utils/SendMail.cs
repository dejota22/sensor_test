using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Core.Utils
{
    public static class SendMail
    {
        private static readonly string _SMTP = "smtp.seguromail.com.br";
        private static readonly string _USER = "noreply@iotnest.com.br";
        private static readonly string _PASS = "t5$<2O9}^|BL";
        private static readonly string _FROM = "noreply@iotnest.com.br";

        public static void Send(string to, string subject, string message)
        {
            var smtpClient = new SmtpClient(_SMTP)
            {
                Port = 587,
                Credentials = new NetworkCredential(_USER, _PASS),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(_FROM, to)
            {
                Subject = subject,
                Body = ComposeBody(message),
                IsBodyHtml = true,
            };

            smtpClient.Send(mailMessage);
        }

        private static string ComposeBody(string message)
        {
            var sb = new StringBuilder();

            sb.Append("<div style=\"text-align:center;\">");
            sb.Append("<p style=\"background-color:#014E6A;color:#fff;font-size:16px;padding:5px;\"><strong>IOT NEST/VIBRAÇÃO</strong></p>");
            
            sb.Append(message);

            sb.Append("<p style=\"margin-top:40px;\">");
            sb.Append("<a href=\"http://telemetria.iotnest.com.br/\" style=\"background-color:#014E6A;color:#fff;padding:5px;text-decoration:none;\">Acessar Plataforma</a>");
            sb.Append("</p></div>");
            
            return sb.ToString();
        }
    }
}
