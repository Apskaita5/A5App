using System.Collections.Generic;
using A5Soft.CARMA.Application;

namespace A5Soft.A5App.Application.Infrastructure
{
    /// <summary>
    /// A container for email message data.
    /// </summary>
    public class EmailMessage
    {

        /// <summary>
        /// an email address of the message receiver (could be ; delimited multiple addresses)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// whether to allow ; delimited multiple addresses
        /// </summary>
        public bool AllowMultipleEmails { get; set; }

        /// <summary>
        /// a subject of the email message
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// a content of the email message
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// whether the message content is in HTML format
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// email message attachments
        /// </summary>
        public List<FileContent> Attachments { get; set; }

    }
}
