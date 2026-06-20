import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Navigation() {
  const { utilisateur, deconnexion } = useAuth();
  const navigate = useNavigate();

  const handleDeconnexion = () => {
    deconnexion();
    navigate('/connexion');
  };

  return (
    <nav style={{ background: '#1a1a2e', padding: '0.75rem 1.5rem', display: 'flex', gap: '1rem', alignItems: 'center', color: 'white' }}>
      <Link to="/" style={{ color: 'white', fontWeight: 'bold', textDecoration: 'none', fontSize: '1.2rem' }}>CVTech</Link>
      <Link to="/annonces" style={{ color: '#ccc', textDecoration: 'none' }}>Annonces</Link>
      <Link to="/appels-offres" style={{ color: '#ccc', textDecoration: 'none' }}>Appels d'offres</Link>
      <Link to="/actualites" style={{ color: '#ccc', textDecoration: 'none' }}>Actualités</Link>
      <span style={{ flex: 1 }} />
      {utilisateur ? (
        <>
          <span style={{ color: '#aaa', fontSize: '0.85rem' }}>{utilisateur.role}</span>
          {utilisateur.role === 'Candidat' && <Link to="/espace-candidat" style={{ color: '#4fc3f7', textDecoration: 'none' }}>Mon espace</Link>}
          {utilisateur.role === 'Entreprise' && <Link to="/espace-entreprise" style={{ color: '#4fc3f7', textDecoration: 'none' }}>Mon espace</Link>}
          {utilisateur.role === 'Administrateur' && <Link to="/espace-admin" style={{ color: '#4fc3f7', textDecoration: 'none' }}>Administration</Link>}
          <button onClick={handleDeconnexion} style={{ background: 'transparent', border: '1px solid #aaa', color: '#aaa', padding: '0.25rem 0.75rem', cursor: 'pointer', borderRadius: 4 }}>Déconnexion</button>
        </>
      ) : (
        <>
          <Link to="/connexion" style={{ color: '#4fc3f7', textDecoration: 'none' }}>Connexion</Link>
          <Link to="/inscription" style={{ color: '#4fc3f7', textDecoration: 'none' }}>Inscription</Link>
        </>
      )}
    </nav>
  );
}
