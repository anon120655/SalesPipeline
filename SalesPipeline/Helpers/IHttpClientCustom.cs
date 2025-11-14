using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Shares;
using SalesPipeline.ViewModels;
using System.Net;
using System.Net.Http.Headers;

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
        private bool isDevOrUat = false;
        private const string HeaderContentType = "Content-Type";
        private const string MediaTypeJson = "application/json";
        public const string Authorization = "Authorization";
        public const string Bearer = "Bearer";


        public HttpClientCustom(HttpClient httpClient, AuthorizeViewModel authorizeViewModel, IOptions<AppSettings> appset)
        {
            _httpClient = httpClient;
            _authorizeViewModel = authorizeViewModel;
            _appSet = appset.Value;

            isDevOrUat = _appSet.ServerSite == ServerSites.DEV || _appSet.ServerSite == ServerSites.UAT;
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
            string fullUrl = string.Empty;

            try
            {
                if (baseUri == null) baseUri = _appSet.baseUriApi;

                fullUrl = baseUri + url;

                var options = new RestClientOptions(fullUrl)
                {
                    ThrowOnAnyError = false,
                    Timeout = TimeSpan.FromMinutes(2)
                };

                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
                {
                    options.Authenticator = new HttpBasicAuthenticator(userName, password);
                }

                var client = new RestClient(options);

                var request = new RestRequest();

                request.Method = Method.Get;
                request.AddHeader(HeaderContentType, MediaTypeJson);

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken) && userName == null)
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
                }

                var responseClient = await client.ExecuteAsync(request);

                if (responseClient.StatusCode == HttpStatusCode.OK && responseClient.Content != null)
                {
                    return responseClient.Content;
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
                                            throw new InvalidOperationException($"{txterror.field} {txterror.message}");
                                        }
                                    }
                                }
                                throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                            }
                            else
                            {
                                var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
                                if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
                                {
                                    throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                                }
                            }
                        }
                        throw new InvalidOperationException(dataMap?.Message);
                    }
                }

                if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (responseClient.Content != null)
                    {
                        if (responseClient.Content.Contains("EnableRetryOnFailure"))
                        {
                            throw new InvalidOperationException("Retry On Failure.");
                        }
                        else
                        {
                            throw new InvalidOperationException("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ");
                        }
                    }
                }

                if (responseClient.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new InvalidOperationException($"Unauthorized! {fullUrl} _accessoken={_accessoken}");
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{GeneralUtils.GetExMessage(ex)} {fullUrl}");
            }
        }

        public async Task<string> PostAsync(string url, string dataJson, string? token = null, string? baseUri = null)
        {
            string fullUrl = string.Empty;

            try
            {
                if (baseUri == null) baseUri = _appSet.baseUriApi;

                fullUrl = baseUri + url;

                var options = new RestClientOptions(fullUrl)
                {
                    ThrowOnAnyError = false,
                    Timeout = TimeSpan.FromMinutes(5)
                };

                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                var client = new RestClient(options);
                var request = new RestRequest();

                request.Method = Method.Post;
                request.AddHeader(HeaderContentType, MediaTypeJson);

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken))
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
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
                                            throw new InvalidOperationException($"{txterror.field} {txterror.message}");
                                        }
                                    }
                                }
                                throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                            }
                            else
                            {
                                var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
                                if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
                                {
                                    throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                                }
                            }
                        }
                        throw new InvalidOperationException(dataMap?.Message);
                    }
                }

                if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (responseClient.Content != null)
                    {
                        throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                    }
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{GeneralUtils.GetExMessage(ex)} {fullUrl}");
            }
        }

        public async Task<string> PutAsync(string url, string dataJson, string? token = null)
        {
            try
            {
                var options = new RestClientOptions(_appSet.baseUriApi + url)
                {
                    ThrowOnAnyError = false,
                    Timeout = TimeSpan.FromMinutes(5)
                };
                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Put;
                request.AddHeader(HeaderContentType, MediaTypeJson);

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken))
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
                }
                request.AddStringBody(dataJson, DataFormat.Json);
                var responseClient = await client.ExecuteAsync(request);
                if (responseClient.StatusCode == HttpStatusCode.OK && responseClient.Content != null)
                {
                    return responseClient.Content;
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
                                            throw new InvalidOperationException($"{txterror.field} {txterror.message}");
                                        }
                                    }
                                }
                                throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                            }
                            else
                            {
                                var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
                                if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
                                {
                                    throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                                }
                            }
                        }
                        throw new InvalidOperationException(dataMap?.Message);
                    }
                }

                if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (responseClient.Content != null)
                    {
                        throw new InvalidOperationException("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ");
                    }
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{GeneralUtils.GetExMessage(ex)}");
            }
        }

        public async Task<string> DeleteAsync(string url, string? token = null)
        {
            try
            {
                var options = new RestClientOptions(_appSet.baseUriApi + url)
                {
                    ThrowOnAnyError = false,
                    Timeout = TimeSpan.FromMinutes(2),
                };
                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                var client = new RestClient(options);

                var request = new RestRequest();
                request.Method = Method.Delete;
                request.AddHeader(HeaderContentType, MediaTypeJson);

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken))
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
                }

                RestResponse responseClient = await client.ExecuteAsync(request);
                if (responseClient.StatusCode == HttpStatusCode.OK && responseClient.Content != null)
                {
                    return responseClient.Content;
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
                                            throw new InvalidOperationException($"{txterror.field} {txterror.message}");
                                        }
                                    }
                                }
                                throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                            }
                            else
                            {
                                var dataMapErrorDefault = JsonConvert.DeserializeObject<ErrorDefaultModel>(responseClient.Content);
                                if (dataMapErrorDefault != null && dataMapErrorDefault.traceId != null)
                                {
                                    throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                                }
                            }
                        }
                        throw new InvalidOperationException(dataMap?.Message);
                    }
                }

                if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (responseClient.Content != null)
                    {
                        throw new InvalidOperationException("เกิดข้อผิดพลาด กรุณาติดต่อผู้ดูแลระบบ");
                    }
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"{GeneralUtils.GetExMessage(ex)}");
            }
        }

        public async Task<byte[]?> GetByteAsync(string url, string? token = null)
        {
            try
            {
                var options = new RestClientOptions(_appSet.baseUriApi + url)
                {
                    ThrowOnAnyError = true,
                    Timeout = TimeSpan.FromMinutes(2),
                };
                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                var client = new RestClient(options);

                var request = new RestRequest();
                request.AddHeader(HeaderContentType, MediaTypeJson);

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken))
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
                }

                RestResponse responseClient = await client.ExecuteAsync(request);
                if (responseClient.StatusCode == HttpStatusCode.OK && responseClient.RawBytes != null)
                {
                    return responseClient.RawBytes;
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(GeneralUtils.GetExMessage(ex));
            }
        }

        public async Task<byte[]?> PostByteAsync(string url, string dataJson, string? token = null)
        {
            try
            {
                var options = new RestClientOptions(_appSet.baseUriApi + url)
                {
                    ThrowOnAnyError = false,
                    Timeout = TimeSpan.FromMinutes(5)
                };
                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader(HeaderContentType, MediaTypeJson);

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken))
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
                }
                request.AddStringBody(dataJson, DataFormat.Json);
                var responseClient = await client.ExecuteAsync(request);
                if (responseClient.StatusCode == HttpStatusCode.OK && responseClient.RawBytes != null)
                {
                    return responseClient.RawBytes;
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(GeneralUtils.GetExMessage(ex));
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
                if (isDevOrUat)
                {
                    options.RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                }

                var client = new RestClient(options);
                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader(HeaderContentType, "multipart/form-data");

                var _accessoken = await _authorizeViewModel.GetAccessToken();

                if (!string.IsNullOrEmpty(token))
                    _accessoken = token;

                if (!string.IsNullOrWhiteSpace(_accessoken))
                {
                    var _authHeader = new AuthenticationHeaderValue(Bearer, _accessoken);
                    request.AddHeader(Authorization, $"{_authHeader.Scheme} {_authHeader.Parameter}");
                }

                if (files != null && files.FileByte != null)
                {
                    request.AddFile("FileData", files.FileByte, files.FileName ?? "", "multipart/form-data");
                    request.AddParameter("Folder", files?.Folder ?? "");
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
                                throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                            }
                        }
                        throw new InvalidOperationException(dataMap?.Message);
                    }
                }

                if (responseClient.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    if (responseClient.Content != null)
                    {
                        throw new InvalidOperationException(GeneralTxt.ExceptionTxtDefault);
                    }
                }

                throw new InvalidOperationException($"{responseClient.StatusCode} Error : {responseClient.ErrorMessage}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(GeneralUtils.GetExMessage(ex));
            }
        }

    }
}
