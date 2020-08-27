# tadpole
Simple registration demo for a recruitment test project. 

This solution uses Razor pages for the web project, unit tests in NUnit, integration tests in NUnit with AngleSharp and a SQL Server Database project.

The CSS was not a requirement and so the bulk of it has been copied from an existing project.

The integration tests use two helper classes adapted from [the xunit ones used here](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/test/integration-tests/samples/3.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/Helpers) to make the tests readable.

The rest of the code is my fault.


## Database Notes

The SQL to create the only table in this project can be found in [Tadpole.Database\Tables\RegisteredUser.sql](https://github.com/PhilipBathe/tadpole/blob/master/Tadpole.Database/Tables/RegisteredUser.sql).

The web project uses a SQL Server database called Tadpole.Database.

The integration test project uses its own database called Tadpole.Test.Database.

Please create both databases on (localdb)\MSSQLLocalDB and add the RegisteredUser table.

Having a dedicated integration test database helps to prevent a developer from breaking the integration tests whilst conducting their own black box tests.
