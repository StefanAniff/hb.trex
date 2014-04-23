using System;
using System.Web;

namespace Trex.Server.Infrastructure.Implemented
{
    public class NHibernateSessionModule : IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
        }

        public void Dispose() {}

        #endregion

        private void context_EndRequest(object sender, EventArgs e)
        {
            var builder = new HybridSessionBuilder();
            var session = builder.GetExistingWebSession();
            if (session != null)
            {
                //Log.Debug(this, "Disposing of ISession " + session.GetHashCode());
                session.Dispose();
            }
        }
    }
}