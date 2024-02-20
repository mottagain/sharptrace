# sharptrace
Toy C# raytracer.

Steps to set up a new solution with test cases:

```
dotnet new sln
mkdir src
cd src
dotnet new console
cd ..
mkdir tests
cd tests
dotnet new xunit
dotnet add reference ../src/src.csproj
cd ..
dotnet sln add src/src.csproj
dotnet sln add tests/tests.csproj
```
