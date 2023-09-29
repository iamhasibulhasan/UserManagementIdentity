
namespace UserManagementIdentity.Service.Models
{
    public class EmailConfiguration
    {
        public string From { get; set; } = null;
        public string SmtpServer { get; set; } = null;
        public int Port { get; set; } = 465;
        public string Username { get; set; } = null;
        public string Passoword { get; set; } = null;
    }
}
