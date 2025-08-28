# Bugar Rogo Jiwo API

API backend untuk aplikasi mobile kebugaran "Bugar Rogo Jiwo" yang dibangun menggunakan ASP.NET Core 8 dengan Clean Architecture.

## 🏗️ Arsitektur

Proyek ini menggunakan **Clean Architecture** yang terdiri dari:

- **Burjo.Core**: Domain layer - berisi entities, enums, interfaces, dan DTOs
- **Burjo.Infrastructure**: Infrastructure layer - implementasi repositories, services, dan data access
- **Burjo.WebApi**: Presentation layer - API controllers dan konfigurasi startup

## 🛠️ Teknologi Stack

- **Framework**: ASP.NET Core 8
- **Database**: SQLite (Development) / PostgreSQL (Production)
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Password Hashing**: BCrypt
- **Documentation**: Swagger/OpenAPI
- **Architecture Pattern**: Clean Architecture

## 📦 Paket NuGet

### Burjo.WebApi
- Swashbuckle.AspNetCore (Swagger/OpenAPI)
- Microsoft.EntityFrameworkCore.Design
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Sqlite

### Burjo.Infrastructure
- Microsoft.EntityFrameworkCore.Sqlite (Development)
- Npgsql.EntityFrameworkCore.PostgreSQL (Production)
- BCrypt.Net-Next
- System.IdentityModel.Tokens.Jwt
- Microsoft.IdentityModel.Tokens

## 🚀 Setup dan Instalasi

### 1. Prerequisites
- .NET 8 SDK
- Visual Studio Code atau Visual Studio
- SQLite (untuk development, otomatis tersedia)
- PostgreSQL (untuk production, opsional)

### 2. Clone dan Setup Project

```bash
# Clone repository
git clone https://github.com/adrianismu/BurjoAPI.git
cd BurjoAPI

# Restore packages
dotnet restore
```

### 3. Database Setup (SQLite - Default Development)

Database SQLite akan dibuat otomatis saat pertama kali menjalankan aplikasi. Tidak perlu setup manual.

```bash
# Jalankan migrations (sudah tersedia)
cd src/Burjo.Infrastructure
dotnet ef database update --startup-project "../Burjo.WebApi"
```

### 4. Run Application

```bash
cd src/Burjo.WebApi
dotnet run
```

API akan berjalan di:
- **HTTP**: http://localhost:5156
- **Swagger UI**: http://localhost:5156 (root URL)

## 📚 API Endpoints

### 🔐 Authentication

