using BlazorBootstrap;
using Microsoft.Extensions.Options;
using SalesPipeline.Helpers;
using SalesPipeline.Utils;
using System.Net.Http;

namespace SalesPipeline.ViewModels
{
	public class UtilsViewModel
	{
		private readonly AppSettings _mySet;
		private ToastService ToastService;

		public UtilsViewModel(IOptions<AppSettings> appset, ToastService _ToastService)
		{
			_mySet = appset.Value;
			ToastService = _ToastService;
		}

		public void AlertSuccess(string txtMessage = "บันทึกข้อมูลสำเร็จ")
		{
			ToastService.Notify(new ToastMessage
			{
				IconName = IconName.CheckCircleFill,
				Type = ToastType.Primary,
				Title = "Successfully",
				HelpText = $"{DateTime.Now}",
				Message = txtMessage,
			});
		}

		public void AlertWarning(string? txtMessage)
		{
			ToastService.Notify(new ToastMessage
			{
				Type = ToastType.Warning,
				Title = "Notification",
				HelpText = $"{DateTime.Now}",
				Message = txtMessage ?? String.Empty,
			});
		}


	}
}
