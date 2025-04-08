
namespace RMB.Abstractions.Infrastructure.Messages
{
    public class EmailConfirmationMessage
    {
        public string ToEmail { get; set; }
        public string ConfirmationLink { get; set; }
    }

}
