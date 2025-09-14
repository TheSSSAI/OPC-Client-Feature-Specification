# 1 Title

Central Management Plane Relational Database

# 2 Name

cmp_main_db

# 3 Db Type

- relational
- timeseries

# 4 Db Technology

PostgreSQL 16 with TimescaleDB Extension

# 5 Entities

## 5.1 Tenant

### 5.1.1 Name

Tenant

### 5.1.2 Description

Represents a customer organization, providing top-level data isolation. (REQ-1-024)

### 5.1.3 Attributes

#### 5.1.3.1 Guid

##### 5.1.3.1.1 Name

tenantId

##### 5.1.3.1.2 Type

🔹 Guid

##### 5.1.3.1.3 Is Required

✅ Yes

##### 5.1.3.1.4 Is Primary Key

✅ Yes

##### 5.1.3.1.5 Is Unique

✅ Yes

##### 5.1.3.1.6 Index Type

UniqueIndex

#### 5.1.3.2.0 VARCHAR

##### 5.1.3.2.1 Name

name

##### 5.1.3.2.2 Type

🔹 VARCHAR

##### 5.1.3.2.3 Is Required

✅ Yes

##### 5.1.3.2.4 Is Primary Key

❌ No

##### 5.1.3.2.5 Is Unique

✅ Yes

##### 5.1.3.2.6 Index Type

UniqueIndex

##### 5.1.3.2.7 Size

255

#### 5.1.3.3.0 VARCHAR

##### 5.1.3.3.1 Name

dataResidencyRegion

##### 5.1.3.3.2 Type

🔹 VARCHAR

##### 5.1.3.3.3 Is Required

✅ Yes

##### 5.1.3.3.4 Is Primary Key

❌ No

##### 5.1.3.3.5 Is Unique

❌ No

##### 5.1.3.3.6 Index Type

Index

##### 5.1.3.3.7 Size

50

#### 5.1.3.4.0 VARCHAR

##### 5.1.3.4.1 Name

isolationModel

##### 5.1.3.4.2 Type

🔹 VARCHAR

##### 5.1.3.4.3 Is Required

✅ Yes

##### 5.1.3.4.4 Is Primary Key

❌ No

##### 5.1.3.4.5 Is Unique

❌ No

##### 5.1.3.4.6 Index Type

Index

##### 5.1.3.4.7 Size

50

##### 5.1.3.4.8 Constraints

- ENUM('RLS', 'SCHEMA')

##### 5.1.3.4.9 Default Value

RLS

#### 5.1.3.5.0 BOOLEAN

##### 5.1.3.5.1 Name

isActive

##### 5.1.3.5.2 Type

🔹 BOOLEAN

##### 5.1.3.5.3 Is Required

✅ Yes

##### 5.1.3.5.4 Is Primary Key

❌ No

##### 5.1.3.5.5 Is Unique

❌ No

##### 5.1.3.5.6 Index Type

Index

##### 5.1.3.5.7 Default Value

true

#### 5.1.3.6.0 DateTimeOffset

##### 5.1.3.6.1 Name

createdAt

##### 5.1.3.6.2 Type

🔹 DateTimeOffset

##### 5.1.3.6.3 Is Required

✅ Yes

##### 5.1.3.6.4 Is Primary Key

❌ No

##### 5.1.3.6.5 Is Unique

❌ No

##### 5.1.3.6.6 Index Type

Index

##### 5.1.3.6.7 Default Value

CURRENT_TIMESTAMP

#### 5.1.3.7.0 DateTimeOffset

##### 5.1.3.7.1 Name

updatedAt

##### 5.1.3.7.2 Type

🔹 DateTimeOffset

##### 5.1.3.7.3 Is Required

✅ Yes

##### 5.1.3.7.4 Is Primary Key

❌ No

##### 5.1.3.7.5 Is Unique

❌ No

##### 5.1.3.7.6 Index Type

None

##### 5.1.3.7.7 Default Value

CURRENT_TIMESTAMP

### 5.1.4.0.0 Primary Keys

- tenantId

### 5.1.5.0.0 Unique Constraints

- {'name': 'UC_Tenant_Name', 'columns': ['name']}

### 5.1.6.0.0 Indexes

#### 5.1.6.1.0 BTree

##### 5.1.6.1.1 Name

IX_Tenant_IsActive

##### 5.1.6.1.2 Columns

- isActive

##### 5.1.6.1.3 Type

🔹 BTree

#### 5.1.6.2.0 BTree

##### 5.1.6.2.1 Name

IX_Tenant_CreatedAt

##### 5.1.6.2.2 Columns

- createdAt

##### 5.1.6.2.3 Type

🔹 BTree

## 5.2.0.0.0 User

### 5.2.1.0.0 Name

User

### 5.2.2.0.0 Description

Represents a system user with profile information and credentials. (REQ-1-011)

### 5.2.3.0.0 Attributes

#### 5.2.3.1.0 Guid

##### 5.2.3.1.1 Name

userId

##### 5.2.3.1.2 Type

🔹 Guid

##### 5.2.3.1.3 Is Required

✅ Yes

##### 5.2.3.1.4 Is Primary Key

✅ Yes

##### 5.2.3.1.5 Is Unique

✅ Yes

##### 5.2.3.1.6 Index Type

UniqueIndex

#### 5.2.3.2.0 Guid

##### 5.2.3.2.1 Name

tenantId

##### 5.2.3.2.2 Type

🔹 Guid

##### 5.2.3.2.3 Is Required

✅ Yes

##### 5.2.3.2.4 Is Primary Key

❌ No

##### 5.2.3.2.5 Is Unique

❌ No

##### 5.2.3.2.6 Index Type

Index

##### 5.2.3.2.7 Is Foreign Key

✅ Yes

#### 5.2.3.3.0 VARCHAR

##### 5.2.3.3.1 Name

email

##### 5.2.3.3.2 Type

🔹 VARCHAR

##### 5.2.3.3.3 Is Required

✅ Yes

##### 5.2.3.3.4 Is Primary Key

❌ No

##### 5.2.3.3.5 Is Unique

✅ Yes

##### 5.2.3.3.6 Index Type

UniqueIndex

##### 5.2.3.3.7 Size

255

##### 5.2.3.3.8 Constraints

- EMAIL_FORMAT

#### 5.2.3.4.0 VARCHAR

##### 5.2.3.4.1 Name

firstName

##### 5.2.3.4.2 Type

🔹 VARCHAR

##### 5.2.3.4.3 Is Required

✅ Yes

##### 5.2.3.4.4 Is Primary Key

❌ No

##### 5.2.3.4.5 Is Unique

❌ No

##### 5.2.3.4.6 Index Type

Index

##### 5.2.3.4.7 Size

100

#### 5.2.3.5.0 VARCHAR

##### 5.2.3.5.1 Name

lastName

##### 5.2.3.5.2 Type

🔹 VARCHAR

##### 5.2.3.5.3 Is Required

✅ Yes

##### 5.2.3.5.4 Is Primary Key

❌ No

##### 5.2.3.5.5 Is Unique

❌ No

##### 5.2.3.5.6 Index Type

Index

##### 5.2.3.5.7 Size

100

#### 5.2.3.6.0 VARCHAR

##### 5.2.3.6.1 Name

identityProviderId

##### 5.2.3.6.2 Type

🔹 VARCHAR

##### 5.2.3.6.3 Is Required

✅ Yes

##### 5.2.3.6.4 Is Primary Key

❌ No

##### 5.2.3.6.5 Is Unique

✅ Yes

##### 5.2.3.6.6 Index Type

UniqueIndex

##### 5.2.3.6.7 Size

255

#### 5.2.3.7.0 JSONB

##### 5.2.3.7.1 Name

notificationPreferences

##### 5.2.3.7.2 Type

🔹 JSONB

##### 5.2.3.7.3 Is Required

❌ No

##### 5.2.3.7.4 Is Primary Key

❌ No

##### 5.2.3.7.5 Is Unique

❌ No

##### 5.2.3.7.6 Index Type

None

##### 5.2.3.7.7 Default Value

{}

##### 5.2.3.7.8 Comment

Stores user-defined notification preferences per channel and type (REQ-FR-022).

#### 5.2.3.8.0 BOOLEAN

##### 5.2.3.8.1 Name

isActive

##### 5.2.3.8.2 Type

🔹 BOOLEAN

##### 5.2.3.8.3 Is Required

✅ Yes

##### 5.2.3.8.4 Is Primary Key

❌ No

##### 5.2.3.8.5 Is Unique

❌ No

##### 5.2.3.8.6 Index Type

Index

##### 5.2.3.8.7 Default Value

true

#### 5.2.3.9.0 BOOLEAN

##### 5.2.3.9.1 Name

isDeleted

##### 5.2.3.9.2 Type

🔹 BOOLEAN

##### 5.2.3.9.3 Is Required

✅ Yes

##### 5.2.3.9.4 Is Primary Key

❌ No

##### 5.2.3.9.5 Is Unique

❌ No

##### 5.2.3.9.6 Index Type

Index

##### 5.2.3.9.7 Default Value

false

##### 5.2.3.9.8 Comment

Supports soft-deleting users instead of permanent removal.

#### 5.2.3.10.0 DateTimeOffset

##### 5.2.3.10.1 Name

createdAt

##### 5.2.3.10.2 Type

🔹 DateTimeOffset

##### 5.2.3.10.3 Is Required

✅ Yes

##### 5.2.3.10.4 Is Primary Key

❌ No

##### 5.2.3.10.5 Is Unique

❌ No

##### 5.2.3.10.6 Index Type

Index

##### 5.2.3.10.7 Default Value

CURRENT_TIMESTAMP

#### 5.2.3.11.0 DateTimeOffset

##### 5.2.3.11.1 Name

updatedAt

##### 5.2.3.11.2 Type

🔹 DateTimeOffset

##### 5.2.3.11.3 Is Required

✅ Yes

##### 5.2.3.11.4 Is Primary Key

❌ No

##### 5.2.3.11.5 Is Unique

❌ No

##### 5.2.3.11.6 Index Type

None

##### 5.2.3.11.7 Default Value

CURRENT_TIMESTAMP

### 5.2.4.0.0 Primary Keys

- userId

### 5.2.5.0.0 Unique Constraints

#### 5.2.5.1.0 UC_User_Email

##### 5.2.5.1.1 Name

UC_User_Email

##### 5.2.5.1.2 Columns

- email

#### 5.2.5.2.0 UC_User_IdentityProviderId

##### 5.2.5.2.1 Name

UC_User_IdentityProviderId

##### 5.2.5.2.2 Columns

- identityProviderId

### 5.2.6.0.0 Indexes

#### 5.2.6.1.0 BTree

##### 5.2.6.1.1 Name

IX_User_TenantId

##### 5.2.6.1.2 Columns

- tenantId

##### 5.2.6.1.3 Type

🔹 BTree

#### 5.2.6.2.0 BTree

##### 5.2.6.2.1 Name

IX_User_Tenant_Active_Name

##### 5.2.6.2.2 Columns

- tenantId
- isActive
- isDeleted
- lastName
- firstName

##### 5.2.6.2.3 Type

🔹 BTree

## 5.3.0.0.0 Role

### 5.3.1.0.0 Name

Role

### 5.3.2.0.0 Description

Defines a set of permissions for users (e.g., Administrator, Operator). (REQ-1-011)

### 5.3.3.0.0 Attributes

#### 5.3.3.1.0 Guid

##### 5.3.3.1.1 Name

roleId

##### 5.3.3.1.2 Type

🔹 Guid

##### 5.3.3.1.3 Is Required

✅ Yes

##### 5.3.3.1.4 Is Primary Key

✅ Yes

##### 5.3.3.1.5 Is Unique

✅ Yes

##### 5.3.3.1.6 Index Type

UniqueIndex

#### 5.3.3.2.0 VARCHAR

##### 5.3.3.2.1 Name

name

##### 5.3.3.2.2 Type

🔹 VARCHAR

##### 5.3.3.2.3 Is Required

✅ Yes

##### 5.3.3.2.4 Is Primary Key

❌ No

##### 5.3.3.2.5 Is Unique

✅ Yes

##### 5.3.3.2.6 Index Type

UniqueIndex

##### 5.3.3.2.7 Size

50

#### 5.3.3.3.0 TEXT

##### 5.3.3.3.1 Name

description

##### 5.3.3.3.2 Type

🔹 TEXT

##### 5.3.3.3.3 Is Required

❌ No

##### 5.3.3.3.4 Is Primary Key

❌ No

##### 5.3.3.3.5 Is Unique

❌ No

##### 5.3.3.3.6 Index Type

None

#### 5.3.3.4.0 BOOLEAN

##### 5.3.3.4.1 Name

isSystemRole

##### 5.3.3.4.2 Type

🔹 BOOLEAN

##### 5.3.3.4.3 Is Required

✅ Yes

##### 5.3.3.4.4 Is Primary Key

❌ No

##### 5.3.3.4.5 Is Unique

❌ No

##### 5.3.3.4.6 Index Type

Index

##### 5.3.3.4.7 Default Value

false

### 5.3.4.0.0 Primary Keys

- roleId

### 5.3.5.0.0 Unique Constraints

- {'name': 'UC_Role_Name', 'columns': ['name']}

### 5.3.6.0.0 Indexes

- {'name': 'IX_Role_IsSystemRole', 'columns': ['isSystemRole'], 'type': 'BTree'}

## 5.4.0.0.0 UserRole

### 5.4.1.0.0 Name

UserRole

### 5.4.2.0.0 Description

Junction table to assign Roles to Users, enabling many-to-many relationships and asset-level scope. (REQ-1-061, REQ-BIZ-001)

### 5.4.3.0.0 Attributes

#### 5.4.3.1.0 Guid

##### 5.4.3.1.1 Name

userId

##### 5.4.3.1.2 Type

🔹 Guid

##### 5.4.3.1.3 Is Required

✅ Yes

##### 5.4.3.1.4 Is Primary Key

✅ Yes

##### 5.4.3.1.5 Is Unique

❌ No

##### 5.4.3.1.6 Index Type

Index

##### 5.4.3.1.7 Is Foreign Key

✅ Yes

#### 5.4.3.2.0 Guid

##### 5.4.3.2.1 Name

roleId

##### 5.4.3.2.2 Type

🔹 Guid

##### 5.4.3.2.3 Is Required

✅ Yes

##### 5.4.3.2.4 Is Primary Key

✅ Yes

##### 5.4.3.2.5 Is Unique

❌ No

##### 5.4.3.2.6 Index Type

Index

##### 5.4.3.2.7 Is Foreign Key

✅ Yes

#### 5.4.3.3.0 Guid

##### 5.4.3.3.1 Name

assetScopeId

##### 5.4.3.3.2 Type

🔹 Guid

##### 5.4.3.3.3 Is Required

❌ No

##### 5.4.3.3.4 Is Primary Key

❌ No

##### 5.4.3.3.5 Is Unique

❌ No

##### 5.4.3.3.6 Index Type

Index

##### 5.4.3.3.7 Is Foreign Key

✅ Yes

##### 5.4.3.3.8 Comment

If NULL, role applies tenant-wide. If populated, role is restricted to this asset and its children.

#### 5.4.3.4.0 DateTimeOffset

##### 5.4.3.4.1 Name

assignedAt

##### 5.4.3.4.2 Type

🔹 DateTimeOffset

##### 5.4.3.4.3 Is Required

✅ Yes

##### 5.4.3.4.4 Is Primary Key

❌ No

##### 5.4.3.4.5 Is Unique

❌ No

##### 5.4.3.4.6 Index Type

None

##### 5.4.3.4.7 Default Value

CURRENT_TIMESTAMP

### 5.4.4.0.0 Primary Keys

- userId
- roleId

### 5.4.5.0.0 Unique Constraints

*No items available*

### 5.4.6.0.0 Indexes

#### 5.4.6.1.0 BTree

##### 5.4.6.1.1 Name

IX_UserRole_RoleId

##### 5.4.6.1.2 Columns

- roleId

##### 5.4.6.1.3 Type

🔹 BTree

#### 5.4.6.2.0 BTree

##### 5.4.6.2.1 Name

IX_UserRole_AssetScopeId

##### 5.4.6.2.2 Columns

- assetScopeId

##### 5.4.6.2.3 Type

🔹 BTree

### 5.4.7.0.0 Caching

| Property | Value |
|----------|-------|
| Strategy | Redis |
| Key | user_permissions:{userId} |
| Invalidation | On user-permission changes. |
| Comment | Caches resolved user permissions for the duration ... |

## 5.5.0.0.0 AuditLog

### 5.5.1.0.0 Name

AuditLog

### 5.5.2.0.0 Description

Stores a record of all significant user and system actions. A digest is anchored to QLDB for tamper evidence. (REQ-1-040, REQ-FR-019)

### 5.5.3.0.0 Attributes

#### 5.5.3.1.0 BIGINT

##### 5.5.3.1.1 Name

auditLogId

