import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import WeatherPage from './pages/WeatherPage'
import ProtectedRoute from './components/ProtectedRoute'
import Navbar from './components/Navbar'
import HistoryPage from './pages/HistoryPage'
import StatsPage from './pages/StatsPage'


function App() {
  return (
    <BrowserRouter>
      <Navbar />
        <Routes>
        <Route path="/" element={<Navigate to="/login" replace />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/weather" element={<ProtectedRoute><WeatherPage /></ProtectedRoute>} />
        <Route path="/history" element={<ProtectedRoute><HistoryPage /></ProtectedRoute>} />
        <Route path="/stats" element={<ProtectedRoute><StatsPage /></ProtectedRoute>} />
      </Routes>
    </BrowserRouter>
  )
}

export default App