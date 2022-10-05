using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarketDashboard.Models
{
    public class RateModel
    {
        public string Symbol { get; set; } // "USD/EUR", "BTC/ETH"
        public decimal Price { get; set; }

    }
    public class RateModelConverter : JsonConverter<RateModel>
    {
        public override RateModel? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            var message = new RateModel();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return message;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected PropertyName token");

                var propName = reader.GetString();
                reader.Read();

                switch (propName)
                {
                    case "price":
                        message.Price = Decimal.Parse(reader.GetString());
                        break;
                    case "symbol":
                        message.Symbol = reader.GetString();
                        break;
                }
            }
            return message;
        }

        public override void Write(Utf8JsonWriter writer, RateModel value, JsonSerializerOptions options)
        {
            
        }
    }
}