##### 5.5.3.1.2 Type

🔹 BIGINT

##### 5.5.3.1.3 Is Required

✅ Yes

##### 5.5.3.1.4 Is Primary Key

✅ Yes

##### 5.5.3.1.5 Is Unique

✅ Yes

##### 5.5.3.1.6 Index Type

UniqueIndex

##### 5.5.3.1.7 Constraints

- AUTO_INCREMENT

#### 5.5.3.2.0 Guid

##### 5.5.3.2.1 Name

tenantId

##### 5.5.3.2.2 Type

🔹 Guid

##### 5.5.3.2.3 Is Required

✅ Yes

##### 5.5.3.2.4 Is Primary Key

❌ No

##### 5.5.3.2.5 Is Unique

❌ No

##### 5.5.3.2.6 Index Type

Index

##### 5.5.3.2.7 Is Foreign Key

✅ Yes

#### 5.5.3.3.0 Guid

##### 5.5.3.3.1 Name

userId

##### 5.5.3.3.2 Type

🔹 Guid

##### 5.5.3.3.3 Is Required

❌ No

##### 5.5.3.3.4 Is Primary Key

❌ No

##### 5.5.3.3.5 Is Unique

❌ No

##### 5.5.3.3.6 Index Type

Index

##### 5.5.3.3.7 Is Foreign Key

✅ Yes

##### 5.5.3.3.8 Comment

NULL for system-initiated actions.

#### 5.5.3.4.0 DateTimeOffset

##### 5.5.3.4.1 Name

timestamp

##### 5.5.3.4.2 Type

🔹 DateTimeOffset

##### 5.5.3.4.3 Is Required

✅ Yes

##### 5.5.3.4.4 Is Primary Key

❌ No

##### 5.5.3.4.5 Is Unique

❌ No

##### 5.5.3.4.6 Index Type

Index

##### 5.5.3.4.7 Default Value

CURRENT_TIMESTAMP

#### 5.5.3.5.0 VARCHAR

##### 5.5.3.5.1 Name

actionType

##### 5.5.3.5.2 Type

🔹 VARCHAR

##### 5.5.3.5.3 Is Required

✅ Yes

##### 5.5.3.5.4 Is Primary Key

❌ No

##### 5.5.3.5.5 Is Unique

❌ No

##### 5.5.3.5.6 Index Type

Index

##### 5.5.3.5.7 Size

100

#### 5.5.3.6.0 VARCHAR

##### 5.5.3.6.1 Name

entityName

##### 5.5.3.6.2 Type

🔹 VARCHAR

##### 5.5.3.6.3 Is Required

❌ No

##### 5.5.3.6.4 Is Primary Key

❌ No

##### 5.5.3.6.5 Is Unique

❌ No

##### 5.5.3.6.6 Index Type

Index

##### 5.5.3.6.7 Size

100

#### 5.5.3.7.0 VARCHAR

##### 5.5.3.7.1 Name

entityId

##### 5.5.3.7.2 Type

🔹 VARCHAR

##### 5.5.3.7.3 Is Required

❌ No

##### 5.5.3.7.4 Is Primary Key

❌ No

##### 5.5.3.7.5 Is Unique

❌ No

##### 5.5.3.7.6 Index Type

Index

##### 5.5.3.7.7 Size

255

#### 5.5.3.8.0 JSONB

##### 5.5.3.8.1 Name

details

##### 5.5.3.8.2 Type

🔹 JSONB

##### 5.5.3.8.3 Is Required

❌ No

##### 5.5.3.8.4 Is Primary Key

❌ No

##### 5.5.3.8.5 Is Unique

❌ No

##### 5.5.3.8.6 Index Type

None

##### 5.5.3.8.7 Default Value

{}

##### 5.5.3.8.8 Comment

Includes details like old/new values for data changes.

#### 5.5.3.9.0 VARCHAR

##### 5.5.3.9.1 Name

sourceIpAddress

##### 5.5.3.9.2 Type

🔹 VARCHAR

##### 5.5.3.9.3 Is Required

❌ No

##### 5.5.3.9.4 Is Primary Key

❌ No

##### 5.5.3.9.5 Is Unique

❌ No

##### 5.5.3.9.6 Index Type

None

##### 5.5.3.9.7 Size

45

#### 5.5.3.10.0 VARCHAR

##### 5.5.3.10.1 Name

qldbDigest

##### 5.5.3.10.2 Type

🔹 VARCHAR

##### 5.5.3.10.3 Is Required

✅ Yes

##### 5.5.3.10.4 Is Primary Key

❌ No

##### 5.5.3.10.5 Is Unique

✅ Yes

##### 5.5.3.10.6 Index Type

UniqueIndex

##### 5.5.3.10.7 Size

255

##### 5.5.3.10.8 Comment

The digest of the corresponding document in Amazon QLDB for integrity verification.

### 5.5.4.0.0 Primary Keys

- auditLogId

### 5.5.5.0.0 Unique Constraints

- {'name': 'UC_AuditLog_QldbDigest', 'columns': ['qldbDigest']}

### 5.5.6.0.0 Indexes

#### 5.5.6.1.0 BTree

##### 5.5.6.1.1 Name

IX_AuditLog_Tenant_Timestamp

##### 5.5.6.1.2 Columns

- tenantId
- timestamp

##### 5.5.6.1.3 Type

🔹 BTree

#### 5.5.6.2.0 BTree

##### 5.5.6.2.1 Name

IX_AuditLog_User_Timestamp

##### 5.5.6.2.2 Columns

- userId
- timestamp

##### 5.5.6.2.3 Type

🔹 BTree

#### 5.5.6.3.0 BTree

##### 5.5.6.3.1 Name

IX_AuditLog_Entity

##### 5.5.6.3.2 Columns

- entityName
- entityId

##### 5.5.6.3.3 Type

🔹 BTree

#### 5.5.6.4.0 GIN

##### 5.5.6.4.1 Name

IX_AuditLog_Details_GIN

##### 5.5.6.4.2 Columns

- details

##### 5.5.6.4.3 Type

🔹 GIN

### 5.5.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Range |
| Column | timestamp |
| Strategy | Monthly |
| Comment | Improves queries by date and simplifies data reten... |

## 5.6.0.0.0 OpcCoreClient

### 5.6.1.0.0 Name

OpcCoreClient

### 5.6.2.0.0 Description

Represents a deployed on-premise or edge client instance. (REQ-1-001, REQ-1-062)

### 5.6.3.0.0 Attributes

#### 5.6.3.1.0 Guid

##### 5.6.3.1.1 Name

opcCoreClientId

##### 5.6.3.1.2 Type

🔹 Guid

##### 5.6.3.1.3 Is Required

✅ Yes

##### 5.6.3.1.4 Is Primary Key

✅ Yes

##### 5.6.3.1.5 Is Unique

✅ Yes

##### 5.6.3.1.6 Index Type

UniqueIndex

#### 5.6.3.2.0 Guid

##### 5.6.3.2.1 Name

tenantId

##### 5.6.3.2.2 Type

🔹 Guid

##### 5.6.3.2.3 Is Required

✅ Yes

##### 5.6.3.2.4 Is Primary Key

❌ No

##### 5.6.3.2.5 Is Unique

❌ No

##### 5.6.3.2.6 Index Type

Index

##### 5.6.3.2.7 Is Foreign Key

✅ Yes

#### 5.6.3.3.0 VARCHAR

##### 5.6.3.3.1 Name

name

##### 5.6.3.3.2 Type

🔹 VARCHAR

##### 5.6.3.3.3 Is Required

✅ Yes

##### 5.6.3.3.4 Is Primary Key

❌ No

##### 5.6.3.3.5 Is Unique

❌ No

##### 5.6.3.3.6 Index Type

Index

##### 5.6.3.3.7 Size

255

#### 5.6.3.4.0 VARCHAR

##### 5.6.3.4.1 Name

status

##### 5.6.3.4.2 Type

🔹 VARCHAR

##### 5.6.3.4.3 Is Required

✅ Yes

##### 5.6.3.4.4 Is Primary Key

❌ No

##### 5.6.3.4.5 Is Unique

❌ No

##### 5.6.3.4.6 Index Type

Index

##### 5.6.3.4.7 Size

50

##### 5.6.3.4.8 Constraints

- ENUM('Online', 'Offline', 'Degraded')

##### 5.6.3.4.9 Default Value

Offline

#### 5.6.3.5.0 VARCHAR

##### 5.6.3.5.1 Name

softwareVersion

##### 5.6.3.5.2 Type

🔹 VARCHAR

##### 5.6.3.5.3 Is Required

❌ No

##### 5.6.3.5.4 Is Primary Key

❌ No

##### 5.6.3.5.5 Is Unique

❌ No

##### 5.6.3.5.6 Index Type

None

##### 5.6.3.5.7 Size

50

#### 5.6.3.6.0 DateTimeOffset

##### 5.6.3.6.1 Name

lastHeartbeat

##### 5.6.3.6.2 Type

🔹 DateTimeOffset

##### 5.6.3.6.3 Is Required

❌ No

##### 5.6.3.6.4 Is Primary Key

❌ No

##### 5.6.3.6.5 Is Unique

❌ No

##### 5.6.3.6.6 Index Type

Index

#### 5.6.3.7.0 BOOLEAN

##### 5.6.3.7.1 Name

isDeleted

##### 5.6.3.7.2 Type

🔹 BOOLEAN

##### 5.6.3.7.3 Is Required

✅ Yes

##### 5.6.3.7.4 Is Primary Key

❌ No

##### 5.6.3.7.5 Is Unique

❌ No

##### 5.6.3.7.6 Index Type

Index

##### 5.6.3.7.7 Default Value

false

#### 5.6.3.8.0 DateTimeOffset

##### 5.6.3.8.1 Name

createdAt

##### 5.6.3.8.2 Type

🔹 DateTimeOffset

##### 5.6.3.8.3 Is Required

✅ Yes

##### 5.6.3.8.4 Is Primary Key

❌ No

##### 5.6.3.8.5 Is Unique

❌ No

##### 5.6.3.8.6 Index Type

Index

##### 5.6.3.8.7 Default Value

CURRENT_TIMESTAMP

#### 5.6.3.9.0 DateTimeOffset

##### 5.6.3.9.1 Name

updatedAt

##### 5.6.3.9.2 Type

🔹 DateTimeOffset

##### 5.6.3.9.3 Is Required

✅ Yes

##### 5.6.3.9.4 Is Primary Key

❌ No

##### 5.6.3.9.5 Is Unique

❌ No

##### 5.6.3.9.6 Index Type

None

##### 5.6.3.9.7 Default Value

CURRENT_TIMESTAMP

### 5.6.4.0.0 Primary Keys

- opcCoreClientId

### 5.6.5.0.0 Unique Constraints

- {'name': 'UC_OpcCoreClient_Tenant_Name_NotDeleted', 'columns': ['tenantId', 'name'], 'condition': 'isDeleted = false'}

### 5.6.6.0.0 Indexes

#### 5.6.6.1.0 BTree

##### 5.6.6.1.1 Name

IX_OpcCoreClient_Tenant_Status_Heartbeat

##### 5.6.6.1.2 Columns

- tenantId
- status
- lastHeartbeat DESC

##### 5.6.6.1.3 Type

🔹 BTree

#### 5.6.6.2.0 BTree

##### 5.6.6.2.1 Name

IX_OpcCoreClient_Tenant_IsDeleted

##### 5.6.6.2.2 Columns

- tenantId
- isDeleted

##### 5.6.6.2.3 Type

🔹 BTree

## 5.7.0.0.0 OpcServerConnection

### 5.7.1.0.0 Name

OpcServerConnection

### 5.7.2.0.0 Description

Stores configuration details for connecting to an industrial OPC server or a Digital Twin. (REQ-1-002, REQ-FR-020)

### 5.7.3.0.0 Attributes

#### 5.7.3.1.0 Guid

##### 5.7.3.1.1 Name

opcServerConnectionId

##### 5.7.3.1.2 Type

🔹 Guid

##### 5.7.3.1.3 Is Required

✅ Yes

##### 5.7.3.1.4 Is Primary Key

✅ Yes

##### 5.7.3.1.5 Is Unique

✅ Yes

##### 5.7.3.1.6 Index Type

UniqueIndex

#### 5.7.3.2.0 Guid

##### 5.7.3.2.1 Name

opcCoreClientId

##### 5.7.3.2.2 Type

🔹 Guid

##### 5.7.3.2.3 Is Required

✅ Yes

##### 5.7.3.2.4 Is Primary Key

❌ No

##### 5.7.3.2.5 Is Unique

❌ No

##### 5.7.3.2.6 Index Type

Index

##### 5.7.3.2.7 Is Foreign Key

✅ Yes

#### 5.7.3.3.0 VARCHAR

##### 5.7.3.3.1 Name

name

##### 5.7.3.3.2 Type

🔹 VARCHAR

##### 5.7.3.3.3 Is Required

✅ Yes

##### 5.7.3.3.4 Is Primary Key

❌ No

##### 5.7.3.3.5 Is Unique

❌ No

##### 5.7.3.3.6 Index Type

Index

##### 5.7.3.3.7 Size

255

#### 5.7.3.4.0 VARCHAR

##### 5.7.3.4.1 Name

protocol

##### 5.7.3.4.2 Type

🔹 VARCHAR

##### 5.7.3.4.3 Is Required

✅ Yes

##### 5.7.3.4.4 Is Primary Key

❌ No

##### 5.7.3.4.5 Is Unique

❌ No

##### 5.7.3.4.6 Index Type

None

##### 5.7.3.4.7 Size

20

##### 5.7.3.4.8 Constraints

- ENUM('OPC-DA', 'OPC-UA', 'OPC-XML-DA')

#### 5.7.3.5.0 VARCHAR

##### 5.7.3.5.1 Name

endpointUrl

##### 5.7.3.5.2 Type

🔹 VARCHAR

##### 5.7.3.5.3 Is Required

✅ Yes

##### 5.7.3.5.4 Is Primary Key

❌ No

##### 5.7.3.5.5 Is Unique

❌ No

##### 5.7.3.5.6 Index Type

None

##### 5.7.3.5.7 Size

512

#### 5.7.3.6.0 JSONB

##### 5.7.3.6.1 Name

securityConfiguration

##### 5.7.3.6.2 Type

🔹 JSONB

##### 5.7.3.6.3 Is Required

❌ No

##### 5.7.3.6.4 Is Primary Key

❌ No

##### 5.7.3.6.5 Is Unique

❌ No

##### 5.7.3.6.6 Index Type

None

##### 5.7.3.6.7 Default Value

{}

#### 5.7.3.7.0 BOOLEAN

##### 5.7.3.7.1 Name

isRedundantPair

##### 5.7.3.7.2 Type

🔹 BOOLEAN

##### 5.7.3.7.3 Is Required

✅ Yes

##### 5.7.3.7.4 Is Primary Key

❌ No

##### 5.7.3.7.5 Is Unique

❌ No

##### 5.7.3.7.6 Index Type

None

##### 5.7.3.7.7 Default Value

false

#### 5.7.3.8.0 Guid

##### 5.7.3.8.1 Name

backupServerConnectionId

##### 5.7.3.8.2 Type

🔹 Guid

##### 5.7.3.8.3 Is Required

❌ No

##### 5.7.3.8.4 Is Primary Key

❌ No

##### 5.7.3.8.5 Is Unique

❌ No

##### 5.7.3.8.6 Index Type

Index

##### 5.7.3.8.7 Is Foreign Key

✅ Yes

#### 5.7.3.9.0 BOOLEAN

##### 5.7.3.9.1 Name

isDigitalTwin

##### 5.7.3.9.2 Type

🔹 BOOLEAN

##### 5.7.3.9.3 Is Required

✅ Yes

##### 5.7.3.9.4 Is Primary Key

❌ No

##### 5.7.3.9.5 Is Unique

❌ No

##### 5.7.3.9.6 Index Type

Index

##### 5.7.3.9.7 Default Value

false

##### 5.7.3.9.8 Comment

Flag to indicate connection is to a digital twin for UI/safety purposes (REQ-FR-020).

### 5.7.4.0.0 Primary Keys

- opcServerConnectionId

### 5.7.5.0.0 Unique Constraints

- {'name': 'UC_OpcServerConnection_Client_Name', 'columns': ['opcCoreClientId', 'name']}

### 5.7.6.0.0 Indexes

- {'name': 'IX_OpcServerConnection_OpcCoreClientId', 'columns': ['opcCoreClientId'], 'type': 'BTree'}

## 5.8.0.0.0 Asset

### 5.8.1.0.0 Name

Asset

### 5.8.2.0.0 Description

Represents a physical or logical asset in a hierarchical structure (ISA-95). (REQ-1-031, REQ-1-046)

### 5.8.3.0.0 Attributes

#### 5.8.3.1.0 Guid

##### 5.8.3.1.1 Name

assetId

##### 5.8.3.1.2 Type

🔹 Guid

##### 5.8.3.1.3 Is Required

