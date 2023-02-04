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

        [HttpGet("last")]
        public async Task<string> getLastCurrencyQuotation([FromQuery] string currency)
        {
            return await getBTCQuotationAsync(currency);
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
            var response = await client.GetAsync($"{API_CURRENCY}/last/{currency}-BRL");
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private async Task<string> getBTCQuotationAsync(string currency)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{API_BTC}{currency}");
            var content = await response.Content.ReadAsStringAsync();

            JObject jsonObject = JObject.Parse(content);
            JObject quotation = (JObject) jsonObject["result"];

            string propertyKey = $"XXBTZ{currency.ToUpper()}";

            var value = quotation[propertyKey]["a"][0];

            return value.ToString();
        }



    }
}