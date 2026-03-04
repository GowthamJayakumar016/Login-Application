import { useNavigate } from 'react-router-dom';

function AdminDashboard() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
  };

  return (
    <div className="auth-wrapper">
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-12 col-md-8 col-lg-6">
            <div className="card shadow-lg border-0 auth-card">
              <div className="card-body p-4 p-md-5 text-center">
                <h2 className="text-primary fw-bold mb-3">Admin Dashboard</h2>
                <p className="text-muted mb-4">Welcome Admin! You have full access.</p>
                <button onClick={handleLogout} className="btn btn-outline-primary">
                  Logout
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default AdminDashboard;
