using SalesPipeline.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Infrastructure.Helpers
{
    public static class HttpClientFactoryService
    {
        private static HttpClient? _client;

        public static HttpClient GetClient(AppSettings appSet)
        {
            if (_client != null)
                return _client;

            var handler = new HttpClientHandler();

            // Custom SSL Validation DEV/UAT
            if (appSet.ServerSite == ServerSites.DEV || appSet.ServerSite == ServerSites.UAT)
            {
                handler.ServerCertificateCustomValidationCallback =
                    (message, cert, chain, errors) =>
                    {
                        if (errors == SslPolicyErrors.None)
                            return true;

                        if (errors.HasFlag(SslPolicyErrors.RemoteCertificateNameMismatch) ||
                            errors.HasFlag(SslPolicyErrors.RemoteCertificateChainErrors))
                            return true;

                        return false;
                    };
            }

            _client = new HttpClient(handler);
            return _client;
        }
    }
}
