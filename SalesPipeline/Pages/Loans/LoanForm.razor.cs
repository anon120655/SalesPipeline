using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;

namespace SalesPipeline.Pages.Loans
{
	public partial class LoanForm
	{
		[Parameter]
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private LookUpResource LookUp = new();
		private allFilter filter = new();
		private User_PermissionCustom _permission = new();
		private LoanCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Loan) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await BootSelectInit();

				await SetModel();
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var dataRateType = await _masterViewModel.GetPre_PayType(filter);
			if (dataRateType != null && dataRateType.Status)
			{
				LookUp.Interest_PayType = dataRateType.Data?.Items;
				StateHasChanged();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnInterest_PayType", "#Interest_PayType");
			}
			else
			{
				_errorMessage = dataRateType?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}
		}

		protected async Task BootSelectInit()
		{
			await Task.Delay(10);
			await SetInitManual();
			await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");
		}

		[JSInvokable]
		public async Task OnInterest_PayType(string _ids, string _name)
		{
			formModel.Master_Pre_Interest_PayTypeId = null;
			formModel.Loan_Periods = new();
			LookUp.Periods = new();
			StateHasChanged();
			await Task.Delay(1);

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Periods");
			if (_ids != null)
			{
				if (Guid.TryParse(_ids, out Guid id))
				{
					formModel.Master_Pre_Interest_PayTypeId = id;

					int startPeriod = 0;
					int lengthPeriod = 0;
					if (id == Guid.Parse("6b7e120f-138a-11ef-93fa-30e37aef72fb")) //อัตราดอกเบี้ยคงที่
					{
						startPeriod = 1;
						lengthPeriod = 1;
					}
					else if (id == Guid.Parse("753e6f06-138a-11ef-93fa-30e37aef72fb")) //อัตราดอกเบี้ยคงที่ตามรอบเวลา
					{
						startPeriod = 2;
						lengthPeriod = 10;
					}

					LookUp.Periods.Add(new SelectModel() { ID = "", Name = "เลือก" });
					for (int i = startPeriod; i <= lengthPeriod; i++)
					{
						LookUp.Periods.Add(new() { ID = i.ToString(), Name = i.ToString(), IsSelected = (i == 1 && startPeriod == 1) });
					}

					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnPeriods", "#Periods");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Periods", 100);

					if (id == Guid.Parse("6b7e120f-138a-11ef-93fa-30e37aef72fb"))
					{
						await OnPeriods("1", "");
					}

				}
			}
		}

		[JSInvokable]
		public async Task OnPeriods(string _ids, string _name)
		{
			formModel.Loan_Periods = new();

			if (_ids != null)
			{
				if (int.TryParse(_ids, out int _length))
				{
					for (int i = 1; i <= _length; i++)
					{
						formModel.Loan_Periods.Add(new() { PeriodNo = i });
					}
				}
			}
			StateHasChanged();
			await Task.Delay(1);
		}

		protected async Task SetModel()
		{
			if (id.HasValue)
			{
				var data = await _loanViewModel.GetById(id.Value);
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
		}

		protected async Task OnInvalidSubmit()
		{
			await Task.Delay(100);
			await _jsRuntimes.InvokeVoidAsync("scrollToElement", "validation-message");
		}

		protected async Task Save()
		{
			_errorMessage = null;
			ShowLoading();

			ResultModel<LoanCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id.HasValue)
			{
				response = await _loanViewModel.Update(formModel);
			}
			else
			{
				response = await _loanViewModel.Create(formModel);
			}

			if (response.Status)
			{
				await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
				Cancel();
			}
			else
			{
				HideLoading();
				_errorMessage = response.errorMessage;
				await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
			}
		}

		public void Cancel()
		{
			_Navs.NavigateTo("/loan");
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

	}
}