#### Register User
```http
POST /api/Auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response (201 Created):**
```json
{
  "message": "User registered successfully",
  "userId": "cb6fe371-e755-4593-8923-daesdf9ba67b4"
}
```

#### Login User
```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "message": "Login successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-08-26T14:30:00Z",
  "profile": {
    "fullName": "John Doe",
    "age": 30,
    "gender": "Laki-laki",
    "heightCm": 175,
    "weightKg": 70,
    "medicalHistory": "Tidak ada",
    "fitnessLevel": "Sedang",
    "bmi": 22.86
  }
}
```

### 👤 Profile Management (Requires Authentication)

#### Get User Profile
```http
GET /api/Profile
Authorization: Bearer <jwt-token>
```

**Response (200 OK):**
```json
{
  "fullName": "John Doe",
  "age": 30,
  "gender": "Laki-laki", 
  "heightCm": 175,
  "weightKg": 70,
  "medicalHistory": "Tidak ada",
  "fitnessLevel": "Sedang",
  "bmi": 22.86
}
```

#### Create/Update User Profile
```http
PUT /api/Profile
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "fullName": "John Doe",
  "age": 30,
  "gender": "Laki-laki",
  "heightCm": 175,
  "weightKg": 70,
  "medicalHistory": "Tidak ada riwayat penyakit serius",
  "fitnessLevel": "Sedang"
}
```

### 🏥 Health Management (Requires Authentication)

#### Save Health Conditions
```http
POST /api/Health/conditions
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "chronicDiseases": "Hipertensi ringan",
  "physicalActivityComplaints": "Nyeri lutut ringan setelah olahraga",
  "dailyPhysicalActivityMinutes": 30
}
```

#### Get Health Conditions
```http
GET /api/Health/conditions
Authorization: Bearer <jwt-token>
```

#### Get Risk Assessment
```http
GET /api/Health/risk-assessment
Authorization: Bearer <jwt-token>
```

**Response (200 OK):**
```json
{
  "riskCategory": "PengawasanRingan",
  "riskCategoryText": "Pengawasan Ringan",
  "recommendationMessage": "Anda perlu pengawasan ringan saat berolahraga. Disarankan untuk berkonsultasi dengan profesional kesehatan.",
  "riskFactors": [
    "Memiliki riwayat penyakit kronis: Hipertensi ringan",
    "Memiliki keluhan saat beraktivitas fisik: Nyeri lutut ringan setelah olahraga"
  ]
}
```

### 💪 Exercise Management (Requires Authentication)

#### Get Exercise Recommendations
```http
GET /api/Exercise/recommendations
Authorization: Bearer <jwt-token>
```

**Response (200 OK):**
```json
{
  "message": "Exercise recommendations retrieved successfully",
  "exercises": [
    {
      "id": "ex001",
      "name": "Jalan Kaki",
      "description": "Jalan kaki selama 30 menit dengan kecepatan sedang",
      "targetRiskCategory": "PengawasanRingan",
      "targetRiskCategoryText": "Pengawasan Ringan"
    }
  ]
}
```

### 📅 Schedule Management (Requires Authentication)

#### Get Weekly Schedule
```http
GET /api/Schedule
Authorization: Bearer <jwt-token>
```

#### Create/Update Schedule
```http
POST /api/Schedule
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "scheduleItems": [
    {
      "exerciseId": "ex001",
      "day": 1,
      "durationMinutes": 30
    },
    {
      "exerciseId": "ex002", 
      "day": 3,
      "durationMinutes": 45
    }
  ]
}
```

### 😊 Mood Tracking (Requires Authentication)

#### Log Daily Mood
```http
POST /api/Mood
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "mood": "Baik",
  "notes": "Merasa energik setelah olahraga pagi"
}
```

#### Get Weekly Mood History
```http
GET /api/Mood/history
Authorization: Bearer <jwt-token>
```

#### Get Latest Mood
```http
GET /api/Mood/latest
Authorization: Bearer <jwt-token>
```

#### Check Today's Mood Status
```http
GET /api/Mood/today-status
Authorization: Bearer <jwt-token>
```

### 💬 Chatbot (Requires Authentication)

#### Send Message to Chatbot
```http
POST /api/Chat/send
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "message": "Halo, saya ingin tips olahraga untuk pemula"
}
```

**Response (200 OK):**
```json
{
  "botResponse": {
    "response": "Halo! Untuk pemula, saya sarankan mulai dengan jalan kaki 15-20 menit setiap hari. Ini adalah olahraga yang aman dan mudah untuk memulai rutinitas kebugaran Anda.",
    "timestamp": "2025-08-26T14:30:00Z",
    "suggestedActions": "Buat jadwal olahraga",
    "quickReplies": [
      "Tips diet sehat",
      "Olahraga untuk menurunkan berat badan",
      "Cara mengatasi nyeri otot"
    ]
  }
}
```

#### Get Welcome Message
```http
GET /api/Chat/welcome
Authorization: Bearer <jwt-token>
```

#### Get Chat Help
```http
GET /api/Chat/help
Authorization: Bearer <jwt-token>
```

### 🏥 Health Check

#### Health Status
```http
GET /api/Health
```

#### System Info  
```http
GET /api/Health/info
```

## 🔧 Konfigurasi

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "Secret": "ThisIsMySecretKeyForJwtTokenGeneration12345678901234567890",
    "Issuer": "BurjoApi",
    "Audience": "BurjoApp",
    "ExpiryHours": 24
  }
}
```

### Database Connection

#### Development (SQLite)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=BurjoDb_Dev.sqlite"
  }
}
```

#### Production (PostgreSQL)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=BurjoDb;Username=postgres;Password=your_password"
  }
}
```

## 📊 Data Models

### Core Entities

