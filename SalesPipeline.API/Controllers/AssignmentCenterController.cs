using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	//[ApiExplorerSettings(IgnoreApi = true)]
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class AssignmentCenterController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public AssignmentCenterController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.AssignmentCenter.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("GetListAutoAssign")]
		public async Task<IActionResult> GetListAutoAssign(allFilter model)
		{
			try
			{
				var response = await _repo.AssignmentCenter.GetListAutoAssign(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("GetListCenter")]
		public async Task<IActionResult> GetListCenter(allFilter model)
		{
			try
			{
				var response = await _repo.AssignmentCenter.GetListCenter(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("Assign")]
		public async Task<IActionResult> Assign(AssignModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentCenter.Assign(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("AssignCenter")]
		public async Task<IActionResult> AssignCenter(List<Assignment_CenterCustom> model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					//await _repo.AssignmentCenter.AssignCenter(model);
					await _repo.AssignmentCenter.AssignCenterUpdateRange(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("UpdateCurrentNumber")]
		public async Task<IActionResult> UpdateCurrentNumber([FromQuery] int? userid)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentCenter.UpdateCurrentNumber(userid);

					_transaction.Commit();

					return Ok();
				}
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("CreateAssignmentCenterAll")]
		public async Task<IActionResult> CreateAssignmentCenterAll()
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentCenter.CreateAssignmentCenterAll(new());

					_transaction.Commit();

					return Ok();
				}
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
