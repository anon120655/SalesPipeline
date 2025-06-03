using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.Loans;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using SalesPipeline.Utils.ConstTypeModel;

namespace SalesPipeline.Pages.Settings.PreApprove.Loans
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

        Guid _payType1 = PrePayTypeIdModel.PayType1; //อัตราดอกเบี้ยคงที่
        Guid _payType2 = PrePayTypeIdModel.PayType2; //อัตราดอกเบี้ยคงที่ตามรอบเวลา
        Guid _rateTypeSpecial = Guid.Parse("11e23023-18cd-11ef-93aa-30e37aef72fb"); //Special - ระบุ

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetPreApprove) ?? new User_PermissionCustom();
            StateHasChanged();
            await Task.Delay(1);

            filter.pagesize = 100;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _jsRuntimes.InvokeVoidAsync("BootSelectClass", "selectInit");

                await SetModel();
                StateHasChanged();

                if (!id.HasValue)
                {
                    await SetLookUp();
                }

                firstRender = false;
            }
        }

        protected async Task SetLookUp()
        {
            var dataRateType = await _masterViewModel.GetPre_PayType(filter);
            if (dataRateType != null && dataRateType.Status)
            {
                await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "Interest_PayType");
                LookUp.Pre_Interest_PayType = new() { new() { Name = "เลือก" } };
                if (dataRateType.Data?.Items != null)
                {
                    LookUp.Pre_Interest_PayType.AddRange(dataRateType.Data.Items);
                }
                StateHasChanged();
                await Task.Delay(10);
                await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnInterest_PayType", "#Interest_PayType");
            }
            else
            {
                _errorMessage = dataRateType?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }


            var Applicant_LoanData = new List<Master_Pre_Applicant_LoanCustom>();
            var BusinessTypeData = new List<Master_Pre_BusinessTypeCustom>();

            var dataApplicant = await _masterViewModel.GetPre_App_Loan(filter);
            if (dataApplicant != null && dataApplicant.Status)
            {
                if (dataApplicant.Data != null)
                {
                    Applicant_LoanData.AddRange(dataApplicant.Data.Items);
                }
            }
            else
            {
                _errorMessage = dataApplicant?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            var dataBusType = await _masterViewModel.GetPre_BusType(filter);
            if (dataBusType != null && dataBusType.Status)
            {
                if (dataBusType.Data != null)
                {
                    BusinessTypeData.AddRange(dataBusType.Data.Items);
                }
            }
            else
            {
                _errorMessage = dataBusType?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            List<SelectModel>? LoanModel = new();
            foreach (var item in Applicant_LoanData)
            {
                bool isSelected = formModel.Loan_AppLoans?.Any(x => x.Master_Pre_Applicant_LoanId == item.Id) ?? false;

                LoanModel.Add(new()
                {
                    ID = item.Id.ToString(),
                    Name = item.Name,
                    IsSelected = isSelected
                });
            }

            List<SelectModel>? BusTypeModel = new();
            foreach (var item in BusinessTypeData)
            {
                bool isSelected = formModel.Loan_BusTypes?.Any(x => x.Master_Pre_BusinessTypeId == item.Id) ?? false;

                BusTypeModel.Add(new()
                {
                    ID = item.Id.ToString(),
                    Name = item.Name,
                    IsSelected = isSelected
                });
            }

            LookUp.Pre_Applicant_LoanModel = LoanModel;
            LookUp.Pre_BusinessTypeModel = BusTypeModel;

            StateHasChanged();
            await Task.Delay(10);
        }

        protected async Task SetModel()
        {
            if (id.HasValue)
            {
                var data = await _loanViewModel.GetById(id.Value);
                if (data != null && data.Status && data.Data != null)
                {
                    formModel = data.Data;

                    if (formModel.Master_Pre_Interest_PayTypeId.HasValue)
                    {
                        await SetLookUp();
                        await SetLookUpPeriod(formModel.Master_Pre_Interest_PayTypeId.Value);
                        await SetLookUpRateType();
                    }
                }
                else
                {
                    _errorMessage = data?.errorMessage;
                    _utilsViewModel.AlertWarning(_errorMessage);
                }
            }
        }

        //ประเภทการชำระดอกเบี้ย
        [JSInvokable]
        public async Task OnInterest_PayType(string _ids, string _name)
        {
            formModel.Master_Pre_Interest_PayTypeId = null;
            formModel.Loan_Periods = new();
            LookUp.Periods = new();
            formModel.RiskPremiumYear = null;
            StateHasChanged();
            await Task.Delay(1);

            if (_ids != null)
            {
                if (Guid.TryParse(_ids, out Guid _payTypeId))
                {
                    formModel.RiskPremiumYear = 3;
                    formModel.PeriodNumber = _payTypeId == _payType1 ? 1 : _payTypeId == _payType2 ? 2 : null;

                    await SetLookUpPeriod(_payTypeId);

                    formModel.Master_Pre_Interest_PayTypeId = _payTypeId;

                    if (_payTypeId == _payType1)
                    {
                        await OnInterest_Periods("1", string.Empty);
                    }
                    else if (_payTypeId == _payType2)
                    {
                        await OnInterest_Periods("2", string.Empty);
                    }
                }
            }
        }

        //จำนวนช่วงเวลา
        [JSInvokable]
        public async Task OnInterest_Periods(string _ids, string _name)
        {
            LookUps = new();
            formModel.PeriodNumber = null;
            formModel.Loan_Periods = new();
            StateHasChanged();
            await Task.Delay(1);

            if (_ids != null)
            {
                if (int.TryParse(_ids, out int _length))
                {
                    formModel.PeriodNumber = _length;

                    for (int i = 1; i <= _length; i++)
                    {
                        formModel.Loan_Periods.Add(new() { PeriodNo = i });
                    }
                }

                await SetLookUpRateType();
            }
        }

        //ประเภทอัตราดอกเบี้ย
        [JSInvokable]
        public async Task OnInterest_RateType(string _ids, string _name, string dataVal)
        {
            if (_ids != null && dataVal != null && Guid.TryParse(_ids, out Guid rateTypeId))
            {
                List<string> stringList = dataVal.Split('@').ToList();
                if (formModel.Loan_Periods != null && stringList.Count == 2)
                {
                    int periodNo = int.Parse(stringList[0]);
                    decimal? Rate = null;
                    if (decimal.TryParse(stringList[1], out decimal _rate))
                    {
                        Rate = _rate;
                    }

                    var period = formModel.Loan_Periods.FirstOrDefault(x => x.PeriodNo == periodNo);
                    if (period != null)
                    {
                        period.Master_Pre_Interest_RateTypeId = null;
                        period.RateValue = null;
                        period.SpecialType = null;
                        period.SpecialRate = null;

                        if (rateTypeId != Guid.Empty)
                        {
                            period.Master_Pre_Interest_RateTypeId = rateTypeId;
                            period.RateValue = Rate;
                            period.RateValueOriginal = Rate;
                        }
                    }
                }
            }
            StateHasChanged();
            await Task.Delay(1);
        }

        public async Task SetLookUpPeriod(Guid _idpaytype)
        {
            LookUp.Periods = new();
            StateHasChanged();
            await Task.Delay(1);

            await _jsRuntimes.InvokeVoidAsync("BootSelectDestroy", "Interest_Periods");

            int startPeriod = 0;
            int lengthPeriod = 0;
            if (_idpaytype == _payType1)
            {
                startPeriod = 1;
                lengthPeriod = 1;
            }
            else if (_idpaytype == _payType2)
            {
                startPeriod = 2;
                lengthPeriod = 10;
            }

            LookUp.Periods.Add(new SelectModel() { ID = "", Name = "เลือก" });
            if (startPeriod > 0)
            {
                for (int i = startPeriod; i <= lengthPeriod; i++)
                {
                    LookUp.Periods.Add(new() { ID = i.ToString(), Name = i.ToString() });
                }
            }

            StateHasChanged();
            await Task.Delay(1);
            await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnInterest_Periods", "#Interest_Periods");
        }

        public async Task SetLookUpRateType()
        {
            if (formModel.Loan_Periods?.Count > 0)
            {
                await _jsRuntimes.InvokeVoidAsync("BootSelectDestroyClass", "interest_RateTypes");

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

                List<Master_Pre_Interest_RateTypeCustom>? rateType = new();
                foreach (var period in formModel.Loan_Periods)
                {
                    rateType = new() { new() { OptionValue = $"{period.PeriodNo}@{0}", Code = "เลือก" } };
                    foreach (var item in Interest_RateTypeData)
                    {
                        rateType.Add(new()
                        {
                            Id = item.Id,
                            OptionValue = $"{period.PeriodNo}@{item.Rate}",
                            Code = item.Code,
                            Name = item.Name
                        });
                    }

                    LookUps.Add(new LookUpResource()
                    {
                        Pre_Interest_RateType = rateType
                    });
                }
                StateHasChanged();
                await Task.Delay(10);

                foreach (var period in formModel.Loan_Periods)
                {
                    await _jsRuntimes.InvokeVoidAsync("BootSelectClass", $"select_{period.PeriodNo}");
                    await _jsRuntimes.InvokeVoidAsync("InitSelectPicker", DotNetObjectReference.Create(this), "OnInterest_RateType", $"#Interest_RateType_{period.PeriodNo}", "optionvalue");
                }

            }
        }

        public async Task OnSpecial(Loan_PeriodCustom period, object? value)
        {
            period.SpecialType = null;

            if (int.TryParse(value?.ToString(), out int specialType))
            {
                period.SpecialType = specialType;
            }

            if (period.SpecialRate.HasValue)
            {
                await OnSpecialRate(period, period.SpecialRate);
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
                period.SpecialRate = val;

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
            _Navs.NavigateTo("/setting/pre/loan");
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

        protected async Task CallbackItemsSelected(DropdownCheckboxMain model)
        {
            if (model != null && formModel.Loan_Periods != null)
            {
                if (model.Type == 1)
                {
                    formModel.Loan_AppLoans = new();
                    foreach (var item in model.SelectedItems)
                    {
                        if (Guid.TryParse(item.ID, out Guid _Id))
                        {
                            formModel.Loan_AppLoans.Add(new()
                            {
                                Master_Pre_Applicant_LoanId = _Id,
                                Master_Pre_Applicant_LoanName = item.Name
                            });
                        }
                    }
                }
                else if (model.Type == 2)
                {
                    formModel.Loan_BusTypes = new();
                    foreach (var item in model.SelectedItems)
                    {
                        if (Guid.TryParse(item.ID, out Guid _Id))
                        {
                            formModel.Loan_BusTypes.Add(new()
                            {
                                Master_Pre_BusinessTypeId = _Id,
                                Master_Pre_BusinessTypeName = item.Name
                            });
                        }
                    }
                }
            }

            await Task.Delay(1);
        }

    }
}