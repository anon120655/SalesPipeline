using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class SystemController : ControllerBase
	{
		private IRepositoryWrapper _repo;

		public SystemController(IRepositoryWrapper repo)
		{
			_repo = repo;
		}

		[HttpPost("CreateSignature")]
		public async Task<IActionResult> CreateSignature(System_SignatureCustom model)
		{
			try
			{
				var data = await _repo.System.CreateSignature(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetSignatureLast")]
		public async Task<IActionResult> GetSignatureLast([FromQuery] int userid)
		{
			try
			{
				var data = await _repo.System.GetSignatureLast(userid);
				return Ok(data ?? new());
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("CreateSLA")]
		public async Task<IActionResult> CreateSLA(System_SLACustom model)
		{
			try
			{
				var data = await _repo.System.CreateSLA(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateSLA")]
		public async Task<IActionResult> UpdateSLA(System_SLACustom model)
		{
			try
			{
				var data = await _repo.System.UpdateSLA(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteSLAById")]
		public async Task<IActionResult> DeleteSLAById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.System.DeleteSLAById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetSLAById")]
		public async Task<IActionResult> GetSLAById([FromQuery] Guid id)
		{
			try
			{
				var data = await _repo.System.GetSLAById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetListSLA")]
		public async Task<IActionResult> GetListSLA([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.System.GetListSLA(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("GetConfig")]
		public async Task<IActionResult> GetConfig()
		{
			try
			{
				var response = await _repo.System.GetConfig();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpGet("GetConfigByCode")]
		public async Task<IActionResult> GetConfigByCode(string code)
		{
			try
			{
				var response = await _repo.System.GetConfigByCode(code);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateConfig")]
		public async Task<IActionResult> UpdateConfig(List<System_ConfigCustom> model)
		{
			try
			{
				await _repo.System.UpdateConfig(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

        [AllowAnonymous]
        [HttpGet("ClearDatabase")]
        public async Task<IActionResult> ClearDatabase(string code)
        {
            try
            {
                await _repo.System.ClearDatabase(code);

                return Ok("clear db success");
            }
            catch (Exception ex)
            {
                return new ErrorResultCustom(new ErrorCustom(), ex);
            }
        }

    }
}
