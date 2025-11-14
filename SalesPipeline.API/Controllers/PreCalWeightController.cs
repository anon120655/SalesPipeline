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
	public class PreCalWeightController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public PreCalWeightController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(List<Pre_Cal_WeightFactorCustom> model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.PreCalWeight.Validate(model);

					Guid id = model.Select(x=>x.Pre_CalId).FirstOrDefault();
					await _repo.PreCalWeight.RemoveAllPreCall(id);

					foreach (var item in model)
					{
						await _repo.PreCalWeight.Create(item);
					}

					_transaction.Commit();

					return Ok();
				}
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
				var data = await _repo.PreCalWeight.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetAllPreCalById")]
		public async Task<IActionResult> GetAllPreCalById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.PreCalWeight.GetAllPreCalById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}
	}
}
