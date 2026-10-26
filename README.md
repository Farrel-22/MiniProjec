# MiniProject

Proyek mini dengan struktur berlapis (Web → Services → DAL → Common).

## Struktur

- `MiniProject/` — Web project (ASP.NET Core, root namespace `MiniProject`). Berisi controller dan konfigurasi startup.
- `MiniProject.Common/` — Entity dan abstract class. `Abstract/BaseEntity` berisi field umum (Uid, DtCreated, TxtCreatedBy, DtUpdated, TxtUpdatedBy, BitActive). `Entity/Master/MSample` adalah contoh entity (tabel `m_sample`).
- `MiniProject.DAL/` — Data access. `Context/MiniProjectDbContext` (DbContext, DbSet `MSamples`) dan `ModelBuilders/` untuk konfigurasi entity.
- `MiniProject.Services/` — Business logic. `Master/ISampleService` dan `SampleService`. `Infrastructure/ServiceCollectionExtensions` berisi `AddMiniProjectServices` untuk registrasi DI.

## Referensi antar project

```
MiniProject → MiniProject.Services → MiniProject.DAL → MiniProject.Common
```

## Menjalankan

1. Isi connection string `ConnectionStrings:MiniProjectConnection` di `MiniProject/appsettings.json` (atau user secrets / environment variable).
2. Build solution:
   ```
   dotnet build MiniProject.slnx
   ```
3. Jalankan web project:
   ```
   dotnet run --project MiniProject
   ```

## Migration

Tambah migration awal dengan:

```
dotnet ef migrations add Init --project MiniProject.DAL --startup-project MiniProject
```
