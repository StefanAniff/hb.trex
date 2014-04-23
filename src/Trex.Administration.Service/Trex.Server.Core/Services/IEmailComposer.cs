using System.Collections.Generic;
using System.Net.Mail;

namespace Trex.Server.Core.Services
{
    public interface IEmailComposer
    {

       string Title { get; set; }
        
        string ComposeContent();
        string Sender { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email is in HTML format
        /// </summary>
        /// <value><c>true</c> if this instance is HTML; otherwise, <c>false</c>. Default value is true</value>
        bool IsHtml { get; set; }

        /// <summary>
        /// Gets the recipients
        /// </summary>
        /// <value>The recipients.</value>
        List<string > Recipients { get; set; }

        /// <summary>
        /// Gets the cc recipients
        /// </summary>
        /// <value>The cc recipients.</value>
        List<string> CcRecipients { get; set; }

        /// <summary>
        /// Gets the BCC recipients
        /// </summary>
        /// <value>The BCC recipients.</value>
        List<string> BccRecipients { get; set; }

        /// <summary>
        /// Gets the attachments.(Filepaths)
        /// </summary>
        /// <value>The attachments.</value>
        List<Attachment> Attachments { get; set; }
    }



    
}
