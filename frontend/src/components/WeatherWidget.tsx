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

export default function WeatherWidget() {
  const [weather, setWeather] = useState<CurrentWeather | null>(null)

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(async (pos) => {
      const token = localStorage.getItem('token')
      const res = await fetch(
        `http://localhost:5069/api/weather/current?lat=${pos.coords.latitude}&lon=${pos.coords.longitude}`,
        { headers: { Authorization: `Bearer ${token}` } }
      )
      if (res.ok) setWeather(await res.json())
    })
  }, [])

  if (!weather) return null

  return (
    <div className="weather-widget">
      <span className="widget-location">{weather.cityName}</span>
      <span className="widget-temp">{Math.round(weather.temperature)}°C</span>
      <span className="widget-desc">{weather.weatherDescription}</span>
      <span className="widget-detail">Feels {Math.round(weather.feelsLike)}°C</span>
      <span className="widget-detail">💧 {weather.humidity}%</span>
      <span className="widget-detail">💨 {weather.windSpeed} m/s</span>
    </div>
  )
}
