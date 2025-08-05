
## Auction System - Project Documentation

### Overview:
Welcome to the Auction System, the ultimate platform where buyers, sellers, and administrators collaborate seamlessly to bring real-time, secure, and efficient online auctions to life. Whether you're a Bidder, a Seller, or an Admin, our system is tailored to provide a smooth and rewarding experience for each user role.

### Key Features:
1. **User Roles**:
   - **Bidder**: Can place bids on auctions, view auction details, and pay for won auctions.
   - **Seller**: Can create and manage auctions, track their own auctions, and view feedback.
   - **Admin**: Can manage and oversee all auctions, ban users, view auction statistics, and handle all activities.

2. **Auction Management**:
   - **Auction Creation**: Sellers can create auctions with start and end times, set starting prices, and add images.
   - **Auction Updates**: Sellers and Admin can update or delete auctions.
   - **Auction Cancellation**: Admin can cancel auctions if there are any issues.
   - **Auction Winner Declaration**: After the auction ends, the highest bid wins, and the winner is automatically declared.

3. **Bidder Features**:
   - **Bidding**: Bidder can place bids on active auctions.
   - **Auction Status**: Bidder can view the current status of the auction, the highest bid, and remaining time.
   - **Payment**: A Bidder must pay a deposit before bidding to ensure seriousness.
   - **Profile**: Bidder can manage their profile, view active bids, and see the auctions they have won.

4. **Seller Features**:
   - **Seller Dashboard**: Seller can manage their auctions, view bids, and track auction status.
   - **Seller Feedback**: Bidders can leave feedback for Sellers based on auction experience.

5. **Admin Features**:
   - **Admin Dashboard**: Admin can manage all auctions, view statistics, and perform user management tasks like banning users.
   - **User Management**: Admin can ban or unban users, handle disputes, and review user behavior.

6. **Real-Time Features**:
   - **SignalR Integration**: Used for real-time updates, notifications for bid placements, auction status changes, and feedback submissions.
   - **Webhooks**: Real-time notifications on payments using Stripe Webhooks.

7. **Payment System**:
   - **Stripe Integration**: Used for handling deposits and payments. Bidder must pay a deposit to place bids. Stripe Webhook ensures payment confirmation and updates the status of the auctions.

8. **OTP for 2FA**:
   - **OTP Verification**: Used for two-factor authentication (2FA) to enhance security during login, registration, and password reset.

9. **Background Services**:
   - **Winner Declaration**: A background service is responsible for declaring the winner of auctions once the auction time expires.

### Technologies Used:
- **.NET 8**: The project is built using the latest version of .NET.
- **SignalR**: Used for real-time communication and notifications.
- **Redis**: Used for handling the wishlist functionality of bidders.
- **Stripe**: Used for handling payments and deposits via Stripe API.
- **JWT Authentication**: Used for user authentication and authorization with access tokens and refresh tokens.
- **Unit of Work and Repository Patterns**: These patterns ensure that all database operations are managed efficiently and consistently.
- AutoMapper: Used for object mapping between models and DTOs. Mappings are defined in MappingProfile.cs.
- Specification Design Pattern: Used to manage filtering, sorting, and pagination for auctions, ensuring scalability and efficiency.



### Validation and Exception Handling:
- **Custom Exception Handling**: The project uses custom exception handling middleware to handle errors throughout the application and return user-friendly error messages.
- **Data Validation**: All input data is validated, including user inputs and auction parameters, to prevent invalid data from affecting the system.
- **BadRequestException**: When incorrect or invalid data is provided, the system throws a **BadRequestException** to ensure that only valid data is processed.

### Database and Data Access:
- **Entity Framework Core**: The system uses EF Core to manage database interactions and perform CRUD operations.
- SQL: For complex queries or performance optimization, raw SQL queries are executed within EF Core using DbSet.FromSqlRaw() and DbContext.Database.ExecuteSqlRaw().
- LINQ: The system heavily relies on Language Integrated Query (LINQ) to query and filter data, making it easier to manipulate data in a more readable and maintainable way.
- **Unit of Work Pattern**: Ensures that database operations are consistent and rolled back if there is an error.
- **Repositories**: Used to abstract database access logic, making it easier to manage and test.

### Project Architecture:
- **Clean Architecture**: The system follows a clean architecture approach, dividing the project into the following layers:
  - **Presentation Layer**: Contains the API controllers that handle incoming HTTP requests.
  - **Application Layer**: Contains the business logic, DTOs, and services.
  - **Domain Layer**: Contains the core entities, exceptions, and interfaces.
  - **Infrastructure Layer**: Contains external services, repositories, and data access code (e.g., Stripe, Redis).
  - **Shared Layer**: Contains cross-cutting functionalities like error handling, utilities, SignalR hubs, middleware, and shared configurations.