#### User Entity
- **Id**: Guid (Primary key, auto-generated)
- **Email**: Unique user email (Required, Max 255 chars)
- **PasswordHash**: BCrypt hashed password (Required, Max 500 chars)
- **IsActive**: Soft delete flag (Boolean)
- **CreatedAt**: Creation timestamp (Auto-generated)
- **UpdatedAt**: Last update timestamp (Auto-generated)
- **Profile**: Navigation property to UserProfile

#### UserProfile Entity
- **Id**: Guid (Primary key, auto-generated)
- **FullName**: User's full name (Required, Max 200 chars)
- **Age**: User's age (Required, Range: 1-150)
- **Gender**: Gender string (Required, e.g., "Laki-laki", "Perempuan")
- **HeightCm**: Height in centimeters (Required, Range: 50-300)
- **WeightKg**: Weight in kilograms (Required, Range: 20-500)
- **MedicalHistory**: Medical conditions/history (Max 1000 chars)
- **FitnessLevel**: Fitness level (Required, e.g., "Rendah", "Sedang", "Tinggi")
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp
- **UserId**: Foreign key to User
- **User**: Navigation property to User
- **Bmi**: Computed BMI calculation (WeightKg / (HeightCm/100)²)

#### HealthCondition Entity
- **Id**: Guid (Primary key)
- **ChronicDiseases**: Chronic diseases information (Max 500 chars)
- **PhysicalActivityComplaints**: Physical activity complaints (Max 500 chars)
- **DailyPhysicalActivityMinutes**: Daily physical activity in minutes
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp
- **UserProfileId**: Foreign key to UserProfile
- **UserProfile**: Navigation property to UserProfile

#### Exercise Entity
- **Id**: Guid (Primary key)
- **Name**: Exercise name (Required, Max 100 chars)
- **Description**: Exercise description (Required, Max 500 chars)
- **TargetRiskCategory**: Target risk category (Enum: Aman, PengawasanRingan, SupervisiMedis)
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp

#### UserScheduleItem Entity
- **Id**: Guid (Primary key)
- **UserId**: Foreign key to User
- **ExerciseName**: Name of the exercise (Max 100 chars)
- **Day**: Day of week (1-7, Monday=1)
- **DurationMinutes**: Exercise duration in minutes
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp
- **User**: Navigation property to User

#### MoodLog Entity
- **Id**: Guid (Primary key)
- **UserId**: Foreign key to User
- **Mood**: Mood type (Enum: SangatBaik, Baik, Sedang, Buruk)
- **LoggedAt**: When mood was logged
- **Notes**: Optional notes about mood (Max 500 chars)
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp
- **User**: Navigation property to User

### Enums

#### RiskCategory
- `Aman`: Safe for normal exercise
- `PengawasanRingan`: Requires light supervision
- `SupervisiMedis`: Requires medical supervision

#### MoodType
- `SangatBaik`: Very Good (4 points)
- `Baik`: Good (3 points)
- `Sedang`: Fair (2 points)
- `Buruk`: Poor (1 point)

## 🏗️ Struktur Proyek

