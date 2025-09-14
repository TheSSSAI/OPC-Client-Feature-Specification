### **Software Requirements Specification (SRS)**

---

### **1. Introduction**

#### **1.1 Purpose**
This document specifies the software requirements for a distributed, multi-tenant OPC Client and Central Management system. It details the functional, non-functional, and interface requirements necessary for the design, development, and testing of the system.

#### **1.2 Project Scope (In-Scope)**
*   **REQ-SCP-001:** The system shall be a cross-platform OPC Client application with a centralized, web-based management plane.
*   The system shall be capable of connecting to OPC DA, OPC UA, and OPC XML-DA servers.
*   The system shall implement real-time data access, historical data access, and alarm and event monitoring features.
*   The system shall integrate AI/ML-driven features for predictive maintenance, anomaly detection, and natural language querying.
*   The web-based management plane shall support remote configuration, monitoring, and deployment of distributed client instances.
*   The system shall support integration with major IoT platforms, AR devices, and Digital Twins.
*   The system shall implement robust security mechanisms, multi-tenancy, and data integrity mechanisms.

---

### **2. Overall Description**

#### **2.1 Product Perspective**
*   **REQ-ARC-001:** The system shall be a distributed software system composed of an on-premise/edge OPC Core Client component and a cloud-hosted Central Management Plane component.
*   The OPC Core Client shall run on industrial hardware and directly interface with OPC servers to perform data acquisition, local control, and edge processing. The OPC Core Client shall be capable of autonomous operation, including data acquisition, buffering, and edge processing, during periods of disconnection from the Central Management Plane.
*   The Central Management Plane shall provide a web-based interface for multi-site management, advanced analytics (model training, batch reporting, and non-real-time analysis), user administration, and data aggregation.
*   The OPC Core Client shall communicate securely with the Central Management Plane using a dual-protocol strategy: gRPC with mutual TLS (mTLS) for high-throughput, low-latency data streaming, and MQTT v5 over TLS for robust command, control, and status message delivery. This approach shall support bidirectional communication, a persistent connection model with keep-alives, and efficient binary serialization suitable for industrial data volumes and potentially unreliable networks.

#### **2.2 User Classes and Permissions**
*   **REQ-USR-001:** The system shall define the following user classes with specific permissions:
    *   **Administrator:** Responsible for system setup, user management, security configuration, and overall system health. This role shall have full control over all system features, including tenant-level settings, user management, role configuration, security settings, client instance management, and the authority to import, validate, and approve AI models for use within the system.
    *   **Data Scientist:** Responsible for developing, training, and validating AI/ML models. This role shall have Read/Write access to a dedicated data sandbox environment, Read access to historical production data, and the ability to submit trained models for validation and approval by an Administrator. They shall have no access to production system configuration or control operations.
    *   **Engineer:** Responsible for configuring data sources, tags, dashboards, and reports. This role shall have Read/Write access to tag configuration, dashboards, and reports; Read access to the approved AI model library and Write access to assign pre-approved models to assets; Read-only access to system logs and audit trails; and no access to user management or system-level security settings.
    *   **Operator:** Responsible for day-to-day monitoring, viewing data, and responding to alarms. This role shall have Read access to assigned dashboards and real-time data, the ability to acknowledge and shelve alarms within their assigned area, and write access to specific process setpoints as defined by an Engineer or Administrator. All setpoint changes shall be logged in the audit trail.
    *   **Viewer:** A read-only role with access to dashboards and reports. This role shall not be able to make any changes to the system.

#### **2.3 Operating Environment**
*   **REQ-ENV-001:** The system shall operate in the following environments:
    *   **OPC Core Client & Edge AI Module:**
        *   Supported Operating Systems: Windows 10/11, Windows Server 2019+, Ubuntu 20.04+, Red Hat Enterprise Linux 8+.
        *   Supported Hardware: Standard Industrial PC (x86-64 architecture), NVIDIA Jetson hardware for Edge AI.
        *   Software Dependencies: .NET 8 Runtime, Docker, OPCFoundation.NetStandard.Opc.Ua library.
        *   The OPC Core Client shall be packaged as a Docker container to ensure consistent deployment across supported operating systems.
    *   **Central Management Plane:**
        *   Deployment Environment: A managed Kubernetes service on Amazon EKS.
        *   User Access: A modern web browser (Chrome, Firefox, Edge, Safari).

