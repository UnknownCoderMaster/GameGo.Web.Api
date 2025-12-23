# GameGo - Complete Project Context

## 📋 PROJECT OVERVIEW

**Project Name:** GameGo  
**Description:** Venue booking system for entertainment places (Computer clubs, Stadiums, Restaurants, etc.) in Uzbekistan  
**Tech Stack:** .NET 8, PostgreSQL, Clean Architecture + Vertical Slice  
**Status:** ✅ Live in Production (Railway.app)  
**API URL:** `https://gamego-production.up.railway.app`  
**Swagger:** `https://gamego-production.up.railway.app/swagger`

---

## 🏗️ ARCHITECTURE

### **Pattern:** Clean Architecture + Vertical Slice Architecture

### **Project Structure:**
```
GameGo.Web.Api/
├── GameGo.Api/                    # Presentation Layer
│   ├── Controllers/               # API Controllers
│   ├── Middleware/                # Exception handling, Logging
│   ├── Extensions/                # Service configuration
│   └── Program.cs                 # Application entry point
│
├── GameGo.Application/            # Application Layer (Use Cases)
│   ├── Features/                  # Vertical Slices
│   │   ├── Authentication/        # Login, Register, VerifyPhone
│   │   ├── Places/                # Place CRUD, Search
│   │   ├── Bookings/              # Booking management
│   │   ├── Ratings/               # Rating & Reviews
│   │   ├── Favourites/            # Favorite places
│   │   └── Users/                 # User profile management
│   ├── Common/
│   │   ├── Behaviours/            # MediatR Pipeline (Validation, Logging, etc.)
│   │   ├── Exceptions/            # Custom exceptions
│   │   └── Models/                # DTOs, Result pattern
│   └── Contracts/                 # Interfaces
│
├── GameGo.Domain/                 # Domain Layer (Core Business Logic)
│   ├── Entities/                  # Domain entities
│   ├── ValueObjects/              # Coordinate, Money
│   ├── Enums/                     # Gender, BookingStatus, etc.
│   ├── Events/                    # Domain events
│   └── Exceptions/                # Domain exceptions
│
├── GameGo.Infrastructure/         # Infrastructure Layer
│   ├── Persistence/
│   │   ├── Configurations/        # EF Core Fluent API
│   │   ├── Migrations/            # Database migrations
│   │   └── ApplicationDbContext.cs
│   ├── Identity/                  # JWT, Authentication
│   └── Services/                  # External services (Email, SMS, File)
│
└── GameGo.Shared/                 # Shared utilities

# Tests (Created but not implemented yet)
├── GameGo.Application.Tests/
├── GameGo.Domain.Tests/
├── GameGo.Infrastructure.Tests/
└── GameGo.Api.IntegrationTests/
```

### **Key Technologies:**
- **Framework:** .NET 8
- **Database:** PostgreSQL (Railway.app)
- **ORM:** Entity Framework Core 8.0
- **CQRS:** MediatR 12.2.0
- **Validation:** FluentValidation 11.9.0
- **Mapping:** AutoMapper 12.0.1
- **Authentication:** JWT Bearer tokens
- **API Documentation:** Swagger/OpenAPI
- **Logging:** Serilog
- **Deployment:** Docker + Railway.app

---

## 📊 DATABASE SCHEMA

### **Core Entities:**

**Users:**
- Id, Email (optional), PhoneNumber (unique, required), PasswordHash
- FirstName, LastName, DateOfBirth, Gender, AvatarUrl
- IsActive, IsEmailVerified, IsPhoneVerified

**Places:**
- Id, Name, Slug, Description, Address
- Latitude, Longitude (with Haversine distance calculation)
- PhoneNumber, Email, Website, Instagram, Telegram
- AverageRating, TotalRatings, TotalBookings
- PlaceTypeId (FK), OwnerId (FK)

**PlaceTypes:** (Seed data: 10 types)
- Computer Clubs, Football Stadiums, Restaurants, Cinemas, Bowling, etc.

**Bookings:**
- Id, UserId (FK), PlaceId (FK), ServiceId (FK, optional)
- BookingDate, StartTime, EndTime, NumberOfPeople
- Status (Pending, Confirmed, Cancelled, Completed, NoShow)
- TotalPrice, SpecialRequests, CancellationReason

**Ratings:**
- Id, UserId (FK), PlaceId (FK), BookingId (FK, optional)
- Score (1-5), Review, Pros, Cons
- IsAnonymous, IsVerified, HelpfulCount

