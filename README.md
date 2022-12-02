# ORM
This sample repository demonstrates the use of Object Relational Mappers (ORMs). It uses both Entity Framework, a proprietary ORM within the .Net environment and a third part micro-ORM, Dapper, that can be used in the .Net environment.

This is a project that reads and writes from a Microsoft SQL Server instance. The structure of the database is a Person that has a 0 to 1 relationship with a table Address, and a 0 to many relationship with table Child.

## Setup
1. Run the `DatabaseCreate.sql` script in the `Setup` folder to generate the database on a SQL Server instance
2. Modify in the `ORM` project the `appsettings.json` file's ConnectionStrings section to match the appropriate connection string completed in step 1 for `tinkering`
3. In `Program.cs` in `ORM` project, uncomment the Inversion of Control Container binding section for either Entity Framework or Dapper to be utilized when you run the app. Only one can be uncommented, and one must be active for the app to run.
4. Run the application in Debug mode. You should see the swagger documentation.

## Example HTTP Requests For POST and PUT
### Person with no address
```
{
   "firstName":"Joey",
   "lastName":"Joe Joe",
   "age":52
}
```
### Person with address
```
{
   "firstName":"Joey",
   "lastName":"Joe Joe",
   "age":52,
   "address": {
      "address1":"123 Fake Street",
      "city":"Fakeville",
      "state":"NJ",
      "zip":"12345"
   }
}
```
