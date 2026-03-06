import { useState } from 'react'

type ForecastItem = {
  dateTime: string
  temperature: number
  temperatureMin: number
  temperatureMax: number
  weatherDescription: string
  weatherIcon: string
  chanceOfPrecipitation: number
  humidity: number
  windSpeed: number
}

type Forecast = {
  cityName: string
  country: string
  fiveDayForecastWeather: ForecastItem[]
}

export default function ForecastPage() {
  const [city, setCity] = useState('')
  const [forecast, setForecast] = useState<Forecast | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    setLoading(true)
    setForecast(null)

    const res = await fetch(
      `http://localhost:5069/api/weather/forecast?city=${encodeURIComponent(city)}`
    )

    if (res.ok) {
      const data = await res.json()
      setForecast(data)
    } else {
      setError('City not found')
    }
    setLoading(false)
  }

  const days: string[] = []
  const times: string[] = []
  const lookup: Record<string, Record<string, ForecastItem>> = {}
  if (forecast) {
    forecast.fiveDayForecastWeather.forEach(item => {
      const date = new Date(item.dateTime)
      const day = date.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric' })
      const timeKey = date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' })
      if (!days.includes(day)) days.push(day)
      if (!times.includes(timeKey)) times.push(timeKey)
      if (!lookup[timeKey]) lookup[timeKey] = {}
      lookup[timeKey][day] = item
    })
  }

  return (
    <div>
      <h2>5-Day Forecast</h2>
      <form onSubmit={handleSearch} style={{ display: 'flex', gap: '0.75rem', marginBottom: '1.5rem' }}>
        <input
          type="text"
          placeholder="Enter city name…"
          value={city}
          onChange={e => setCity(e.target.value)}
          style={{ maxWidth: '300px' }}
        />
        <button type="submit" className="btn-primary" disabled={loading || !city.trim()} style={{ width: 'auto', padding: '0.5rem 1rem', fontSize: '0.85rem' }}>
          {loading ? 'Loading…' : 'Search'}
        </button>
      </form>

      {error && <p className="error-msg">{error}</p>}

      {forecast && (
        <div>
          <h3>{forecast.cityName}, {forecast.country}</h3>
          <div className="forecast-table-wrapper">
            <table className="forecast-table">
              <thead>
                <tr>
                  <th className="forecast-time-header"></th>
                  {days.map(day => (
                    <th key={day} className="forecast-day-header">{day}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {times.map(time => (
                  <tr key={time}>
                    <td className="forecast-time-label">{time}</td>
                    {days.map(day => {
                      const item = lookup[time]?.[day]
                      return (
                        <td key={day} className="forecast-cell">
                          {item && (
                            <>
                              <img src={`https://openweathermap.org/img/wn/${item.weatherIcon}.png`} alt={item.weatherDescription} />
                              <p className="forecast-temp">{Math.round(item.temperature)}°C</p>
                              <p className="forecast-desc">{item.weatherDescription}</p>
                              <p className="forecast-rain">💧 {Math.round(item.chanceOfPrecipitation * 100)}%</p>
                            </>
                          )}
                        </td>
                      )
                    })}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  )
}