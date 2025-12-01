import React, { useState, useEffect } from 'react';
import { auroraForecastService, AuroraForecast, userService, User, locationService, Location } from '../services/api';
import './Forecast.css';

export const Forecast: React.FC = () => {
  const [forecasts, setForecasts] = useState<AuroraForecast[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [userId, setUserId] = useState<number>(10);
  const [userInfo, setUserInfo] = useState<User | null>(null);
  const [addLocation, setAddLocation] = useState(false);
  const periodsStart = ["1 AM", "4 AM", "7 AM", "10 AM", "1 PM", "4 PM", "7 PM", "10 PM"];

  const fetchForecast = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await auroraForecastService.getForecastsByUserId(userId);
      console.log('Fetched forecasts:', data);
      setForecasts(data);
    } catch (err: any) {
      if (err.response?.status === 404) {
        setError(`No forecasts found for user ID ${userId}`);
      } else if (err.response?.status === 503) {
        setError('Unable to fetch aurora data from external service. Please try again later.');
      } else {
        setError('Failed to fetch forecasts. Check console for details.');
      }
      console.error('Fetch error:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchUserInfo = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await userService.getUserById(userId);
      console.log('Fetched user info:', data);
      setUserInfo(data);
      await fetchForecast();
    } catch (err: any) {
      setError('Failed to fetch user info. Check console for details.');
    } finally {
      setLoading(false);
    }
  };

  const saveLocation = async () => {
    setLoading(true);
    setError(null);
    try {
      const location: Location = {  
        id: 0,
        name: (document.getElementById('location-name') as HTMLInputElement).value,
        latitude: parseFloat((document.getElementById('location-latitude') as HTMLInputElement).value),
        longitude: parseFloat((document.getElementById('location-longitude') as HTMLInputElement).value),
        userId: userId,
      };
      await locationService.addLocationAsync(location);
      setAddLocation(false);
      await fetchForecast();
    } catch (err: any) {
      setError('Failed to add location. Check console for details.');
    }
  };

  const deleteLocation = async (locationId: number) => {
    setLoading(true);
    setError(null);
    try {
      await locationService.deleteLocationAsync(locationId);
      await fetchForecast();
    } catch (err: any) {
      setError('Failed to delete location. Check console for details.');
    } finally {
      setLoading(false);
    }
  };

  const convertDate = (dateString: Date) => {
    var date = new Date(dateString);
    return date.getMonth() + 1 + '/' + date.getDate() + '/' + date.getFullYear();
  };

  useEffect(() => {
    fetchUserInfo();
  }, []);

  return (
    <div className="forecast-display">

      {loading && <div className="loading">Loading forecast data...</div>}
      
      {error && (
        <div className="error">
          <strong>Error:</strong> {error}
        </div>
      )}
      
      {forecasts.length > 0 && !loading && (
        <div className="forecast-container">
          
          <div className="controls">
            <div className="user-info">
              <label>User: {userInfo?.name}</label>
            </div>
          </div>
          <div className="forecast-all-locations">
            {forecasts.map((locationForecast) => (
              <table className="forecast-table">
                <thead className="forecast-table-header">
                  <tr>
                    <th colSpan={9}>
                      {locationForecast.location.name} ({locationForecast.location.latitude.toFixed(4)}°, {locationForecast.location.longitude.toFixed(4)}°)
                    </th>
                  </tr>
                </thead>
                <tbody>
                  <tr className="forecast-table-row">
                    <td className="forecast-table-cell">Date</td>
                    {periodsStart.map((period) => (
                      <td className="forecast-table-cell">{period}</td>
                    ))}
                  </tr>
                  {locationForecast.dayForecasts.map((dayForecast) => (
                    <tr className="forecast-table-row">
                      <td className="forecast-table-cell">{convertDate(dayForecast.date)}</td>
                      {dayForecast.periodForecasts.map((periodForecast) => (
                        <td className={`forecast-table-cell kp-index-${periodForecast.colour}`}>{periodForecast.kp}</td>
                      ))}
                    </tr>
                  ))}
                </tbody>
              </table>
            ))}
          </div>
          <div className="forecast-container-footer">
            {addLocation && (
              <div className="add-location-container">
                <label>Location name: </label>
                <input id="location-name" type="text"/>
                <label>Latitude: </label>
                <input id="location-latitude" type="text"/>
                <label>Longitude: </label>
                <input id="location-longitude" type="text"/>
                <button onClick={saveLocation} disabled={loading} className="btn-primary">Add Location</button>
              </div>
            )} 
            {!addLocation && (
              <div className="add-location-button">
                <button onClick={() => setAddLocation(true)} disabled={loading} className="btn-primary">Add Location</button>
              </div>
            )}
          </div>
        </div>
      )}

      {forecasts.length === 0 && !loading && !error && (
        <div className="no-data">
          <p>No forecasts available for user ID {userId}.</p>
          <p>Make sure the user has locations configured.</p>
        </div>
      )}
    </div>
  );
};