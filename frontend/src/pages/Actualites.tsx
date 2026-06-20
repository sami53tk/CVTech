import { useEffect, useState } from 'react';

interface ItemRss {
  titre: string;
  lien: string;
  description: string;
  datePublication: string;
}

export default function Actualites() {
  const [items, setItems] = useState<ItemRss[]>([]);
  const [erreur, setErreur] = useState('');

  useEffect(() => {
    fetch('http://localhost:5298/feed/rss')
      .then(r => r.text())
      .then(xml => {
        const parser = new DOMParser();
        const doc = parser.parseFromString(xml, 'application/xml');
        const parsed = Array.from(doc.querySelectorAll('item')).map(item => ({
          titre: item.querySelector('title')?.textContent ?? '',
          lien: item.querySelector('link')?.textContent ?? '',
          description: item.querySelector('description')?.textContent ?? '',
          datePublication: item.querySelector('pubDate')?.textContent ?? '',
        }));
        setItems(parsed);
      })
      .catch(() => setErreur('Impossible de charger le flux RSS.'));
  }, []);

  return (
    <div style={{ maxWidth: 800, margin: '2rem auto', padding: '0 1rem' }}>
      <h2>Actualités éditoriales</h2>
      <p style={{ color: '#aaa', fontSize: '0.85rem' }}>Flux RSS : <a href="http://localhost:5298/feed/rss" target="_blank" rel="noreferrer">http://localhost:5298/feed/rss</a></p>
      {erreur && <p style={{ color: 'red' }}>{erreur}</p>}
      {items.length === 0 && !erreur && <p style={{ color: '#aaa' }}>Aucun article publié.</p>}
      {items.map((item, i) => (
        <div key={i} style={{ border: '1px solid #333', borderRadius: 8, padding: '1rem', marginBottom: '0.75rem' }}>
          <h3 style={{ margin: 0 }}><a href={item.lien} target="_blank" rel="noreferrer" style={{ color: '#4fc3f7' }}>{item.titre}</a></h3>
          <p style={{ color: '#aaa', margin: '0.5rem 0' }}>{item.description.slice(0, 200)}{item.description.length > 200 ? '...' : ''}</p>
          <p style={{ color: '#666', fontSize: '0.8rem' }}>{item.datePublication}</p>
        </div>
      ))}
    </div>
  );
}
