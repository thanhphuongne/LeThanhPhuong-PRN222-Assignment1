# FU News Management System

## Project Overview
The News Management System (NMS), named FUNewsManagementSystem, is designed for universities and educational institutions to manage, organize, and publish news content.

## Features
- Account Management (Admin, Staff, Lecturer roles)
- News Article Management with CRUD operations
- Category Management
- Tag Management
- Authentication and Authorization
- Search and Filter functionality
- Statistical Reports
- Profile Management

## Architecture
- 3-Layer Architecture (Presentation, Business, Data Access)
- Repository Pattern
- Singleton Pattern
- Entity Framework Core
- ASP.NET Core MVC

## Default Admin Account
- Email: admin@FUNewsManagementSystem.org
- Password: @@abc123@@

## Database
- MSSQL Server
- Entity Framework Core Code First approach
- Connection string in appsettings.json

## Roles and Permissions
- **Admin (Role 1)**: Manage accounts, view reports
- **Staff (Role 2)**: Manage categories, news articles, own profile
- **Lecturer (Role 3)**: View active news articles
- **Public**: View active news articles (no authentication required)
