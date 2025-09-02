# Changelog

All notable changes to the Bugar Rogo Jiwo API project will be documented in this file.

## [1.3.0] - 2025-01-14

### ✨ Added
- **Time-based Chat Restrictions**: Implemented operating hours (08:00-16:00 WIB) for chat service
- **Emergency Detection**: 24/7 emergency keyword detection system that bypasses time restrictions
- **Operating Hours API**: New endpoint `/api/Chat/operating-hours` to check service availability
- **Enhanced Chat DTOs**: Added time-based properties (`IsWithinOperatingHours`, `IsEmergencyDetected`, `OperatingHoursInfo`)
- **ChatSettings Configuration**: Configurable operating hours, timezone, and emergency keywords
- **IChatTimeValidationService**: New service interface for time validation and emergency detection
- **ChatTimeValidationService**: Implementation of time validation logic with timezone handling

### 🔧 Changed
- **ChatService**: Enhanced with time validation and emergency detection logic
- **ChatController**: Updated to include operating hours validation and new endpoints
- **ChatResponseDto**: Extended with time-based status information
- **Program.cs**: Added service registrations for time validation components

### 📋 Configuration
- **Emergency Keywords**: ["darurat", "emergency", "sakit parah", "nyeri dada", "sesak napas", "pingsan", "kecelakaan"]
- **Timezone**: SE Asia Standard Time (UTC+7)
- **Operating Hours**: Monday-Sunday 08:00-16:00 WIB
- **Out-of-hours Message**: Customizable message with next available time and tips

### 🧪 Testing
- Added comprehensive testing documentation in `TIME_BASED_CHAT_TESTING.md`
- Swagger UI integration for testing time-based restrictions
- Emergency detection validation scenarios

---

## [1.2.0] - 2025-01-13

### ✨ Added
- **Exercise Recommendations API**: AI-powered exercise recommendations based on user profile
- **Mood Analytics**: Advanced mood tracking with trend analysis
- **Enhanced Health Metrics**: Comprehensive health data management
- **Improved Chatbot**: Better response handling and context awareness

### 🔧 Changed
- **Clean Architecture**: Restructured project to follow Clean Architecture principles
- **JWT Authentication**: Enhanced security with proper token validation
- **Database Schema**: Optimized database design for better performance

---

## [1.1.0] - 2025-01-12

### ✨ Added
- **Mood Tracking**: Complete CRUD operations for mood entries
- **Health Data Management**: Comprehensive health metrics tracking
- **Chatbot Integration**: Indonesian language chatbot for health consultations

### 🔧 Changed
- **API Documentation**: Enhanced Swagger documentation
- **Error Handling**: Improved error responses and validation

---

## [1.0.0] - 2025-01-11

### ✨ Added
- **Initial Release**: Basic API structure with authentication
- **User Management**: Registration and login functionality
- **JWT Authentication**: Secure token-based authentication
- **SQLite Database**: Development database setup
- **Swagger UI**: API documentation interface

### 🏗️ Architecture
- **Clean Architecture**: Domain, Infrastructure, and Presentation layers
- **ASP.NET Core 8**: Modern web API framework
- **Entity Framework Core**: ORM for database operations
- **BCrypt**: Secure password hashing

---

## Legend
- ✨ Added: New features
- 🔧 Changed: Changes in existing functionality  
- 🐛 Fixed: Bug fixes
- 📋 Configuration: Configuration changes
- 🧪 Testing: Testing improvements
- 🏗️ Architecture: Architectural changes
- 🔒 Security: Security improvements
- 📚 Documentation: Documentation updates
