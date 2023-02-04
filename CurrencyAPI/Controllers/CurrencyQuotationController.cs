using CurrencyAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;

namespace CurrencyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyQuotationController : ControllerBase
    {

        private const string API_CURRENCY = "https://economia.awesomeapi.com.br/json";
        private const string API_BTC = "https://api.kraken.com/0/public/Ticker?pair=XBT";

        public CurrencyQuotationController()
        {
        }

        [HttpGet("last/currency")]
        public async Task<IActionResult> getLastCurrencyQuotation([FromQuery] string currency)
        {
            var response = await getLastCurrencyQuotationAsync(currency);
            return Ok(response);
        }

        [HttpGet("last/btc")]
        public async Task<IActionResult> getLastBTCQuotation()
        {
            var response =  new List<BtcQuotation>() {
                await getBTCQuotationAsync("eur"),
                await getBTCQuotationAsync("usd") 
            };
            return Ok(response);
        }

        [HttpGet("daily")]
        public async Task<IActionResult> getDailyCurrencyQuotation([FromQuery] string currency)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{API_CURRENCY}/daily/{currency}-BRL/10");
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        private async Task<string> getLastCurrencyQuotationAsync(string currency)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{API_CURRENCY}/last/{currency}-BRL");
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<BtcQuotation?> getBTCQuotationAsync(string currency)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{API_BTC}{currency}");
            string content = await response.Content.ReadAsStringAsync();

            JObject jsonObject = JObject.Parse(content);
            var quotation = jsonObject["result"];

            string propertyKey = $"XXBTZ{currency.ToUpper()}";

            if (quotation != null) {
                var value = quotation[propertyKey]?["a"]?[0];
                if (value != null) return new BtcQuotation() { Currency = currency, Quotation = value.ToString() };
            }
            return null;
        }

    }
}