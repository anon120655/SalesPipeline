using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Pages.Settings.PreApprove;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Customers;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils.Resources.Thailands;
using System.Linq;

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
		private bool IsView = true;
		private LookUpResource LookUp = new();
		private CustomerCustom formModel = new();
		private bool IsVerify = false;
		ModalConfirm modalConfirm = default!;
		ModalFailed modalFailed = default!;

		protected override async Task OnInitializedAsync()
		{
			isLoadingContent = true;
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.Customers) ?? new User_PermissionCustom();
			if (UserInfo.RoleCode == RoleCodes.BRANCH_REG_01)
			{
				_permission.IsView = false;
			}
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

			var tSIC = await _masterViewModel.GetTSIC(new allFilter() { status = StatusModel.Active, pagesize = 50 });
			if (tSIC != null && tSIC.Status)
			{
				LookUp.TSIC = tSIC.Data?.Items;
			}
			else
			{
				_errorMessage = tSIC?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "ContactChannel");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "BusinessSize");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "TSIC");
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnBusinessType", "#BusinessType");

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

			var loanType = await _masterViewModel.GetLoanType(new allFilter() { status = StatusModel.Active });
			if (loanType != null && loanType.Status)
			{
				LookUp.LoanType = loanType.Data?.Items;
			}
			else
			{
				_errorMessage = loanType?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			isLoadingContent = false;
			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Yield");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Chain");
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "LoanType");

			var branchs = await _masterViewModel.GetBranchs(new allFilter() { status = StatusModel.Active, pagesize = 2000 });
			if (branchs != null && branchs.Status)
			{
				LookUp.Branchs = branchs.Data?.Items;
			}
			else
			{
				_errorMessage = branchs?.errorMessage;
				_utilsViewModel.AlertWarning(_errorMessage);
			}

			//var iSICCode = await _masterViewModel.GetISICCode(new allFilter() { status = StatusModel.Active, pagesize = 2000 });
			//if (iSICCode != null && iSICCode.Status)
			//{
			//	LookUp.ISICCode = iSICCode.Data?.Items;
			//}
			//else
			//{
			//	_errorMessage = iSICCode?.errorMessage;
			//	_utilsViewModel.AlertWarning(_errorMessage);
			//}

			StateHasChanged();
			await _jsRuntimes.InvokeVoidAsync("BootSelectId", "Branch");
			//await _jsRuntimes.InvokeVoidAsync("BootSelectId", "ISICCode");

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
			await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnProvince", "#Province");

			await Task.Delay(10);
			await SetDataLookup();

		}

		protected async Task SetModel()
		{
			if (id != Guid.Empty)
			{
				var data = await _customerViewModel.GetById(id);
				if (data != null && data.Status && data.Data != null)
				{
					formModel = data.Data;
					if (formModel.Sales != null)
					{
						var sales = formModel.Sales.FirstOrDefault();
						if (sales != null)
						{
							formModel.Branch_RegionId = sales.Master_Branch_RegionId;

							if (UserInfo.RoleCode != null && !UserInfo.RoleCode.Contains(RoleCodes.ADMIN) && data.Data.ProvinceId.HasValue)
							{
								if (UserInfo.RoleCode == RoleCodes.CENTER)
								{
									//เช็คว่าถูกมอบหมายหรือไม่
									if (sales.AssCenterUserId != UserInfo.Id)
									{
										//ผจศ. เห็นเฉพาะพนักงาน RM ภายใต้พื้นที่การดูแล และงานที่ถูกมอบหมายมาจาก ธญ
										//if (sales.AssUserId.HasValue)
										//{
										//	//ดึงพื้นที่ดูแลพนักงาน RM 
										//	var areaRM = await _userViewModel.GetAreaByUserId(sales.AssUserId.Value);
										//	if (areaRM != null && areaRM.Data != null)
										//	{
										//		int index = 0;
										//		//เช็คว่าอยู่ในพื้นที่ดูแลของ ผจศ หรือป่าว
										//		foreach (var item in areaRM.Data)
										//		{
										//			if (index > 0)
										//			{
										//				if (IsView) break;
										//			}
										//			IsView = UserInfo.User_Areas?.Any(a => a.ProvinceId == item.ProvinceId) ?? false;
										//			index++;
										//		}
										//	}
										//}
									}
								}
							}
						}
					}
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

			//if (!formModel.DateContact.HasValue)
			//{
			//	formModel.DateContact = DateTime.Now.Date;
			//}

			formModel.EmployeeId = UserInfo.EmployeeId;
			formModel.EmployeeName = UserInfo.FullName;
			//formModel.BranchName = UserInfo.BranchName;
			//if (UserInfo.RoleCode != null)
			//{
			//	if (UserInfo.RoleCode == RoleCodes.SUPERADMIN || UserInfo.RoleCode.StartsWith(RoleCodes.LOAN))
			//	{
			//		formModel.BranchName = "สายงานธุรกิจสินเชื่อ";
			//	}
			//}
		}

		protected async Task SetDataLookup()
		{
			if (formModel.Master_BusinessTypeId.HasValue)
			{
				LookUp.ISICCode = new List<Master_ISICCodeCustom>();
				StateHasChanged();
				await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "ISICCode");

				var iSICCode = await _masterViewModel.GetISICCode(new allFilter()
				{
					status = StatusModel.Active,
					pagesize = 2000,
					businesstype = formModel.Master_BusinessTypeId.Value.ToString()
				});
				if (iSICCode != null && iSICCode.Status)
				{
					LookUp.ISICCode = new() { new() { Name = "--เลือก--" } };
					if (iSICCode.Data != null)
					{
						LookUp.ISICCode.AddRange(iSICCode.Data.Items);

						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "ISICCode", 100);
					}
				}
			}

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
		public async Task OnBusinessType(string _ID, string _Name)
		{
			LookUp.ISICCode = new List<Master_ISICCodeCustom>();
			formModel.Master_BusinessTypeId = null;
			formModel.Master_ISICCodeId = null;
			StateHasChanged();

			await _jsRuntimes.InvokeVoidAsync("BootSelectEmptyID", "ISICCode");

			if (_ID != null && Guid.TryParse(_ID, out Guid businessTypeId))
			{
				formModel.Master_BusinessTypeId = businessTypeId;

				var iSICCode = await _masterViewModel.GetISICCode(new allFilter()
				{
					status = StatusModel.Active,
					pagesize = 2000,
					businesstype = businessTypeId.ToString()
				});
				if (iSICCode != null && iSICCode.Status)
				{
					LookUp.ISICCode = new() { new() { Name = "--เลือก--" } };
					if (iSICCode.Data != null)
					{
						LookUp.ISICCode.AddRange(iSICCode.Data.Items);

						StateHasChanged();
						await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "ISICCode", 100);
					}
				}
				else
				{
					_errorMessage = iSICCode?.errorMessage;
					_utilsViewModel.AlertWarning(_errorMessage);
				}
			}
		}

		[JSInvokable]
		public async Task OnProvince(string _provinceID, string _provinceName)
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
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnAmphur", "#Amphur");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Amphur", 100);
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Tambol", 100);
				}
			}
		}

		[JSInvokable]
		public async Task OnAmphur(string _amphurID, string _amphurName)
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
					await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnTambol", "#Tambol");
					await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshID", "Tambol", 100);
				}

			}
		}

		[JSInvokable]
		public void OnTambol(string _tambolID, string _tambolName)
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

			if (String.IsNullOrEmpty(formModel.JuristicPersonRegNumber) || formModel.JuristicPersonRegNumber.Length < 10)
			{
				if (String.IsNullOrEmpty(formModel.JuristicPersonRegNumber))
				{
					_errorMessage = "ระบุเลขนิติบุคคล";
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
				else if (formModel.JuristicPersonRegNumber.Length < 10)
				{
					_errorMessage = "ระบุเลขนิติบุคคลไม่ถูกต้อง";
					await _jsRuntimes.InvokeVoidAsync("WarningAlert", _errorMessage);
				}
			}
			else
			{
				var data = await _customerViewModel.VerifyByNumber(formModel.JuristicPersonRegNumber, UserInfo.Id);
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

			formModel.StatusSaleId = StatusSaleModel.NotStatus;

			if (UserInfo.RoleCode != null)
			{
				if (UserInfo.RoleCode.StartsWith(RoleCodes.LOAN))
				{
					//Role สายงานธุรกิจสินเชื่อ --> รอมอบหมาย
					formModel.StatusSaleId = StatusSaleModel.WaitAssign;
				}
				else if (UserInfo.RoleCode.StartsWith(RoleCodes.BRANCH_REG))
				{
					//Role กิจการสาขาภาค --> รอมอบหมาย(ศูนย์สาขา)
					formModel.StatusSaleId = StatusSaleModel.WaitAssignCenter;
				}
				else if (UserInfo.RoleCode == RoleCodes.CENTER || UserInfo.RoleCode == RoleCodes.SUPERADMIN)
				{
					//Role ผู้จัดการศูนย์ --> รอมอบหมาย
					formModel.StatusSaleId = StatusSaleModel.WaitAssign;
				}
			}

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

		protected void Cancel()
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

		//[JSInvokable]
		//public async Task OnDepBranch(string _ids, string _name)
		//{
		//	formModel.Branch_RegionId = null;

		//	if (_ids != null)
		//	{
		//		if (Guid.TryParse(_ids, out Guid id))
		//		{
		//			formModel.Branch_RegionId = id;
		//		}
		//	}
		//	StateHasChanged();
		//	await Task.Delay(1);
		//}


	}
}