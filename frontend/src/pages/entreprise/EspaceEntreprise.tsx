import { useState } from 'react';
import { api } from '../../api/client';

export default function EspaceEntreprise() {
  const [tab, setTab] = useState<'annonces' | 'ao'>('annonces');

  // Formulaire annonce
  const [annonceForm, setAnnonceForm] = useState({ titre: '', description: '', typeContrat: 1, domaineMetierCode: '', localisation: '' });
  // Formulaire appel d'offre
  const [aoForm, setAoForm] = useState({ titre: '', description: '', domaineMetierCode: '', localisation: '', budgetMax: '' });

  const publierAnnonce = async () => {
    try {
      await api.post('/api/annonces', annonceForm);
      alert('Annonce publiée !');
      setAnnonceForm({ titre: '', description: '', typeContrat: 1, domaineMetierCode: '', localisation: '' });
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  const publierAo = async () => {
    try {
      await api.post('/api/appels-offres', { ...aoForm, budgetMax: parseFloat(aoForm.budgetMax) });
      alert('Appel d\'offre publié !');
      setAoForm({ titre: '', description: '', domaineMetierCode: '', localisation: '', budgetMax: '' });
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  const inputStyle = { width: '100%', padding: '0.5rem', marginBottom: '0.5rem', boxSizing: 'border-box' as const };

  return (
    <div style={{ maxWidth: 700, margin: '2rem auto', padding: '0 1rem' }}>
      <h2>Espace Entreprise</h2>
      <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '1.5rem' }}>
        {(['annonces', 'ao'] as const).map(t => (
          <button key={t} onClick={() => setTab(t)}
            style={{ padding: '0.5rem 1rem', background: tab === t ? '#1a1a2e' : 'transparent', color: tab === t ? 'white' : '#aaa', border: '1px solid #333', cursor: 'pointer', borderRadius: 4 }}>
            {t === 'annonces' ? 'Publier une annonce' : "Publier un appel d'offre"}
          </button>
        ))}
      </div>

      {tab === 'annonces' && (
        <div>
          <h3>Nouvelle annonce d'emploi</h3>
          <input placeholder="Titre du poste" value={annonceForm.titre} onChange={e => setAnnonceForm(f => ({ ...f, titre: e.target.value }))} style={inputStyle} />
          <textarea placeholder="Description" value={annonceForm.description} onChange={e => setAnnonceForm(f => ({ ...f, description: e.target.value }))} rows={4} style={inputStyle} />
          <select value={annonceForm.typeContrat} onChange={e => setAnnonceForm(f => ({ ...f, typeContrat: Number(e.target.value) }))} style={inputStyle}>
            <option value={1}>CDI</option>
            <option value={2}>CDD</option>
            <option value={3}>Stage</option>
            <option value={4}>Alternance</option>
          </select>
          <input placeholder="Domaine métier (ex: dev-web)" value={annonceForm.domaineMetierCode} onChange={e => setAnnonceForm(f => ({ ...f, domaineMetierCode: e.target.value }))} style={inputStyle} />
          <input placeholder="Localisation" value={annonceForm.localisation} onChange={e => setAnnonceForm(f => ({ ...f, localisation: e.target.value }))} style={inputStyle} />
          <button onClick={publierAnnonce} style={{ padding: '0.5rem 1.5rem', cursor: 'pointer', background: '#1a1a2e', color: 'white', border: 'none', borderRadius: 4 }}>Publier</button>
        </div>
      )}

      {tab === 'ao' && (
        <div>
          <h3>Nouvel appel d'offre freelance</h3>
          <input placeholder="Titre de la mission" value={aoForm.titre} onChange={e => setAoForm(f => ({ ...f, titre: e.target.value }))} style={inputStyle} />
          <textarea placeholder="Description" value={aoForm.description} onChange={e => setAoForm(f => ({ ...f, description: e.target.value }))} rows={4} style={inputStyle} />
          <input placeholder="Domaine métier (ex: cloud-azure)" value={aoForm.domaineMetierCode} onChange={e => setAoForm(f => ({ ...f, domaineMetierCode: e.target.value }))} style={inputStyle} />
          <input placeholder="Localisation" value={aoForm.localisation} onChange={e => setAoForm(f => ({ ...f, localisation: e.target.value }))} style={inputStyle} />
          <input type="number" placeholder="Budget max (€/j)" value={aoForm.budgetMax} onChange={e => setAoForm(f => ({ ...f, budgetMax: e.target.value }))} style={inputStyle} />
          <button onClick={publierAo} style={{ padding: '0.5rem 1.5rem', cursor: 'pointer', background: '#1a1a2e', color: 'white', border: 'none', borderRadius: 4 }}>Publier</button>
        </div>
      )}
    </div>
  );
}
