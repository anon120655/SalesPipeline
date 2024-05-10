using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Infrastructure.Helpers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ValidationModel;
using System.Data;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Wrapper;

namespace SalesPipeline.API.Controllers
{
	//[Authorizes]
	[ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class ExportController : Controller
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;

		public ExportController(IRepositoryWrapper repo, IOptions<AppSettings> appSet)
		{
			_repo = repo;
			_appSet = appSet.Value;
		}

		[HttpPost("ExcelTotalImport")]
		public async Task<IActionResult> ExcelTotalImport(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"TotalImport.xlsx";

				var memory = new MemoryStream();

				path = path.Replace(@"\", "/");
				using (var fs = new FileStream(Path.Combine(path, sFileName), FileMode.Create, FileAccess.Write))
				{
					IWorkbook workbook = new XSSFWorkbook();
					var titleFont = workbook.CreateFont();
					titleFont.IsBold = true;
					var titleStyle = workbook.CreateCellStyle();
					titleStyle.SetFont(titleFont);

					ISheet excelSheet1 = workbook.CreateSheet("Sheet1");
					IRow row = excelSheet1.CreateRow(0);

					DataTable dt = new DataTable();

					string Column1 = "ชื่อลูกค้า";
					string Column2 = "ผู้ติดต่อ";
					string Column3 = "ประเภทธุรกิจ";
					string Column4 = "กิจการสาขาภาค";
					string Column5 = "จังหวัด";
					string Column6 = "สาขา";
					string Column7 = "สถานะ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);

					//เพิ่มคอลัมน์ลงใน Sheet Excel
					int indexCell = 0;
					foreach (DataColumn item in dt.Columns)
					{
						var cell = row.CreateCell(indexCell);
						cell.CellStyle = titleStyle;
						cell.SetCellValue(item.ColumnName.ToString());
						excelSheet1.AutoSizeColumn(indexCell);
						indexCell++;
					}

					//เพิ่มแถวลงใน Datatable
					model.pagesize = 10000;
					var data = await _repo.Sales.GetList(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = GeneralUtils.EmptyTo(item.CompanyName, string.Empty);
							row_data[Column2] = GeneralUtils.EmptyTo(item.Customer?.ContactName, string.Empty);
							row_data[Column3] = GeneralUtils.EmptyTo(item.Customer?.Master_BusinessTypeName, string.Empty);
							row_data[Column4] = GeneralUtils.EmptyTo(GeneralUtils.EmptyTo(item.Master_Branch_RegionName), string.Empty);
							row_data[Column5] = GeneralUtils.EmptyTo(GeneralUtils.EmptyTo(item.ProvinceName), string.Empty);
							row_data[Column6] = GeneralUtils.EmptyTo(GeneralUtils.EmptyTo(item.BranchName), string.Empty);
							row_data[Column7] = GeneralUtils.EmptyTo(item.StatusSaleName, string.Empty);
							dt.Rows.Add(row_data);
						}
					}

					//เพิ่มแถวลงใน Sheet Excel
					int rowIndex = 1;
					foreach (DataRow item_row in dt.Rows)
					{
						row = excelSheet1.CreateRow(rowIndex);

						int cellIndex = 0;
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column1].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column2].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column3].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column4].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column5].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column6].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column7].ToString());

						rowIndex++;
					}

					workbook.Write(fs, false);
				}
				using (var stream = new FileStream(Path.Combine(path, sFileName), FileMode.Open))
				{
					await stream.CopyToAsync(memory);
				}
				memory.Position = 0;

				return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
			}
			catch (Exception ex)
			{
				return BadRequest(new ResultModel<Boolean>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				});
			}
		}

	}
}
