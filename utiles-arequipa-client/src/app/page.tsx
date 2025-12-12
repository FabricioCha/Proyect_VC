import Link from "next/link";
import { Button } from "@/components/ui/button";

export default function Home() {
  return (
    <div className="flex min-h-screen flex-col items-center justify-center bg-gray-50">
      <main className="text-center space-y-8">
        <h1 className="text-4xl font-bold text-gray-900">
          Bienvenido a Utiles Arequipa
        </h1>
        <p className="text-xl text-gray-600">
          Tu tienda de útiles escolares y de oficina.
        </p>
        <div className="flex gap-4 justify-center">
          <Link href="/login">
            <Button size="lg">Iniciar Sesión</Button>
          </Link>
          <Link href="/register">
            <Button variant="outline" size="lg">Registrarse</Button>
          </Link>
        </div>
      </main>
    </div>
  );
}
