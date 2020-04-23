using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspNetCoreAzureTranslationService.Services {
    public interface IAzureTranslationService {
        Task<string> TranslationTextRequest(AzureTranslationData data);
    }

    public class AzureTranslationOptions {
        public string TranslatorTextSubscriptionKey { get; set; }
        public string TranslatorTextEndpoint { get; set; }
    }

    public class AzureTranslationData {
        public AzureTextTranslationData[] Texts { get; set; }
        public string Language { get; set; }
    }

    public class AzureTextTranslationData {
        public string Text { get; set; }
    }

    public class AzureTranslationService : IAzureTranslationService {
        private readonly HttpClient _httpClient;
        private readonly AzureTranslationOptions _options;

        public AzureTranslationService(IHttpClientFactory clientFactory, IOptionsMonitor<AzureTranslationOptions> optionsAccessor) {
            _options = optionsAccessor.CurrentValue;
            if(null == _options.TranslatorTextSubscriptionKey) {
                throw new Exception("Please set Azure translator text subscription key");
            }
            if(null == _options.TranslatorTextEndpoint) {
                throw new Exception("Please set Azure translator text endpoint");
            }
            _httpClient = clientFactory.CreateClient();
        }

        public async Task<string> TranslationTextRequest(AzureTranslationData data) {
            var route = "/translate?api-version=3.0&to=" + data.Language;
            var requestBody = JsonConvert.SerializeObject(data.Texts);
            using(var request = new HttpRequestMessage()) {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(_options.TranslatorTextEndpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _options.TranslatorTextSubscriptionKey);
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}
