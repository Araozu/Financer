FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine3.22 AS build-env
WORKDIR /app

COPY *.slnx ./
COPY dotnet-tools.json ./

RUN dotnet tool restore

# Copy project files first for better layer caching
COPY Api/*.csproj ./Api/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/

# Restore packages for the target runtime - this layer gets cached unless project files change
RUN dotnet restore -r linux-musl-x64

# Copy source code
COPY Api/ ./Api
COPY Application/ ./Application
COPY Domain/ ./Domain
COPY Infrastructure/ ./Infrastructure

# Build and publish the application
RUN dotnet publish Api -c Release -r linux-musl-x64 \
    -o /app/out


# Create EF migrations bundle (self-contained)
ENV ConnectionStrings__DefaultConnection=Host=localhost;Database=financer;Username=root;Password=root
RUN dotnet ef migrations bundle --project Api \
    -r linux-musl-x64 \
    --configuration Release \
    -o /app/out/efbundle


FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine3.22 AS runtime
WORKDIR /app

# Install minimal runtime dependencies
# Needed for docker compose healthchecks
RUN apk add --no-cache curl ffmpeg libwebp-tools

# Create non-root user for security
RUN adduser -D -s /bin/sh appuser

# Copy published application
COPY --from=build-env /app/out ./
COPY +devops/docker/docker-entrypoint.sh ./

# Copy the built frontend static assets
COPY Web/build /app/out/wwwroot

# Make scripts executable
RUN chmod +x ./docker-entrypoint.sh ./efbundle

# Change ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

ENTRYPOINT ["./docker-entrypoint.sh"]
