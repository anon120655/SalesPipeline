using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using Microsoft.Extensions.Options;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Infrastructure.Repositorys;
using SalesPipeline.Utils.ValidationModel;
using System.Net.Http;

namespace SalesPipeline.API.Controllers
{
	[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class UserController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;
		public readonly HttpClient _httpClient;

		public UserController(IRepositoryWrapper repo, IOptions<AppSettings> appSet, HttpClient httpClient)
		{
			_repo = repo;
			_appSet = appSet.Value;
			_httpClient = httpClient;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> Create(UserCustom model)
		{
			try
			{
				var data = await _repo.User.Create(model);

				try
				{
					var t = Task.Factory.StartNew(async () =>
					{
						string urlFull = $"{_appSet.baseUriApi}/v1/Mail/SendNewUser?id={data.Id}";
						HttpResponseMessage response = await _httpClient.GetAsync(urlFull);
						if (response.IsSuccessStatusCode)
						{ }
					});
				}
				catch { }

				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> Update(UserCustom model)
		{
			try
			{
				var data = await _repo.User.Update(model);

				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteById")]
		public async Task<IActionResult> DeleteById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.User.DeleteById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateStatusById")]
		public async Task<IActionResult> UpdateStatusById(UpdateModel model)
		{
			try
			{
				await _repo.User.UpdateStatusById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetById")]
		public async Task<IActionResult> GetById([FromQuery] int id)
		{
			try
			{
				var data = await _repo.User.GetById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] UserFilter model)
		{
			try
			{
				var response = await _repo.User.GetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("CreateRole")]
		public async Task<IActionResult> CreateRole(User_RoleCustom model)
		{
			try
			{
				var data = await _repo.User.CreateRole(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateRole")]
		public async Task<IActionResult> UpdateRole(User_RoleCustom model)
		{
			try
			{
				var data = await _repo.User.UpdateRole(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpDelete("DeleteRoleById")]
		public async Task<IActionResult> DeleteRoleById([FromQuery] UpdateModel model)
		{
			try
			{
				await _repo.User.DeleteRoleById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateIsModifyRoleById")]
		public async Task<IActionResult> UpdateIsModifyRoleById(UpdateModel model)
		{
			try
			{
				await _repo.User.UpdateIsModifyRoleById(model);
				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetRoleById")]
		public async Task<IActionResult> GetRoleById([FromQuery] int id)
		{
			try
			{
				var data = await _repo.User.GetRoleById(id);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetListRole")]
		public async Task<IActionResult> GetListRole([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.User.GetListRole(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpGet("GetListLevel")]
		public async Task<IActionResult> GetListLevel([FromQuery] allFilter model)
		{
			try
			{
				var response = await _repo.User.GetListLevel(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("ValidateUpload")]
		public async Task<IActionResult> ValidateUpload(List<UserCustom> model)
		{
			try
			{
				var data = await _repo.User.ValidateUpload(model);
				return Ok(data);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[AllowAnonymous]
		[HttpPost("ImportUser")]
		public async Task<IActionResult> ImportUser(IFormFile files)
		{
			try
			{
				await Task.Delay(1);
				List<UserCustom> UserList = new List<UserCustom>();

				if (files == null) throw new ExceptionCustom("Select File.");

				int fileLimit = 100; //MB
				int TenMegaBytes = fileLimit * 1024 * 1024;
				var fileSize = files.Length;
				if (fileSize > TenMegaBytes)
				{
					throw new ExceptionCustom($"ขนาดไฟล์ไม่เกิน {fileLimit} MB");
				}

				string folderName = @$"{_appSet.ContentRootPath}\import\excel";

				if (files.Length > 0)
				{
					string sFileExtension = Path.GetExtension(files.FileName).ToLower();
					if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
						throw new ExceptionCustom("FileExtension Not Support.");

					ISheet sheet;
					string fullPath = Path.Combine(folderName, files.FileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						files.CopyTo(stream);
						stream.Position = 0;
						int sheetCount = 0;
						if (sFileExtension == ".xls")
						{
							throw new ExceptionCustom("not support  Excel 97-2000 formats.");
						}

						XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
						sheetCount = hssfwb.NumberOfSheets;
						sheet = hssfwb.GetSheetAt(0);

						int firstRowNum = sheet.FirstRowNum;
						for (int i = (firstRowNum + 1); i <= sheet.LastRowNum; i++)
						{
							IRow row = sheet.GetRow(i);
							if (row == null) continue;
							if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

							var EmployeeId = row.GetCell(0) != null ? row.GetCell(0).ToString() : null;
							var FullName = row.GetCell(1) != null ? row.GetCell(1).ToString() : null;
							var Email = row.GetCell(2) != null ? row.GetCell(2).ToString() : null;
							var PositionId = row.GetCell(3) != null ? row.GetCell(3).ToString() : null;
							var LevelId = row.GetCell(4) != null ? row.GetCell(4).ToString() : null;
							var RoleId = row.GetCell(5) != null ? row.GetCell(5).ToString() : null;

							int.TryParse(PositionId, out int _positionId);
							int.TryParse(LevelId, out int _levelid);
							int.TryParse(RoleId, out int _roleid);

							UserList.Add(new UserCustom()
							{
								CurrentUserId = 2,
								EmployeeId = EmployeeId,
								FullName = FullName,
								Email = Email,
								PositionId = _positionId > 0 ? _positionId : null,
								LevelId = _levelid > 0 ? _levelid : null,
								RoleId = _roleid > 0 ? _roleid : null
							});

						}

					}
				}

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("GetUserTargetList")]
		public async Task<IActionResult> GetUserTargetList(allFilter model)
		{
			try
			{
				var response = await _repo.User.GetUserTargetList(model);

				return Ok(response);
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPut("UpdateUserTarget")]
		public async Task<IActionResult> UpdateUserTarget(User_Main model)
		{
			try
			{
				await _repo.User.UpdateUserTarget(model);

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

		[HttpPost("ChangePassword")]
		public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
		{
			try
			{
				await _repo.User.ChangePassword(model);

				return Ok();
			}
			catch (Exception ex)
			{
				return new ErrorResultCustom(new ErrorCustom(), ex);
			}
		}

	}
}