**Favourites:**
- Id, UserId (FK), PlaceId (FK)
- Simple many-to-many relationship

**Games, Genres, Devices:**
- For computer clubs/gaming venues
- Many-to-many relationships (GameGenres, GameDevices, PlaceGames)

**Services:**
- Id, PlaceId (FK), Name, Description, Price, Currency
- DurationMinutes, Capacity, IsActive

**WorkingHours:**
- Id, PlaceId (FK), DayOfWeek, OpenTime, CloseTime, IsClosed

**PlaceImages:**
- Id, PlaceId (FK), ImageUrl, IsPrimary, DisplayOrder

**Notifications:**
- Id, UserId (FK), Title, Message, Type
- RelatedEntityType, RelatedEntityId, IsRead

**Verifications:**
- Id, UserId (FK), VerificationType (Email/Phone/PasswordReset)
- Code, ExpiresAt, IsUsed

---

## ✅ IMPLEMENTED FEATURES

### **1. Authentication & Authorization**
- ✅ Register with PhoneNumber + Password (Email optional)
- ✅ Login with PhoneNumber + Password (JWT tokens)
- ✅ Phone verification with SMS codes
- ✅ JWT token generation and validation
- ⚠️ SMS integration configured (Eskiz.uz) but with template limitations

**Endpoints:**
```
POST /api/auth/register
POST /api/auth/login
POST /api/auth/verify-phone
```

### **2. Places Management**
- ✅ Create place (with authentication)
- ✅ Get place by ID (with full details: images, features, working hours, services)
- ✅ Search places with filters:
  - SearchTerm (name, phone, description, address)
  - PlaceTypeId
  - MinRating
  - Location-based (Latitude, Longitude, RadiusKm) with Haversine formula
  - Pagination
- ✅ Get place types (seed data)

**Endpoints:**
```
POST   /api/places
GET    /api/places/{id}
GET    /api/places/search?searchTerm=...&placeTypeId=...&latitude=...&longitude=...&radiusKm=...
GET    /api/places/types
```

### **3. Bookings**
- ✅ Create booking (with authentication)
- ✅ Get user bookings (with pagination, status filter)
- ✅ Booking validation (working hours check, conflict detection)
- ⚠️ Booking confirmation/cancellation (handlers ready, not fully tested)

**Endpoints:**
```
POST /api/bookings
GET  /api/bookings/my-bookings?pageNumber=1&pageSize=10&status=...
```

### **4. User Profile**
- ✅ Get user profile (with statistics: total bookings, ratings, favourites)
- ✅ Update profile (FirstName, LastName, DateOfBirth, Gender)
- ✅ Change password
- ✅ Upload avatar image

**Endpoints:**
```
GET  /api/users/profile
PUT  /api/users/profile
PUT  /api/users/change-password
POST /api/users/avatar
```

### **5. Favourites**
- ✅ Add place to favourites
- ✅ Remove from favourites
- ✅ Get user favourites (with PlaceType filter, pagination)
- ✅ Check if place is favourite

**Endpoints:**
```
GET    /api/favourites?placeTypeId=1&pageNumber=1&pageSize=10
POST   /api/favourites/{placeId}
DELETE /api/favourites/{placeId}
GET    /api/favourites/check/{placeId}
```

### **6. Infrastructure**
- ✅ File upload service (local storage to wwwroot/uploads)
- ✅ JWT token service
- ✅ Identity service (password hashing, user creation)
- ✅ Email service (MailKit) - configured but not actively used
- ✅ SMS service (Eskiz.uz) - configured with test template limitations
- ✅ Mock SMS service for development
- ✅ DateTime service
- ✅ Current user service

### **7. MediatR Pipeline Behaviors**
- ✅ ValidationBehaviour - FluentValidation integration
- ✅ LoggingBehaviour - Request/Response logging
- ✅ PerformanceBehaviour - Slow query detection (>500ms)
- ✅ TransactionBehaviour - Database transaction management
- ✅ UnhandledExceptionBehaviour - Global error handling

### **8. Database**
- ✅ EF Core migrations created and applied
- ✅ Seed data implemented:
  - 10 Place Types (Computer Clubs, Restaurants, etc.)
  - 10 Game Genres (Action, RPG, etc.)
  - 8 Gaming Devices (PS5, Xbox, PC, etc.)
- ✅ Fluent API configurations for all entities
- ✅ Indexes on frequently queried columns
- ✅ Cascade delete rules configured

### **9. Deployment**
- ✅ Dockerfile created for .NET 8
- ✅ Deployed to Railway.app
- ✅ PostgreSQL database in production
- ✅ Environment variables configured
- ✅ HTTPS enabled
- ✅ Swagger UI available in production

