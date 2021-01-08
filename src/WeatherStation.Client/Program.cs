using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Newtonsoft.Json;
using WeatherStation.Contracts;

namespace WeatherStation.Client
{
    public static class Program
    {
        private const string BragaCode = "BRG";

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Requesting current weather..");

            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client = new WeatherService.WeatherServiceClient(channel);

            var request = new WeatherRequest
            {
                Location = BragaCode
            };

            var weatherData = await client.RequestCurrentWeatherDataAsync(request);

            Console.WriteLine(JsonConvert.SerializeObject(weatherData));

            Console.WriteLine("Requesting historic data..");

            var historicData = await client.RequestHistoricDataAsync(request);

            Console.WriteLine(JsonConvert.SerializeObject(historicData));

            Console.ReadLine();
        }
    }
}
