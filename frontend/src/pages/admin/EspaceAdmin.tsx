import { useEffect, useState } from 'react';
import { api } from '../../api/client';

interface Annonce { id: string; titre: string; }
interface DomaineMetier { code: string; libelle: string; }

export default function EspaceAdmin() {
  const [tab, setTab] = useState<'annonces' | 'articles' | 'domaines'>('annonces');
  const [annonces, setAnnonces] = useState<Annonce[]>([]);
  const [domaines, setDomaines] = useState<DomaineMetier[]>([]);
  const [articleForm, setArticleForm] = useState({ titre: '', contenu: '', domaineMetierCode: '', lienExterne: '' });
  const [domaineForm, setDomaineForm] = useState({ code: '', libelle: '' });

  useEffect(() => {
    api.get<Annonce[]>('/api/annonces').then(setAnnonces).catch(() => null);
    api.get<DomaineMetier[]>('/api/domaines-metier').then(setDomaines).catch(() => null);
  }, []);

  const modererAnnonce = async (id: string) => {
    await api.delete(`/api/annonces/${id}`);
    setAnnonces(prev => prev.filter(a => a.id !== id));
  };

  const publierArticle = async () => {
    try {
      await api.post('/api/articles', articleForm);
      alert('Article publié !');
      setArticleForm({ titre: '', contenu: '', domaineMetierCode: '', lienExterne: '' });
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  const creerDomaine = async () => {
    try {
      await api.post('/api/domaines-metier', domaineForm);
      const updated = await api.get<DomaineMetier[]>('/api/domaines-metier');
      setDomaines(updated);
      setDomaineForm({ code: '', libelle: '' });
    } catch (e: unknown) { alert(e instanceof Error ? e.message : 'Erreur'); }
  };

  const inputStyle = { width: '100%', padding: '0.5rem', marginBottom: '0.5rem', boxSizing: 'border-box' as const };

  return (
    <div style={{ maxWidth: 800, margin: '2rem auto', padding: '0 1rem' }}>
      <h2>Administration</h2>
      <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '1.5rem' }}>
        {(['annonces', 'articles', 'domaines'] as const).map(t => (
          <button key={t} onClick={() => setTab(t)}
            style={{ padding: '0.5rem 1rem', background: tab === t ? '#1a1a2e' : 'transparent', color: tab === t ? 'white' : '#aaa', border: '1px solid #333', cursor: 'pointer', borderRadius: 4 }}>
            {t === 'annonces' ? 'Modération' : t === 'articles' ? 'Articles' : 'Référentiel'}
          </button>
        ))}
      </div>

      {tab === 'annonces' && (
        <div>
          <h3>Modérer les annonces</h3>
          {annonces.length === 0 && <p style={{ color: '#aaa' }}>Aucune annonce active.</p>}
          {annonces.map(a => (
            <div key={a.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '0.75rem', border: '1px solid #333', borderRadius: 4, marginBottom: '0.5rem' }}>
              <span>{a.titre}</span>
              <button onClick={() => modererAnnonce(a.id)} style={{ color: 'red', background: 'none', border: '1px solid red', padding: '0.25rem 0.75rem', cursor: 'pointer', borderRadius: 4 }}>Supprimer</button>
            </div>
          ))}
        </div>
      )}

      {tab === 'articles' && (
        <div>
          <h3>Publier un article éditorial</h3>
          <input placeholder="Titre" value={articleForm.titre} onChange={e => setArticleForm(f => ({ ...f, titre: e.target.value }))} style={inputStyle} />
          <textarea placeholder="Contenu" value={articleForm.contenu} onChange={e => setArticleForm(f => ({ ...f, contenu: e.target.value }))} rows={5} style={inputStyle} />
          <input placeholder="Domaine métier (optionnel)" value={articleForm.domaineMetierCode} onChange={e => setArticleForm(f => ({ ...f, domaineMetierCode: e.target.value }))} style={inputStyle} />
          <input placeholder="Lien externe (URL)" value={articleForm.lienExterne} onChange={e => setArticleForm(f => ({ ...f, lienExterne: e.target.value }))} style={inputStyle} />
          <button onClick={publierArticle} style={{ padding: '0.5rem 1.5rem', cursor: 'pointer', background: '#1a1a2e', color: 'white', border: 'none', borderRadius: 4 }}>Publier l'article</button>
        </div>
      )}

      {tab === 'domaines' && (
        <div>
          <h3>Référentiel domaines métier</h3>
          <div style={{ display: 'flex', gap: '0.5rem', marginBottom: '1rem' }}>
            <input placeholder="Code (ex: ia-generative)" value={domaineForm.code} onChange={e => setDomaineForm(f => ({ ...f, code: e.target.value }))} style={{ flex: 1, padding: '0.5rem' }} />
            <input placeholder="Libellé" value={domaineForm.libelle} onChange={e => setDomaineForm(f => ({ ...f, libelle: e.target.value }))} style={{ flex: 1, padding: '0.5rem' }} />
            <button onClick={creerDomaine} style={{ padding: '0.5rem 1rem', cursor: 'pointer' }}>Ajouter</button>
          </div>
          {domaines.map((d, i) => (
            <div key={i} style={{ padding: '0.5rem', border: '1px solid #333', borderRadius: 4, marginBottom: '0.25rem' }}>
              <strong>{d.code}</strong> — {d.libelle}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
