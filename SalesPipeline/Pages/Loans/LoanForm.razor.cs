using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;

namespace SalesPipeline.Pages.Loans
{
	public partial class LoanForm
	{
		[Parameter]
		public Guid? id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private LookUpResource LookUp = new();
		List<LookUpResource> LookUps = new List<LookUpResource>();
		private allFilter filter = new();
		private User_PermissionCustom _permission = new();
		private LoanCustom formModel = new();

		Guid _payType1 = Guid.Parse("6b7e120f-138a-11ef-93fa-30e37aef72fb"); //อัตราดอกเบี้ยคงที่
		Guid _payType2 = Guid.Parse("753e6f06-138a-11ef-93fa-30e37aef72fb"); //อัตราดอกเบี้ยคงที่ตามรอบเวลา
		Guid _rateTypeSpecial = Guid.Parse("11e23023-18cd-11ef-93aa-30e37aef72fb"); //Special - ระบุ

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
				await SetInitManual();

				await SetModel();
				StateHasChanged();

				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");
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

		[JSInvokable]
		public async Task OnInterest_PayType(string _ids, string _name)
		{
			formModel.Master_Pre_Interest_PayTypeId = null;
			formModel.Loan_Periods = new();
			LookUp.Periods = new();
			formModel.RiskPremiumYear = 3;
			StateHasChanged();
			await Task.Delay(1);

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Interest_Periods");
			if (_ids != null)
			{
				if (Guid.TryParse(_ids, out Guid id))
				{
					formModel.Master_Pre_Interest_PayTypeId = id;

					int startPeriod = 0;
					int lengthPeriod = 0;
					if (id == _payType1)
					{
						startPeriod = 1;
						lengthPeriod = 1;
					}
					else if (id == _payType2)
					{
						startPeriod = 2;
						lengthPeriod = 10;
					}

					formModel.PeriodNumber = startPeriod;

					LookUp.Periods.Add(new SelectModel() { ID = "", Name = "เลือก" });
					for (int i = startPeriod; i <= lengthPeriod; i++)
					{
						LookUp.Periods.Add(new() { ID = i.ToString(), Name = i.ToString() });
					}

					StateHasChanged();
					await Task.Delay(1);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnInterest_Periods", "#Interest_Periods");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Interest_Periods", 100);

					if (id == _payType1)
					{
						await OnInterest_Periods("1", string.Empty);
					}
					else if (id == _payType2)
					{
						await OnInterest_Periods("2", string.Empty);
					}
				}
			}
		}

		[JSInvokable]
		public async Task OnInterest_Periods(string _ids, string _name)
		{
			LookUps = new();
			formModel.Loan_Periods = new();
			StateHasChanged();
			await Task.Delay(1);
			await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "interest_RateTypes");

			if (_ids != null)
			{
				if (int.TryParse(_ids, out int _length))
				{
					for (int i = 1; i <= _length; i++)
					{
						//await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", $"Interest_RateType_{i}");
						formModel.Loan_Periods.Add(new() { PeriodNo = i });
					}
				}

				if (formModel.Loan_Periods.Count > 0)
				{
					var Interest_RateTypeData = new List<Master_Pre_Interest_RateTypeCustom>();

					var dataRateType = await _masterViewModel.GetPre_RateType(filter);
					if (dataRateType != null && dataRateType.Status)
					{
						if (dataRateType.Data != null)
						{
							Interest_RateTypeData.AddRange(dataRateType.Data.Items);
						}
					}
					else
					{
						_errorMessage = dataRateType?.errorMessage;
						_utilsViewModel.AlertWarning(_errorMessage);
					}


					foreach (var period in formModel.Loan_Periods)
					{
						List<Master_Pre_Interest_RateTypeCustom>? rateType = new() { new() { OptionValue = $"{period.PeriodNo}@0@00000000-0000-0000-0000-000000000000", Code = "เลือก" } };
						foreach (var item in Interest_RateTypeData)
						{
							rateType.Add(new()
							{
								Id = item.Id,
								OptionValue = $"{period.PeriodNo}@{item.Rate}@{item.Id}",
								Code = item.Code,
								Name = item.Name
							});
						}

						LookUps.Add(new LookUpResource()
						{
							Interest_RateType = rateType
						});
					}
					StateHasChanged();
					await Task.Delay(10);

					foreach (var period in formModel.Loan_Periods)
					{
						await _jsRuntimes.InvokeVoidAsync("BootSelectClass", $"select_{period.PeriodNo}");
						await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnInterest_RateType", $"#Interest_RateType_{period.PeriodNo}");
					}

				}
			}
		}

		[JSInvokable]
		public async Task OnInterest_RateType(string _ids, string _name)
		{
			if (_ids != null)
			{
				List<string> stringList = _ids.Split('@').ToList();
				if (formModel.Loan_Periods != null && stringList.Count == 3)
				{
					int periodNo = int.Parse(stringList[0]);
					decimal? Rate = null;
					if (decimal.TryParse(stringList[1], out decimal _rate))
					{
						Rate = _rate;
					}
					Guid id = Guid.Parse(stringList[2]);

					var period = formModel.Loan_Periods.FirstOrDefault(x => x.PeriodNo == periodNo);
					if (period != null)
					{
						if (id != Guid.Empty)
						{
							period.Master_Pre_Interest_RateTypeId = id;
							period.RateValue = Rate;
							period.RateValueOriginal = Rate;
						}
						else
						{
							period.Master_Pre_Interest_RateTypeId = null;
							period.RateValue = null;
							period.SpecialType = null;
							period.SpecialRate = null;
						}
					}
				}
			}
			StateHasChanged();
			await Task.Delay(1);
		}

		public async Task OnSpecial(Loan_PeriodCustom period, object? value)
		{
			period.SpecialType = null;

			if (int.TryParse(value?.ToString(), out int specialType))
			{
				period.SpecialType = specialType;
			}
			StateHasChanged();
			await Task.Delay(1);
		}

		public async Task OnSpecialRate(Loan_PeriodCustom period, object? value)
		{
			period.RateValue = period.RateValueOriginal;
			period.SpecialRate = null;

			if (decimal.TryParse(value?.ToString(), out decimal val))
			{
				if (period.SpecialType == SpecialTypeModel.Plus)
				{
					period.RateValue = period.RateValueOriginal + val;
				}
				else if (period.SpecialType == SpecialTypeModel.Minus)
				{
					period.RateValue = period.RateValueOriginal - val;
				}
				else if (period.SpecialType == SpecialTypeModel.Specify)
				{
					period.RateValue = val;
				}
				else
				{
				}
			}

			StateHasChanged();
			await Task.Delay(1);
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