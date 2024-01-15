using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public class SignalRUtls
	{
		/// <summary>
		/// /HubUserUrl
		/// </summary>
		public static string HubUserUrl = "/userCount";
		/// <summary>
		/// UpdateCount
		/// </summary>
		public static string UpdateCount = "UpdateCount";
		/// <summary>
		/// GetUserOnline
		/// </summary>
		public static string GetUserOnline = "GetUserOnline";
		/// <summary>
		/// SendUserOnline
		/// </summary>
		public static string SendUserOnline = "SendUserOnline";
		/// <summary>
		/// ReceiveUserOnline
		/// </summary>
		public static string ReceiveUserOnline = "ReceiveUserOnline";
		/// <summary>
		/// RemoveUserOnline
		/// </summary>
		public static string RemoveUserOnline = "RemoveUserOnline";
		/// <summary>
		/// ResetUserOnline
		/// </summary>
		public static string ResetUserOnline = "ResetUserOnline";
	}
}
