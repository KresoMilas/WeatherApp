import { useState, useEffect } from 'react'

type CurrentWeather = {
  cityName: string
  country: string
  temperature: number
  feelsLike: number
  weatherDescription: string
  humidity: number
  windSpeed: number
}

export default function WeatherPage() {
  const [weather, setWeather] = useState<CurrentWeather | null>(null)
  const [error, setError] = useState('')

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(
      async (pos) => {
        const lat = pos.coords.latitude
        const lon = pos.coords.longitude
        const token = localStorage.getItem('token')

        const res = await fetch(
          `http://localhost:5069/api/weather/current?lat=${lat}&lon=${lon}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        )

        if (res.ok) {
          const data = await res.json()
          setWeather(data)
        } else {
          setError('Failed to load weather')
        }
      },
      () => setError('Location access denied')
    )
  }, [])

  if (error) return <p>{error}</p>
  if (!weather) return <p>Loading...</p>

  return (
    <div className="weather-card">
      <h2>{weather.cityName}, {weather.country}</h2>
      <p className="temp">{Math.round(weather.temperature)}°C</p>
      <p className="description">{weather.weatherDescription}</p>
      <div className="weather-details">
        <span>Feels like {Math.round(weather.feelsLike)}°C</span>
        <span>Humidity {weather.humidity}%</span>
        <span>Wind {weather.windSpeed} m/s</span>
      </div>
    </div>
  )
}