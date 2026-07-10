# Gym Management System with Smart Fitness Coach

A web-based Gym Management System built with **ASP.NET Core MVC**, **.NET 9**, and **SQL Server**.

The system is designed to manage gym members, trainers, employees, memberships, equipment, pending payments, blogs, and role-based administration from a single platform.

It also includes a **Smart AI Fitness Coach** integrated with **OpenRouter API** to provide personalized workout, nutrition, recovery, and safety-related fitness guidance based on member profile information.

---

<a id="quick-navigation"></a>

## Quick Navigation

<p align="center">
  <a href="#key-features">
    <img src="https://img.shields.io/badge/Key%20Features-%E2%86%92-0D6EFD?style=for-the-badge" />
  </a>
  <a href="#technology-stack">
    <img src="https://img.shields.io/badge/Technology%20Stack-%E2%86%92-198754?style=for-the-badge" />
  </a>
  <a href="#project-architecture">
    <img src="https://img.shields.io/badge/Architecture-%E2%86%92-6F42C1?style=for-the-badge" />
  </a>
  <a href="#screenshots">
    <img src="https://img.shields.io/badge/Screenshots-%E2%86%92-FD7E14?style=for-the-badge" />
  </a>
  <a href="#how-to-run-locally">
    <img src="https://img.shields.io/badge/Run%20Locally-%E2%86%92-DC3545?style=for-the-badge" />
  </a>
  <a href="#configuration">
    <img src="https://img.shields.io/badge/Configuration-%E2%86%92-20C997?style=for-the-badge" />
  </a>
  <a href="#database-setup">
    <img src="https://img.shields.io/badge/Database%20Setup-%E2%86%92-0DCAF0?style=for-the-badge" />
  </a>
  <a href="#demo-login-credentials">
    <img src="https://img.shields.io/badge/Demo%20Login-%E2%86%92-6610F2?style=for-the-badge" />
  </a>
  <a href="#ai-fitness-coach-integration">
    <img src="https://img.shields.io/badge/AI%20Fitness%20Coach-%E2%86%92-FF69B4?style=for-the-badge" />
  </a>
  <a href="#project-status">
    <img src="https://img.shields.io/badge/Project%20Status-%E2%86%92-6C757D?style=for-the-badge" />
  </a>
  <a href="#future-improvements">
    <img src="https://img.shields.io/badge/Future%20Improvements-%E2%86%92-17A2B8?style=for-the-badge" />
  </a>
  <a href="#author">
    <img src="https://img.shields.io/badge/Author-%E2%86%92-343A40?style=for-the-badge" />
  </a>
</p>

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

<p align="right">
  <a href="#quick-navigation">
    <img src="https://img.shields.io/badge/Back%20to%20Top-%E2%86%91-555555?style=for-the-badge" />
  </a>
</p>

---

## Technology Stack

- **Language:** C#
- **Framework:** ASP.NET Core MVC / .NET 9
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Architecture:** Clean Architecture, Repository Pattern, Unit of Work
- **Dependency Injection:** Autofac
- **Object Mapping:** AutoMapper
- **Logging:** Serilog
- **Frontend:** HTML, CSS, Bootstrap, JavaScript
- **AI Integration:** OpenRouter API
- **IDE:** Visual Studio
- **Version Control:** Git & GitHub

<p align="right">
  <a href="#quick-navigation">
    <img src="https://img.shields.io/badge/Back%20to%20Top-%E2%86%91-555555?style=for-the-badge" />
  </a>
</p>

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
