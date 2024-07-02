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
using Microsoft.JSInterop;
using System.Linq;
using System;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.API.Controllers
{
    [ApiVersion(1.0)]
	[ApiController]
	[ServiceFilter(typeof(ValidationFilterAttribute))]
	[Route("v{version:apiVersion}/[controller]")]
	public class ExportController : Controller
	{
		private IRepositoryWrapper _repo;
		private readonly AppSettings _appSet;
		private LookUpResource LookUp = new();

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

		[HttpPost("ExcelReturnedItems")]
		public async Task<IActionResult> ExcelReturnedItems(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"ReturnedItems.xlsx";

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
					string Column7 = "วันที่ส่งกลับ";
					string Column8 = "ผู้รับผิดชอบ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
							DateTime? createDate = null;
							if (item.Sale_Statuses != null)
							{
								createDate = item.Sale_Statuses.OrderByDescending(x => x.CreateDate).FirstOrDefault()?.CreateDate;
							}
							row_data[Column7] = GeneralUtils.EmptyTo(GeneralUtils.DateToThString(createDate), string.Empty);
							row_data[Column8] = GeneralUtils.EmptyTo(item.AssUserName, string.Empty);
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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelNotAchievedTarget")]
		public async Task<IActionResult> ExcelNotAchievedTarget(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"NotAchievedTarget.xlsx";

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

					string Column1 = "รหัสพนักงาน";
					string Column2 = "ชื่อ-สกุล";
					string Column3 = "กิจการสาขาภาค";
					string Column4 = "สาขา";
					string Column5 = "ยอดเป้าหมาย";
					string Column6 = "ยอดที่ทำได้";
					string Column7 = "การบรรลุเป้าหมาย";

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
					var data = await _repo.Dashboard.GetListTarget_SaleById(model);
					if (data != null && data.Items.Count > 0)
					{
						var dataDepBranchReg = await _repo.MasterBranchReg.GetBranchRegs(new allFilter() { status = StatusModel.Active });
						if (dataDepBranchReg != null)
						{
							LookUp.DepartmentBranch = new();
							if (dataDepBranchReg.Items.Count > 0)
							{
								LookUp.DepartmentBranch.AddRange(dataDepBranchReg.Items);
							}
						}

						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = GeneralUtils.EmptyTo(item.User?.EmployeeId, string.Empty);
							row_data[Column2] = GeneralUtils.EmptyTo(item.User?.FullName, string.Empty);
							if (LookUp.DepartmentBranch != null && item.User != null && item.User.Master_Branch_RegionId.HasValue)
							{
								row_data[Column3] = LookUp.DepartmentBranch.FirstOrDefault(x => x.Id == item.User.Master_Branch_RegionId.Value)?.Name ?? string.Empty;
							}
							row_data[Column4] = GeneralUtils.EmptyTo(item.User?.BranchName, string.Empty);
							row_data[Column5] = GeneralUtils.EmptyTo(item.AmountTarget.ToString(GeneralTxt.FormatDecimal2), string.Empty);
							row_data[Column6] = GeneralUtils.EmptyTo(item.AmountActual.ToString(GeneralTxt.FormatDecimal2), string.Empty);
							if (item.IsSuccessTarger)
							{
								row_data[Column7] = "สำเร็จ";
							}
							else
							{
								row_data[Column7] = "ยังไม่สำเร็จ";
							}

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

		[HttpPost("ExcelAvgDurationCloseSale")]
		public async Task<IActionResult> ExcelAvgDurationCloseSale(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgDurationCloseSale.xlsx";

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

					string Column1 = "วันที่เริ่มติดต่อ";
					string Column2 = "ชื่อลูกค้า";
					string Column3 = "ผู้ติดต่อ";
					string Column4 = "รอการติดต่อ (วัน)";
					string Column5 = "เข้าพบ (วัน)";
					string Column6 = "พิจารณาเอกสาร (วัน)";
					string Column7 = "ระยะเวลารวม (วัน)";
					string Column8 = "ผู้รับผิดชอบ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
					var data = await _repo.Dashboard.GetDuration(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = GeneralUtils.DateToThString(item.ContactStartDate);
							row_data[Column2] = item.Sale?.CompanyName;
							row_data[Column3] = item.ContactName;
							row_data[Column4] = (item.WaitContact + item.Contact);
							row_data[Column5] = item.Meet;
							row_data[Column6] = item.Document;
							row_data[Column7] = item.WaitContact + item.Contact + item.Meet + item.Document;
							row_data[Column8] = item.Sale?.AssUserName;

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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelAvgDurationLostSale")]
		public async Task<IActionResult> ExcelAvgDurationLostSale(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgDurationLostSale.xlsx";

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

					string Column1 = "วันที่เริ่มติดต่อ";
					string Column2 = "ชื่อลูกค้า";
					string Column3 = "ผู้ติดต่อ";
					string Column4 = "รอการติดต่อ (วัน)";
					string Column5 = "เข้าพบ (วัน)";
					string Column6 = "พิจารณาเอกสาร (วัน)";
					string Column7 = "ระยะเวลารวม (วัน)";
					string Column8 = "ผู้รับผิดชอบ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
					var data = await _repo.Dashboard.GetDuration(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = GeneralUtils.DateToThString(item.ContactStartDate);
							row_data[Column2] = item.Sale?.CompanyName;
							row_data[Column3] = item.ContactName;
							row_data[Column4] = (item.WaitContact + item.Contact);
							row_data[Column5] = item.Meet;
							row_data[Column6] = item.Document;
							row_data[Column7] = item.WaitContact + item.Contact + item.Meet + item.Document;
							row_data[Column8] = item.Sale?.AssUserName;

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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelClosingSale")]
		public async Task<IActionResult> ExcelClosingSale(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"ClosingSale.xlsx";

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
					string Column3 = "ห่วงโซ่";
					string Column4 = "ประเภทธุรกิจ";
					string Column5 = "กิจการสาขาภาค";
					string Column6 = "จังหวัด";
					string Column7 = "สาขา";
					string Column8 = "การปิดการขาย";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
							row_data[Column1] = item.CompanyName;
							row_data[Column2] = item.Customer?.ContactName;
							row_data[Column3] = item.Customer?.Master_ChainName;
							row_data[Column4] = item.Customer?.Master_BusinessTypeName;
							row_data[Column5] = GeneralUtils.EmptyTo(item.Master_Branch_RegionName);
							row_data[Column6] = GeneralUtils.EmptyTo(item.ProvinceName);
							row_data[Column7] = GeneralUtils.EmptyTo(item.BranchName);
							row_data[Column8] = item.StatusSaleId == StatusSaleModel.CloseSale ? "สำเร็จ" : "ไม่สำเร็จ";

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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelTargetSales")]
		public async Task<IActionResult> ExcelTargetSales(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"TargetSales.xlsx";

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
					string Column7 = "ผู้รับผิดชอบ";
					string Column8 = "ยอดการกู้";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
							row_data[Column1] = item.CompanyName;
							row_data[Column2] = item.Customer?.ContactName;
							row_data[Column3] = item.Customer?.Master_BusinessTypeName;
							row_data[Column4] = GeneralUtils.EmptyTo(item.Master_Branch_RegionName);
							row_data[Column5] = GeneralUtils.EmptyTo(item.ProvinceName);
							row_data[Column6] = GeneralUtils.EmptyTo(item.BranchName);
							row_data[Column7] = GeneralUtils.EmptyTo(item.AssUserName);
							row_data[Column8] = item.LoanAmount.HasValue ? item.LoanAmount.Value.ToString(GeneralTxt.FormatDecimal2) : string.Empty;

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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelTopSales")]
		public async Task<IActionResult> ExcelTopSales(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"TopSales.xlsx";

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

					string Column1 = "จังหวัด";
					string Column2 = "ยอดขาย";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);

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
					var data = await _repo.Dashboard.GetTopSale(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.ProvinceName;
							row_data[Column2] = item.SalesAmount.ToString(GeneralTxt.FormatDecimal2);

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

		[HttpPost("ExcelLostSales")]
		public async Task<IActionResult> ExcelLostSales(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"LostSales.xlsx";

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

					string Column1 = "จังหวัด";
					string Column2 = "ยอดขาย";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);

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
					var data = await _repo.Dashboard.GetLostSale(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.ProvinceName;
							row_data[Column2] = item.SalesAmount.ToString(GeneralTxt.FormatDecimal2);

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

		[HttpPost("ExcelDurationOnStage")]
		public async Task<IActionResult> ExcelDurationOnStage(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"DurationOnStage.xlsx";

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

					string Column1 = "เลขนิติบุคคล";
					string Column2 = "ชื่อลูกค้า";
					string Column3 = "สาขา";
					string Column4 = "ติดต่อ (วัน)";
					string Column5 = "เข้าพบ (วัน)";
					string Column6 = "ยื่นเอกสาร (วัน)";
					string Column7 = "รวม (วัน)";
					string Column8 = "ชื่อผู้รับผิดชอบ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
					var data = await _repo.Dashboard.GetDuration(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Sale?.Customer?.JuristicPersonRegNumber;
							row_data[Column2] = item.Sale?.CompanyName;
							row_data[Column3] = item.Sale?.BranchName;
							row_data[Column4] = item.WaitContact + item.Contact;
							row_data[Column5] = item.Meet;
							row_data[Column6] = item.Document;
							row_data[Column7] = item.WaitContact + item.Contact + item.Meet + item.Document;
							row_data[Column8] = GeneralUtils.EmptyTo(item.Sale?.AssUserName);

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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelAvgDealBranch")]
		public async Task<IActionResult> ExcelAvgDealBranch(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgDealBranch.xlsx";

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

					string Column1 = "สาขา";
					string Column2 = "จำนวนดีล";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);

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
					var data = await _repo.Dashboard.GetListDealBranchById(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Name;
							row_data[Column2] = item.Value.ToString("N0");
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

		[HttpPost("ExcelAvgSaleActCloseDeal")]
		public async Task<IActionResult> ExcelAvgSaleActCloseDeal(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgSaleActCloseDeal.xlsx";

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
					string Column2 = "ชื่อผู้ติดต่อ";
					string Column3 = "สาขา";
					string Column4 = "ติดต่อ (ครั้ง)";
					string Column5 = "เข้าพบ (ครั้ง)";
					string Column6 = "ยื่นเอกสาร (ครั้ง)";
					string Column7 = "รวม (ครั้ง)";

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
					var data = await _repo.Dashboard.GetActivity(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Sale?.CompanyName;
							row_data[Column2] = item.ContactName;
							row_data[Column3] = item.Sale?.BranchName;
							row_data[Column4] = item.Contact;
							row_data[Column5] = item.Meet;
							row_data[Column6] = item.Document;
							row_data[Column7] = item.Contact + item.Meet + item.Document;
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

		[HttpPost("ExcelAvgDeliveryDuration")]
		public async Task<IActionResult> ExcelAvgDeliveryDuration(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgDeliveryDuration.xlsx";

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

					string Column1 = "เลขนิติบุคคล";
					string Column2 = "ชื่อลูกค้า";
					string Column3 = "สาขา";
					string Column4 = "สสผ. ส่งมอบ (วัน)";
					string Column5 = "ผจภ. ส่งมอบ (วัน)";
					string Column6 = "ผจศ. ส่งมอบ (วัน)";
					string Column7 = "ปิดการขาย (วัน)";
					string Column8 = "ผู้รับผิดชอบ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);

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
					var data = await _repo.Dashboard.GetDeliver(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Sale?.Customer?.JuristicPersonRegNumber;
							row_data[Column2] = item.Sale?.CompanyName;
							row_data[Column3] = item.Sale?.BranchName;
							row_data[Column4] = item.LoanToBranchReg;
							row_data[Column5] = item.BranchRegToCenBranch;
							row_data[Column6] = item.CenBranchToRM;
							row_data[Column7] = item.LoanToBranchReg + item.BranchRegToCenBranch + item.CenBranchToRM;
							row_data[Column8] = GeneralUtils.EmptyTo(item.Sale?.AssUserName);
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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());

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

		[HttpPost("ExcelAvgDealRm")]
		public async Task<IActionResult> ExcelAvgDealRm(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgDealRm.xlsx";

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

					string Column1 = "รายชื่อพนักงาน";
					string Column2 = "สาขา";
					string Column3 = "จำนวนดีล";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);

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
					var data = await _repo.Dashboard.GetListDealRMById(model);
					if (data != null && data.Items.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data.Items)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Sales?.FirstOrDefault()?.AssUserName;
							row_data[Column2] = item.Sales?.FirstOrDefault()?.BranchName;
							row_data[Column3] = item.Sales?.Count;
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

		[HttpPost("ExcelNumCusTypeBusiness")]
		public async Task<IActionResult> ExcelNumCusTypeBusiness(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"NumCusTypeBusiness.xlsx";

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

					string Column1 = "ประเภทธุรกิจ";
					string Column2 = "จำนวนดีล";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);

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
					var data = await _repo.Dashboard.GetListNumberCustomer(model);
					if (data != null && data.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Name;
							row_data[Column2] = item.Value;
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

		[HttpPost("ExcelNumCusSizeBusiness")]
		public async Task<IActionResult> ExcelNumCusSizeBusiness(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"NumCusSizeBusiness.xlsx";

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

					string Column1 = "ขนาดธุรกิจ";
					string Column2 = "จำนวนดีล";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);

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
					var data = await _repo.Dashboard.GetListNumberCustomer(model);
					if (data != null && data.Count > 0)
					{
						DataRow row_data;
						foreach (var item in data)
						{
							row_data = dt.NewRow();
							row_data[Column1] = item.Name;
							row_data[Column2] = item.Value;
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
		
		[HttpPost("ExcelReasonNotLoan")]
		public async Task<IActionResult> ExcelReasonNotLoan(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"ReasonNotLoan.xlsx";

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

					string Column1 = "วันที่เริ่มติดต่อ";
					string Column2 = "ชื่อลูกค้า";
					string Column3 = "ผู้ติดต่อ";
					string Column4 = "ประเภทธุรกิจ";
					string Column5 = "กิจการสาขาภาค";
					string Column6 = "จังหวัด";
					string Column7 = "สาขา";
					string Column8 = "เหตุผล";
					string Column9 = "ผู้รับผิดชอบ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);
					dt.Columns.Add(Column9);

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
							row_data[Column1] = GeneralUtils.DateToThString(item.ContactStartDate);
							row_data[Column2] = item.CompanyName;
							row_data[Column3] = item.Customer?.ContactName;
							row_data[Column4] = item.Customer?.Master_BusinessTypeName;
							row_data[Column5] = GeneralUtils.EmptyTo(item.Master_Branch_RegionName);
							row_data[Column6] = GeneralUtils.EmptyTo(item.ProvinceName);
							row_data[Column7] = GeneralUtils.EmptyTo(item.BranchName);
							row_data[Column8] = GeneralUtils.EmptyTo(item.StatusDescription);
							row_data[Column9] = item.AssUserName;
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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column9].ToString());

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

		[HttpPost("ExcelAvgPerDeal_Region")]
		public async Task<IActionResult> ExcelAvgPerDeal_Region(allFilter model)
		{
			try
			{
				string path = @$"{_appSet.ContentRootPath}\export\excel";
				string sFileName = @"AvgPerDeal_Region.xlsx";

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

					string Column1 = "วันที่";
					string Column2 = "เลขทะเบียนนิติบุคคล";
					string Column3 = "ชื่อลูกค้า";
					string Column4 = "กิจการสาขาภาค";
					string Column5 = "สาขา";
					string Column6 = "ประเภทธุรกิจ";
					string Column7 = "เบอร์โทรศัพท์";
					string Column8 = "ผู้รับผิดชอบ";
					string Column9 = "ยอดสินเชื่อ";

					//เพิ่มคอลัมน์ลงใน Datatable
					dt.Columns.Add(Column1);
					dt.Columns.Add(Column2);
					dt.Columns.Add(Column3);
					dt.Columns.Add(Column4);
					dt.Columns.Add(Column5);
					dt.Columns.Add(Column6);
					dt.Columns.Add(Column7);
					dt.Columns.Add(Column8);
					dt.Columns.Add(Column9);

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
							row_data[Column1] = GeneralUtils.DateToThString(item.CreateDate);
							row_data[Column2] = item.Customer?.JuristicPersonRegNumber;
							row_data[Column3] = item.CompanyName;
							row_data[Column4] = GeneralUtils.EmptyTo(item.Master_Branch_RegionName);
							row_data[Column5] = GeneralUtils.EmptyTo(item.BranchName);
							row_data[Column6] = item.Customer?.Master_BusinessTypeName;
							row_data[Column7] = GeneralUtils.EmptyTo(item.Customer?.ContactTel);
							row_data[Column8] = GeneralUtils.EmptyTo(item.AssUserName);
							row_data[Column9] = item.LoanAmount.HasValue ? item.LoanAmount.Value.ToString(GeneralTxt.FormatDecimal2) : string.Empty;
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
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column8].ToString());
						row.CreateCell(cellIndex++).SetCellValue(item_row[Column9].ToString());

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
