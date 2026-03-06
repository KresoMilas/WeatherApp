import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import './App.css'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import ProtectedRoute from './components/ProtectedRoute'
import Navbar from './components/Navbar'
import HistoryPage from './pages/HistoryPage'
import StatsPage from './pages/StatsPage'
import ForecastPage from './pages/ForecastPage'


function App() {
  return (
    <BrowserRouter>
      <Navbar />
      <main>
        <Routes>
        <Route path="/" element={<Navigate to="/forecast" replace />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/history" element={<ProtectedRoute><HistoryPage /></ProtectedRoute>} />
        <Route path="/stats" element={<ProtectedRoute><StatsPage /></ProtectedRoute>} />
        <Route path="/forecast" element={<ProtectedRoute><ForecastPage /></ProtectedRoute>} />
        </Routes>
      </main>
    </BrowserRouter>
  )
}

export default App