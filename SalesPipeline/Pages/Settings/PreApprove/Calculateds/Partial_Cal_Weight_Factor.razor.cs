using Microsoft.AspNetCore.Components;
using SalesPipeline.Utils.Resources.PreApprove;

namespace SalesPipeline.Pages.Settings.PreApprove.Calculateds
{
	public partial class Partial_Cal_Weight_Factor
	{
		[Parameter]
		public Guid pre_CalId { get; set; }

		[Parameter]
		public bool IsShowTab { get; set; }

		string? _errorMessage = null;
		private bool isLoading = false;
		//private Pre_Cal_InfoCustom formModel = new();
		public bool _internalIsShowTab { get; set; }

	}
}