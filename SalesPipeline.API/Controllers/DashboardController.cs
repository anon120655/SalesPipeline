using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class DashboardController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public DashboardController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpGet("GetStatus_TotalById")]
		public async Task<IActionResult> GetStatus_TotalById([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Dashboard.GetStatus_TotalById(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("UpdateStatus_TotalById")]
		public async Task<IActionResult> UpdateStatus_TotalById([FromQuery] int userid)
		{
			try
			{
				await _repo.Dashboard.UpdateStatus_TotalById(userid);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetAvg_NumberById")]
		public async Task<IActionResult> GetAvg_NumberById([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Dashboard.GetAvg_NumberById(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("UpdateAvg_NumberById")]
		public async Task<IActionResult> UpdateAvg_NumberById([FromQuery] int userid)
		{
			try
			{
				await _repo.Dashboard.UpdateAvg_NumberById(userid);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetMap_ThailandById")]
		public async Task<IActionResult> GetMap_ThailandById([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Dashboard.GetMap_ThailandById(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("UpdateMap_ThailandById")]
		public async Task<IActionResult> UpdateMap_ThailandById([FromQuery] int userid)
		{
			try
			{
				await _repo.Dashboard.UpdateMap_ThailandById(userid);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetPieCloseSaleReason")]
		public async Task<IActionResult> GetPieCloseSaleReason([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Dashboard.GetPieCloseSaleReason(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetPieNumberCustomer")]
		public async Task<IActionResult> GetPieNumberCustomer([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Dashboard.GetPieNumberCustomer(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetPieLoanValue")]
		public async Task<IActionResult> GetPieLoanValue([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Dashboard.GetPieLoanValue(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
