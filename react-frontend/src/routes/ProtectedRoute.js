import { Navigate } from 'react-router-dom';
import { jwtDecode } from 'jwt-decode';

function ProtectedRoute({ children, allowedRole }) {
  const token = localStorage.getItem('token');

  if (!token) {
    return <Navigate to="/" replace />;
  }

  try {
    const decodedToken = jwtDecode(token);
    const tokenRole = decodedToken.role || decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (tokenRole !== allowedRole) {
      return <Navigate to="/" replace />;
    }

    return children;
  } catch {
    localStorage.removeItem('token');
    return <Navigate to="/" replace />;
  }
}

export default ProtectedRoute;
