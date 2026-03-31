# OutTube - Video Sharing Platform

A full-featured YouTube-like video sharing platform built with ASP.NET Core MVC, featuring real-time interactions, content moderation, and comprehensive user management.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Database Setup](#database-setup)
- [Usage](#usage)
- [Default Admin Account](#default-admin-account)
- [Real-time Features](#real-time-features)
- [API Documentation](#api-documentation)
- [Security](#security)
- [Contributing](#contributing)
- [License](#license)

## Overview

OutTube is a modern video sharing platform that allows users to upload, watch, like, comment, and subscribe to video content. The application includes a comprehensive admin panel for content moderation and user management, real-time updates via SignalR, and robust security features including JWT authentication.

## Features

### Core Video Features
- **Video Upload & Management** - Upload videos with thumbnails (up to 500MB)
- **Video Browsing** - Browse all public videos with pagination
- **Video Playback** - Stream videos with detailed metadata
- **Visibility Control** - Toggle between public and private videos
- **Trending Videos** - Sort by view count
- **Latest Videos** - Sort by upload date
- **Watch History** - Track and review watched videos

### Social Features
- **Like System** - Like and unlike videos with real-time updates
- **Comments** - Create, update, and delete comments on videos
- **Real-time Updates** - Live comment and like notifications via SignalR
- **Subscriptions** - Subscribe to channels and track subscribers
- **Video History** - Automatic tracking of watched content

### Admin Features
- **Dashboard Analytics** - View statistics for users, videos, views, and reports
- **User Management** - View all users with ban/unban capabilities
- **Video Moderation** - Manage video visibility and content
- **Report System** - Review and manage user-submitted reports
- **Content Moderation** - Remove inappropriate content and ban violators

### Reporting System
- **Report Videos** - Users can flag inappropriate content
- **Detailed Reasons** - Specify violation type and description
- **Status Tracking** - Track reports from submission to resolution
- **Admin Review** - Comprehensive review and action system

## Technology Stack

### Backend
- **.NET 10.0** 
- **ASP.NET Core MVC** - Web application framework
- **Entity Framework Core 10.0.5** - Object-relational mapping
- **SQL Server** - Primary database
- **SignalR** - Real-time bidirectional communication

### Authentication & Security
- **ASP.NET Core Identity** - User management and authentication
- **JWT (JSON Web Tokens)** - Secure token-based authentication
- **Role-based Authorization** - Admin and User role separation

### Additional Libraries
- **FluentValidation 12.1.1** - Model validation
- **TagLibSharp 2.3.0** - Video metadata extraction

## Project Structure

```
outTube/
├── Controllers/         # MVC Controllers
├── Models/             # Domain models and entities
├── Services/           # Business logic layer
├── Repositories/       # Data access layer
├── Data/              # Database context and migrations
├── Hubs/              # SignalR hubs for real-time features
├── Middlewares/       # Custom middleware components
├── ViewModels/        # View-specific models
├── Validation/        # FluentValidation validators
├── Views/             # Razor views
├── wwwroot/           # Static files (CSS, JS, images, videos)
└── Docs/              # Documentation and database schema
```

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or higher)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/outtube.git
   cd outtube
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

### Configuration

1. **Copy the template configuration file**
   ```bash
   cp appsettings-template.json appsettings.json
   ```

2. **Update `appsettings.json` with your settings**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=OutTubeDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Jwt": {
       "Key": "your-secret-key-minimum-32-characters-long",
       "Issuer": "OutTube",
       "Audience": "OutTubeUsers",
       "ExpiryInMinutes": 1440
     },
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*"
   }
   ```

### Database Setup

1. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

2. **Verify database creation**
   
   The application will automatically seed an admin account on first run.

### Running the Application

```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## Usage

### User Registration
1. Navigate to `/Account/Register`
2. Fill in the registration form
3. Submit to create your account

### Uploading Videos
1. Log in to your account
2. Navigate to `/Video/Create`
3. Provide video title, description, video file, and thumbnail
4. Submit to upload (max 500MB)

### Admin Access
1. Log in with admin credentials
2. Access admin panel at `/Admin/Index`
3. Manage users, videos, and reports

## Default Admin Account

The application seeds a default admin account on startup:

- **Email:** `admin@outtube.com`
- **Username:** `admin`
- **Password:** `Admin@123`

**Important:** Change the default admin password immediately after first login.

## Real-time Features

The application uses SignalR for real-time updates:

### Comments Hub (`/commentsHub`)
- Real-time comment creation
- Live comment updates
- Instant comment deletion notifications

### Likes Hub (`/likesHub`)
- Live like count updates
- Instant like/unlike feedback

### Subscribe Hub (`/subscribeHub`)
- Real-time subscription updates
- Live subscriber count changes

## API Documentation

### Video Endpoints
- `GET /Video/Details/{id}` - View video details
- `POST /Video/Create` - Upload new video [Authenticated]
- `DELETE /Video/Delete/{id}` - Delete video [Authenticated, Owner]

### Social Endpoints
- `POST /Like/LikeToggle/{videoId}` - Toggle like [Authenticated]
- `GET /Subscription/GetSubscripedVideos` - Get subscribed content [Authenticated]

### Admin Endpoints
- `GET /Admin/Index` - Admin dashboard [Admin Role]
- `POST /Admin/BanUser/{userId}` - Ban user [Admin Role]
- `POST /Admin/UnbanUser/{userId}` - Unban user [Admin Role]
- `POST /Admin/ToggleVisibility/{videoId}` - Toggle video visibility [Admin Role]

## Security

### Implemented Security Features
- **JWT Authentication** - Secure token-based authentication
- **HttpOnly Cookies** - XSS protection for token storage
- **Role-based Authorization** - Separate Admin and User permissions
- **Password Requirements** - Configurable password strength rules
- **Anti-forgery Tokens** - CSRF protection on forms
- **User Ban System** - Account lockout capabilities
- **File Upload Validation** - Size and type restrictions
- **Email Uniqueness** - Enforced unique email addresses

### Security Best Practices
- Change default admin credentials after deployment
- Use strong JWT secret keys (minimum 32 characters)
- Enable HTTPS in production
- Regularly update dependencies
- Review user reports promptly

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request


---

**Developed as part of ITI Training Program**
