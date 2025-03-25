using RepMed.Core;
using RepMed.Services;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SP.Service.Email
{
    public interface IEmailSender
    {
        /// <summary>
        /// Send Email Async
        /// </summary>
        /// <param name="email">An Email</param>
        /// <param name="subject">The Subject</param>
        /// <param name="message">The Message</param>
        /// <returns>Empty</returns>
        Task SendEmailAsync(string email, string subject, string message);
        
    }

    public class EmailSender : BaseService, IEmailSender, IDisposable
    {
       
        public EmailSender(IDbConnection sqlConnection, IDbTransaction dbTransaction, IOptions<AppSettings> appSettings, IOptions<EmailSettings> emailSettings):base(sqlConnection, dbTransaction, appSettings, emailSettings)
        {
        }
        

        /// <summary>
        /// Send Email Async
        /// </summary>
        /// <param name="email">An Email</param>
        /// <param name="subject">The Subject</param>
        /// <param name="message">The Message</param>
        /// <returns>Empty</returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }
        /// <summary>
        /// This Method is used for sending mail to recepient
        /// </summary>
        /// <param name="email">An Email</param>
        /// <param name="subject">The Subject</param>
        /// <param name="message">The Message</param>
        /// <returns>Empty</returns>
        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.FromEmail, "Care U App")
                };
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = _emailSettings.EnableSSL;
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    await smtp.SendMailAsync(mail);
                    smtp.Dispose();
                }
                mail.Dispose();
                //var smtp = new System.Net.Mail.SmtpClient();
                //{
                //    smtp.Host = "smtp.gmail.com"; //set with your smtp server
                //    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                //    smtp.Credentials = new NetworkCredential(__emailSettings.UsernameEmail, __emailSettings.UsernamePassword);
                //    smtp.Timeout = 20000;
                //}
                //// Now send your email with this smtp
                //smtp.Send(mail);
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        
    }
}