#### **2.4 Design and Implementation Constraints**
*   **REQ-CON-001:** The system shall adhere to the following design and implementation constraints:
    *   The core application logic shall be developed using the .NET framework (version 8).
    *   The Central Management Dashboard frontend shall be developed using React.
    *   The system shall support OPC DA, OPC UA, and OPC XML-DA protocols.
    *   The audit trail feature shall be designed to support compliance with regulations such as FDA 21 CFR Part 11, requiring tamper-evident, timestamped logs of all significant actions.
    *   The system architecture shall be multi-tenant to support a SaaS licensing model.
    *   **Multi-Tenancy Data Isolation:** Data isolation between tenants shall be strictly enforced using a hybrid strategy:
        *   **Databases (PostgreSQL/TimescaleDB):** Use row-level security (RLS) with a mandatory `tenant_id` column in all shared tables as the primary strategy. Schema-per-tenant shall be offered as a premium option for tenants requiring stricter physical data segregation.
        *   **Caching (Redis):** Use tenant-specific key prefixes to segregate cached data.
        *   **Object Storage (S3):** Use tenant-specific folder paths within a shared bucket. Dedicated buckets per tenant shall be offered for premium tiers.
        *   **Data Privacy:** All Personally Identifiable Information (PII), such as user names and email addresses, shall be classified as sensitive data. It must be encrypted at rest and in transit, and access shall be restricted based on the principle of least privilege. The system must provide capabilities to support data subject rights under GDPR, including data access, rectification, and erasure requests.
    *   The system shall support flexible, tiered licensing models.
    *   All external-facing REST APIs shall implement URI versioning (e.g., `/api/v1/...`) to ensure backward compatibility for clients.
*   **REQ-CON-002: Domain-Specific Logic:** All business calculations, including Overall Equipment Effectiveness (OEE) and other Key Performance Indicators (KPIs), must be implemented based on explicitly documented formulas. These formulas and their software implementation must be version-controlled and subject to a formal validation process by qualified subject matter experts before deployment.
*   **REQ-CON-003: Legal and Contractual Constraints:** The system shall adhere to all licensing agreements for third-party software components. For all tenants, a Data Processing Agreement (DPA) compliant with GDPR must be in place. The system shall not store or process data in a geographic region other than that specified in the tenant's service agreement.
*   **REQ-CON-004: Industry Standards Adherence:** The Asset Hierarchy Management module (REQ-FR-021) shall be designed to be compatible with the ISA-95 standard for enterprise-control system integration.
*   **REQ-CON-005: Organizational Policies:** The system shall include a configurable, role-based approval workflow for critical system changes. This workflow shall apply to actions including, but not limited to, the deployment of new AI models to production assets, changes to alarm priority levels, and modifications to security policies. This workflow must align with standard Management of Change (MOC) procedures.

---

### **3. System Features (Functional Requirements)**

#### **3.1 Core OPC Client and Asset Management Features**
*   **REQ-FR-001: Real-Time Data Access**
    *   The system shall enable reading and writing of real-time data from OPC DA and OPC UA servers.
    *   Users shall be able to browse server namespaces and read current tag values.
    *   Authorized users shall be able to write updates to tags.
    *   The system shall ensure low-latency communication for time-critical applications.
    *   The system shall handle and visualize data quality flags (Good, Bad, Uncertain) associated with each tag value, as per the OPC specification.
*   **REQ-FR-002: Historical Data Access**
    *   The system shall retrieve historical data for analysis and reporting by implementing the OPC Historical Access (HDA) specification.
    *   The system shall support queries over specific time ranges with data aggregation functions (min, max, average) and filtering capabilities.
    *   The system shall provide trend visualization tools.
    *   The system shall provide functionality to export queried and aggregated historical data to CSV and Excel formats.
