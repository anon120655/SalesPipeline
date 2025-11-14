using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ValidationModel;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Assignments;
using Microsoft.AspNetCore.Authorization;

namespace SalesPipeline.API.Controllers
{
	//[ApiExplorerSettings(IgnoreApi = true)]
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class AssignmentRMController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public AssignmentRMController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		//[AllowAnonymous]
		[HttpPost("GetListAutoAssign")]
		public async Task<IActionResult> GetListAutoAssign(allFilter model)
		{
			try
			{
				var response = await _repo.AssignmentRM.GetListAutoAssign4(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("Assign")]
		public async Task<IActionResult> Assign(List<Assignment_RMCustom> model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentRM.Assign(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetListRM")]
		public async Task<IActionResult> GetListRM(allFilter model)
		{
			try
			{
				var response = await _repo.AssignmentRM.GetListRM(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("AssignChange")]
		public async Task<IActionResult> AssignChange(AssignChangeModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentRM.AssignChange(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("AssignReturnChange")]
		public async Task<IActionResult> AssignReturnChange(AssignChangeModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentRM.AssignReturnChange(model);

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
		[HttpPost("Update")]
		public async Task<IActionResult> Update(Assignment_RMCustom model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentRM.Update(model);

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
					await _repo.AssignmentRM.UpdateCurrentNumber(userid);

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
		[HttpGet("CreateAssignmentRMAll")]
		public async Task<IActionResult> CreateAssignmentRMAll()
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.AssignmentRM.CreateAssignmentRMAll(new());

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
