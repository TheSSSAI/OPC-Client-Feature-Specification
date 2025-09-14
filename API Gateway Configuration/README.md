# API Gateway Configuration Repository

This repository contains the declarative configuration for the **Kong API Gateway**, which serves as the single, secure entry point for all external traffic to the Industrial IoT Platform's Central Management Plane.

This repository follows GitOps principles. The `main` branch is the source of truth for the gateway's configuration across all environments (Development, Staging, Production). All changes are managed via Pull Requests and validated by an automated CI pipeline.

## 🚀 Overview

The API Gateway is responsible for:
- **Routing**: Directing incoming REST API requests to the appropriate backend microservices.
- **Security**: Enforcing JWT validation for all authenticated endpoints.
- **Traffic Management**: Applying rate-limiting policies to prevent abuse and ensure stability.
- **CORS**: Handling Cross-Origin Resource Sharing policies for the frontend single-page application.
- **API Documentation**: Aggregating OpenAPI specifications from backend services into a unified, public-facing API definition.

## 📁 Repository Structure

This repository uses [Kustomize](https://kustomize.io/) to manage configurations across multiple environments.

- **`base/`**: Contains the common, environment-agnostic Kubernetes manifests (Ingress, KongPlugin, etc.). This is the foundational configuration.
  - **`plugins/`**: Defines reusable Kong policies like JWT validation and rate-limiting.
  - **`routing/`**: Defines the Ingress rules that map public API paths to internal Kubernetes services.
- **`overlays/`**: Contains environment-specific patches that modify the `base` configuration.
  - **`development/`**: Configuration for the `dev` environment (e.g., development hostnames, relaxed rate limits).
  - **`staging/`**: Configuration for the `staging` environment.
  - **`production/`**: Configuration for the `prod` environment (e.g., public FQDNs, strict rate limits, TLS secrets).

## 🛠️ Prerequisites

To work with this repository locally, you need the following tools. We highly recommend using [asdf](https://asdf-vm.com/) to manage tool versions.

1.  **asdf**: A CLI tool to manage multiple runtime versions.
2.  **kubectl**: The Kubernetes command-line tool.
3.  **kustomize**: A tool to customize Kubernetes configurations.
4.  **yamllint**: A linter for YAML files.

Once `asdf` is installed, run the following command to install the tools specified in `.tool-versions`:

```sh
make install-tools
```

## ⚙️ Common Commands

A `Makefile` is provided to simplify common tasks.

- **`make lint`**: Lints all YAML files in the repository to ensure they adhere to style guidelines.
- **`make validate`**: Runs `kustomize build` on all environment overlays to check for syntactical and structural errors. This is the primary validation step in the CI pipeline.
- **`make apply-dev`**: (For local development) Applies the `development` overlay configuration to the Kubernetes cluster currently targeted by your `KUBECONFIG`.
- **`make delete-dev`**: (For local development) Deletes the `development` overlay configuration from your cluster.
- **`make help`**: Displays a list of all available Makefile commands.

## 🚦 CI/CD Pipeline

All Pull Requests targeting the `main` branch will trigger the **PR Validation Check** GitHub Actions workflow (`.github/workflows/pr-check.yml`). This workflow performs the following checks:

1.  **YAML Linting**: Runs `make lint`.
2.  **Kustomize Validation**: Runs `make validate`.

A Pull Request cannot be merged unless all checks pass, ensuring that the `main` branch always contains a valid and deployable configuration.