*   **REQ-FR-003: Alarms and Events Monitoring**
    *   The system shall manage alarms and events by supporting the OPC Alarms and Conditions (A&C) specification.
    *   The system shall provide a user interface for acknowledging, suppressing, and prioritizing alerts.
    *   The system shall include alarm shelving functionality, allowing operators to temporarily silence an alarm for a predefined duration with a required justification.
    *   The system shall support configurable alarm notification routing, enabling alerts to be sent via email, SMS, or configurable webhooks for integration with external systems including PagerDuty, based on alarm priority, area, type, or on-call schedules.
*   **REQ-FR-004: Security Features**
    *   The system shall ensure secure data exchange by implementing certificate-based authentication, 256-bit encryption, message signing, and user authentication per OPC UA standards.
    *   The system shall support role-based access control (RBAC).
    *   The system shall include comprehensive session management controls, such as configurable user session timeouts for inactivity and limits on concurrent sessions per user account.
*   **REQ-FR-005: Audit Trails**
    *   The system shall provide a comprehensive, tamper-evident logging of all significant user and system actions.
    *   Logged actions shall include user login/logout attempts, data write operations, alarm acknowledgements/shelving/suppression, changes to client configuration, and security policy changes.
    *   Each log entry shall be timestamped and include the responsible user and originating workstation/IP address.
    *   For data write operations, the log entry shall contain the tag name, the old value, and the new value.
    *   For actions requiring compliance with regulations like 21 CFR Part 11, the system shall support electronic signatures, linking the signature to the specific record and action.
    *   Audit logs shall be immutable, preventing modification or deletion by any user, including administrators. The system shall provide a secure mechanism for authorized personnel to export audit logs for external review and analysis.
*   **REQ-FR-006: Interoperability**
    *   The system shall connect to OPC servers from multiple vendors (e.g., Siemens, Rockwell) by supporting OPC UA, and OPC XML-DA protocols. Support for the OPC DA protocol, which is based on Microsoft COM/DCOM, shall be provided only when the OPC Core Client is deployed on a Windows operating system.
*   **REQ-FR-007: Platform Independence**
    *   The system shall operate across multiple operating systems, specifically Windows and Linux.
    *   The system shall be developed using .NET to ensure a consistent user experience across supported platforms.
*   **REQ-FR-008: Subscription Mechanism**
    *   The system shall subscribe to data changes for real-time updates using monitored items and notifications to reduce polling overhead.
    *   The system shall allow configurable update rates for efficiency.
*   **REQ-FR-009:** The GUI shall include drag-and-drop tag configuration, namespace browsing, and customizable dashboards. Dashboards and user layouts shall be user-specific and persistent. The interface shall support localization (Internationalization - i18n), with English, German, and Spanish provided as baseline languages. The architecture must allow for the addition of new languages via resource files without requiring code changes.
*   **REQ-FR-011: Redundancy and Failover**
    *   The system shall support redundant OPC servers and automatic failover.
    *   The client shall provide a configuration interface for setting up primary/backup server pairs, defining failover trigger conditions (connection loss, status code), and configuring health check parameters.
    *   An alert shall be generated and logged upon any failover event.
*   **REQ-FR-021: Asset Hierarchy Management**
    *   The system shall include a dedicated Asset Management module.
    *   This module shall allow users to build a hierarchical representation of their physical plant (an asset model), such as Site > Area > Line > Machine.
    *   Users shall be able to map OPC tags to specific assets and their properties within this hierarchy, providing essential context for data analysis, AI model assignment, and AR visualization.
    *   The module shall support the creation of asset templates to accelerate the configuration of similar equipment or production lines.

#### **3.2 Advanced AI/ML and Integration Features**
*   **REQ-FR-012: AI-Driven Predictive Maintenance**
    *   The system shall use machine learning to predict equipment failures by integrating pre-trained models to analyze historical data.
    *   Models shall be able to run on edge devices for low-latency predictions.
    *   The system shall provide a model management interface for importing custom models in ONNX format, versioning models, and assigning models to specific data tags or assets.
    *   The system shall include a workflow for retraining models with new historical data.
    *   The system shall monitor the performance of deployed AI models, tracking metrics such as prediction accuracy and data drift, and generate alerts if performance degrades below a configurable threshold.
