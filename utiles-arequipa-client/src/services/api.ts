import axios from 'axios';
import { useAuthStore } from '@/store/useAuthStore';

const api = axios.create({
    baseURL: 'http://localhost:5000/api', // Ajustar puerto si es necesario
    headers: {
        'Content-Type': 'application/json'
    }
});

// Interceptor para inyectar token
api.interceptors.request.use(config => {
    // Usamos getState() para acceder al store fuera de un componente React
    const token = useAuthStore.getState().token;
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Interceptor para manejar errores globales (opcional)
api.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.status === 401) {
            useAuthStore.getState().logout();
        }
        return Promise.reject(error);
    }
);

export default api;
