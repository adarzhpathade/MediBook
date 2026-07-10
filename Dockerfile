# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
# Expose port 8080 for web traffic (standard for cloud platforms like Render)
EXPOSE 8080

# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["MediBook/MediBook.csproj", "MediBook/"]
RUN dotnet restore "./MediBook/MediBook.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
WORKDIR "/src/MediBook"
RUN dotnet build "./MediBook.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application to a folder
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MediBook.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: copy the published app to the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variable to listen on port 8080 (expected by most PaaS)
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MediBook.dll"]
