using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class PreCreditController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public PreCreditController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(Pre_CreditScoreCustom model)
		{
			try
			{
				var data = await _repo.PreCreditScore.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update(Pre_CreditScoreCustom model)
		{
			try
			{
				var data = await _repo.PreCreditScore.Update(model);
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
				await _repo.PreCreditScore.DeleteById(model);
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
				var data = await _repo.PreCreditScore.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.PreCreditScore.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
