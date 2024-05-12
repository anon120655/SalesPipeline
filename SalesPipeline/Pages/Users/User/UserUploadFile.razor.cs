using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using SalesPipeline.Utils;
using BlazorBootstrap;

namespace SalesPipeline.Pages.Users.User
{
	public partial class UserUploadFile
	{
		private string? _errorMessage = null;
		private bool isLoading = false;

		private bool bClearInputFile = false;
		private string _inputFileId = Guid.NewGuid().ToString();

		private User_PermissionCustom _permission = new();
		List<UserCustom>? UserList = new();
		private Modal modalResult = default!;
		List<ResultImport> resultImport = new();
		private string dropClass = "";
		private string? _fileName = null;
		private List<string> header_list_key = new();

		protected override void OnInitialized()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.RMUser) ?? new User_PermissionCustom();
			StateHasChanged();
		}

		protected async Task OnChooseFile(InputFileChangeEventArgs inputFileChangeEvent)
		{
			_errorMessage = null;
			UserList = null;
			var file = inputFileChangeEvent.File;

			int _SizeLimit = 10; //MB

			int TenMegaBytes = _SizeLimit * 1024 * 1024;
			var fileSize = file.Size;
			if (fileSize > TenMegaBytes)
			{
				ClearInputFile();
				_errorMessage = $"Limited Max. {_SizeLimit} MB per file.";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				try
				{
					using (var stream = file.OpenReadStream(TenMegaBytes))
					{
						MemoryStream ms = new MemoryStream();
						await stream.CopyToAsync(ms);
						stream.Close();

						var bytefile = ms.ToArray();
						IFormFile files = new FormFile(ms, 0, bytefile.Length, "name", "UserImport.xlsx");
						if (files == null) throw new ExceptionCustom("File Not Support.");

						string folderName = @$"{_appSet.Value.ContentRootPath}\import\excel";

						if (files.Length > 0)
						{
							string sFileExtension = Path.GetExtension(files.FileName).ToLower();
							if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
								throw new ExceptionCustom("FileExtension Not Support.");

							ISheet sheet;
							string fullPath = Path.Combine(folderName, files.FileName);
							using (var streamread = new FileStream(fullPath, FileMode.Create))
							{
								files.CopyTo(streamread);
								streamread.Position = 0;
								int sheetCount = 0;
								if (sFileExtension == ".xls")
								{
									throw new ExceptionCustom("not support  Excel 97-2000 formats.");
								}

								XSSFWorkbook hssfwb = new XSSFWorkbook(streamread); //This will read 2007 Excel format  
								sheetCount = hssfwb.NumberOfSheets;
								sheet = hssfwb.GetSheetAt(0);

								IRow row_header = sheet.GetRow(0);
								try
								{
									var row_0 = row_header.GetCell(0).ToString()?.Trim();
									var row_1 = row_header.GetCell(1).ToString()?.Trim();
									var row_2 = row_header.GetCell(2).ToString()?.Trim();
									var row_3 = row_header.GetCell(3).ToString()?.Trim();
									var row_4 = row_header.GetCell(4).ToString()?.Trim();
									var row_5 = row_header.GetCell(5).ToString()?.Trim();
									var row_6 = row_header.GetCell(6).ToString()?.Trim();
									var row_7 = row_header.GetCell(7).ToString()?.Trim();
									var row_8 = row_header.GetCell(8).ToString()?.Trim();
									var row_9 = row_header.GetCell(9).ToString()?.Trim();
									if (row_0 != "รหัสพนักงาน"
										|| row_1 != "ชื่อ-สกุล"
										|| row_2 != "Email"
										|| row_3 != "Tel"
										|| row_4 != "ตำแหน่ง"
										|| row_5 != "ระดับหน้าที่"
										|| row_6 != "ระดับ"
										|| row_7 != "กิจการสาขาภาค"
										|| row_8 != "จังหวัด"
										|| row_9 != "สาขา")
									{
										throw new ExceptionCustom("Template file not support.");
									}
								}
								catch
								{
									throw new ExceptionCustom("Template file not support.");
								}


								Dictionary<string, int> header_list =
														row_header.Cells
																  .Select(x => new { x.StringCellValue, x.ColumnIndex })
																  .ToDictionary(x => x.StringCellValue, x => x.ColumnIndex);

								header_list_key = header_list.Select(x => x.Key).ToList();
								if (header_list_key.Count < 10)
								{
									throw new ExceptionCustom("Template file not support.");
								}

								string? EmployeeId = null;
								string? FullName = null;
								string? Email = null;
								string? Tel = null;
								int? PositionId = null;
								int? RoleId = null;
								int? LevelId = null;
								Guid? Master_Branch_RegionId = null;
								int? ProvinceId = null;
								int? BranchId = null;

								for (var rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
								{
									EmployeeId = null;
									FullName = null;
									Email = null;
									Tel = null;
									PositionId = null;
									RoleId = null;
									LevelId = null;
									Master_Branch_RegionId = null;
									ProvinceId = null;
									BranchId = null;

									var row = sheet.GetRow(rowIndex);
									int cellIndex = 0;
									int idMaster = 0;
									Guid guidMaster = Guid.Empty;

									if (header_list.TryGetValue("รหัสพนักงาน", out cellIndex))
									{
										EmployeeId = row.GetCell(cellIndex).ToString();
									}
									if (header_list.TryGetValue("ชื่อ-สกุล", out cellIndex))
									{
										FullName = row.GetCell(cellIndex).ToString();
									}
									if (header_list.TryGetValue("Email", out cellIndex))
									{
										Email = row.GetCell(cellIndex).ToString();
									}
									if (header_list.TryGetValue("Tel", out cellIndex))
									{
										Tel = row.GetCell(cellIndex).ToString();
									}
									if (header_list.TryGetValue("ตำแหน่ง", out cellIndex))
									{
										if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										{
											PositionId = idMaster;
										}
									}
									if (header_list.TryGetValue("ระดับหน้าที่", out cellIndex))
									{
										if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										{
											RoleId = idMaster;
										}
									}
									if (header_list.TryGetValue("ระดับ", out cellIndex))
									{
										if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										{
											LevelId = idMaster;
										}
									}
									if (header_list.TryGetValue("กิจการสาขาภาค", out cellIndex))
									{
										if (Guid.TryParse(row.GetCell(cellIndex).ToString(), out guidMaster))
										{
											Master_Branch_RegionId = guidMaster;
										}
									}
									if (header_list.TryGetValue("จังหวัด", out cellIndex))
									{
										if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										{
											ProvinceId = idMaster;
										}
									}
									if (header_list.TryGetValue("สาขา", out cellIndex))
									{
										if (int.TryParse(row.GetCell(cellIndex).ToString(), out idMaster))
										{
											BranchId = idMaster;
										}
									}

									if (UserList == null) UserList = new();

									if (string.IsNullOrEmpty(EmployeeId))
										throw new ExceptionCustom("รหัสพนักงาน ไม่ครบถ้วน");

									if (string.IsNullOrEmpty(FullName))
										throw new ExceptionCustom("ชื่อ-สกุล ไม่ครบถ้วน");

									if (string.IsNullOrEmpty(Email))
										throw new ExceptionCustom("Email ไม่ครบถ้วน");

									if (string.IsNullOrEmpty(Tel))
										throw new ExceptionCustom("Tel ไม่ครบถ้วน");

									if (!PositionId.HasValue)
										throw new ExceptionCustom("ตำแหน่ง ไม่ครบถ้วน");

									if (UserList.Select(x => x.EmployeeId).Any(x => x == EmployeeId))
										throw new ExceptionCustom("มีรหัสพนักงานซ้ำ กรุณาตรวจสอบอีกครั้ง");

									if (!RoleId.HasValue)
										throw new ExceptionCustom("ระดับหน้าที่ ไม่ครบถ้วน");

									if (RoleId < 5 || RoleId > 8)
									{
										throw new ExceptionCustom("ระดับหน้าที่ไม่ถูกต้อง ต้องอยู่ในช่วง(5-8)");
									}

									if (!LevelId.HasValue)
										throw new ExceptionCustom("ระดับ ไม่ครบถ้วน");

									if (!Master_Branch_RegionId.HasValue)
										throw new ExceptionCustom("กิจการสาขาภาค ไม่ครบถ้วน");

									if (!ProvinceId.HasValue)
										throw new ExceptionCustom("จังหวัด ไม่ครบถ้วน");

									if (!BranchId.HasValue)
										throw new ExceptionCustom("สาขา ไม่ครบถ้วน");

									UserList.Add(new UserCustom()
									{
										Status = StatusModel.Active,
										CurrentUserId = UserInfo.Id,
										EmployeeId = EmployeeId,
										FullName = FullName,
										Email = Email,
										Tel = Tel,
										PositionId = PositionId,
										RoleId = RoleId,
										LevelId = LevelId,
										Master_Branch_RegionId = Master_Branch_RegionId,
										ProvinceId = ProvinceId,
										BranchId = BranchId,
									});
								}

								if (UserList?.Count > 0)
								{
									var response = await _userViewModel.ValidateUpload(UserList);

									if (response.Status)
									{
										UserList = response.Data?.OrderBy(x => x.IsValidate == true).ToList();
									}
									else
									{
										HideLoading();
										_errorMessage = response.errorMessage;
										await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					ClearInputFile();
					_errorMessage = ex.Message;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}

		}

		protected async Task Save()
		{
			//UserList = UserList?.Where(x => x.IsValidate == true).ToList();

			if (UserList == null || UserList.Count(x => x.IsValidate == true) == 0)
			{
				_errorMessage = "ตรวจสอบไฟล์แนบอีกครั้ง";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				_errorMessage = null;
				ShowLoading();

				foreach (var item in UserList.Where(x => x.IsValidate == true).ToList())
				{
					var resultModel = new ResultImport();
					resultModel.Name = item.FullName;

					var response = await _userViewModel.Create(item);
					if (!response.Status)
					{
						resultModel.Success = false;
						resultModel.errorMessage = response.errorMessage;
					}
					resultImport.Add(resultModel);
				}

				await OnShowResult();
			}

		}

		public void Cancel()
		{
			_Navs.NavigateTo("/user");
		}

		protected void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		protected void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}

		protected void ClearInputFile()
		{
			UserList = new();
			bClearInputFile = true;
			StateHasChanged();
			bClearInputFile = false;
			StateHasChanged();
		}

		private async Task OnShowResult()
		{
			HideLoading();
			await modalResult.ShowAsync();
		}

		private async Task OnHideResult()
		{
			await modalResult.HideAsync();
		}

		private void OnHiddenResult()
		{
			Cancel();
		}

		private void HandleDragEnter()
		{
			dropClass = "dropzone-drag";
		}

		private void HandleDragLeave()
		{
			dropClass = "";
		}



	}
}