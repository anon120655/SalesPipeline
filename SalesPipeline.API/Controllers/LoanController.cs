using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class LoanController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public LoanController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(LoanCustom model)
		{
			try
			{
				var data = await _repo.Loan.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update(LoanCustom model)
		{
			try
			{
				var data = await _repo.Loan.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteById")]
		public async Task<IActionResult> DeleteById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.Loan.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusById")]
		public async Task<IActionResult> UpdateStatusById(UpdateModel model)
		{
			try
			{
				await _repo.Loan.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Loan.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetList")]
		public async Task<IActionResult> GetList(allFilter model)
		{
			try
			{
				var response = await _repo.Loan.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
