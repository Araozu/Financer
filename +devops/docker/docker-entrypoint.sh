#!/bin/sh
set -euo pipefail

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

log() {
    echo "$(date '+%Y-%m-%d %H:%M:%S') [$1] $2"
}

log_info() {
    log "INFO" "$1"
}

log_warn() {
    log "WARN" "$1"
}

log_error() {
    log "ERROR" "$1"
}

# Run migrations with proper error handling
run_migrations() {
    log_info "Running database migrations..."

    if [ ! -x "./efbundle" ]; then
        log_error "EF bundle not found or not executable!"
        exit 1
    fi

    if ! ./efbundle; then
        log_error "Database migration failed!"
        exit 1
    fi

    log_info "Database migrations completed successfully"
}

# Start the application
start_application() {
    log_info "Starting application..."

    if [ ! -x "./Reezer.Api" ]; then
        log_error "Application executable not found!"
        exit 1
    fi

    # Execute the application
    exec ./Reezer.Api
}

# Graceful shutdown handler
cleanup() {
    log_info "Received shutdown signal, cleaning up..."
    # Add any cleanup logic here
    exit 0
}

# Set up signal handlers
trap cleanup SIGTERM SIGINT

# Main execution
main() {
    log_info "Starting container..."

    # Run database migrations (DB should be ready thanks to Docker Compose)
    run_migrations

    # Start the application
    start_application
}

# Run main function
main "$@" 
