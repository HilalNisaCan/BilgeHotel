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
    /*"EmailService, kullanıcıya aktivasyon kodu, bilgilendirme veya sistem mesajı göndermek için kullanılmaktadır.
SMTP ayarları önceden tanımlıdır ve Gmail üzerinden güvenli bağlantıyla gönderim yapılır.
Statik yapıda tutulduğu için hızlı erişim ve kolay entegrasyon sağlar."*/

    /*"EmailService projemizin tüm katmanlarında kullanılabilen bağımsız bir servis olduğu için Common katmanında konumlandırılmıştır.
    Böylece MVC, BLL veya başka bir yapı mail gönderimi gerektiğinde doğrudan bu servise erişebilir.
    Katman bağımsızlığı ve tekrar kullanılabilirlik prensibine uygun olarak Project.Common.Tools altında yer alır."

    */
    public static class EmailService
    {
        private static readonly string _smtpServer = "smtp.gmail.com";
        private static readonly int _smtpPort = 587;
        private static readonly string _senderEmail = "bilgehoteltest@gmail.com";
        private static readonly string _senderPassword = "hudoiiowpkpjmrgm"; // GİZLİ TUTULMALI

        public static bool Send(string toEmail, string body, string subject)
        {
            try
            {
                MailAddress fromAddress = new MailAddress(_senderEmail, "BilgeHotel");
                MailAddress toAddress = new MailAddress(toEmail);

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = _smtpServer;
                    smtp.Port = _smtpPort;
                    smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    smtp.EnableSsl = true;

                    using (MailMessage message = new MailMessage(fromAddress, toAddress))
                    {
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        smtp.Send(message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mail gönderim hatası: " + ex.Message);
                return false;
            }
        }
    }
}