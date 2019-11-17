using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AnubisDBMS.Infraestructure.Data.Geography.ExternalApis;
using Newtonsoft.Json;

namespace AnubisDBMS.Infraestructure.Geography.ExternalApis
{
    public class GoogleMapsDistanceMatrixApiClient
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly string _apiBaseUrl = "https://maps.googleapis.com/maps/api/distancematrix/";

        internal Uri GetApiUri() => new Uri(_apiBaseUrl + ClientConfiguration.OutputFormat);

        private GoogleMapsDistanceMatrixClientConfiguration ClientConfiguration { get; }
    
        public GoogleMapsDistanceMatrixApiClient(GoogleMapsDistanceMatrixClientConfiguration clientConfiguration)
        {
            ClientConfiguration = clientConfiguration;
            _httpClient.BaseAddress = GetApiUri();
        }

        public async Task<DirectionsResponse> GetDirections(decimal latitudOrigen, decimal longitudOrigen, decimal latitudDestino, decimal longitudDestino)
        {
            var getDirectionsBaseUri = string.Format("?key={0}&units={1}&origins={2},{3}&destinations={4},{5}&language={6}",
                ClientConfiguration.ApiKey, ClientConfiguration.UnitType, latitudOrigen.ToString().Replace(',','.'), longitudOrigen.ToString().Replace(',', '.'), latitudDestino.ToString().Replace(',', '.'),
                longitudDestino.ToString().Replace(',', '.'), ClientConfiguration.Language);
            var request = await _httpClient.GetAsync(getDirectionsBaseUri);

            if (request.IsSuccessStatusCode)
            {
                var jsonResponse = await request.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DirectionsResponse>(jsonResponse);
            }
            
            return null;
        }
    }
}
