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
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class AssignmentController : Controller
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public AssignmentController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpPost("GetListAutoAssign")]
		public async Task<IActionResult> GetListAutoAssign(allFilter model)
		{
			try
			{
				var response = await _repo.Assignment.GetListAutoAssign(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//[AllowAnonymous]
		[HttpPost("Assign")]
		public async Task<IActionResult> Assign(List<AssignmentCustom> model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Assignment.Assign(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