*   **REQ-FR-013: Anomaly Detection**
    *   The system shall detect unusual patterns in real-time data using AI models and provide configurable thresholds and alerts for anomalies. To meet real-time performance requirements, models used for this purpose must be deployed and executed on the Edge AI Module.
    *   The user interface shall allow operators to provide feedback on flagged anomalies (mark as 'true anomaly' or 'false positive'). This feedback shall be logged for use in future model retraining.
*   **REQ-FR-014: Natural Language Querying and Voice Control**
    *   The system shall enable data queries and client commands using natural language, via typed text or voice input.
    *   The system shall integrate with AWS Transcribe and AWS Comprehend to process queries.
    *   The supported grammar shall cover retrieving real-time/historical data and acknowledging alarms.
    *   All natural language queries and the system's interpretation of them shall be logged for auditing and continuous improvement purposes.
*   **REQ-FR-016: Edge AI Processing**
    *   The system shall perform AI computations at the edge by running lightweight AI models on devices like NVIDIA Jetson.
    *   The centralized client management dashboard shall include functionality to deploy, start, stop, and monitor the health and performance of AI models running on remote edge devices.
*   **REQ-FR-017: Integration with IoT Platforms**
    *   The system shall connect to AWS IoT, Azure IoT, and Google Cloud IoT.
    *   The integration shall be configurable for bidirectional data flow (OPC-to-Cloud and Cloud-to-OPC).
    *   The system shall include a data mapping and transformation tool to align OPC tag structures with the target cloud platform's data schema.
*   **REQ-FR-018: Augmented Reality (AR) Dashboards**
    *   The system shall visualize data on physical equipment via AR devices (e.g., HoloLens).
    *   The system shall require a configuration tool to create and manage the mapping between OPC tags and their corresponding physical locations or asset markers (e.g., QR codes).
*   **REQ-FR-019: Blockchain for Data Integrity**
    *   To periodically verify the integrity of the tamper-evident log store for compliance and auditing, the system shall integrate with Amazon Quantum Ledger Database (QLDB). This leverages the ledger as a verification mechanism for the log's integrity rather than as a high-speed, real-time data logger.
*   **REQ-FR-020: Digital Twin Support**
    *   The system shall connect to digital twins for simulation and testing.
    *   The client shall clearly distinguish between a connection to a physical system versus a digital twin in the UI to prevent accidental writes to live equipment.
    *   The client shall support connecting to digital twins via the Asset Administration Shell (AAS) standard.

#### **3.3 Business and Management Features**
*   **REQ-BIZ-001: Multi-User Support**
    *   The system shall allow multiple users with role-based access, implementing RBAC to control access levels and support concurrent users.
    *   The system shall include five default, configurable roles: Administrator, Data Scientist, Engineer, Operator, and Viewer.
    *   The RBAC system shall allow for granular permission settings for specific plant areas or data groups.
*   **REQ-BIZ-002: Centralized Management**
    *   The system shall manage multiple client instances centrally via a dashboard for monitoring and configuring clients across sites.
    *   The central dashboard shall support remote health monitoring (CPU, memory, connection status), license management, pushing of configuration updates, remote diagnostics and log retrieval, and deploying software updates to distributed client instances.
*   **REQ-BIZ-003: Flexible Licensing Models**
    *   The system shall offer varied licensing options, supporting per-user, per-site, or subscription-based models, with tiered features based on license level.
*   **REQ-BIZ-004: Regular Updates and Maintenance**
    *   The system shall keep software current with regular updates for new features, security patches, and compatibility improvements.
    *   The client shall include a mechanism for applying updates, which can be initiated either centrally or locally. This mechanism shall support the ability to roll back to a previous version in case of a failed update.
*   **REQ-FR-015: Automated Reporting with AI**
    *   The system shall generate reports based on data trends and anomalies, using AI to create customized reports highlighting KPIs and issues.
    *   The system shall provide a report configuration module where users can define report templates, select data sources, specify AI analysis to include, and set up distribution schedules and output formats (PDF, HTML).
*   **REQ-FR-022: User Notification Preferences**
    *   The system shall provide a User Notification Preferences page within each user's profile settings.
    *   This page shall allow users to choose which types of notifications they receive (e.g., critical alarms, system updates, reports).
    *   Users shall be able to select their preferred delivery channels for each notification type (e.g., email, in-app, SMS), providing granular control to prevent notification fatigue.

