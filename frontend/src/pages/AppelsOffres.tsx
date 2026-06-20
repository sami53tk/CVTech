import { useEffect, useState } from 'react';
import { api } from '../api/client';
import { useAuth } from '../context/AuthContext';

interface AppelOffre {
  id: string;
  titre: string;
  domaineMetierCode: string;
  localisation: string;
  budgetMax: number;
  datePublication: string;
  laureatSelectionne: boolean;
}

export default function AppelsOffres() {
  const [appels, setAppels] = useState<AppelOffre[]>([]);
  const [erreur, setErreur] = useState('');
  const [propositionForm, setPropositionForm] = useState<{ aoId: string } | null>(null);
  const [description, setDescription] = useState('');
  const [tjm, setTjm] = useState('');
  const [duree, setDuree] = useState('');
  const { utilisateur } = useAuth();

  useEffect(() => {
    api.get<AppelOffre[]>('/api/appels-offres').then(setAppels).catch((e: Error) => setErreur(e.message));
  }, []);

  const soumettre = async () => {
    if (!propositionForm) return;
    try {
      await api.post(`/api/appels-offres/${propositionForm.aoId}/propositions`, {
        description, tauxJournalier: parseFloat(tjm), dureeEstimeeJours: parseInt(duree)
      });
      setPropositionForm(null);
      alert('Proposition soumise !');
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  return (
    <div style={{ maxWidth: 800, margin: '2rem auto', padding: '0 1rem' }}>
      <h2>Appels d'offres freelance</h2>
      {erreur && <p style={{ color: 'red' }}>{erreur}</p>}
      {appels.length === 0 && <p style={{ color: '#aaa' }}>Aucun appel d'offre.</p>}
      {appels.map(ao => (
        <div key={ao.id} style={{ border: '1px solid #333', borderRadius: 8, padding: '1rem', marginBottom: '0.75rem' }}>
          <h3 style={{ margin: 0 }}>{ao.titre} {ao.laureatSelectionne && <span style={{ color: 'green', fontSize: '0.8rem' }}>✓ Attribué</span>}</h3>
          <p style={{ color: '#aaa', margin: '0.25rem 0' }}>{ao.domaineMetierCode} · {ao.localisation} · Budget max {ao.budgetMax}€/j</p>
          {utilisateur?.role === 'Candidat' && !ao.laureatSelectionne && (
            <button onClick={() => setPropositionForm({ aoId: ao.id })}
              style={{ marginTop: '0.5rem', padding: '0.25rem 0.75rem', cursor: 'pointer' }}>
              Soumettre une proposition
            </button>
          )}
        </div>
      ))}
      {propositionForm && (
        <div style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.6)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
          <div style={{ background: '#1a1a1a', padding: '2rem', borderRadius: 8, width: 380 }}>
            <h3>Soumettre une proposition</h3>
            <textarea placeholder="Description de votre proposition" value={description} onChange={e => setDescription(e.target.value)} rows={3} style={{ width: '100%', padding: '0.5rem', marginBottom: '0.5rem' }} />
            <input type="number" placeholder="TJM (€)" value={tjm} onChange={e => setTjm(e.target.value)} style={{ width: '100%', padding: '0.5rem', marginBottom: '0.5rem' }} />
            <input type="number" placeholder="Durée estimée (jours)" value={duree} onChange={e => setDuree(e.target.value)} style={{ width: '100%', padding: '0.5rem', marginBottom: '1rem' }} />
            <div style={{ display: 'flex', gap: '0.5rem' }}>
              <button onClick={soumettre} style={{ flex: 1, padding: '0.5rem', cursor: 'pointer' }}>Envoyer</button>
              <button onClick={() => setPropositionForm(null)} style={{ flex: 1, padding: '0.5rem', cursor: 'pointer' }}>Annuler</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
