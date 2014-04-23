using RegistrationWebsite.RegistrationService;

namespace RegistrationWebsite
{
    public static class ServiceAgent
    {
        public static RegistrationService.CustomerServiceClient GetServiceClient()
        {
            return new CustomerServiceClient();
        }
    }
}