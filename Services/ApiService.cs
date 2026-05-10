using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using AptekaLib;

namespace Apteka.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService()
        {
            var baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/api";
            _http = new HttpClient { BaseAddress = new System.Uri(baseUrl) };
        }

        // 🔹 ПРОСТОЙ ТЕСТОВЫЙ МЕТОД
        public async Task<string> TestConnectionAsync()
        {
            try
            {
                var response = await _http.GetAsync("medicines/categories");
                if (response.IsSuccessStatusCode)
                    return "OK: " + await response.Content.ReadAsStringAsync();
                return "ERROR: " + response.StatusCode;
            }
            catch (System.Exception ex)
            {
                return "EXCEPTION: " + ex.Message;
            }
        }
    }
}