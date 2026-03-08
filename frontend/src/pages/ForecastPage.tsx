import { useState } from 'react'
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts'
import { ReferenceLine } from 'recharts'

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
  const [selectedDayFrom, setSelectedDayFrom] = useState('')
  const [selectedDayTo, setSelectedDayTo] = useState('')
  const [selectedCondition, setSelectedCondition] = useState('All')

  async function handleSearch(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    setLoading(true)
    setForecast(null)

    const token = localStorage.getItem('token')
    const res = await fetch(
      `http://localhost:5069/api/weather/forecast?city=${encodeURIComponent(city)}`,
      { headers: { Authorization: `Bearer ${token}` } }
    )

    if (res.ok) {
      const data = await res.json()
      setForecast(data)
      setSelectedDayFrom('')
      setSelectedDayTo('')
      setSelectedCondition('All')
    } else {
      setError('City not found')
    }
    setLoading(false)
  }

  const dayLabels: string[] = []
  if (forecast) {
    forecast.fiveDayForecastWeather.forEach(item => {
      const label = new Date(item.dateTime).toLocaleDateString('en-US', { weekday: 'long', day: 'numeric', month: 'short' })
      if (!dayLabels.includes(label)) dayLabels.push(label)
    })
  }

  const conditionLabels: string[] = []
  if (forecast) {
    forecast.fiveDayForecastWeather.forEach(item => {
      const condition = item.weatherDescription
      if (!conditionLabels.includes(condition)) conditionLabels.push(condition)
    })
    conditionLabels.sort()
  }

  const filteredItems = forecast && dayLabels.length > 0
    ? forecast.fiveDayForecastWeather.filter(item => {
        const label = new Date(item.dateTime).toLocaleDateString('en-US', { weekday: 'long', day: 'numeric', month: 'short' })
        if (!selectedDayFrom || !selectedDayTo) return true
        const fromIdx = dayLabels.indexOf(selectedDayFrom)
        const toIdx = dayLabels.indexOf(selectedDayTo)
        const idx = dayLabels.indexOf(label)
        return idx >= fromIdx && idx <= toIdx
      })
    : []

  const isConditionMatch = (slot: ForecastItem | undefined) =>
    selectedCondition === 'All' || (slot && slot.weatherDescription === selectedCondition)

  const days: { label: string; night?: ForecastItem; morning?: ForecastItem; afternoon?: ForecastItem; evening?: ForecastItem; min: number; max: number; rain: number; wind: number }[] = []
  if (filteredItems.length > 0) {
    const grouped: Record<string, ForecastItem[]> = {}
    filteredItems.forEach(item => {
      const date = new Date(item.dateTime)
      const key = date.toLocaleDateString('en-US', { weekday: 'long', day: 'numeric', month: 'short' })
      if (!grouped[key]) grouped[key] = []
      grouped[key].push(item)
    })
    for (const [label, items] of Object.entries(grouped)) {
      const pick = (from: number, to: number) => items.find(i => { const h = new Date(i.dateTime).getHours(); return h >= from && h < to })
      days.push({
        label,
        night: pick(0, 6),
        morning: pick(6, 12),
        afternoon: pick(12, 18),
        evening: pick(18, 24),
        min: Math.round(Math.min(...items.map(i => i.temperatureMin))),
        max: Math.round(Math.max(...items.map(i => i.temperatureMax))),
        rain: Math.round(Math.max(...items.map(i => i.chanceOfPrecipitation)) * 100),
        wind: Math.round(Math.max(...items.map(i => i.windSpeed)))
      })
    }
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

          <div className="forecast-filters">
            <label>
              Days from:
              <select value={selectedDayFrom} onChange={e => setSelectedDayFrom(e.target.value)}>
                <option value="">All</option>
                {dayLabels.map(d => <option key={d} value={d}>{d}</option>)}
              </select>
            </label>
            <label>
              to:
              <select value={selectedDayTo} onChange={e => setSelectedDayTo(e.target.value)}>
                <option value="">All</option>
                {dayLabels.map(d => <option key={d} value={d}>{d}</option>)}
              </select>
            </label>
            <label>
              Filter by condition:
              <select value={selectedCondition} onChange={e => setSelectedCondition(e.target.value)}>
                <option value="All">All conditions</option>
                {conditionLabels.map(c => <option key={c} value={c}>{c}</option>)}
              </select>
            </label>
          </div>

          <div className="forecast-table-wrapper">
            <table className="forecast-table">
              <thead>
                <tr>
                  <th>Day</th>
                  <th>Night</th>
                  <th>Morning</th>
                  <th>Afternoon</th>
                  <th>Evening</th>
                  <th>Temp</th>
                  <th>Rain</th>
                  <th>Wind</th>
                </tr>
              </thead>
              <tbody>
                {days.map(day => (
                  <tr key={day.label}>
                    <td className="yr-day-label">{day.label}</td>
                    {([day.night, day.morning, day.afternoon, day.evening] as (ForecastItem | undefined)[]).map((slot, i) => {
                      const match = isConditionMatch(slot)
                      const dimmed = selectedCondition !== 'All' && !match
                      const highlighted = selectedCondition !== 'All' && match
                      return (
                        <td key={i} className={`yr-slot${dimmed ? ' yr-slot-dimmed' : ''}${highlighted ? ' yr-slot-highlighted' : ''}`}>
                          {slot ? (
                            <>
                              <img src={`https://openweathermap.org/img/wn/${slot.weatherIcon}.png`} alt={slot.weatherDescription} />
                              <span className="yr-slot-desc">{slot.weatherDescription}</span>
                            </>
                          ) : <span className="yr-slot-empty">—</span>}
                        </td>
                      )
                    })}
                    <td className="yr-temp">{day.max}° / {day.min}°</td>
                    <td className="yr-rain">{day.rain > 0 ? `${day.rain}%` : ''}</td>
                    <td className="yr-wind">{day.wind} m/s</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="forecast-chart">
            <h4 style={{ marginBottom: '0.5rem', color: 'var(--text-muted)' }}>Temperature Over Time</h4>
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={filteredItems.map(item => ({
                time: new Date(item.dateTime).toLocaleDateString('en-US', {
                  weekday: 'short', hour: '2-digit', minute: '2-digit'
                }),
                temp: Math.round(item.temperature),
                rain: Math.round(item.chanceOfPrecipitation * 100),
                isMatch: selectedCondition === 'All' || item.weatherDescription === selectedCondition
              }))}>
                <CartesianGrid strokeDasharray="3 3" stroke="var(--border)" />
                <XAxis dataKey="time" tick={{ fontSize: 11 }} angle={-45} textAnchor="end" height={70} />
                <YAxis tick={{ fontSize: 12 }} unit="°C" />
                <Tooltip />
                {selectedCondition !== 'All' && filteredItems.map(item => (
                  item.weatherDescription === selectedCondition ? (
                    <ReferenceLine key={item.dateTime} x={new Date(item.dateTime).toLocaleDateString('en-US', {
                      weekday: 'short', hour: '2-digit', minute: '2-digit'
                    })} stroke="#339af0" strokeWidth={3} opacity={0.15} />
                  ) : null
                ))}
                <Line
                  type="monotone"
                  dataKey="temp"
                  stroke="var(--accent)"
                  strokeWidth={2}
                  dot={({ cx, cy, payload }) => {
                    if (selectedCondition === 'All') {
                      return <circle cx={cx} cy={cy} r={3} fill="var(--accent)" />
                    }
                    return <circle cx={cx} cy={cy} r={payload.isMatch ? 5 : 3} fill={payload.isMatch ? '#339af0' : '#bbb'} opacity={payload.isMatch ? 1 : 0.3} />
                  }}
                  name="Temp (°C)"
                />
                <Line type="monotone" dataKey="rain" stroke="#339af0" strokeWidth={1.5} dot={{ r: 2 }} name="Rain (%)" />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </div>
      )}
    </div>
  )
}