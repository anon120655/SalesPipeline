using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class PreCalController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public PreCalController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(Pre_CalCustom model)
		{
			try
			{
				var data = await _repo.PreCal.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update(Pre_CalCustom model)
		{
			try
			{
				var data = await _repo.PreCal.Update(model);
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
				await _repo.PreCal.DeleteById(model);
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
				await _repo.PreCal.UpdateStatusById(model);
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
				var data = await _repo.PreCal.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetIncludeAllById")]
		public async Task<IActionResult> GetIncludeAllById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.PreCal.GetIncludeAllById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetCalByAppBusId")]
		public async Task<IActionResult> GetCalByAppBusId([FromQuery] Guid appid, [FromQuery] Guid busid)
		{
			try
			{
				var data = await _repo.PreCal.GetCalByAppBusId(appid, busid);
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
				var response = await _repo.PreCal.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