#### **3.4 Data Management and Migration**
*   **REQ-DM-001: Conceptual Data Model**
    *   The system shall be based on a defined data model that includes, at a minimum, the following entities and their core relationships:
        *   **Tenant:** The top-level entity for data isolation. A Tenant has many Users, Assets, and OPC Core Clients.
        *   **User:** Belongs to a Tenant and is assigned one or more Roles.
        *   **Asset:** A hierarchical entity representing physical equipment, belonging to a Tenant. An Asset can have many associated OPC Tags and assigned AI Models.
        *   **OPC Tag:** A data point mapped from an OPC Server, associated with an Asset.
        *   **AI Model:** A versioned entity that can be assigned to an Asset for analysis.
        *   **Alarm:** An event generated by the system or an OPC server, associated with a Tag or Asset.
*   **REQ-DM-002: Data Validation**
    *   The system shall implement input validation for all user-configurable data (via UI or API) to ensure data integrity. This includes format validation (valid IP addresses for OPC servers), range checks, and consistency checks.
*   **REQ-DM-003: Data Migration**
    *   The system shall provide tools and documented procedures to support data migration from legacy systems.
    *   This shall include capabilities for bulk import/export of asset hierarchy configurations, tag lists, and user accounts via CSV and JSON formats.
    *   A strategy for migrating historical time-series data into the system's database shall be defined.

---

### **4. External Interface Requirements**

#### **4.1 User Interfaces**
*   **REQ-IFC-001:** The UI shall be clean, modern, and intuitive, minimizing clicks for common tasks.
*   The Central Management Dashboard shall be responsive and usable on screen sizes from a standard desktop monitor (1920x1080) down to a tablet (1024x768).
*   The web-based UI shall comply with Web Content Accessibility Guidelines (WCAG) 2.1 Level AA.
*   The UI shall support both a standard light theme and a dark theme.
*   The UI shall provide context-sensitive help, such as tooltips or info icons, for complex configuration options and features.
*   **REQ-FR-009:** The GUI shall include drag-and-drop tag configuration, namespace browsing, and customizable dashboards. Dashboards and user layouts shall be user-specific and persistent. The interface shall support localization (Internationalization - i18n), with English, German, and Spanish provided as baseline languages. The architecture must allow for the addition of new languages via resource files without requiring code changes.

#### **4.2 Hardware Interfaces**
*   **REQ-IFC-002:** The OPC Core Client shall run on standard x86-64 industrial PCs.
*   The Edge AI Module shall be compatible with NVIDIA Jetson series devices.
*   The system shall provide a REST API for integration with AR devices like the Microsoft HoloLens 2.

#### **4.3 Software Interfaces**
*   **REQ-IFC-003:** The system shall interface with:
    *   Third-party OPC servers via OPC DA (COM/DCOM), OPC UA (Binary TCP), and OPC XML-DA (SOAP/HTTP).
    *   Cloud platforms (AWS IoT, Azure IoT, Google Cloud IoT) via their respective APIs, primarily using MQTT.
    *   Cloud-based AI services: AWS Comprehend and AWS Transcribe.
    *   Notification services: Twilio (SMS) and SendGrid (email).
    *   Internal microservices using a defined strategy: gRPC for high-performance, low-latency internal communication; REST APIs with OpenAPI specification for external-facing services.

#### **4.4 Communication Interfaces**
*   **REQ-IFC-004:** The system shall use TCP/IP, HTTP/S, WebSockets, and MQTT network protocols.
*   Data formats shall include JSON for REST APIs, Protocol Buffers for gRPC, and OPC UA Binary Encoding.
*   All external communication shall be encrypted using TLS 1.3.
*   Communication with OPC UA servers shall use the security policies defined in the OPC UA specification.

---

### **5. Non-Functional Requirements**