### Features Summary:
- **Real-time bidding system** with automatic winner declaration.
- **Bidder management** with secure payments, profile management, and bidding history.
- **Seller management** with auction creation, tracking, and feedback collection.
- **Admin panel** for overseeing auctions, user management, and statistics tracking.
- **Two-factor authentication (2FA)** via OTP for enhanced security.
- **Payment handling** via Stripe with support for deposits and payment verification using webhooks.
- **Background service** for automatic auction winner declaration.

### Setup:
1. **Clone the repository**:
   ```
   git clone https://github.com/ahmedzakaria2003/auction-system.git
   ```
2. **Install dependencies**:
   Make sure you have .NET 8 SDK installed, then run:
   ```
   dotnet restore
   ```
3. **Set up the environment**:
   Create a `.env` file or configure the connection strings in `appsettings.json` for your database, Redis, and Stripe API keys.
4. **Run the application**:
   ```
   dotnet run
   ```
5. **Run tests**:
   For unit tests and integration tests:
   ```
   dotnet test
   ```

### API Endpoints:
| Endpoint                                      | Description                                            |
|-----------------------------------------------|--------------------------------------------------------|
| **/api/Admin/auctions-summary**               | Get summary statistics of auctions                     |
| **/api/Admin/seller-auctions-management**     | Manage auctions of sellers                             |
| **/api/Admin/all-users**                      | Get all users in the system                            |
| **/api/Admin/ban-user/{userId}**              | Ban a user                                             |
| **/api/Admin/unban-user/{userId}**            | Unban a user                                           |
| **/api/Auction/all-auctions**                 | Get all auctions                                       |
| **/api/Auction**                              | Create a new auction                                   |
| **/api/Auction/{auctionId}**                  | Get auction details by ID                              |
| **/api/Auction/active-auctions**              | Get all active auctions                                |
| **/api/Auction/details/{auctionId}**          | Get details of a specific auction                      |
| **/api/Auction/my-auctions**                  | Get auctions created by the logged-in user             |
| **/api/Auction/canceled/{auctionId}**         | Cancel an auction                                      |
| **/api/Auction/declare-winner/{auctionId}**   | Declare the winner of an auction                       |
| **/api/Auction/hot-auctions**                 | Get popular auctions                                   |
| **/api/Auction/recommended-auctions**         | Get recommended auctions for bidders                   |
| **/api/Authentication/login**                 | User login                                             |
| **/api/Authentication/register**              | User registration                                      |
| **/api/Authentication/reset-password**        | Reset user password                                    |
| **/api/Authentication/send-otp**              | Send OTP for 2FA                                       |
| **/api/Authentication/verify-otp**            | Verify OTP                                             |
| **/api/Authentication/refresh-token**         | Refresh user authentication token                      |
| **/api/Authentication/logout**                | Logout user                                            |
| **/api/Bid/place-bid**                        | Place a bid on an auction                              |
| **/api/Bid/history/{auctionId}**              | Get the bidding history of an auction                  |
| **/api/Bid/highest/{auctionId}**              | Get the highest bid for an auction                     |
| **/api/Category/with-active-auctions/{categoryId}** | Get auctions for a specific category                 |
| **/api/Category/{categoryId}**                 | Get a category by ID                                   |
| **/api/Category/with-auctions**               | Get all categories with auctions                       |
| **/api/Deposit/create-intent/{auctionId}**    | Create a payment intent for deposit                    |
| **/api/Deposit/webhook**                      | Handle Stripe Webhook for deposit                      |
| **/api/Deposit/has-paid**                     | Check if the deposit has been paid                     |
| **/api/Payment/create-intent/{auctionId}**    | Create a payment intent for payment                    |
| **/api/Payment/webhook**                      | Handle Stripe Webhook for payment                      |
| **/api/Profile/active-bids**                  | Get active bids of the logged-in user                  |
| **/api/Profile/won-auctions**                 | Get won auctions of the logged-in user                 |
| **/api/Seller/auctions-summary**              | Get summary of seller auctions                         |
| **/api/SellerFeedback**                       | Submit feedback for a seller                           |
| **/api/SellerFeedback/seller/{sellerId}**     | Get feedback for a specific seller                     |
| **/api/SellerFeedback/seller/{sellerId}/average-rating** | Get average rating of a seller                   |
| **/api/Wishlist/{key}**                       | Get wishlist of a bidder                               |
| **/api/Wishlist/{key}**                       | Add or remove items in the wishlist                    |

### Conclusion:
This Auction System provides a full-featured auction platform for bidders, sellers, and admins. It integrates modern patterns such as CQRS, SignalR, and uses external services like Stripe for payments and Redis for caching the wishlist. The system is highly scalable and designed to handle various auction operations efficiently.
