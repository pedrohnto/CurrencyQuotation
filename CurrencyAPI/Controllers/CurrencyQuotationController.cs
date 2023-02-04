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

        public CurrencyQuotationController()
        {
        }

        [HttpGet]
        public async Task<string> getCurrencyQuotationAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL,BTC-BRL");
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }


      
    }
}