---

## ⚠️ PARTIALLY IMPLEMENTED / ISSUES

### **1. SMS Service**
- **Status:** Configured but limited
- **Issue:** Eskiz.uz test account only allows specific message template: "Kod: {code}. Bu Eskiz dan test"
- **Solution options:**
  - Upgrade to paid Eskiz.uz account
  - Integrate Twilio (recommended for production)
  - Use mock SMS service for development

### **2. Email Service**
- **Status:** Configured but email field is optional
- **Current:** Email is not required for registration
- **Usage:** Currently not actively sending emails

### **3. Redis Cache**
- **Status:** Package installed, service interface created, but NOT configured in DependencyInjection
- **Reason:** Commented out to avoid Railway deployment issues
- **Next step:** Add Redis service in Railway and enable cache

### **4. Image Upload**
- **Status:** Working with local file storage
- **Current:** Files saved to wwwroot/uploads/{folder}
- **Limitation:** Not suitable for production scale
- **Next step:** Integrate AWS S3 or Azure Blob Storage

---

## 🔴 NOT IMPLEMENTED YET

### **1. Ratings & Reviews**
- **Commands:** CreateRating, UpdateRating, DeleteRating
- **Queries:** GetPlaceRatings, GetUserRatings
- **Features:** MarkRatingHelpful, Report inappropriate reviews
- **Status:** Domain entities and database ready, handlers NOT written

### **2. Place Management (Advanced)**
- **Commands:** 
  - AddWorkingHours, UpdateWorkingHours, DeleteWorkingHours
  - AddService, UpdateService, DeleteService
  - AddPlaceImages, DeletePlaceImage, SetPrimaryImage
  - AddPlaceFeatures, RemovePlaceFeature
  - AddGamesToPlace, RemoveGameFromPlace
- **Status:** Some handlers drafted but not fully tested

### **3. Booking Management (Advanced)**
- **Commands:**
  - ConfirmBooking (by place owner)
  - CancelBooking (with refund logic)
  - CompleteBooking
  - MarkAsNoShow
- **Queries:**
  - GetBookingById
  - GetPlaceBookings (for place owners)
  - CheckAvailability (time slot checker)
- **Status:** Basic structure exists, business logic incomplete

### **4. Games Management**
- **Commands:** CreateGame, UpdateGame, DeleteGame
- **Queries:** GetGames, GetGameById, SearchGames
- **Status:** Database schema ready, handlers NOT written

### **5. Notifications System**
- **Features:**
  - Send notification on booking confirmation
  - Send reminder before booking
  - Send promotional notifications
- **Commands:** SendNotification, MarkAsRead
- **Queries:** GetUserNotifications, GetUnreadCount
- **Status:** Database ready, handlers NOT written

### **6. Payment Integration**
- **Status:** NOT started
- **Needed:** Payme, Click, or Stripe integration
- **Tables needed:** Payments, Transactions

### **7. Analytics & Reporting**
- **Status:** NOT started
- **Features needed:**
  - Place owner dashboard
  - Booking statistics
  - Revenue reports
  - Popular places/times

### **8. Admin Panel**
- **Status:** NOT started
- **Features needed:**
  - User management
  - Place verification
  - Content moderation
  - System settings

---

## 🐛 KNOWN ISSUES & TECHNICAL DEBT

### **1. ExecuteUpdateAsync() Usage**
- **Issue:** Code uses EF Core 7+ feature `ExecuteUpdateAsync()` but might not work in all environments
- **Current solution:** Using classic `SaveChangesAsync()` pattern instead
- **Location:** Throughout handlers (e.g., UploadAvatarCommandHandler)

### **2. Nullable Reference Types**
- **Issue:** `nullable` and `implicit usings` disabled in .csproj files
- **Impact:** All code written without nullable annotations (`?`)
- **Reason:** Developer preference for explicit nullability handling

### **3. MediatR License Warning**
- **Warning:** "You do not have a valid license key for Lucky Penny software MediatR"
- **Impact:** Warning message in production logs
- **Status:** Non-critical, works fine in production

### **4. Domain Event Dispatching**
- **Status:** Infrastructure exists but minimal usage
- **Current:** Only BookingCreated, RatingCreated events defined
- **Opportunity:** Expand for notification triggering, analytics

### **5. Unit Tests**
- **Status:** Test projects created but NO tests written
- **Coverage:** 0%
- **Priority:** Medium (should add before major features)