```
Burjo/
├── Core/                           # Domain Layer
│   ├── Entities/                   # Domain entities
│   │   ├── User.cs                 # User entity
│   │   ├── UserProfile.cs          # User profile entity
│   │   ├── HealthCondition.cs      # Health condition entity
│   │   ├── Exercise.cs             # Exercise entity
│   │   ├── UserScheduleItem.cs     # Schedule item entity
│   │   └── MoodLog.cs              # Mood log entity
│   ├── Enums/                      # Domain enums
│   │   ├── RiskCategory.cs         # Risk category enum
│   │   └── MoodType.cs             # Mood type enum
│   ├── Interfaces/                 # Repository interfaces
│   │   ├── IUserRepository.cs      # User repository interface
│   │   ├── IUserProfileRepository.cs # User profile repository interface
│   │   ├── IHealthConditionRepository.cs # Health condition repository interface
│   │   ├── IExerciseRepository.cs  # Exercise repository interface
│   │   ├── IUserScheduleRepository.cs # Schedule repository interface
│   │   └── IMoodLogRepository.cs   # Mood log repository interface
│   └── DTOs/                       # Data Transfer Objects
│       ├── UserDTOs.cs             # User-related DTOs
│       ├── ProfileDTOs.cs          # Profile-related DTOs
│       ├── HealthDTOs.cs           # Health-related DTOs
│       ├── ExerciseDTOs.cs         # Exercise-related DTOs
│       ├── ScheduleDTOs.cs         # Schedule-related DTOs
│       └── MoodDTOs.cs             # Mood-related DTOs
├── Infrastructure/                 # Infrastructure Layer
│   ├── Data/                       # Database context and configurations
│   │   ├── ApplicationDbContext.cs # Main database context
│   │   └── Configurations/         # Entity configurations
│   │       ├── UserConfiguration.cs # User entity configuration
│   │       ├── UserProfileConfiguration.cs # User profile configuration
│   │       ├── HealthConditionConfiguration.cs # Health condition configuration
│   │       ├── ExerciseConfiguration.cs # Exercise configuration
│   │       ├── UserScheduleItemConfiguration.cs # Schedule configuration
│   │       └── MoodLogConfiguration.cs # Mood log configuration
│   ├── Repositories/               # Repository implementations
│   │   ├── UserRepository.cs       # User repository implementation
│   │   ├── UserProfileRepository.cs # User profile repository implementation
│   │   ├── HealthConditionRepository.cs # Health condition repository implementation
│   │   ├── ExerciseRepository.cs   # Exercise repository implementation
│   │   ├── UserScheduleRepository.cs # Schedule repository implementation
│   │   └── MoodLogRepository.cs    # Mood log repository implementation
│   ├── Services/                   # Business logic services
│   │   ├── AuthService.cs          # Authentication service
│   │   ├── HealthService.cs        # Health assessment service
│   │   ├── RecommendationService.cs # Exercise recommendation service
│   │   ├── ScheduleService.cs      # Schedule management service
│   │   ├── MoodService.cs          # Mood tracking service
│   │   └── ChatService.cs          # Chatbot service
│   └── Migrations/                 # Entity Framework migrations
│       └── [DateTime]_InitialCreate.cs
├── WebApi/                         # Presentation Layer
│   ├── Controllers/                # API controllers
│   │   ├── AuthController.cs       # Authentication endpoints
│   │   ├── ProfileController.cs    # Profile management endpoints
│   │   ├── HealthController.cs     # Health assessment endpoints
│   │   ├── ExerciseController.cs   # Exercise recommendation endpoints
│   │   ├── ScheduleController.cs   # Schedule management endpoints
│   │   ├── MoodController.cs       # Mood tracking endpoints
│   │   ├── ChatController.cs       # Chatbot endpoints
│   │   └── UsersController.cs      # User management endpoints
│   ├── Middleware/                 # Custom middleware (if any)
│   ├── Program.cs                  # Application entry point
│   ├── appsettings.json           # Configuration settings
│   ├── appsettings.Development.json # Development configuration
│   └── WebApi.csproj              # Project file
├── BurjoDb_Dev.sqlite             # SQLite database file
└── README.md                      # Project documentation
```

## 🌟 Fitur Utama

### 1. **Autentikasi & Manajemen User**
- Registrasi dengan validasi email unik
- Login dengan JWT authentication
- Profil user dengan data biometrik lengkap
- Soft delete untuk user management

### 2. **Penilaian Kesehatan & Risiko**
- Input kondisi kesehatan dan riwayat medis
- Kalkulasi BMI otomatis
- Kategorisasi risiko berdasarkan kondisi kesehatan:
  - **Aman**: BMI normal, tanpa penyakit kronis
  - **Pengawasan Ringan**: BMI tidak ideal atau ada keluhan ringan
  - **Supervisi Medis**: BMI ekstrem atau ada penyakit kronis

### 3. **Rekomendasi Olahraga**
- 12 jenis olahraga ter-kategorisasi berdasarkan tingkat risiko
- Rekomendasi otomatis berdasarkan profil kesehatan user
- Filter berdasarkan kategori risiko

### 4. **Penjadwalan Olahraga**
- Penjadwalan mingguan dengan durasi
- Management jadwal per user
- Update dan delete jadwal

