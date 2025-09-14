#!/bin/sh

# Exit immediately if a command exits with a non-zero status.
set -e

# --- Configuration ---
# The name of the container to be updated. This should be consistent.
CONTAINER_NAME="opc-core-client"
# The new image tag is passed as the first argument to this script.
NEW_IMAGE_TAG=$1

# --- Validation ---
if [ -z "$NEW_IMAGE_TAG" ]; then
    echo "Error: No new image tag provided."
    echo "Usage: ./update.sh <new-image-tag>"
    exit 1
fi

echo "--- Starting OPC Core Client Update Process ---"
echo "Target version: $NEW_IMAGE_TAG"

# --- Step 1: Pull the new Docker image ---
echo "Step 1: Pulling new image..."
docker pull "$NEW_IMAGE_TAG"
echo "Pull complete."

# --- Step 2: Stop the currently running container (if it exists) ---
# Check if the container exists and is running
if [ "$(docker ps -q -f name=^/${CONTAINER_NAME}$)" ]; then
    echo "Step 2: Stopping currently running container '$CONTAINER_NAME'..."
    docker stop "$CONTAINER_NAME"
    echo "Stop complete."
else
    echo "Step 2: No running container named '$CONTAINER_NAME' found. Skipping stop."
fi

# --- Step 3: Remove the old container (if it exists) ---
if [ "$(docker ps -aq -f name=^/${CONTAINER_NAME}$)" ]; then
    echo "Step 3: Removing old container '$CONTAINER_NAME'..."
    docker rm "$CONTAINER_NAME"
    echo "Remove complete."
else
    echo "Step 3: No container named '$CONTAINER_NAME' found. Skipping remove."
fi

# --- Step 4: Start the new container with the new image ---
# This section must be customized with the correct volumes, network settings,
# and environment variables required for your deployment.
echo "Step 4: Starting new container with image '$NEW_IMAGE_TAG'..."
docker run -d \
    --name "$CONTAINER_NAME" \
    --restart always \
    -v opc-client-buffer:/app/buffer \
    -v /path/to/certs:/app/certs:ro \
    --env-file /path/to/client.env \
    "$NEW_IMAGE_TAG"

echo "New container started successfully."

# --- Step 5: Clean up old, dangling images (optional) ---
echo "Step 5: Cleaning up old images..."
docker image prune -f
echo "Cleanup complete."

echo "--- OPC Core Client Update Process Finished Successfully ---"

exit 0