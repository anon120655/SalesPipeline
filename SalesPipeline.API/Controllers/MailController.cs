using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class MailController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public MailController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}


		[HttpPost("SendMail")]
		public async Task<IActionResult> SendMail(SendMailModel model)
		{
			try
			{
				await _repo.EmailSender.SendEmail(model);
				return Ok(new ResultModel<Boolean>
				{
					Data = true
				});
			}
			catch (Exception ex)
			{
				return Ok(new ResultModel<Boolean>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				});
			}
		}

	}
}
