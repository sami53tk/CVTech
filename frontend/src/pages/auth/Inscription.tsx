import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { api } from '../../api/client';

const ROLES = [
  { label: 'Candidat / Freelance', value: 1 },
  { label: 'Entreprise', value: 2 },
];

export default function Inscription() {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [motDePasse, setMotDePasse] = useState('');
  const [role, setRole] = useState(1);
  const [nom, setNom] = useState('');
  const [raisonSociale, setRaisonSociale] = useState('');
  const [erreur, setErreur] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErreur('');
    try {
      await api.post('/api/auth/inscription', {
        email, motDePasse, role,
        nom: role === 1 ? nom : undefined,
        raisonSociale: role === 2 ? raisonSociale : undefined,
      });
      navigate('/connexion');
    } catch (err: unknown) {
      setErreur(err instanceof Error ? err.message : "Erreur d'inscription");
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: '4rem auto', padding: '2rem', border: '1px solid #333', borderRadius: 8 }}>
      <h2>Inscription</h2>
      <form onSubmit={handleSubmit} style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
        <input type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)} required style={{ padding: '0.5rem' }} />
        <input type="password" placeholder="Mot de passe" value={motDePasse} onChange={e => setMotDePasse(e.target.value)} required style={{ padding: '0.5rem' }} />
        <select value={role} onChange={e => setRole(Number(e.target.value))} style={{ padding: '0.5rem' }}>
          {ROLES.map(r => <option key={r.value} value={r.value}>{r.label}</option>)}
        </select>
        {role === 1 && <input placeholder="Nom" value={nom} onChange={e => setNom(e.target.value)} style={{ padding: '0.5rem' }} />}
        {role === 2 && <input placeholder="Raison sociale" value={raisonSociale} onChange={e => setRaisonSociale(e.target.value)} style={{ padding: '0.5rem' }} />}
        {erreur && <p style={{ color: 'red' }}>{erreur}</p>}
        <button type="submit" style={{ padding: '0.5rem', background: '#1a1a2e', color: 'white', border: 'none', cursor: 'pointer', borderRadius: 4 }}>S'inscrire</button>
      </form>
    </div>
  );
}
