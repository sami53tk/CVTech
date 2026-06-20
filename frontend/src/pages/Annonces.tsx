import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { api } from '../api/client';
import { useAuth } from '../context/AuthContext';

interface Annonce {
  id: string;
  titre: string;
  typeContrat: string;
  domaineMetierCode: string;
  localisation: string;
  datePublication: string;
}

export default function Annonces() {
  const [annonces, setAnnonces] = useState<Annonce[]>([]);
  const [domaine, setDomaine] = useState('');
  const [erreur, setErreur] = useState('');
  const { utilisateur } = useAuth();
  const navigate = useNavigate();

  const charger = async (d?: string) => {
    try {
      const url = d ? `/api/annonces?domaine=${d}` : '/api/annonces';
      const data = await api.get<Annonce[]>(url);
      setAnnonces(data);
    } catch (err: unknown) {
      setErreur(err instanceof Error ? err.message : 'Erreur');
    }
  };

  useEffect(() => { charger(); }, []);

  return (
    <div style={{ maxWidth: 800, margin: '2rem auto', padding: '0 1rem' }}>
      <h2>Annonces d'emploi</h2>
      <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '1rem' }}>
        <input placeholder="Filtrer par domaine (ex: dev-web)" value={domaine}
          onChange={e => setDomaine(e.target.value)}
          style={{ padding: '0.5rem', flex: 1 }} />
        <button onClick={() => charger(domaine)} style={{ padding: '0.5rem 1rem' }}>Filtrer</button>
      </div>
      {erreur && <p style={{ color: 'red' }}>{erreur}</p>}
      {annonces.length === 0 && <p style={{ color: '#aaa' }}>Aucune annonce.</p>}
      {annonces.map(a => (
        <div key={a.id} style={{ border: '1px solid #333', borderRadius: 8, padding: '1rem', marginBottom: '0.75rem' }}>
          <h3 style={{ margin: 0 }}>{a.titre}</h3>
          <p style={{ color: '#aaa', margin: '0.25rem 0' }}>{a.domaineMetierCode} · {a.localisation} · {a.typeContrat}</p>
          <p style={{ color: '#666', fontSize: '0.8rem' }}>{new Date(a.datePublication).toLocaleDateString('fr-FR')}</p>
          {utilisateur?.role === 'Candidat' && (
            <button onClick={() => navigate(`/annonces/${a.id}/postuler`)}
              style={{ marginTop: '0.5rem', padding: '0.25rem 0.75rem', cursor: 'pointer' }}>
              Postuler
            </button>
          )}
        </div>
      ))}
    </div>
  );
}
