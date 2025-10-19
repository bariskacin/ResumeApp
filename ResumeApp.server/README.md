# ResumeApp.server

This is the backend API for ResumeApp, built with ASP.NET Core.

## Features
- RESTful API for resume management
- User authentication and authorization
- CRUD operations for resumes, education, experience, and skills
- Entity Framework Core for database access

## Getting Started
1. Open the `ResumeApp.server` folder in Visual Studio or your preferred IDE.
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Update the connection string in `appsettings.json` if needed.
4. Run the server:
   ```sh
   dotnet run
   ```

## Project Structure
- `Controllers/` - API controllers
- `Models/` - Data models
- `ResumeDbContext.cs` - Entity Framework Core context
- `appsettings.json` - Configuration file

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- SQL Server (or SQLite)

## License
This project is licensed under the MIT License.

## Author
Barış KAÇİN
