import { createContext, useContext, useState } from 'react';
import type { ReactNode } from 'react';
import { api } from '../api/client';

interface Utilisateur {
  utilisateurId: string;
  role: string;
  jeton: string;
}

interface AuthContextType {
  utilisateur: Utilisateur | null;
  connexion: (email: string, motDePasse: string) => Promise<void>;
  deconnexion: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [utilisateur, setUtilisateur] = useState<Utilisateur | null>(() => {
    const stored = localStorage.getItem('cvtech_user');
    return stored ? JSON.parse(stored) : null;
  });

  const connexion = async (email: string, motDePasse: string) => {
    const data = await api.post<Utilisateur>('/api/auth/connexion', { email, motDePasse });
    localStorage.setItem('cvtech_token', data.jeton);
    localStorage.setItem('cvtech_user', JSON.stringify(data));
    setUtilisateur(data);
  };

  const deconnexion = () => {
    localStorage.removeItem('cvtech_token');
    localStorage.removeItem('cvtech_user');
    setUtilisateur(null);
  };

  return <AuthContext.Provider value={{ utilisateur, connexion, deconnexion }}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth doit être utilisé dans AuthProvider');
  return ctx;
}
