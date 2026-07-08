# Gym Management System with Smart Fitness Coach

A web-based Gym Management System built with ASP.NET Core MVC and SQL Server.  
The system is designed to manage gym members, trainers, employees, memberships, equipment, pending payments, and role-based administration from a single platform.

It also includes a Smart AI Fitness Coach integrated with OpenRouter API to provide personalized workout, food, recovery, and safety-related fitness guidance based on user profile information.

---

## Key Features

### Admin & Management
- Admin dashboard with summary cards
- User/member management
- Employee management
- Trainer management
- Equipment management
- Blog management
- Role-based user management

### Membership & Payment
- Membership plan tracking
- Active, pending, and expired membership status
- Pending cash payment approval
- Membership history tracking

### Trainer & Member Features
- Dedicated member profile
- Trainer assignment
- Assigned student management for trainers
- Member fitness, health, trainer, and membership information display

### Smart AI Fitness Coach
- AI-powered fitness assistant using OpenRouter API
- Personalized workout guidance
- Food and recovery suggestions
- Safety-aware fitness advice based on health notes
- Supports user queries related to workout plans, exercise guidance, and fitness improvement

---

## Technology Stack

- **Language:** C#
- **Framework:** ASP.NET Core MVC 9 / .NET 9
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Frontend:** HTML, CSS, Bootstrap, JavaScript
- **AI Integration:** OpenRouter API
- **IDE:** Visual Studio
- **Version Control:** Git & GitHub

---

## Project Architecture

The project follows a layered structure to separate concerns and keep the application organized.

```text
GYM.Mi.Web
├── GYM.Mi
│   ├── GYM.Application
│   ├── GYM.Domain
│   ├── GYM.Infrastructure
│   ├── GYM.Mi
│   └── GYM.Mi.sln
├── screenshots
└── README.md
