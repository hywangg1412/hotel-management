# Hotel Management System (FUMiniHotelSystem)

## Assignment Overview
**PRN212 Assignment 02** - Building a Hotel Management System with Windows Presentation Foundation (WPF)

## Project Description
FUMiniHotelSystem is a comprehensive hotel management application that helps streamline and automate various aspects of hotel operations including room reservations, customer management, and financial transactions.

## Key Features
- **Online Booking & Reservation**: Real-time room availability and booking confirmations
- **Room Management**: Efficient management of rooms and their occupants
- **Customer Management**: Database of customer information with booking history
- **Authentication System**: Admin and Customer role-based access control

## Technical Requirements

### Development Tools
- Visual Studio 2019 or above (.NET 5/6/7/8)
- MSSQL Server 2012 or above
- Windows Presentation Foundation (WPF)

### Architecture & Patterns
- **3-Layer Architecture**: Presentation, Business Logic, Data Access
- **MVVM Pattern**: Model-View-ViewModel implementation
- **Repository Pattern**: Data access abstraction
- **Singleton Pattern**: Resource management
- **Entity Framework Core**: ORM for database operations
- **LINQ**: Data querying and sorting

### Database Design
- **Room Types**: One-to-many relationship with rooms
- **Booking Reservations**: Many-to-many relationship with rooms
- **Customer Bookings**: One customer can have multiple reservations

## User Roles & Permissions

### Admin Account
- **Default Credentials**: admin@FUMiniHotelSystem.com / @@abc123@@
- **Permissions**:
  - Manage customer information (CRUD operations)
  - Manage room information (CRUD operations)
  - Manage booking reservations and details
  - Generate statistical reports by date range
  - Delete rooms (only if not associated with bookings)

### Customer Account
- **Permissions**:
  - Manage personal profile
  - View booking reservation history

## Core Functions

### CRUD Operations
All management modules support:
- **Create**: Add new records via popup dialogs
- **Read**: View existing records
- **Update**: Modify records via popup dialogs
- **Delete**: Remove records with confirmation
- **Search**: Find specific records

### Data Validation
- Type validation for all input fields
- Form validation and error handling

### Reporting
- Statistical reports by date range
- Data sorting in descending order

## Project Structure
```
StudentName_ClassCode_A02.sln
├── StudentNameWPF (WPF Application)
├── Class Library (.dll) projects
└── Database: FUMiniHotelManagement
```

## Configuration
- Database connection string stored in `appsettings.json`
- Default login window as startup interface
- Repository pattern for all database connections

## Development Notes
- No direct database connections from WPF
- All database operations through Repository and Data Access Objects
- Popup dialogs for Create/Update operations
- Confirmation dialogs for Delete operations
- Comprehensive data validation implementation
