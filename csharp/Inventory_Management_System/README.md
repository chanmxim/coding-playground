# Smart Inventory Management System

## Description

The Smart Inventory Management System is a dynamic, web-based platform designed for small businesses. The system allows businesses to track inventory for products across different categories, manage sales orders, and monitor low stock levels.

## Architecture

The application is built upon a robust Model-View-Controller (MVC) architecture, separating business logic, data, and presentation:

- **Presentation Layer (Views)**: Dynamic, server-rendered interfaces built using **Razor Views**. These views combine C#, HTML, CSS, and JavaScript, heavily augmented with AJAX for asynchronous, real-time data loading.

- **Business Logic Layer (Controllers)**: Handles user requests, enforces role-based access control, and processes inventory and order actions.

- **Data Layer (Models)**: Utilizes an Object-Relational Mapper(ORM) to interact with a normalized database schema, ensuring data integrity and structured relationships.

## Tech Stack

- **Framework**: ASP.NET Core MVC (.NET 9.0)

- **Language**: C#

- **Databases & ORM**: PostgreSQL, LINQ, Entity Framework Core 

- **Frontend**: Razor Views, JavaScript 

- **Security**: ASP.NET Core Identity

- **Logging**: Serilog

- **Testing**: Unit Testing (Models, Controllers, Services) 

## Key Features

- **Authentication & Roles**: Integrates ASP.NET Core Identity for secure logins, email verification, password resets, and role-based access

- **Database Migrations**: Uses Entity Framework Core for reliable, version-controlled schema updates.

- **Dynamic UI(AJAX)**: Asynchronous searches, form submissions, and real-time UI updates without full page reloads.

- **Inventory Management**: Full CRUD operations for products and categories, including automated low-stock alerts.

- **Error Handling & Logging**: Global exception handling, custom 404/500 error pages, and diagnostic logging via Serilog.

- **Search & Filtering**: Real-time product discovery by name, category, price range, and stock status.

- **Order Processing**: Seamless cart management and detailed order summary generation.
