# ABC Car Traders Windows Forms Application

## Table of Contents

1. [Introduction](#introduction)
2. [Features](#features)
3. [System Architecture](#system-architecture)
   - [Architectural Design](#architectural-design)
   - [ER Diagram](#er-diagram)
   - [Class Diagram](#class-diagram)
   - [Use Case Diagram](#use-case-diagram)
4. [Installation Guide](#installation-guide)
5. [Usage](#usage)
   - [Customer Registration and Authentication](#customer-registration-and-authentication)
   - [Ordering an Item and Viewing Order Status](#ordering-an-item-and-viewing-order-status)
   - [Admin Module](#admin-module)
   - [Managing Data](#managing-data)
6. [Report Generation](#report-generation)
7. [License](#license)

## Introduction

This Windows Forms application for ABC Car Traders is designed to enhance customer service and operational efficiency. It integrates advanced C# features to provide a seamless user experience for both customers and staff, handling user authentication, order processing, inventory management, and administrative tasks.

## Features

- **Customer Registration and Authentication**: Secure registration and login for customers.
- **Order Processing**: Browse, search, and purchase cars and car parts.
- **Inventory Management**: Admin functionalities for managing cars, car parts, and orders.
- **Report Generation**: Generate various business reports for insights.

## System Architecture

### Architectural Design

The architectural design of the application is built on a layered structure, ensuring modularity and ease of maintenance:

1. **Presentation Layer**: Handles the user interface and interactions.
2. **Business Logic Layer**: Contains core functionalities and business rules.
3. **Data Access Layer**: Manages data retrieval and storage.

### Architectural Diagarm
<img width="735" alt="Screenshot 2024-06-24 at 21 06 05" src="https://github.com/Sahiru2007/Car-Sales-System/assets/75121314/8f0c5a76-050f-4427-8d65-34372d21dde1">

### ER Diagram
<img width="1073" alt="Screenshot 2024-06-24 at 21 03 26" src="https://github.com/Sahiru2007/Car-Sales-System/assets/75121314/e266bb00-53b4-4ffa-a51a-8a20b69e9379">
The ER diagram illustrates the relationships between entities such as customers, orders, cars, and car parts.

### Class Diagram
<img width="1010" alt="Screenshot 2024-06-24 at 21 03 49" src="https://github.com/Sahiru2007/Car-Sales-System/assets/75121314/6cc62bdc-51a3-49b0-8a08-23260027d2c7">
The class diagram shows the classes used in the application, their properties, and methods, along with the relationships between them.

### Use Case Diagram
<img width="663" alt="Screenshot 2024-06-24 at 21 04 01" src="https://github.com/Sahiru2007/Car-Sales-System/assets/75121314/4d00a612-b7dc-4247-a274-44168492f068">
The use case diagram provides an overview of the interactions between users (customers and admins) and the system.

## Installation Guide

### Prerequisites

- Windows operating system
- .NET Framework
- SQL Server or compatible database

### Steps

1. **Download and Install Visual Studio**
   - Go to the [Visual Studio website](https://visualstudio.microsoft.com/) and download the Community edition (or any edition suitable for you).
   - Follow the installation instructions provided on the website.
   - During installation, ensure you select the ".NET desktop development" workload.

2. **Clone the Repository**
   - Open Visual Studio.
   - Go to `File` > `Clone or check out code`.
   - Enter the repository URL: `https://github.com/Sahiru2007/Car-Sales-System.git`.
   - Click `Clone`.

3. **Build the Solution**
   - After cloning the repository, open the solution file (`.sln`) in Visual Studio.
   - Restore the NuGet packages by right-clicking on the solution in the Solution Explorer and selecting `Restore NuGet Packages`.
   - Build the solution by pressing `Ctrl+Shift+B` or by selecting `Build` > `Build Solution`.

4. **Configure the Database**
   - Ensure SQL Server is installed and running on your machine.
   - Update the connection string in the `appsettings.json` or `Web.config` file to point to your SQL Server instance.
   - Apply migrations to create the database schema. Open the Package Manager Console in Visual Studio and run the following commands:
     ```
     Update-Database
     ```

5. **Run the Application**
   - Set the startup project by right-clicking on the project in the Solution Explorer and selecting `Set as Startup Project`.
   - Run the application by pressing `F5` or by selecting `Debug` > `Start Debugging`.

## Usage

### Customer Registration and Authentication

1. **Open the application**: Launch the application from the desktop shortcut.
2. **Navigate to Customer Registration**: Click on 'Customer' on the home page and go to the registration section.
3. **Fill out the registration form**: Enter all required details and click 'Register'.
4. **Login**: After registration, use the credentials to log in.

### Ordering an Item and Viewing Order Status

1. **Browse items**: Navigate to the 'Cars' or 'Car Parts' section.
2. **Search for items**: Enter a search term and select a category, then click 'Search'.
3. **Place an order**: Select the desired item, specify the quantity, and click 'Purchase'.
4. **View order status**: Go to the 'My Orders' section to see the status of your orders.

### Admin Module

1. **Admin login**: Navigate to the admin login page and enter the admin credentials.
2. **Dashboard**: Access the admin dashboard to view various metrics and manage operations.

### Managing Data

- **Add data**: Navigate to the relevant section (Cars, Car Parts, Orders, Customers), fill in the details, and click 'Add'.
- **Search data**: Use the search functionality to filter data based on specified criteria.
- **Update data**: Select a record, enter the new values, and click 'Update'.
- **Delete data**: Select a record and click 'Delete', then confirm the deletion.

## Report Generation

Admins can generate various reports to gain insights into the business operations. These reports include data on sales, inventory, customer activity, and more.

1. **Login as admin**: Access the admin section.
2. **Navigate to reports**: Go to the 'Reports' tab.
3. **Generate reports**: Select the type of report and click 'Generate'.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
