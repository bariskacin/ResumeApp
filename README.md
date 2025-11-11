# ResumeApp

ResumeApp is a full-stack web application for creating, managing, and sharing professional resumes. Built with Angular for the frontend and ASP.NET for the backend, it provides a seamless experience for users to showcase their education, experience, and skills.

## Features
- User authentication and authorization
- Create, edit, and delete resumes
- Add education, work experience, and skills
- Responsive design for desktop and mobile
- RESTful API backend
- Secure data storage

## Project Structure
- `ResumeApp.client/` - Angular frontend
  - Contains all UI components, routing, and styles
- `ResumeApp.server/` - ASP.NET backend
  - Handles API requests, authentication, and database operations

## Getting Started
1. **Clone the repository:**
   ```sh
   git clone <your-repo-url>
   ```
2. **Install dependencies:**
   - Frontend: Navigate to `ResumeApp.client` and run `npm install`
   - Backend: Open `ResumeApp.server` in Visual Studio or use `dotnet restore`
3. **Run the application (two terminals):**
   - Backend API:
     ```sh
     cd ResumeApp.server
     dotnet watch run
     ```
   - Frontend (proxies `/api` calls to the backend):
     ```sh
     cd ResumeApp.client
     npm start -- --proxy-config proxy.conf.json
     ```

### Configuration & Database
- Update `ResumeApp.server/appsettings*.json` to tweak the `FrontendUrl`, database connection string, or JWT secrets.
- SQLite is the default store (`Data Source=resume.db`). The database file lives under `ResumeApp.server/` and is ignored by git.
- Entity Framework Core migrations live in `ResumeApp.server/Migrations`. They are applied automatically on startup, but you can run them manually with:
  ```sh
  cd ResumeApp.server
  dotnet ef database update
  ```

## Technologies Used
- Angular
- ASP.NET Core
- Entity Framework Core
- SQL Server (or SQLite)

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author
Barış KAÇİN
