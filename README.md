# Project Pipeline Management System

A comprehensive pipeline tracking system built with .NET Core 8 and Next.js 14.

## Architecture

- **Backend**: ASP.NET Core 8 Web API
- **Frontend**: Next.js 14 with TypeScript
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT + ASP.NET Core Identity

## Project Structure


ProjectPipelineAPI/
├── src/
│ ├── ProjectPipeline.API/ # Web API Layer
│ ├── ProjectPipeline.Core/ # Domain Layer
│ ├── ProjectPipeline.Infrastructure/ # Infrastructure Layer
│ └── ProjectPipeline.Shared/ # Shared Utilities
├── tests/ # Test Projects
└── docs/ # Documentation

## Getting Started

1. Clone the repository
2. Update connection string in appsettings.json
3. Run `dotnet ef database update` to create database
4. Run `dotnet run --project src/ProjectPipeline.API`

## Features

- Project pipeline management
- Role-based access control
- Resource tracking
- Multi-business unit support
- Comprehensive reporting