✅ Yes

##### 5.8.3.1.4 Is Primary Key

✅ Yes

##### 5.8.3.1.5 Is Unique

✅ Yes

##### 5.8.3.1.6 Index Type

UniqueIndex

#### 5.8.3.2.0 Guid

##### 5.8.3.2.1 Name

tenantId

##### 5.8.3.2.2 Type

🔹 Guid

##### 5.8.3.2.3 Is Required

✅ Yes

##### 5.8.3.2.4 Is Primary Key

❌ No

##### 5.8.3.2.5 Is Unique

❌ No

##### 5.8.3.2.6 Index Type

Index

##### 5.8.3.2.7 Is Foreign Key

✅ Yes

#### 5.8.3.3.0 VARCHAR

##### 5.8.3.3.1 Name

name

##### 5.8.3.3.2 Type

🔹 VARCHAR

##### 5.8.3.3.3 Is Required

✅ Yes

##### 5.8.3.3.4 Is Primary Key

❌ No

##### 5.8.3.3.5 Is Unique

❌ No

##### 5.8.3.3.6 Index Type

Index

##### 5.8.3.3.7 Size

255

#### 5.8.3.4.0 Guid

##### 5.8.3.4.1 Name

parentAssetId

##### 5.8.3.4.2 Type

🔹 Guid

##### 5.8.3.4.3 Is Required

❌ No

##### 5.8.3.4.4 Is Primary Key

❌ No

##### 5.8.3.4.5 Is Unique

❌ No

##### 5.8.3.4.6 Index Type

Index

##### 5.8.3.4.7 Is Foreign Key

✅ Yes

#### 5.8.3.5.0 Guid

##### 5.8.3.5.1 Name

assetTemplateId

##### 5.8.3.5.2 Type

🔹 Guid

##### 5.8.3.5.3 Is Required

❌ No

##### 5.8.3.5.4 Is Primary Key

❌ No

##### 5.8.3.5.5 Is Unique

❌ No

##### 5.8.3.5.6 Index Type

Index

##### 5.8.3.5.7 Is Foreign Key

✅ Yes

#### 5.8.3.6.0 JSONB

##### 5.8.3.6.1 Name

properties

##### 5.8.3.6.2 Type

🔹 JSONB

##### 5.8.3.6.3 Is Required

❌ No

##### 5.8.3.6.4 Is Primary Key

❌ No

##### 5.8.3.6.5 Is Unique

❌ No

##### 5.8.3.6.6 Index Type

None

##### 5.8.3.6.7 Default Value

{}

#### 5.8.3.7.0 BOOLEAN

##### 5.8.3.7.1 Name

isDeleted

##### 5.8.3.7.2 Type

🔹 BOOLEAN

##### 5.8.3.7.3 Is Required

✅ Yes

##### 5.8.3.7.4 Is Primary Key

❌ No

##### 5.8.3.7.5 Is Unique

❌ No

##### 5.8.3.7.6 Index Type

Index

##### 5.8.3.7.7 Default Value

false

#### 5.8.3.8.0 DateTimeOffset

##### 5.8.3.8.1 Name

createdAt

##### 5.8.3.8.2 Type

🔹 DateTimeOffset

##### 5.8.3.8.3 Is Required

✅ Yes

##### 5.8.3.8.4 Is Primary Key

❌ No

##### 5.8.3.8.5 Is Unique

❌ No

##### 5.8.3.8.6 Index Type

Index

##### 5.8.3.8.7 Default Value

CURRENT_TIMESTAMP

#### 5.8.3.9.0 DateTimeOffset

##### 5.8.3.9.1 Name

updatedAt

##### 5.8.3.9.2 Type

🔹 DateTimeOffset

##### 5.8.3.9.3 Is Required

✅ Yes

##### 5.8.3.9.4 Is Primary Key

❌ No

##### 5.8.3.9.5 Is Unique

❌ No

##### 5.8.3.9.6 Index Type

None

##### 5.8.3.9.7 Default Value

CURRENT_TIMESTAMP

### 5.8.4.0.0 Primary Keys

- assetId

### 5.8.5.0.0 Unique Constraints

- {'name': 'UC_Asset_Tenant_Parent_Name_NotDeleted', 'columns': ['tenantId', 'parentAssetId', 'name'], 'condition': 'isDeleted = false'}

### 5.8.6.0.0 Indexes

#### 5.8.6.1.0 BTree

##### 5.8.6.1.1 Name

IX_Asset_Tenant_Parent

##### 5.8.6.1.2 Columns

- tenantId
- parentAssetId

##### 5.8.6.1.3 Type

🔹 BTree

#### 5.8.6.2.0 BTree

##### 5.8.6.2.1 Name

IX_Asset_Tenant_IsDeleted

##### 5.8.6.2.2 Columns

- tenantId
- isDeleted

##### 5.8.6.2.3 Type

🔹 BTree

#### 5.8.6.3.0 BTree

##### 5.8.6.3.1 Name

IX_Asset_AssetTemplateId

##### 5.8.6.3.2 Columns

- assetTemplateId

##### 5.8.6.3.3 Type

🔹 BTree

### 5.8.7.0.0 Caching

| Property | Value |
|----------|-------|
| Strategy | Redis |
| Key | asset_hierarchy:{tenantId} |
| Invalidation | On CUD operations on the Asset table. |
| Comment | Caches the tree structure to avoid expensive recur... |

## 5.9.0.0.0 OpcTag

### 5.9.1.0.0 Name

OpcTag

### 5.9.2.0.0 Description

Represents a data point (tag) from an OPC server, mapped to an asset. (REQ-1-047)

### 5.9.3.0.0 Attributes

#### 5.9.3.1.0 Guid

##### 5.9.3.1.1 Name

opcTagId

##### 5.9.3.1.2 Type

🔹 Guid

##### 5.9.3.1.3 Is Required

✅ Yes

##### 5.9.3.1.4 Is Primary Key

✅ Yes

##### 5.9.3.1.5 Is Unique

✅ Yes

##### 5.9.3.1.6 Index Type

UniqueIndex

#### 5.9.3.2.0 Guid

##### 5.9.3.2.1 Name

assetId

##### 5.9.3.2.2 Type

🔹 Guid

##### 5.9.3.2.3 Is Required

✅ Yes

##### 5.9.3.2.4 Is Primary Key

❌ No

##### 5.9.3.2.5 Is Unique

❌ No

##### 5.9.3.2.6 Index Type

Index

##### 5.9.3.2.7 Is Foreign Key

✅ Yes

#### 5.9.3.3.0 Guid

##### 5.9.3.3.1 Name

opcServerConnectionId

##### 5.9.3.3.2 Type

🔹 Guid

##### 5.9.3.3.3 Is Required

✅ Yes

##### 5.9.3.3.4 Is Primary Key

❌ No

##### 5.9.3.3.5 Is Unique

❌ No

##### 5.9.3.3.6 Index Type

Index

##### 5.9.3.3.7 Is Foreign Key

✅ Yes

#### 5.9.3.4.0 VARCHAR

##### 5.9.3.4.1 Name

name

##### 5.9.3.4.2 Type

🔹 VARCHAR

##### 5.9.3.4.3 Is Required

✅ Yes

##### 5.9.3.4.4 Is Primary Key

❌ No

##### 5.9.3.4.5 Is Unique

❌ No

##### 5.9.3.4.6 Index Type

Index

##### 5.9.3.4.7 Size

255

#### 5.9.3.5.0 VARCHAR

##### 5.9.3.5.1 Name

nodeId

##### 5.9.3.5.2 Type

🔹 VARCHAR

##### 5.9.3.5.3 Is Required

✅ Yes

##### 5.9.3.5.4 Is Primary Key

❌ No

##### 5.9.3.5.5 Is Unique

❌ No

##### 5.9.3.5.6 Index Type

Index

##### 5.9.3.5.7 Size

512

#### 5.9.3.6.0 VARCHAR

##### 5.9.3.6.1 Name

dataType

##### 5.9.3.6.2 Type

🔹 VARCHAR

##### 5.9.3.6.3 Is Required

❌ No

##### 5.9.3.6.4 Is Primary Key

❌ No

##### 5.9.3.6.5 Is Unique

❌ No

##### 5.9.3.6.6 Index Type

None

##### 5.9.3.6.7 Size

50

#### 5.9.3.7.0 BOOLEAN

##### 5.9.3.7.1 Name

isWritable

##### 5.9.3.7.2 Type

🔹 BOOLEAN

##### 5.9.3.7.3 Is Required

✅ Yes

##### 5.9.3.7.4 Is Primary Key

❌ No

##### 5.9.3.7.5 Is Unique

❌ No

##### 5.9.3.7.6 Index Type

None

##### 5.9.3.7.7 Default Value

false

#### 5.9.3.8.0 INT

##### 5.9.3.8.1 Name

subscriptionUpdateRateMs

##### 5.9.3.8.2 Type

🔹 INT

##### 5.9.3.8.3 Is Required

❌ No

##### 5.9.3.8.4 Is Primary Key

❌ No

##### 5.9.3.8.5 Is Unique

❌ No

##### 5.9.3.8.6 Index Type

None

##### 5.9.3.8.7 Default Value

1000

### 5.9.4.0.0 Primary Keys

- opcTagId

### 5.9.5.0.0 Unique Constraints

- {'name': 'UC_OpcTag_Connection_NodeId', 'columns': ['opcServerConnectionId', 'nodeId']}

### 5.9.6.0.0 Indexes

- {'name': 'IX_OpcTag_AssetId', 'columns': ['assetId'], 'type': 'BTree'}

### 5.9.7.0.0 Caching

| Property | Value |
|----------|-------|
| Strategy | Redis |
| Key | opctag_config:{opcServerConnectionId} |
| Invalidation | On tag configuration changes. |
| Comment | Caches tag configuration which is read frequently ... |

## 5.10.0.0.0 TagDataPoint

### 5.10.1.0.0 Name

TagDataPoint

### 5.10.2.0.0 Description

Represents a single time-series data point for an OPC tag. Stored in TimescaleDB. (REQ-1-003)

### 5.10.3.0.0 Attributes

#### 5.10.3.1.0 DateTimeOffset

##### 5.10.3.1.1 Name

timestamp

##### 5.10.3.1.2 Type

🔹 DateTimeOffset

##### 5.10.3.1.3 Is Required

✅ Yes

##### 5.10.3.1.4 Is Primary Key

✅ Yes

##### 5.10.3.1.5 Is Unique

❌ No

##### 5.10.3.1.6 Index Type

Index

#### 5.10.3.2.0 Guid

##### 5.10.3.2.1 Name

opcTagId

##### 5.10.3.2.2 Type

🔹 Guid

##### 5.10.3.2.3 Is Required

✅ Yes

##### 5.10.3.2.4 Is Primary Key

✅ Yes

##### 5.10.3.2.5 Is Unique

❌ No

##### 5.10.3.2.6 Index Type

Index

##### 5.10.3.2.7 Is Foreign Key

✅ Yes

#### 5.10.3.3.0 Guid

##### 5.10.3.3.1 Name

tenantId

##### 5.10.3.3.2 Type

🔹 Guid

##### 5.10.3.3.3 Is Required

✅ Yes

##### 5.10.3.3.4 Is Primary Key

❌ No

##### 5.10.3.3.5 Is Unique

❌ No

##### 5.10.3.3.6 Index Type

Index

##### 5.10.3.3.7 Is Foreign Key

✅ Yes

##### 5.10.3.3.8 Comment

Denormalized for performance and RLS.

#### 5.10.3.4.0 Guid

##### 5.10.3.4.1 Name

assetId

##### 5.10.3.4.2 Type

🔹 Guid

##### 5.10.3.4.3 Is Required

✅ Yes

##### 5.10.3.4.4 Is Primary Key

❌ No

##### 5.10.3.4.5 Is Unique

❌ No

##### 5.10.3.4.6 Index Type

Index

##### 5.10.3.4.7 Is Foreign Key

✅ Yes

##### 5.10.3.4.8 Comment

Denormalized for performance.

#### 5.10.3.5.0 DOUBLE PRECISION

##### 5.10.3.5.1 Name

value

##### 5.10.3.5.2 Type

🔹 DOUBLE PRECISION

##### 5.10.3.5.3 Is Required

✅ Yes

##### 5.10.3.5.4 Is Primary Key

❌ No

##### 5.10.3.5.5 Is Unique

❌ No

##### 5.10.3.5.6 Index Type

None

#### 5.10.3.6.0 VARCHAR

##### 5.10.3.6.1 Name

quality

##### 5.10.3.6.2 Type

🔹 VARCHAR

##### 5.10.3.6.3 Is Required

✅ Yes

##### 5.10.3.6.4 Is Primary Key

❌ No

##### 5.10.3.6.5 Is Unique

❌ No

##### 5.10.3.6.6 Index Type

None

##### 5.10.3.6.7 Size

50

### 5.10.4.0.0 Primary Keys

- opcTagId
- timestamp

### 5.10.5.0.0 Unique Constraints

*No items available*

### 5.10.6.0.0 Indexes

- {'name': 'IX_TagDataPoint_Tenant_Asset_Timestamp', 'columns': ['tenantId', 'assetId', 'timestamp DESC'], 'type': 'BTree'}

