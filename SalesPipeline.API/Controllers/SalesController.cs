using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;
using System.Net.Http;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
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
		[AllowAnonymous]
		[HttpPost("GetList")]
		public async Task<IActionResult> GetList(allFilter model)
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
		[AllowAnonymous]
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

		//[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("UpdateStatusOnly")]
		public async Task<IActionResult> UpdateStatusOnly(Sale_StatusCustom model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.Sales.UpdateStatusOnly(model);


					if (model.StatusId == StatusSaleModel.WaitAPIPHOENIX)
					{
						//รอส่ง API ส่งไประบบวิเคราะห์สินเชื่อ (PHOENIX/LPS)


					}

					_transaction.Commit();
				}

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpPost("UpdateStatusOnlyList")]
		public async Task<IActionResult> UpdateStatusOnlyList(List<Sale_StatusCustom> model)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					foreach (var item in model)
					{
						await _repo.Sales.UpdateStatusOnly(item);
					}

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
		/// รายชื่อลูกค้าที่ส่งคืน
		/// </summary>
		[AllowAnonymous]
		[HttpGet("GetListReturn")]
		public async Task<IActionResult> GetListReturn([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.Sales.GetListReturn(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลผลรวมสถานะต่างๆ ById
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		[HttpGet("GetStatusTotalById")]
		public async Task<IActionResult> GetStatusTotalById([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Sales.GetStatusTotalById(userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//[AllowAnonymous]
		//[HttpGet("UpdateStatusTotalById")]
		//public async Task<IActionResult> UpdateStatusTotalById([FromQuery] int userid)
		//{
		//	try
		//	{
		//		await _repo.Sales.UpdateStatusTotalById(userid);
		//		return Ok();
		//	}
		//	catch (Exception ex)
		//	{
		//		return new ErrorResultCustom(new ErrorCustom(), ex);
		//	}
		//}

		[AllowAnonymous]
		[HttpGet("SetIsUpdateStatusTotal")]
		public async Task<IActionResult> SetIsUpdateStatusTotal([FromQuery] int userid)
		{
			try
			{
				await _repo.Sales.SetIsUpdateStatusTotal(userid);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		//[AllowAnonymous]
		//[HttpGet("UpdateStatusTotalAll")]
		//public async Task<IActionResult> UpdateStatusTotalAll()
		//{
		//	try
		//	{
		//		await _repo.Sales.UpdateStatusTotalAll();
		//		return Ok();
		//	}
		//	catch (Exception ex)
		//	{
		//		return new ErrorResultCustom(new ErrorCustom(), ex);
		//	}
		//}

	}
}
