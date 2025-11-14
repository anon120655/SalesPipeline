using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
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

		public SalesController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		/// <summary>
		/// ข้อมูลและสถานะลูกค้าทั้งหมด
		/// </summary>
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

		[HttpGet("GetListStatusById")]
		public async Task<IActionResult> GetListStatusById([FromQuery] Guid id)
		{
			try
			{
				var response = await _repo.Sales.GetListStatusById(id);

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
		/// ข้อมูลและสถานะลูกค้า ById
		/// </summary>
		[HttpGet("GetByCustomerId")]
		public async Task<IActionResult> GetByCustomerId([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Sales.GetByCustomerId(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("IsViewSales")]
		public async Task<IActionResult> IsViewSales([FromQuery] Guid id, [FromQuery] int userid)
		{
			try
			{
				var data = await _repo.Sales.IsViewSales(id, userid);
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
		[HttpPost("GetListReturn")]
		public async Task<IActionResult> GetListReturn(allFilter model)
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

		[AllowAnonymous]
		[HttpPost("GetOverdueCount")]
		public async Task<IActionResult> GetOverdueCount(allFilter model)
		{
			try
			{
				var data = await _repo.Sales.GetOverdueCount(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

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

		/// <summary>
		/// เพิ่มข้อมูลผู้ติดต่อ
		/// </summary>
		[HttpPost("CreateInfo")]
		public async Task<IActionResult> CreateInfo(Sale_Contact_InfoCustom model)
		{
			try
			{
				var data = await _repo.Sales.CreateInfo(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขข้อมูลผู้ติดต่อ
		/// </summary>
		[HttpPut("UpdateInfo")]
		public async Task<IActionResult> UpdateInfo(Sale_Contact_InfoCustom model)
		{
			try
			{
				var data = await _repo.Sales.UpdateInfo(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลผู้ติดต่อ ById
		/// </summary>
		[HttpGet("GetInfoById")]
		public async Task<IActionResult> GetInfoById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Sales.GetInfoById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลผู้ติดต่อทั้งหมด
		/// </summary>
		[HttpPost("GetListInfo")]
		public async Task<IActionResult> GetListInfo(allFilter model)
		{
			try
			{
				var response = await _repo.Sales.GetListInfo(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// เพิ่มคู่ค้า
		/// </summary>
		[HttpPost("CreatePartner")]
		public async Task<IActionResult> CreatePartner(Sale_PartnerCustom model)
		{
			try
			{
				var data = await _repo.Sales.CreatePartner(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขคู่ค้า
		/// </summary>
		[HttpPut("UpdatePartner")]
		public async Task<IActionResult> UpdatePartner(Sale_PartnerCustom model)
		{
			try
			{
				var data = await _repo.Sales.UpdatePartner(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// คู่ค้า ById
		/// </summary>
		[HttpGet("GetPartnerById")]
		public async Task<IActionResult> GetPartnerById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Sales.GetPartnerById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// คู่ค้าทั้งหมด
		/// </summary>
		[HttpPost("GetListPartner")]
		public async Task<IActionResult> GetListPartner(allFilter model)
		{
			try
			{
				var response = await _repo.Sales.GetListPartner(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ประวัติการกู้สินเชื่อ
		/// </summary>
		[AllowAnonymous]
		[HttpPost("GetListHistoryLoan")]
		public async Task<IActionResult> GetListHistoryLoan(allFilter model)
		{
			try
			{
				var response = await _repo.Sales.GetListHistoryLoan(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("RePurpose")]
		public async Task<IActionResult> RePurpose(RePurposeModel model)
		{
			try
			{
				var response = await _repo.Sales.RePurpose(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}



	}
}
