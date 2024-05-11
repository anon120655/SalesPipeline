using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IEmailSender
	{
		Task SendEmail(SendMailModel indata);
		Task LogSendEmail(ResourceEmail resource);
	}
}
