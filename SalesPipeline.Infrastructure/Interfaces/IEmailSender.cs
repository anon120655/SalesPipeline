using SalesPipeline.Utils.Resources.Email;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface IEmailSender
	{
		Task<SendMail_TemplateCustom> GetTemplate(string code);
		Task SendEmail(SendMailModel indata);
		Task LogSendEmail(ResourceEmail data);
		Task SendNewUser(int? id);
	}
}
