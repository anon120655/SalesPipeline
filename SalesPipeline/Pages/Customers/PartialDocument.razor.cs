using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Sales;
using System.IO;
using static System.Net.WebRequestMethods;

namespace SalesPipeline.Pages.Customers
{
	public partial class PartialDocument
	{
		[Parameter]
		public SaleCustom? formModel { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		bool showDocumentType = true;

		Modal modalUploadFile = default!;
		private bool bClearInput = false;
		//ใส่กรณี clear file แล้ว input ไม่ update
		string _inputFileId = Guid.NewGuid().ToString();

		Sale_DocumentCustom? document = null;
		List<Sale_Document_UploadCustom>? document_Upload = null;
		Sale_Document_UploadCustom formUploadModel = new();

		protected override async Task OnParametersSetAsync()
		{
			if (formModel != null)
			{
				formUploadModel.CurrentUserId = UserInfo.Id;
				formUploadModel.SaleId = formModel.Id;

				document = formModel.Sale_Documents?.FirstOrDefault();
				document_Upload = formModel.Sale_Document_Files;
				await Task.Delay(1);
			}
		}

		void ToggleDocumentType()
		{
			showDocumentType = !showDocumentType;
		}

		private async Task OnUploadFileChanged(InputFileChangeEventArgs inputFileChangeEvent)
		{
			_errorMessage = null;
			StateHasChanged();
			var file = inputFileChangeEvent.File;
			int _SizeLimit = 5; //MB
			var _Size = 1024000 * _SizeLimit;

			if (file.Size > _Size)
			{
				_errorMessage = $"Limited Max. {_SizeLimit} MB per file.";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				using (var stream = file.OpenReadStream(_Size))
				{
					MemoryStream ms = new MemoryStream();
					await stream.CopyToAsync(ms);

					var fileBytes = ms.ToArray();

					if (formUploadModel.Files == null) formUploadModel.Files = new();
					formUploadModel.Files.appSet = new();
					formUploadModel.Files.FileName = file.Name;
					formUploadModel.Files.FileName = file.Name;
					formUploadModel.Files.FileByte = fileBytes;
				}
			}
			_inputFileId = Guid.NewGuid().ToString();
		}

		private void ClearInputFileMedia()
		{
			_errorMessage = null;		
			StateHasChanged();
			bClearInput = true;
			StateHasChanged();
			bClearInput = false;
			StateHasChanged();
		}

		private async Task SaveFile()
		{
			_errorMessage = null;
			ShowLoading();

			if (formUploadModel.Files == null || formUploadModel.Files.FileByte == null)
			{
				HideLoading();
				_errorMessage = "เลือกไฟล์";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				formUploadModel.Files.appSet = _appSet.Value;
				formUploadModel.CurrentUserId = UserInfo.Id;

				var response = await _processSaleViewModel.CreateDocumentFile(formUploadModel);

				if (response.Status)
				{
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
					await OnHideUploadFile();
				}
				else
				{
					HideLoading();
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}

		}

		private async Task OnShowUploadFile()
		{
			ClearInputFileMedia();
			await modalUploadFile.ShowAsync();
		}

		private async Task OnHideUploadFile()
		{
			await modalUploadFile.HideAsync();
		}

		private void ShowLoading()
		{
			isLoading = true;
			StateHasChanged();
		}

		private void HideLoading()
		{
			isLoading = false;
			StateHasChanged();
		}

	}
}