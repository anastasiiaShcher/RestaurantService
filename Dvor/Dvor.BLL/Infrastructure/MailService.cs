using Dvor.Common.Entities;
using Dvor.Common.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Dvor.BLL.Infrastructure
{
    public class MailService : IMailService
    {
        private readonly MailAddress _from = new MailAddress("oleh.freher@nure.ua", "Game Store");

        private readonly SmtpClient _smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential("oleh.freher.gamestore@gmail.com", "GameStore1")
        };

        public void Send(string email, Notification notification)
        {
            var to = new MailAddress(email);
            var m = new MailMessage(_from, to) { Subject = notification.Title, Body = notification.Content };
            _smtp.Send(m);
        }
    }
}