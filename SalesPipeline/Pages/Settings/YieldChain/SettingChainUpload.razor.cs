using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.YieldChain
{
    public partial class SettingChainUpload
    {
        private string? _errorMessage = null;
        private bool isLoading = false;

        private bool bClearInputFile = false;
        private string _inputFileId = Guid.NewGuid().ToString();

        private User_PermissionCustom _permission = new();
        List<Master_ChainCustom>? ChainList = new();
        private Modal modalResult = default!;
        List<ResultImport> resultImport = new();
        private string dropClass = "";
        private string? _fileName = null;
        private List<string> header_list_key = new();

        protected override void OnInitialized()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetYieldChain) ?? new User_PermissionCustom();
            StateHasChanged();
        }

        protected async Task OnChooseFile(InputFileChangeEventArgs inputFileChangeEvent)
        {
            _errorMessage = null;
            ChainList = null;
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
                        IFormFile files = new FormFile(ms, 0, bytefile.Length, "name", "ChainImport.xlsx");
                        if (files == null) throw new Exception("File Not Support.");

                        string folderName = @$"{_appSet.Value.ContentRootPath}\import\excel";

                        if (files.Length > 0)
                        {
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

                                IRow row_header = sheet.GetRow(0);
                                try
                                {
                                    var row_0 = row_header.GetCell(0).ToString()?.Trim();
                                    if (row_0 != "ชื่อห่วงโซ่")
                                    {
                                        throw new Exception("Template file not support.");
                                    }
                                }
                                catch
                                {
                                    throw new Exception("Template file not support.");
                                }


                                Dictionary<string, int> header_list =
                                                        row_header.Cells
                                                                  .Select(x => new { x.StringCellValue, x.ColumnIndex })
                                                                  .ToDictionary(x => x.StringCellValue, x => x.ColumnIndex);

                                header_list_key = header_list.Select(x => x.Key).ToList();
                                if (header_list_key.Count != 1)
                                {
                                    throw new Exception("Template file not support.");
                                }

                                string? Name = null;

                                for (var rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                                {
                                    Name = null;

                                    var row = sheet.GetRow(rowIndex);
                                    int cellIndex = 0;

                                    if (header_list.TryGetValue("ชื่อห่วงโซ่", out cellIndex))
                                    {
                                        Name = row.GetCell(cellIndex).ToString();
                                    }

                                    if (ChainList == null) ChainList = new();

                                    if (string.IsNullOrEmpty(Name))
                                        throw new Exception("ชื่อห่วงโซ่ ไม่ครบถ้วน");

                                    ChainList.Add(new()
                                    {
                                        Status = StatusModel.Active,
                                        CurrentUserId = UserInfo.Id,
                                        Name = Name,
                                    });
                                }

                                if (ChainList?.Count > 0)
                                {
                                    var response = await _masterViewModel.ValidateUploadChain(ChainList);

                                    if (response.Status)
                                    {
                                        ChainList = response.Data?.OrderBy(x => x.IsValidate == true).ToList();
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

            if (ChainList == null || ChainList.Count(x => x.IsValidate == true) == 0)
            {
                _errorMessage = "ตรวจสอบไฟล์แนบอีกครั้ง";
                await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
            }
            else
            {
                _errorMessage = null;
                ShowLoading();

                foreach (var item in ChainList.Where(x => x.IsValidate == true).ToList())
                {
                    var resultModel = new ResultImport();
                    resultModel.Name = item.Name;

                    var response = await _masterViewModel.CreateChain(item);
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
            _Navs.NavigateTo("/setting/yieldchain");
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
            ChainList = new();
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