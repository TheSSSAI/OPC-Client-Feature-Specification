# CI/CD Pipelines Repository

This repository centralizes the definition of all Continuous Integration and Continuous Deployment (CI/CD) workflows for the entire system, using GitHub Actions as specified in REQ-1-089. It is responsible for automating the build, testing, containerization, and deployment of all microservices and the frontend application.

## 🚀 Core Principles

This repository and its workflows are built upon three core principles:

1.  **Pipeline as Code**: All CI/CD processes are defined declaratively in version-controlled YAML files. This provides auditability, repeatability, and disaster recovery for the entire software delivery process.
2.  **Modularity & Reusability**: Common pipeline stages (e.g., build .NET app, deploy with Helm) are encapsulated into self-contained, reusable workflows. This prevents configuration drift, reduces code duplication, and simplifies maintenance for our microservices architecture.
3.  **Security by Default**: Workflows authenticate with cloud providers (AWS) using OpenID Connect (OIDC), exchanging a GitHub-issued OIDC token for temporary cloud credentials. This eliminates the need for storing long-lived secrets and adheres to the principle of least privilege.

## 📁 Repository Structure

The repository is structured according to GitHub Actions conventions to promote separation of concerns:

-   `.github/workflows/`: Contains the primary, top-level orchestrator workflows. These are the main entry points triggered by GitHub events (e.g., pull requests, pushes to main).
    -   `ci-pull-request.yml`: Validates all pull requests.
    -   `cd-service-staging.yml`: Deploys services to the staging environment.
    -   `cd-service-production.yml`: Manages approval-gated deployments to production.
-   `.github/workflows/reusable/`: Contains modular, reusable workflows that encapsulate common pipeline stages (build, test, scan, deploy). These are called by the orchestrator workflows.
-   `.github/actions/`: Contains custom composite actions that bundle a sequence of steps into a single, reusable action (e.g., AWS authentication).
-   `helm/charts/application/`: Contains the master Helm chart template used for deploying all standard application microservices.
-   `scripts/`: Contains utility scripts (e.g., shell, Python) executed by workflow steps for tasks too complex for simple YAML `run` commands.

## ✨ Adding a New Service to CI/CD

To onboard a new microservice, you will typically only need to modify the matrix strategy in the orchestrator workflows (`ci-pull-request.yml`, `cd-service-staging.yml`, etc.) to include the new service's name and path. The reusable workflows handle the rest of the logic.

Example addition to a `matrix`:

```yaml
matrix:
  service:
    - name: 'identity-service'
      path: './services/iam'
    - name: 'asset-service'
      path: './services/asset-topology'
    - name: 'new-awesome-service' # Add new service here
      path: './services/new-awesome' # And its path
```

## 🛠️ Local Development

### Linting

This repository uses `yamllint` and `actionlint` to maintain code quality. Please run these tools locally before committing changes.

```bash
# Install actionlint (see official docs for other methods)
brew install actionlint

# Run linters from the root of the repository
yamllint .
actionlint
```

### Helm Chart Validation

To test changes to the Helm chart locally:

```bash
# Install Helm (see official docs)
brew install helm

# Lint the chart
helm lint ./helm/charts/application

# See the rendered output of a template
helm template my-release ./helm/charts/application --values ./helm/charts/application/values.yaml
```

This ensures that the chart is syntactically correct and will be deployable by the CI/CD pipeline.