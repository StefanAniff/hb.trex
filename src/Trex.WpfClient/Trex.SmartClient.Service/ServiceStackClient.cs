using System;
using System.Net;
using System.Threading.Tasks;
using ServiceStack.Common.ServiceClient.Web;
using ServiceStack.ServiceClient.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Text;
using Trex.SmartClient.Core.Services;

namespace Trex.SmartClient.Service
{
    public class ServiceStackClient : IServiceStackClient
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly JsonServiceClient _jsonServiceClient;

        static ServiceStackClient()
        {
            JsConfig.DateHandler = JsonDateHandler.ISO8601;            
        }

        public ServiceStackClient(IUserSettingsService userSettingsService, IAppSettings appSettings)
        {
            _userSettingsService = userSettingsService;
            _jsonServiceClient = new JsonServiceClient(appSettings.JsonEndpointUri);
        }

        public Task AuthorizeAsync(ILoginSettings loginSettings)
        {
            var taskCompletionSource = new TaskCompletionSource<AuthResponse>();
            _jsonServiceClient.LocalHttpWebRequestFilter = LocalHttpWebRequestFilter(loginSettings);
            var request = new Auth
                {
                    UserName = loginSettings.UserName,
                    Password = loginSettings.Password,
                    RememberMe = true, //important tell client to retain permanent cookies
                };
            _jsonServiceClient.PostAsync(request, taskCompletionSource.SetResult,
                                         (authResponse, error) => taskCompletionSource.SetException(error));
            return taskCompletionSource.Task;
        }

        public void Authorize(ILoginSettings loginSettings)
        {
            _jsonServiceClient.LocalHttpWebRequestFilter = LocalHttpWebRequestFilter(loginSettings);
            var request = new Auth
            {
                UserName = loginSettings.UserName,
                Password = loginSettings.Password,
                RememberMe = true, //important tell client to retain permanent cookies
            };
            _jsonServiceClient.Post(request);            
        }

        private static Action<HttpWebRequest> LocalHttpWebRequestFilter(ILoginSettings loginSettings)
        {
            return requestH =>
                {
                    var hasHeaders = requestH.Headers.GetValues("CustomerId") != null;
                    if (!hasHeaders)
                    {
                        requestH.Headers.Add("CustomerId", loginSettings.CustomerId);
                        requestH.Headers.Add("ClientApplicationType", "3");
                    }
                };
        }

        public T Get<T>(IReturn<T> request)
        {
            Authorize(_userSettingsService.GetSettings());
            var response = _jsonServiceClient.Get(request);
            return response;
        }

        public async Task<T> GetAsync<T>(IReturn<T> request)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();            

            await AuthorizeAsync(_userSettingsService.GetSettings());

            _jsonServiceClient.GetAsync(request,
                                        taskCompletionSource.SetResult,
                                        (obj, error) => taskCompletionSource.SetException(error));
            return await taskCompletionSource.Task;
        }

       
        /// <summary>
        /// Recomended for larger object graphs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(IReturn<T> request)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            await AuthorizeAsync(_userSettingsService.GetSettings());

            _jsonServiceClient.PostAsync(request,
                                        taskCompletionSource.SetResult,
                                        (obj, error) => taskCompletionSource.SetException(error));
            return await taskCompletionSource.Task;
        }
    }
}