### 5. **Pelacakan Mood**
- Log mood harian dengan 4 kategori
- Statistik mood mingguan
- History mood dengan notes opsional

### 6. **Chatbot Berbahasa Indonesia**
- Respons otomatis dalam bahasa Indonesia
- Kategori pertanyaan: olahraga, diet, motivasi, umum
- Respons yang disesuaikan dengan konteks

## 🚀 Panduan Pengembangan

### Menambah Entitas Baru
1. Buat entitas di folder `Core/Entities/`
2. Tambahkan konfigurasi di `Infrastructure/Data/Configurations/`
3. Buat repository interface di `Core/Interfaces/`
4. Implementasikan repository di `Infrastructure/Repositories/`
5. Tambahkan DTOs di `Core/DTOs/`
6. Buat controller di `WebApi/Controllers/`

### Menambah Migrasi Database
```powershell
# Tambah migration
dotnet ef migrations add NamaMigration --project Infrastructure --startup-project WebApi

# Update database
dotnet ef database update --project Infrastructure --startup-project WebApi
```

### Testing API
1. Jalankan aplikasi: `dotnet run --project WebApi`
2. Buka Swagger UI: http://localhost:5156
3. Register user baru via `/api/auth/register`
4. Login via `/api/auth/login` untuk mendapat token
5. Gunakan token untuk authorize endpoint lain

## 🔍 Troubleshooting

### Database Issues
- **SQLite file missing**: Migration akan membuat file otomatis
- **Connection string error**: Pastikan path SQLite benar
- **Migration error**: Hapus migrations dan buat ulang

### Authentication Issues
- **JWT Secret**: Pastikan minimal 32 karakter di appsettings.json
- **Token expired**: Default 24 jam, sesuaikan di JwtSettings
- **Bearer format**: Gunakan format "Bearer {token}" di header Authorization

### API Issues
- **CORS errors**: Konfigurasi CORS sudah include untuk development
- **Validation errors**: Periksa required fields dan format data
- **500 errors**: Cek logs untuk detail error message

## 📱 Integrasi Mobile (Flutter)

### Dependencies yang Dibutuhkan
```yaml
dependencies:
  http: ^1.1.0
  shared_preferences: ^2.2.2
  flutter_secure_storage: ^9.0.0
```

### Contoh Implementasi Service
```dart
class ApiService {
  static const String baseUrl = 'http://localhost:5156/api';
  
  // Authentication
  Future<Map<String, dynamic>> login(String email, String password) async {
    final response = await http.post(
      Uri.parse('$baseUrl/auth/login'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'email': email, 'password': password}),
    );
    return jsonDecode(response.body);
  }
  
  // Authorized requests
  Future<Map<String, dynamic>> getProfile(String token) async {
    final response = await http.get(
      Uri.parse('$baseUrl/profile'),
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
    );
    return jsonDecode(response.body);
  }
}
```

## 🗄️ Database Management & Struktur

### Cara Menampilkan Database

