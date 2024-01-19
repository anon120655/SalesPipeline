using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SalesPipeline.Helpers;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using System.Net.Http;

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

		public async Task<ResultModel<List<Master_RegionCustom>>> Regions(allFilter parameters)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/Regions?{parameters.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<Master_RegionCustom>>(content);

				return new ResultModel<List<Master_RegionCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Master_RegionCustom>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<List<Master_BranchCustom>>> Branchs(allFilter parameters)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/Branchs?{parameters.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<List<Master_BranchCustom>>(content);

				return new ResultModel<List<Master_BranchCustom>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<List<Master_BranchCustom>>
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

		//ฝ่ายกิจการสาขา
		public async Task<ResultModel<Master_Division_BranchCustom>> CreateDivBranch(Master_Division_BranchCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateDivBranch", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Division_BranchCustom>(content);
				return new ResultModel<Master_Division_BranchCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Division_BranchCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_Division_BranchCustom>> UpdateDivBranch(Master_Division_BranchCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateDivBranch", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Division_BranchCustom>(content);
				return new ResultModel<Master_Division_BranchCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Division_BranchCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteDivBranchById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteDivBranchById?{model.SetParameter(true)}");
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

		public async Task<ResultModel<bool>?> UpdateStatusDivBranchById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusDivBranchById", dataJson, token: tokenJwt);
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

		public async Task<ResultModel<Master_Division_BranchCustom>?> GetDivBranchById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDivBranchById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_Division_BranchCustom>(content);
				return new ResultModel<Master_Division_BranchCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Division_BranchCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_Division_BranchCustom>>>> GetDivBranchs(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDivBranchs?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_Division_BranchCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_Division_BranchCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_Division_BranchCustom>>>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		//ฝ่ายธุรกิจสินเชื่อ
		public async Task<ResultModel<Master_Division_LoanCustom>> CreateDivLoans(Master_Division_LoanCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PostAsync($"/v1/Master/CreateDivLoans", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Division_LoanCustom>(content);
				return new ResultModel<Master_Division_LoanCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Division_LoanCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<Master_Division_LoanCustom>> UpdateDivLoans(Master_Division_LoanCustom model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				var content = await _httpClient.PutAsync($"/v1/Master/UpdateDivLoans", dataJson, token: tokenJwt);
				var dataMap = JsonConvert.DeserializeObject<Master_Division_LoanCustom>(content);
				return new ResultModel<Master_Division_LoanCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Division_LoanCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<bool>?> DeleteDivLoansById(UpdateModel model)
		{
			try
			{
				await _httpClient.DeleteAsync($"/v1/Master/DeleteDivLoansById?{model.SetParameter(true)}");
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

		public async Task<ResultModel<bool>?> UpdateStatusDivLoansById(UpdateModel model)
		{
			try
			{
				string tokenJwt = await _authorizeViewModel.GetAccessToken();
				string dataJson = JsonConvert.SerializeObject(model);
				await _httpClient.PutAsync($"/v1/Master/UpdateStatusDivLoansById", dataJson, token: tokenJwt);
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

		public async Task<ResultModel<Master_Division_LoanCustom>?> GetDivLoansById(Guid id)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDivLoansById?id={id}");
				var dataMap = JsonConvert.DeserializeObject<Master_Division_LoanCustom>(content);
				return new ResultModel<Master_Division_LoanCustom>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<Master_Division_LoanCustom>
				{
					Status = false,
					errorMessage = GeneralUtils.GetExMessage(ex)
				};
			}
		}

		public async Task<ResultModel<PaginationView<List<Master_Division_LoanCustom>>>> GetDivLoans(allFilter model)
		{
			try
			{
				var content = await _httpClient.GetAsync($"/v1/Master/GetDivLoans?{model.SetParameter(true)}");
				var dataMap = JsonConvert.DeserializeObject<PaginationView<List<Master_Division_LoanCustom>>>(content);

				return new ResultModel<PaginationView<List<Master_Division_LoanCustom>>>()
				{
					Data = dataMap
				};
			}
			catch (Exception ex)
			{
				return new ResultModel<PaginationView<List<Master_Division_LoanCustom>>>
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

	}
}
