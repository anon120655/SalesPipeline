using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Loggers;
using System.Text.RegularExpressions;

namespace SalesPipeline.Infrastructure.Helpers
{
	public class RequestResponseMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly AppSettings _appSet;

		public RequestResponseMiddleware(RequestDelegate next, IOptions<AppSettings> options)
		{
			_next = next;
			_appSet = options.Value;
		}

		public async Task InvokeAsync(HttpContext httpContext, IRepositoryWrapper repo)
		{
			RequestResponseLogModel log = new();
			// Middleware is enabled only when the 
			// EnableRequestResponseLogging config value is set.
			if (_appSet == null || _appSet.RequestResponseLogger == null || !_appSet.RequestResponseLogger.IsEnabled)
			{
				await _next(httpContext);
				return;
			}
			log.RequestDateTimeUtc = DateTime.Now;
			HttpRequest request = httpContext.Request;

			/*log*/
			log.LogId = Guid.NewGuid().ToString();
			log.TraceId = httpContext.TraceIdentifier;
			var ip = request.HttpContext.Connection.RemoteIpAddress;
			log.ClientIp = ip == null ? null : ip.ToString();
			log.Node = _appSet.RequestResponseLogger.Name;

			/*request*/
			log.RequestMethod = request.Method;
			log.RequestPath = request.Path;
			log.RequestQuery = request.QueryString.ToString();
			//log.RequestQueries = FormatQueries(request.QueryString.ToString());
			//log.RequestHeaders = FormatHeaders(request.Headers);
			log.RequestBody = await ReadBodyFromRequest(request);
			log.RequestScheme = request.Scheme;
			log.RequestHost = request.Host.ToString();
			log.RequestContentType = request.ContentType;

			if (request != null)
			{
				string device_info = string.Empty;
				var browserinfo = new BrowserModel()
				{
					userAgent = request.Headers["User-Agent"].ToString(),
					OS = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline),
					device = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline)
				};

				try
				{
					if (!String.IsNullOrEmpty(browserinfo.userAgent))
					{
						//if (browserinfo.OS.IsMatch(browserinfo.userAgent))
						//{
						//	device_info = browserinfo.OS.Match(browserinfo.userAgent).Groups[0].Value;
						//}
						//if (browserinfo.userAgent.Length >= 3 && browserinfo.device.IsMatch(browserinfo.userAgent.Substring(0, 4)))
						//{
						//	device_info += browserinfo.device.Match(browserinfo.userAgent).Groups[0].Value;
						//}

						//if (!string.IsNullOrEmpty(device_info))
						//{
						//	log.DeviceInfo = "Mobile device. " + device_info;
						//}
						//else
						//{
						//	log.DeviceInfo = "Desktop device.";
						//}

						log.DeviceInfo = browserinfo.userAgent;
					}
				}
				catch (Exception ex)
				{
					log.DeviceInfo = "Error device." + ex.GetBaseException().Message;
				}
			}

			// Temporarily replace the HttpResponseStream, 
			// which is a write-only stream, with a MemoryStream to capture 
			// its value in-flight.
			HttpResponse response = httpContext.Response;
			var originalResponseBody = response.Body;
			using var newResponseBody = new MemoryStream();
			response.Body = newResponseBody;

			// Call the next middleware in the pipeline
			try
			{
				await _next(httpContext);
			}
			catch (Exception exception)
			{
				/*exception: but was not managed at app.UseExceptionHandler() 
                  or by any middleware*/
				LogError(log, exception);
			}

			newResponseBody.Seek(0, SeekOrigin.Begin);
			var responseBodyText = await new StreamReader(response.Body).ReadToEndAsync();

			newResponseBody.Seek(0, SeekOrigin.Begin);
			await newResponseBody.CopyToAsync(originalResponseBody);

			/*response*/
			log.ResponseContentType = response.ContentType;
			log.ResponseStatus = response.StatusCode.ToString();
			//log.ResponseHeaders = FormatHeaders(response.Headers);
			log.ResponseBody = responseBodyText;
			log.ResponseDateTimeUtc = DateTime.Now;


			/*exception: but was managed at app.UseExceptionHandler() 
              or by any middleware*/
			var contextFeature = httpContext.Features.Get<IExceptionHandlerPathFeature>();
			if (contextFeature != null && contextFeature.Error != null)
			{
				Exception exception = contextFeature.Error;
				LogError(log, exception);
			}

			//_ = repo.Logger.SaveLog(log).ConfigureAwait(false); //บันทึก log ในพื้นหลัง โดยไม่รอให้เสร็จสิ้น มีโอกาสที่ log อาจจะยังบันทึกไม่เสร็จและ Task ทำงานจบก่อน
			await repo.Logger.SaveLog(log).ConfigureAwait(false); //แบบนี้ไม่ทำให้เกิด deadlock แต่จะช้ากว่าแบบ _ =
																  //await repo.Logger.SaveLog(log);
		}

		private void LogError(RequestResponseLogModel log, Exception exception)
		{
			log.ExceptionMessage = exception.Message;
			log.ExceptionStackTrace = exception.StackTrace;
		}

		//private Dictionary<string, string> FormatHeaders(IHeaderDictionary headers)
		//{
		//	Dictionary<string, string> pairs = new Dictionary<string, string>();
		//	foreach (var header in headers)
		//	{
		//		pairs.Add(header.Key, header.Value);
		//	}
		//	return pairs;
		//}

		private List<KeyValuePair<string, string>> FormatQueries(string queryString)
		{
			List<KeyValuePair<string, string>> pairs =
				 new List<KeyValuePair<string, string>>();
			string key, value;
			foreach (var query in queryString.TrimStart('?').Split("&"))
			{
				var items = query.Split("=");
				key = items.Count() >= 1 ? items[0] : string.Empty;
				value = items.Count() >= 2 ? items[1] : string.Empty;
				if (!String.IsNullOrEmpty(key))
				{
					pairs.Add(new KeyValuePair<string, string>(key, value));
				}
			}
			return pairs;
		}

		private async Task<string> ReadBodyFromRequest(HttpRequest request)
		{
			// Ensure the request's body can be read multiple times 
			// (for the next middlewares in the pipeline).
			request.EnableBuffering();
			using var streamReader = new StreamReader(request.Body, leaveOpen: true);
			var requestBody = await streamReader.ReadToEndAsync();
			// Reset the request's body stream position for 
			// next middleware in the pipeline.
			request.Body.Position = 0;
			return requestBody;
		}

	}
}
