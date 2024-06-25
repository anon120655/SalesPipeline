using Microsoft.Extensions.Options;
using RestSharp.Authenticators;
using RestSharp;
using SalesPipeline.Utils;
using System.Net.Http.Headers;
using SalesPipeline.ViewModels;
using Newtonsoft.Json;
using SalesPipeline.Utils.Resources.Shares;

namespace SalesPipeline.Helpers
{

	public interface IHttpClientCustom
	{
		string GetBaseAddress();
		Task<string> GetAsync(string url, string? token = null, string? baseUri = null, string? userName = null, string? password = null);
		Task<string> PostAsync(string url, string jsonTxt, string? token = null, string? baseUri = null);
		Task<string> PutAsync(string url, string jsonTxt, string? token = null);
		Task<string> DeleteAsync(string url, string? token = null);
		Task<byte[]?> GetByteAsync(string url, string? token = null);
		Task<byte[]?> PostByteAsync(string url, string jsonTxt, string? token = null);
		Task<string> PostFileAsync(string url, FileModel formFiles, string? token = null);
	}


	public class HttpClientCustom : IHttpClientCustom
	{
		private readonly HttpClient _httpClient;
		private readonly AuthorizeViewModel _authorizeViewModel;
		private readonly AppSettings _appSet;

		public HttpClientCustom(HttpClient httpClient, AuthorizeViewModel authorizeViewModel, IOptions<AppSettings> appset)
		{
			_httpClient = httpClient;
			_authorizeViewModel = authorizeViewModel;
			_appSet = appset.Value;
		}

		public string GetBaseAddress()
		{
			String _BaseAddress = String.Empty;

			if (_httpClient.BaseAddress != null && _httpClient.BaseAddress.IsAbsoluteUri)
			{
				_BaseAddress = _httpClient.BaseAddress.AbsoluteUri;
			}
			return _BaseAddress;
		}

		public async Task<string> GetAsync(string url, string? token = null, string? baseUri = null, string? userName = null, string? password = null)
		{
			try
			{
				if (baseUri == null) baseUri = _appSet.baseUriApi;

				var options = new RestClientOptions(baseUri + url)
				{
					ThrowOnAnyError = false,
					Timeout = TimeSpan.FromMinutes(2)
				};

				if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
				{
					options.Authenticator = new HttpBasicAuthenticator(userName, password);
				}

				var client = new RestClient(options);

				var request = new RestRequest();

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

				//ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

				request.Method = Method.Get;
				request.AddHeader("Content-Type", "application/json");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken) && userName == null)
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}

