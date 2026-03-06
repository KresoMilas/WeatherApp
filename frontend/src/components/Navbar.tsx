import { Link, useNavigate } from 'react-router-dom'

export default function Navbar() {
  const navigate = useNavigate()
  const token = localStorage.getItem('token')
  const email = localStorage.getItem('email')

  function handleLogout() {
    localStorage.removeItem('token')
    localStorage.removeItem('email')
    navigate('/login')
  }

  return (
    <nav>
      {token ? (
        <>
          <Link to="/weather">Weather</Link>
          <Link to="/history">History</Link>
          <Link to="/stats">Stats</Link>
          <Link to="/forecast">Forecast</Link>
          <span>{email}</span>
          <button onClick={handleLogout}>Logout</button>
        </>
      ) : (
        <>
          <Link to="/login">Login</Link>
          <Link to="/register">Register</Link>
        </>
      )}
    </nav>
  )
}