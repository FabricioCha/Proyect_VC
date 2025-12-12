import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
    role?: string;
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"?: string;
    [key: string]: any;
}

interface AuthState {
    token: string | null;
    isAuthenticated: boolean;
    role: string | null;
    login: (token: string) => void;
    logout: () => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            token: null,
            isAuthenticated: false,
            role: null,
            login: (token: string) => {
                try {
                    const decoded = jwtDecode<DecodedToken>(token);
                    const role = decoded.role || decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || null;
                    set({ token, isAuthenticated: true, role });
                } catch (error) {
                    console.error('Error decoding token:', error);
                    set({ token, isAuthenticated: true, role: null });
                }
            },
            logout: () => set({ token: null, isAuthenticated: false, role: null }),
        }),
        {
            name: 'auth-storage',
            storage: createJSONStorage(() => localStorage),
        }
    )
);