				var responseClient = await client.ExecuteAsync(request);
				//if (responseClient.ErrorException != null)
				//{
				//	throw responseClient.ErrorException;
				//}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK)
				{
					if (responseClient.Content != null)
					{
						return responseClient.Content;
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseClient.Content != null)
					{
						var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(responseClient.Content);
						if (dataMap != null && dataMap.Message == null)
						{
							if (dataMap.Status == 422)
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<Error422Model>(responseClient.Content);
								if (dataMapErrorDefault != null)
								{
									if (dataMapErrorDefault.errors?.Count > 0)
									{
										var txterror = dataMapErrorDefault.errors.FirstOrDefault();
										if (txterror != null)
										{
											throw new Exception($"{txterror.field} {txterror.message}");
										}
									}
								}
								throw new Exception(GeneralTxt.ExceptionTxtDefault);
							}
							else
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
								if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
								{
									throw new Exception(GeneralTxt.ExceptionTxtDefault);
								}
							}
						}
						throw new Exception(dataMap?.Message);
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					if (responseClient.Content != null)
					{
						if (responseClient.Content.Contains("EnableRetryOnFailure"))
						{
							throw new Exception("Retry On Failure.");
						}
						else
						{
							throw new Exception("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ");
						}
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
					throw new Exception("Unauthorized!");
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception($"{GeneralUtils.GetExMessage(ex)}");
			}
		}

		public async Task<string> PostAsync(string url, string dataJson, string? token = null, string? baseUri = null)
		{
			try
			{
				if (baseUri == null) baseUri = _appSet.baseUriApi;

				var options = new RestClientOptions(baseUri + url)
				{
					ThrowOnAnyError = false
				};
				var client = new RestClient(options);
				var request = new RestRequest();

				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				request.Method = Method.Post;
				request.AddHeader("Content-Type", "application/json");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken))
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}
				request.AddStringBody(dataJson, DataFormat.Json);

				var responseClient = await client.ExecuteAsync(request);
				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK || responseClient.StatusCode == System.Net.HttpStatusCode.Created)
				{
					if (responseClient.Content != null)
					{
						return responseClient.Content;
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseClient.Content != null)
					{
						var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(responseClient.Content);
						if (dataMap != null && dataMap.Message == null)
						{
							if (dataMap.Status == 422)
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<Error422Model>(responseClient.Content);
								if (dataMapErrorDefault != null)
								{
									if (dataMapErrorDefault.errors?.Count > 0)
									{
										var txterror = dataMapErrorDefault.errors.FirstOrDefault();
										if (txterror != null)
										{
											throw new Exception($"{txterror.field} {txterror.message}");
										}
									}
								}
								throw new Exception(GeneralTxt.ExceptionTxtDefault);
							}
							else
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
								if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
								{
									throw new Exception(GeneralTxt.ExceptionTxtDefault);
								}
							}
						}
						throw new Exception(dataMap?.Message);
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					if (responseClient.Content != null)
					{
						throw new Exception(GeneralTxt.ExceptionTxtDefault);
					}
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception($"{GeneralUtils.GetExMessage(ex)}");
			}
		}

		public async Task<string> PutAsync(string url, string dataJson, string? token = null)
		{
			try
			{
				var options = new RestClientOptions(_appSet.baseUriApi + url)
				{
					ThrowOnAnyError = false
				};
				var client = new RestClient(options);
				var request = new RestRequest();
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				request.Method = Method.Put;
				request.AddHeader("Content-Type", "application/json");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken))
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}
				request.AddStringBody(dataJson, DataFormat.Json);
				var responseClient = await client.ExecuteAsync(request);
				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK)
				{
					if (responseClient.Content != null)
					{
						return responseClient.Content;
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseClient.Content != null)
					{
						var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(responseClient.Content);
						if (dataMap != null && dataMap.Message == null)
						{
							if (dataMap.Status == 422)
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<Error422Model>(responseClient.Content);
								if (dataMapErrorDefault != null)
								{
									if (dataMapErrorDefault.errors?.Count > 0)
									{
										var txterror = dataMapErrorDefault.errors.FirstOrDefault();
										if (txterror != null)
										{
											throw new Exception($"{txterror.field} {txterror.message}");
										}
									}
								}
								throw new Exception(GeneralTxt.ExceptionTxtDefault);
							}
							else
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
								if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
								{
									throw new Exception(GeneralTxt.ExceptionTxtDefault);
								}
							}
						}
						throw new Exception(dataMap?.Message);
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					if (responseClient.Content != null)
					{
						throw new Exception("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ");
					}
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception($"{GeneralUtils.GetExMessage(ex)}");
			}
		}

		public async Task<string> DeleteAsync(string url, string? token = null)
		{
			try
			{
				var options = new RestClientOptions(_appSet.baseUriApi + url)
				{
					ThrowOnAnyError = false
				};
				var client = new RestClient(options);

				var request = new RestRequest();
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				request.Method = Method.Delete;
				request.AddHeader("Content-Type", "application/json");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken))
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}

				RestResponse responseClient = await client.ExecuteAsync(request);
				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK)
				{
					if (responseClient.Content != null)
					{
						return responseClient.Content;
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseClient.Content != null)
					{
						var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(responseClient.Content);
						if (dataMap != null && dataMap.Message == null)
						{
							if (dataMap.Status == 422)
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<Error422Model>(responseClient.Content);
								if (dataMapErrorDefault != null)
								{
									if (dataMapErrorDefault.errors?.Count > 0)
									{
										var txterror = dataMapErrorDefault.errors.FirstOrDefault();
										if (txterror != null)
										{
											throw new Exception($"{txterror.field} {txterror.message}");
										}
									}
								}
								throw new Exception(GeneralTxt.ExceptionTxtDefault);
							}
							else
							{
								var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
								if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
								{
									throw new Exception(GeneralTxt.ExceptionTxtDefault);
								}
							}
						}
						throw new Exception(dataMap?.Message);
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					if (responseClient.Content != null)
					{
						throw new Exception("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ");
					}
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception($"{GeneralUtils.GetExMessage(ex)}");
			}
		}

		public async Task<byte[]?> GetByteAsync(string url, string? token = null)
		{
			try
			{
				var options = new RestClientOptions(_appSet.baseUriApi + url)
				{
					ThrowOnAnyError = true
				};
				var client = new RestClient(options);

				var request = new RestRequest();
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				request.AddHeader("Content-Type", "application/json");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken))
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}

				RestResponse responseClient = await client.ExecuteAsync(request);
				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var RawBytes = responseClient.RawBytes;

					if (responseClient.RawBytes != null)
					{
						return responseClient.RawBytes;
					}
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception(GeneralUtils.GetExMessage(ex));
			}
		}

		public async Task<byte[]?> PostByteAsync(string url, string dataJson, string? token = null)
		{
			try
			{
				var options = new RestClientOptions(_appSet.baseUriApi + url)
				{
					ThrowOnAnyError = false,
					Timeout = TimeSpan.FromMinutes(2)
				};
				var client = new RestClient(options);
				var request = new RestRequest();
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				request.Method = Method.Post;
				request.AddHeader("Content-Type", "application/json");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken))
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}
				request.AddStringBody(dataJson, DataFormat.Json);
				var responseClient = await client.ExecuteAsync(request);
				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK)
				{
					var RawBytes = responseClient.RawBytes;

					if (responseClient.RawBytes != null)
					{
						return responseClient.RawBytes;
					}
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception(GeneralUtils.GetExMessage(ex));
			}
		}

		public async Task<string> PostFileAsync(string url, FileModel files, string? token = null)
		{
			try
			{
				var options = new RestClientOptions(_appSet.baseUriApi + url)
				{
					ThrowOnAnyError = false,
					Timeout = TimeSpan.FromMinutes(2)
				};
				var client = new RestClient(options);
				var request = new RestRequest();
				System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				request.Method = Method.Post;
				//request.AlwaysMultipartFormData = true;
				request.AddHeader("Content-Type", "multipart/form-data");

				var _accessoken = await _authorizeViewModel.GetAccessToken();
				if (token is not null) _accessoken = token;
				if (!string.IsNullOrWhiteSpace(_accessoken))
				{
					var _authHeader = new AuthenticationHeaderValue("Bearer", _accessoken);
					request.AddHeader("Authorization", $"{_authHeader.Scheme} {_authHeader.Parameter}");
				}

				if (files != null && files.FileByte != null)
				{
					request.AddFile("FileData", files.FileByte, files.FileName ?? "", "multipart/form-data");
					request.AddParameter("Folder", files?.Folder ?? "");
					//request.AddParameter("Id", formFiles?.Id.ToString());
					//request.AddParameter("Categorie", formFiles?.Categorie ?? "");
					//request.AddParameter("Type", formFiles?.Type.ToString() ?? "");
					//request.AddParameter("SubType", formFiles?.SubType ?? "");
					//request.AddParameter("FileSize", formFiles?.FileSize.ToString());
					//request.AddParameter("MimeType", formFiles?.MimeType ?? "");
				}

				var responseClient = await client.ExecuteAsync(request);
				if (responseClient.StatusCode == System.Net.HttpStatusCode.OK || responseClient.StatusCode == System.Net.HttpStatusCode.Created)
				{
					if (responseClient.Content != null)
					{
						return responseClient.Content;
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.BadRequest)
				{
					if (responseClient.Content != null)
					{
						var dataMap = JsonConvert.DeserializeObject<ErrorCustom>(responseClient.Content);
						if (dataMap != null && dataMap.Message == null)
						{
							var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
							if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
							{
								throw new Exception(GeneralTxt.ExceptionTxtDefault);
							}
						}
						throw new Exception(dataMap?.Message);
					}
				}

				if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
				{
					if (responseClient.Content != null)
					{
						throw new Exception(GeneralTxt.ExceptionTxtDefault);
					}
				}

				throw new Exception($"StatusCode : {responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
			}
			catch (Exception ex)
			{
				throw new Exception(GeneralUtils.GetExMessage(ex));
			}
		}

	}
}
