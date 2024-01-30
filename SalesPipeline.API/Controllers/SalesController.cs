using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class SalesController : Controller
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public SalesController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		/// <summary>
		/// ข้อมูลและสถานะลูกค้าทั้งหมด
		/// </summary>
		//[AllowAnonymous]
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.Sales.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลและสถานะลูกค้า ById
		/// </summary>
		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Sales.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// เพิ่มข้อมูลลูกค้า
		/// </summary>
		[HttpPost("UpdateStatusOnly")]
		public async Task<IActionResult> UpdateStatusOnly(Sale_StatusCustom model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Sales.UpdateStatusOnly(model);

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
