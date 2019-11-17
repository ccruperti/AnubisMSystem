using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Extensions.ApiExtensions
{
    public static class GetServiceClient
    {
        public static string GetAppVersionHeader(this HttpRequestMessage requestMessage)
        {
            var versionHeader = requestMessage.Headers.TryGetValues("AppVersion", out var values);

            if (versionHeader)
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}
