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
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class ReturnController : Controller
	{
		private IRepositoryWrapper _repo;

		public ReturnController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		/// <summary>
		/// พนักงาน RM ส่งคืน ผู้จัดการศูนย์
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("RMToCenBranch")]
		public async Task<IActionResult> RMToCenBranch(ReturnModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Return.RMToCenBranch(model);

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
		/// ผู้จัดการศูนย์ ส่งคืน สำนักงานใหญ่
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("CenBranchToLoan")]
		public async Task<IActionResult> CenBranchToLoan(ReturnModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Return.CenBranchToLoan(model);

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
		/// กิจการสาขาภาค ส่งคืน ศูนย์ธุรกิจสินเชื่อ
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("BranchRegToLoan")]
		public async Task<IActionResult> BranchRegToLoan(ReturnModel model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Return.BranchRegToLoan(model);

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
