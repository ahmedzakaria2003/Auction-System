

# Auction System - Full-Stack Platform

Welcome to the **Auction System** repository! This project represents a fully-featured auction platform designed to offer a seamless and efficient auction experience for **Bidders**, **Sellers**, and **Admins**. Built with cutting-edge technologies, this platform is optimized for performance, scalability, and security.

### 🚀 **Demo Link**

Before diving into the technical details, feel free to watch a demo of the system on my [LinkedIn Post](https://www.linkedin.com/posts/ahmed-zakaria-454aa8319_%D8%AD%D8%A7%D8%A8%D8%A8-%D8%A3%D8%B4%D8%A7%D8%B1%D9%83-%D9%85%D8%B9%D8%A7%D9%83%D9%85-%D8%B4%D8%BA%D8%A7%D9%84-%D8%B9%D9%84%D9%8A%D9%87-%D8%A7%D9%84%D9%81%D8%AA%D8%B1%D8%A9-activity-7364184661952012288-KuF6?utm_source=share&utm_medium=member_desktop&rcm=ACoAAFDJKoABOn-WUl7taCRZ6RDfzEiHpbFFNT4).

---

## Key Highlights

### 💡 **Real-Time Bidding**

Using **SignalR**, this auction system provides real-time updates and notifications, ensuring that users receive instant notifications about bid updates and auction status.

### 💳 **Stripe Payment Integration**

Secure payment processing with **Stripe** for handling deposits and payments, ensuring a safe and smooth transaction process.

### 🔐 **JWT Authentication with Refresh Tokens**

Secure user authentication using **JWT** tokens, combined with **Refresh Tokens**, to maintain uninterrupted user access without needing to log in repeatedly.

### 🛡️ **Two-Factor Authentication (2FA)**

Added **2FA** with **OTP** to increase the security of user accounts and prevent unauthorized access.

### 🛠️ **Role-Based Access Control (RBAC)**

Different roles for **Bidders**, **Sellers**, and **Admins**, each with specific access to different features of the platform.

### 🔄 **Background Services**

Automatic winner declaration using background services, ensuring fair and accurate auction results.

---

## Key Features

### For **Bidders**:

* Place bids, view auction details, and manage profile
* Real-time updates of auction progress
* Secure payments via **Stripe**

### For **Sellers**:

* Create, manage, and track auctions
* Monitor bids and feedback
* View auction statistics

### For **Admins**:

* Oversee all auctions, manage users, and view detailed statistics

---

## Technology Stack

### Backend:

* **.NET 8**
* **SignalR** (Real-time bidding)
* **Stripe** (Payments)
* **JWT Authentication** (Secure user management)
* **EF Core** (Database management)
* **Redis** (Caching)
* **Onion Architecture**, **Unit of Work**, **Repository Patterns** for maintainability and scalability

### Frontend:

* **Angular 18** (Modern features and performance optimizations)
* **RxJS** (Reactive programming)
* **Bootstrap** (Responsive design)
* **SignalR Integration** (Real-time updates)
* **Stripe Integration** (Secure payments)
* **2FA with OTP** (Enhanced security)

### Additional Features:

* **Time Remaining**: Real-time countdown for auctions using **RxJS**.
* **Advanced Search**: Sort and filter auctions by name, price, and time left.
* **Role Guards**: Ensuring secure access to different pages based on user roles.

---

## How to Run Locally

### 1. Clone the repository

```bash
git clone https://github.com/ahmedzakaria2003/auction-system.git
cd auction-system
```

### 2. Backend Setup

Make sure you have **.NET 8** installed. Then, run the following:

```bash
cd AuctionSystem.API
dotnet restore
dotnet run
```

### 3. Frontend Setup

Ensure you have **Node.js** and **Angular CLI** installed. Then, run:

```bash
cd AuctionSystem.Frontend
npm install
ng serve
```

### 4. Database Setup

Make sure **SQL Server** is running and use **Entity Framework Core** for database migrations:

```bash
dotnet ef database update
```

### 5. Stripe Configuration

Configure **Stripe** by adding your secret API key to the `.env` file:

```bash
STRIPE_SECRET_KEY=your_secret_key
```

---

## Contributing

We welcome contributions to improve the project. Feel free to fork this repository, make changes, and create a pull request. Please make sure your code adheres to the existing standards and that all tests pass.

---



