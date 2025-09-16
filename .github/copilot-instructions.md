# SmartFlowUI Application

**ALWAYS follow these instructions first and only fallback to additional search and context gathering if the information provided here is incomplete or found to be in error.**

SmartFlowUI is a Blazor WebAssembly application with ASP.NET Core backend that provides an AI-powered chat interface. The application uses Azure services (OpenAI, Cognitive Search, Cosmos DB, Storage) and follows a clean architecture pattern with separate frontend, backend, and shared projects.

## Working Effectively

### Bootstrap and Build the Application
- Ensure .NET 8 SDK is installed: `dotnet --version` (should show 8.0.x)
- Navigate to the project root: `cd src/SmartFlowUI`
- Restore NuGet packages: `dotnet restore` -- takes 25-30 seconds. NEVER CANCEL.
- Build the solution: `dotnet build --configuration Release` -- takes 25-30 seconds. NEVER CANCEL. Set timeout to 180+ seconds.
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

## Infrastructure and Deployment

### GitHub Actions Workflows (`.github/workflows/`)
- **Main Workflows**:
  - `1-deploy-infra.yml` - Deploy Azure infrastructure using Bicep templates
  - `2-build-deploy-apps.yml` - Build and deploy Container Apps to Azure
  - `3-deploy-aif-project.yml` - Deploy Azure AI Foundry projects
- **Template Workflows** (reusable components):
  - `template-create-infra.yml` - Infrastructure deployment template
  - `template-aca-build.yml` - Container App build template
  - `template-aca-create.yml` - Container App creation template
  - `template-aca-deploy.yml` - Container App deployment template
  - `template-scan-code.yml` - Security scanning template

### Azure DevOps Pipelines (`.azdo/pipelines/`)
- **Main Pipelines**:
  - `1-deploy-infra-pipeline.yml` - Infrastructure deployment
  - `2-build-deploy-app-pipeline.yml` - Application build and deployment
  - `3-deploy-aif-project-pipeline.yml` - AI Foundry project deployment
  - `4-pr-pipeline.yml` - Pull request validation
  - `5-scan-pipeline.yml` - Security scanning
- **Additional Features**:
  - `pipes/` - Reusable pipeline components
  - `vars/` - Variable templates for different environments
  - `readme.md` - Pipeline documentation and setup instructions

### Infrastructure as Code (`infra/bicep/`)
- **Main Bicep Templates**:
  - `main-basic.bicep` - Basic infrastructure deployment (public endpoints, all services)
  - `main-advanced.bicep` - Advanced deployment with VNET integration
  - `main-project.bicep` - AI Foundry project-specific deployment
  - `main-test.bicep` - Testing infrastructure template
- **Parameter Files**:
  - `main-basic-gh.bicepparam` - GitHub Actions parameters
  - `main-basic-azdo.bicepparam` - Azure DevOps parameters
  - Similar parameter files for advanced and project templates
- **Supporting Files**:
  - `resourcenames.bicep` - Centralized resource naming logic
  - `modules/` - Modular Bicep components for individual services
  - `data/` - Reference data and configuration
  - `scripts/` - Deployment and utility scripts

### Azure Services Deployed
The Bicep templates deploy comprehensive Azure infrastructure including:
- **Container Apps Environment** with workload profiles
- **Azure Container Registry** for container images
- **Azure OpenAI/Foundry** with GPT-4o and GPT-4.1 models
- **Azure AI Search** for semantic search capabilities
- **Cosmos DB** for chat history and session storage
- **Storage Account** for file uploads and batch processing
- **Key Vault** for secrets management
- **Application Insights** for monitoring and telemetry
- **API Management** (optional) for API gateway functionality
- **Document Intelligence** (optional) for document processing

## Common Development Tasks

### Making Changes
- **Always build and run both applications** after making changes to validate functionality.
- **Code formatting**: The project uses EditorConfig for consistent formatting.
- **Shared library changes**: Require restart of both frontend and backend applications.

### Infrastructure Changes
- **Bicep validation**: Use `az deployment group what-if` to validate infrastructure changes before deployment
- **Parameter files**: Update appropriate `.bicepparam` files for GitHub Actions or Azure DevOps
- **Testing deployments**: Use `main-test.bicep` for testing infrastructure changes

### Pipeline Development
- **GitHub Actions**: Test workflows locally using `act` or validate syntax with GitHub CLI
- **Azure DevOps**: Use pipeline validation features and test in development environments first
- **Template reuse**: Leverage existing template workflows for consistent deployment patterns

### Troubleshooting
- **Port conflicts**: Applications use random ports. Check console output for actual URLs.
- **Configuration errors**: Check `backend/appsettings.local.json` for proper formatting.
- **Loading issues**: External CDN resources are blocked but don't affect core functionality.
- **Infrastructure issues**: Check Azure portal for resource deployment status and logs.
- **Pipeline failures**: Review GitHub Actions or Azure DevOps logs for detailed error messages.

### Additional Resources
- **Documentation**: Comprehensive docs in `Docs/` folder covering coding standards, infrastructure, and GitHub workflows.
- **API Testing**: Use `backend/tests.http` file for HTTP request testing.
- **Docker commands**: See `docker-readme.md` for container operations (limited in sandboxed environment).
- **Pipeline docs**: See `.azdo/pipelines/readme.md` for Azure DevOps pipeline setup.
- **GitHub setup**: See `.github/setup.md` for GitHub Actions configuration.

## Critical Reminders
- **NEVER CANCEL builds or long-running commands**. Always wait for completion.
- **ALWAYS validate changes** by running both applications and testing user scenarios.
- **Both frontend and backend must run simultaneously** for proper application functionality.
- **Set timeouts of 180+ seconds** for build operations to prevent premature cancellation.
- **Infrastructure changes**: Always use what-if deployments before making changes to production environments.
- **Pipeline testing**: Test infrastructure and pipeline changes in development environments first.