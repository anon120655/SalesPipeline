using Microsoft.AspNetCore.SignalR;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.Utils;
using System.Xml.Linq;

namespace SalesPipeline.Hubs
{
	public class UserCount : Hub
	{
		//private static int Count;
		private static List<UserOnlineModel> UserOnlineModels = new();

		public async Task GetUserOnline()
		{
			await Clients.All.SendAsync(SignalRUtls.ReceiveUserOnline, UserOnlineModels);
		}

		public async Task SendUserOnline(UserOnlineModel user)
		{
			UserOnlineModels.Add(user);
			await Clients.All.SendAsync(SignalRUtls.ReceiveUserOnline, UserOnlineModels);
		}

		public async Task RemoveUserOnline(UserOnlineModel user)
		{
			UserOnlineModel? itemRemove;

			itemRemove = UserOnlineModels.FirstOrDefault(x => x.Id == user.Id && x.UserKey == user.UserKey);

			if (itemRemove != null)
			{
				UserOnlineModels.Remove(itemRemove);
				await Clients.All.SendAsync(SignalRUtls.ReceiveUserOnline, UserOnlineModels);
			}
		}

		public async Task ResetUserOnline()
		{
			UserOnlineModels = new();
			await Clients.All.SendAsync(SignalRUtls.ReceiveUserOnline, UserOnlineModels);
		}

	}
}
