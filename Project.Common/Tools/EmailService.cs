using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Tools
{
    public static class EmailService
    {
        private static readonly string _smtpServer = "smtp.gmail.com";
        private static readonly int _smtpPort = 587;
        private static readonly string _senderEmail = "hilalnisacantest@gmail.com"; // ← '@' eksikti, düzelttim
        private static readonly string _senderPassword = "yeotzjcgitxdheoo";

        public static bool Send(string toEmail, string body, string subject)
        {
            try
            {
                var fromAddress = new MailAddress(_senderEmail, "BilgeHotel");
                var toAddress = new MailAddress(toEmail);

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = _smtpServer;
                    smtp.Port = _smtpPort;
                    smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    smtp.EnableSsl = true;

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    })
                    {
                        smtp.Send(message);
                    }
                }

                return true; // ✅ Başarılıysa true dön
            }
            catch (Exception ex)
            {
                // 🔴 Log mekanizması varsa buraya eklenebilir
                Console.WriteLine("Mail gönderim hatası: " + ex.Message);
                return false; // ❌ Hatalıysa false dön
            }
        }
    }
}
