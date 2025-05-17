
# 📦 Proyecto Empresa Mayorista

Sistema de gestión de ventas para una empresa mayorista, desarrollado con **ASP.NET Core Web API** utilizando **Entity Framework Core** y base de datos **MySQL**.

---

## 📑 Descripción

Esta API permite administrar:

- Clientes
- Ventas
- Detalles de ventas
- Productos (pendiente de implementación si se desea)
- Gestión de stock y registros de compras (opcional a futuro)

---

## 🛠️ Tecnologías utilizadas

- **ASP.NET Core 8 Web API**
- **Entity Framework Core**
- **MySQL**
- **Visual Studio / Visual Studio Code**
- **Postman** (para pruebas de endpoints)

---

## 📂 Estructura del Proyecto

```
Proyecto-Empresa-Mayorista/
│
├── Controllers/                # Controladores de API
│   ├── ClienteController.cs
│   ├── VentaController.cs
│   └── DetalleVentaController.cs
│
├── Models/                     # Modelos de datos
│   ├── Cliente.cs
│   ├── Venta.cs
│   └── DetalleVenta.cs
│
├── Data/                       # Contexto de base de datos
│   └── AppDbContext.cs
│
├── appsettings.json            # Configuración de cadena de conexión
├── Proyecto-Empresa-Mayorista.csproj
├── Program.cs                  # Configuración inicial de la API
└── README.md
```

---

## 📌 Configuración inicial

1. Crear la base de datos en MySQL:

```sql
CREATE DATABASE EmpresaMayoristaDB;
```

2. Configurar la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=EmpresaMayoristaDB;user=root;password=tu_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 📦 Instalación de dependencias

Desde la terminal de tu proyecto:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Pomelo.EntityFrameworkCore.MySql
```

---

## 🗃️ Crear y aplicar migraciones

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 🚀 Ejecutar la API

```bash
dotnet run
```

Por defecto se ejecutará en:

```
https://localhost:3000
http://localhost:3000
```

---

## 📬 Endpoints disponibles

### 📌 ClienteController

- `GET /api/Cliente`
- `GET /api/Cliente/{id}`
- `POST /api/Cliente`
- `PUT /api/Cliente/{id}`
- `DELETE /api/Cliente/{id}`

### 📌 VentaController

- `GET /api/Venta`
- `GET /api/Venta/{id}`
- `POST /api/Venta`
- `PUT /api/Venta/{id}`
- `DELETE /api/Venta/{id}`

### 📌 DetalleVentaController

- `GET /api/DetalleVenta`
- `GET /api/DetalleVenta/{id}`
- `POST /api/DetalleVenta`
- `PUT /api/DetalleVenta/{id}`
- `DELETE /api/DetalleVenta/{id}`

---

## 📖 Notas

- Este proyecto está desarrollado en .NET 8.
- Se utiliza `ControllerBase` como clase base para los controladores de API.
- Los controladores y contextos se registran en `Program.cs`.
- La base de datos se gestiona vía migraciones de Entity Framework Core.

---

## 📸 Vista previa de la estructura

```
Proyecto-Empresa-Mayorista/
├── Controllers/
├── Models/
├── Data/
├── appsettings.json
├── Program.cs
├── Proyecto-Empresa-Mayorista.csproj
└── README.md
```

---

## 📝 Autor

**Tomas [Tu Apellido]**

---

## 📄 Licencia

Este proyecto se distribuye bajo licencia MIT. Puedes utilizarlo, modificarlo y distribuirlo libremente.

---
