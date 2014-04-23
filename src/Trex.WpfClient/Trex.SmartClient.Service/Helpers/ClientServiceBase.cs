using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using ServiceStack.ServiceHost;
using Trex.SmartClient.Core.Interfaces;
using Trex.SmartClient.Service.TrexPortalService;
using log4net;

namespace Trex.SmartClient.Service
{
    public class ClientServiceBase
    {
        [Dependency]
        public Func<IServiceStackClient> Client { get; set; }

        [Dependency]
        public ICommonDialogs CommonDialogs { get; set; }

        public static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static T Decompress<T>(CompressedObject compressedObject)
        {
            var serializationHelper = new SerializationHelper();

            byte[] serializedResponse = compressedObject.SerializedResponse;

            if (compressedObject.ResponseHeaders.ContainsKey("compression"))
            {
                var compressionMode = compressedObject.ResponseHeaders["compression"];
                switch (compressionMode)
                {
                    case "gzip":
                        serializedResponse = serializationHelper.Decompress(compressedObject.SerializedResponse);
                        break;
                    default:
                        var message = string.Format("Sorry, but the response seems to be compressed with {0} which is not supported by this client", compressionMode);
                        throw new NotSupportedException(message);
                }
            }

            return serializationHelper.Deserialize<T>(serializedResponse);
        }

        /// <summary>
        /// Send ServiceStack request with error handling
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="errorMsgProvider"></param>
        /// <returns></returns>
        public async Task<T> TrySendAsync<T>(IReturn<T> request, Func<string> errorMsgProvider)
        {
            var result = default(T);

            try
            {
                result = await Client().PostAsync(request);
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
                CommonDialogs
                .Error(new StringBuilder()
                        .AppendLine(errorMsgProvider.Invoke())
                        .AppendLine()
                        .AppendLine(exp.ToString()) // IVA: Do exception dialog
                        .ToString());

                //CommonDialogs.Error("Error from server", errorMsgProvider.Invoke(), exp);
            }

            return result;
        }
    }
}