### **6. API Versioning**
- **Status:** NOT implemented
- **Current:** All endpoints at v1 (implicit)
- **Package installed:** Asp.Versioning.Mvc but not configured

### **7. Rate Limiting**
- **Status:** Package installed (AspNetCoreRateLimit) but NOT configured
- **Risk:** API abuse possible

### **8. Health Checks**
- **Status:** Basic health endpoint exists (`/health`)
- **Packages:** AspNetCore.HealthChecks.NpgSql, Redis installed but NOT configured
- **Current:** Only returns static "Healthy" status

---

## 🔒 AUTHENTICATION FLOW

### **Current Implementation:**
1. User registers with PhoneNumber + Password (Email optional)
2. Verification code sent via SMS (optional - not enforced)
3. User can login with PhoneNumber + Password
4. JWT token returned (AccessToken + RefreshToken)
5. Token used in Authorization header: `Bearer {token}`

### **Token Structure:**
- **Claims:** userId, email (or phoneNumber if no email)
- **Expiry:** 60 minutes (configurable)
- **Refresh Token:** 7 days (stored in response, but refresh logic NOT implemented)

### **Protected Endpoints:**
- All endpoints with `[Authorize]` attribute require valid JWT token
- CurrentUserService extracts userId from token claims

---

## 📁 KEY FILES & LOCATIONS

### **Configuration:**
```
GameGo.Api/appsettings.json           # Base configuration
GameGo.Api/appsettings.Production.json # Production overrides
GameGo.Api/Program.cs                  # Application startup
Dockerfile                             # Docker build configuration
```

### **Database:**
```
GameGo.Infrastructure/Persistence/ApplicationDbContext.cs
GameGo.Infrastructure/Persistence/ApplicationDbContextFactory.cs  # Design-time factory
GameGo.Infrastructure/Persistence/ApplicationDbContextInitializer.cs  # Seed data
GameGo.Infrastructure/Persistence/Configurations/*.cs  # EF Core configurations
GameGo.Infrastructure/Migrations/*.cs  # EF migrations
```

### **Domain:**
```
GameGo.Domain/Entities/*.cs      # All domain entities
GameGo.Domain/ValueObjects/*.cs  # Coordinate, Money
GameGo.Domain/Enums/*.cs         # All enums
GameGo.Domain/Events/*.cs        # Domain events
```

### **Application:**
```
GameGo.Application/Features/{Feature}/Commands/{Command}/*.cs
GameGo.Application/Features/{Feature}/Queries/{Query}/*.cs
GameGo.Application/Common/Behaviours/*.cs  # MediatR pipeline
GameGo.Application/Contracts/*.cs          # Interfaces
```

---

## 🚀 DEPLOYMENT DETAILS

### **Railway.app Configuration:**

**Environment Variables:**
```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=Host=...;Port=5432;Database=railway;Username=postgres;Password=...;SSL Mode=Require;Trust Server Certificate=true
Jwt__Key=YourSuperSecretKey32Characters!
Jwt__Issuer=GameGoAPI
Jwt__Audience=GameGoClient
Jwt__ExpiryInMinutes=60
Cors__AllowedOrigins__0=https://gamego.uz
FileStorage__Provider=Local
```

