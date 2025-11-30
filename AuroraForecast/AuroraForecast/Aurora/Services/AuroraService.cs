using System.Collections;
using AuroraForecast.Aurora.Interfaces;
using AuroraForecast.Locations.Interfaces;
using AuroraForecast.Locations.Models;
using System.Globalization;
using AuroraForecast.Aurora.Models;

namespace AuroraForecast.Aurora.Services;

class AuroraService : IAuroraService
{
    private readonly ILocationService _locationService;
    private readonly IAuroraApiWrapper _auroraApiWrapper;

    public AuroraService(ILocationService locationService, IAuroraApiWrapper auroraApiWrapper)
    {
        _locationService = locationService;
        _auroraApiWrapper = auroraApiWrapper;
    }

    public async Task<IEnumerable<Models.AuroraForecast>> GetForecastsAsync(int userId)
    {
        var result = new List<Models.AuroraForecast>();

        var locations = await _locationService.GetUserLocationsAsync(userId).ConfigureAwait(false);

        foreach (var location in locations)
        {
            var locationForecast = await GetForecastForLocationAsync(location);
            result.Add(locationForecast);
        }

        return result;
    }

    private async Task<Models.AuroraForecast> GetForecastForLocationAsync(Location location)
    {
        var result = new Models.AuroraForecast
        {
            Location = location
        };

        var apiResponse = await _auroraApiWrapper.GetAuroraDataAsync(location).ConfigureAwait(false);

        if (apiResponse?.ThreeDay == null)
        {
            return result;
        }

        foreach (var dayForecastResponse in apiResponse.ThreeDay.Values)
        {
            var dayForecast = new AuroraDayForecast();
            foreach (var forecastResponse in dayForecastResponse)
            {
                dayForecast.PeriodForecasts.Add(new AuroraPeriodForecast
                {
                    Start = forecastResponse.Start,
                    End = forecastResponse.End,
                    KP = decimal.Parse(forecastResponse.Value, CultureInfo.InvariantCulture),
                    Colour = forecastResponse.Colour
                });
            }
            dayForecast.Date = dayForecast.PeriodForecasts.First().Start.Date;
            result.DayForecasts.Add(dayForecast);
        }

        return result;
    }
}


/*
 {
  "date": "2025-11-29T12:01:16+00:00",
  "ace": {
    "date": "2025-11-29T12:01:16+00:00",
    "bz": "1",
    "density": "0.75",
    "speed": "591.5",
    "kp1hour": "4.00",
    "kp4hour": "4.00",
    "kp": "4.00",
    "colour": {
      "bz": "green",
      "density": "green",
      "speed": "orange",
      "kp1hour": "yellow",
      "kp4hour": "yellow",
      "kp": "yellow"
    }
  },
  "weather": false,
  "threeday": {
    "date": "2025-11-29T12:01:16+00:00",
    "dates": [
      "2025-11-29T00:00:00+00:00",
      "2025-11-30T00:00:00+00:00",
      "2025-12-01T00:00:00+00:00"
    ],
    "values": [
      [
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T00:00:00+00:00",
          "end": "2025-11-29T03:00:00+00:00",
          "value": "2.33",
          "colour": "green"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T03:00:00+00:00",
          "end": "2025-11-29T06:00:00+00:00",
          "value": "2.67",
          "colour": "green"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T06:00:00+00:00",
          "end": "2025-11-29T09:00:00+00:00",
          "value": "3.33",
          "colour": "yellow"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T09:00:00+00:00",
          "end": "2025-11-29T12:00:00+00:00",
          "value": "3.67",
          "colour": "yellow"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": true,
          "start": "2025-11-29T12:00:00+00:00",
          "end": "2025-11-29T15:00:00+00:00",
          "value": "2.67",
          "colour": "green"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T15:00:00+00:00",
          "end": "2025-11-29T18:00:00+00:00",
          "value": "2.00",
          "colour": "green"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T18:00:00+00:00",
          "end": "2025-11-29T21:00:00+00:00",
          "value": "2.00",
          "colour": "green"
        },
        {
          "date": "2025-11-29T00:00:00+00:00",
          "now": false,
          "start": "2025-11-29T21:00:00+00:00",
          "end": "2025-11-30T00:00:00+00:00",
          "value": "2.67",
          "colour": "green"
        }
      ],
      [
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T00:00:00+00:00",
          "end": "2025-11-30T03:00:00+00:00",
          "value": "3.00",
          "colour": "yellow"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T03:00:00+00:00",
          "end": "2025-11-30T06:00:00+00:00",
          "value": "2.67",
          "colour": "green"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T06:00:00+00:00",
          "end": "2025-11-30T09:00:00+00:00",
          "value": "2.33",
          "colour": "green"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T09:00:00+00:00",
          "end": "2025-11-30T12:00:00+00:00",
          "value": "2.00",
          "colour": "green"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": true,
          "start": "2025-11-30T12:00:00+00:00",
          "end": "2025-11-30T15:00:00+00:00",
          "value": "1.33",
          "colour": "green"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T15:00:00+00:00",
          "end": "2025-11-30T18:00:00+00:00",
          "value": "1.67",
          "colour": "green"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T18:00:00+00:00",
          "end": "2025-11-30T21:00:00+00:00",
          "value": "1.67",
          "colour": "green"
        },
        {
          "date": "2025-11-30T00:00:00+00:00",
          "now": false,
          "start": "2025-11-30T21:00:00+00:00",
          "end": "2025-12-01T00:00:00+00:00",
          "value": "1.67",
          "colour": "green"
        }
      ],
      [
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T00:00:00+00:00",
          "end": "2025-12-01T03:00:00+00:00",
          "value": "1.67",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T03:00:00+00:00",
          "end": "2025-12-01T06:00:00+00:00",
          "value": "2.00",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T06:00:00+00:00",
          "end": "2025-12-01T09:00:00+00:00",
          "value": "1.33",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T09:00:00+00:00",
          "end": "2025-12-01T12:00:00+00:00",
          "value": "1.33",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": true,
          "start": "2025-12-01T12:00:00+00:00",
          "end": "2025-12-01T15:00:00+00:00",
          "value": "1.33",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T15:00:00+00:00",
          "end": "2025-12-01T18:00:00+00:00",
          "value": "1.00",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T18:00:00+00:00",
          "end": "2025-12-01T21:00:00+00:00",
          "value": "1.33",
          "colour": "green"
        },
        {
          "date": "2025-12-01T00:00:00+00:00",
          "now": false,
          "start": "2025-12-01T21:00:00+00:00",
          "end": "2025-12-02T00:00:00+00:00",
          "value": "1.33",
          "colour": "green"
        }
      ]
    ]
  },
  "probability": {
    "date": "2025-11-29T12:01:16+00:00",
    "calculated": {
      "lat": 58.7109,
      "long": 28.8281,
      "value": 3,
      "colour": "green"
    },
    "colour": "green",
    "lat": "50.06143",
    "long": "19.93658",
    "value": 0,
    "highest": {
      "date": "2025-11-29T12:01:16+00:00",
      "colour": "orange",
      "lat": 54.8438,
      "long": 80.5078,
      "value": 53
    }
  },
  "message": [
    "Due to changes to data from the Space Weather Prediction Centre, the 1 hour and 4 hour Kp forecasts are unavailable and have been replaced with the current Kp. This issue will be resolved when alternate data is available"
  ]
}
 */