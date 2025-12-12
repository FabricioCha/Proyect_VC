'use client';

import { useAuthStore } from "@/store/useAuthStore";
import { useRouter } from "next/navigation";
import { useEffect } from "react";
import { Button } from "@/components/ui/button";

export default function DashboardPage() {
    const { isAuthenticated, logout, user } = useAuthStore() as any; // Adjust type if needed
    const router = useRouter();

    useEffect(() => {
        if (!isAuthenticated) {
            router.push("/login");
        }
    }, [isAuthenticated, router]);

    if (!isAuthenticated) return null;

    return (
        <div className="p-8">
            <h1 className="text-3xl font-bold mb-4">Mi Panel de Cliente</h1>
            <p className="mb-4">Bienvenido al área de clientes.</p>
            <Button onClick={() => {
                logout();
                router.push("/login");
            }}>
                Cerrar Sesión
            </Button>
        </div>
    );
}
