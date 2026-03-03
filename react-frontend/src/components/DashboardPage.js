import { useEffect, useState } from 'react';
import { Button, Card } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

function DashboardPage() {
  const [username, setUsername] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const loadProfile = async () => {
      try {
        const response = await api.get('/auth/profile');
        setUsername(response.data.username);
      } catch {
        localStorage.removeItem('token');
        navigate('/login');
      }
    };

    loadProfile();
  }, [navigate]);

  const logout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div className="auth-page">
      <Card className="auth-card shadow text-center">
        <Card.Body>
          <h2 className="text-primary mb-3">Dashboard</h2>
          <p className="mb-4">Welcome, <strong>{username || 'User'}</strong>!</p>
          <Button variant="primary" onClick={logout}>Logout</Button>
        </Card.Body>
      </Card>
    </div>
  );
}

export default DashboardPage;
