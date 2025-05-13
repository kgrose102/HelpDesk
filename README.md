# HelpDesk Mock site

![Computer monitor with a hand holding a wrench](/HelpdeskWebsite/wwwroot/images/icon.png)

## Languages and Frameworks:
### C#, ASAP.Net Core, .Net 8.0, SQL, JSON, HTML, CSS, BootStrap, JQuery. Entity Framework, LINQ

## About The project

### This site uses a Client Server approach where the user interface and SQL based database is seperated through a Data Access Layer.

### It is a test site for creating a ticketing system for an IT backend where tickets are submitted and are able to be assigned to a worker, and closed to be viewed but not editable after being closed.

### There are test cases for testing the functionality of the access layers ensuring they are functioning properly.

## Setting it up
### You will need to open the Visual Studio project.
### Ensure the required NuGet Packages are installed by right-clicking the solution in the solution explorer and selecting "Restore NuGet Packages"
### You will also need to create an SQL database called "HelpdeskDb" or update the database name in: HelpdeskDAL -> HelpdeskContext.cs utilizing the included "HelpDesk_Setup_SQL.sql" file for creating the database tables.
### Lazy Loading is currently enabled to streamline the local dev access to the server.
### Ensure HelpdeskWebsite is set as the default start up project before running the project.

### © Kenneth Rose 2025
