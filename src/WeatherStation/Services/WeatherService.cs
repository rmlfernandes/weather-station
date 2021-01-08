using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace WeatherStation.Services
{
    public class WeatherService : Contracts.WeatherService.WeatherServiceBase
    {
        private const string BragaCode = "BRG";
        private const string PortoCode = "PRT";
        private const string LisboaCode = "LIS";

        private readonly ILogger<WeatherService> logger;

        public WeatherService(ILogger<WeatherService> logger)
        {
            this.logger = logger;
        }

        public override Task<Contracts.WeatherDataResponse> RequestCurrentWeatherData(
            Contracts.WeatherRequest request,
            ServerCallContext context)
        {
            this.logger.LogInformation($"Requesting current weather for location {request.Location}..");

            return Task.FromResult(GetWeatherData(request.Location));
        }

        public override Task<Contracts.WeatherHistoricResponse> RequestHistoricData(
            Contracts.WeatherRequest request,
            ServerCallContext context)
        {
            this.logger.LogInformation($"Requesting historic data for location {request.Location}..");

            return Task.FromResult(GetWeatherHistoric(request.Location));
        }

        private static Contracts.WeatherDataResponse GetWeatherData(string location)
        {
            return location switch
            {
                BragaCode => new Contracts.WeatherDataResponse
                {
                    Location = BragaCode,
                    Temperature = 12,
                    Winddirection = 1,
                    Windspeed = 2,
                    Time = Timestamp.FromDateTime(DateTime.UtcNow)
                },

                PortoCode => new Contracts.WeatherDataResponse
                {
                    Location = PortoCode,
                    Temperature = 20,
                    Winddirection = 2,
                    Windspeed = 3,
                    Time = Timestamp.FromDateTime(DateTime.UtcNow)
                },

                LisboaCode => new Contracts.WeatherDataResponse
                {
                    Location = LisboaCode,
                    Temperature = 25,
                    Winddirection = 3,
                    Windspeed = 2,
                    Time = Timestamp.FromDateTime(DateTime.UtcNow)
                },

                _ => new Contracts.WeatherDataResponse
                {
                    Location = "n/a",
                    Temperature = 0,
                    Winddirection = 0,
                    Windspeed = 0,
                    Time = Timestamp.FromDateTime(DateTime.MinValue)
                }
            };
        }

        private static Contracts.WeatherHistoricResponse GetWeatherHistoric(string location)
        {
            var response = new Contracts.WeatherHistoricResponse();

            for (int i = 0; i < 5; i++)
            {
                var weatherData = GetWeatherData(location);
                weatherData.Temperature += i;
                weatherData.Time = Timestamp.FromDateTime(weatherData.Time.ToDateTime().AddDays(-i));

                response.Data.Add(weatherData);
            }

            return response;
        }
    }
}
