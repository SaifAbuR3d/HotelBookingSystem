# HotelBookingAPI 🏨🔑🌐

This API offers a suite of endpoints for managing hotel-related activities. It allows for handling bookings, managing hotel and city information, and providing guest services.

Developed during my internship at Foothill Technology Solutions, This project represents not only a technical endeavor but also a personal journey of professional growth and learning. By integrating modern software development practices and technologies, the HotelBookingAPI serves as proof of the practical application of theoretical knowledge in a real-world setting.

The live version of the API is deployed at [https://saif-hotels.azurewebsites.net/](https://saif-hotels.azurewebsites.net/). Feel free to explore the endpoints and functionalities in a real-world environment.


## Documentation
For a complete overview of all available endpoints, enriched with usage examples and expected error responses. refer to:
### Swagger Documentation

- **Hotel Booking System API v1**
  - [OAS3 Documentation](https://app.swaggerhub.com/apis-docs/SAEFRAED2/hotel-booking_system_api/v1)

### Bump.sh Documentation

- **Hotel Booking System API v1**
  - [Documentation](https://bump.sh/saifabur3d/doc/hotel-booking-system)


## Setup Guide

Setting up the Hotel Booking System API is straightforward. You can choose between a Docker-based setup or a manual setup.

### Docker-based Setup
1. **Download the `docker-compose.yml` file**: This file(included in the repo) contains the configuration to set up both the API and the SQL Server database.
2. **Run the Application**: Execute `docker-compose up` in the directory containing the `docker-compose.yml` file. Docker will pull the necessary images and set up the containers. The API will be accessible on `localhost:8080`.

### Manual Setup
1. **Clone the Repository**: Clone the GitHub repository to your local machine.
2. **Run the Application**: Navigate to the project directory and run `dotnet run`. The application will automatically apply any required database migrations. Make sure your SQL Server instance is running and accessible as per your application's configuration.

Choose the method that best suits your environment and preferences.

### Continuous Integration, Delivery, and Deployment

The Hotel Booking System API uses a streamlined CI/CD pipeline, leveraging GitHub Actions and Azure Web App for efficient and reliable software delivery.

#### CI/CD Pipeline Overview

- **GitHub Actions**: Automatically manages the build and test processes. The pipeline is triggered on every push to the `master` branch and via manual workflow dispatch.
- **Build Process**: The application is compiled using .NET 8, ensuring the code integrates correctly.
- **Automated Testing**: After building, the pipeline runs automated tests to verify the application's functionality and maintain code quality.
- **Artifact Packaging**: Successfully built and tested code is packaged into an artifact, ready for deployment.

#### Deployment Process

- **Azure Web App Deployment**: The application is deployed to Azure Web App, a hosting service for web applications. The live version of the API, reflecting the most recent changes from the `master` branch, is automatically updated and can be accessed at [https://saif-hotels.azurewebsites.net/](https://saif-hotels.azurewebsites.net/).
- **Publish Profile**: Uses Azure Publish Profile for secure deployments.
- **Automated Deployment**: The latest build from the `master` branch is automatically deployed to the production environment, ensuring rapid delivery of updates.

This CI/CD approach simplifies the development process, automates testing and deployment, and ensures that new changes are efficiently and reliably delivered.


## ⭐ Key Features

### Booking Management:
- **Create Booking:** Allows users to create a new booking.
- **Get Booking:** Retrieve details of a specific booking by its ID.
- **Delete Booking:** Enables deletion of an existing booking.
- **Get Invoice:** Retrieve a booking invoice.
- **Download Invoice as PDF:** Download the invoice of a booking in PDF format.

### City Management:
- **Create City:** Add a new city to the system.
- **Get City:** Retrieve details of a city by its ID.
- **Update City:** Modify details of an existing city.
- **Delete City:** Remove a city from the system.
- **List Cities:** Retrieve a list of cities, with optional filtering and pagination.
- **Get Trending Destinations:** Retrieve a list of top visited cities.
- **Upload City Images:** Add images to a city's profile.

### Hotel Management:
- **Create Hotel:** Add a new hotel to the system.
- **Get Hotel:** Retrieve details of a hotel by its ID.
- **Update Hotel:** Modify details of an existing hotel.
- **Delete Hotel:** Remove a hotel from the system.
- **List Hotels:** Retrieve a list of hotels, with optional filtering and pagination.
- **Upload Hotel Images:** Add images to a hotel's profile.

### Room Management:
- **Create Room:** Add a new room to a hotel.
- **Get Room:** Retrieve details of a room by its ID.
- **Update Room:** Modify details of an existing room.
- **Delete Room:** Remove a room from a hotel.
- **Upload Room Images:** Add images to a room's profile.
- **Create Room Discount:** Offer discounts on rooms.
- **Get Room Discount:** Retrieve details of a room's discount by its ID.
- **Delete Room Discount:** Remove a discount from a room.
- **List Featured Deals:** Retrieve a list of featured room deals.

### Guest Management:
- **Get Recently Visited Hotels:** Retrieve a list of recently visited hotels for a guest.

### Review Management:
- **Add Review:** Submit a review for a hotel.
- **Get Review:** Retrieve a specific review by its ID.
- **Update Review:** Modify an existing review.
- **Delete Review:** Remove a review.
- **Get Hotel Reviews:** Retrieve reviews for a specific hotel.
- **Get Average Hotel Rating:** Calculate the average rating of a hotel.

### Identity Management:
- **Guest Registration:** Register a new guest user.
- **Admin Registration:** Register a new admin user.
- **User Login:** Authenticate a user.


## Endpoints

### Identity(Authentication) Endpoints

| HTTP Method | Endpoint              | Description           |
|-------------|-----------------------|-----------------------|
| POST        | `/api/register`       | Register a guest      |
| POST        | `/api/register-admin` | Register an admin     |
| POST        | `/api/login`          | Login a user          |

### Cities Endpoints

| HTTP Method | Endpoint                            | Description                              |
|-------------|-------------------------------------|------------------------------------------|
| GET         | `/api/Cities/{id}`                  | Get a city by its ID                     |
| DELETE      | `/api/Cities/{id}`                  | Delete a city                            |
| PUT         | `/api/Cities/{id}`                  | Update a city                            |
| POST        | `/api/Cities`                       | Create a new city                        |
| GET         | `/api/Cities`                       | Retrieve list of cities with parameters  |
| GET         | `/api/Cities/trending-destinations` | Retrieve top visited cities              |
| POST        | `/api/Cities/{id}/images`           | Upload an image to a city                |

### Hotels Endpoints

| HTTP Method | Endpoint                       | Description                           |
|-------------|--------------------------------|---------------------------------------|
| GET         | `/api/Hotels/{id}`             | Get a hotel by its ID                 |
| DELETE      | `/api/Hotels/{id}`             | Delete a hotel                        |
| PUT         | `/api/Hotels/{id}`             | Update a hotel                        |
| POST        | `/api/Hotels`                  | Create a new hotel                    |
| GET         | `/api/Hotels`                  | Retrieve list of hotels with params   |
| POST        | `/api/Hotels/{id}/images`      | Upload an image to a hotel            |
| GET         | `/api/Hotels/search`           | Search and filter hotels              |

### Rooms Endpoints

| HTTP Method | Endpoint                  | Description                                  |
|-------------|---------------------------|----------------------------------------------|
| GET         | `/api/Rooms/{id}`         | Get a room by its ID                         |
| DELETE      | `/api/Rooms/{id}`         | Delete a room                                |
| PUT         | `/api/Rooms/{id}`         | Update a room                                |
| POST        | `/api/Rooms`              | Create a new room                            |
| GET         | `/api/Rooms`              | Retrieve a list of rooms with parameters     |
| POST        | `/api/Rooms/{id}/images`  | Upload an image to a room                    |

### Bookings Endpoints

| HTTP Method | Endpoint                          | Description                           |
|-------------|-----------------------------------|---------------------------------------|
| GET         | `/api/Bookings/{id}`              | Get a booking by its ID               |
| DELETE      | `/api/Bookings/{id}`              | Delete a booking                      |
| POST        | `/api/Bookings`                   | Create a new booking                  |
| GET         | `/api/Bookings/{id}/invoice`      | Retrieve an invoice for a booking     |
| GET         | `/api/Bookings/{id}/pdf`          | Download booking invoice as PDF       |

### Reviews Endpoints

| HTTP Method | Endpoint                                | Description                                           |
|-------------|-----------------------------------------|-------------------------------------------------------|
| POST        | `/api/hotels/{hotelId}/reviews`         | Adds a review for a specific hotel                    |
| GET         | `/api/hotels/{hotelId}/reviews`         | Retrieves reviews for a specific hotel                |
| GET         | `/api/hotels/{hotelId}/reviews/{reviewId}` | Get a specific review by its ID                      |
| PUT         | `/api/hotels/{hotelId}/reviews/{reviewId}` | Update a specific review                             |
| DELETE      | `/api/hotels/{hotelId}/reviews/{reviewId}` | Delete a specific review                             |
| GET         | `/api/hotels/{hotelId}/reviews/average` | Get the average rating of a hotel                     |

### Discounts and Deals Endpoints

| HTTP Method | Endpoint                               | Description                             |
|-------------|----------------------------------------|-----------------------------------------|
| POST        | `/api/rooms/{roomId}/discounts`        | Create a new discount for a room        |
| GET         | `/api/rooms/{roomId}/discounts/{id}`   | Get a discount by its ID                |
| DELETE      | `/api/rooms/{roomId}/discounts/{id}`   | Delete a discount                       |
| GET         | `/api/rooms/featured-deals`            | Retrieves a collection of featured deals|

### Guest Endpoints

| HTTP Method | Endpoint                                      | Description                                           |
|-------------|-----------------------------------------------|-------------------------------------------------------|
| GET         | `/api/Guests/{guestId}/recently-visited-hotels`| Retrieves recently visited hotels for a specific guest |
| GET         | `/api/Guests/recently-visited-hotels`         | Retrieves recently visited hotels for the current guest|


## Tools and Concepts

This section provides an overview of the key tools, technologies, and concepts used in the development and operation of the Hotel Booking System API.

### Programming Languages and Frameworks
- **C#**: Primary programming language used.
- **.NET Core**: Framework for building high-performance, cross-platform web APIs.

### Database
- **Entity Framework Core**: Object-relational mapping (ORM) framework for .NET.
- **SQL Server**: Database management system used for storing all application data.

The code-first approach enhances the project's flexibility, making it easier to evolve the database schema alongside the application development and maintain version control over database changes.

### API Documentation and Design
- **Swagger/OpenAPI**: Used for API specification and documentation.
- **Swagger UI**: Provides a web-based UI for interacting with the API's endpoints.

### Authentication and Authorization
- **JWT (JSON Web Tokens)**: Method for securely transmitting information between parties as a JSON object.
- **Microsoft.Identity**: For identity management, integrate Microsoft.Identity, a part of the Microsoft identity platform. This framework provides a comprehensive set of functionalities for handling user authentication and authorization in .NET applications. It simplifies managing user identities, securing passwords, and implementing token-based authentication.


### Testing
- **xUnit**: Unit testing tool for the C# programming language.
- **Postman**: Tool for API testing and exploring. Here is a set of API tests [Access the Postman Collection](https://lively-astronaut-76528.postman.co/workspace/HotelBooking~12d17fd1-1aa8-4088-bc94-77943732170f/collection/31090301-da27e327-a3fd-472e-8683-22eeb7581ecd?action=share&creator=31090301&active-environment=31090301-fd88509e-4be4-4e59-bf1b-3fb013efca43)

### Monitoring and Logging
- **Serilog**: Logging library for .NET applications.
- **Application Insights**: Extensive logging, monitoring, and alerting system.

### Design Patterns and Architectural Concepts
- **RESTful Principles**: A design approach for APIs that use HTTP requests to access and manipulate data.
- **Repository Pattern**: Used for abstracting the data layer, making our application more maintainable, testable, and clean.
- **Options Pattern**: Used for accessing and managing configurations within the application.
- **Clean Architecture**: Organizing the project into four layers:
  - **External Layers**: 
    - Web: Contains Controllers.
    - Infrastructure: Contains Persistence, Email, PDF, and Identity Infrastructure.
  - **Core Layers**:
    - Application: Business logic and application rules. It also defines Abstractions for working with Infrastructure.
    - Domain: Fundamental business rules and entities.

### Security
- **HTTPS**: Ensuring secure communication over the network.
- **Data Encryption**: Encrypting sensitive data in the database.

By leveraging these tools and concepts, the Hotel Booking System API provides a robust, scalable, and secure platform for hotel booking management.


## API Versioning
Using Asp.Versioning.Mvc
The Hotel Booking System API employs a query parameter-based versioning approach. This strategy allows clients to seamlessly access different API versions without any changes to the base URL or path. Importantly, the version parameter is optional, offering flexibility and ease of use.

### Versioning Strategy
- **Query Parameter Versioning**: Versions are specified using the `api-version` query parameter in the request URL. This method is straightforward and simplifies the management of different API versions.

### Usage
Clients can specify the API version by appending the `api-version` query parameter to the URL. If no version is specified, the API defaults to the latest version. For example:
- Specifying a version: `saif-hotels.azurewebsites.net/api/cities?api-version=1.0`
- Without specifying a version: `saif-hotels.azurewebsites.net/api/cities`

This versioning approach ensures that the Hotel Booking System API is user-friendly, adaptable, and scalable, catering to diverse client needs as it evolves.


## Get Involved

Your feedback and contributions are welcome! I value any insights, suggestions, or contributions you might have.

### Ways to Contribute:
- **Feedback**: Your opinions and ideas are welcome. Feel free to share your thoughts.
- **Issue Reporting**: If you find any bugs or issues, please report them on [GitHub Issues page](https://github.com/SaifAbuR3d/HotelBookingSystem/issues).
- **Code Contributions**: Contributions to the codebase are always appreciated.

### Contact and Support:
For any questions, suggestions, or discussions, please feel free to reach out at [saefraed2@gmail.com](saefraed2@gmail.com)

For additional contact methods and to view my other projects, visit my GitHub profile at [SaifAbuR3d](https://github.com/SaifAbuR3d).

Thank you for your interest. I look forward to hearing from you!


