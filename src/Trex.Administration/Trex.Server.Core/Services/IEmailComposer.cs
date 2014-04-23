namespace Trex.Server.Core.Services
{
    public interface IEmailComposer
    {
        void SendForgotPasswordEmail(string fullName, string password, string recipient);
    }
}