using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using RegistrationWebsite.Helpers;

namespace RegistrationWebsite.BaseClasses
{
    public class PageBase:Page
    {
       

        protected override void InitializeCulture()
        {

            var culture = Thread.CurrentThread.CurrentCulture;

            string language = null;
            CurrentCulture = culture;

            if ((language = Request.QueryString["lng"]) != null)
            {
                CurrentCulture = new CultureInfo(language);
            }

            Page.Culture = CurrentCulture.TwoLetterISOLanguageName;
            Page.UICulture = CurrentCulture.TwoLetterISOLanguageName;
        }

        public CultureInfo CurrentCulture { get; set; }

        protected void OnError(Exception ex)
        {
           LogHelper.LogError(ex);
        }
    }
}