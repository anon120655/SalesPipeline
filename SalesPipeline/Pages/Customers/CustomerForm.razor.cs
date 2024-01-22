using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;

namespace SalesPipeline.Pages.Customers
{
	public partial class CustomerForm
	{
		[Parameter]
		public Guid id { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private CustomerCustom formModel = new();

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Customers) ?? new User_PermissionCustom();
			StateHasChanged();

			//await SetInitManual();
			await SetModel();
		}

		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SetInitManual();
				await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");
				StateHasChanged();
				firstRender = false;
			}
		}

		protected async Task SetInitManual()
		{
			var contactChannel = await _masterViewModel.GetContactChannel(new allFilter() { status = StatusModel.Active });
			if (contactChannel != null && contactChannel.Status)
			{
				LookUp.ContactChannel = contactChannel.Data?.Items;
			}
			else
			{
				_errorMessage = contactChannel?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var businessType = await _masterViewModel.GetBusinessType(new allFilter() { status = StatusModel.Active });
			if (businessType != null && businessType.Status)
			{
				LookUp.BusinessType = businessType.Data?.Items;
			}
			else
			{
				_errorMessage = businessType?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var businessSize = await _masterViewModel.GetBusinessSize(new allFilter() { status = StatusModel.Active });
			if (businessSize != null && businessSize.Status)
			{
				LookUp.BusinessSize = businessSize.Data?.Items;
			}
			else
			{
				_errorMessage = businessSize?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var iSICCode = await _masterViewModel.GetISICCode(new allFilter() { status = StatusModel.Active });
			if (iSICCode != null && iSICCode.Status)
			{
				LookUp.ISICCode = iSICCode.Data?.Items;
			}
			else
			{
				_errorMessage = iSICCode?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "ContactChannel");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessType");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessSize");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "ISICCode");


			var yields = await _masterViewModel.GetYields(new allFilter() { status = StatusModel.Active });
			if (yields != null && yields.Status)
			{
				LookUp.Yield = yields.Data?.Items;
			}
			else
			{
				_errorMessage = yields?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			var chain = await _masterViewModel.GetChains(new allFilter() { status = StatusModel.Active });
			if (chain != null && chain.Status)
			{
				LookUp.Chain = chain.Data?.Items;
			}
			else
			{
				_errorMessage = chain?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Yield");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Chain");


			var province = await _masterViewModel.GetProvince();
			if (province != null && province.Status)
			{
				LookUp.Provinces = province.Data;
			}
			else
			{
				_errorMessage = province?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");
		}

		[JSInvokable]
		public async Task ProvinceChange(string _provinceID, string _provinceName)
		{
			await Task.Delay(1);
			LookUp.Amphurs = new List<InfoAmphurCustom>();
			LookUp.Tambols = new List<InfoTambolCustom>();
			this.StateHasChanged();

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				formModel.ProvinceId = provinceID;

				var amphurs = await _masterViewModel.GetAmphur(provinceID);
				if (amphurs != null && amphurs.Data?.Count > 0)
				{
					LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
					LookUp.Amphurs.AddRange(amphurs.Data);
				}

				this.StateHasChanged();
				await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", "#Amphur");
				//await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
				//await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Tambol", 100);
			}
		}

		[JSInvokable]
		public async Task AmphurChange(string _amphurID, string _amphurName)
		{
			if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
			{
				LookUp.Tambols = new List<InfoTambolCustom>();
				this.StateHasChanged();
				await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Tambol");

				formModel.AmphurId = amphurID;

				var tambols = await _masterViewModel.GetTambol(formModel.ProvinceId ?? 0, amphurID);
				if (tambols != null && tambols.Data != null && tambols.Data.Count > 0)
				{
					LookUp.Tambols = new List<InfoTambolCustom> { new InfoTambolCustom() { TambolID = 0, TambolName = "--เลือก--" } };
					LookUp.Tambols.AddRange(tambols.Data);
				}

				this.StateHasChanged();
				await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "TambolChange", "#Tambol");
				//await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "tambolBehalf", 100);
			}
		}

		[JSInvokable]
		public void TambolChange(string _tambolID, string _tambolName)
		{
			if (_tambolID != null && int.TryParse(_tambolID, out int tambolID))
			{
				formModel.TambolId = tambolID;
			}
		}

		protected async Task SetModel()
		{
			if (id != Guid.Empty)
			{
				var data = await _customerViewModel.GetById(id);
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

			if (formModel.Customer_Committees == null || formModel.Customer_Committees.Count == 0)
			{
				formModel.Customer_Committees = new() { new() { Id = Guid.NewGuid() } };
			}

			if (formModel.Customer_Shareholders == null || formModel.Customer_Shareholders.Count == 0)
			{
				formModel.Customer_Shareholders = new() { new() { Id = Guid.NewGuid() } };
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

			ResultModel<CustomerCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			if (id != Guid.Empty)
			{
				response = await _customerViewModel.Update(formModel);
			}
			else
			{
				response = await _customerViewModel.Create(formModel);
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
			_Navs.NavigateTo("/customer");
		}

		protected async Task InsertCommittee()
		{
			if (formModel.Customer_Committees == null) formModel.Customer_Committees = new();

			formModel.Customer_Committees.Add(new()
			{
				Id = Guid.NewGuid()
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveCommittee(Guid ID)
		{
			var itemToRemove = formModel.Customer_Committees?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Customer_Committees?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task InsertShareholder()
		{
			if (formModel.Customer_Shareholders == null) formModel.Customer_Shareholders = new();

			formModel.Customer_Shareholders.Add(new()
			{
				Id = Guid.NewGuid()
			});

			await Task.Delay(1);
			StateHasChanged();
		}

		protected async Task RemoveShareholder(Guid ID)
		{
			var itemToRemove = formModel.Customer_Shareholders?.FirstOrDefault(r => r.Id == ID);
			if (itemToRemove != null)
				formModel.Customer_Shareholders?.Remove(itemToRemove);

			await Task.Delay(1);
			StateHasChanged();
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