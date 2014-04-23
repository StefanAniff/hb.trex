using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Implemented
{
    public abstract class EmailComposerBase : IEmailComposer
    {
        private const string SmartClientDownloadUrlMarker = "[SmartClientDownloadUrl]";
        private const string AdministrationUrlMarker = "[AdministrationUrl]";
        private const string ResourceUrlMarker = "[ResourcesUrl]";

        protected EmailComposerBase()
        {     
            Recipients = new List<string>();
            CcRecipients = new List<string>();
            BccRecipients = new List<string>();
            Attachments = new List<Attachment>();
            IsHtml = true;

        }
        public string Title { get; set; }
        public abstract string ComposeContent();
        public string Sender { get; set; }
        public bool IsHtml { get; set; }
        public List<string> Recipients { get; set; }
        public List<string> CcRecipients { get; set; }
        public List<string> BccRecipients { get; set; }
        public List<Attachment> Attachments { get; set; }        

        protected string GetContent(string fileName)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "EmailTemplates", fileName);
            EnsurePathExists(templatePath);
            return File.ReadAllText(templatePath);
        }

        private void EnsurePathExists(string templatePath)
        {
            if (!File.Exists(templatePath))
                throw new Exception(string.Format("Template for file {0} not found!", templatePath));
        }

        protected string ReplaceContentFromAppsettings(string content, IAppSettings appSettings)
        {
            content = content.Replace(SmartClientDownloadUrlMarker, appSettings.SmartClientDownloadUrl);
            content = content.Replace(AdministrationUrlMarker, appSettings.AdministrationUrl);
            content = content.Replace(ResourceUrlMarker, appSettings.HostResourcesUrl);
            return content;
        }
    }
}
