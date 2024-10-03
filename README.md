# Talabat Clone

Talabat Clone is an advanced, full-stack web application built to simulate the core functionalities of a food ordering platform. The project features an Admin Dashboard and RESTful APIs using ASP.NET Core, integrating modern technologies like Redis for caching, Stripe for payment handling, and advanced architectural patterns such as Onion Architecture, Repository Pattern, and Unit of Work.

## Features

### Admin Dashboard (ASP.NET Core MVC)
- **Product Management**: Admins can create, update, delete, and manage food items.
- **Order Management**: View, track, and update orders.
- **Basket Management**: Admins can manage items in the users' baskets.
- **Authentication & Authorization**: Secure login for admin access using JWT tokens.

### ASP.NET Core APIs
- **User Authentication**: JWT-based authentication with refresh tokens.
- **CRUD Operations**: Full-fledged Create, Read, Update, and Delete functionalities for products, orders, and categories.
- **Basket Management**: Users can add, update, or remove items from their basket.
- **Order Placement**: Users can place orders after selecting items and making payments.
- **Caching with Redis**: Optimized API responses using Redis for caching frequently accessed data.
-  **Authentication & Authorization**: Secure login for admin access using JWT tokens.
- **Stripe Payment Gateway Integration**: Handling payment transactions seamlessly.
- **Error Handling & Logging**: Centralized error management and logging for debugging purposes.
  
## Tech Stack

### Backend
- **ASP.NET Core MVC**: For building the Admin Dashboard and APIs.
- **ASP.NET Core Web API**: For building RESTful APIs consumed by the frontend.
- **Entity Framework Core**: ORM for interacting with the database using the Code First approach.
- **Redis**: For caching frequently accessed data.
- **Stripe**: For handling payments.
- **Swagger**: API documentation and testing tool.
- **SQL Server**: Database management system.

### Architecture & Design Patterns
- **Onion Architecture**: For maintaining a clean, decoupled code structure.
- **Repository Pattern**: For abstracting database interaction.
- **Unit of Work**: To manage multiple repositories and ensure transactional integrity.
- **Specifications**: For handling complex query logic.
  
## Getting Started

### Prerequisites
- .NET 8
- SQL Server
- Redis
- Stripe Account (for payment processing)

### Installation

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/iHebaMohammed/Talabat-Clone
    ```
   
2. **Navigate to the Project Directory**:
    ```bash
    cd Talabat-Clone
    ```

3. **Set Up the Database**:
    - Update your `appsettings.json` file with the correct SQL Server connection string.
    - Run database migrations to create the database:
      ```bash
      dotnet ef database update
      ```

4. **Configure Redis**:
    - Ensure Redis is installed and running on your machine.
    - Update `appsettings.json` with the Redis connection string.

5. **Configure Stripe**:
    - Set your Stripe API keys in the environment variables or `appsettings.json`.

6. **Run the Application**:
    ```bash
    dotnet run
    ```

### Running the Project
Once the project is set up, you can run the **Admin Dashboard** by navigating to:
    ```bash
http://localhost:5160
    ```
The **APIs** are available at:
    ```bash
http://localhost:5290
    ```

## Usage

- **Admin Dashboard**: Manage products, orders, and users , roles.
- **User APIs**: Allow users to browse items, manage their basket, and place orders.
- **Caching**: Frequently accessed data (e.g., product listings) is cached using Redis for optimized performance.
- **Payment Integration**: Orders can be paid via Stripe, ensuring secure transaction handling.

## Demo Video


## Future Enhancements
- **User Management**: Add user roles and permissions for finer access control.
- **Notification System**: Implement a real-time notification system for order updates.
- **Third-Party Integration**: Integrate delivery tracking services for real-time order tracking.

## Contributing
Contributions are welcome! Please follow the standard GitHub workflow:

1. Fork the project.
2. Create a new branch (`feature/your-feature`).
3. Commit your changes.
4. Open a pull request.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

