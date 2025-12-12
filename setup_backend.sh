#!/bin/bash

# Detener el script si hay errores
set -e

echo "🚀 Iniciando creación de la estructura para UtilesArequipa..."

# 1. Crear la Solución
echo "📦 Creando solución..."
dotnet new sln -n UtilesArequipa

# 2. Crear Proyectos (Capas)
echo "📂 Creando proyectos..."

# Domain: El núcleo (Entidades, Interfaces de Repositorio)
dotnet new classlib -n UtilesArequipa.Domain -f net9.0

# Application: Lógica de negocio, CQRS, DTOs
dotnet new classlib -n UtilesArequipa.Application -f net9.0

# Infrastructure: Implementación de EF Core, Migraciones, Servicios Externos
dotnet new classlib -n UtilesArequipa.Infrastructure -f net9.0

# API: Punto de entrada REST
dotnet new webapi -n UtilesArequipa.API -f net9.0

# 3. Agregar Proyectos a la Solución
echo "🔗 Agregando proyectos a la solución..."
dotnet sln add UtilesArequipa.Domain/UtilesArequipa.Domain.csproj
dotnet sln add UtilesArequipa.Application/UtilesArequipa.Application.csproj
dotnet sln add UtilesArequipa.Infrastructure/UtilesArequipa.Infrastructure.csproj
dotnet sln add UtilesArequipa.API/UtilesArequipa.API.csproj

# 4. Establecer Referencias (Regla de Dependencia)
echo "🔗 Estableciendo referencias entre proyectos..."

# Application depende de Domain
dotnet add UtilesArequipa.Application/UtilesArequipa.Application.csproj reference UtilesArequipa.Domain/UtilesArequipa.Domain.csproj

# Infrastructure depende de Application
dotnet add UtilesArequipa.Infrastructure/UtilesArequipa.Infrastructure.csproj reference UtilesArequipa.Application/UtilesArequipa.Application.csproj

# API depende de Application e Infrastructure
dotnet add UtilesArequipa.API/UtilesArequipa.API.csproj reference UtilesArequipa.Application/UtilesArequipa.Application.csproj
dotnet add UtilesArequipa.API/UtilesArequipa.API.csproj reference UtilesArequipa.Infrastructure/UtilesArequipa.Infrastructure.csproj

# 5. Instalar Paquetes NuGet
echo "⬇️  Instalando paquetes NuGet..."

# --- Application ---
# MediatR: Patrón Mediator para CQRS
dotnet add UtilesArequipa.Application/UtilesArequipa.Application.csproj package MediatR
# AutoMapper: Mapeo entre Entidades y DTOs
dotnet add UtilesArequipa.Application/UtilesArequipa.Application.csproj package AutoMapper
# FluentValidation: Validaciones fluidas
dotnet add UtilesArequipa.Application/UtilesArequipa.Application.csproj package FluentValidation

# --- Infrastructure ---
# Npgsql.EntityFrameworkCore.PostgreSQL: Provider para PostgreSQL
dotnet add UtilesArequipa.Infrastructure/UtilesArequipa.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.2
# Microsoft.EntityFrameworkCore.Tools: Para comandos de EF Core (Migraciones, etc.)
dotnet add UtilesArequipa.Infrastructure/UtilesArequipa.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools

# --- API ---
# Swashbuckle.AspNetCore: Documentación Swagger
dotnet add UtilesArequipa.API/UtilesArequipa.API.csproj package Swashbuckle.AspNetCore
# Hangfire: Tareas en segundo plano
dotnet add UtilesArequipa.API/UtilesArequipa.API.csproj package Hangfire
# MediatR (Includes DI extensions in v12+): Inyección de dependencias para MediatR
# Nota: El usuario solicitó 'MediatR.Extensions.Microsoft.DependencyInjection', pero en versiones recientes (12+)
# esto es parte del paquete principal 'MediatR'. Instalamos 'MediatR' en la API para asegurar que AddMediatR esté disponible.
dotnet add UtilesArequipa.API/UtilesArequipa.API.csproj package MediatR

# 6. Crear Estructura de Carpetas Interna (Clean Architecture)
echo "📂 Generando estructura de carpetas interna..."

# Domain
mkdir -p UtilesArequipa.Domain/Entities
mkdir -p UtilesArequipa.Domain/Interfaces

# Application
mkdir -p UtilesArequipa.Application/DTOs
mkdir -p UtilesArequipa.Application/Interfaces
mkdir -p UtilesArequipa.Application/Features
mkdir -p UtilesArequipa.Application/Validations

# Infrastructure
mkdir -p UtilesArequipa.Infrastructure/Persistence/Contexts
mkdir -p UtilesArequipa.Infrastructure/Persistence/Repositories
mkdir -p UtilesArequipa.Infrastructure/Services

echo "✅ ¡Estructura base generada exitosamente!"
