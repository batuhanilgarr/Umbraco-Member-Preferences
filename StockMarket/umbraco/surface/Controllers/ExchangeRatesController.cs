using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using StockMarket.Models;

namespace StockMarket.Controllers
{
    [ApiController]
    [Route("umbraco/surface/api/[controller]")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string apiKey = "cf59c435feebd83694769e79adb4513a";
        private const string ApiUrl = $"https://api.exchangeratesapi.io/v1/latest?access_key={apiKey}";

        public ExchangeRatesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestExchangeRates()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiUrl);

                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch exchange rates.");

                var jsonResponse = await response.Content.ReadAsStringAsync();

                Console.WriteLine(jsonResponse);

                var exchangeRates = JsonSerializer.Deserialize<ExchangeRatesResponse>(jsonResponse);

                return Ok(exchangeRates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