#### **5.1 Performance Requirements**
*   **REQ-NFR-001:**
    *   End-to-end latency for real-time data visualization shall be less than 500ms.
    *   The 95th percentile (P95) latency for all Central Management Plane API endpoints shall be less than 200ms under nominal load.
    *   The initial load time for the main dashboard view in the Central Management Plane shall be under 3 seconds on a standard broadband connection (10 Mbps).
    *   The system shall support ingestion of up to 10,000 values per second per tenant into the time-series database.
    *   Queries for a single tag over a 24-hour period shall return in less than 1 second.
    *   Latency for AI model inference on an edge device shall be less than 100ms.
*   **REQ-FR-010:** The system shall group OPC items for efficient read/write operations, support Time-Sensitive Networking (TSN), and minimize latency for large datasets.

#### **5.2 Safety, Reliability, and Disaster Recovery**
*   **REQ-NFR-002:**
    *   **Backup Scope and Policy:** All persistent data, including configuration databases (PostgreSQL), time-series data (TimescaleDB), object storage, and secrets, shall be backed up.
    *   The cloud database shall have Point-in-Time Recovery (PITR) enabled, with daily snapshots retained for 30 days.
    *   A read replica of the primary database shall be maintained in a different availability zone.
    *   The system shall recover from an availability zone failure within 15 minutes.
    *   The system shall support automatic failover for redundant OPC servers as per REQ-FR-011 and for its own cloud-based microservices via Kubernetes.
    *   **On-Disk Buffering:** The OPC Core Client shall implement a persistent on-disk buffer to queue data during network outages. This buffer shall have a configurable size limit (1 GB or 24 hours of data, whichever is smaller). When the limit is reached, the system shall log an alert and overwrite the oldest data in the buffer (circular buffer behavior) to prioritize the most recent data. Buffered data shall be forwarded automatically upon reconnection.
    *   **Autonomous Operation:** The OPC Core Client shall be designed for autonomous operation. During a network outage, it shall continue all local data acquisition, alarm monitoring, and edge AI model execution using buffered data.
    *   **Disaster Recovery:** A formal Disaster Recovery (DR) plan shall be defined and tested for region-level failures. This plan must specify a Recovery Time Objective (RTO) of less than 4 hours and a Recovery Point Objective (RPO) of less than 1 hour, utilizing cross-region database replicas and object storage replication.
    *   **Recovery Testing:** The disaster recovery and data restoration procedures shall be tested automatically on a quarterly basis to ensure their validity and to verify RTO/RPO targets.

#### **5.3 Security Requirements**
*   **REQ-NFR-003:**
    *   User authentication shall be managed by a centralized Identity Provider (Keycloak) supporting OAuth 2.0 and OIDC.
    *   All API access shall be secured using JWT Bearer Tokens, with validation performed at an API Gateway layer.
    *   The system shall implement a Role-Based Access Control (RBAC) model, with permissions checked at the API Gateway and enforced within each microservice.
    *   All network communication shall be encrypted using TLS 1.3.
    *   All data at rest (databases, object storage) shall be encrypted using AES-256.
    *   All sensitive information (passwords, API keys, certificates) shall be stored in AWS Secrets Manager.
    *   The system shall maintain a tamper-evident audit trail as detailed in REQ-FR-005.
    *   **Secure Client Provisioning:** A secure workflow shall be implemented for bootstrapping new OPC Core Client instances. This process shall involve generating a one-time registration token in the Central Management Plane, which the client uses on its first connection to authenticate and receive a long-term client certificate via an X.509 certificate signing request for all subsequent communication.
    *   The system shall undergo annual third-party security audits and penetration testing, with a commitment to remediate identified critical and high-severity vulnerabilities within 30 and 90 days, respectively.

#### **5.4 System Attributes**
*   **REQ-NFR-004: Availability**
    *   The Central Management Plane shall have a minimum uptime of 99.9%, excluding planned maintenance.
    *   Planned maintenance windows shall be scheduled with 14 days advance notice to customers and shall not exceed 4 hours per month.
*   **REQ-NFR-005: Scalability**
    *   The system shall support up to 1,000 concurrent users per tenant.
    *   All cloud microservices shall be stateless and designed to scale horizontally using Kubernetes Horizontal Pod Autoscalers.
    *   The Central Management Plane shall be capable of managing up to 10,000 distributed OPC Core Client instances.
