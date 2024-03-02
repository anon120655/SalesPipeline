using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetTopologySuite.Index.HPRtree;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils.ValidationModel;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class ImportController : ControllerBase
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public ImportController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpPost("InfoBranch")]
		public async Task<IActionResult> InfoBranch(IFormFile files)
		{
			try
			{
				await Task.Delay(1);
				List<InfoBranchCustom> InfoBranchList = new List<InfoBranchCustom>();

				if (files == null) throw new Exception("Select File.");

				int fileLimit = 100; //MB
				int TenMegaBytes = fileLimit * 1024 * 1024;
				var fileSize = files.Length;
				if (fileSize > TenMegaBytes)
				{
					throw new Exception($"ขนาดไฟล์ไม่เกิน {fileLimit} MB");
				}

				string folderName = @$"{_appSet.ContentRootPath}\import\excel";

				if (files.Length > 0)
				{
					string sFileExtension = Path.GetExtension(files.FileName).ToLower();
					if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
						throw new Exception("FileExtension Not Support.");

					ISheet sheet;
					string fullPath = Path.Combine(folderName, files.FileName);
					using (var stream = new FileStream(fullPath, FileMode.Create))
					{
						files.CopyTo(stream);
						stream.Position = 0;
						int sheetCount = 0;
						if (sFileExtension == ".xls")
						{
							throw new Exception("not support  Excel 97-2000 formats.");
						}

						XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
						sheetCount = hssfwb.NumberOfSheets;
						sheet = hssfwb.GetSheetAt(2);

						int firstRowNum = sheet.FirstRowNum;
						for (int i = (firstRowNum + 1); i <= sheet.LastRowNum; i++)
						{
							IRow row = sheet.GetRow(i);
							if (row == null) continue;
							if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

							var BranchName = row.GetCell(0) != null ? row.GetCell(0).ToString() : null;
							var BranchNameMain = row.GetCell(1) != null ? row.GetCell(1).ToString() : null;
							var ProvinceName = row.GetCell(2) != null ? row.GetCell(2).ToString() : null;
							var ProvinceId = row.GetCell(3) != null ? row.GetCell(3).ToString() : null;
							var BusinessBranch = row.GetCell(4) != null ? row.GetCell(4).ToString() : null;
							var Code = row.GetCell(5) != null ? row.GetCell(5).ToString() : null;

							int.TryParse(ProvinceId, out int _ProvinceId);
							//int.TryParse(LevelId, out int _levelid);
							//int.TryParse(RoleId, out int _roleid);

							InfoBranchList.Add(new()
							{
								ProvinceID = _ProvinceId,
								BranchName = BranchName,
								BranchNameMain = BranchNameMain,
							});
						}

						InfoBranchList = InfoBranchList.OrderBy(x=>x.ProvinceID).ToList();

						using (var _transaction = _repo.BeginTransaction())
						{
							foreach (var item in InfoBranchList)
							{
								await _repo.Thailand.CreateBranch(item);
							}

							_transaction.Commit();
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



	}
}
