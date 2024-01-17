using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Loggers;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[Route("v{version:apiVersion}/[controller]")]
	public class CustomerController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public CustomerController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
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
		/// เพิ่มข้อมูลลูกค้า
		/// </summary>
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
		/// ลบข้อมูลลูกค้า ById
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
		public async Task<IActionResult> GetList([FromQuery] CustomerFilter model)
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

		/// <summary>
		/// Test Error
		/// </summary>
		[ApiExplorerSettings(IgnoreApi = true)]
		[AllowAnonymous]
		[HttpGet("TestError")]
		public async Task<IActionResult> TestError([FromQuery] CustomerFilter model)
		{
			try
			{
				throw new ExceptionCustom(GeneralTxt.ExceptionTxtDefault);

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