**Resources:**
- PostgreSQL database (Railway managed)
- Container: .NET 8 runtime
- Port: 8080 (mapped to Railway's public port)

**Build Process:**
1. Railway detects Dockerfile
2. Builds Docker image with .NET SDK 8.0
3. Publishes application
4. Runs with .NET Runtime 8.0
5. Automatic HTTPS certificate
6. Database migrations run on startup

---

## 📋 NEXT STEPS (PRIORITIZED)

### **HIGH PRIORITY:**

1. **Ratings & Reviews System**
   - Implement CreateRating, GetPlaceRatings handlers
   - Test rating calculation and average updates
   - Add rating moderation (report inappropriate)

2. **Booking Confirmation Flow**
   - Implement ConfirmBooking handler for place owners
   - Add notification on booking confirmation
   - Test full booking lifecycle

3. **Redis Cache Integration**
   - Add Redis service in Railway
   - Enable cache in DependencyInjection
   - Cache frequently accessed data (place types, place details)

4. **Place Images Management**
   - Test full image upload/delete flow
   - Implement SetPrimaryImage
   - Consider cloud storage (AWS S3)

### **MEDIUM PRIORITY:**

5. **Working Hours & Services CRUD**
   - Complete CRUD handlers for WorkingHours
   - Complete CRUD handlers for Services
   - Test availability checking logic

6. **Notifications System**
   - Implement SendNotification handler
   - Add background job for booking reminders
   - Test notification delivery

7. **Unit Tests**
   - Start with domain entity tests
   - Add handler tests with mocked dependencies
   - Aim for 60%+ coverage on critical paths

8. **API Documentation**
   - Add XML comments to controllers
   - Configure Swagger to show examples
   - Create Postman collection

### **LOW PRIORITY:**

9. **Payment Integration**
   - Research Payme/Click APIs
   - Design payment flow
   - Implement payment handlers

10. **Admin Panel Backend**
    - User management endpoints
    - Place verification workflow
    - Content moderation tools

11. **Analytics Endpoints**
    - Booking statistics
    - Popular places/times
    - Revenue reporting

12. **Performance Optimization**
    - Add database query profiling
    - Implement response caching
    - Optimize N+1 query issues

---

## 🛠️ DEVELOPMENT COMMANDS

### **Local Development:**
```bash
# Run API
dotnet run --project GameGo.Api

# Run with watch (auto-reload)
dotnet watch run --project GameGo.Api

# Create migration
dotnet ef migrations add MigrationName --project GameGo.Infrastructure --startup-project GameGo.Api

# Apply migration
dotnet ef database update --project GameGo.Infrastructure --startup-project GameGo.Api

# Build
dotnet build

# Test (when tests exist)
dotnet test
```

### **Docker:**
```bash
# Build image
docker build -t gamego .

# Run container
docker run -p 8080:8080 gamego

# Run with environment variables
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="..." gamego
```

### **Railway Deployment:**
```bash
# Deploy via GitHub push (automatic)
git add .
git commit -m "Update feature"
git push origin main

# Railway CLI (alternative)
railway up
```

---

## 📞 API ENDPOINTS SUMMARY

### **Public (No Auth):**
```
GET  /health
GET  /api/places/types
GET  /api/places/{id}
GET  /api/places/search
POST /api/auth/register
POST /api/auth/login
POST /api/auth/verify-phone
```

### **Protected (Auth Required):**
```
POST   /api/places
POST   /api/bookings
GET    /api/bookings/my-bookings
GET    /api/users/profile
PUT    /api/users/profile
PUT    /api/users/change-password
POST   /api/users/avatar
GET    /api/favourites
POST   /api/favourites/{placeId}
DELETE /api/favourites/{placeId}
GET    /api/favourites/check/{placeId}
```

---

## 🎯 PROJECT GOALS

### **Short-term (Current Phase):**
- ✅ Core API functionality working
- ✅ Database schema finalized
- ✅ Basic CRUD operations for all entities
- ✅ Authentication & authorization
- ✅ Production deployment

### **Mid-term (Next 1-2 months):**
- Complete all CRUD operations
- Implement notifications
- Add Redis caching
- Payment integration
- Mobile app development starts

### **Long-term (3-6 months):**
- Admin dashboard
- Analytics & reporting
- Push notifications
- SMS/Email campaigns
- Scale to 10,000+ users

---

## 💡 DEVELOPER NOTES

### **Code Style:**
- No nullable reference types (`?` not used)
- Explicit using statements (no implicit usings)
- FluentValidation for all commands
- Result pattern for error handling (no exceptions in business logic)
- Private setters on entities (encapsulation)
- Factory methods for entity creation (e.g., `User.Create()`)

### **Patterns Used:**
- Clean Architecture (layered)
- Vertical Slice Architecture (feature-based)
- CQRS (Command Query Responsibility Segregation)
- Repository pattern (minimal, mostly DbContext direct)
- Domain-Driven Design (Rich Domain Model)
- Pipeline pattern (MediatR behaviors)

### **Best Practices:**
- Validators co-located with commands
- One handler per command/query
- DTOs separate from entities
- Mapping via AutoMapper
- Logging at infrastructure boundaries
- Domain events for cross-feature communication

---

## 🔗 USEFUL LINKS

- **Production API:** https://gamego-production.up.railway.app
- **Swagger UI:** https://gamego-production.up.railway.app/swagger
- **Railway Dashboard:** https://railway.app/project/...
- **GitHub Repo:** (Add your repo URL here)

---

## 📄 LICENSE & CREDITS

**Project:** GameGo - Venue Booking System  
**Developer:** ORZIBEK  
**Framework:** .NET 8  
**Database:** PostgreSQL  
**Hosting:** Railway.app  

---

**Last Updated:** December 4, 2024  
**Version:** 1.0 (Production)  
**Status:** ✅ Live and operational