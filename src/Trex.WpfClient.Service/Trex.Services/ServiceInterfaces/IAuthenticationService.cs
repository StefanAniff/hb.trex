using System.ServiceModel;

namespace TrexSL.Web.ServiceInterfaces
{
    [ServiceContract(Namespace = "")]
    public interface IAuthenticationService
    {
        [OperationContract]
        bool ValidateUser(string userName, string password, string customerId);
        [OperationContract]
        bool ResetPassword(string userName);
    }
}