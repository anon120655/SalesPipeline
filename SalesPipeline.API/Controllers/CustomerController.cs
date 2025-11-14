using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Loggers;
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
	public class CustomerController : ControllerBase
	{
		private readonly IRepositoryWrapper _repo;

		public CustomerController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[AllowAnonymous]
		[HttpPost("SaveLog")]
		public async Task<IActionResult> SaveLog(RequestResponseLogModel model)
		{
			try
			{
				await _repo.Logger.SaveLog(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ตรวจสอบข้อมูลลูกค้า
		/// </summary>
		//[AllowAnonymous]
		[HttpGet("VerifyByNumber")]
		public async Task<IActionResult> VerifyByNumber([FromQuery] string juristicNumber, int? userid = null)
		{
			try
			{
				if (juristicNumber.Length < 10) throw new ExceptionCustom("ระบุข้อมูลไม่ถูกต้อง");

				var data = await _repo.Customer.VerifyByNumber(juristicNumber, userid);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("ValidateUpload")]
		public async Task<IActionResult> ValidateUpload(List<CustomerCustom> model)
		{
			try
			{
				var data = await _repo.Customer.ValidateUpload(model);
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
        [AllowAnonymous]
        [HttpPost("Create")]
		public async Task<IActionResult> Create(CustomerCustom model)
		{
			try
			{
				var data = await _repo.Customer.Create(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขข้อมูลลูกค้า
		/// </summary>
		//[AllowAnonymous]
		[HttpPut("Update")]
		public async Task<IActionResult> Update(CustomerCustom model)
		{
			try
			{
				var data = await _repo.Customer.Update(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลลูกค้า ById
		/// </summary>
		[HttpDelete("DeleteById")]
		public async Task<IActionResult> DeleteById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.Customer.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไข Status Only
		/// </summary>
		[HttpPut("UpdateStatusById")]
		public async Task<IActionResult> UpdateStatusById(UpdateModel model)
		{
			try
			{
				await _repo.Customer.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลลูกค้า ById
		/// </summary>
		[AllowAnonymous]
		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.Customer.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลลูกค้าทั้งหมด
		/// </summary>
		[AllowAnonymous]
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.Customer.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("GetListHistory")]
		public async Task<IActionResult> GetListHistory([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.Customer.GetListHistory(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}


	}
}
