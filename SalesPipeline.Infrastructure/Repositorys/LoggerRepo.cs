using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Repositorys
{
	public class LoggerRepo : ILoggerRepo
	{
		private IRepositoryWrapper _repo;
		private readonly IRepositoryBase _db;
		private readonly AppSettings _appSet;

		public LoggerRepo(IRepositoryWrapper repo, IRepositoryBase db, IOptions<AppSettings> appSet)
		{
			_db = db;
			_repo = repo;
			_appSet = appSet.Value;
		}

		public async Task SaveLog(RequestResponseLogModel logs)
		{
			bool responseExcept = GeneralUtils.LOGExcept(logs.RequestPath);
			if (!responseExcept || (logs.ResponseStatus != null && !logs.ResponseStatus.StartsWith("20")))
			{
				int BodyLimit = 1000;
				string? _requestBody = logs.RequestBody;
				string? _responseBody = logs.ResponseBody;

				if (_requestBody != null && _requestBody.Length > BodyLimit)
				{
					_requestBody = _requestBody.Substring(0, BodyLimit);
				}
				if (_responseBody != null && _responseBody.Length > BodyLimit)
				{
					_responseBody = _responseBody.Substring(0, BodyLimit);
				}

				try
				{
					//_repo.Context.ChangeTracker.Clear();

					var logging = new Data.Entity.Logging()
					{
						LogId = Guid.NewGuid(),
						RequestDate = logs.RequestDateTimeUtc,
						Method = logs.RequestMethod,
						Scheme = logs.RequestScheme,
						Host = logs.RequestHost,
						Path = logs.RequestPath,
						Query = logs.RequestQuery,
						ContentType = logs.RequestContentType,
						RequestBody = _requestBody,
						ResponseStatus = logs.ResponseStatus,
						ResponseContentType = logs.ResponseContentType,
						ResponseBody = _responseBody,
						ClientIp = logs.ClientIp,
						ResponseDate = logs.ResponseDateTimeUtc,
						ExceptionMessage = logs.ExceptionMessage
					};
					await _db.InsterAsync(logging);
					await _db.SaveAsync();

					if (logs.ResponseStatus == StatusCodes.Status500InternalServerError.ToString() || GeneralUtils.LineTxtAlert(logs.ResponseBody))
					{
						var _responseBodyLine = _responseBody;
						if (_responseBodyLine != null && _responseBodyLine.Length > 300)
						{
							_responseBodyLine = _responseBodyLine.Substring(0, 300);
						}

						string lineNotiMsg = $"{Environment.NewLine} LOGID : {logging.LogId}";
						lineNotiMsg += $"{Environment.NewLine} PATH : {logging.Path}";
						lineNotiMsg += $"{Environment.NewLine} STATUS : {logging.ResponseStatus}";
						lineNotiMsg += $"{Environment.NewLine} MSG : {_responseBodyLine}";
						await _repo.Notifys.LineNotify(lineNotiMsg);
					}
				}
				catch (Exception ex)
				{

				}
			}
		}


	}
}
