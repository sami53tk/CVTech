import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

export default function Connexion() {
  const { connexion, utilisateur } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [motDePasse, setMotDePasse] = useState('');
  const [erreur, setErreur] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErreur('');
    try {
      await connexion(email, motDePasse);
      navigate('/');
    } catch (err: unknown) {
      setErreur(err instanceof Error ? err.message : 'Erreur de connexion');
    }
  };

  if (utilisateur) navigate('/');

  return (
    <div style={{ maxWidth: 400, margin: '4rem auto', padding: '2rem', border: '1px solid #333', borderRadius: 8 }}>
      <h2>Connexion</h2>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
        <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} required style={{ padding: '0.5rem' }} />
        <input type="password" placeholder="Mot de passe" value={motDePasse} onChange={e => setMotDePasse(e.target.value)} required style={{ padding: '0.5rem' }} />
        {erreur && <p style={{ color: 'red' }}>{erreur}</p>}
        <button type="submit" style={{ padding: '0.5rem', background: '#1a1a2e', color: 'white', border: 'none', cursor: 'pointer', borderRadius: 4 }}>Se connecter</button>
      </form>
      <p style={{ marginTop: '1rem', fontSize: '0.85rem', color: '#aaa' }}>
        Comptes de démonstration :<br />
        admin@cvtech.fr / Admin123!<br />
        (ou inscrivez-vous ci-dessus)
      </p>
    </div>
  );
}
