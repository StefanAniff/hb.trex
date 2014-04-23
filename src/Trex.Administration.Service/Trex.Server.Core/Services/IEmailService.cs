namespace Trex.Server.Core.Services
{
    public interface IEmailService
    {
        bool SendEmail(IEmailComposer emailComposer);
    }
}
