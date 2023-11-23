using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace SensorApi
{
    public class SendMail
    {
        private string _SMTP = "";
        private int _PORT = 0;
        private string _USER = "";
        private string _PASS = "";
        private string _FROM = "";

        public SendMail()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", true, true);
            var config = builder.Build();
            _SMTP = config.GetSection("EmailConfiguration")["SmtpServer"];
            _PORT = int.Parse(config.GetSection("EmailConfiguration")["Port"]);
            _USER = config.GetSection("EmailConfiguration")["Username"];
            _PASS = config.GetSection("EmailConfiguration")["Password"];
            _FROM = config.GetSection("EmailConfiguration")["From"];
        }

        public void Send(string to, string subject, string message)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var smtpClient = new SmtpClient(_SMTP)
            {
                Port = _PORT,
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

        private string ComposeBody(string message)
        {
            var sb = new StringBuilder();

            sb.Append("<div style=\"text-align:center;\">");
            sb.Append("<p style=\"background-color:#014E6A;color:#fff;font-size:16px;padding:5px;\"><strong>IOTNEST/Vibração</strong></p>");

            sb.Append(message);

            sb.Append("<p style=\"margin-top:40px;\">");
            sb.Append("<a href=\"https://vibracao.iotnest.com.br/\" style=\"background-color:#014E6A;color:#fff;padding:5px;text-decoration:none;\">Acessar Plataforma</a>");
            sb.Append("</p></div>");

            return sb.ToString();
        }
    }
}
