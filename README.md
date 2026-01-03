# PPMP - Project Portal Management Platform

## Overview
PPMP is a comprehensive client portal and project management system designed to provide automated, consistent updates to clients. The platform streamlines project communication, tracking, and reporting for improved client engagement and project transparency.

## Prerequisites
Before running this project, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Node.js v22.16.0](https://nodejs.org/)
- Git

## Project Structure
```
PPMP/
├── Devops/          # Docker Compose configurations
├── PPMP/            # ASP.NET MVC application
├── .gitignore
└── README.md
```

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd PPMP
```

### 2. Start Database Services
Navigate to the Devops folder and start the MariaDB container:
```bash
cd Devops
docker-compose -f mariaDB.yml up -d
```

### 3. Install Frontend Dependencies
Navigate to the PPMP project folder and install Tailwind CSS dependencies:
```bash
cd PPMP
npm install
```

### 4. Build Tailwind CSS
```bash
npm run build:css
# or for development with watch mode
npm run watch:css
```

### 5. Configure Application Settings
Create an `appsettings.Development.json` file in the PPMP folder with your connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=ppmp;User=root;Password=yourpassword;"
  }
}
```

### 6. Run Database Migrations
```bash
dotnet ef database update
```

### 7. Run the Application
```bash
dotnet run
```

## Development

### Database Management
- **Start database**: `docker-compose -f mariaDB.yml up -d` (in Devops folder)
- **Stop database**: `docker-compose -f mariaDB.yml down`
- **View logs**: `docker-compose -f mariaDB.yml logs -f`

### Tailwind CSS Development
To watch for CSS changes during development:
```bash
npm run watch:css
```

### Common Commands
```bash
# Restore packages
dotnet restore

# Build the project
dotnet build

# Run tests
dotnet test

# Create new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## Configuration

### Environment Variables
Create a `.env` file in the Devops folder for Docker configuration:
```env
MARIADB_ROOT_PASSWORD=yourpassword
MARIADB_DATABASE=ppmp
MARIADB_USER=ppmpuser
MARIADB_PASSWORD=userpassword
```

## Deployment

### Production Build
```bash
dotnet publish -c Release -o ./publish
```

### Docker Deployment
```bash
docker-compose -f docker-compose.prod.yml up -d
```

## Contributing
1. Create a feature branch (`git checkout -b feature/AmazingFeature`)
2. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
3. Push to the branch (`git push origin feature/AmazingFeature`)
4. Open a Pull Request

## Troubleshooting

### Database Connection Issues
- Ensure Docker containers are running: `docker ps`
- Check MariaDB logs: `docker-compose logs mariadb`
- Verify connection string in `appsettings.Development.json`

### Tailwind CSS Not Building
- Verify Node.js version: `node --version`
- Reinstall dependencies: `npm install`
- Check `tailwind.config.js` configuration


📄 License
This project is open source and available under the MIT License.