#### 1. Menggunakan SQLite Browser (Recommended)
1. Download [DB Browser for SQLite](https://sqlitebrowser.org/)
2. Install dan buka aplikasi
3. Klik "Open Database" dan pilih `BurjoDb_Dev.sqlite`
4. Browse data menggunakan tab "Browse Data"
5. Lihat struktur menggunakan tab "Database Structure"

#### 2. Menggunakan Entity Framework Tools
```powershell
# Lihat daftar migrations
dotnet ef migrations list --project src/Burjo.Infrastructure --startup-project src/Burjo.WebApi

# Update database ke migration terbaru
dotnet ef database update --project src/Burjo.Infrastructure --startup-project src/Burjo.WebApi

# Buat script SQL dari migrations
dotnet ef migrations script --project src/Burjo.Infrastructure --startup-project src/Burjo.WebApi
```

#### 3. Menggunakan Visual Studio Code Extensions
- Install extension: **SQLite Viewer**
- Buka file `BurjoDb_Dev.sqlite` dari VS Code Explorer
- Klik kanan → "Open Database"

#### 4. Menggunakan Command Line (jika sqlite3 tersedia)
```bash
# Masuk ke SQLite console
sqlite3 BurjoDb_Dev.sqlite

# Commands dalam SQLite:
.tables                    # Lihat semua tabel
.schema                    # Lihat struktur database
.schema table_name         # Lihat struktur tabel tertentu
SELECT * FROM Users;       # Query data
.quit                      # Keluar
```

### Database Schema & Struktur Lengkap

#### **Tabel: Users**
```sql
CREATE TABLE "Users" (
    "Id" TEXT PRIMARY KEY NOT NULL,
    "Email" TEXT(255) NOT NULL UNIQUE,
    "PasswordHash" TEXT(500) NOT NULL,
    "IsActive" INTEGER NOT NULL DEFAULT 1,
    "CreatedAt" TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Index
CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
```

#### **Tabel: UserProfiles**
```sql
CREATE TABLE "UserProfiles" (
    "Id" TEXT PRIMARY KEY NOT NULL,
    "FullName" TEXT(200) NOT NULL,
    "Age" INTEGER NOT NULL CHECK("Age" >= 1 AND "Age" <= 150),
    "Gender" TEXT(50) NOT NULL,
    "HeightCm" REAL NOT NULL CHECK("HeightCm" >= 50 AND "HeightCm" <= 300),
    "WeightKg" REAL NOT NULL CHECK("WeightKg" >= 20 AND "WeightKg" <= 500),
    "MedicalHistory" TEXT(1000) NOT NULL,
    "FitnessLevel" TEXT(50) NOT NULL,
    "CreatedAt" TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UserId" TEXT NOT NULL,
    FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- Index
CREATE UNIQUE INDEX "IX_UserProfiles_UserId" ON "UserProfiles" ("UserId");
```

#### **Tabel: health_conditions**
```sql
CREATE TABLE "health_conditions" (
    "id" TEXT PRIMARY KEY NOT NULL,
    "chronic_diseases" TEXT(500) NOT NULL,
    "physical_activity_complaints" TEXT(500) NOT NULL,
    "daily_physical_activity_minutes" INTEGER NOT NULL,
    "created_at" TEXT NOT NULL,
    "updated_at" TEXT NOT NULL,
    "user_profile_id" TEXT NOT NULL,
    FOREIGN KEY ("user_profile_id") REFERENCES "UserProfiles" ("Id") ON DELETE CASCADE
);

-- Index
CREATE UNIQUE INDEX "ix_health_conditions_user_profile_id" ON "health_conditions" ("user_profile_id");
```

#### **Tabel: exercises**
```sql
CREATE TABLE "exercises" (
    "id" TEXT PRIMARY KEY NOT NULL,
    "name" TEXT(100) NOT NULL,
    "description" TEXT(500) NOT NULL,
    "target_risk_category" TEXT NOT NULL,
    "created_at" TEXT NOT NULL,
    "updated_at" TEXT NOT NULL
);

-- Indexes
CREATE INDEX "ix_exercises_name" ON "exercises" ("name");
CREATE INDEX "ix_exercises_target_risk_category" ON "exercises" ("target_risk_category");
```

#### **Tabel: user_schedule_items**
```sql
CREATE TABLE "user_schedule_items" (
    "id" TEXT PRIMARY KEY NOT NULL,
    "user_id" TEXT NOT NULL,
    "exercise_name" TEXT(100) NOT NULL,
    "day" INTEGER NOT NULL CHECK("day" >= 1 AND "day" <= 7),
    "duration_minutes" INTEGER NOT NULL,
    "created_at" TEXT NOT NULL,
    "updated_at" TEXT NOT NULL,
    FOREIGN KEY ("user_id") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- Indexes
CREATE INDEX "ix_user_schedule_items_user_id" ON "user_schedule_items" ("user_id");
CREATE INDEX "ix_user_schedule_items_user_id_day" ON "user_schedule_items" ("user_id", "day");
```

#### **Tabel: mood_logs**
```sql
CREATE TABLE "mood_logs" (
    "id" TEXT PRIMARY KEY NOT NULL,
    "user_id" TEXT NOT NULL,
    "mood" INTEGER NOT NULL CHECK("mood" >= 1 AND "mood" <= 4),
    "logged_at" TEXT NOT NULL,
    "notes" TEXT(500),
    "created_at" TEXT NOT NULL,
    "updated_at" TEXT NOT NULL,
    FOREIGN KEY ("user_id") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

-- Indexes
CREATE INDEX "ix_mood_logs_user_id" ON "mood_logs" ("user_id");
CREATE INDEX "ix_mood_logs_logged_at" ON "mood_logs" ("logged_at");
CREATE INDEX "ix_mood_logs_user_id_logged_at" ON "mood_logs" ("user_id", "logged_at");
```

### Database Relationships

```
Users (1) ←→ (1) UserProfiles
  ↓
  ├── (1:N) user_schedule_items
  ├── (1:N) mood_logs
  └── UserProfiles (1) ←→ (1) health_conditions

exercises (No direct relationship, used for recommendations)
```

### Database Constraints & Rules

#### **Primary Keys**
- Semua tabel menggunakan `GUID` sebagai primary key
- Format: `TEXT` type di SQLite (UUID string)

#### **Foreign Key Constraints**
- `CASCADE DELETE`: Jika user dihapus, semua data terkait ikut terhapus
- `UserProfiles.UserId` → `Users.Id`
- `health_conditions.user_profile_id` → `UserProfiles.Id`
- `user_schedule_items.user_id` → `Users.Id`
- `mood_logs.user_id` → `Users.Id`

#### **Data Validation**
- **Email**: Unique constraint
- **Age**: Range 1-150
- **Height**: Range 50-300 cm
- **Weight**: Range 20-500 kg
- **Day**: Range 1-7 (Monday=1, Sunday=7)
- **Mood**: Range 1-4 (1=Buruk, 4=SangatBaik)

#### **Indexing Strategy**
- **Performance indexes**: User queries, date ranges
- **Unique constraints**: Email, UserProfile per User
- **Composite indexes**: User+Day for schedules, User+Date for moods

### Sample Database Commands

#### **View All Tables**
```sql
SELECT name FROM sqlite_master WHERE type='table';
```

#### **Count Records**
```sql
SELECT 'Users' as table_name, COUNT(*) as count FROM Users
UNION ALL
SELECT 'UserProfiles', COUNT(*) FROM UserProfiles
UNION ALL
SELECT 'health_conditions', COUNT(*) FROM health_conditions
UNION ALL
SELECT 'exercises', COUNT(*) FROM exercises
UNION ALL
SELECT 'user_schedule_items', COUNT(*) FROM user_schedule_items
UNION ALL
SELECT 'mood_logs', COUNT(*) FROM mood_logs;
```

#### **User Summary Query**
```sql
SELECT 
    u.Email,
    up.FullName,
    up.Age,
    ROUND(up.WeightKg / (up.HeightCm/100.0 * up.HeightCm/100.0), 2) as BMI,
    COUNT(DISTINCT usi.id) as ScheduleItems,
    COUNT(DISTINCT ml.id) as MoodLogs
FROM Users u
LEFT JOIN UserProfiles up ON u.Id = up.UserId
LEFT JOIN user_schedule_items usi ON u.Id = usi.user_id
LEFT JOIN mood_logs ml ON u.Id = ml.user_id
WHERE u.IsActive = 1
GROUP BY u.Id, u.Email, up.FullName, up.Age, up.WeightKg, up.HeightCm;
```

## 📝 Catatan Pengembangan

- **Database**: Saat ini menggunakan SQLite untuk development. Untuk production, bisa beralih ke PostgreSQL
- **Security**: JWT token sudah implement dengan expiry time. Pastikan refresh token mechanism untuk production
- **Validation**: Semua input sudah divalidasi dengan Data Annotations dan FluentValidation
- **Error Handling**: Implemented global error handling dengan response yang konsisten
- **Logging**: Menggunakan built-in ASP.NET Core logging dengan Serilog untuk production logging

## 🎯 Roadmap

### Future Enhancements
- [ ] Implementasi refresh token mechanism
- [ ] Social media authentication (Google, Facebook)
- [ ] Push notification untuk reminder olahraga
- [ ] Data analytics dan insights
- [ ] Export data ke PDF/Excel
- [ ] Integrasi dengan wearable devices
- [ ] Multi-language support
- [ ] Offline mode untuk mobile app

## 👥 Kontribusi

1. Fork repository
2. Buat feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push ke branch (`git push origin feature/AmazingFeature`)
5. Buat Pull Request

## 📄 Lisensi

Project ini menggunakan MIT License. Lihat file `LICENSE` untuk detail lengkap.

---

**Bugar Rogo Jiwo** - Aplikasi fitness yang memahami kebutuhan kesehatan Indonesia dengan pendekatan yang holistik dan personal.

Dibuat dengan ❤️ menggunakan ASP.NET Core 8 dan Clean Architecture.

## 🔒 Authentication & Authorization

API menggunakan JWT Bearer tokens untuk authentication. Setelah login berhasil, client akan menerima token yang harus disertakan dalam header Authorization untuk endpoint yang dilindungi.

```http
Authorization: Bearer <jwt-token>
```

## 📖 Swagger Documentation

Swagger UI tersedia di root URL (/) ketika aplikasi berjalan dalam mode development. UI ini menyediakan:

- Interactive API documentation
- Request/response examples
- JWT authentication support
- Try-it-out functionality

## 🗂️ Struktur Project

```
Burjo/
├── src/
│   ├── Burjo.Core/
│   │   ├── Entities/
│   │   │   └── User.cs
│   │   ├── Enums/
│   │   │   └── FitnessEnums.cs
│   │   ├── Interfaces/
│   │   │   ├── IUserRepository.cs
│   │   │   └── IAuthService.cs
│   │   ├── Models/
│   │   │   └── JwtSettings.cs
│   │   └── DTOs/
│   │       └── UserDtos.cs
│   ├── Burjo.Infrastructure/
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   └── Configurations/
│   │   │       └── UserConfiguration.cs
│   │   ├── Repositories/
│   │   │   └── UserRepository.cs
│   │   └── Services/
│   │       └── AuthService.cs
│   └── Burjo.WebApi/
│       ├── Controllers/
│       │   ├── BaseApiController.cs
│       │   ├── AuthController.cs
│       │   ├── UsersController.cs
│       │   └── HealthController.cs
│       ├── Program.cs
│       ├── appsettings.json
│       └── appsettings.Development.json
└── Burjo.sln
```

## 🔄 Development Workflow

1. **Add new entities** di `Burjo.Core/Entities/`
2. **Define interfaces** di `Burjo.Core/Interfaces/`
3. **Implement repositories** di `Burjo.Infrastructure/Repositories/`
4. **Create DTOs** di `Burjo.Core/DTOs/`
5. **Build controllers** di `Burjo.WebApi/Controllers/`
6. **Add migrations** dengan `dotnet ef migrations add <MigrationName>`
7. **Update database** dengan `dotnet ef database update`

## 🚧 Pengembangan Selanjutnya

API ini merupakan foundation yang dapat dikembangkan lebih lanjut dengan fitur:

- **Workout Management**: Entities untuk workout plans, exercises, sessions
- **Nutrition Tracking**: Food logging, calorie tracking
- **Progress Monitoring**: Weight tracking, measurements, photos
- **Social Features**: Friends, challenges, leaderboards
- **Push Notifications**: Workout reminders, achievement notifications
- **File Upload**: Profile pictures, progress photos
- **Advanced Analytics**: Detailed reporting and insights

## 📝 Environment Variables

Untuk production, gunakan environment variables untuk konfigurasi sensitif:

```bash
export ConnectionStrings__DefaultConnection="Host=prod-host;Database=BurjoDb;Username=prod-user;Password=prod-pass"
export JwtSettings__Secret="your-production-secret-key"
```

## 🧪 Testing

API dapat ditest menggunakan:
- **Swagger UI**: Interactive testing di browser
- **Postman**: Import OpenAPI specification
- **cURL**: Command line testing
- **Unit Tests**: Akan ditambahkan pada fase selanjutnya

## 📄 License

This project is proprietary software for Bugar Rogo Jiwo application.

---

**Bugar Rogo Jiwo API** - Backend solution for fitness mobile application built with modern .NET technologies and Clean Architecture principles.
