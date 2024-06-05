using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class PreFactorController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public PreFactorController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(Pre_FactorCustom model)
		{
			try
			{
				var data = await _repo.PreFactor.Create(model);
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
				var data = await _repo.PreFactor.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}



	}
}
