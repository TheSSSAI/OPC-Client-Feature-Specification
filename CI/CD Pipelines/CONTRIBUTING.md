# Contributing to the CI/CD Pipelines Repository

Thank you for your interest in contributing! This document outlines the process and guidelines for making changes to our CI/CD workflows. Due to the critical nature of this repository, all changes are subject to stringent review.

## 📜 Guiding Principles

1.  **Security First**: Any change must uphold our security standards. This includes using OIDC for cloud access, managing secrets appropriately, and scanning for vulnerabilities. Changes that introduce long-lived credentials will be rejected.
2.  **Modularity is Mandatory**: Do not repeat yourself. If you find yourself copying and pasting steps between workflows, that logic should be extracted into a reusable workflow or a composite action.
3.  **Stability is Key**: The pipelines in this repository deploy code to production. Changes must be thoroughly tested and backward-compatible where possible.

## 🚀 Development Process

1.  **Create an Issue**: Before starting work, please create a GitHub issue describing the change you wish to make.
2.  **Create a Feature Branch**: Create a new branch from `main` for your changes.
    ```bash
    git checkout -b feat/my-new-feature
    ```
3.  **Make Your Changes**:
    -   Modify or add workflows, actions, or scripts.
    -   Ensure all new or modified YAML files pass local linting checks.
4.  **Lint Locally**: Before pushing, run the linters to catch common errors.
    ```bash
    yamllint .
    actionlint
    ```
5.  **Commit and Push**: Use conventional commit messages to describe your changes.
    ```bash
    git commit -m "feat: add reusable workflow for static analysis"
    git push origin feat/my-new-feature
    ```
6.  **Create a Pull Request**: Open a pull request from your feature branch to `main`.
    -   Fill out the PR template, linking to the issue you created.
    -   Ensure all automated checks and tests pass.
7.  **Request a Review**: Request a review from at least two members of the DevOps/Platform team.

## 🔬 Pull Request Review Criteria

A pull request will be approved and merged if it meets the following criteria:

-   [ ] **All automated checks are passing**.
-   [ ] The change adheres to the guiding principles of **Security and Modularity**.
-   [ ] The code is well-documented with comments where necessary.
-   [ ] The change has been manually tested or is covered by a new automated test if applicable.
-   [ ] The PR description is clear and explains the "what" and "why" of the change.
-   [ ] The change has been approved by at least **two** required reviewers.

## 📝 Style Guides

-   **YAML**: Follow the rules defined in `.yamllint.yml`. Use 2 spaces for indentation.
-   **Shell Scripts**: Use `shellcheck` to validate your scripts. Scripts should be POSIX-compliant where possible.
-   **GitHub Actions**: Use descriptive names for jobs and steps. Use environment variables for clarity and reusability.