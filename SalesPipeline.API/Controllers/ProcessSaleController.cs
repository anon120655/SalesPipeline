using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Sales;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class ProcessSaleController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public ProcessSaleController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		/// <summary>
		/// ข้อมูลฟอร์มกระบวนการขาย ById
		/// </summary>
		/// 
		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.ProcessSale.GetById(id);

				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขฟอร์มกระบวนการขาย
		/// </summary>
		[HttpPut("Update")]
		public async Task<IActionResult> Update(ProcessSaleCustom model)
		{
			try
			{
				var data = await _repo.ProcessSale.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลฟอร์มกระบวนการขายทั้งหมด
		/// </summary>
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.ProcessSale.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// เพิ่มกระบวนการขาย
		/// </summary>
		[HttpPost("CreateReply")]
		public async Task<IActionResult> CreateReply(Sale_ReplyCustom model)
		{
			try
			{
				var data = await _repo.ProcessSale.CreateReply(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขกระบวนการขาย
		/// </summary>
		[HttpPut("UpdateReply")]
		public async Task<IActionResult> UpdateReply(Sale_ReplyCustom model)
		{
			try
			{
				var data = await _repo.ProcessSale.UpdateReply(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลกระบวนการขาย ById
		/// </summary>
		[HttpGet("GetReplyById")]
		public async Task<IActionResult> GetReplyById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.ProcessSale.GetReplyById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลกระบวนการขายทั้งหมด
		/// </summary>
		[HttpGet("GetListReply")]
		public async Task<IActionResult> GetListReply([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.ProcessSale.GetListReply(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลเอกสารทั้งหมด
		/// </summary>
		[HttpGet("GetListDocument")]
		public async Task<IActionResult> GetListDocument([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.ProcessSale.GetListDocument(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลประวัติการติดต่อทั้งหมด
		/// </summary>
		[HttpGet("GetListContactHistory")]
		public async Task<IActionResult> GetListContactHistory([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.ProcessSale.GetListContactHistory(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลปฏิทิน
		/// </summary>
		[HttpPost("GetListCalendar")]
		public async Task<IActionResult> GetListCalendar(allFilter model)
		{
			try
			{
				var response = await _repo.ProcessSale.GetListCalendar(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
