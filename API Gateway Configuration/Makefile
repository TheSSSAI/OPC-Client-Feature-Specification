# Makefile for API Gateway Configuration Repository

# Define the shell to use
SHELL := /bin/bash

# Define directories for Kustomize overlays
OVERLAYS := overlays/development overlays/staging overlays/production

# Default target executed when `make` is called without arguments
.DEFAULT_GOAL := help

## ----------------------------------------------------------------------------
## Tooling Setup
## ----------------------------------------------------------------------------

.PHONY: install-tools
install-tools: ## Install development tools specified in .tool-versions using asdf
	@echo "🛠️  Installing tools via asdf..."
	@if ! command -v asdf &> /dev/null; then \
		echo "Error: asdf is not installed. Please follow instructions at https://asdf-vm.com"; \
		exit 1; \
	fi
	asdf plugin add yamllint || true
	asdf plugin add kubectl || true
	asdf plugin add kustomize || true
	asdf install

## ----------------------------------------------------------------------------
## Quality & Validation
## ----------------------------------------------------------------------------

.PHONY: lint
lint: ## Lint all YAML files in the repository
	@echo "🧹 Linting all YAML files..."
	@yamllint .

.PHONY: validate
validate: ## Validate Kustomize configurations for all environments
	@echo "🔍 Validating Kustomize build for all environments..."
	@for overlay in $(OVERLAYS); do \
		echo "--- Validating $$overlay ---"; \
		kustomize build $$overlay > /dev/null; \
		if [ $$? -ne 0 ]; then \
			echo "❌ Validation failed for $$overlay"; \
			exit 1; \
		else \
			echo "✅ Validation successful for $$overlay"; \
		fi; \
	done
	@echo "All environments validated successfully."

## ----------------------------------------------------------------------------
## Local Deployment (for development/testing)
## ----------------------------------------------------------------------------
# Note: Requires KUBECONFIG to be set to the target cluster

.PHONY: apply-dev
apply-dev: validate ## Apply the development overlay to the current Kubernetes context
	@echo "🚀 Applying development configuration..."
	@kustomize build overlays/development | kubectl apply -f -

.PHONY: delete-dev
delete-dev: ## Delete the development overlay from the current Kubernetes context
	@echo "🔥 Deleting development configuration..."
	@kustomize build overlays/development | kubectl delete --ignore-not-found=true -f -

## ----------------------------------------------------------------------------
## Help
## ----------------------------------------------------------------------------

.PHONY: help
help: ## Display this help screen
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'