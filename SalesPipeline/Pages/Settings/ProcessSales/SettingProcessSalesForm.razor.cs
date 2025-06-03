using global::Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Shared.Modals;
using SalesPipeline.Utils;
using SalesPipeline.Utils.ConstTypeModel;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using SalesPipeline.Utils.Resources.ProcessSales;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Pages.Settings.ProcessSales
{
    public partial class SettingProcessSalesForm
    {
        [Parameter]
        public Guid id { get; set; }

        string? _errorMessage = null;
        private bool isLoading = false;
        private LookUpResource LookUp = new();
        private User_PermissionCustom _permission = new();
        private ProcessSaleCustom formModel = new();

        ModalConfirm modalConfirmDeleteSection = default!;

        protected override async Task OnInitializedAsync()
        {
            _permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.SetProcessSales) ?? new User_PermissionCustom();
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

                await Task.Delay(100);
                await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
                firstRender = false;
            }
        }

        protected async Task SetInitManual()
        {
            var masterLists = await _masterViewModel.MasterLists(new allFilter() { status = StatusModel.Active });
            if (masterLists != null && masterLists.Status)
            {
                LookUp.MasterList = masterLists.Data;
            }
            else
            {
                _errorMessage = masterLists?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }

            StateHasChanged();
            await Task.Delay(10);
        }

        protected async Task SetModel()
        {
            var data = await _processSaleViewModel.GetById(id);
            if (data != null && data.Status && data.Data != null)
            {
                formModel = data.Data;

                if (formModel.ProcessSale_Sections == null || formModel.ProcessSale_Sections.Count == 0)
                {
                    await InsertSection(0);
                }
            }
            else
            {
                _errorMessage = data?.errorMessage;
                _utilsViewModel.AlertWarning(_errorMessage);
            }
        }

        protected async Task OnItemTypes(ChangeEventArgs e, Guid sectionId, Guid saleItemId)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null)
            {
                var item = section.ProcessSale_Section_Items?.FirstOrDefault(r => r.Id == saleItemId);
                if (e.Value != null && item != null)
                {
                    item.ItemType = e.Value.ToString();
                    item.ProcessSale_Section_ItemOptions = new List<ProcessSale_Section_ItemOptionCustom>();

                    await InsertSectionItemOption(sectionId, saleItemId);

                    StateHasChanged();
                    await Task.Delay(1);
                    if (item.ItemType == FieldTypes.Multiplechoice || item.ItemType == FieldTypes.Dropdown || item.ItemType == FieldTypes.DropdownMaster)
                    {
                        await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
                    }
                }
            }
        }

        protected async Task InsertSection(int sequenceNo)
        {
            if (formModel.ProcessSale_Sections == null) formModel.ProcessSale_Sections = new();

            sequenceNo++;

            var calSequenceNo = formModel.ProcessSale_Sections.Where(x => x.Status == StatusModel.Active && x.SequenceNo >= sequenceNo).ToList();
            if (calSequenceNo.Count > 0)
            {
                foreach (var item in calSequenceNo)
                {
                    item.SequenceNo = item.SequenceNo + 1;
                }
            }

            Guid _idGen = Guid.NewGuid();
            var secName = $"Section {sequenceNo}";
            formModel.ProcessSale_Sections.Add(new()
            {
                Id = _idGen,
                Status = StatusModel.Active,
                ProcessSaleId = id,
                SequenceNo = sequenceNo,
                Name = secName,
            });

            await InsertSectionItem(_idGen, 0);

            formModel.ProcessSale_Sections = formModel.ProcessSale_Sections.OrderBy(x => x.SequenceNo).ToList();

            StateHasChanged();
            await Task.Delay(10);
            await _jsRuntimes.InvokeVoidAsync("BootSelectRefreshClass", "dropdownsection");
        }

        protected async Task RemoveSection(Guid sectionId)
        {
            var saleSection = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (saleSection != null)
            {
                saleSection.Status = StatusModel.Delete;
            }

            var calSequenceNo = formModel.ProcessSale_Sections?.Where(x => x.Status == StatusModel.Active).OrderBy(x => x.SequenceNo).ToList();
            if (calSequenceNo != null && calSequenceNo.Count > 0)
            {
                int No = 0;
                foreach (var item in calSequenceNo)
                {
                    No++;
                    item.SequenceNo = No;
                }
            }

            StateHasChanged();
            await Task.Delay(1);
        }

        protected async Task ConfirmDeleteSection(string? id, string? txt)
        {
            await modalConfirmDeleteSection.OnShowConfirm(id, $"คุณต้องการลบ Section <span class='text-primary'>{txt}</span>");
        }

        protected async Task DeleteSection(string id)
        {
            await modalConfirmDeleteSection.OnHideConfirm();

            await RemoveSection(Guid.Parse(id));
        }

        protected async Task InsertSectionItem(Guid sectionId, int sequenceNo)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null)
            {
                if (section.ProcessSale_Section_Items == null) section.ProcessSale_Section_Items = new();

                sequenceNo++;

                var calSequenceNo = section.ProcessSale_Section_Items.Where(x => x.Status == StatusModel.Active && x.SequenceNo >= sequenceNo).ToList();
                if (calSequenceNo.Count > 0)
                {
                    foreach (var item in calSequenceNo)
                    {
                        item.SequenceNo = item.SequenceNo + 1;
                    }
                }

                Guid _idGen = Guid.NewGuid();
                section.ProcessSale_Section_Items.Add(new()
                {
                    Id = _idGen,
                    Status = StatusModel.Active,
                    PSaleSectionId = sectionId,
                    SequenceNo = sequenceNo,
                    ItemType = FieldTypes.ShortAnswer
                });
                await InsertSectionItemOption(sectionId, _idGen);

                section.ProcessSale_Section_Items = section.ProcessSale_Section_Items.OrderBy(x => x.SequenceNo).ToList();

                StateHasChanged();
                await Task.Delay(1);
            }
        }

        protected async Task RemoveSectionItem(Guid sectionId, Guid saleItemId)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null)
            {
                var sectionitem = section.ProcessSale_Section_Items?.FirstOrDefault(r => r.Id == saleItemId);
                if (sectionitem != null)
                {
                    sectionitem.Status = StatusModel.Delete;

                    //SetSequenceItem();
                }

                StateHasChanged();
                await Task.Delay(1);
            }

        }

        protected async Task SectionShowAlwaysChange(ChangeEventArgs e, Guid sectionId)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null && e.Value != null)
            {
                if (bool.TryParse(e.Value.ToString(), out bool val))
                {
                    section.ShowAlways = val ? 1 : 0;
                }
                var ItemType = e.Value.ToString();


                StateHasChanged();
                await Task.Delay(1);
            }

        }

        protected async Task CopySectionItem(Guid sectionId, Guid saleItemId)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null)
            {
                var sectionitem = section.ProcessSale_Section_Items?.FirstOrDefault(r => r.Id == saleItemId);
                if (sectionitem != null)
                {
                    int sequenceNo = 0;
                    var sectionitems = section.ProcessSale_Section_Items?.Where(x => x.Status == StatusModel.Active).ToList();
                    if (sectionitems?.Count() > 0)
                    {
                        sequenceNo = sectionitems.Max(x => x.SequenceNo) + 1;
                    }

                    var itemOption = new List<ProcessSale_Section_ItemOptionCustom>();
                    var processSale_Section_ItemOptions = sectionitem.ProcessSale_Section_ItemOptions?.Where(x => x.Status == StatusModel.Active).ToList();
                    if (processSale_Section_ItemOptions?.Count > 0)
                    {
                        foreach (var item_option in processSale_Section_ItemOptions)
                        {
                            itemOption.Add(new()
                            {
                                Id = Guid.NewGuid(),
                                Status = StatusModel.Active,
                                PSaleSectionItemId = item_option.PSaleSectionItemId,
                                SequenceNo = item_option.SequenceNo,
                                OptionLabel = item_option.OptionLabel,
                                DefaultValue = item_option.DefaultValue,
                                ShowSectionId = item_option.ShowSectionId,
                                Master_ListId = item_option.Master_ListId,
                            });
                        }
                    }

                    Guid _idGen = Guid.NewGuid();
                    section.ProcessSale_Section_Items?.Add(new()
                    {
                        Id = _idGen,
                        Status = StatusModel.Active,
                        PSaleSectionId = sectionitem.PSaleSectionId,
                        SequenceNo = sequenceNo,
                        ItemLabel = sectionitem.ItemLabel,
                        ItemType = sectionitem.ItemType,
                        Required = sectionitem.Required,
                        ProcessSale_Section_ItemOptions = itemOption
                    });
                }

                StateHasChanged();
                await Task.Delay(1);
                await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
            }

        }

        protected async Task InsertSectionItemOption(Guid sectionId, Guid saleItemId)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null)
            {
                var sectionitem = section.ProcessSale_Section_Items?.FirstOrDefault(r => r.Id == saleItemId);
                if (sectionitem != null)
                {
                    if (sectionitem.ProcessSale_Section_ItemOptions == null) sectionitem.ProcessSale_Section_ItemOptions = new();

                    int sequenceNo = 1;

                    var itemOptions = sectionitem.ProcessSale_Section_ItemOptions.Where(x => x.Status == StatusModel.Active).ToList();

                    if (itemOptions.Count() > 0)
                    {
                        sequenceNo = itemOptions.Max(x => x.SequenceNo) + 1;
                    }

                    sectionitem.ProcessSale_Section_ItemOptions.Add(new()
                    {
                        Id = Guid.NewGuid(),
                        Status = StatusModel.Active,
                        PSaleSectionItemId = saleItemId,
                        SequenceNo = sequenceNo
                    });
                }
            }

            StateHasChanged();
            await Task.Delay(1);
            await _jsRuntimes.InvokeVoidAsync("selectPickerInitialize");
        }

        protected async Task RemoveSectionItemOption(Guid sectionId, Guid saleItemId, Guid saleItemOptionId)
        {
            var section = formModel.ProcessSale_Sections?.FirstOrDefault(r => r.Id == sectionId);
            if (section != null)
            {
                var sectionitem = section.ProcessSale_Section_Items?.FirstOrDefault(r => r.Id == saleItemId);
                if (sectionitem != null)
                {
                    if (sectionitem != null && sectionitem.ProcessSale_Section_ItemOptions?.Count > 0)
                    {
                        var itemOption = sectionitem.ProcessSale_Section_ItemOptions.FirstOrDefault(r => r.Id == saleItemOptionId);
                        if (itemOption != null)
                        {
                            itemOption.Status = StatusModel.Delete;

                            int sequenceNo = 1;
                            foreach (var item_option in sectionitem.ProcessSale_Section_ItemOptions.Where(x => x.Status == StatusModel.Active).OrderBy(x => x.SequenceNo))
                            {
                                item_option.SequenceNo = sequenceNo;
                                sequenceNo++;
                            }
                        }
                    }
                }
            }

            await Task.Delay(1);
            StateHasChanged();
        }

        private async Task Save()
        {
            _errorMessage = null;
            ShowLoading();

            formModel.CurrentUserId = UserInfo.Id;

            var response = await _processSaleViewModel.Update(formModel);

            if (response.Status)
            {
                await _jsRuntimes.InvokeVoidAsync("SuccessAlert");
                HideLoading();
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
            _Navs.NavigateTo("/setting/processsales");
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