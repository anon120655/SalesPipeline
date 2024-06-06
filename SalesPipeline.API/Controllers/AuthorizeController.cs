using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Utils.Resources.Authorizes.Auths;
using SalesPipeline.Utils.Resources.Shares;
using Asp.Versioning;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class AuthorizeController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public AuthorizeController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Authenticate(AuthenticateRequest model)
		{
			try
			{
				var xx = (100 * 13)/100;
				var xx2 = (20 * 3d)/100;
				var xx3 = (50 * 10)/100;

				var response = await _repo.Authorizes.Authenticate(model);

				if (response != null)
				{
					await _repo.User.LogLogin(new()
					{
						UserId = response.Id,
						IPAddress = model.IPAddress,
						DeviceVersion = model.DeviceVersion,
						SystemVersion = model.SystemVersion,
						AppVersion = model.AppVersion,
						tokenNoti = model.tokenNoti
					});
				}

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("ExpireToken")]
		public IActionResult ExpireToken([FromQuery] string token)
		{
			try
			{
				var response = _repo.Authorizes.ExpireToken(token);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}


	}
}
