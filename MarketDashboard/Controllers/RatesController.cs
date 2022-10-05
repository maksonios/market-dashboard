using MarketDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace MarketDashboard.Controllers
{
    [Route("api/rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        public object JsonConvert { get; private set; }

        [HttpGet("available-tickers")]
        public async Task<IActionResult> GetAvaialableTickers()
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync("https://api.binance.com/api/v3/ticker/price");
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            options.Converters.Add(new RateModelConverter());
            var serialize = JsonSerializer.Deserialize<IEnumerable<RateModel>>(content, options);
            
            return Ok(serialize.Select(x => x.Symbol));
        }

        // GET api/rates/USD-BTC RateModelConverter
        [HttpGet("{ticker}")]
        public async Task <IActionResult> GetRate(string ticker)
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync("https://api.binance.com/api/v3/ticker/price");
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            options.Converters.Add(new RateModelConverter());
            var rates = JsonSerializer.Deserialize<IEnumerable<RateModel>>(content, options);
            return Ok(rates.FirstOrDefault(x => x.Symbol == ticker));
        }



        /* [HttpGet("GetWeatherForecast")]
         public IEnumerable<RateModel> Get()
         {
             return Enumerable.Range(1, 5).Select(index => new RateModel
             {
                 Symbol = "BTC/EUR",
                 //Provider = "Binance",
                 Price = 1.5M,
                 //DateTime = DateTime.Now
             })
             .ToArray();
         }*/
    }
}
