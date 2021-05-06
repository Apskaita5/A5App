using A5Soft.CARMA.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using A5Soft.A5App.Application.Infrastructure;
using A5Soft.CARMA.Application;

namespace A5Soft.A5App.Infrastructure
{
    /// <summary>
    /// <see cref="IEmailProvider"/> implementation using <see cref="SmtpClient"/>.
    /// </summary>
    [DefaultServiceImplementation(typeof(IEmailProvider))]
    public class NativeEmailSender : IEmailProvider
    {
        private readonly INativeEmailConfiguration _configuration;


        public NativeEmailSender(INativeEmailConfiguration config)
        {
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
            ValidateConfiguration();
        }
                            

        /// <inheritdoc cref="IEmailProvider.SendAsync"/>
        public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
        {
            if (null == message) throw new ArgumentNullException(nameof(message));

            ValidateConfiguration();

            ValidateMessage(message);

            var retries = _configuration.Retries;
            if (retries < 0) retries = 0;
            if (retries > 10) retries = 10;

            var retryWait = _configuration.RetryWaitPeriod;
            if (retryWait < 10) retryWait = 10;
            if (retryWait > 120000) retryWait = 120000;

            using (var msg = CreateMailMessage(message))
            {
                await SendMessageAsync(msg, retries, retryWait, ct);
            }
        }


        private void ValidateConfiguration()
        {
            var missingFields = new List<string>();
            if (_configuration.EmailAddress.IsNullOrWhiteSpace()) missingFields.Add("sender email address");
            if (_configuration.DisplayName.IsNullOrWhiteSpace()) missingFields.Add("sender display name");
            if (_configuration.Host.IsNullOrWhiteSpace()) missingFields.Add("email SMTP server address (host)");
            if (_configuration.Port < 1) missingFields.Add("email SMTP server port");
            if (!_configuration.UseDefaultCredentials && _configuration.UserName.IsNullOrWhiteSpace()) missingFields.Add("email user name"); 
            if (!_configuration.UseDefaultCredentials && _configuration.Password.IsNullOrWhiteSpace()) missingFields.Add("email user password");

            if (missingFields.Any()) throw new ArgumentException(
                $"Native email sender config field(s) {string.Join(", ", missingFields)} are not specified.");
        }

        private void ValidateMessage(EmailMessage message)
        {
            if (message.Email.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Receiver email is not specified.", nameof(message));
            if (message.Subject.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Message subject is not specified.", nameof(message));
            if (message.Content.IsNullOrWhiteSpace()) throw new ArgumentException(
                "Message content is not specified.", nameof(message));

            if (!message.AllowMultipleEmails && message.Email.Contains(";")) throw new ArgumentException(
                "Message does not allow multiple receivers yet the receiver address contains ';' separator.", 
                nameof(message));

            if (null == message.Attachments) message.Attachments = new List<FileContent>();
            foreach (var attachment in message.Attachments)
            {
                if (null == attachment.Content || attachment.Content.Length < 1) throw new ArgumentException(
                    "Attachment content is empty.", nameof(message));
                if (attachment.ContentType.IsNullOrWhiteSpace()) throw new ArgumentException(
                    "Attachment content type is empty.", nameof(message));
                if (attachment.FileName.IsNullOrWhiteSpace()) throw new ArgumentException(
                    "Attachment file name is not specified.", nameof(message));
            }
        }

        private MailMessage CreateMailMessage(EmailMessage message)
        {
            var result = new MailMessage
            {
                From = new MailAddress(_configuration.EmailAddress, _configuration.DisplayName),
                Subject = message.Subject,
                IsBodyHtml = message.IsHtml,
                BodyEncoding = Encoding.UTF8,
                BodyTransferEncoding = TransferEncoding.EightBit,
                Body = message.Content
            };

            if (message.Email.Contains(";"))
            {
                foreach (var address in message.Email.Trim().Split(
                    new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
                {
                    result.To.Add(address.Trim());
                }
            }
            else
            {
                result.To.Add(message.Email.Trim());
            }

            if (!message.OnBehalfOfEmail.IsNullOrWhiteSpace())
            {
                result.ReplyToList.Add(new MailAddress(message.OnBehalfOfEmail, 
                    message.OnBehalfOfDisplayName.IsNullOrWhiteSpace() 
                        ? message.OnBehalfOfEmail : message.OnBehalfOfDisplayName));
            }

            foreach (var attachment in message.Attachments)
            {
                result.Attachments.Add(new Attachment(new MemoryStream(attachment.Content, false), 
                    attachment.FileName, attachment.ContentType));
            }

            return result;
        }
          
        private async Task SendMessageAsync(MailMessage message, int retries, int retryWaitPeriod, CancellationToken ct)
        {

            if (ct.IsCancellationRequested) throw new TaskCanceledException();

            using (var smtpClient = new SmtpClient())
            {

                smtpClient.UseDefaultCredentials = _configuration.UseDefaultCredentials;
                smtpClient.Host = _configuration.Host;
                smtpClient.Port = _configuration.Port;
                smtpClient.EnableSsl = _configuration.EnableSsl;
                if (!_configuration.UseDefaultCredentials) smtpClient.Credentials = new System.Net.NetworkCredential(
                    _configuration.UserName, _configuration.Password);

                for (int i = 0; i < retries + 1; i++)
                {
                    try
                    {
                        if (ct.IsCancellationRequested) throw new TaskCanceledException();
                        await smtpClient.SendMailAsync(message);
                        return;
                    }
                    catch (SmtpFailedRecipientException)
                    {
                        if (i == retries) throw;
                    }
                    catch (SmtpException ex)
                    {
                        if (i == retries || !CanRecoverFromException(ex)) throw;
                    }
                    catch (AggregateException ex)
                    {
                        if (i == retries) throw;
                        ex.Handle((x) =>
                        {
                            if (x is SmtpFailedRecipientException || (x is SmtpException &&
                                CanRecoverFromException((SmtpException)x))) 
                            {
                               return true;
                            }
                            return false; 
                        });
                    }
                    await Task.Delay(retryWaitPeriod, ct);
                    retryWaitPeriod = retryWaitPeriod * 2;
                }                

            }
        }

        private static bool CanRecoverFromException(SmtpException ex)
        {
            return (ex.StatusCode == SmtpStatusCode.InsufficientStorage
                || ex.StatusCode == SmtpStatusCode.LocalErrorInProcessing
                || ex.StatusCode == SmtpStatusCode.MailboxBusy
                || ex.StatusCode == SmtpStatusCode.MailboxUnavailable
                || ex.StatusCode == SmtpStatusCode.ServiceClosingTransmissionChannel
                || ex.StatusCode == SmtpStatusCode.ServiceNotAvailable
                || ex.StatusCode == SmtpStatusCode.TransactionFailed);
        }

    }
}