*   **REQ-NFR-006: Maintainability**
    *   All backend code shall maintain a minimum of 80% unit test coverage.
    *   All API endpoints shall be documented using the OpenAPI specification.
    *   The system shall be built on a microservices architecture to ensure loose coupling and independent deployability, fronted by the Kong API Gateway to manage routing, authentication, and rate limiting.
*   **REQ-NFR-008: Testability**
    *   The system shall include a dedicated staging environment that is a functional replica of the production environment for user acceptance testing (UAT), integration testing, and performance testing.
    *   Automated performance and load tests shall be executed as part of the CI/CD pipeline to validate performance requirements (REQ-NFR-001) before deployment to production.

#### **5.5 Data Management and Retention**
*   **REQ-NFR-007: Data Retention Policies**
    *   The system shall implement configurable data retention policies for different data types to meet varying compliance and cost requirements.
    *   These policies shall be configurable on a per-tenant basis.
    *   Default policies shall be: retain raw time-series data for 1 year, aggregated hourly data for 5 years, and audit logs for 7 years.
    *   The system shall leverage automated data lifecycle features of TimescaleDB to enforce these policies.

---

### **6. Other Requirements**

#### **6.1 Technology Stack**
*   **REQ-ARC-002:** The system shall be built using the following technology stack:
    *   **Frontend:** React 18 with TypeScript, Redux Toolkit for state management, Material-UI component library, Vite for build tooling.
    *   **Backend:** .NET 8 / ASP.NET Core for Core Client and Cloud Services.
    *   **APIs:** REST for external communication, gRPC for internal microservice communication.
    *   **Real-time Communication:** SignalR for web, OPC UA Binary TCP for server communication.
    *   **Databases:** PostgreSQL 16 for relational/configuration data, TimescaleDB for time-series data. Database schema changes shall be managed via EF Core Migrations.
    *   **Caching & Storage:** Redis 7 for caching, Amazon S3 for object storage.
    *   **Cloud & Orchestration:** AWS as the cloud provider, Docker for containerization, Kubernetes for orchestration (Amazon EKS for cloud, K3s for edge).
    *   **Testing:**
        *   Backend: xUnit, Moq, FluentAssertions.
        *   Frontend: Vitest, React Testing Library.
        *   End-to-End: Playwright.
    *   **DevOps:** GitHub Actions for CI/CD, Terraform for Infrastructure as Code.
    *   **Identity:** Keycloak as the Identity Provider.

#### **6.2 Monitoring, Logging, and Reporting**
*   **REQ-MON-001:**
    *   The system shall provide a flexible reporting module as defined in REQ-FR-015, with schedulable reports in PDF and HTML formats distributed via email.
    *   Prometheus shall be used to scrape metrics from all system components, including a `/metrics` endpoint exposed by the OPC Core Client.
    *   A standard set of Key Performance Indicators (KPIs) shall be defined and monitored, including API error rates, request latency, message queue depth, database connection pool usage, and client connectivity status.
    *   Logs from all containers and services shall be aggregated using Fluentd and stored in a centralized OpenSearch cluster. Logs shall be structured in JSON format and include timestamp, severity, service name, and correlation ID.
    *   OpenTelemetry shall be integrated into all microservices to enable distributed tracing.
    *   Grafana shall be used for visualizing system metrics and logs.
    *   Alertmanager shall be configured to send critical alerts to on-call personnel via PagerDuty and Slack.
    *   All microservices and the OPC Core Client shall expose standardized health check endpoints (`/healthz` for liveness, `/readyz` for readiness) to integrate with Kubernetes orchestration.

#### **6.3 Documentation**
*   **REQ-DOC-001: User and Administrator Documentation**
    *   A comprehensive User Manual shall be provided, detailing all system features from an end-user perspective (Operator, Engineer).
    *   A System Administration Guide shall be provided, covering system setup, configuration, user management, security configuration, and routine maintenance tasks.
*   **REQ-DOC-002: Technical Documentation**
    *   API documentation shall be automatically generated from the OpenAPI specification and made available to developers.
    *   A Deployment Guide shall be created, detailing the steps to deploy and configure the OPC Core Client and the prerequisites for the Central Management Plane.
    *   A high-level architecture document shall be maintained, describing the major components and their interactions.

