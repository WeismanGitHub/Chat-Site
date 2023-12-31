# Stage 1: Build React.js application with TypeScript
FROM node:16 AS build-client
WORKDIR /usr/src/app/Source/Client

# Copy package.json and package-lock.json separately to leverage Docker cache
COPY ./Source/Client/package*.json ./
RUN npm install

# Copy the React.js application files
COPY ./Source/Client .

# Build TypeScript source
RUN npm run build

# Stage 2: Build ASP.NET Core application with TypeScript
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-api
WORKDIR /usr/src/app/Source/API

# Copy the project files and restore dependencies
COPY ./Source/API/API.csproj .
RUN dotnet restore

# Copy the application files
COPY ./Source/API .

# Build TypeScript source
RUN dotnet publish -c Release -o out

# Stage 3: Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /usr/src/app/Source/API

# Copy the ASP.NET Core application files
COPY --from=build-api /usr/src/app/Source/API/out .

# Expose the port that the application will run on
EXPOSE 8080/tcp

# Define the entry point for the application
ENTRYPOINT ["dotnet", "API.dll"]
