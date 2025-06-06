Parky
A personal project by João Luiz demonstrating a RESTful API built with ASP.NET Core Web API, following best practices for API development.

📁 Project Structure
ParkyAPI/: Contains the ASP.NET Core Web API project.

ParkyWeb/: Front-end application consuming the API.

src/: Source code directory.

azure-pipelines-2.yml: Azure Pipelines configuration for CI/CD.

🛠️ Technologies Used
ASP.NET Core Web API

Entity Framework Core

Azure Pipelines

🚀 Getting Started
Clone the repository:

bash
Copiar
Editar
git clone https://github.com/JoaoLuizDeveloper/Parky.git
cd Parky
Set up the database:

Ensure you have a SQL Server instance running. Update the connection string in the appsettings.json file as needed.

Run the application:

Open the solution in Visual Studio and run the project. Alternatively, use the .NET CLI:

bash
Copiar
Editar
dotnet build
dotnet run
The API will be available at http://localhost:5000.

📄 License
This project is licensed under the MIT License.
