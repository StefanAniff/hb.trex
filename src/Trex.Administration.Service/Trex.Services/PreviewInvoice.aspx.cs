using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;
using Trex.ServiceContracts;
using TrexSL.Web.StructureMap;

namespace TrexSL.Web
{
    public partial class PreviewInvoice : Page
    {
        private readonly IFileDownloadService _downloadService;
        public PreviewInvoice(IFileDownloadService fileDownloadService)
        {
            _downloadService = fileDownloadService;
        }

        public PreviewInvoice()
        {
            ObjectFactory.Initialize(fac =>
            {
                fac.UseDefaultStructureMapConfigFile = false;
                fac.AddRegistry<RequestRegistry>();
            });
            _downloadService = ObjectFactory.GetInstance<IFileDownloadService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid guid = Guid.Parse(Request.QueryString["Guid"]);
            int format = int.Parse(Request.QueryString["format"]);

            var service = _downloadService;
            var data = service.DownloadPdfFile(guid, format);

            var bootStrapper = new BootStrapper();
            bootStrapper.Setup();

            if(data == null)
                return;

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF");
            Response.AddHeader("content-length", data.Length.ToString());
            Response.BinaryWrite(data);
            Response.Flush();
            Response.Close();
            Response.End();
        }
    }
}