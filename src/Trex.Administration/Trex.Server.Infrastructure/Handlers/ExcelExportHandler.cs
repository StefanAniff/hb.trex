using System;
using System.IO;
using System.Text;
using System.Web;
using StructureMap;
using Trex.Server.Core.Services;

namespace Trex.Server.Infrastructure.Handlers
{
    public class ExcelExportHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var exportId = new Guid(context.Request.QueryString["exportid"]);

            var appSettings = ObjectFactory.GetInstance<IAppSettings>();

            var path = String.Format("{0}/{1}.{2}", appSettings.ExcelExportPath, exportId, context.Request.QueryString["type"]);

            if (File.Exists(path))
            {
                SendFileToBrowser(path, context);
            }
            else
            {
                SaveFile(path, context);
            }
        }

        #endregion

        private void SendFileToBrowser(string path, HttpContext context)
        {
            var output = File.ReadAllBytes(path);
            context.Response.BinaryWrite(output);
            context.Response.ContentType = GetContentType(context.Request.QueryString["type"]);
            context.Response.AddHeader("content-disposition", "attachment; filename=" + path);
            File.Delete(path);
        }

        private string GetContentType(string type)
        {
            switch (type)
            {
                case "xls":
                    return "application/vnd.ms-excel";
                case "csv":
                    return "csv/text";

                default:
                    return "";
            }
        }

        private void SaveFile(string path, HttpContext context)
        {
            try
            {
                using (var fs = File.Create(path))
                {
                    using (var sr = new StreamReader(context.Request.InputStream))
                    {
                        var info = Encoding.Default.GetBytes(sr.ReadToEnd());
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}