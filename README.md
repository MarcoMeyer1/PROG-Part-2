# Marketplace Website

## Introduction

Welcome to the Marketplace Website repository. This e-commerce platform allows users to browse and search for products, view farmer profiles, and much more. The website is built using ASP.NET MVC and is integrated with an Azure SQL Database, making setup easy and straightforward.

## Table of Contents

1. Development Environment Setup
2. Building and Running the Project
3. System Functionalities and User Roles
4. Detailed Prototype Functionality Requirements
5. Additional Notes

## Development Environment Setup

### Prerequisites

- Visual Studio 2019 or later
- .NET 6.0 SDK
- Git

### Step-by-Step Instructions

1. Clone the Repository

2. Open the Solution in Visual Studio:

   Launch Visual Studio and open the cloned repository by selecting the `.sln` file.

3. Restore NuGet Packages:

   Right-click on the solution in the Solution Explorer and select `Restore NuGet Packages`.

## Building and Running the Project

1. Build the Project:

   In Visual Studio, select `Build` from the top menu and click `Build Solution`.

2. Run the Project:

   Press `F5` or click the `Start` button in Visual Studio to run the project. The website will automatically launch in your default web browser. No additional setup is needed for the database as it is hosted on Azure.

## System Functionalities and User Roles

### User Roles

**Normal Users:**
   - Browse and search for products.
   - View farmer profiles.

**Employees:**
   - Add new farmer profiles.
   - View all products from specific farmers.
   - Use filters for product searching.

### Key Functionalities

1. **Product Filtering:**
   - Users can filter products by start date, end date, category, and farmer.

2. **Farmer Profiles:**
   - Each farmer has a profile page displaying their products and details.

3. **Dynamic Product Display:**
   - The marketplace dynamically displays products based on the applied filters.

## Detailed Prototype Functionality Requirements

1. **Database Development and Integration:**
   The system integrates a relational database hosted on Azure to manage information about farmers and their products. The database is pre-populated with sample data to simulate real-world scenarios, ensuring the demonstration is robust and comprehensive.

2. **User Role Definition and Authentication System:**
   There are two distinct user roles: Farmers can add products to their profiles and view their own listings. Employees can add new farmer profiles, view all products from specific farmers, and use filters for searching. The system includes secure login functionality with authentication mechanisms to protect user data and ensure role-specific access.

3. **Functional Features for Farmers and Employees:**
   For Farmers, the system enables adding new products with details like name, category, and production date. For Employees, the system allows adding new farmer profiles with essential details and viewing/filtering products from any farmer based on criteria like date range and product type.

4. **User Interface Design and Usability:**
   The website features a user-friendly interface with intuitive navigation and a clear layout. It is responsive and accessible on various devices (desktops, tablets, smartphones). The data presentation is clear and accurate, avoiding any ambiguity or errors.

5. **Data Accuracy and Validation:**
   The system incorporates data validation checks to maintain the accuracy and consistency of the information entered. It includes error-handling mechanisms to prevent system crashes and data corruption.

6. **Development Process and Testing:**
   The prototype is developed iteratively, with each functionality tested as it is implemented. User experience (UX) testing with sample users is conducted (if possible) to gather feedback on the usability and effectiveness of the interface.

7. **Documentation and Readme File:**
   This comprehensive readme file includes step-by-step instructions for setting up the development environment, detailed guidelines on how to build and run the prototype, and explanations of the system’s functionalities and user roles. It is written in clear, concise language, making it accessible for both technical and non-technical stakeholders.

## Additional Notes

**Database Integration:**
   The website is integrated with an Azure SQL Database, ensuring that all data is stored and retrieved securely. No additional database setup is required as it is hosted on Azure.


