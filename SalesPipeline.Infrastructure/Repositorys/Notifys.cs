using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class Notifys : INotifys
	{
		private IRepositoryWrapper _repo;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public Notifys(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet)
		{
			_db = db;
			_repo = repo;
			_appSet = appSet.Value;
		}

		public async Task LineNotify(string msg)
		{
			try
			{
				using (var client = new HttpClient())
				{
					var request = new HttpRequestMessage(HttpMethod.Post, _appSet.LineNotify?.baseUri);
					request.Headers.Add("Authorization", $"Bearer {_appSet.LineNotify?.Token}");
					var collection = new List<KeyValuePair<string, string>>();
					collection.Add(new("message", msg));
					var content = new FormUrlEncodedContent(collection);
					request.Content = content;
					var response = await client.SendAsync(request);
					//response.EnsureSuccessStatusCode();
					//Console.WriteLine(await response.Content.ReadAsStringAsync());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

	}
}
