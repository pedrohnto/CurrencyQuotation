using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyQuotationController : ControllerBase
    {

        private const string API_CURRENCY = "https://economia.awesomeapi.com.br/json";

        public CurrencyQuotationController()
        {
        }

        [HttpGet("last")]
        public async Task<string> getLastCurrencyQuotation([FromQuery] string currency)
        {
            return await getLastCurrencyQuotationAsync(currency);
        }

        [HttpGet("daily")]
        public async Task<string> getDailyCurrencyQuotation([FromQuery] string currency)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{API_CURRENCY}/daily/{currency}-BRL/10");
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private async Task<string> getLastCurrencyQuotationAsync(string currency)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{API_BTC}{currency}");
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

      
    }
}