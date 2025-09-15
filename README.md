📘 InfinionInterviewProject

A .NET 9 Web API microservice built with Onion Architecture to support customer onboarding for a digital banking system.

The solution demonstrates clean architecture practices, secure JWT authentication, external API integration, and Entity Framework Core with SQL Server. It includes mocked services for OTP verification and email/SMS delivery.

✨ Features

✅ Customer onboarding with phone, email, state, and LGA validation

✅ OTP verification (mocked service)

✅ State/LGA mapping from external API (nga-states-lga.onrender.com)

✅ Bank listing via external API (alat-tech-test-api)

✅ Secure JWT Authentication (Login / Protect APIs)

✅ Onion architecture: Domain, Application, Infrastructure, API

✅ EF Core + SQL Server (with migrations & seeding)

✅ Swagger/OpenAPI documentation

✅ Unit tests for services and controllers

✅ Extensible infrastructure with fake email/SMS service

🏗️ Project Structure (Onion Architecture)
InfinionInterviewProject
│
├── src/
│   ├── InfinionInterviewProject.API             → Web API Layer (Controllers, Program.cs, JWT Middleware)
│   ├── InfinionInterviewProject.Application     → Business Logic (DTOs, Interfaces, Services)
│   ├── InfinionInterviewProject.Domain          → Entities & Core Business Rules
│   ├── InfinionInterviewProject.Infrastructure  → EF Core, Repositories, Seeders, Fake Email/SMS
│
├── tests/
│   ├── InfinionInterviewProject.UnitTests       → xUnit-based unit tests
│
└── README.md

⚙️ Prerequisites

.NET 9 SDK

SQL Server LocalDB or Full SQL Server

Postman
 or Swagger UI
 for API testing

🚀 Getting Started
1. Clone Repository
https://github.com/GreenZoneResources/InfinionProjectInterview.git
cd InfinionInterviewProject/src/InfinionInterviewProject.API

2. Database Setup

Update your appsettings.json with your SQL Server connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InfinionInterviewDb;Trusted_Connection=True;"
}


Run migrations:

dotnet ef database update --project ../InfinionInterviewProject.Infrastructure --startup-project .

3. JWT Configuration

In appsettings.json:

"JwtSettings": {
  "Key": "SuperSecretKeyMustBeAtLeast256BitsLongForHS256",
  "Issuer": "InfinionInterviewProject",
  "Audience": "InfinionInterviewUsers",
  "ExpireMinutes": 60
}

4. Run Application
dotnet run


Swagger will be available at:

https://localhost:7191/swagger

🔐 Authentication Flow (JWT)

Onboard Customer → Generates OTP and sends via fake SMS/Email service

Verify OTP → Marks customer as active

Login → Returns JWT token

Use JWT in Swagger or Postman by entering it under Authorization → Bearer Token

📡 API Endpoints
1. Customer Onboarding

POST /api/customers/onboard

{
  "phoneNumber": "08012345678",
  "email": "test@example.com",
  "password": "StrongP@ssword1",
  "state": "Lagos",
  "lga": "Agege"
}


Business Rules:

✅ Phone number verified with OTP (mocked)

✅ LGA must match selected state

2. Get All Customers

GET /api/customers

3. Get States & LGAs

GET /api/states

GET /api/states/{state}/lgas

(Uses nga-states-lga.onrender.com API)

4. Get Banks

GET /api/banks

Consumes external API:
alat-tech-test-api

5. Customer Login (JWT)

POST /api/customers/login

{
  "email": "test@example.com",
  "password": "StrongP@ssword1"
}


Response:

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}

🧪 Unit Testing

Run tests:

cd tests/InfinionInterviewProject.UnitTests
dotnet test


Test coverage includes:

✅ CustomerService (onboard, login, get by email)

✅ StateService (fetch states/LGAs)

✅ BankService (consume external banks API)

✅ Controller integration tests

📧 Mock Services

FakeEmailSmsService logs OTPs to console (simulating delivery).

Replace with real SMTP/Twilio later.

📖 Use Case Documentation
Use Case 1: Onboard Customer

Actor: New banking customer
Flow:

Submit phone, email, password, state, and LGA

System generates OTP and "sends" via FakeEmailSmsService

Customer enters OTP → system verifies

Customer becomes active

Use Case 2: Login Customer

Actor: Registered banking customer
Flow:

Customer submits email + password

System validates credentials

Issues JWT token with claims: Name, Email, Id

Use Case 3: View Banks

Actor: Authenticated user
Flow:

Calls /api/banks

System fetches from Wema Alat API

Returns structured list of banks

👨‍💻 Author

InfinionInterviewProject
Built for Infinion Banking Technical Interview
