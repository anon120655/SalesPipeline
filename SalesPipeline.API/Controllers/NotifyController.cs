using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Authorization;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class NotifyController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public NotifyController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		//[AllowAnonymous]
		[HttpGet("LineNotify")]
		public async Task<IActionResult> LineNotify([FromQuery] string msg)
		{
			try
			{
				await _repo.Notifys.LineNotify(msg);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
