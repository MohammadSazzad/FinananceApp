# 💰 Finance App - Personal Finance Management System

[![.NET Version](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-purple.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Supabase-green.svg)](https://supabase.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A modern, professional personal finance management application built with ASP.NET Core 9.0, featuring real-time data storage with Supabase PostgreSQL, comprehensive user authentication, and an intuitive responsive UI.

## 🎯 Project Overview

FinanceApp is a full-stack web application that helps users track their expenses, manage their financial data, and maintain complete control over their personal finances. The application features a professional UI with advanced security measures including Row Level Security (RLS) policies.

## ✨ Key Features

### 🔐 **Authentication & Security**
- **JWT-based Authentication** with secure session management
- **Row Level Security (RLS)** policies for data isolation
- **Role-based Authorization** (User/Admin roles)
- **Password Change** functionality with validation
- **Session Management** with automatic token refresh

### 👤 **User Management**
- **User Registration** with email validation
- **Professional Profile Management** with avatar support
- **Comprehensive Settings Panel** with 4-tab interface
- **Account Statistics** and activity tracking
- **Profile Information** updates with real-time validation

### 💳 **Expense Management**
- **Complete CRUD Operations** for expense management (MVC + API)
- **Category-based Organization** of expenses
- **Real-time Data Storage** with Supabase PostgreSQL
- **User-specific Data Isolation** through RLS policies
- **Expense History** with detailed views and statistics
- **Chart Data API** for visualization
- **Expense Summary** with category breakdown
- **RESTful API** for third-party integrations

### 🎨 **Modern UI/UX**
- **Professional Gradient Design** with consistent theming
- **Responsive Bootstrap 5** layout for all devices
- **Fixed Header Navigation** with active states and dropdowns
- **Animated Transitions** and hover effects throughout
- **Font Awesome Icons** for enhanced visual appeal
- **Mobile-optimized** navigation and forms
- **Professional Settings Interface** with tabbed navigation
- **Floating Labels** and modern form design

### 🌐 **API Documentation**
- **Scalar API Documentation** with interactive endpoints
- **OpenAPI 3.0 Specification** for all controllers
- **RESTful API Design** with proper HTTP status codes
- **Real-time API Testing** through Scalar interface

## 🏗️ Technology Stack

### **Backend**
- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core 9.0.7** - ORM for database operations
- **Npgsql 9.0.4** - PostgreSQL data provider
- **JWT Authentication** - Secure token-based authentication
- **BCrypt** - Password hashing and validation

### **Database**
- **Supabase PostgreSQL** - Cloud-hosted PostgreSQL database
- **Row Level Security (RLS)** - Database-level security policies
- **Entity Framework Migrations** - Database schema management
- **Connection Pooling** - Optimized database connections

### **Frontend**
- **Razor Views** - Server-side rendered UI
- **Bootstrap 5** - Responsive CSS framework
- **Font Awesome 6** - Icon library
- **jQuery** - JavaScript enhancements
- **Custom CSS** - Professional styling and animations

### **API Documentation**
- **Scalar.AspNetCore** - Modern API documentation
- **OpenAPI 3.0** - API specification standard
- **Interactive Testing** - Built-in API testing tools

### **Development Tools**
- **.NET CLI** - Command-line interface
- **Entity Framework Tools** - Database management
- **Hot Reload** - Development efficiency

## 📋 Project Structure

```
FinanceApp/
├── Controllers/
│   ├── BaseController.cs           # Base controller with authentication context
│   ├── HomeController.cs           # Landing page and error handling
│   ├── UsersController.cs          # User management (MVC)
│   ├── UsersApiController.cs       # User management (API)
│   ├── ExpensesController.cs       # Expense management (MVC)
│   └── ExpensesApiController.cs    # Expense management (API)
├── Data/
│   ├── FinanaceAppContext.cs      # Entity Framework DbContext
│   └── Service/
│       ├── IExpensesService.cs    # Expense service interface
│       ├── ExpenseService.cs      # Expense business logic
│       ├── IUserService.cs        # User service interface
│       └── UserService.cs         # User authentication and management
├── Models/
│   ├── User.cs                    # User entity model
│   ├── Expense.cs                 # Expense entity model
│   ├── ErrorViewModel.cs          # Error handling model
│   └── DTO/
│       ├── UserRegistrationDTO.cs # User registration data transfer object
│       ├── UserLoginDTO.cs        # User login data transfer object
│       └── LoginResponse.cs       # Login response model
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml         # Master layout with fixed header/footer
│   │   └── Error.cshtml           # Error page template
│   ├── Home/
│   │   ├── Index.cshtml           # Professional landing page
│   │   └── Privacy.cshtml         # Professional privacy policy page
│   └── Users/
│       ├── Register.cshtml        # Professional registration form
│       ├── Login.cshtml           # Professional login form
│       ├── Profile.cshtml         # Comprehensive user profile page
│       └── Settings.cshtml        # 4-tab professional settings interface
├── wwwroot/
│   ├── css/
│   │   └── site.css              # Custom professional styling and themes
│   ├── js/
│   │   └── site.js               # Client-side JavaScript enhancements
│   └── lib/                      # Third-party libraries (Bootstrap, jQuery, etc.)
├── Properties/
│   └── launchSettings.json       # Development server configuration
├── Migrations/                   # Entity Framework database migrations
├── Documentation/
│   ├── README.md                 # This comprehensive documentation
│   ├── SUPABASE_SETUP.md        # Supabase integration guide
│   ├── RLS_SETUP_GUIDE.md       # Row Level Security implementation
│   ├── USERS_RLS_GUIDE.md       # Users table RLS policies
│   ├── complete-rls-policies.sql # All RLS policies for production
│   ├── expense-select-policy.sql # Expense-specific RLS policies
│   └── setup-supabase.sh        # Automated Supabase setup script
├── appsettings.json              # Main application configuration
├── appsettings.Development.json  # Development environment overrides
├── appsettings.Production.json   # Production environment configuration
├── Program.cs                    # Application entry point and service configuration
└── FinanceApp.csproj            # Project dependencies and build configuration
```

## 🚀 Quick Start

### **Prerequisites**
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Supabase Account](https://supabase.com/) with PostgreSQL database
- [Git](https://git-scm.com/) for version control

### **1. Clone the Repository**
```bash
git clone https://github.com/MohammadSazzad/FinananceApp.git
cd FinanceApp
```

### **2. Configure Database Connection**
Update your Supabase connection string in `appsettings.json` and `appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=aws-0-ap-south-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.osytnqmhrsafbmgjvdlx;Password=YOUR_SUPABASE_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
  }
}
```

### **3. Set Up Database Schema**
```bash
# Create and apply database migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### **4. Configure Row Level Security (RLS)**
Execute the RLS policies in your Supabase SQL Editor (see `complete-rls-policies.sql`):

```sql
-- Enable RLS on tables
ALTER TABLE public."Users" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."Expenses" ENABLE ROW LEVEL SECURITY;

-- Apply user isolation policies
-- (See complete-rls-policies.sql for full implementation)
```

### **5. Run the Application**
```bash
# Development
dotnet run

# Production
dotnet run --environment Production
```

### **6. Access the Application**
- **Web App**: `http://localhost:5299`
- **API Documentation**: `http://localhost:5299/scalar/v1`
- **OpenAPI Spec**: `http://localhost:5299/openapi/v1.json`

## 📚 API Documentation

### **Scalar Integration**
FinanceApp includes comprehensive API documentation powered by Scalar, providing:

- **Interactive API Explorer** at `/scalar/v1`
- **Real-time Testing** of all endpoints
- **Authentication Testing** with JWT tokens
- **Request/Response Examples** for all operations
- **Schema Validation** and error handling

### **Complete API Endpoints**

#### **Authentication & Users API** (`/api/users`)
```http
GET    /api/users              # Get all users (Admin only)
GET    /api/users/{id}         # Get user by ID
POST   /api/users/register     # User registration
POST   /api/users/login        # User authentication
```

**User Registration**
```json
POST /api/users/register
Content-Type: application/json

{
  "username": "johndoe",
  "email": "john@example.com", 
  "password": "securepassword123",
  "role": "User"
}

Response (201 Created):
{
  "message": "User created successfully",
  "userId": 1
}
```

**User Login**
```json
POST /api/users/login
Content-Type: application/json

{
  "username": "johndoe",
  "password": "securepassword123"
}

Response (200 OK):
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "username": "johndoe",
  "email": "john@example.com",
  "role": "User",
  "message": "Login successful"
}
```

#### **Expenses API** (`/api/expenses`)
```http
GET    /api/expenses           # Get user's expenses (requires userId param)
GET    /api/expenses/{id}      # Get expense by ID
POST   /api/expenses           # Create new expense
GET    /api/expenses/chart     # Get chart data for user
GET    /api/expenses/summary   # Get expense summary statistics
```

**Create Expense**
```json
POST /api/expenses
Content-Type: application/json
Authorization: Bearer {jwt-token}

{
  "description": "Grocery shopping",
  "amount": 85.50,
  "category": "Food",
  "date": "2025-08-07T10:30:00Z"
}

Response (201 Created):
{
  "id": 1,
  "userId": 1,
  "description": "Grocery shopping",
  "amount": 85.50,
  "category": "Food",
  "date": "2025-08-07T10:30:00Z"
}
```

**Get Expenses**
```http
GET /api/expenses?userId=1
Authorization: Bearer {jwt-token}

Response (200 OK):
[
  {
    "id": 1,
    "userId": 1,
    "description": "Grocery shopping",
    "amount": 85.50,
    "category": "Food",
    "date": "2025-08-07T10:30:00Z"
  }
]
```

**Expense Summary**
```json
GET /api/expenses/summary
Authorization: Bearer {jwt-token}

Response (200 OK):
{
  "totalExpenses": 25,
  "totalAmount": 1250.75,
  "averageAmount": 50.03,
  "categories": [
    {
      "category": "Food",
      "count": 8,
      "total": 420.50
    },
    {
      "category": "Transportation", 
      "count": 5,
      "total": 275.25
    }
  ]
}
```

#### **Web MVC Routes** (Non-API)
```http
GET    /                       # Landing page
GET    /Users/Register         # Registration form
POST   /Users/Register         # Process registration
GET    /Users/Login            # Login form
POST   /Users/Login            # Process login
POST   /Users/Logout           # User logout
GET    /Users/Profile          # User profile page
GET    /Users/Settings         # Settings interface
POST   /Users/UpdateProfile    # Update profile
POST   /Users/ChangePassword   # Change password
GET    /Home/Privacy           # Privacy policy
```

### **API Testing with Scalar**

#### **Access Scalar Documentation**
1. **Start the application**: `dotnet run`
2. **Open Scalar UI**: Navigate to `http://localhost:5299/scalar/v1`
3. **View OpenAPI Spec**: `http://localhost:5299/openapi/v1.json`

#### **Authentication Flow**
1. **Register/Login**: Use the `/api/users/register` or `/api/users/login` endpoint
2. **Copy JWT Token**: From the login response
3. **Authorize in Scalar**: Click "Authorize" button and paste token as `Bearer {token}`
4. **Test Protected Endpoints**: All expense endpoints require authentication

#### **Error Responses**
All API endpoints return consistent error responses:

```json
// 400 Bad Request
{
  "message": "Validation failed",
  "errors": {
    "Username": ["Username is required"],
    "Email": ["Please enter a valid email address"]
  }
}

// 401 Unauthorized  
{
  "message": "Invalid credentials"
}

// 404 Not Found
{
  "message": "User with ID 1 not found"
}

// 500 Internal Server Error
{
  "message": "Internal server error: {details}"
}
```

### **Data Transfer Objects (DTOs)**

#### **UserRegistrationDTO**
```csharp
{
  "username": "string (3-100 chars, required)",
  "email": "string (valid email, required)", 
  "password": "string (min 6 chars, required)",
  "role": "string (optional, defaults to 'User')"
}
```

#### **UserLoginDTO**
```csharp
{
  "username": "string (required)",
  "password": "string (required)"
}
```

#### **LoginResponse**
```csharp
{
  "token": "string (JWT token)",
  "userId": "integer",
  "username": "string",
  "email": "string", 
  "role": "string"
}
```

#### **Expense Model**
```csharp
{
  "id": "integer",
  "userId": "integer", 
  "description": "string (max 200 chars, required)",
  "amount": "decimal (> 0, required)",
  "category": "string (max 50 chars, required)",
  "date": "datetime (ISO 8601 format)"
}
```

## 🔒 Security Implementation

### **Row Level Security (RLS)**
The application implements comprehensive database-level security:

#### **Users Table Policies**
- Users can only view/edit their own profile
- Registration is open but restricted to 'User' role
- Admin users have full access to all user data
- Account deletion is restricted for security

#### **Expenses Table Policies**
- Complete data isolation between users
- Users can only CRUD their own expenses
- Admin override for system management
- Automatic filtering at database level

#### **JWT Authentication**
```csharp
// JWT Claims Structure
{
  "nameid": "user_id",        // Used for RLS policies
  "name": "username",         // Display name
  "email": "user@email.com",  // User email
  "role": "User|Admin"        // Authorization role
}
```

## 🎨 UI/UX Features

### **Professional Design System**
- **Consistent Gradient Themes** throughout the application
- **Responsive Grid Layout** with Bootstrap 5
- **Professional Typography** and spacing
- **Intuitive Navigation** with active states
- **Mobile-First Design** approach

### **Settings Interface**
The comprehensive settings panel includes:
- **Profile Information** - Update username and email
- **Security & Password** - Change password with validation
- **Preferences** - Notification and display settings
- **Account Management** - Statistics and danger zone

### **Responsive Features**
- **Fixed Header** with collapsible mobile menu
- **Responsive Footer** with newsletter signup
- **Mobile-optimized Forms** with floating labels
- **Touch-friendly Interface** for tablets and phones

## 🛠️ Development

### **Running in Development Mode**
```bash
# Start with hot reload
dotnet watch run

# View logs
dotnet run --verbosity detailed

# Check for updates
dotnet outdated
```

### **Database Operations**
```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Generate SQL script
dotnet ef migrations script
```

### **Build and Deployment**
```bash
# Build for production
dotnet build --configuration Release

# Publish application
dotnet publish --configuration Release --output ./publish

# Run production build
dotnet ./publish/FinanceApp.dll
```

## 📁 Configuration Files

### **Database Setup Files**
- `SUPABASE_SETUP.md` - Complete Supabase integration guide
- `complete-rls-policies.sql` - All RLS policies for security
- `setup-supabase.sh` - Automated setup script
- `RLS_SETUP_GUIDE.md` - Row Level Security documentation

### **Development Configuration**
- `appsettings.json` - Main configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production configuration
- `launchSettings.json` - Development server settings

## 🧪 Testing

### **Manual Testing Checklist**
- [ ] User registration and email validation
- [ ] User login and JWT token generation
- [ ] Profile updates with session management
- [ ] Password change functionality
- [ ] Settings interface navigation
- [ ] RLS policy enforcement
- [ ] Responsive design on mobile devices
- [ ] API documentation accessibility

### **Security Testing**
- [ ] Verify RLS policies block unauthorized access
- [ ] Test JWT token expiration and refresh
- [ ] Confirm password hashing is secure
- [ ] Validate input sanitization
- [ ] Check for SQL injection prevention

## 🚀 Deployment

### **Production Deployment Steps**
1. **Configure Production Database**
   - Set up Supabase production database
   - Apply RLS policies for security
   - Configure connection strings

2. **Environment Configuration**
   - Set `ASPNETCORE_ENVIRONMENT=Production`
   - Configure production JWT settings
   - Update logging levels

3. **Build and Deploy**
   ```bash
   dotnet publish --configuration Release
   # Deploy to your hosting platform
   ```

4. **Post-Deployment Verification**
   - Test user registration and login
   - Verify RLS policy enforcement
   - Check API documentation accessibility
   - Validate responsive design

## � Deployment

### **Production Deployment Steps**
1. **Configure Production Database**
   - Set up Supabase production database
   - Apply RLS policies for security (`complete-rls-policies.sql`)
   - Configure connection strings with your actual password

2. **Environment Configuration**
   - Set `ASPNETCORE_ENVIRONMENT=Production`
   - Configure production JWT settings in `appsettings.Production.json`
   - Update logging levels for production

3. **Build and Deploy**
   ```bash
   # Build for production
   dotnet build --configuration Release
   
   # Publish application
   dotnet publish --configuration Release --output ./publish
   
   # Deploy to your hosting platform (vercel)
   ```

4. **Post-Deployment Verification**
   - Test user registration and login functionality
   - Verify RLS policy enforcement for data isolation
   - Check API documentation accessibility at `/scalar/v1`
   - Validate responsive design across devices
   - Test expense CRUD operations through both UI and API

### **Environment Variables (Production)**
```bash
# Database
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=your_supabase_connection_string

# JWT Configuration
AppSettings__Token=your_super_secure_jwt_secret_key
AppSettings__Issuer=YourAppName
AppSettings__Audience=YourAppAudience

# Logging
Logging__LogLevel__Default=Warning
```

## 📸 Screenshots

### **Professional Landing Page**
*Modern gradient design with professional navigation and responsive layout*

### **User Authentication**
*Clean login and registration forms with validation and error handling*

### **User Profile Dashboard**
*Comprehensive profile management with statistics and professional design*

### **Settings Interface**
*4-tab professional settings panel with profile, security, preferences, and account management*

### **API Documentation**
*Interactive Scalar documentation with real-time testing capabilities*

### **Mobile Responsive Design**
*Fully responsive interface optimized for mobile devices and tablets*

## �📖 Additional Documentation

## 📖 Additional Documentation

### **Generated Documentation Files**
- `USERS_RLS_GUIDE.md` - Users table RLS implementation guide
- `expense-select-policy.sql` - Expense table security policies
- `RLS_SETUP_GUIDE.md` - Complete RLS setup documentation
- `SUPABASE_SETUP.md` - Comprehensive Supabase integration guide
- `setup-supabase.sh` - Automated database setup script

### **API Documentation**
- Access interactive Scalar documentation at `/scalar/v1` when running
- OpenAPI 3.0 specification available at `/openapi/v1.json`
- Real-time API testing through Scalar interface
- Complete endpoint documentation with examples

## ✅ Feature Checklist

### **Core Features**
- [x] **User Registration & Authentication** with JWT tokens
- [x] **Professional Login Interface** with validation
- [x] **User Profile Management** with comprehensive settings
- [x] **Password Change Functionality** with security validation
- [x] **Session Management** with automatic token handling
- [x] **Role-based Authorization** (User/Admin roles)

### **Expense Management**
- [x] **Expense CRUD Operations** (Create, Read, Update, Delete)
- [x] **Category-based Organization** of expenses
- [x] **Real-time Data Storage** with Supabase PostgreSQL
- [x] **User Data Isolation** through RLS policies
- [x] **Expense Statistics** and summary reporting
- [x] **Chart Data API** for visualization

### **Security & Data Protection**
- [x] **Row Level Security (RLS)** policies for data isolation
- [x] **JWT Authentication** with secure token generation
- [x] **Password Hashing** with BCrypt encryption
- [x] **Database-level Security** enforced by PostgreSQL
- [x] **Input Validation** and sanitization
- [x] **CSRF Protection** and secure headers

### **Professional UI/UX**
- [x] **Responsive Design** with Bootstrap 5
- [x] **Professional Gradient Themes** throughout application
- [x] **Fixed Header Navigation** with active states
- [x] **Mobile-optimized Interface** for all devices
- [x] **4-tab Settings Interface** with comprehensive options
- [x] **Animated Transitions** and modern interactions
- [x] **Font Awesome Icons** and professional typography

### **API & Documentation**
- [x] **RESTful API Design** with proper HTTP methods
- [x] **Interactive Scalar Documentation** at `/scalar/v1`
- [x] **OpenAPI 3.0 Specification** for all endpoints
- [x] **Real-time API Testing** capabilities
- [x] **Comprehensive Error Handling** with proper status codes
- [x] **DTO Models** for clean API contracts

### **Production Ready Features**
- [x] **Supabase Integration** for cloud database
- [x] **Environment Configuration** for development/production
- [x] **Connection Pooling** and retry logic
- [x] **Logging Configuration** with different levels
- [x] **Build and Deployment Scripts** for production
- [x] **Documentation** for setup and maintenance

## 🎯 Project Summary

FinanceApp is a **production-ready personal finance management system** built with modern technologies and best practices. The application demonstrates:

- **Full-Stack Development** with ASP.NET Core 9.0 and PostgreSQL
- **Professional UI/UX Design** with responsive layouts and modern interactions
- **Enterprise-level Security** with RLS policies and JWT authentication
- **Comprehensive API Documentation** with interactive testing capabilities
- **Cloud Integration** with Supabase for real-time data storage
- **Mobile-First Approach** with responsive design principles

The project serves as both a **functional finance application** and a **demonstration of modern web development practices**, including proper separation of concerns, secure authentication, professional UI design, and comprehensive documentation.

### **Perfect For:**
- Personal finance management and expense tracking
- Learning modern ASP.NET Core development patterns
- Understanding Row Level Security implementation
- API design and documentation best practices
- Professional UI/UX development with Bootstrap
- Cloud database integration with Supabase

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Mohammad Sazzad**
- GitHub: [@MohammadSazzad](https://github.com/MohammadSazzad)
- Email: sazzad19@student.sust.edu

## 🙏 Acknowledgments

- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) - Web framework
- [Supabase](https://supabase.com/) - Backend-as-a-Service platform
- [Bootstrap](https://getbootstrap.com/) - CSS framework
- [Font Awesome](https://fontawesome.com/) - Icon library
- [Scalar](https://github.com/scalar/scalar) - API documentation
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM

---

## 🔧 Technical Details

### **JWT Configuration**
```json
{
  "AppSettings": {
    "Token": "MySuperSecureAndRandomKeyThatLooksJustAwesomeAndNeedsToBeVeryVeryLong!!!111oneeleven",
    "Issuer": "MyAwesomeApp",
    "Audience": "MyAwesomeAudience"
  }
}
```

### **Database Schema**
```sql
-- Users Table
CREATE TABLE "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Username" VARCHAR(100) NOT NULL UNIQUE,
    "Email" VARCHAR(100) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "Role" VARCHAR(20) DEFAULT 'User',
    "CreatedAt" TIMESTAMP DEFAULT NOW()
);

-- Expenses Table
CREATE TABLE "Expenses" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL REFERENCES "Users"("Id"),
    "Description" VARCHAR(200) NOT NULL,
    "Amount" DECIMAL(18,2) NOT NULL,
    "Category" VARCHAR(50) NOT NULL,
    "Date" TIMESTAMP DEFAULT NOW()
);
```

### **Scalar API Features**
- **Interactive Documentation** with real-time testing
- **Authentication Integration** with JWT Bearer tokens
- **Schema Validation** for request/response models
- **Error Handling** examples and status codes
- **Mobile-Responsive** documentation interface

Built with ❤️ using .NET 9.0 and modern web technologies.
