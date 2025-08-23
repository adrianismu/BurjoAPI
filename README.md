# Bugar Rogo Jiwo API

API backend untuk aplikasi mobile kebugaran "Bugar Rogo Jiwo" yang dibangun menggunakan ASP.NET Core 8 dengan Clean Architecture.

## 🏗️ Arsitektur

Proyek ini menggunakan **Clean Architecture** yang terdiri dari:

- **Burjo.Core**: Domain layer - berisi entities, enums, dan interfaces
- **Burjo.Infrastructure**: Infrastructure layer - implementasi repositories, services, dan data access
- **Burjo.WebApi**: Presentation layer - API controllers dan konfigurasi startup

## 🛠️ Teknologi Stack

- **Framework**: ASP.NET Core 8
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **Password Hashing**: BCrypt
- **Documentation**: Swagger/OpenAPI
- **Architecture Pattern**: Clean Architecture

## 📦 Paket NuGet

### Burjo.WebApi
- Swashbuckle.AspNetCore (Swagger)
- Microsoft.EntityFrameworkCore.Design
- Microsoft.AspNetCore.Authentication.JwtBearer

### Burjo.Infrastructure
- Npgsql.EntityFrameworkCore.PostgreSQL
- BCrypt.Net-Next
- System.IdentityModel.Tokens.Jwt
- Microsoft.IdentityModel.Tokens

## 🚀 Setup dan Instalasi

### 1. Prerequisites
- .NET 8 SDK
- PostgreSQL
- Visual Studio Code atau Visual Studio

### 2. Clone dan Setup Database

```bash
# Clone repository
git clone <repository-url>
cd Burjo

# Setup database PostgreSQL
# Buat database dengan nama: BurjoDb_Dev (untuk development)

# Update connection string di appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=BurjoDb_Dev;Username=your_username;Password=your_password"
  }
}
```

### 3. Run Migrations

```bash
cd src/Burjo.WebApi
dotnet ef database update --project ../Burjo.Infrastructure
```

### 4. Run Application

```bash
cd src/Burjo.WebApi
dotnet run
```

API akan berjalan di:
- **HTTPS**: https://localhost:7289
- **HTTP**: http://localhost:5289
- **Swagger UI**: https://localhost:7289 (root URL)

## 📚 API Endpoints

### Authentication

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "expiresAt": "2025-08-24T10:30:00Z",
  "profile": null
}
```

#### Login User
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "email": "user@example.com",
  "expiresAt": "2025-08-24T10:30:00Z",
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

### Profile Management (Requires Authentication)

#### Get User Profile
```http
GET /api/profile
Authorization: Bearer <jwt-token>
```

**Response:**
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

#### Update/Create User Profile
```http
PUT /api/profile
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "fullName": "John Doe",
  "age": 30,
  "gender": "Laki-laki",
  "heightCm": 175,
  "weightKg": 70,
  "medicalHistory": "Hipertensi",
  "fitnessLevel": "Sedang"
}
```

### Health Check

#### Health Status
```http
GET /api/health
```

#### System Info
```http
GET /api/health/info
```

## 🔧 Konfigurasi

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "Secret": "your-super-secret-key-here",
    "Issuer": "BurjoApi",
    "Audience": "BurjoApp",
    "ExpiryHours": 24
  }
}
```

### Database Connection
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=BurjoDb;Username=postgres;Password=your_password"
  }
}
```

## 📊 Data Models

### User Entity
- **Id**: Guid (Primary key, auto-generated)
- **Email**: Unique user email
- **PasswordHash**: BCrypt hashed password
- **IsActive**: Soft delete flag
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp
- **Profile**: Navigation property to UserProfile

### UserProfile Entity
- **Id**: Guid (Primary key, auto-generated)
- **FullName**: User's full name
- **Age**: User's age
- **Gender**: Gender string (e.g., "Laki-laki", "Perempuan")
- **HeightCm**: Height in centimeters
- **WeightKg**: Weight in kilograms
- **MedicalHistory**: Medical conditions/history
- **FitnessLevel**: Fitness level (e.g., "Rendah", "Sedang", "Tinggi")
- **CreatedAt**: Creation timestamp
- **UpdatedAt**: Last update timestamp
- **UserId**: Foreign key to User
- **User**: Navigation property to User

### Computed Properties
- **Bmi**: Body Mass Index calculation (WeightKg / (HeightCm/100)²)

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
