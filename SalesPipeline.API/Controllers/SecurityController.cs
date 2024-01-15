using Asp.Versioning;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[Route("v{version:apiVersion}/[controller]")]
	public class SecurityController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public SecurityController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpGet("EnhancedHash")]
		public IActionResult EnhancedHash([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = BCrypt.Net.BCrypt.EnhancedHashPassword(val, hashType: HashType.SHA384);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("MD5Encrypt")]
		public IActionResult MD5Encrypt([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = Securitys.MD5Encrypt(val);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("MD5Decrypt")]
		public IActionResult MD5Decrypt([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = Securitys.MD5Decrypt(val);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("Base64StringEncode")]
		public IActionResult Base64StringEncode([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = Securitys.Base64StringEncode(val);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("Base64StringDecode")]
		public IActionResult Base64StringDecode([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = Securitys.Base64StringDecode(val);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("EncryptAES")]
		public IActionResult EncryptAES([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = Securitys.EncryptAES(val);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[ApiExplorerSettings(IgnoreApi = true)]
		[HttpGet("DecryptAES")]
		public IActionResult DecryptAES([FromQuery] string val)
		{
			try
			{
				string passwordHashGen = Securitys.DecryptAES(val);
				return Ok(passwordHashGen);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
