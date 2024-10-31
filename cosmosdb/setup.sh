#!/bin/bash

set -eu

# Wait for the Cosmos DB Emulator to be ready
until curl -sk https://localhost:8081/_explorer/emulator.pem; do
  >&2 echo "setup.sh | Cosmos DB Emulator is unavailable - sleeping"
  sleep 5
done

# Download the emulator certificate and add it to the trusted certificates
curl -sk https://localhost:8081/_explorer/emulator.pem > /usr/local/share/ca-certificates/emulator.crt && update-ca-certificates
export REQUESTS_CA_BUNDLE=/etc/ssl/certs/emulator.pem

# By default use the static development key for Cosmos DB Emulator
auth_token=${AZURE_COSMOS_EMULATOR_KEY:-"C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="}

# Create a database
az cosmosdb database create --key $auth_token --db-name "mydatabase" --url-connection "https://localhost:8081"

# Create a container
az cosmosdb collection create --key $auth_token --db-name "mydatabase" --collection-name "mycontainer" --partition-key-path "/id" --url-connection "https://localhost:8081"

echo "setup.sh | Setup complete"
