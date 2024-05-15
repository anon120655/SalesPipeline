using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using System.Net.Http;
using SalesPipeline.Utils.Resources.Thailands;
using SalesPipeline.Utils.Resources.Assignments;

namespace SalesPipeline.ViewModels
{
	public class MasterViewModel
	{
		private readonly IHttpClientCustom _httpClient;
		private readonly AppSettings _appSet;
		private readonly AuthorizeViewModel _authorizeViewModel;

		public MasterViewModel(IHttpClientCustom httpClient, IOptions<AppSettings> appset, AuthorizeViewModel authorizeViewModel)
		{
			_httpClient = httpClient;
			_appSet = appset.Value;
			_authorizeViewModel = authorizeViewModel;
		}

		public async Task<ResultModel<List<Master_ListCustom>>> MasterLists(allFilter parameters)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/MasterLists?{parameters.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<Master_ListCustom>>(content);

				return new ResultModel<List<Master_ListCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Master_ListCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_DepartmentCustom>>>> GetDepartments(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDepartments?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_DepartmentCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_DepartmentCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_DepartmentCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Master_PositionCustom>>> Positions(allFilter parameters)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/Positions?{parameters.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<Master_PositionCustom>>(content);

				return new ResultModel<List<Master_PositionCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Master_PositionCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Assignment_CenterBranchCustom>>>> GetListCenter(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/AssignmentCenter/GetListCenter", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Assignment_CenterBranchCustom>>>(content);

				return new ResultModel<PaginationView<List<Assignment_CenterBranchCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Assignment_CenterBranchCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Assignment_RMCustom>>>> GetListRM(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/AssignmentRM/GetListRM", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Assignment_RMCustom>>>(content);

				return new ResultModel<PaginationView<List<Assignment_RMCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Assignment_RMCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<MenuItemCustom>>> MenuItem(allFilter parameters)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/MenuItem?{parameters.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<MenuItemCustom>>(content);

				return new ResultModel<List<MenuItemCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<MenuItemCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_BusinessSizeCustom>>>> GetBusinessSize(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetBusinessSize?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_BusinessSizeCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_BusinessSizeCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_BusinessSizeCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_BusinessTypeCustom>>>> GetBusinessType(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetBusinessType?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_BusinessTypeCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_BusinessTypeCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_BusinessTypeCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_ContactChannelCustom>>>> GetContactChannel(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetContactChannel?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_ContactChannelCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_ContactChannelCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_ContactChannelCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_ISICCodeCustom>>>> GetISICCode(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetISICCode?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_ISICCodeCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_ISICCodeCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_ISICCodeCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_TSICCustom>>>> GetTSIC(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetTSIC?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_TSICCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_TSICCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_TSICCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_StatusSaleCustom>>>> GetStatusSale(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetStatusSale?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_StatusSaleCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_StatusSaleCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_StatusSaleCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<InfoProvinceCustom>>> GetProvince(Guid? department_BranchId = null)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetProvince?department_BranchId={department_BranchId}");
				var dataMap = JsonConvert.DeserializeObject<List<InfoProvinceCustom>>(content);

				return new ResultModel<List<InfoProvinceCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<InfoProvinceCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<InfoAmphurCustom>>> GetAmphur(int provinceID)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetAmphur?provinceID={provinceID}");
				var dataMap = JsonConvert.DeserializeObject<List<InfoAmphurCustom>>(content);

				return new ResultModel<List<InfoAmphurCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<InfoAmphurCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<InfoTambolCustom>>> GetTambol(int provinceID, int amphurID)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetTambol?provinceID={provinceID}&amphurID={amphurID}");
				var dataMap = JsonConvert.DeserializeObject<List<InfoTambolCustom>>(content);

				return new ResultModel<List<InfoTambolCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<InfoTambolCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<InfoBranchCustom>>> GetBranch(int provinceID)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetBranch?provinceID={provinceID}");
				var dataMap = JsonConvert.DeserializeObject<List<InfoBranchCustom>>(content);

				return new ResultModel<List<InfoBranchCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<InfoBranchCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<InfoBranchCustom>>> GetBranchByDepBranchId(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/GetBranchByDepBranchId", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<List<InfoBranchCustom>>(content);
				return new ResultModel<List<InfoBranchCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<InfoBranchCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Master_YearCustom>>> GetYear(allFilter model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/GetYear", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<List<Master_YearCustom>>(content);
				return new ResultModel<List<Master_YearCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Master_YearCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//ฝ่ายกิจการสาขา
		public async Task<ResultModel<Master_Branch_RegionCustom>> CreateDepBranch(Master_Branch_RegionCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateDepBranch", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Branch_RegionCustom>(content);
				return new ResultModel<Master_Branch_RegionCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Branch_RegionCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_Branch_RegionCustom>> UpdateDepBranch(Master_Branch_RegionCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateDepBranch", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Branch_RegionCustom>(content);
				return new ResultModel<Master_Branch_RegionCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Branch_RegionCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteDepBranchById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteDepBranchById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusDepBranchById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusDepBranchById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_Branch_RegionCustom>?> GetDepBranchById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDepBranchById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_Branch_RegionCustom>(content);
				return new ResultModel<Master_Branch_RegionCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Branch_RegionCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_Branch_RegionCustom>>>> GetDepBranchs(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDepBranchs?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_Branch_RegionCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_Branch_RegionCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_Branch_RegionCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//ประเภทสินเชื่อ
		public async Task<ResultModel<Master_LoanTypeCustom>> CreateLoanType(Master_LoanTypeCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateLoanType", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_LoanTypeCustom>(content);
				return new ResultModel<Master_LoanTypeCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_LoanTypeCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_LoanTypeCustom>> UpdateLoanType(Master_LoanTypeCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateLoanType", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_LoanTypeCustom>(content);
				return new ResultModel<Master_LoanTypeCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_LoanTypeCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteLoanTypeById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteLoanTypeById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusLoanTypeById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusLoanTypeById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_LoanTypeCustom>?> GetLoanTypeById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetLoanTypeById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_LoanTypeCustom>(content);
				return new ResultModel<Master_LoanTypeCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_LoanTypeCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_LoanTypeCustom>>>> GetLoanType(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetLoanType?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_LoanTypeCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_LoanTypeCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_LoanTypeCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//เหตุผลการส่งคืน
		public async Task<ResultModel<Master_ReasonReturnCustom>> CreateReasonReturn(Master_ReasonReturnCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateReasonReturn", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_ReasonReturnCustom>(content);
				return new ResultModel<Master_ReasonReturnCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_ReasonReturnCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_ReasonReturnCustom>> UpdateReasonReturn(Master_ReasonReturnCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateReasonReturn", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_ReasonReturnCustom>(content);
				return new ResultModel<Master_ReasonReturnCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_ReasonReturnCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteReasonReturnById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteReasonReturnById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusReasonReturnById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusReasonReturnById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_ReasonReturnCustom>?> GetReasonReturnById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetReasonReturnById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_ReasonReturnCustom>(content);
				return new ResultModel<Master_ReasonReturnCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_ReasonReturnCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_ReasonReturnCustom>>>> GetReasonReturns(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetReasonReturns?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_ReasonReturnCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_ReasonReturnCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_ReasonReturnCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//SLA การดำเนินการ
		public async Task<ResultModel<Master_SLAOperationCustom>> CreateSLAOpe(Master_SLAOperationCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateSLAOpe", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_SLAOperationCustom>(content);
				return new ResultModel<Master_SLAOperationCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_SLAOperationCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_SLAOperationCustom>> UpdateSLAOpe(Master_SLAOperationCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateSLAOpe", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_SLAOperationCustom>(content);
				return new ResultModel<Master_SLAOperationCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_SLAOperationCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteSLAOpeById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteSLAOpeById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusSLAOpeById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusSLAOpeById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_SLAOperationCustom>?> GetSLAOpeById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetSLAOpeById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_SLAOperationCustom>(content);
				return new ResultModel<Master_SLAOperationCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_SLAOperationCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_SLAOperationCustom>>>> GetSLAOperations(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetSLAOperations?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_SLAOperationCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_SLAOperationCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_SLAOperationCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//ผลผลิต
		public async Task<ResultModel<Master_YieldCustom>> CreateYield(Master_YieldCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateYield", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_YieldCustom>(content);
				return new ResultModel<Master_YieldCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_YieldCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_YieldCustom>> UpdateYield(Master_YieldCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateYield", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_YieldCustom>(content);
				return new ResultModel<Master_YieldCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_YieldCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteYieldById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteYieldById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusYieldById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusYieldById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_YieldCustom>?> GetYieldById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetYieldById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_YieldCustom>(content);
				return new ResultModel<Master_YieldCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_YieldCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_YieldCustom>>>> GetYields(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetYields?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_YieldCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_YieldCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_YieldCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//ห่วงโซ่
		public async Task<ResultModel<Master_ChainCustom>> CreateChain(Master_ChainCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateChain", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_ChainCustom>(content);
				return new ResultModel<Master_ChainCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_ChainCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_ChainCustom>> UpdateChain(Master_ChainCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateChain", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_ChainCustom>(content);
				return new ResultModel<Master_ChainCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_ChainCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteChainById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteChainById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusChainById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusChainById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_ChainCustom>?> GetChainById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetChainById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_ChainCustom>(content);
				return new ResultModel<Master_ChainCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_ChainCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_ChainCustom>>>> GetChains(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetChains?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_ChainCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_ChainCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_ChainCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//Pre Approve
		//ประเภทอัตราดอกเบี้ย
		public async Task<ResultModel<bool>> UpdatePre_RateType(List<Master_Pre_Interest_RateTypeCustom>? model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdatePre_RateType", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_Pre_Interest_RateTypeCustom>>>> GetPre_RateType(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetPre_RateType?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_Pre_Interest_RateTypeCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_Pre_Interest_RateTypeCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_Pre_Interest_RateTypeCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//ประเภทผู้ขอสินเชื่อ
		public async Task<ResultModel<Master_Pre_Loan_ApplicantCustom>> CreatePre_Loan_App(Master_Pre_Loan_ApplicantCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreatePre_Loan_App", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Pre_Loan_ApplicantCustom>(content);
				return new ResultModel<Master_Pre_Loan_ApplicantCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Pre_Loan_ApplicantCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_Pre_Loan_ApplicantCustom>> UpdatePre_Loan_App(Master_Pre_Loan_ApplicantCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdatePre_Loan_App", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Pre_Loan_ApplicantCustom>(content);
				return new ResultModel<Master_Pre_Loan_ApplicantCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Pre_Loan_ApplicantCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeletePre_Loan_AppById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeletePre_Loan_AppById?{model.SetParameter(true)}");
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> UpdateStatusPre_Loan_AppById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusPre_Loan_AppById", dataJson, token: tokenJwt);
				return new ResultModel<bool>();
			}
			catch (Exception ex)
			{
				return new ResultModel<bool>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_Pre_Loan_ApplicantCustom>?> GetPre_Loan_AppById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetPre_Loan_AppById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_Pre_Loan_ApplicantCustom>(content);
				return new ResultModel<Master_Pre_Loan_ApplicantCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Pre_Loan_ApplicantCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_Pre_Loan_ApplicantCustom>>>> GetPre_Loan_App(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetPre_Loan_App?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_Pre_Loan_ApplicantCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_Pre_Loan_ApplicantCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_Pre_Loan_ApplicantCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}


	}
}
