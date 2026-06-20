import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import Navigation from './components/Navigation';
import Connexion from './pages/auth/Connexion';
import Inscription from './pages/auth/Inscription';
import Annonces from './pages/Annonces';
import AppelsOffres from './pages/AppelsOffres';
import Actualites from './pages/Actualites';
import EspaceCandidat from './pages/candidat/EspaceCandidat';
import EspaceEntreprise from './pages/entreprise/EspaceEntreprise';
import EspaceAdmin from './pages/admin/EspaceAdmin';

function RouteProtegee({ children, roles }: { children: React.ReactNode; roles: string[] }) {
  const { utilisateur } = useAuth();
  if (!utilisateur) return <Navigate to="/connexion" replace />;
  if (!roles.includes(utilisateur.role)) return <Navigate to="/" replace />;
  return <>{children}</>;
}

function AppInterne() {
  return (
    <div style={{ minHeight: '100vh', background: '#0d0d0d', color: '#e0e0e0' }}>
      <Navigation />
      <Routes>
        <Route path="/" element={<Annonces />} />
        <Route path="/annonces" element={<Annonces />} />
        <Route path="/appels-offres" element={<AppelsOffres />} />
        <Route path="/actualites" element={<Actualites />} />
        <Route path="/connexion" element={<Connexion />} />
        <Route path="/inscription" element={<Inscription />} />
        <Route path="/espace-candidat" element={
          <RouteProtegee roles={['Candidat']}>
            <EspaceCandidat />
          </RouteProtegee>
        } />
        <Route path="/espace-entreprise" element={
          <RouteProtegee roles={['Entreprise']}>
            <EspaceEntreprise />
          </RouteProtegee>
        } />
        <Route path="/espace-admin" element={
          <RouteProtegee roles={['Administrateur']}>
            <EspaceAdmin />
          </RouteProtegee>
        } />
      </Routes>
    </div>
  );
}

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <AppInterne />
      </AuthProvider>
    </BrowserRouter>
  );
}
