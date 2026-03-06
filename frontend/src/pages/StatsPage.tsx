import { useState, useEffect } from 'react'

type TopCity = {
  cityName: string
  searchCount: number
}

type RecentSearch = {
  cityName: string
  weatherCondition: string
  searchDateTime: string
}

type ConditionCount = {
  condition: string
  count: number
}

type Stats = {
  topCities: TopCity[]
  recentSearches: RecentSearch[]
  conditionDistribution: ConditionCount[]
}

export default function StatsPage() {
  const [stats, setStats] = useState<Stats | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const token = localStorage.getItem('token')

    fetch('http://localhost:5069/api/stats', {
      headers: { Authorization: `Bearer ${token}` },
    })
      .then(res => res.json())
      .then(data => setStats(data))
      .catch(() => setError('Failed to load stats'))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <p>Loading...</p>
  if (error) return <p>{error}</p>
  if (!stats) return null

  return (
    <div>
      <h2>Your Stats</h2>

      <h3>Top Cities</h3>
      <ul>
        {stats.topCities.map((c, i) => (
          <li key={i}>{c.cityName} — {c.searchCount} searches</li>
        ))}
      </ul>

      <h3>Recent Searches</h3>
      <ul>
        {stats.recentSearches.map((r, i) => (
          <li key={i}>
            {r.cityName} — {r.weatherCondition} —{' '}
            {new Date(r.searchDateTime).toLocaleString()}
          </li>
        ))}
      </ul>

      <h3>Weather Conditions</h3>
      <ul>
        {stats.conditionDistribution.map((c, i) => (
          <li key={i}>{c.condition} — {c.count}×</li>
        ))}
      </ul>
    </div>
  )
}