import { useEffect, useState } from 'react';
import { api } from '../../api/client';

interface Cv { titre: string; resume: string; competences: string[]; }
interface Abonnement { id: string; domaineMetierCode: string; }
interface Notification { id: string; message: string; domaineMetierCode: string; typeSource: string; estLue: boolean; dateCreation: string; }

export default function EspaceCandidat() {
  const [cv, setCv] = useState<Cv | null>(null);
  const [abonnements, setAbonnements] = useState<Abonnement[]>([]);
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [tab, setTab] = useState<'cv' | 'abonnements' | 'notifications'>('cv');
  const [cvForm, setCvForm] = useState({ titre: '', resume: '', competences: '' });
  const [newDomaine, setNewDomaine] = useState('');

  useEffect(() => {
    api.get<Cv>('/api/cv').then(setCv).catch(() => null);
    api.get<Abonnement[]>('/api/abonnements').then(setAbonnements).catch(() => null);
    api.get<Notification[]>('/api/notifications').then(setNotifications).catch(() => null);
  }, []);

  const sauvegarderCv = async () => {
    try {
      await api.put('/api/cv', {
        titre: cvForm.titre,
        resume: cvForm.resume,
        competences: cvForm.competences.split(',').map(s => s.trim()).filter(Boolean),
      });
      setCv({ titre: cvForm.titre, resume: cvForm.resume, competences: cvForm.competences.split(',').map(s => s.trim()).filter(Boolean) });
      alert('CV mis à jour !');
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  const sabonner = async () => {
    try {
      await api.post('/api/abonnements', { domaineMetierCode: newDomaine });
      const updated = await api.get<Abonnement[]>('/api/abonnements');
      setAbonnements(updated);
      setNewDomaine('');
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  const seDesabonner = async (id: string) => {
    await api.delete(`/api/abonnements/${id}`);
    setAbonnements(prev => prev.filter(a => a.id !== id));
  };

  return (
    <div style={{ maxWidth: 800, margin: '2rem auto', padding: '0 1rem' }}>
      <h2>Espace Candidat</h2>
      <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '1.5rem' }}>
        {(['cv', 'abonnements', 'notifications'] as const).map(t => (
          <button key={t} onClick={() => setTab(t)}
            style={{ padding: '0.5rem 1rem', background: tab === t ? '#1a1a2e' : 'transparent', color: tab === t ? 'white' : '#aaa', border: '1px solid #333', cursor: 'pointer', borderRadius: 4 }}>
            {t === 'cv' ? 'Mon CV' : t === 'abonnements' ? 'Abonnements' : `Notifications (${notifications.filter(n => !n.estLue).length})`}
          </button>
        ))}
      </div>

      {tab === 'cv' && (
        <div>
          <h3>Mon CV</h3>
          {cv && <div style={{ marginBottom: '1rem', padding: '1rem', border: '1px solid #333', borderRadius: 8 }}>
            <strong>{cv.titre}</strong><p>{cv.resume}</p><p>Compétences : {cv.competences.join(', ')}</p>
          </div>}
          <input placeholder="Titre du poste recherché" value={cvForm.titre || cv?.titre || ''}
            onChange={e => setCvForm(f => ({ ...f, titre: e.target.value }))} style={{ width: '100%', padding: '0.5rem', marginBottom: '0.5rem' }} />
          <textarea placeholder="Résumé professionnel" value={cvForm.resume || cv?.resume || ''}
            onChange={e => setCvForm(f => ({ ...f, resume: e.target.value }))} rows={3} style={{ width: '100%', padding: '0.5rem', marginBottom: '0.5rem' }} />
          <input placeholder="Compétences (séparées par virgule)" value={cvForm.competences || cv?.competences.join(', ') || ''}
            onChange={e => setCvForm(f => ({ ...f, competences: e.target.value }))} style={{ width: '100%', padding: '0.5rem', marginBottom: '0.5rem' }} />
          <button onClick={sauvegarderCv} style={{ padding: '0.5rem 1rem', cursor: 'pointer' }}>Enregistrer le CV</button>
        </div>
      )}

      {tab === 'abonnements' && (
        <div>
          <h3>Mes abonnements domaines</h3>
          <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '1rem' }}>
            <input placeholder="dev-web, cloud-azure, devops..." value={newDomaine}
              onChange={e => setNewDomaine(e.target.value)} style={{ flex: 1, padding: '0.5rem' }} />
            <button onClick={sabonner} style={{ padding: '0.5rem 1rem', cursor: 'pointer' }}>S'abonner</button>
          </div>
          {abonnements.map(a => (
            <div key={a.id} style={{ display: 'flex', justifyContent: 'space-between', padding: '0.5rem', border: '1px solid #333', borderRadius: 4, marginBottom: '0.25rem' }}>
              <span>{a.domaineMetierCode}</span>
              <button onClick={() => seDesabonner(a.id)} style={{ color: 'red', background: 'none', border: 'none', cursor: 'pointer' }}>✕</button>
            </div>
          ))}
        </div>
      )}

      {tab === 'notifications' && (
        <div>
          <h3>Mes notifications</h3>
          {notifications.length === 0 && <p style={{ color: '#aaa' }}>Aucune notification.</p>}
          {notifications.map(n => (
            <div key={n.id} style={{ padding: '0.75rem', border: '1px solid #333', borderRadius: 4, marginBottom: '0.5rem', opacity: n.estLue ? 0.6 : 1 }}>
              <p style={{ margin: 0 }}>{n.message}</p>
              <p style={{ margin: '0.25rem 0 0', color: '#666', fontSize: '0.8rem' }}>{new Date(n.dateCreation).toLocaleString('fr-FR')}</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