### 5.10.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Hypertable |
| Column | timestamp |
| Comment | Critical for time-series performance. (REQ-NFR-001... |

### 5.10.8.0.0 Optimizations

- {'type': 'Continuous Aggregate', 'suggestion': 'Create materialized views for hourly/daily averages, min/max for fast reporting.', 'comment': 'Massively reduces query latency for dashboards.'}

## 5.11.0.0.0 Alarm

### 5.11.1.0.0 Name

Alarm

### 5.11.2.0.0 Description

Represents an alarm or event from an OPC A&C server or generated by the system. (REQ-1-035)

### 5.11.3.0.0 Attributes

#### 5.11.3.1.0 Guid

##### 5.11.3.1.1 Name

alarmId

##### 5.11.3.1.2 Type

🔹 Guid

##### 5.11.3.1.3 Is Required

✅ Yes

##### 5.11.3.1.4 Is Primary Key

✅ Yes

##### 5.11.3.1.5 Is Unique

✅ Yes

##### 5.11.3.1.6 Index Type

UniqueIndex

#### 5.11.3.2.0 Guid

##### 5.11.3.2.1 Name

tenantId

##### 5.11.3.2.2 Type

🔹 Guid

##### 5.11.3.2.3 Is Required

✅ Yes

##### 5.11.3.2.4 Is Primary Key

❌ No

##### 5.11.3.2.5 Is Unique

❌ No

##### 5.11.3.2.6 Index Type

Index

##### 5.11.3.2.7 Is Foreign Key

✅ Yes

##### 5.11.3.2.8 Comment

Denormalized for performance and RLS.

#### 5.11.3.3.0 Guid

##### 5.11.3.3.1 Name

assetId

##### 5.11.3.3.2 Type

🔹 Guid

##### 5.11.3.3.3 Is Required

✅ Yes

##### 5.11.3.3.4 Is Primary Key

❌ No

##### 5.11.3.3.5 Is Unique

❌ No

##### 5.11.3.3.6 Index Type

Index

##### 5.11.3.3.7 Is Foreign Key

✅ Yes

##### 5.11.3.3.8 Comment

Denormalized for performance.

#### 5.11.3.4.0 Guid

##### 5.11.3.4.1 Name

opcTagId

##### 5.11.3.4.2 Type

🔹 Guid

##### 5.11.3.4.3 Is Required

✅ Yes

##### 5.11.3.4.4 Is Primary Key

❌ No

##### 5.11.3.4.5 Is Unique

❌ No

##### 5.11.3.4.6 Index Type

Index

##### 5.11.3.4.7 Is Foreign Key

✅ Yes

#### 5.11.3.5.0 VARCHAR

##### 5.11.3.5.1 Name

state

##### 5.11.3.5.2 Type

🔹 VARCHAR

##### 5.11.3.5.3 Is Required

✅ Yes

##### 5.11.3.5.4 Is Primary Key

❌ No

##### 5.11.3.5.5 Is Unique

❌ No

##### 5.11.3.5.6 Index Type

Index

##### 5.11.3.5.7 Size

50

##### 5.11.3.5.8 Constraints

- ENUM('Active', 'Acknowledged', 'Shelved', 'Suppressed', 'Cleared')

##### 5.11.3.5.9 Default Value

Active

#### 5.11.3.6.0 INT

##### 5.11.3.6.1 Name

severity

##### 5.11.3.6.2 Type

🔹 INT

##### 5.11.3.6.3 Is Required

✅ Yes

##### 5.11.3.6.4 Is Primary Key

❌ No

##### 5.11.3.6.5 Is Unique

❌ No

##### 5.11.3.6.6 Index Type

Index

##### 5.11.3.6.7 Default Value

500

#### 5.11.3.7.0 TEXT

##### 5.11.3.7.1 Name

message

##### 5.11.3.7.2 Type

🔹 TEXT

##### 5.11.3.7.3 Is Required

❌ No

##### 5.11.3.7.4 Is Primary Key

❌ No

##### 5.11.3.7.5 Is Unique

❌ No

##### 5.11.3.7.6 Index Type

None

#### 5.11.3.8.0 DateTimeOffset

##### 5.11.3.8.1 Name

activeTimestamp

##### 5.11.3.8.2 Type

🔹 DateTimeOffset

##### 5.11.3.8.3 Is Required

✅ Yes

##### 5.11.3.8.4 Is Primary Key

❌ No

##### 5.11.3.8.5 Is Unique

❌ No

##### 5.11.3.8.6 Index Type

Index

#### 5.11.3.9.0 DateTimeOffset

##### 5.11.3.9.1 Name

acknowledgedTimestamp

##### 5.11.3.9.2 Type

🔹 DateTimeOffset

##### 5.11.3.9.3 Is Required

❌ No

##### 5.11.3.9.4 Is Primary Key

❌ No

##### 5.11.3.9.5 Is Unique

❌ No

##### 5.11.3.9.6 Index Type

None

#### 5.11.3.10.0 DateTimeOffset

##### 5.11.3.10.1 Name

shelvedUntilTimestamp

##### 5.11.3.10.2 Type

🔹 DateTimeOffset

##### 5.11.3.10.3 Is Required

❌ No

##### 5.11.3.10.4 Is Primary Key

❌ No

##### 5.11.3.10.5 Is Unique

❌ No

##### 5.11.3.10.6 Index Type

Index

### 5.11.4.0.0 Primary Keys

- alarmId

### 5.11.5.0.0 Unique Constraints

*No items available*

### 5.11.6.0.0 Indexes

#### 5.11.6.1.0 BTree

##### 5.11.6.1.1 Name

IX_Alarm_OpcTagId

##### 5.11.6.1.2 Columns

- opcTagId

##### 5.11.6.1.3 Type

🔹 BTree

#### 5.11.6.2.0 BTree

##### 5.11.6.2.1 Name

IX_Alarm_Tenant_State_Severity_Timestamp

##### 5.11.6.2.2 Columns

- tenantId
- state
- severity DESC
- activeTimestamp DESC

##### 5.11.6.2.3 Type

🔹 BTree

#### 5.11.6.3.0 BTree

##### 5.11.6.3.1 Name

IX_Alarm_AssetId

##### 5.11.6.3.2 Columns

- assetId

##### 5.11.6.3.3 Type

🔹 BTree

## 5.12.0.0.0 AlarmHistory

### 5.12.1.0.0 Name

AlarmHistory

### 5.12.2.0.0 Description

Logs all state changes and actions for a given alarm. (REQ-1-036)

### 5.12.3.0.0 Attributes

#### 5.12.3.1.0 BIGINT

##### 5.12.3.1.1 Name

alarmHistoryId

##### 5.12.3.1.2 Type

🔹 BIGINT

##### 5.12.3.1.3 Is Required

✅ Yes

##### 5.12.3.1.4 Is Primary Key

✅ Yes

##### 5.12.3.1.5 Is Unique

✅ Yes

##### 5.12.3.1.6 Index Type

UniqueIndex

##### 5.12.3.1.7 Constraints

- AUTO_INCREMENT

#### 5.12.3.2.0 Guid

##### 5.12.3.2.1 Name

alarmId

##### 5.12.3.2.2 Type

🔹 Guid

##### 5.12.3.2.3 Is Required

✅ Yes

##### 5.12.3.2.4 Is Primary Key

❌ No

##### 5.12.3.2.5 Is Unique

❌ No

##### 5.12.3.2.6 Index Type

Index

##### 5.12.3.2.7 Is Foreign Key

✅ Yes

#### 5.12.3.3.0 Guid

##### 5.12.3.3.1 Name

userId

##### 5.12.3.3.2 Type

🔹 Guid

##### 5.12.3.3.3 Is Required

✅ Yes

##### 5.12.3.3.4 Is Primary Key

❌ No

##### 5.12.3.3.5 Is Unique

❌ No

##### 5.12.3.3.6 Index Type

Index

##### 5.12.3.3.7 Is Foreign Key

✅ Yes

#### 5.12.3.4.0 VARCHAR

##### 5.12.3.4.1 Name

action

##### 5.12.3.4.2 Type

🔹 VARCHAR

##### 5.12.3.4.3 Is Required

✅ Yes

##### 5.12.3.4.4 Is Primary Key

❌ No

##### 5.12.3.4.5 Is Unique

❌ No

##### 5.12.3.4.6 Index Type

None

##### 5.12.3.4.7 Size

50

##### 5.12.3.4.8 Constraints

- ENUM('Acknowledge', 'Shelve', 'Unshelve', 'Suppress')

#### 5.12.3.5.0 DateTimeOffset

##### 5.12.3.5.1 Name

timestamp

##### 5.12.3.5.2 Type

🔹 DateTimeOffset

##### 5.12.3.5.3 Is Required

✅ Yes

##### 5.12.3.5.4 Is Primary Key

❌ No

##### 5.12.3.5.5 Is Unique

❌ No

##### 5.12.3.5.6 Index Type

Index

##### 5.12.3.5.7 Default Value

CURRENT_TIMESTAMP

#### 5.12.3.6.0 TEXT

##### 5.12.3.6.1 Name

comment

##### 5.12.3.6.2 Type

🔹 TEXT

##### 5.12.3.6.3 Is Required

❌ No

##### 5.12.3.6.4 Is Primary Key

❌ No

##### 5.12.3.6.5 Is Unique

❌ No

##### 5.12.3.6.6 Index Type

None

### 5.12.4.0.0 Primary Keys

- alarmHistoryId

### 5.12.5.0.0 Unique Constraints

*No items available*

### 5.12.6.0.0 Indexes

#### 5.12.6.1.0 BTree

##### 5.12.6.1.1 Name

IX_AlarmHistory_Alarm_Timestamp

##### 5.12.6.1.2 Columns

- alarmId
- timestamp

##### 5.12.6.1.3 Type

🔹 BTree

#### 5.12.6.2.0 BTree

##### 5.12.6.2.1 Name

IX_AlarmHistory_User_Timestamp

##### 5.12.6.2.2 Columns

- userId
- timestamp

##### 5.12.6.2.3 Type

🔹 BTree

### 5.12.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Range |
| Column | timestamp |
| Strategy | Monthly |
| Comment | Manages growth of historical alarm event data. |

## 5.13.0.0.0 AiModel

### 5.13.1.0.0 Name

AiModel

### 5.13.2.0.0 Description

Represents a machine learning model available in the system. (REQ-1-049)

### 5.13.3.0.0 Attributes

#### 5.13.3.1.0 Guid

##### 5.13.3.1.1 Name

aiModelId

##### 5.13.3.1.2 Type

🔹 Guid

##### 5.13.3.1.3 Is Required

✅ Yes

##### 5.13.3.1.4 Is Primary Key

✅ Yes

##### 5.13.3.1.5 Is Unique

✅ Yes

##### 5.13.3.1.6 Index Type

UniqueIndex

#### 5.13.3.2.0 Guid

##### 5.13.3.2.1 Name

tenantId

##### 5.13.3.2.2 Type

🔹 Guid

##### 5.13.3.2.3 Is Required

✅ Yes

##### 5.13.3.2.4 Is Primary Key

❌ No

##### 5.13.3.2.5 Is Unique

❌ No

##### 5.13.3.2.6 Index Type

Index

##### 5.13.3.2.7 Is Foreign Key

✅ Yes

#### 5.13.3.3.0 VARCHAR

##### 5.13.3.3.1 Name

name

##### 5.13.3.3.2 Type

🔹 VARCHAR

##### 5.13.3.3.3 Is Required

✅ Yes

##### 5.13.3.3.4 Is Primary Key

❌ No

##### 5.13.3.3.5 Is Unique

❌ No

##### 5.13.3.3.6 Index Type

Index

##### 5.13.3.3.7 Size

255

#### 5.13.3.4.0 TEXT

##### 5.13.3.4.1 Name

description

##### 5.13.3.4.2 Type

🔹 TEXT

##### 5.13.3.4.3 Is Required

❌ No

##### 5.13.3.4.4 Is Primary Key

❌ No

##### 5.13.3.4.5 Is Unique

❌ No

##### 5.13.3.4.6 Index Type

None

#### 5.13.3.5.0 VARCHAR

##### 5.13.3.5.1 Name

modelType

##### 5.13.3.5.2 Type

🔹 VARCHAR

##### 5.13.3.5.3 Is Required

✅ Yes

##### 5.13.3.5.4 Is Primary Key

❌ No

##### 5.13.3.5.5 Is Unique

❌ No

##### 5.13.3.5.6 Index Type

Index

##### 5.13.3.5.7 Size

100

##### 5.13.3.5.8 Constraints

- ENUM('PredictiveMaintenance', 'AnomalyDetection')

#### 5.13.3.6.0 DateTimeOffset

##### 5.13.3.6.1 Name

createdAt

##### 5.13.3.6.2 Type

🔹 DateTimeOffset

##### 5.13.3.6.3 Is Required

✅ Yes

##### 5.13.3.6.4 Is Primary Key

❌ No

##### 5.13.3.6.5 Is Unique

❌ No

##### 5.13.3.6.6 Index Type

Index

##### 5.13.3.6.7 Default Value

CURRENT_TIMESTAMP

#### 5.13.3.7.0 DateTimeOffset

##### 5.13.3.7.1 Name

updatedAt

##### 5.13.3.7.2 Type

🔹 DateTimeOffset

##### 5.13.3.7.3 Is Required

✅ Yes

##### 5.13.3.7.4 Is Primary Key

❌ No

##### 5.13.3.7.5 Is Unique

❌ No

##### 5.13.3.7.6 Index Type

None

##### 5.13.3.7.7 Default Value

CURRENT_TIMESTAMP

### 5.13.4.0.0 Primary Keys

- aiModelId

### 5.13.5.0.0 Unique Constraints

- {'name': 'UC_AiModel_Tenant_Name', 'columns': ['tenantId', 'name']}

### 5.13.6.0.0 Indexes

- {'name': 'IX_AiModel_Tenant_ModelType', 'columns': ['tenantId', 'modelType'], 'type': 'BTree'}

## 5.14.0.0.0 AiModelVersion

### 5.14.1.0.0 Name

AiModelVersion

### 5.14.2.0.0 Description

Manages versions of an AI model, including its file and approval status. (REQ-1-050)

### 5.14.3.0.0 Attributes

#### 5.14.3.1.0 Guid

##### 5.14.3.1.1 Name

aiModelVersionId

##### 5.14.3.1.2 Type

🔹 Guid

##### 5.14.3.1.3 Is Required

✅ Yes

##### 5.14.3.1.4 Is Primary Key

✅ Yes

##### 5.14.3.1.5 Is Unique

✅ Yes

##### 5.14.3.1.6 Index Type

UniqueIndex

#### 5.14.3.2.0 Guid

##### 5.14.3.2.1 Name

aiModelId

##### 5.14.3.2.2 Type

🔹 Guid

##### 5.14.3.2.3 Is Required

✅ Yes

##### 5.14.3.2.4 Is Primary Key

❌ No

##### 5.14.3.2.5 Is Unique

❌ No

##### 5.14.3.2.6 Index Type

Index

##### 5.14.3.2.7 Is Foreign Key

✅ Yes

#### 5.14.3.3.0 VARCHAR

##### 5.14.3.3.1 Name

version

##### 5.14.3.3.2 Type

🔹 VARCHAR

##### 5.14.3.3.3 Is Required

✅ Yes

##### 5.14.3.3.4 Is Primary Key

❌ No

##### 5.14.3.3.5 Is Unique

❌ No

##### 5.14.3.3.6 Index Type

Index

##### 5.14.3.3.7 Size

50

#### 5.14.3.4.0 VARCHAR

##### 5.14.3.4.1 Name

status

##### 5.14.3.4.2 Type

🔹 VARCHAR

##### 5.14.3.4.3 Is Required

✅ Yes

##### 5.14.3.4.4 Is Primary Key

❌ No

##### 5.14.3.4.5 Is Unique

❌ No

##### 5.14.3.4.6 Index Type

Index

##### 5.14.3.4.7 Size

50

##### 5.14.3.4.8 Constraints

- ENUM('PendingApproval', 'Approved', 'Rejected', 'Archived')

##### 5.14.3.4.9 Default Value

PendingApproval

#### 5.14.3.5.0 VARCHAR

##### 5.14.3.5.1 Name

storagePath

##### 5.14.3.5.2 Type

🔹 VARCHAR

##### 5.14.3.5.3 Is Required

✅ Yes

##### 5.14.3.5.4 Is Primary Key

❌ No

##### 5.14.3.5.5 Is Unique

❌ No

##### 5.14.3.5.6 Index Type

None

##### 5.14.3.5.7 Size

1,024

##### 5.14.3.5.8 Comment

Path to the model file in an object store like S3.

#### 5.14.3.6.0 VARCHAR

##### 5.14.3.6.1 Name

modelFormat

##### 5.14.3.6.2 Type

🔹 VARCHAR

##### 5.14.3.6.3 Is Required

✅ Yes

##### 5.14.3.6.4 Is Primary Key

❌ No

##### 5.14.3.6.5 Is Unique

❌ No

##### 5.14.3.6.6 Index Type

None

##### 5.14.3.6.7 Size

20

##### 5.14.3.6.8 Constraints

- ENUM('ONNX')

##### 5.14.3.6.9 Default Value

ONNX

#### 5.14.3.7.0 Guid

##### 5.14.3.7.1 Name

submittedByUserId

##### 5.14.3.7.2 Type

🔹 Guid

##### 5.14.3.7.3 Is Required

✅ Yes

##### 5.14.3.7.4 Is Primary Key

❌ No

##### 5.14.3.7.5 Is Unique

❌ No

##### 5.14.3.7.6 Index Type

Index

##### 5.14.3.7.7 Is Foreign Key

✅ Yes

#### 5.14.3.8.0 Guid

##### 5.14.3.8.1 Name

approvedByUserId

##### 5.14.3.8.2 Type

🔹 Guid

##### 5.14.3.8.3 Is Required

❌ No

##### 5.14.3.8.4 Is Primary Key

❌ No

##### 5.14.3.8.5 Is Unique

❌ No

##### 5.14.3.8.6 Index Type

Index

##### 5.14.3.8.7 Is Foreign Key

✅ Yes

#### 5.14.3.9.0 DateTimeOffset

##### 5.14.3.9.1 Name

createdAt

##### 5.14.3.9.2 Type

🔹 DateTimeOffset

##### 5.14.3.9.3 Is Required

✅ Yes

##### 5.14.3.9.4 Is Primary Key

❌ No

##### 5.14.3.9.5 Is Unique

❌ No

##### 5.14.3.9.6 Index Type

Index

##### 5.14.3.9.7 Default Value

CURRENT_TIMESTAMP

### 5.14.4.0.0 Primary Keys

- aiModelVersionId

### 5.14.5.0.0 Unique Constraints

- {'name': 'UC_AiModelVersion_Model_Version', 'columns': ['aiModelId', 'version']}

### 5.14.6.0.0 Indexes

#### 5.14.6.1.0 BTree

##### 5.14.6.1.1 Name

IX_AiModelVersion_AiModelId

##### 5.14.6.1.2 Columns

- aiModelId

##### 5.14.6.1.3 Type

🔹 BTree

#### 5.14.6.2.0 BTree

##### 5.14.6.2.1 Name

IX_AiModelVersion_Status

##### 5.14.6.2.2 Columns

- status

##### 5.14.6.2.3 Type

🔹 BTree

## 5.15.0.0.0 ModelAssignment

### 5.15.1.0.0 Name

ModelAssignment

### 5.15.2.0.0 Description

Links a specific AI model version to an asset for execution on an edge client. (REQ-1-014, REQ-1-056)

### 5.15.3.0.0 Attributes

#### 5.15.3.1.0 Guid

##### 5.15.3.1.1 Name

modelAssignmentId

##### 5.15.3.1.2 Type

🔹 Guid

##### 5.15.3.1.3 Is Required

✅ Yes

##### 5.15.3.1.4 Is Primary Key

✅ Yes

##### 5.15.3.1.5 Is Unique

✅ Yes

##### 5.15.3.1.6 Index Type

UniqueIndex

#### 5.15.3.2.0 Guid

##### 5.15.3.2.1 Name

assetId

##### 5.15.3.2.2 Type

🔹 Guid

##### 5.15.3.2.3 Is Required

✅ Yes

##### 5.15.3.2.4 Is Primary Key

❌ No

##### 5.15.3.2.5 Is Unique

❌ No

##### 5.15.3.2.6 Index Type

Index

##### 5.15.3.2.7 Is Foreign Key

✅ Yes

#### 5.15.3.3.0 Guid

##### 5.15.3.3.1 Name

aiModelVersionId

##### 5.15.3.3.2 Type

🔹 Guid

##### 5.15.3.3.3 Is Required

✅ Yes

##### 5.15.3.3.4 Is Primary Key

❌ No

##### 5.15.3.3.5 Is Unique

❌ No

##### 5.15.3.3.6 Index Type

Index

##### 5.15.3.3.7 Is Foreign Key

✅ Yes

#### 5.15.3.4.0 Guid

##### 5.15.3.4.1 Name

opcCoreClientId

##### 5.15.3.4.2 Type

🔹 Guid

##### 5.15.3.4.3 Is Required

✅ Yes

##### 5.15.3.4.4 Is Primary Key

❌ No

##### 5.15.3.4.5 Is Unique

❌ No

##### 5.15.3.4.6 Index Type

Index

##### 5.15.3.4.7 Is Foreign Key

✅ Yes

##### 5.15.3.4.8 Comment

Specifies which client is responsible for executing the model.

#### 5.15.3.5.0 BOOLEAN

##### 5.15.3.5.1 Name

isActive

##### 5.15.3.5.2 Type

🔹 BOOLEAN

##### 5.15.3.5.3 Is Required

✅ Yes

##### 5.15.3.5.4 Is Primary Key

❌ No

##### 5.15.3.5.5 Is Unique

❌ No

##### 5.15.3.5.6 Index Type

Index

##### 5.15.3.5.7 Default Value

true

#### 5.15.3.6.0 DateTimeOffset

##### 5.15.3.6.1 Name

assignedAt

##### 5.15.3.6.2 Type

🔹 DateTimeOffset

##### 5.15.3.6.3 Is Required

✅ Yes

##### 5.15.3.6.4 Is Primary Key

❌ No

##### 5.15.3.6.5 Is Unique

❌ No

##### 5.15.3.6.6 Index Type

None

##### 5.15.3.6.7 Default Value

CURRENT_TIMESTAMP

### 5.15.4.0.0 Primary Keys

- modelAssignmentId

### 5.15.5.0.0 Unique Constraints

- {'name': 'UC_ModelAssignment_Asset_Version', 'columns': ['assetId', 'aiModelVersionId']}

### 5.15.6.0.0 Indexes

#### 5.15.6.1.0 BTree

##### 5.15.6.1.1 Name

IX_ModelAssignment_AssetId

##### 5.15.6.1.2 Columns

- assetId

##### 5.15.6.1.3 Type

🔹 BTree

#### 5.15.6.2.0 BTree

##### 5.15.6.2.1 Name

IX_ModelAssignment_AiModelVersionId

##### 5.15.6.2.2 Columns

- aiModelVersionId

##### 5.15.6.2.3 Type

🔹 BTree

#### 5.15.6.3.0 BTree

##### 5.15.6.3.1 Name

IX_ModelAssignment_OpcCoreClientId

##### 5.15.6.3.2 Columns

- opcCoreClientId

##### 5.15.6.3.3 Type

🔹 BTree

## 5.16.0.0.0 AnomalyEvent

### 5.16.1.0.0 Name

AnomalyEvent

### 5.16.2.0.0 Description

Records an anomaly detected by an AI model. (REQ-1-052)

### 5.16.3.0.0 Attributes

#### 5.16.3.1.0 DateTimeOffset

##### 5.16.3.1.1 Name

timestamp

##### 5.16.3.1.2 Type

🔹 DateTimeOffset

##### 5.16.3.1.3 Is Required

✅ Yes

##### 5.16.3.1.4 Is Primary Key

✅ Yes

##### 5.16.3.1.5 Is Unique

❌ No

##### 5.16.3.1.6 Index Type

Index

#### 5.16.3.2.0 Guid

##### 5.16.3.2.1 Name

modelAssignmentId

##### 5.16.3.2.2 Type

🔹 Guid

##### 5.16.3.2.3 Is Required

✅ Yes

##### 5.16.3.2.4 Is Primary Key

✅ Yes

##### 5.16.3.2.5 Is Unique

❌ No

##### 5.16.3.2.6 Index Type

Index

##### 5.16.3.2.7 Is Foreign Key

✅ Yes

#### 5.16.3.3.0 Guid

##### 5.16.3.3.1 Name

anomalyEventId

##### 5.16.3.3.2 Type

🔹 Guid

##### 5.16.3.3.3 Is Required

✅ Yes

##### 5.16.3.3.4 Is Primary Key

❌ No

##### 5.16.3.3.5 Is Unique

✅ Yes

##### 5.16.3.3.6 Index Type

UniqueIndex

##### 5.16.3.3.7 Default Value

GENERATE_UUID()

#### 5.16.3.4.0 DOUBLE PRECISION

##### 5.16.3.4.1 Name

anomalyScore

##### 5.16.3.4.2 Type

🔹 DOUBLE PRECISION

##### 5.16.3.4.3 Is Required

✅ Yes

##### 5.16.3.4.4 Is Primary Key

❌ No

##### 5.16.3.4.5 Is Unique

❌ No

##### 5.16.3.4.6 Index Type

None

#### 5.16.3.5.0 BOOLEAN

##### 5.16.3.5.1 Name

isTrueAnomaly

##### 5.16.3.5.2 Type

🔹 BOOLEAN

##### 5.16.3.5.3 Is Required

❌ No

##### 5.16.3.5.4 Is Primary Key

❌ No

##### 5.16.3.5.5 Is Unique

❌ No

##### 5.16.3.5.6 Index Type

Index

##### 5.16.3.5.7 Comment

Used for operator feedback to aid model retraining.

#### 5.16.3.6.0 TEXT

##### 5.16.3.6.1 Name

feedbackComment

##### 5.16.3.6.2 Type

🔹 TEXT

##### 5.16.3.6.3 Is Required

❌ No

##### 5.16.3.6.4 Is Primary Key

❌ No

##### 5.16.3.6.5 Is Unique

❌ No

##### 5.16.3.6.6 Index Type

None

### 5.16.4.0.0 Primary Keys

- timestamp
- modelAssignmentId

### 5.16.5.0.0 Unique Constraints

- {'name': 'UC_AnomalyEvent_Id', 'columns': ['anomalyEventId']}

### 5.16.6.0.0 Indexes

- {'name': 'IX_AnomalyEvent_IsTrueAnomaly', 'columns': ['isTrueAnomaly'], 'type': 'BTree'}

### 5.16.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Hypertable |
| Column | timestamp |
| Comment | Improves query performance for analyzing anomaly t... |

## 5.17.0.0.0 License

### 5.17.1.0.0 Name

License

### 5.17.2.0.0 Description

Manages software licenses and feature entitlements assigned to tenants. (REQ-1-063)

### 5.17.3.0.0 Attributes

#### 5.17.3.1.0 Guid

##### 5.17.3.1.1 Name

licenseId

##### 5.17.3.1.2 Type

🔹 Guid

##### 5.17.3.1.3 Is Required

✅ Yes

##### 5.17.3.1.4 Is Primary Key

✅ Yes

##### 5.17.3.1.5 Is Unique

✅ Yes

##### 5.17.3.1.6 Index Type

UniqueIndex

#### 5.17.3.2.0 Guid

##### 5.17.3.2.1 Name

tenantId

##### 5.17.3.2.2 Type

🔹 Guid

##### 5.17.3.2.3 Is Required

✅ Yes

##### 5.17.3.2.4 Is Primary Key

❌ No

##### 5.17.3.2.5 Is Unique

✅ Yes

##### 5.17.3.2.6 Index Type

UniqueIndex

##### 5.17.3.2.7 Is Foreign Key

✅ Yes

#### 5.17.3.3.0 VARCHAR

##### 5.17.3.3.1 Name

licenseTier

##### 5.17.3.3.2 Type

🔹 VARCHAR

##### 5.17.3.3.3 Is Required

✅ Yes

##### 5.17.3.3.4 Is Primary Key

❌ No

##### 5.17.3.3.5 Is Unique

❌ No

##### 5.17.3.3.6 Index Type

Index

##### 5.17.3.3.7 Size

50

##### 5.17.3.3.8 Default Value

Silver

#### 5.17.3.4.0 VARCHAR

##### 5.17.3.4.1 Name

licenseKey

##### 5.17.3.4.2 Type

🔹 VARCHAR

##### 5.17.3.4.3 Is Required

✅ Yes

##### 5.17.3.4.4 Is Primary Key

❌ No

##### 5.17.3.4.5 Is Unique

✅ Yes

##### 5.17.3.4.6 Index Type

UniqueIndex

##### 5.17.3.4.7 Size

255

#### 5.17.3.5.0 DateTimeOffset

##### 5.17.3.5.1 Name

validFrom

##### 5.17.3.5.2 Type

🔹 DateTimeOffset

##### 5.17.3.5.3 Is Required

✅ Yes

##### 5.17.3.5.4 Is Primary Key

❌ No

##### 5.17.3.5.5 Is Unique

❌ No

##### 5.17.3.5.6 Index Type

None

#### 5.17.3.6.0 DateTimeOffset

##### 5.17.3.6.1 Name

validTo

##### 5.17.3.6.2 Type

🔹 DateTimeOffset

##### 5.17.3.6.3 Is Required

✅ Yes

##### 5.17.3.6.4 Is Primary Key

❌ No

##### 5.17.3.6.5 Is Unique

❌ No

##### 5.17.3.6.6 Index Type

Index

#### 5.17.3.7.0 INT

##### 5.17.3.7.1 Name

maxUsers

##### 5.17.3.7.2 Type

🔹 INT

##### 5.17.3.7.3 Is Required

❌ No

##### 5.17.3.7.4 Is Primary Key

❌ No

##### 5.17.3.7.5 Is Unique

❌ No

##### 5.17.3.7.6 Index Type

None

#### 5.17.3.8.0 INT

##### 5.17.3.8.1 Name

maxClients

##### 5.17.3.8.2 Type

🔹 INT

##### 5.17.3.8.3 Is Required

❌ No

##### 5.17.3.8.4 Is Primary Key

❌ No

##### 5.17.3.8.5 Is Unique

❌ No

##### 5.17.3.8.6 Index Type

None

### 5.17.4.0.0 Primary Keys

- licenseId

### 5.17.5.0.0 Unique Constraints

#### 5.17.5.1.0 UC_License_TenantId

##### 5.17.5.1.1 Name

UC_License_TenantId

##### 5.17.5.1.2 Columns

- tenantId

#### 5.17.5.2.0 UC_License_LicenseKey

##### 5.17.5.2.1 Name

UC_License_LicenseKey

##### 5.17.5.2.2 Columns

- licenseKey

### 5.17.6.0.0 Indexes

- {'name': 'IX_License_ValidTo', 'columns': ['validTo'], 'type': 'BTree'}

## 5.18.0.0.0 ReportTemplate

### 5.18.1.0.0 Name

ReportTemplate

### 5.18.2.0.0 Description

Defines the configuration for automated, schedulable reports. (REQ-1-065, REQ-FR-015)

### 5.18.3.0.0 Attributes

#### 5.18.3.1.0 Guid

##### 5.18.3.1.1 Name

reportTemplateId

##### 5.18.3.1.2 Type

🔹 Guid

##### 5.18.3.1.3 Is Required

✅ Yes

##### 5.18.3.1.4 Is Primary Key

✅ Yes

##### 5.18.3.1.5 Is Unique

✅ Yes

##### 5.18.3.1.6 Index Type

UniqueIndex

#### 5.18.3.2.0 Guid

##### 5.18.3.2.1 Name

tenantId

##### 5.18.3.2.2 Type

🔹 Guid

##### 5.18.3.2.3 Is Required

✅ Yes

##### 5.18.3.2.4 Is Primary Key

❌ No

##### 5.18.3.2.5 Is Unique

❌ No

##### 5.18.3.2.6 Index Type

Index

##### 5.18.3.2.7 Is Foreign Key

✅ Yes

#### 5.18.3.3.0 VARCHAR

##### 5.18.3.3.1 Name

name

##### 5.18.3.3.2 Type

🔹 VARCHAR

##### 5.18.3.3.3 Is Required

✅ Yes

##### 5.18.3.3.4 Is Primary Key

❌ No

##### 5.18.3.3.5 Is Unique

❌ No

##### 5.18.3.3.6 Index Type

Index

##### 5.18.3.3.7 Size

255

#### 5.18.3.4.0 JSONB

##### 5.18.3.4.1 Name

configuration

##### 5.18.3.4.2 Type

🔹 JSONB

##### 5.18.3.4.3 Is Required

✅ Yes

##### 5.18.3.4.4 Is Primary Key

❌ No

##### 5.18.3.4.5 Is Unique

❌ No

##### 5.18.3.4.6 Index Type

None

##### 5.18.3.4.7 Default Value

{}

##### 5.18.3.4.8 Comment

Contains report parameters, data sources, assets, etc.

#### 5.18.3.5.0 VARCHAR

##### 5.18.3.5.1 Name

schedule

##### 5.18.3.5.2 Type

🔹 VARCHAR

##### 5.18.3.5.3 Is Required

❌ No

##### 5.18.3.5.4 Is Primary Key

❌ No

##### 5.18.3.5.5 Is Unique

❌ No

##### 5.18.3.5.6 Index Type

Index

##### 5.18.3.5.7 Size

100

##### 5.18.3.5.8 Constraints

- CRON_FORMAT

#### 5.18.3.6.0 VARCHAR

##### 5.18.3.6.1 Name

outputFormat

##### 5.18.3.6.2 Type

🔹 VARCHAR

##### 5.18.3.6.3 Is Required

✅ Yes

##### 5.18.3.6.4 Is Primary Key

❌ No

##### 5.18.3.6.5 Is Unique

❌ No

##### 5.18.3.6.6 Index Type

None

##### 5.18.3.6.7 Size

20

##### 5.18.3.6.8 Constraints

- ENUM('PDF', 'HTML', 'CSV')

##### 5.18.3.6.9 Default Value

PDF

#### 5.18.3.7.0 JSONB

##### 5.18.3.7.1 Name

distributionList

##### 5.18.3.7.2 Type

🔹 JSONB

##### 5.18.3.7.3 Is Required

❌ No

##### 5.18.3.7.4 Is Primary Key

❌ No

##### 5.18.3.7.5 Is Unique

❌ No

##### 5.18.3.7.6 Index Type

None

##### 5.18.3.7.7 Default Value

[]

##### 5.18.3.7.8 Comment

List of emails or webhook URLs for distribution.

### 5.18.4.0.0 Primary Keys

- reportTemplateId

### 5.18.5.0.0 Unique Constraints

- {'name': 'UC_ReportTemplate_Tenant_Name', 'columns': ['tenantId', 'name']}

### 5.18.6.0.0 Indexes

#### 5.18.6.1.0 BTree

##### 5.18.6.1.1 Name

IX_ReportTemplate_TenantId

##### 5.18.6.1.2 Columns

- tenantId

##### 5.18.6.1.3 Type

🔹 BTree

#### 5.18.6.2.0 BTree

##### 5.18.6.2.1 Name

IX_ReportTemplate_Schedule

##### 5.18.6.2.2 Columns

- schedule

##### 5.18.6.2.3 Type

🔹 BTree

## 5.19.0.0.0 ApprovalRequest

### 5.19.1.0.0 Name

ApprovalRequest

### 5.19.2.0.0 Description

Tracks a request for a critical system change under a Management of Change (MOC) workflow. (REQ-1-032, REQ-CON-005)

### 5.19.3.0.0 Attributes

#### 5.19.3.1.0 Guid

##### 5.19.3.1.1 Name

approvalRequestId

##### 5.19.3.1.2 Type

🔹 Guid

##### 5.19.3.1.3 Is Required

✅ Yes

##### 5.19.3.1.4 Is Primary Key

✅ Yes

##### 5.19.3.1.5 Is Unique

✅ Yes

##### 5.19.3.1.6 Index Type

UniqueIndex

#### 5.19.3.2.0 Guid

##### 5.19.3.2.1 Name

tenantId

##### 5.19.3.2.2 Type

🔹 Guid

##### 5.19.3.2.3 Is Required

✅ Yes

##### 5.19.3.2.4 Is Primary Key

❌ No

##### 5.19.3.2.5 Is Unique

❌ No

##### 5.19.3.2.6 Index Type

Index

##### 5.19.3.2.7 Is Foreign Key

✅ Yes

#### 5.19.3.3.0 VARCHAR

##### 5.19.3.3.1 Name

requestType

##### 5.19.3.3.2 Type

🔹 VARCHAR

##### 5.19.3.3.3 Is Required

✅ Yes

##### 5.19.3.3.4 Is Primary Key

❌ No

##### 5.19.3.3.5 Is Unique

❌ No

##### 5.19.3.3.6 Index Type

Index

##### 5.19.3.3.7 Size

100

##### 5.19.3.3.8 Constraints

- ENUM('DeployAiModel', 'ModifyAlarmPriority', 'ChangeSecurityPolicy')

#### 5.19.3.4.0 VARCHAR

##### 5.19.3.4.1 Name

status

##### 5.19.3.4.2 Type

🔹 VARCHAR

##### 5.19.3.4.3 Is Required

✅ Yes

##### 5.19.3.4.4 Is Primary Key

❌ No

##### 5.19.3.4.5 Is Unique

❌ No

##### 5.19.3.4.6 Index Type

Index

##### 5.19.3.4.7 Size

50

##### 5.19.3.4.8 Constraints

- ENUM('Pending', 'Approved', 'Rejected')

##### 5.19.3.4.9 Default Value

Pending

#### 5.19.3.5.0 Guid

##### 5.19.3.5.1 Name

requestedByUserId

##### 5.19.3.5.2 Type

🔹 Guid

##### 5.19.3.5.3 Is Required

✅ Yes

##### 5.19.3.5.4 Is Primary Key

❌ No

##### 5.19.3.5.5 Is Unique

❌ No

##### 5.19.3.5.6 Index Type

Index

##### 5.19.3.5.7 Is Foreign Key

✅ Yes

#### 5.19.3.6.0 Guid

##### 5.19.3.6.1 Name

approvedByUserId

##### 5.19.3.6.2 Type

🔹 Guid

##### 5.19.3.6.3 Is Required

❌ No

##### 5.19.3.6.4 Is Primary Key

❌ No

##### 5.19.3.6.5 Is Unique

❌ No

##### 5.19.3.6.6 Index Type

Index

##### 5.19.3.6.7 Is Foreign Key

✅ Yes

#### 5.19.3.7.0 JSONB

##### 5.19.3.7.1 Name

requestDetails

##### 5.19.3.7.2 Type

🔹 JSONB

##### 5.19.3.7.3 Is Required

✅ Yes

##### 5.19.3.7.4 Is Primary Key

❌ No

##### 5.19.3.7.5 Is Unique

❌ No

##### 5.19.3.7.6 Index Type

None

##### 5.19.3.7.7 Default Value

{}

#### 5.19.3.8.0 DateTimeOffset

##### 5.19.3.8.1 Name

createdAt

##### 5.19.3.8.2 Type

🔹 DateTimeOffset

##### 5.19.3.8.3 Is Required

✅ Yes

##### 5.19.3.8.4 Is Primary Key

❌ No

##### 5.19.3.8.5 Is Unique

❌ No

##### 5.19.3.8.6 Index Type

Index

##### 5.19.3.8.7 Default Value

CURRENT_TIMESTAMP

#### 5.19.3.9.0 DateTimeOffset

##### 5.19.3.9.1 Name

updatedAt

##### 5.19.3.9.2 Type

🔹 DateTimeOffset

##### 5.19.3.9.3 Is Required

✅ Yes

##### 5.19.3.9.4 Is Primary Key

❌ No

##### 5.19.3.9.5 Is Unique

❌ No

##### 5.19.3.9.6 Index Type

None

##### 5.19.3.9.7 Default Value

CURRENT_TIMESTAMP

### 5.19.4.0.0 Primary Keys

- approvalRequestId

### 5.19.5.0.0 Unique Constraints

*No items available*

### 5.19.6.0.0 Indexes

#### 5.19.6.1.0 BTree

##### 5.19.6.1.1 Name

IX_ApprovalRequest_Tenant_Status_Type_Created

##### 5.19.6.1.2 Columns

- tenantId
- status
- requestType
- createdAt

##### 5.19.6.1.3 Type

🔹 BTree

#### 5.19.6.2.0 BTree

##### 5.19.6.2.1 Name

IX_ApprovalRequest_RequestedByUser

##### 5.19.6.2.2 Columns

- requestedByUserId

##### 5.19.6.2.3 Type

🔹 BTree

#### 5.19.6.3.0 BTree

##### 5.19.6.3.1 Name

IX_ApprovalRequest_ApprovedByUser

##### 5.19.6.3.2 Columns

- approvedByUserId

##### 5.19.6.3.3 Type

🔹 BTree

## 5.20.0.0.0 Dashboard

### 5.20.1.0.0 Name

Dashboard

### 5.20.2.0.0 Description

Stores user-specific, customizable dashboard configurations. (REQ-FR-009)

### 5.20.3.0.0 Attributes

#### 5.20.3.1.0 Guid

##### 5.20.3.1.1 Name

dashboardId

##### 5.20.3.1.2 Type

🔹 Guid

##### 5.20.3.1.3 Is Required

✅ Yes

##### 5.20.3.1.4 Is Primary Key

✅ Yes

##### 5.20.3.1.5 Is Unique

✅ Yes

##### 5.20.3.1.6 Index Type

UniqueIndex

#### 5.20.3.2.0 Guid

##### 5.20.3.2.1 Name

tenantId

##### 5.20.3.2.2 Type

🔹 Guid

##### 5.20.3.2.3 Is Required

✅ Yes

##### 5.20.3.2.4 Is Primary Key

❌ No

##### 5.20.3.2.5 Is Unique

❌ No

##### 5.20.3.2.6 Index Type

Index

##### 5.20.3.2.7 Is Foreign Key

✅ Yes

#### 5.20.3.3.0 Guid

##### 5.20.3.3.1 Name

userId

##### 5.20.3.3.2 Type

🔹 Guid

##### 5.20.3.3.3 Is Required

✅ Yes

##### 5.20.3.3.4 Is Primary Key

❌ No

##### 5.20.3.3.5 Is Unique

❌ No

##### 5.20.3.3.6 Index Type

Index

##### 5.20.3.3.7 Is Foreign Key

✅ Yes

#### 5.20.3.4.0 VARCHAR

##### 5.20.3.4.1 Name

name

##### 5.20.3.4.2 Type

🔹 VARCHAR

##### 5.20.3.4.3 Is Required

✅ Yes

##### 5.20.3.4.4 Is Primary Key

❌ No

##### 5.20.3.4.5 Is Unique

❌ No

##### 5.20.3.4.6 Index Type

None

##### 5.20.3.4.7 Size

255

#### 5.20.3.5.0 TEXT

##### 5.20.3.5.1 Name

description

##### 5.20.3.5.2 Type

🔹 TEXT

##### 5.20.3.5.3 Is Required

❌ No

##### 5.20.3.5.4 Is Primary Key

❌ No

##### 5.20.3.5.5 Is Unique

❌ No

##### 5.20.3.5.6 Index Type

None

#### 5.20.3.6.0 JSONB

##### 5.20.3.6.1 Name

layoutConfiguration

##### 5.20.3.6.2 Type

🔹 JSONB

##### 5.20.3.6.3 Is Required

❌ No

##### 5.20.3.6.4 Is Primary Key

❌ No

##### 5.20.3.6.5 Is Unique

❌ No

##### 5.20.3.6.6 Index Type

None

##### 5.20.3.6.7 Comment

Stores grid layout information for all widgets on this dashboard.

#### 5.20.3.7.0 BOOLEAN

##### 5.20.3.7.1 Name

isDefault

##### 5.20.3.7.2 Type

🔹 BOOLEAN

##### 5.20.3.7.3 Is Required

✅ Yes

##### 5.20.3.7.4 Is Primary Key

❌ No

##### 5.20.3.7.5 Is Unique

❌ No

##### 5.20.3.7.6 Index Type

Index

##### 5.20.3.7.7 Default Value

false

#### 5.20.3.8.0 DateTimeOffset

##### 5.20.3.8.1 Name

createdAt

##### 5.20.3.8.2 Type

🔹 DateTimeOffset

##### 5.20.3.8.3 Is Required

✅ Yes

##### 5.20.3.8.4 Is Primary Key

❌ No

##### 5.20.3.8.5 Is Unique

❌ No

##### 5.20.3.8.6 Index Type

None

##### 5.20.3.8.7 Default Value

CURRENT_TIMESTAMP

#### 5.20.3.9.0 DateTimeOffset

##### 5.20.3.9.1 Name

updatedAt

##### 5.20.3.9.2 Type

🔹 DateTimeOffset

##### 5.20.3.9.3 Is Required

✅ Yes

##### 5.20.3.9.4 Is Primary Key

❌ No

##### 5.20.3.9.5 Is Unique

❌ No

##### 5.20.3.9.6 Index Type

None

##### 5.20.3.9.7 Default Value

CURRENT_TIMESTAMP

### 5.20.4.0.0 Primary Keys

- dashboardId

### 5.20.5.0.0 Unique Constraints

- {'name': 'UC_Dashboard_User_Name', 'columns': ['userId', 'name']}

### 5.20.6.0.0 Indexes

- {'name': 'IX_Dashboard_TenantId_UserId', 'columns': ['tenantId', 'userId'], 'type': 'BTree'}

## 5.21.0.0.0 Widget

### 5.21.1.0.0 Name

Widget

### 5.21.2.0.0 Description

Represents a single component (e.g., a trend chart, a gauge) on a dashboard. (REQ-FR-009)

### 5.21.3.0.0 Attributes

#### 5.21.3.1.0 Guid

##### 5.21.3.1.1 Name

widgetId

##### 5.21.3.1.2 Type

🔹 Guid

##### 5.21.3.1.3 Is Required

✅ Yes

##### 5.21.3.1.4 Is Primary Key

✅ Yes

##### 5.21.3.1.5 Is Unique

✅ Yes

##### 5.21.3.1.6 Index Type

UniqueIndex

#### 5.21.3.2.0 Guid

##### 5.21.3.2.1 Name

dashboardId

##### 5.21.3.2.2 Type

🔹 Guid

##### 5.21.3.2.3 Is Required

✅ Yes

##### 5.21.3.2.4 Is Primary Key

❌ No

##### 5.21.3.2.5 Is Unique

❌ No

##### 5.21.3.2.6 Index Type

Index

##### 5.21.3.2.7 Is Foreign Key

✅ Yes

#### 5.21.3.3.0 VARCHAR

##### 5.21.3.3.1 Name

widgetType

##### 5.21.3.3.2 Type

🔹 VARCHAR

##### 5.21.3.3.3 Is Required

✅ Yes

##### 5.21.3.3.4 Is Primary Key

❌ No

##### 5.21.3.3.5 Is Unique

❌ No

##### 5.21.3.3.6 Index Type

Index

##### 5.21.3.3.7 Size

50

##### 5.21.3.3.8 Constraints

- ENUM('TrendChart', 'Gauge', 'SingleValue', 'AlarmList', 'AssetStatus')

#### 5.21.3.4.0 JSONB

##### 5.21.3.4.1 Name

configuration

##### 5.21.3.4.2 Type

🔹 JSONB

##### 5.21.3.4.3 Is Required

✅ Yes

##### 5.21.3.4.4 Is Primary Key

❌ No

##### 5.21.3.4.5 Is Unique

❌ No

##### 5.21.3.4.6 Index Type

None

##### 5.21.3.4.7 Comment

Contains widget-specific settings like associated tag IDs, time range, thresholds, etc.

#### 5.21.3.5.0 DateTimeOffset

##### 5.21.3.5.1 Name

createdAt

##### 5.21.3.5.2 Type

🔹 DateTimeOffset

##### 5.21.3.5.3 Is Required

✅ Yes

##### 5.21.3.5.4 Is Primary Key

❌ No

##### 5.21.3.5.5 Is Unique

❌ No

##### 5.21.3.5.6 Index Type

None

##### 5.21.3.5.7 Default Value

CURRENT_TIMESTAMP

#### 5.21.3.6.0 DateTimeOffset

##### 5.21.3.6.1 Name

updatedAt

##### 5.21.3.6.2 Type

🔹 DateTimeOffset

##### 5.21.3.6.3 Is Required

✅ Yes

##### 5.21.3.6.4 Is Primary Key

❌ No

##### 5.21.3.6.5 Is Unique

❌ No

##### 5.21.3.6.6 Index Type

None

##### 5.21.3.6.7 Default Value

CURRENT_TIMESTAMP

### 5.21.4.0.0 Primary Keys

- widgetId

### 5.21.5.0.0 Unique Constraints

*No items available*

### 5.21.6.0.0 Indexes

- {'name': 'IX_Widget_DashboardId', 'columns': ['dashboardId'], 'type': 'BTree'}

## 5.22.0.0.0 ArTagMapping

### 5.22.1.0.0 Name

ArTagMapping

### 5.22.2.0.0 Description

Maps an OPC tag to a physical marker for Augmented Reality visualization. (REQ-FR-018)

### 5.22.3.0.0 Attributes

#### 5.22.3.1.0 Guid

##### 5.22.3.1.1 Name

arTagMappingId

##### 5.22.3.1.2 Type

🔹 Guid

##### 5.22.3.1.3 Is Required

✅ Yes

##### 5.22.3.1.4 Is Primary Key

✅ Yes

##### 5.22.3.1.5 Is Unique

✅ Yes

##### 5.22.3.1.6 Index Type

UniqueIndex

#### 5.22.3.2.0 Guid

##### 5.22.3.2.1 Name

tenantId

##### 5.22.3.2.2 Type

🔹 Guid

##### 5.22.3.2.3 Is Required

✅ Yes

##### 5.22.3.2.4 Is Primary Key

❌ No

##### 5.22.3.2.5 Is Unique

❌ No

##### 5.22.3.2.6 Index Type

Index

##### 5.22.3.2.7 Is Foreign Key

✅ Yes

#### 5.22.3.3.0 Guid

##### 5.22.3.3.1 Name

assetId

##### 5.22.3.3.2 Type

🔹 Guid

##### 5.22.3.3.3 Is Required

✅ Yes

##### 5.22.3.3.4 Is Primary Key

❌ No

##### 5.22.3.3.5 Is Unique

❌ No

##### 5.22.3.3.6 Index Type

Index

##### 5.22.3.3.7 Is Foreign Key

✅ Yes

#### 5.22.3.4.0 Guid

##### 5.22.3.4.1 Name

opcTagId

##### 5.22.3.4.2 Type

🔹 Guid

##### 5.22.3.4.3 Is Required

✅ Yes

##### 5.22.3.4.4 Is Primary Key

❌ No

##### 5.22.3.4.5 Is Unique

❌ No

##### 5.22.3.4.6 Index Type

Index

##### 5.22.3.4.7 Is Foreign Key

✅ Yes

#### 5.22.3.5.0 VARCHAR

##### 5.22.3.5.1 Name

markerType

##### 5.22.3.5.2 Type

🔹 VARCHAR

##### 5.22.3.5.3 Is Required

✅ Yes

##### 5.22.3.5.4 Is Primary Key

❌ No

##### 5.22.3.5.5 Is Unique

❌ No

##### 5.22.3.5.6 Index Type

Index

##### 5.22.3.5.7 Size

50

##### 5.22.3.5.8 Constraints

- ENUM('QRCode', 'ImageTarget', 'SpatialAnchor')

#### 5.22.3.6.0 VARCHAR

##### 5.22.3.6.1 Name

markerIdentifier

##### 5.22.3.6.2 Type

🔹 VARCHAR

##### 5.22.3.6.3 Is Required

✅ Yes

##### 5.22.3.6.4 Is Primary Key

❌ No

##### 5.22.3.6.5 Is Unique

❌ No

##### 5.22.3.6.6 Index Type

Index

##### 5.22.3.6.7 Size

512

#### 5.22.3.7.0 JSONB

##### 5.22.3.7.1 Name

visualizationConfiguration

##### 5.22.3.7.2 Type

🔹 JSONB

##### 5.22.3.7.3 Is Required

❌ No

##### 5.22.3.7.4 Is Primary Key

❌ No

##### 5.22.3.7.5 Is Unique

❌ No

##### 5.22.3.7.6 Index Type

None

##### 5.22.3.7.7 Comment

Defines how to render the data in AR, e.g., position offset, color coding.

#### 5.22.3.8.0 DateTimeOffset

##### 5.22.3.8.1 Name

createdAt

##### 5.22.3.8.2 Type

🔹 DateTimeOffset

##### 5.22.3.8.3 Is Required

✅ Yes

##### 5.22.3.8.4 Is Primary Key

❌ No

##### 5.22.3.8.5 Is Unique

❌ No

##### 5.22.3.8.6 Index Type

None

##### 5.22.3.8.7 Default Value

CURRENT_TIMESTAMP

### 5.22.4.0.0 Primary Keys

- arTagMappingId

### 5.22.5.0.0 Unique Constraints

- {'name': 'UC_ArTagMapping_Marker', 'columns': ['tenantId', 'markerType', 'markerIdentifier']}

### 5.22.6.0.0 Indexes

- {'name': 'IX_ArTagMapping_AssetId', 'columns': ['assetId'], 'type': 'BTree'}

## 5.23.0.0.0 DataImportJob

### 5.23.1.0.0 Name

DataImportJob

### 5.23.2.0.0 Description

Tracks the status and results of bulk data import operations. (REQ-DM-003)

### 5.23.3.0.0 Attributes

#### 5.23.3.1.0 Guid

##### 5.23.3.1.1 Name

dataImportJobId

##### 5.23.3.1.2 Type

🔹 Guid

##### 5.23.3.1.3 Is Required

✅ Yes

##### 5.23.3.1.4 Is Primary Key

✅ Yes

##### 5.23.3.1.5 Is Unique

✅ Yes

##### 5.23.3.1.6 Index Type

UniqueIndex

#### 5.23.3.2.0 Guid

##### 5.23.3.2.1 Name

tenantId

##### 5.23.3.2.2 Type

🔹 Guid

##### 5.23.3.2.3 Is Required

✅ Yes

##### 5.23.3.2.4 Is Primary Key

❌ No

##### 5.23.3.2.5 Is Unique

❌ No

##### 5.23.3.2.6 Index Type

Index

##### 5.23.3.2.7 Is Foreign Key

✅ Yes

#### 5.23.3.3.0 Guid

##### 5.23.3.3.1 Name

submittedByUserId

##### 5.23.3.3.2 Type

🔹 Guid

##### 5.23.3.3.3 Is Required

✅ Yes

##### 5.23.3.3.4 Is Primary Key

❌ No

##### 5.23.3.3.5 Is Unique

❌ No

##### 5.23.3.3.6 Index Type

Index

##### 5.23.3.3.7 Is Foreign Key

✅ Yes

#### 5.23.3.4.0 VARCHAR

##### 5.23.3.4.1 Name

jobType

##### 5.23.3.4.2 Type

🔹 VARCHAR

##### 5.23.3.4.3 Is Required

✅ Yes

##### 5.23.3.4.4 Is Primary Key

❌ No

##### 5.23.3.4.5 Is Unique

❌ No

##### 5.23.3.4.6 Index Type

Index

##### 5.23.3.4.7 Size

50

##### 5.23.3.4.8 Constraints

- ENUM('UserImport', 'AssetImport', 'TagImport', 'HistoricalDataImport')

#### 5.23.3.5.0 VARCHAR

##### 5.23.3.5.1 Name

status

##### 5.23.3.5.2 Type

🔹 VARCHAR

##### 5.23.3.5.3 Is Required

✅ Yes

##### 5.23.3.5.4 Is Primary Key

❌ No

##### 5.23.3.5.5 Is Unique

❌ No

##### 5.23.3.5.6 Index Type

Index

##### 5.23.3.5.7 Size

50

##### 5.23.3.5.8 Constraints

- ENUM('Pending', 'InProgress', 'Completed', 'Failed', 'CompletedWithErrors')

##### 5.23.3.5.9 Default Value

Pending

#### 5.23.3.6.0 VARCHAR

##### 5.23.3.6.1 Name

sourceFileName

##### 5.23.3.6.2 Type

🔹 VARCHAR

##### 5.23.3.6.3 Is Required

❌ No

##### 5.23.3.6.4 Is Primary Key

❌ No

##### 5.23.3.6.5 Is Unique

❌ No

##### 5.23.3.6.6 Index Type

None

##### 5.23.3.6.7 Size

255

#### 5.23.3.7.0 VARCHAR

##### 5.23.3.7.1 Name

storagePath

##### 5.23.3.7.2 Type

🔹 VARCHAR

##### 5.23.3.7.3 Is Required

✅ Yes

##### 5.23.3.7.4 Is Primary Key

❌ No

##### 5.23.3.7.5 Is Unique

❌ No

##### 5.23.3.7.6 Index Type

None

##### 5.23.3.7.7 Size

1,024

##### 5.23.3.7.8 Comment

Path to the import file in an object store like S3.

#### 5.23.3.8.0 JSONB

##### 5.23.3.8.1 Name

results

##### 5.23.3.8.2 Type

🔹 JSONB

##### 5.23.3.8.3 Is Required

❌ No

##### 5.23.3.8.4 Is Primary Key

❌ No

##### 5.23.3.8.5 Is Unique

❌ No

##### 5.23.3.8.6 Index Type

None

##### 5.23.3.8.7 Comment

Summary of results, e.g., rows processed, errors.

#### 5.23.3.9.0 DateTimeOffset

##### 5.23.3.9.1 Name

submittedAt

##### 5.23.3.9.2 Type

🔹 DateTimeOffset

##### 5.23.3.9.3 Is Required

✅ Yes

##### 5.23.3.9.4 Is Primary Key

❌ No

##### 5.23.3.9.5 Is Unique

❌ No

##### 5.23.3.9.6 Index Type

Index

##### 5.23.3.9.7 Default Value

CURRENT_TIMESTAMP

#### 5.23.3.10.0 DateTimeOffset

##### 5.23.3.10.1 Name

completedAt

##### 5.23.3.10.2 Type

🔹 DateTimeOffset

##### 5.23.3.10.3 Is Required

❌ No

##### 5.23.3.10.4 Is Primary Key

❌ No

##### 5.23.3.10.5 Is Unique

❌ No

##### 5.23.3.10.6 Index Type

None

### 5.23.4.0.0 Primary Keys

- dataImportJobId

### 5.23.5.0.0 Unique Constraints

*No items available*

### 5.23.6.0.0 Indexes

- {'name': 'IX_DataImportJob_Tenant_Status_Submitted', 'columns': ['tenantId', 'status', 'submittedAt'], 'type': 'BTree'}

# 6.0.0.0.0 Relations

## 6.1.0.0.0 OneToMany

### 6.1.1.0.0 Name

TenantUsers

### 6.1.2.0.0 Id

REL_TENANT_USER_001

### 6.1.3.0.0 Source Entity

Tenant

### 6.1.4.0.0 Target Entity

User

### 6.1.5.0.0 Type

🔹 OneToMany

### 6.1.6.0.0 Source Multiplicity

1

### 6.1.7.0.0 Target Multiplicity

0..*

### 6.1.8.0.0 Cascade Delete

❌ No

### 6.1.9.0.0 Is Identifying

❌ No

### 6.1.10.0.0 On Delete

Restrict

### 6.1.11.0.0 On Update

Cascade

## 6.2.0.0.0 OneToMany

### 6.2.1.0.0 Name

TenantOpcCoreClients

### 6.2.2.0.0 Id

REL_TENANT_OPCCLIENT_001

### 6.2.3.0.0 Source Entity

Tenant

### 6.2.4.0.0 Target Entity

OpcCoreClient

### 6.2.5.0.0 Type

🔹 OneToMany

### 6.2.6.0.0 Source Multiplicity

1

### 6.2.7.0.0 Target Multiplicity

0..*

### 6.2.8.0.0 Cascade Delete

❌ No

### 6.2.9.0.0 Is Identifying

❌ No

### 6.2.10.0.0 On Delete

Restrict

### 6.2.11.0.0 On Update

Cascade

## 6.3.0.0.0 OneToMany

### 6.3.1.0.0 Name

TenantAssets

### 6.3.2.0.0 Id

REL_TENANT_ASSET_001

### 6.3.3.0.0 Source Entity

Tenant

### 6.3.4.0.0 Target Entity

Asset

### 6.3.5.0.0 Type

🔹 OneToMany

### 6.3.6.0.0 Source Multiplicity

1

### 6.3.7.0.0 Target Multiplicity

0..*

### 6.3.8.0.0 Cascade Delete

❌ No

### 6.3.9.0.0 Is Identifying

❌ No

### 6.3.10.0.0 On Delete

Restrict

### 6.3.11.0.0 On Update

Cascade

## 6.4.0.0.0 OneToMany

### 6.4.1.0.0 Name

TenantAuditLogs

### 6.4.2.0.0 Id

REL_TENANT_AUDITLOG_001

### 6.4.3.0.0 Source Entity

Tenant

### 6.4.4.0.0 Target Entity

AuditLog

### 6.4.5.0.0 Type

🔹 OneToMany

### 6.4.6.0.0 Source Multiplicity

1

### 6.4.7.0.0 Target Multiplicity

0..*

### 6.4.8.0.0 Cascade Delete

✅ Yes

### 6.4.9.0.0 Is Identifying

✅ Yes

### 6.4.10.0.0 On Delete

Cascade

### 6.4.11.0.0 On Update

Cascade

## 6.5.0.0.0 OneToMany

### 6.5.1.0.0 Name

UserAuditLogs

### 6.5.2.0.0 Id

REL_USER_AUDITLOG_001

### 6.5.3.0.0 Source Entity

User

### 6.5.4.0.0 Target Entity

AuditLog

### 6.5.5.0.0 Type

🔹 OneToMany

### 6.5.6.0.0 Source Multiplicity

1

### 6.5.7.0.0 Target Multiplicity

0..*

### 6.5.8.0.0 Cascade Delete

❌ No

### 6.5.9.0.0 Is Identifying

❌ No

### 6.5.10.0.0 On Delete

SetNull

### 6.5.11.0.0 On Update

Cascade

## 6.6.0.0.0 ManyToMany

### 6.6.1.0.0 Name

UserHasRoles

### 6.6.2.0.0 Id

REL_USER_ROLE_001

### 6.6.3.0.0 Source Entity

User

### 6.6.4.0.0 Target Entity

Role

### 6.6.5.0.0 Type

🔹 ManyToMany

### 6.6.6.0.0 Source Multiplicity

0..*

### 6.6.7.0.0 Target Multiplicity

0..*

### 6.6.8.0.0 Cascade Delete

❌ No

### 6.6.9.0.0 Is Identifying

❌ No

### 6.6.10.0.0 On Delete

Cascade

### 6.6.11.0.0 On Update

Cascade

### 6.6.12.0.0 Join Table

#### 6.6.12.1.0 Name

UserRole

#### 6.6.12.2.0 Columns

##### 6.6.12.2.1 userId

###### 6.6.12.2.1.1 Name

userId

###### 6.6.12.2.1.2 References

User.userId

##### 6.6.12.2.2.0 roleId

###### 6.6.12.2.2.1 Name

roleId

###### 6.6.12.2.2.2 References

Role.roleId

##### 6.6.12.2.3.0 assetScopeId

###### 6.6.12.2.3.1 Name

assetScopeId

###### 6.6.12.2.3.2 References

Asset.assetId

## 6.7.0.0.0.0 OneToMany

### 6.7.1.0.0.0 Name

ClientServerConnections

### 6.7.2.0.0.0 Id

REL_OPCCLIENT_OPCSERVER_001

### 6.7.3.0.0.0 Source Entity

OpcCoreClient

### 6.7.4.0.0.0 Target Entity

OpcServerConnection

### 6.7.5.0.0.0 Type

🔹 OneToMany

### 6.7.6.0.0.0 Source Multiplicity

1

### 6.7.7.0.0.0 Target Multiplicity

0..*

### 6.7.8.0.0.0 Cascade Delete

✅ Yes

### 6.7.9.0.0.0 Is Identifying

✅ Yes

### 6.7.10.0.0.0 On Delete

Cascade

### 6.7.11.0.0.0 On Update

Cascade

## 6.8.0.0.0.0 OneToOne

### 6.8.1.0.0.0 Name

OpcServerRedundancy

### 6.8.2.0.0.0 Id

REL_OPCSERVER_REDUNDANCY_001

### 6.8.3.0.0.0 Source Entity

OpcServerConnection

### 6.8.4.0.0.0 Target Entity

OpcServerConnection

### 6.8.5.0.0.0 Type

🔹 OneToOne

### 6.8.6.0.0.0 Source Multiplicity

0..1

### 6.8.7.0.0.0 Target Multiplicity

0..1

### 6.8.8.0.0.0 Cascade Delete

❌ No

### 6.8.9.0.0.0 Is Identifying

❌ No

### 6.8.10.0.0.0 On Delete

SetNull

### 6.8.11.0.0.0 On Update

Cascade

## 6.9.0.0.0.0 OneToMany

### 6.9.1.0.0.0 Name

AssetHierarchy

### 6.9.2.0.0.0 Id

REL_ASSET_HIERARCHY_001

### 6.9.3.0.0.0 Source Entity

Asset

### 6.9.4.0.0.0 Target Entity

Asset

### 6.9.5.0.0.0 Type

🔹 OneToMany

### 6.9.6.0.0.0 Source Multiplicity

0..1

### 6.9.7.0.0.0 Target Multiplicity

0..*

### 6.9.8.0.0.0 Cascade Delete

❌ No

### 6.9.9.0.0.0 Is Identifying

❌ No

### 6.9.10.0.0.0 On Delete

Restrict

### 6.9.11.0.0.0 On Update

Cascade

## 6.10.0.0.0.0 OneToMany

### 6.10.1.0.0.0 Name

AssetOpcTags

### 6.10.2.0.0.0 Id

REL_ASSET_OPCTAG_001

### 6.10.3.0.0.0 Source Entity

Asset

### 6.10.4.0.0.0 Target Entity

OpcTag

### 6.10.5.0.0.0 Type

🔹 OneToMany

### 6.10.6.0.0.0 Source Multiplicity

1

### 6.10.7.0.0.0 Target Multiplicity

0..*

### 6.10.8.0.0.0 Cascade Delete

✅ Yes

### 6.10.9.0.0.0 Is Identifying

✅ Yes

### 6.10.10.0.0.0 On Delete

Cascade

### 6.10.11.0.0.0 On Update

Cascade

## 6.11.0.0.0.0 OneToMany

### 6.11.1.0.0.0 Name

ConnectionOpcTags

### 6.11.2.0.0.0 Id

REL_OPCSERVER_OPCTAG_001

### 6.11.3.0.0.0 Source Entity

OpcServerConnection

### 6.11.4.0.0.0 Target Entity

OpcTag

### 6.11.5.0.0.0 Type

🔹 OneToMany

### 6.11.6.0.0.0 Source Multiplicity

1

### 6.11.7.0.0.0 Target Multiplicity

0..*

### 6.11.8.0.0.0 Cascade Delete

❌ No

### 6.11.9.0.0.0 Is Identifying

❌ No

### 6.11.10.0.0.0 On Delete

Restrict

### 6.11.11.0.0.0 On Update

Cascade

## 6.12.0.0.0.0 OneToMany

### 6.12.1.0.0.0 Name

OpcTagDataPoints

### 6.12.2.0.0.0 Id

REL_OPCTAG_DATAPOINT_001

### 6.12.3.0.0.0 Source Entity

OpcTag

### 6.12.4.0.0.0 Target Entity

TagDataPoint

### 6.12.5.0.0.0 Type

🔹 OneToMany

### 6.12.6.0.0.0 Source Multiplicity

1

### 6.12.7.0.0.0 Target Multiplicity

0..*

### 6.12.8.0.0.0 Cascade Delete

✅ Yes

### 6.12.9.0.0.0 Is Identifying

✅ Yes

### 6.12.10.0.0.0 On Delete

Cascade

### 6.12.11.0.0.0 On Update

Cascade

## 6.13.0.0.0.0 OneToMany

### 6.13.1.0.0.0 Name

OpcTagAlarms

### 6.13.2.0.0.0 Id

REL_OPCTAG_ALARM_001

### 6.13.3.0.0.0 Source Entity

OpcTag

### 6.13.4.0.0.0 Target Entity

Alarm

### 6.13.5.0.0.0 Type

🔹 OneToMany

### 6.13.6.0.0.0 Source Multiplicity

1

### 6.13.7.0.0.0 Target Multiplicity

0..*

### 6.13.8.0.0.0 Cascade Delete

✅ Yes

### 6.13.9.0.0.0 Is Identifying

❌ No

### 6.13.10.0.0.0 On Delete

Cascade

### 6.13.11.0.0.0 On Update

Cascade

## 6.14.0.0.0.0 OneToMany

### 6.14.1.0.0.0 Name

AlarmHistoryEntries

### 6.14.2.0.0.0 Id

REL_ALARM_ALARMHISTORY_001

### 6.14.3.0.0.0 Source Entity

Alarm

### 6.14.4.0.0.0 Target Entity

AlarmHistory

### 6.14.5.0.0.0 Type

🔹 OneToMany

### 6.14.6.0.0.0 Source Multiplicity

1

### 6.14.7.0.0.0 Target Multiplicity

1..*

### 6.14.8.0.0.0 Cascade Delete

✅ Yes

### 6.14.9.0.0.0 Is Identifying

✅ Yes

### 6.14.10.0.0.0 On Delete

Cascade

### 6.14.11.0.0.0 On Update

Cascade

## 6.15.0.0.0.0 OneToMany

### 6.15.1.0.0.0 Name

UserAlarmActions

### 6.15.2.0.0.0 Id

REL_USER_ALARMHISTORY_001

### 6.15.3.0.0.0 Source Entity

User

### 6.15.4.0.0.0 Target Entity

AlarmHistory

### 6.15.5.0.0.0 Type

🔹 OneToMany

### 6.15.6.0.0.0 Source Multiplicity

1

### 6.15.7.0.0.0 Target Multiplicity

0..*

### 6.15.8.0.0.0 Cascade Delete

❌ No

### 6.15.9.0.0.0 Is Identifying

❌ No

### 6.15.10.0.0.0 On Delete

Restrict

### 6.15.11.0.0.0 On Update

Cascade

## 6.16.0.0.0.0 OneToMany

### 6.16.1.0.0.0 Name

TenantAiModels

### 6.16.2.0.0.0 Id

REL_TENANT_AIMODEL_001

### 6.16.3.0.0.0 Source Entity

Tenant

### 6.16.4.0.0.0 Target Entity

AiModel

### 6.16.5.0.0.0 Type

🔹 OneToMany

### 6.16.6.0.0.0 Source Multiplicity

1

### 6.16.7.0.0.0 Target Multiplicity

0..*

### 6.16.8.0.0.0 Cascade Delete

❌ No

### 6.16.9.0.0.0 Is Identifying

❌ No

### 6.16.10.0.0.0 On Delete

Restrict

### 6.16.11.0.0.0 On Update

Cascade

## 6.17.0.0.0.0 OneToMany

### 6.17.1.0.0.0 Name

AiModelVersions

### 6.17.2.0.0.0 Id

REL_AIMODEL_VERSION_001

### 6.17.3.0.0.0 Source Entity

AiModel

### 6.17.4.0.0.0 Target Entity

AiModelVersion

### 6.17.5.0.0.0 Type

🔹 OneToMany

### 6.17.6.0.0.0 Source Multiplicity

1

### 6.17.7.0.0.0 Target Multiplicity

1..*

### 6.17.8.0.0.0 Cascade Delete

✅ Yes

### 6.17.9.0.0.0 Is Identifying

✅ Yes

### 6.17.10.0.0.0 On Delete

Cascade

### 6.17.11.0.0.0 On Update

Cascade

## 6.18.0.0.0.0 OneToMany

### 6.18.1.0.0.0 Name

UserSubmittedModelVersions

### 6.18.2.0.0.0 Id

REL_USER_SUBMITTEDMODEL_001

### 6.18.3.0.0.0 Source Entity

User

### 6.18.4.0.0.0 Target Entity

AiModelVersion

### 6.18.5.0.0.0 Type

🔹 OneToMany

### 6.18.6.0.0.0 Source Multiplicity

1

### 6.18.7.0.0.0 Target Multiplicity

0..*

### 6.18.8.0.0.0 Cascade Delete

❌ No

### 6.18.9.0.0.0 Is Identifying

❌ No

### 6.18.10.0.0.0 On Delete

SetNull

### 6.18.11.0.0.0 On Update

Cascade

## 6.19.0.0.0.0 OneToMany

### 6.19.1.0.0.0 Name

UserApprovedModelVersions

### 6.19.2.0.0.0 Id

REL_USER_APPROVEDMODEL_001

### 6.19.3.0.0.0 Source Entity

User

### 6.19.4.0.0.0 Target Entity

AiModelVersion

### 6.19.5.0.0.0 Type

🔹 OneToMany

### 6.19.6.0.0.0 Source Multiplicity

1

### 6.19.7.0.0.0 Target Multiplicity

0..*

### 6.19.8.0.0.0 Cascade Delete

❌ No

### 6.19.9.0.0.0 Is Identifying

❌ No

### 6.19.10.0.0.0 On Delete

SetNull

### 6.19.11.0.0.0 On Update

Cascade

## 6.20.0.0.0.0 OneToMany

### 6.20.1.0.0.0 Name

AssetModelAssignments

### 6.20.2.0.0.0 Id

REL_ASSET_MODELASSIGN_001

### 6.20.3.0.0.0 Source Entity

Asset

### 6.20.4.0.0.0 Target Entity

ModelAssignment

### 6.20.5.0.0.0 Type

🔹 OneToMany

### 6.20.6.0.0.0 Source Multiplicity

1

### 6.20.7.0.0.0 Target Multiplicity

0..*

### 6.20.8.0.0.0 Cascade Delete

✅ Yes

### 6.20.9.0.0.0 Is Identifying

❌ No

### 6.20.10.0.0.0 On Delete

Cascade

### 6.20.11.0.0.0 On Update

Cascade

## 6.21.0.0.0.0 OneToMany

### 6.21.1.0.0.0 Name

VersionModelAssignments

### 6.21.2.0.0.0 Id

REL_VERSION_MODELASSIGN_001

### 6.21.3.0.0.0 Source Entity

AiModelVersion

### 6.21.4.0.0.0 Target Entity

ModelAssignment

### 6.21.5.0.0.0 Type

🔹 OneToMany

### 6.21.6.0.0.0 Source Multiplicity

1

### 6.21.7.0.0.0 Target Multiplicity

0..*

### 6.21.8.0.0.0 Cascade Delete

❌ No

### 6.21.9.0.0.0 Is Identifying

❌ No

### 6.21.10.0.0.0 On Delete

Restrict

### 6.21.11.0.0.0 On Update

Cascade

## 6.22.0.0.0.0 OneToMany

### 6.22.1.0.0.0 Name

ClientModelAssignments

### 6.22.2.0.0.0 Id

REL_OPCCLIENT_MODELASSIGN_001

### 6.22.3.0.0.0 Source Entity

OpcCoreClient

### 6.22.4.0.0.0 Target Entity

ModelAssignment

### 6.22.5.0.0.0 Type

🔹 OneToMany

### 6.22.6.0.0.0 Source Multiplicity

1

### 6.22.7.0.0.0 Target Multiplicity

0..*

### 6.22.8.0.0.0 Cascade Delete

❌ No

### 6.22.9.0.0.0 Is Identifying

❌ No

### 6.22.10.0.0.0 On Delete

Restrict

### 6.22.11.0.0.0 On Update

Cascade

## 6.23.0.0.0.0 OneToMany

### 6.23.1.0.0.0 Name

AssignmentAnomalyEvents

### 6.23.2.0.0.0 Id

REL_ASSIGNMENT_ANOMALY_001

### 6.23.3.0.0.0 Source Entity

ModelAssignment

### 6.23.4.0.0.0 Target Entity

AnomalyEvent

### 6.23.5.0.0.0 Type

🔹 OneToMany

### 6.23.6.0.0.0 Source Multiplicity

1

### 6.23.7.0.0.0 Target Multiplicity

0..*

### 6.23.8.0.0.0 Cascade Delete

✅ Yes

### 6.23.9.0.0.0 Is Identifying

✅ Yes

### 6.23.10.0.0.0 On Delete

Cascade

### 6.23.11.0.0.0 On Update

Cascade

## 6.24.0.0.0.0 OneToOne

### 6.24.1.0.0.0 Name

TenantLicense

### 6.24.2.0.0.0 Id

REL_TENANT_LICENSE_001

### 6.24.3.0.0.0 Source Entity

Tenant

### 6.24.4.0.0.0 Target Entity

License

### 6.24.5.0.0.0 Type

🔹 OneToOne

### 6.24.6.0.0.0 Source Multiplicity

1

### 6.24.7.0.0.0 Target Multiplicity

1

### 6.24.8.0.0.0 Cascade Delete

✅ Yes

### 6.24.9.0.0.0 Is Identifying

✅ Yes

### 6.24.10.0.0.0 On Delete

Cascade

### 6.24.11.0.0.0 On Update

Cascade

## 6.25.0.0.0.0 OneToMany

### 6.25.1.0.0.0 Name

TenantReportTemplates

### 6.25.2.0.0.0 Id

REL_TENANT_REPORTTEMPLATE_001

### 6.25.3.0.0.0 Source Entity

Tenant

### 6.25.4.0.0.0 Target Entity

ReportTemplate

### 6.25.5.0.0.0 Type

🔹 OneToMany

### 6.25.6.0.0.0 Source Multiplicity

1

### 6.25.7.0.0.0 Target Multiplicity

0..*

### 6.25.8.0.0.0 Cascade Delete

✅ Yes

### 6.25.9.0.0.0 Is Identifying

❌ No

### 6.25.10.0.0.0 On Delete

Cascade

### 6.25.11.0.0.0 On Update

Cascade

## 6.26.0.0.0.0 OneToMany

### 6.26.1.0.0.0 Name

TenantApprovalRequests

### 6.26.2.0.0.0 Id

REL_TENANT_APPROVAL_001

### 6.26.3.0.0.0 Source Entity

Tenant

### 6.26.4.0.0.0 Target Entity

ApprovalRequest

### 6.26.5.0.0.0 Type

🔹 OneToMany

### 6.26.6.0.0.0 Source Multiplicity

1

### 6.26.7.0.0.0 Target Multiplicity

0..*

### 6.26.8.0.0.0 Cascade Delete

✅ Yes

### 6.26.9.0.0.0 Is Identifying

❌ No

### 6.26.10.0.0.0 On Delete

Cascade

### 6.26.11.0.0.0 On Update

Cascade

## 6.27.0.0.0.0 OneToMany

### 6.27.1.0.0.0 Name

UserRequestedApprovals

### 6.27.2.0.0.0 Id

REL_USER_REQUESTEDAPPROVAL_001

### 6.27.3.0.0.0 Source Entity

User

### 6.27.4.0.0.0 Target Entity

ApprovalRequest

### 6.27.5.0.0.0 Type

🔹 OneToMany

### 6.27.6.0.0.0 Source Multiplicity

1

### 6.27.7.0.0.0 Target Multiplicity

0..*

### 6.27.8.0.0.0 Cascade Delete

❌ No

### 6.27.9.0.0.0 Is Identifying

❌ No

### 6.27.10.0.0.0 On Delete

Restrict

### 6.27.11.0.0.0 On Update

Cascade

## 6.28.0.0.0.0 OneToMany

### 6.28.1.0.0.0 Name

UserApprovedApprovals

### 6.28.2.0.0.0 Id

REL_USER_APPROVEDAPPROVAL_001

### 6.28.3.0.0.0 Source Entity

User

### 6.28.4.0.0.0 Target Entity

ApprovalRequest

### 6.28.5.0.0.0 Type

🔹 OneToMany

### 6.28.6.0.0.0 Source Multiplicity

1

### 6.28.7.0.0.0 Target Multiplicity

0..*

### 6.28.8.0.0.0 Cascade Delete

❌ No

### 6.28.9.0.0.0 Is Identifying

❌ No

### 6.28.10.0.0.0 On Delete

SetNull

### 6.28.11.0.0.0 On Update

Cascade

## 6.29.0.0.0.0 OneToMany

### 6.29.1.0.0.0 Name

AssetRoleScope

### 6.29.2.0.0.0 Id

REL_ASSET_ROLESCOPE_001

### 6.29.3.0.0.0 Source Entity

Asset

### 6.29.4.0.0.0 Target Entity

UserRole

### 6.29.5.0.0.0 Type

🔹 OneToMany

### 6.29.6.0.0.0 Source Multiplicity

1

### 6.29.7.0.0.0 Target Multiplicity

0..*

### 6.29.8.0.0.0 Cascade Delete

❌ No

### 6.29.9.0.0.0 Is Identifying

❌ No

### 6.29.10.0.0.0 On Delete

SetNull

### 6.29.11.0.0.0 On Update

Cascade

## 6.30.0.0.0.0 OneToMany

### 6.30.1.0.0.0 Name

TenantDashboards

### 6.30.2.0.0.0 Id

REL_TENANT_DASHBOARD_001

### 6.30.3.0.0.0 Source Entity

Tenant

### 6.30.4.0.0.0 Target Entity

Dashboard

### 6.30.5.0.0.0 Type

🔹 OneToMany

### 6.30.6.0.0.0 Source Multiplicity

1

### 6.30.7.0.0.0 Target Multiplicity

0..*

### 6.30.8.0.0.0 Cascade Delete

✅ Yes

### 6.30.9.0.0.0 Is Identifying

❌ No

### 6.30.10.0.0.0 On Delete

Cascade

### 6.30.11.0.0.0 On Update

Cascade

## 6.31.0.0.0.0 OneToMany

### 6.31.1.0.0.0 Name

UserDashboards

### 6.31.2.0.0.0 Id

REL_USER_DASHBOARD_001

### 6.31.3.0.0.0 Source Entity

User

### 6.31.4.0.0.0 Target Entity

Dashboard

### 6.31.5.0.0.0 Type

🔹 OneToMany

### 6.31.6.0.0.0 Source Multiplicity

1

### 6.31.7.0.0.0 Target Multiplicity

0..*

### 6.31.8.0.0.0 Cascade Delete

✅ Yes

### 6.31.9.0.0.0 Is Identifying

✅ Yes

### 6.31.10.0.0.0 On Delete

Cascade

### 6.31.11.0.0.0 On Update

Cascade

## 6.32.0.0.0.0 OneToMany

### 6.32.1.0.0.0 Name

DashboardWidgets

### 6.32.2.0.0.0 Id

REL_DASHBOARD_WIDGET_001

### 6.32.3.0.0.0 Source Entity

Dashboard

### 6.32.4.0.0.0 Target Entity

Widget

### 6.32.5.0.0.0 Type

🔹 OneToMany

### 6.32.6.0.0.0 Source Multiplicity

1

### 6.32.7.0.0.0 Target Multiplicity

0..*

### 6.32.8.0.0.0 Cascade Delete

✅ Yes

### 6.32.9.0.0.0 Is Identifying

✅ Yes

### 6.32.10.0.0.0 On Delete

Cascade

### 6.32.11.0.0.0 On Update

Cascade

## 6.33.0.0.0.0 OneToMany

### 6.33.1.0.0.0 Name

TenantArTagMappings

### 6.33.2.0.0.0 Id

REL_TENANT_ARTAGMAP_001

### 6.33.3.0.0.0 Source Entity

Tenant

### 6.33.4.0.0.0 Target Entity

ArTagMapping

### 6.33.5.0.0.0 Type

🔹 OneToMany

### 6.33.6.0.0.0 Source Multiplicity

1

### 6.33.7.0.0.0 Target Multiplicity

0..*

### 6.33.8.0.0.0 Cascade Delete

✅ Yes

### 6.33.9.0.0.0 Is Identifying

❌ No

### 6.33.10.0.0.0 On Delete

Cascade

### 6.33.11.0.0.0 On Update

Cascade

## 6.34.0.0.0.0 OneToMany

### 6.34.1.0.0.0 Name

AssetArTagMappings

### 6.34.2.0.0.0 Id

REL_ASSET_ARTAGMAP_001

### 6.34.3.0.0.0 Source Entity

Asset

### 6.34.4.0.0.0 Target Entity

ArTagMapping

### 6.34.5.0.0.0 Type

🔹 OneToMany

### 6.34.6.0.0.0 Source Multiplicity

1

### 6.34.7.0.0.0 Target Multiplicity

0..*

### 6.34.8.0.0.0 Cascade Delete

✅ Yes

### 6.34.9.0.0.0 Is Identifying

❌ No

### 6.34.10.0.0.0 On Delete

Cascade

### 6.34.11.0.0.0 On Update

Cascade

## 6.35.0.0.0.0 OneToMany

### 6.35.1.0.0.0 Name

OpcTagArTagMappings

### 6.35.2.0.0.0 Id

REL_OPCTAG_ARTAGMAP_001

### 6.35.3.0.0.0 Source Entity

OpcTag

### 6.35.4.0.0.0 Target Entity

ArTagMapping

### 6.35.5.0.0.0 Type

🔹 OneToMany

### 6.35.6.0.0.0 Source Multiplicity

1

### 6.35.7.0.0.0 Target Multiplicity

0..*

### 6.35.8.0.0.0 Cascade Delete

✅ Yes

### 6.35.9.0.0.0 Is Identifying

❌ No

### 6.35.10.0.0.0 On Delete

Cascade

### 6.35.11.0.0.0 On Update

Cascade

## 6.36.0.0.0.0 OneToMany

### 6.36.1.0.0.0 Name

TenantDataImportJobs

### 6.36.2.0.0.0 Id

REL_TENANT_IMPORTJOB_001

### 6.36.3.0.0.0 Source Entity

Tenant

### 6.36.4.0.0.0 Target Entity

DataImportJob

### 6.36.5.0.0.0 Type

🔹 OneToMany

### 6.36.6.0.0.0 Source Multiplicity

1

### 6.36.7.0.0.0 Target Multiplicity

0..*

### 6.36.8.0.0.0 Cascade Delete

✅ Yes

### 6.36.9.0.0.0 Is Identifying

❌ No

### 6.36.10.0.0.0 On Delete

Cascade

### 6.36.11.0.0.0 On Update

Cascade

## 6.37.0.0.0.0 OneToMany

### 6.37.1.0.0.0 Name

UserDataImportJobs

### 6.37.2.0.0.0 Id

REL_USER_IMPORTJOB_001

### 6.37.3.0.0.0 Source Entity

User

### 6.37.4.0.0.0 Target Entity

DataImportJob

### 6.37.5.0.0.0 Type

🔹 OneToMany

### 6.37.6.0.0.0 Source Multiplicity

1

### 6.37.7.0.0.0 Target Multiplicity

0..*

### 6.37.8.0.0.0 Cascade Delete

❌ No

### 6.37.9.0.0.0 Is Identifying

❌ No

### 6.37.10.0.0.0 On Delete

Restrict

### 6.37.11.0.0.0 On Update

Cascade

