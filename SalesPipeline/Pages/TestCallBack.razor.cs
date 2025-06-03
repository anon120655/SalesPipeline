using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SalesPipeline.Utils;
using System.Text;

namespace SalesPipeline.Pages
{
    public partial class TestCallBack
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "redirecturl")]
        public string? redirecturl { get; set; }

        string? _errorMessage = null;
        public string? redirecturl_decode = null;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(redirecturl))
                {
                    try
                    {
                        // แปลง Base64 กลับเป็น byte array
                        var bytes = Convert.FromBase64String(redirecturl);

                        // แปลง byte array กลับเป็นสตริงปกติ (UTF-8)
                        redirecturl_decode = Encoding.UTF8.GetString(bytes);

                    }
                    catch (Exception)
                    {
                        _errorMessage = "รูปแบบ base64 ไม่ถูกต้อง";
                    }
                    await Task.CompletedTask;
                }
                firstRender = false;
            }
        }


        protected async Task ToApp()
        {
            _errorMessage = null;
            try
            {
                if (!string.IsNullOrEmpty(redirecturl_decode))
                {
                    // ลองสั่งเปิด app ด้วย custom URL scheme
                    //await _jsRuntimes.InvokeVoidAsync("openCustomUrlScheme", redirecturl_decode);
                    _Navs.NavigateTo(redirecturl_decode);
                }
            }
            catch (Exception ex)
            {
                _errorMessage = GeneralUtils.GetExMessage(ex);
                // handle กรณีที่ไม่สามารถเปิดได้
            }
        }


    }
}