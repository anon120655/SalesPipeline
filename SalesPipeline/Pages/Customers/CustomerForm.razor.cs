using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
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

		public Guid? _internalid { get; set; }
		string? _errorMessage = null;
		private bool isLoading = false;
		private bool isLoadingContent = false;
		private User_PermissionCustom _permission = new();
		private LookUpResource LookUp = new();
		private CustomerCustom formModel = new();
		private bool IsVerify = false;
		ModalConfirm modalConfirm = default!;
		ModalFailed modalFailed = default!;

		protected override async Task OnInitializedAsync()
		{
			isLoadingContent = true;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Customers) ?? new User_PermissionCustom();
			StateHasChanged();

			await SetModel();
		}

		protected override async Task OnParametersSetAsync()
		{
			await Task.Delay(1);
			if (id != Guid.Empty && _internalid != id)
			{
				_internalid = id;
			}
		}
		
		protected async override Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await BootSelectInit();

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

			isLoadingContent = false;
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
			await Task.Delay(10);
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "ProvinceChange", "#Province");

			await Task.Delay(10);
			await SetAddress();
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

		protected async Task SetAddress()
		{
			if (formModel.ProvinceId.HasValue)
			{
				LookUp.Amphurs = new List<InfoAmphurCustom>();
				StateHasChanged();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "Amphur");

				var amphur = await _masterViewModel.GetAmphur(formModel.ProvinceId.Value);
				if (amphur != null && amphur.Data != null)
				{
					LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
					LookUp.Amphurs?.AddRange(amphur.Data);
					StateHasChanged();
					await Task.Delay(10);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", $"#Amphur");

					if (formModel.AmphurId.HasValue)
					{
						LookUp.Tambols = new List<InfoTambolCustom>();
						StateHasChanged();
						await Task.Delay(10);
						await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", $"Tambol");

						var tambol = await _masterViewModel.GetTambol(formModel.ProvinceId.Value, formModel.AmphurId.Value);
						if (tambol != null && tambol.Data != null)
						{
							LookUp.Tambols = new List<InfoTambolCustom> { new InfoTambolCustom() { TambolID = 0, TambolName = "--เลือก--" } };
							LookUp.Tambols?.AddRange(tambol.Data);
							StateHasChanged();
							await Task.Delay(10);
							await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "TambolChange", $"#Tambol");
						}
					}
				}
			}
		}

		[JSInvokable]
		public async Task ProvinceChange(string _provinceID, string _provinceName)
		{
			LookUp.Amphurs = new List<InfoAmphurCustom>();
			LookUp.Tambols = new List<InfoTambolCustom>();
			formModel.ZipCode = null;
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Amphur");
			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Tambol");

			if (_provinceID != null && int.TryParse(_provinceID, out int provinceID))
			{
				formModel.ProvinceId = provinceID;

				var amphurs = await _masterViewModel.GetAmphur(provinceID);
				if (amphurs != null && amphurs.Data?.Count > 0)
				{
					LookUp.Amphurs = new List<InfoAmphurCustom>() { new InfoAmphurCustom() { AmphurID = 0, AmphurName = "--เลือก--" } };
					LookUp.Amphurs.AddRange(amphurs.Data);

					StateHasChanged();
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "AmphurChange", "#Amphur");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Tambol", 100);
				}
			}
		}

		[JSInvokable]
		public async Task AmphurChange(string _amphurID, string _amphurName)
		{
			if (_amphurID != null && int.TryParse(_amphurID, out int amphurID))
			{
				LookUp.Tambols = new List<InfoTambolCustom>();
				formModel.ZipCode = null;
				StateHasChanged();
				await Task.Delay(10);
				await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "Tambol");

				formModel.AmphurId = amphurID;

				var tambols = await _masterViewModel.GetTambol(formModel.ProvinceId ?? 0, amphurID);
				if (tambols != null && tambols.Data != null && tambols.Data.Count > 0)
				{
					LookUp.Tambols = new List<InfoTambolCustom> { new InfoTambolCustom() { TambolID = 0, TambolName = "--เลือก--" } };
					LookUp.Tambols.AddRange(tambols.Data);

					StateHasChanged();
					await Task.Delay(10);
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "TambolChange", "#Tambol");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Tambol", 100);
				}

			}
		}

		[JSInvokable]
		public void TambolChange(string _tambolID, string _tambolName)
		{
			if (_tambolID != null && int.TryParse(_tambolID, out int tambolID))
			{
				formModel.TambolId = tambolID;

				if (LookUp.Tambols?.Count > 0)
				{
					var tambol = LookUp.Tambols.FirstOrDefault(x => x.TambolID == tambolID);
					if (tambol != null)
					{
						formModel.ZipCode = tambol.ZipCode;
						StateHasChanged();
					}
				}

			}
		}

		protected async Task OnVerify()
		{
			_errorMessage = null;

			if (String.IsNullOrEmpty(formModel.JuristicPersonRegNumber) || formModel.JuristicPersonRegNumber.Length != 13)
			{
				if (String.IsNullOrEmpty(formModel.JuristicPersonRegNumber))
				{
					_errorMessage = "ระบุเลขนิติบุคคล";
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
				else if (formModel.JuristicPersonRegNumber.Length != 13)
				{
					_errorMessage = "ระบุเลขนิติบุคคลไม่ถูกต้อง";
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}
			else
			{
				var data = await _customerViewModel.VerifyByNumber(formModel.JuristicPersonRegNumber);
				if (data != null && data.Status && data.Data != null)
				{
					if (data.Data.Code == "pass")
					{
						IsVerify = true;
						StateHasChanged();
						await BootSelectInit();
					}
					else if (data.Data.Code == "duplicate")
					{
						await ConfirmProceedModal(data.Data.ID, data.Data.Message, "<i class=\"fa-solid fa-book fs_5r text-primary\"></i>");
					}
					else if (data.Data.Code == "proceed")
					{
						await FailedModal(data.Data.ID, data.Data.Message);
					}
					else
					{
						_errorMessage = data.Data.Message;
						_utilsViewModel.AlertWarning(_errorMessage);
					}
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

			ResultModel<CustomerCustom> response;

			formModel.CurrentUserId = UserInfo.Id;

			//******** รอเช็ค *********			
			//Role กิจการสาขาภาค
			//if (UserInfo.RoleId == RoleCodes.BRANCH)
			//{
			//	formModel.StatusSaleId = StatusSaleModel.WaitAssignCenter;
			//}
			//Role ผู้จัดการศูนย์
			formModel.StatusSaleId = StatusSaleModel.WaitAssign;

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
				Id = Guid.NewGuid(),
				Status = StatusModel.Active
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
				Id = Guid.NewGuid(),
				Status = StatusModel.Active
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

		protected async Task FailedModal(string? _id, string? txt)
		{
			await modalFailed.OnShow(_id, $"{txt}");
		}

		protected async Task ConfirmProceedModal(string? _id, string? txt, string? icon = null)
		{
			await modalConfirm.OnShowConfirm(_id, $"{txt}", icon);
		}

		protected async Task ConfirmProceed(string _id)
		{
			await modalConfirm.OnHideConfirm();
			IsVerify = true;
			id = Guid.Parse(_id);

			await _jsRuntimes.InvokeVoidAsync("ChangeUrl", $"/customer/update/{id}");
			await SetModel();
			await BootSelectInit();
		}

		protected async Task BootSelectInit()
		{
			await Task.Delay(10);
			await SetInitManual();
			await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");
		}


	}
}