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
	public class PreCalStanController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public PreCalStanController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(Pre_Cal_Fetu_StanCustom model)
		{
			try
			{
				var data = await _repo.PreCalStan.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update(Pre_Cal_Fetu_StanCustom model)
		{
			try
			{
				var data = await _repo.PreCalStan.Update(model);
				return Ok(data);
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
				var data = await _repo.PreCalStan.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
