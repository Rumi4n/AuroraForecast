import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL || 'https://aurora-forecast-api-bqgbargcerfbamfx.centralus-01.azurewebsites.net';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add request interceptor for debugging
api.interceptors.request.use(
  (config) => {
    console.log('API Request:', config.method?.toUpperCase(), config.url);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add response interceptor for error handling
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.data || error.message);
    return Promise.reject(error);
  }
);

export interface User {
  id: number;
  name: string;
}

export interface Location {
  id: number;
  userId: number;
  name: string;
  latitude: number;
  longitude: number;
}

export interface AuroraForecast {
  location: Location;
  dayForecasts: AuroraDayForecast[];
}

export interface AuroraDayForecast {
  date: Date;
  periodForecasts: AuroraPeriodForecast[];
}

export interface AuroraPeriodForecast {
  start: Date;
  end: Date;
  kp: number;
  colour: string;
}

export const userService = {
  getUserById: async (id: number): Promise<User> => {
    const response = await api.get<User>(`/users/${id}`);
    return response.data;
  },
};

export const locationService = {
  getUserLocationsAsync: async (userId: number): Promise<Location[]> => {
    const response = await api.get<Location[]>(`/location/user/${userId}`);
    return response.data;
  },
  
  addLocationAsync: async (location: Location): Promise<Location> => {
    const response = await api.put<Location>(`/location`, location);
    return response.data;
  },

  deleteLocationAsync: async (locationId: number): Promise<void> => {
    await api.delete<void>(`/location/${locationId}`);
  },
};

export const auroraForecastService = {
    getForecastsByUserId: async (userId: number): Promise<AuroraForecast[]> => {
        const response = await api.get<AuroraForecast[]>(`/aurora/forecast/user/${userId}`);
        console.log('Raw API Response:', response.data);
        return response.data;
    },
};

export default api;