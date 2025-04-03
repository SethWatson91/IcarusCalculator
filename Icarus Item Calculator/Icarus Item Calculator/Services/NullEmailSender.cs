using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Icarus_Item_Calculator.Services
{
    public class NullEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Do nothing (no-op implementation)
            return Task.CompletedTask;
        }
    }
}