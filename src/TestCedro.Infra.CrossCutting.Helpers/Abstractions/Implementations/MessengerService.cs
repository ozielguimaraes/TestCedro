using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TestCedro.Infra.CrossCutting.Helpers.Abstractions.Interfaces;

namespace TestCedro.Infra.CrossCutting.Helpers.Abstractions.Implementations
{
    public class MessengerService : IMessengerService
    {
        public string From => Constants.Strings.Emails.Sender;
        public string NetworkCredentialUserName => Constants.Strings.Emails.Sender;
        public string NetworkCredentialPassword => "v7p,bAeLJWSs";
        public string Host => "smtp.gmail.com";
        public int Port => 587;

        public bool Send(string from, string to, string subject, string body, bool isBodyHtml)
        {
            try
            {
                Smtp().Send(Mail(from, to, subject, body, isBodyHtml));
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> SendAsync(string from, string to, string subject, string body, bool isBodyHtml)
        {
            try
            {
                await Smtp().SendMailAsync(Mail(from, to, subject, body, isBodyHtml));
                return true;
            }
            catch (SmtpFailedRecipientException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private MailMessage Mail(string from, string to, string subject, string body, bool isBodyHtml)
        {
            return new MailMessage(string.IsNullOrEmpty(from) ? From : from, to, subject, body)
            {
                IsBodyHtml = isBodyHtml,
                Priority = MailPriority.Normal,
                SubjectEncoding = Encoding.GetEncoding("ISO-8859-1"),
                BodyEncoding = Encoding.GetEncoding("ISO-8859-1")
            };
        }

        private SmtpClient Smtp()
        {
            return new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                Credentials = new NetworkCredential(NetworkCredentialUserName, NetworkCredentialPassword)
            };
        }
    }
}