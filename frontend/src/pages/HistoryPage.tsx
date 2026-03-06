import { useState, useEffect } from 'react'

type HistoryItem = {
  city: string
  weatherDescription: string
  searchDateTime: string
}

export default function HistoryPage() {
  const [history, setHistory] = useState<HistoryItem[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const token = localStorage.getItem('token')

    fetch('http://localhost:5069/api/searchhistory', {
      headers: { Authorization: `Bearer ${token}` },
    })
      .then(res => res.json())
      .then(data => setHistory(data))
      .catch(() => setError('Failed to load history'))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <p>Loading...</p>
  if (error) return <p>{error}</p>

  return (
    <div>
      <h2>Search History</h2>
      {history.length === 0 ? (
        <p>No searches yet.</p>
      ) : (
        <table className="data-table">
          <thead>
            <tr>
              <th>City</th>
              <th>Condition</th>
              <th>Date</th>
            </tr>
          </thead>
          <tbody>
            {history.map((item, i) => (
              <tr key={i}>
                <td>{item.city}</td>
                <td>{item.weatherDescription}</td>
                <td>{new Date(item.searchDateTime).toLocaleString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}