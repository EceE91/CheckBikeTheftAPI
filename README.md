# CheckBikeTheftAPI

The project structure is described below:
CheckBikeTheftAPI
  - CheckBikeTheftAPI.Api (Contains controllers, ViewModels, StartUp)
  - CheckBikeTheftAPI.Core (Contains Models and Interfaces and reposity)
  - CheckBikeTheftAPI.Data (Contains repository and service implementations)
CheckBikeTheftAPI.Tests
  - ControllerTests (XUnit tests with FluentAssertions and Moq library)
  - IntegrationTests (Xunit tests with TestServer)
  
  Technology stack:
  - net6.0
  - Xunit with FluentAssertions and Moq for testing
  - TestServer for integration testing
  - Repository pattern
  - SwaggerUI to display and test endpoints
  - API versioning
  - Docker support

What's missing?
- Acceptance testing with Specflow
- Cancellationtoken
- Monitoring
