# DevicePulse Project Overview

## Project Structure
This project is structured using Clean Architecture principles and Domain-Driven Design (DDD). The root aggregate in this solution is the Device, which encapsulates the core business logic and domain events.

## Key Components
- **Domain Layer**: Contains the core business logic, domain entities, and domain events. The `DeviceMovedDomainEvent` is an example of a domain event that signifies a change in the state of a device.
  
- **Application Layer**: Implements the use cases of the application. It includes integration events and commands, such as `ProcessTelemetryCommand`, which are handled using the MediatR library to implement the Command Query Responsibility Segregation (CQRS) pattern.

- **Infrastructure Layer**: Contains the implementation details for external services, such as Azure IoT Hub. The `DeviceNotificationService` and `TelemetryListenerService` are examples of services that interact with Azure IoT SDK to send notifications and listen for telemetry data.

- **API Layer**: Exposes the application functionality through HTTP endpoints. The `DeviceController` handles requests related to devices, such as retrieving events by device ID.

## Technologies Used
- **Azure IoT SDK**: For communication with IoT devices.
- **MediatR**: For implementing the CQRS pattern.
- **Serilog**: For logging throughout the application.
- **Entity Framework Core**: (if applicable) for data access (not explicitly mentioned in the context but commonly used in such architectures).

## Features
- **Event-Driven Architecture**: The application uses domain events to trigger actions in response to changes in the state of devices.
- **CQRS**: Commands and queries are separated, allowing for more scalable and maintainable code.
- **Dependency Injection**: Services are registered in the `AddInfrastructureServices` method, promoting loose coupling and easier testing.

## Getting Started
## Additional Information

### Application Layer
The `DevicePulse.Application` project is responsible for implementing the use cases of the application. It utilizes the MediatR library to facilitate the Command Query Responsibility Segregation (CQRS) pattern. Key components include:

- **Commands**: Such as `ProcessTelemetryCommand`, which encapsulates the data needed to process telemetry from devices.
- **Queries**: For retrieving device information and telemetry data.
- **Handlers**: Each command and query has a corresponding handler that contains the business logic for processing the request.

### Domain Layer
The `DevicePulse.Domain` project contains the core business logic and domain entities. It defines the `Device` aggregate, which includes:

- **Entities**: Such as `Device`, which represents the IoT device and its properties.
- **Value Objects**: For encapsulating attributes that are part of the domain but do not have a unique identity.
- **Domain Events**: Like `DeviceMovedDomainEvent`, which triggers actions in response to state changes.

### Infrastructure Layer
The `DevicePulse.Infrastructure` project provides the implementation details for external services. It includes:

- **DeviceNotificationService**: A service that sends notifications to devices using Azure IoT Hub.
- **TelemetryListenerService**: A service that listens for telemetry data from devices and processes it accordingly.
- **Configuration**: Uses `Microsoft.Extensions.Configuration` to manage application settings and connection strings.

### API Layer
The `DevicePulse.API` project exposes the application functionality through HTTP endpoints. It includes:

- **Controllers**: Such as `DeviceController`, which handles requests related to devices, including retrieving events by device ID.
- **Swagger Integration**: Utilizes Swashbuckle to provide API documentation and testing capabilities.

### Conclusion
The DevicePulse project is designed to be scalable and maintainable, leveraging modern architectural patterns and practices. It effectively manages IoT devices and their data, providing a robust backend solution.
1. Clone the repository.
2. Configure the Azure IoT Hub connection string in the `appsettings.json`.
3. Run the application and use the API endpoints to interact with the device data.

## Conclusion
The DevicePulse project is a robust backend solution designed for managing IoT devices using modern architectural patterns and practices. It leverages Azure services to provide a scalable and maintainable system.
