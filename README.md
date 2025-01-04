# GitHub Repositories Search Application

## Overview
This project implements a simple web application for searching GitHub repositories using the GitHub API. It features a server-side implementation using .NET Core with a client-side Angular application.

## Features
1. **Search Repositories**: Users can search for GitHub repositories by typing a keyword.
2. **Bookmarking**: Users can bookmark repositories for later access.
3. **Multi-Device Support**: The backend supports user-specific session management for bookmarks.
4. **Secure Communication**: Implements JWT-based authentication between the client and server.
5. **Authentication**: Includes an `AuthController` and `AuthService` using JWT with HttpOnly Cookies for production.
   - Angular implements an `AuthGuard`, `AuthInterceptor`, and `HttpInterceptor` to manage authentication and protect routes.
6. **Login and Registration**: Includes `LoginComponent` and `RegisterComponent` for user authentication.
7. **Gallery Component**: Integrated into the `SearchComponent` to display search results as a gallery.

---

## Technologies Used
- **Frontend**: Angular (v8+), Angular Material
- **Backend**: .NET Core (6 or 8), C#
- **UI Framework**: Bootstrap, Angular Material
- **HTTP Client**: `IHttpClientFactory`

---

## Project Structure

### Backend (.NET Core)
#### Key Components
1. **Controllers**:
   - `GitHubController`: Handles API requests to GitHub and communicates results.
   - `BookmarksController`: Manages user bookmarks with session-based persistence.
   - `AuthController`: Handles user authentication and JWT management.
2. **Services**:
   - Configured `IHttpClientFactory` for HTTP requests.
   - `AuthService`: Manages JWT creation and validation, storing tokens in HttpOnly cookies for secure production use.
3. **Database**:
   - Example schema for bookmarks using EF Core.
4. **Authentication**:
   - JWT-based authentication for secure API communication.

#### Setup Steps
1. Add the following NuGet packages:
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
   ```
2. Register `IHttpClientFactory` in `Program.cs`:
   ```csharp
   builder.Services.AddHttpClient();
   ```
3. Configure `AppDbContext` for database operations:
   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   ```

### Frontend (Angular)
#### Key Components
1. **Modules**:
   - `AppRoutingModule`: Manages application routes.
   - Angular Material modules for UI design.
2. **Components**:
   - `SearchComponent`: Allows users to search for repositories.
   - `GalleryComponent`: Displays search results within `SearchComponent`.
   - `BookmarksComponent`: Displays bookmarked repositories.
   - `LoginComponent`: Handles user login.
   - `RegisterComponent`: Handles user registration.
3. **Services**:
   - `GitHubService`: Handles communication with the GitHub API.
   - `BookmarkService`: Manages bookmarks via backend API.
4. **Authentication Features**:
   - `AuthGuard`: Protects routes requiring authentication.
   - `AuthInterceptor`: Attaches authentication tokens to HTTP requests.
   - `HttpInterceptor`: Handles HTTP requests and responses globally.

#### Setup Steps
1. Install Angular Material:
   ```bash
   ng add @angular/material
   ```
2. Add Angular Material and Bootstrap styles to `angular.json`:
   ```json
   "styles": [
     "src/styles.css",
     "node_modules/bootstrap/dist/css/bootstrap.min.css"
   ]
   ```
3. Define routes in `AppRoutingModule`:
   ```typescript
   const routes: Routes = [
     { path: 'search', component: SearchComponent },
     { path: 'bookmarks', component: BookmarksComponent },
     { path: 'login', component: LoginComponent },
     { path: 'register', component: RegisterComponent },
     { path: '', redirectTo: '/search', pathMatch: 'full' },
     { path: '**', redirectTo: '/search' },
   ];
   ```

---

## Configuration
### Backend (`appsettings.json`)
Add GitHub API configuration:
```json
{
  "GitHub": {
    "SearchRepositoriesUrl": "https://api.github.com/search/repositories?q=",
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BookmarksDb;Trusted_Connection=True;"
  }
}
```

### Frontend
Update `environment.ts` for backend URL:
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
};
```

---

## Running the Application

### Backend
1. Apply migrations and create the database:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
2. Run the backend:
   ```bash
   dotnet run
   ```

### Frontend
1. Install dependencies:
   ```bash
   npm install
   ```
2. Run the Angular app:
   ```bash
   ng serve
   ```

---

## Example API Usage
1. **Search Repositories**:
   - Endpoint: `GET /api/github/search?q=angular`
2. **Add Bookmark**:
   - Endpoint: `POST /api/bookmarks`
   - Body:
     ```json
     {
       "repositoryId": "12345",
       "repositoryName": "Angular",
       "ownerAvatarUrl": "https://example.com/avatar.png"
     }
     ```
3. **Get Bookmarks**:
   - Endpoint: `GET /api/bookmarks`
4. **Delete Bookmark**:
   - Endpoint: `DELETE /api/bookmarks/{id}`
5. **User Authentication**:
   - Login Endpoint: `POST /api/auth/login`
   - Logout Endpoint: `POST /api/auth/logout`

---

## Known Issues
- **CORS Errors**:
   Ensure CORS is enabled in the backend.
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddDefaultPolicy(policy =>
           policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
   });
   ```

---

## Future Enhancements
1. Add user authentication with registration and login.
2. Implement persistent database storage for bookmarks.
3. Improve the UI with additional GitHub data (e.g., stars, forks).

---

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

---

## License
This project is licensed under the MIT License. See the LICENSE file for details.
