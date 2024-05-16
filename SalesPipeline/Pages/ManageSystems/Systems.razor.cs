using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.ManageSystems;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.ManageSystems
{
	public partial class Systems
	{
		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();

		Modal modalSignatureApprove = default!;
		private bool bClearInputSignatureApprove = false;
		//ใส่กรณี clear file แล้ว input ไม่ update
		string _inputFileId = Guid.NewGuid().ToString();
		private System_SignatureCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Systems) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetModel()
		{
			var data = await _systemViewModel.GetSignatureLast(UserInfo.Id);
			if (data != null && data.Status && data.Data != null)
			{
				formModel = data.Data;
			}
			else
			{
				_errorMessage = data?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		private async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			if (formModel.Files == null || String.IsNullOrEmpty(formModel.Files.ImgBase64Only))
			{
				HideLoading();
				_errorMessage = "เลือกรูปลายเซ็นอนุมัติ";
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
			else
			{
				formModel.Files.appSet = _appSet.Value;
				formModel.CurrentUserId = UserInfo.Id;

				var response = await _systemViewModel.CreateSignature(formModel);

				if (response.Status)
				{
					await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
					await OnHideSignature();
				}
				else
				{
					HideLoading();
					_errorMessage = response.errorMessage;
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}

		}

		private async Task OnShowSignature()
		{
			ClearInputFileMedia();
			await SetModel();
			await modalSignatureApprove.ShowAsync();
		}

		private async Task OnHideSignature()
		{
			await modalSignatureApprove.HideAsync();
		}

		private async Task OnSignatureApproveChanged(InputFileChangeEventArgs inputFileChangeEvent)
		{
			_errorMessage = null;
			formModel.ImgUrl = null;
			StateHasChanged();
			var file = inputFileChangeEvent.File;
			int _SizeLimit = 1; //MB
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

					var byteIMG = ms.ToArray();

					var extension = Path.GetExtension(file.Name);
					string base64String = Convert.ToBase64String(byteIMG);
					formModel.ImgUrl = $"data:image/{extension};base64,{base64String}";

					if (formModel.Files == null) formModel.Files = new();
					formModel.Files.appSet = new();
					formModel.Files.FileName = file.Name;
					formModel.Files.ImgBase64Only = base64String;
					formModel.Files.MimeType = extension;
				}
			}
			_inputFileId = Guid.NewGuid().ToString();
		}

		private void ClearInputFileMedia()
		{
			_errorMessage = null;
			if (formModel.ImgUrl != null)
			{
				formModel.ImgUrl = null;
			}
			StateHasChanged();
			bClearInputSignatureApprove = true;
			StateHasChanged();
			bClearInputSignatureApprove = false;
			StateHasChanged();
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