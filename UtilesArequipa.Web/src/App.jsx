import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import LoginPage from './pages/LoginPage';
import CatalogPage from './pages/CatalogPage';

const ProtectedRoute = ({ children }) => {
    const { isAuthenticated } = useAuth();
    if (!isAuthenticated) {
        return <Navigate to="/login" />;
    }
    return children;
};

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/catalog" element={
            <ProtectedRoute>
              <CatalogPage />
            </ProtectedRoute>
          } />
          <Route path="/" element={<Navigate to="/catalog" />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
