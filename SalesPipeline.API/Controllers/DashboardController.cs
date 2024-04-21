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

		[HttpPost("GetStatus_TotalById")]
		public async Task<IActionResult> GetStatus_TotalById(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetStatus_TotalById(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("UpdateStatus_TotalById")]
		public async Task<IActionResult> UpdateStatus_TotalById(allFilter model)
		{
			try
			{
				await _repo.Dashboard.UpdateStatus_TotalById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("Get_SalesPipelineById")]
		public async Task<IActionResult> Get_SalesPipelineById(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.Get_SalesPipelineById(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetAvgTop_NumberById")]
		public async Task<IActionResult> GetAvgTop_NumberById(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetAvgTop_NumberById(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("UpdateAvg_NumberById")]
		public async Task<IActionResult> UpdateAvg_NumberById(allFilter model)
		{
			try
			{
				await _repo.Dashboard.UpdateAvg_NumberById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetAvgBottom_NumberById")]
		public async Task<IActionResult> GetAvgBottom_NumberById(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetAvgBottom_NumberById(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetListDealBranchById")]
		public async Task<IActionResult> GetListDealBranchById(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetListDealBranchById(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetListDealRMById")]
		public async Task<IActionResult> GetListDealRMById(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetListDealRMById(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetMap_ThailandById")]
		public async Task<IActionResult> GetMap_ThailandById(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetMap_ThailandById(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("UpdateMap_ThailandById")]
		public async Task<IActionResult> UpdateMap_ThailandById(allFilter model)
		{
			try
			{
				await _repo.Dashboard.UpdateMap_ThailandById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetTopSale")]
		public async Task<IActionResult> GetTopSale(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetTopSale(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetLostSale")]
		public async Task<IActionResult> GetLostSale(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetLostSale(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetAvgOnStage")]
		public async Task<IActionResult> GetAvgOnStage(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetAvgOnStage(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetPieCloseSaleReason")]
		public async Task<IActionResult> GetPieCloseSaleReason(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetPieCloseSaleReason(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetPieNumberCustomer")]
		public async Task<IActionResult> GetPieNumberCustomer(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetPieNumberCustomer(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetListNumberCustomer")]
		public async Task<IActionResult> GetListNumberCustomer(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetListNumberCustomer(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetPieLoanValue")]
		public async Task<IActionResult> GetPieLoanValue(allFilter model)
		{
			try
			{
				var data = await _repo.Dashboard.GetPieLoanValue(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetDuration")]
		public async Task<IActionResult> GetDuration(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetDuration(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("UpdateDurationById")]
		public async Task<IActionResult> UpdateDurationById(allFilter model)
		{
			try
			{
				await _repo.Dashboard.UpdateDurationById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetActivity")]
		public async Task<IActionResult> GetActivity(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetActivity(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("UpdateActivityById")]
		public async Task<IActionResult> UpdateActivityById(allFilter model)
		{
			try
			{
				await _repo.Dashboard.UpdateActivityById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetGroupReasonNotLoan")]
		public async Task<IActionResult> GetGroupReasonNotLoan(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetGroupReasonNotLoan(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetGroupDealBranch")]
		public async Task<IActionResult> GetGroupDealBranch(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetGroupDealBranch(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetSalesFunnel")]
		public async Task<IActionResult> GetSalesFunnel(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetSalesFunnel(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetPieRM")]
		public async Task<IActionResult> GetPieRM(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetPieRM(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetAvgTopBar")]
		public async Task<IActionResult> GetAvgTopBar(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetAvgTopBar(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetAvgRegionBar")]
		public async Task<IActionResult> GetAvgRegionBar(allFilter model)
		{
			try
			{
				var response = await _repo.Dashboard.GetAvgRegionBar(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