#### **6.4 System Administration and Support**
*   **REQ-OPS-001: Administrative CLI**
    *   A secure Command Line Interface (CLI) shall be provided for advanced administrative and troubleshooting tasks, such as bulk user management, system diagnostics, and running maintenance scripts.
*   **REQ-OPS-002: Support and Maintenance Process**
    *   A formal incident management process shall be defined, including severity level definitions (Sev1, Sev2, Sev3), escalation paths, and target response times.
    *   The system shall provide a mechanism for administrators to securely package and export diagnostic information (logs, configuration files) to assist with support requests.

---

### **7. Transition Requirements**

#### **7.1 Implementation Approach**
*   **REQ-TRN-001:** The system shall be deployed using a phased rollout methodology.
    *   **Phase 1 (Pilot):** Deployment to a single, pre-selected site to validate functionality, performance, and operational readiness in a production environment.
    *   **Phase 2 (Regional Rollout):** A site-by-site deployment across a single region, incorporating lessons learned from the pilot phase.
    *   **Phase 3 (Full Deployment):** A global rollout to all remaining sites.
    *   Each phase shall have specific entry and exit criteria that must be met before proceeding to the next.

#### **7.2 Data Migration Strategy**
*   **REQ-TRN-002:** A comprehensive data migration plan shall be executed for each site.
    *   **Extraction:** Documented procedures and scripts shall be provided to extract configuration data (asset hierarchies, tag lists) and historical time-series data from specified legacy systems (e.g., OSIsoft PI, Wonderware Historian).
    *   **Transformation:** Data shall be transformed into the system's required CSV or JSON formats. This includes mapping legacy asset structures to the new ISA-95 compatible model.
    *   **Loading:** The system's bulk import tools (as per REQ-DM-003) shall be used to load the transformed data.
    *   **Validation:** Post-migration validation checks are mandatory. These shall include record count verification, spot-checking of critical data points, and validation of asset-tag relationships. A migration summary report must be generated and signed off by the site engineer.

#### **7.3 Training Requirements**
*   **REQ-TRN-003:** Role-based training shall be mandatory for all users prior to gaining system access.
    *   **Training Modules:** Specific training curricula shall be developed for each user role (Administrator, Engineer, Operator, Data Scientist).
    *   **Training Materials:** A full set of training materials shall be provided, including user manuals, quick-reference guides, and pre-recorded video tutorials.
    *   **Delivery Method:** Training shall be delivered via a combination of self-paced online modules and live, instructor-led virtual sessions for Q&A and hands-on exercises.

#### **7.4 System Cutover Plan**
*   **REQ-TRN-004:** A detailed cutover plan shall be created and approved for each site deployment.
    *   **Pre-Cutover Checklist:** A checklist of all prerequisite tasks (data migration, user training, network configuration) must be completed.
    *   **Go/No-Go Criteria:** A formal go/no-go decision meeting shall be held prior to the cutover, based on the completion of the checklist and the results of User Acceptance Testing (UAT).
    *   **Cutover Window:** The cutover shall be scheduled during a low-impact production period.
    *   **Fallback Plan:** A documented fallback procedure must be in place to revert to the legacy system within a 2-hour window if the new system fails to meet critical success criteria post-launch.
    *   **Post-Cutover Support:** A period of hyper-care support (minimum 72 hours) with dedicated technical staff shall be provided immediately following the cutover.

#### **7.5 Legacy System Integration and Decommissioning**
*   **REQ-TRN-005:** A plan for managing and decommissioning legacy systems shall be defined.
    *   **Parallel Operation:** For a defined period (minimum 30 days) post-cutover, the legacy system shall run in parallel in a read-only mode to allow for data validation and user adjustment.
    *   **Data Synchronization:** During the parallel operation period, no data synchronization from the new system back to the legacy system will be supported. The new system is the single source of truth from the moment of cutover.
    *   **Decommissioning:** After the successful completion of the parallel run period, the legacy system shall be formally decommissioned. This includes archiving all historical data according to data retention policies and shutting down system hardware.