using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class MailController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public MailController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[AllowAnonymous]
		[HttpPost("SendMail")]
		public async Task<IActionResult> SendMail(SendMailModel model)
		{
			try
			{
				string messageBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n\t<meta name=\"viewport\" content=\"width=device-width\" />\r\n\t<meta charset=\"UTF-8\">\r\n\t<title>email sender</title>\r\n</head>\r\n<body>\r\n\t<div style=\"background-color: #499bc7; height:6px;\">\r\n\t</div>\r\n\t<div style=\"font-size: 12px; margin: auto; padding: 20px; text-align: left; background-color:#E3F4F9; \">\r\n\t\t<table style=\"color: #ffffff;\" width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n\t\t\t<tbody>\r\n\t\t\t\t<tr>\r\n\t\t\t\t\t<td width=\"5%\">\r\n\t\t\t\t\t\t<img src=\"https://img2.pic.in.th/pic/logo_bacc.png\" width=\"50\">\r\n\t\t\t\t\t</td>\r\n\t\t\t\t\t<td style=\"padding: 0px 15px 0 15px; border-right: 0px; \" width=\"95%\">\r\n\t\t\t\t\t\t<b style=\"font-size: 12pt; color: #40ae49;\">ธนาคารเพื่อการเกษตรและสหกรณ์การเกษตร</b> <br />\r\n\t\t\t\t\t\t<b style=\"font-size: 9pt; color:#8d8d8d;\">Bank for Agriculture and Agricultural Coolperatives</b> <br />\r\n\t\t\t\t\t</td>\r\n\t\t\t\t</tr>\r\n\t\t\t</tbody>\r\n\t\t</table>\r\n\t</div>\r\n\t<div>\r\n\t\t<div style=\"font-size: 13px; padding: 20px; font-family: Arial, Helvetica; color: #303030;\">\r\n\t\t\t<div>\r\n\t\t\t\t<p>\r\n\t\t\t\t\t<b>เรียน...{0}</b>\r\n\t\t\t\t</p>\r\n\t\t\t\t<p style=\"margin-left:15px;\"><b>วันที่เริ่มใช้งาน :</b> {1} {2}</p>\r\n\t\t\t\t<p style=\"margin-left:15px;\"><b>รหัสผ่าน :</b> {3}</p>\r\n\t\t\t</div>\r\n\t\t\t<br />\r\n\t\t\t<div>\r\n\t\t\t\t<p>รหัสผ่านนี้เป็นใช้ครั้งแรกเท่านั้น สามารถเปลี่ยนรหัสผ่านได้ที่</p>\r\n\t\t\t</div>\r\n\t\t\t<br />\r\n\t\t\t<div style=\"text-align:left;\">\r\n\t\t\t\t<a href=\"{4}\" target=\"_blank\"\r\n\t\t\t\t   style=\"font-size: 15px; background-color: #4CB6DB; line-height: 1.0; font-weight: 400; height: 38px; text-decoration: none; padding: 0.775rem 3.55rem; border-radius: 8px; color: #fff;\">\r\n\t\t\t\t\tChange Password\r\n\t\t\t\t</a>\r\n\t\t\t</div>\r\n\t\t\t<br />\r\n\t\t\t<div>\r\n\t\t\t\t<p>หรือสามารถล็อกอินเข้าใช้งานได้ที่ <span style=\"color:#4CB6DB;\">https://rmsales.app.baac.or.th</span></p>\r\n\t\t\t</div>\r\n\t\t\t<br />\r\n\t\t\t<p>ขอแสดงความนับถือ</p>\r\n\t\t\t<p>Support ธ.ก.ส.</p>\r\n\t\t\t<p>ธนาคารเพื่อการเกษตรและสหกรณ์การเกษตร</p>\r\n\t\t\t<br />\r\n\t\t\t<hr style=\"border: 1px solid #e6e6e6;width: 96%;\" />\r\n\t\t\t<br />\r\n\t\t\t<div style=\"text-align:center;\">\r\n\t\t\t\t<p>Copy Right 2023 Bank for Agriculture and Agricultural Cooperatives</p>\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t</div>\r\n\t<div style=\"background-color: #499bc7; height:6px;\">\r\n\t</div>\r\n</body>\r\n</html>";

				model.Body = string.Format(messageBody,
											"พนักงานRM01 ทดสอบ",
											"01 พฤศจิกายน 2566",
											"16:09 น.",
											"I14bpz2v",
											"https://localhost:7235/changepassword");

				await _repo.EmailSender.SendEmail(model);
				return Ok(new ResultModel<Boolean>
				{
					Data = true
				});
			}
			catch (Exception ex)
			{
				return Ok(new ResultModel<Boolean>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				});
			}
		}

		[HttpGet("SendNewUser")]
		public async Task<IActionResult> SendNewUser([FromQuery] int? id)
		{
			try
			{
				using (var _transaction = _repo.BeginTransaction())
				{
					await _repo.EmailSender.SendNewUser(id);

					await _transaction.CommitAsync();
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
