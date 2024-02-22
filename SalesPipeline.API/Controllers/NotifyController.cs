using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using Microsoft.AspNetCore.Authorization;
using SalesPipeline.Utils.ValidationModel;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Notifications;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class NotifyController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public NotifyController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		//[AllowAnonymous]
		[HttpGet("LineNotify")]
		public async Task<IActionResult> LineNotify([FromQuery] string msg)
		{
			try
			{
				await _repo.Notifys.LineNotify(msg);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// ข้อมูลแจ้งเตือนทั้งหมด
		/// </summary>
		[AllowAnonymous]
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] NotiFilter model)
		{
			try
			{
				var response = await _repo.Notifys.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		/// <summary>
		/// แก้ไขสถานะการอ่านแจ้งเตือน
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPut("UpdateRead")]
		public async Task<IActionResult> UpdateRead(List<Guid> model)
		{
			try
			{
				await _repo.Notifys.UpdateRead(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
