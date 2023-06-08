# dotnetcore-signalr-example

This repository contains three projects that demonstrate the usage of SignalR in a .NET Core application, along with client examples in JavaScript and Flutter.

## Projects

### OrderService

The `OrderService` project is a .NET Core application that showcases the implementation of a SignalR hub and a REST API. It provides real-time functionality to clients and exposes RESTful endpoints for managing orders. 

#### Prerequisites

- .NET Core SDK [link](https://dotnet.microsoft.com/download)
- Visual Studio or Visual Studio Code (optional)

#### Getting Started

1. Clone the repository: 
   ```
   git clone https://github.com/your-username/dotnetcore-signalr-example.git
   ```

2. Navigate to the `OrderService` directory:
   ```
   cd OrderService
   ```

3. Restore dependencies and build the project:
   ```
   dotnet restore
   dotnet build
   ```

4. Run the application:
   ```
   dotnet run
   ```

5. The OrderService will be accessible at `http://localhost:5000`. The SignalR hub will be available at `/hubs/orders`.

### JavascriptClient

The `JavascriptClient` project is an example client application written in JavaScript. It demonstrates how to connect to the `OrderService` SignalR hub and receive real-time updates.

#### Prerequisites

- Any modern web browser

#### Getting Started

1. Clone the repository: 
   ```
   git clone https://github.com/your-username/dotnetcore-signalr-example.git
   ```

2. Navigate to the `JavascriptClient` directory:
   ```
   cd JavascriptClient
   ```
3. Install required dependencies:
   ```
   npm install
   ```
3. Run the application:
   ```
   npm start
   ```
4. The client application will connect to the `OrderService` SignalR hub and display real-time updates.

### FlutterClient

The `FlutterClient` project is an example client application written in Flutter. It demonstrates how to connect to the `OrderService` SignalR hub and receive real-time updates.

#### Prerequisites

- Flutter SDK [link](https://flutter.dev/docs/get-started/install)

#### Getting Started

1. Clone the repository: 
   ```
   git clone https://github.com/your-username/dotnetcore-signalr-example.git
   ```

2. Navigate to the `FlutterClient` directory:
   ```
   cd FlutterClient
   ```

3. Retrieve the project dependencies:
   ```
   flutter pub get
   ```

4. Run the application:
   ```
   flutter run
   ```

5. The client application will connect to the `OrderService` SignalR hub and display real-time updates.

## Contributing

If you'd like to contribute to this project, please follow these guidelines:

1. Fork the repository on GitHub.
2. Create a new branch with a descriptive name for your feature/bug fix.
3. Make your changes and ensure that the code is properly formatted.
4. Write tests if applicable.
5. Commit your changes and push the branch to your forked repository.
6. Submit a pull request to the main repository.

## License

This project is licensed under the [MIT License](LICENSE).
