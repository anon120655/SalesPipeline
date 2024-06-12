using SalesPipeline.Utils.Resources.Notifications;
using SalesPipeline.Utils.Resources.PreApprove;
using SalesPipeline.Utils.Resources.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Interfaces
{
	public interface INotifys
	{
		Task LineNotify(string msg);
		Task<NotificationCustom> Create(NotificationCustom model);
		Task<PaginationView<List<NotificationCustom>>> GetList(NotiFilter model);
		Task UpdateRead(List<Guid> model);
		Task<List<User_Login_TokenNotiCustom>> GetUserSendNotiById(int userid);
		Task<NotificationMobileResponse?> NotiMobile(NotificationMobile model);
		void SendNotification(string message);
		Task<int> SetScheduleNoti();

	}
}
