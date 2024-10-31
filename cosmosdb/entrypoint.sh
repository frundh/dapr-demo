#!/bin/bash

set -m

# Start the Cosmos DB Emulator
/usr/local/bin/cosmos/bin/start.sh &

# Run setup script
./setup.sh

fg %1
