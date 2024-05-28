
# WindowsDashboard+API
 This folder contains a Visual Studio 2022 solution with two projects that run on the desktop app.

- The first one is a 'Windows Form App' made with .NET Framework 4.7.2 that serves as a 'Dashboard' with CRUD commands and visualization that connects with a PostgreSQL database online. Its executable will also be in the folder.

- The second solution is a Web API made with .NET 6.0 that fetches information from the database and updates it if necessary, this API is used for communication with the mobile applicatio


## To run both projects at once, open the solution and in solution properties, select "Common Properties -> Startup Project" and select "Multiple startup projects", then, change the action for both projects to "Start", apply the changes and start the projects. They will run at the same time.