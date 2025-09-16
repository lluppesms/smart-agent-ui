# SmartFlowUI Application

**ALWAYS follow these instructions first and only fallback to additional search and context gathering if the information provided here is incomplete or found to be in error.**

SmartFlowUI is a Blazor WebAssembly application with ASP.NET Core backend that provides an AI-powered chat interface. The application uses Azure services (OpenAI, Cognitive Search, Cosmos DB, Storage) and follows a clean architecture pattern with separate frontend, backend, and shared projects.

## Working Effectively

### Bootstrap and Build the Application
- Ensure .NET 8 SDK is installed: `dotnet --version` (should show 8.0.x)
- Navigate to the project root: `cd src/SmartFlowUI`
- Restore NuGet packages: `dotnet restore` -- takes 25-30 seconds. NEVER CANCEL.
- Build the solution: `dotnet build --configuration Release` -- takes 25-30 seconds. NEVER CANCEL. Set timeout to 120+ seconds.
- **Build Status**: The build succeeds with warnings but no errors. Warnings are expected and related to nullable reference types.

### Run the Application Locally
- **CRITICAL**: Both frontend and backend must run simultaneously for full functionality.
- **Backend**: 
  - `cd src/SmartFlowUI/backend`
  - `dotnet run` -- starts in 10-15 seconds. Runs on random ports (typically https://localhost:605xx, http://localhost:605xx).
  - Backend provides Swagger UI at `/swagger/index.html` for API documentation.
- **Frontend**:
  - `cd src/SmartFlowUI/frontend` 
  - `dotnet run` -- starts in 15-20 seconds. Runs on http://localhost:4206.
- **NEVER CANCEL** either application during startup. Both must remain running for testing.

### Configuration Setup
- Create local configuration: `cp backend/appsettings.Template-Simple.json backend/appsettings.local.json`
- The application can run with minimal configuration for local development.
- **Expected Behavior**: Application loads with profile loading errors (normal without Azure services configured).

## Validation and Testing

### Manual Validation Scenarios
- **ALWAYS run complete validation scenarios after making changes.**
- **Primary Workflow**: 
  1. Navigate to http://localhost:4206
  2. Verify the chat interface loads with "How can I help you today?" message
  3. Test navigation to History page at http://localhost:4206/v2/history
  4. Verify the history table displays with proper columns (Description, Profile, Timestamp)
- **Expected UI**: Blue-themed interface with sidebar navigation, chat input field, and proper MudBlazor styling.
- **Expected Errors**: "Error loading profiles...! Call to api/user failed!" is normal without Azure configuration.

### Health Check Endpoints
- Backend Swagger UI: http://localhost:605xx/swagger/index.html
- Test API endpoints like `/api/agents` (may return empty responses without Azure config)

### Known Limitations
- **Docker Build**: `docker build` fails due to network restrictions accessing NuGet.org. Document as "Docker build not available in sandboxed environment."
- **External Resources**: CDN resources (fonts, jQuery, highlight.js) are blocked but don't affect core functionality.
- **Azure Services**: Application expects Azure OpenAI, Cognitive Search, and Cosmos DB but gracefully handles missing configuration.

## Build and Test Timing

### Critical Timing Information
- **dotnet restore**: 25-30 seconds. NEVER CANCEL. Set timeout to 180+ seconds.
- **dotnet build**: 25-30 seconds. NEVER CANCEL. Set timeout to 180+ seconds.
- **Application startup**: Backend 10-15 seconds, Frontend 15-20 seconds. NEVER CANCEL.
- **No unit tests**: The solution contains no test projects. `dotnet test` returns immediately.

### Performance Expectations
- **Blazor WASM loading**: Initial page load takes 10-15 seconds due to WebAssembly bootstrap.
- **Navigation**: Client-side routing is immediate after initial load.

## Project Structure

### Solution Organization
- **backend/**: ASP.NET Core Web API (MinimalApi.csproj) - provides REST APIs and serves the frontend
- **frontend/**: Blazor WebAssembly application (ClientApp.csproj) - the main user interface
- **shared/**: Shared models and services (Shared.csproj) - common code between frontend and backend

### Key Directories and Files
- **Backend Services**: `backend/Services/` - contains chat, profile, and document services
- **Frontend Components**: `frontend/Components/` - reusable Blazor components
- **Configuration**: `backend/appsettings.*.json` - application configuration templates
- **Docker**: `Dockerfile` - containerization support (note: fails in sandboxed environment)

### Important Configuration Files
- `Directory.Build.props` and `Directory.Packages.props` - centralized package management
- `.editorconfig` - code formatting rules
- `nuget.config` - NuGet package source configuration

## Common Development Tasks

### Making Changes
- **Always build and run both applications** after making changes to validate functionality.
- **Code formatting**: The project uses EditorConfig for consistent formatting.
- **Shared library changes**: Require restart of both frontend and backend applications.

### Troubleshooting
- **Port conflicts**: Applications use random ports. Check console output for actual URLs.
- **Configuration errors**: Check `backend/appsettings.local.json` for proper formatting.
- **Loading issues**: External CDN resources are blocked but don't affect core functionality.

### Additional Resources
- **Documentation**: Comprehensive docs in `Docs/` folder covering coding standards, infrastructure, and GitHub workflows.
- **API Testing**: Use `backend/tests.http` file for HTTP request testing.
- **Docker commands**: See `docker-readme.md` for container operations (limited in sandboxed environment).

## Critical Reminders
- **NEVER CANCEL builds or long-running commands**. Always wait for completion.
- **ALWAYS validate changes** by running both applications and testing user scenarios.
- **Both frontend and backend must run simultaneously** for proper application functionality.
- **Set timeouts of 180+ seconds** for build operations to prevent premature cancellation.