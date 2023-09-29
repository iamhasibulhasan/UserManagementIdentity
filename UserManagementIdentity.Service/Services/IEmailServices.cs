using UserManagementIdentity.Service.Models;

namespace UserManagementIdentity.Service.Services
{
    public interface IEmailServices
    {
        void SendEmail(Message message);
    }
}
