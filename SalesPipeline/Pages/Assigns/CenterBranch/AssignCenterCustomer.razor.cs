using Hangfire.MemoryStorage.Database;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Assignments;
using SalesPipeline.Utils.Resources.Authorizes.Users;
using System.Text.Json;

namespace SalesPipeline.Pages.Assigns.CenterBranch
{
	public partial class AssignCenterCustomer
	{
		string? _errorMessage = null;
		private User_PermissionCustom _permission = new();
		private List<Assignment_CenterCustom>? Items;

		protected override async Task OnInitializedAsync()
		{
			_permission = UserInfo.User_Permissions.FirstOrDefault(x => x.MenuNumber == MenuNumbers.AssignLoan) ?? new User_PermissionCustom();
			StateHasChanged();
			await Task.Delay(1);

			var jsonData = await _jsRuntimes.InvokeAsync<string>("localStorage.getItem", $"AssignCenterData_{UserInfo.Id}");
			if (jsonData != null)
			{
				Items = JsonSerializer.Deserialize<List<Assignment_CenterCustom>>(jsonData);
				// ลบข้อมูลจาก Local Storage หลังจากดึงมาใช้แล้ว
				await _jsRuntimes.InvokeVoidAsync("localStorage.removeItem", $"AssignCenterData_{UserInfo.Id}");
			}
			else
			{
				_Navs.NavigateTo("/assign/center");
			}
		}

	}
}