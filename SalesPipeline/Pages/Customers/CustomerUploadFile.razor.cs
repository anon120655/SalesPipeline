using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Customers
{
	public partial class CustomerUploadFile
	{

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private allFilter filter = new();
		List<UserCustom>? UserList = new();

		//ใส่กรณี clear file แล้ว input ไม่ update
		private string _inputFileId = Guid.NewGuid().ToString();
		private bool bClearInputFile = false;
		private Modal modalResult = default!;
		List<ResultImport> resultImport = new();
		private string dropClass = "";
		private string? _fileName = null;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Customers) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await Task.Delay(10);
				firstRender = false;
			}
		}

		protected async Task OnChooseFile(InputFileChangeEventArgs inputFileChangeEvent)
		{
			_fileName = null;
			dropClass = "";
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
						IFormFile files = new FormFile(ms, 0, bytefile.Length, "name", "LoanUser.xlsx");
						if (files == null) throw new Exception("File Not Support.");

						string folderName = @$"{_appSet.Value.ContentRootPath}\import\excel";

						if (files.Length > 0)
						{
							_fileName = files.FileName;
							string sFileExtension = Path.GetExtension(files.FileName).ToLower();
							if (sFileExtension != ".xls" && sFileExtension != ".xlsx" && sFileExtension != ".csv")
								throw new Exception("FileExtension Not Support.");

							ISheet sheet;
							string fullPath = Path.Combine(folderName, files.FileName);
							using (var streamread = new FileStream(fullPath, FileMode.Create))
							{
								files.CopyTo(streamread);
								streamread.Position = 0;
								int sheetCount = 0;
								if (sFileExtension == ".xls")
								{
									throw new Exception("not support  Excel 97-2000 formats.");
								}

								XSSFWorkbook hssfwb = new XSSFWorkbook(streamread); //This will read 2007 Excel format  
								sheetCount = hssfwb.NumberOfSheets;
								sheet = hssfwb.GetSheetAt(0);

								IRow row_check = sheet.GetRow(0);
								var row_0 = row_check.GetCell(0).ToString()?.Trim();
								var row_1 = row_check.GetCell(1).ToString()?.Trim();
								var row_2 = row_check.GetCell(2).ToString()?.Trim();
								var row_3 = row_check.GetCell(3).ToString()?.Trim();
								var row_4 = row_check.GetCell(4).ToString()?.Trim();
								var row_5 = row_check.GetCell(5).ToString();
								if (row_0 != "รหัสพนักงาน"
									|| row_1 != "ชื่อ-สกุล"
									|| row_2 != "Email"
									|| row_3 != "ตำแหน่ง"
									|| row_4 != "ระดับ"
									|| row_5 != "ระดับหน้าที่")
								{
									throw new Exception("Template file not support.");
								}

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

									if (UserList == null) UserList = new();

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
			_Navs.NavigateTo("/customer");
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