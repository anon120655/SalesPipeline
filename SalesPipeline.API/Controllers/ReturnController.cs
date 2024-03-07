using Asp.Versioning;
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
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class ReturnController : Controller
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public ReturnController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		/// <summary>
		/// RM ส่งคืน ผจศ.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("RMToMCenter")]
		public async Task<IActionResult> RMToMCenter(ReturnModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Return.RMToMCenter(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ผจศ. ส่งคืน สาขาภาค
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("MCenterToBranch")]
		public async Task<IActionResult> MCenterToBranch(ReturnModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Return.MCenterToBranch(model);

					_transaction.Commit();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// สาขาภาคส่งคืนศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("BranchToLCenter")]
		public async Task<IActionResult> BranchToLCenter(ReturnModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Return.BranchToLCenter(model);

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
