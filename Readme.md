# Project: ImageSourceStorage
## Table of Content

- Introduction
- Project set-up 
- Database Entities 
- Features


## Introduction

Introduction: Backend part implementation and deployment of a service which stores and sells digital image resources. basically It’s a simple version of the famous Pinterest app. The following technologies/sources were used To develop this project:

- Entity Framework Core
- Swagger UI 
- Dependency injection in ASP.NET Core
- Unit tests coverage (xUnit)
- SQL Server 

## Project set-up

To set-up the project please follow these steps:

1- Create a database at sql server.
2- Clone the code of the repository -InternshipAssignment- which could be found at https://github.com/eliasINDG/InternshipAssignment
3- Change the ConnectionStrings.ImageSourceDatabase in your appsettings.json
4- Build the project 
5- Open Package Manager
6- For the Default project Chose: ImageSourcesStorage.DataAccessLayer
7- Insert the following commands:

```sh
Update-Database
```
## Database Entities 

The database entities are generated after you finish step 7 above

the following entites should be present at the database:

- Users
- PinBoards
- Pins
- Boards

## Features

- The user should have the opportunity to create edit and remove a board with some name. 
User cannot have two or more boards with the same name. The maximum length of the board name is 50 characters

- The user should have the opportunity to upload the image to the board.

- The user should be able to see all pins in the system.

- The user should be able to see all his boards with related pins.

- The user should be able to save/assign the pin of another user to his board.

 - The user should be able to unassign the pin from the board.

