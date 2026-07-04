# School ERP System

A full-stack, production-ready School Enterprise Resource Planning (ERP) System.

## Technology Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Next.js 14 (App Router), Tailwind CSS, Framer Motion, Recharts |
| Backend | ASP.NET Core 10 Web API, Clean Architecture |
| Database | PostgreSQL 15 |
| Auth | JWT Bearer Tokens with Refresh Tokens |
| ORM | Entity Framework Core 10 |

---

## Project Structure

```
student_erp_test/
├── frontend/                   ← Next.js 14 frontend
├── backend/                    ← ASP.NET Core Clean Architecture
│   ├── SchoolERP.Domain/       ← Entities, Enums
│   ├── SchoolERP.Application/  ← DTOs, Interfaces
│   ├── SchoolERP.Persistence/  ← EF Core, Seeder
│   ├── SchoolERP.Infrastructure/← JWT, BCrypt
│   └── SchoolERP.API/          ← Controllers, Middleware
└── docker-compose.yml          ← PostgreSQL + pgAdmin
```

---

## Quick Start

### 1. Start Database (Docker)
```bash
docker-compose up -d
```
PostgreSQL → `localhost:5432`  
pgAdmin → `http://localhost:5050` (admin@schoolerp.com / admin)

### 2. Run Backend
```bash
cd backend
dotnet run --project SchoolERP.API
```
API → `http://localhost:5000`  
Swagger → `http://localhost:5000/swagger`

### 3. Run Frontend
```bash
cd frontend
npm run dev
```
App → `http://localhost:3000`

---

## Demo Credentials

| Role | Email | Password |
|------|-------|----------|
| Super Admin | superadmin@schoolerp.com | Admin@123 |
| School Admin | admin@schoolerp.com | Admin@123 |
| Principal | principal@schoolerp.com | Admin@123 |
| Teacher | priya.sharma@schoolerp.com | Teacher@123 |
| Student | arjun.mehta@student.schoolerp.com | Student@123 |
| Parent | suresh.mehta@parent.schoolerp.com | Parent@123 |
| Accountant | accountant@schoolerp.com | Admin@123 |
| Librarian | librarian@schoolerp.com | Admin@123 |

---

## Core Modules

- ✅ **Authentication** — JWT login, logout, refresh token, forgot/reset password
- ✅ **Role-Based Access** — 8 roles with different dashboards and permissions
- ✅ **Student Management** — Admission, profiles, search, CRUD
- ✅ **Teacher Management** — Profiles, departments, CRUD
- ✅ **Class & Section Management** — Create classes and sections
- ✅ **Subject Management** — Create subjects, assign teachers
- ✅ **Timetable** — Per-class and per-teacher timetables
- ✅ **Attendance** — Daily marking, monthly reports, analytics
- ✅ **Fee Management** — Fee structures, payments, receipts
- ✅ **Library** — Book catalog, issue/return
- ✅ **Notifications** — Global announcements, per-class notices
- ✅ **Messaging** — Teacher ↔ Student ↔ Parent communication
- ✅ **Dashboard Analytics** — Charts, KPIs per role
- ✅ **Demo Data Seeder** — Pre-filled with realistic data

---

## API Endpoints (Swagger)

Open `http://localhost:5000/swagger` for full documentation.

Key endpoints:
- `POST /api/auth/login` — Login
- `GET /api/dashboard/stats` — Dashboard KPIs
- `GET /api/students` — List students (paginated)
- `POST /api/students` — Admit student
- `GET /api/teachers` — List teachers
- `POST /api/attendance/mark` — Mark attendance
- `GET /api/attendance/report` — Attendance report
- `GET /api/classes` — All classes with sections
- `GET /api/notifications` — All notifications

---

## Environment Configuration

Backend — `backend/SchoolERP.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=SchoolERPDb;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "SchoolERP_SuperSecretJwtKey_2025_MustBe32CharsLong!",
    "Issuer": "SchoolERP.API",
    "Audience": "SchoolERP.Client"
  }
}
```

Frontend — create `frontend/.env.local`:
```
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```
