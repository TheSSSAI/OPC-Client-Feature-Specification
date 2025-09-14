# 1 Entities

## 1.1 Tenant

### 1.1.1 Name

Tenant

### 1.1.2 Description

Represents a customer organization, providing top-level data isolation. (REQ-1-024)

### 1.1.3 Attributes

#### 1.1.3.1 Guid

##### 1.1.3.1.1 Name

tenantId

##### 1.1.3.1.2 Type

🔹 Guid

##### 1.1.3.1.3 Is Required

✅ Yes

##### 1.1.3.1.4 Is Primary Key

✅ Yes

##### 1.1.3.1.5 Is Unique

✅ Yes

##### 1.1.3.1.6 Index Type

UniqueIndex

##### 1.1.3.1.7 Size

0

##### 1.1.3.1.8 Constraints

*No items available*

##### 1.1.3.1.9 Default Value



##### 1.1.3.1.10 Is Foreign Key

❌ No

##### 1.1.3.1.11 Precision

0

##### 1.1.3.1.12 Scale

0

#### 1.1.3.2.0 VARCHAR

##### 1.1.3.2.1 Name

name

##### 1.1.3.2.2 Type

🔹 VARCHAR

##### 1.1.3.2.3 Is Required

✅ Yes

##### 1.1.3.2.4 Is Primary Key

❌ No

##### 1.1.3.2.5 Is Unique

✅ Yes

##### 1.1.3.2.6 Index Type

UniqueIndex

##### 1.1.3.2.7 Size

255

##### 1.1.3.2.8 Constraints

*No items available*

##### 1.1.3.2.9 Default Value



##### 1.1.3.2.10 Is Foreign Key

❌ No

##### 1.1.3.2.11 Precision

0

##### 1.1.3.2.12 Scale

0

#### 1.1.3.3.0 VARCHAR

##### 1.1.3.3.1 Name

dataResidencyRegion

##### 1.1.3.3.2 Type

🔹 VARCHAR

##### 1.1.3.3.3 Is Required

✅ Yes

##### 1.1.3.3.4 Is Primary Key

❌ No

##### 1.1.3.3.5 Is Unique

❌ No

##### 1.1.3.3.6 Index Type

Index

##### 1.1.3.3.7 Size

50

##### 1.1.3.3.8 Constraints

*No items available*

##### 1.1.3.3.9 Default Value



##### 1.1.3.3.10 Is Foreign Key

❌ No

##### 1.1.3.3.11 Precision

0

##### 1.1.3.3.12 Scale

0

#### 1.1.3.4.0 VARCHAR

##### 1.1.3.4.1 Name

isolationModel

##### 1.1.3.4.2 Type

🔹 VARCHAR

##### 1.1.3.4.3 Is Required

✅ Yes

##### 1.1.3.4.4 Is Primary Key

❌ No

##### 1.1.3.4.5 Is Unique

❌ No

##### 1.1.3.4.6 Index Type

Index

##### 1.1.3.4.7 Size

50

##### 1.1.3.4.8 Constraints

- ENUM('RLS', 'SCHEMA')

##### 1.1.3.4.9 Default Value

RLS

##### 1.1.3.4.10 Is Foreign Key

❌ No

##### 1.1.3.4.11 Precision

0

##### 1.1.3.4.12 Scale

0

#### 1.1.3.5.0 BOOLEAN

##### 1.1.3.5.1 Name

isActive

##### 1.1.3.5.2 Type

🔹 BOOLEAN

##### 1.1.3.5.3 Is Required

✅ Yes

##### 1.1.3.5.4 Is Primary Key

❌ No

##### 1.1.3.5.5 Is Unique

❌ No

##### 1.1.3.5.6 Index Type

Index

##### 1.1.3.5.7 Size

0

##### 1.1.3.5.8 Constraints

*No items available*

##### 1.1.3.5.9 Default Value

true

##### 1.1.3.5.10 Is Foreign Key

❌ No

##### 1.1.3.5.11 Precision

0

##### 1.1.3.5.12 Scale

0

#### 1.1.3.6.0 DateTimeOffset

##### 1.1.3.6.1 Name

createdAt

##### 1.1.3.6.2 Type

🔹 DateTimeOffset

##### 1.1.3.6.3 Is Required

✅ Yes

##### 1.1.3.6.4 Is Primary Key

❌ No

##### 1.1.3.6.5 Is Unique

❌ No

##### 1.1.3.6.6 Index Type

Index

##### 1.1.3.6.7 Size

0

##### 1.1.3.6.8 Constraints

*No items available*

##### 1.1.3.6.9 Default Value

CURRENT_TIMESTAMP

##### 1.1.3.6.10 Is Foreign Key

❌ No

##### 1.1.3.6.11 Precision

0

##### 1.1.3.6.12 Scale

0

#### 1.1.3.7.0 DateTimeOffset

##### 1.1.3.7.1 Name

updatedAt

##### 1.1.3.7.2 Type

🔹 DateTimeOffset

##### 1.1.3.7.3 Is Required

✅ Yes

##### 1.1.3.7.4 Is Primary Key

❌ No

##### 1.1.3.7.5 Is Unique

❌ No

##### 1.1.3.7.6 Index Type

None

##### 1.1.3.7.7 Size

0

##### 1.1.3.7.8 Constraints

*No items available*

##### 1.1.3.7.9 Default Value

CURRENT_TIMESTAMP

##### 1.1.3.7.10 Is Foreign Key

❌ No

##### 1.1.3.7.11 Precision

0

##### 1.1.3.7.12 Scale

0

### 1.1.4.0.0 Primary Keys

- tenantId

### 1.1.5.0.0 Unique Constraints

- {'name': 'UC_Tenant_Name', 'columns': ['name']}

### 1.1.6.0.0 Indexes

#### 1.1.6.1.0 BTree

##### 1.1.6.1.1 Name

IX_Tenant_IsActive

##### 1.1.6.1.2 Columns

- isActive

##### 1.1.6.1.3 Type

🔹 BTree

#### 1.1.6.2.0 BTree

##### 1.1.6.2.1 Name

IX_Tenant_CreatedAt

##### 1.1.6.2.2 Columns

- createdAt

##### 1.1.6.2.3 Type

🔹 BTree

## 1.2.0.0.0 User

### 1.2.1.0.0 Name

User

### 1.2.2.0.0 Description

Represents a system user with profile information and credentials. (REQ-1-011)

### 1.2.3.0.0 Attributes

#### 1.2.3.1.0 Guid

##### 1.2.3.1.1 Name

userId

##### 1.2.3.1.2 Type

🔹 Guid

##### 1.2.3.1.3 Is Required

✅ Yes

##### 1.2.3.1.4 Is Primary Key

✅ Yes

##### 1.2.3.1.5 Is Unique

✅ Yes

##### 1.2.3.1.6 Index Type

UniqueIndex

##### 1.2.3.1.7 Size

0

##### 1.2.3.1.8 Constraints

*No items available*

##### 1.2.3.1.9 Default Value



##### 1.2.3.1.10 Is Foreign Key

❌ No

##### 1.2.3.1.11 Precision

0

##### 1.2.3.1.12 Scale

0

#### 1.2.3.2.0 Guid

##### 1.2.3.2.1 Name

tenantId

##### 1.2.3.2.2 Type

🔹 Guid

##### 1.2.3.2.3 Is Required

✅ Yes

##### 1.2.3.2.4 Is Primary Key

❌ No

##### 1.2.3.2.5 Is Unique

❌ No

##### 1.2.3.2.6 Index Type

Index

##### 1.2.3.2.7 Size

0

##### 1.2.3.2.8 Constraints

*No items available*

##### 1.2.3.2.9 Default Value



##### 1.2.3.2.10 Is Foreign Key

✅ Yes

##### 1.2.3.2.11 Precision

0

##### 1.2.3.2.12 Scale

0

#### 1.2.3.3.0 VARCHAR

##### 1.2.3.3.1 Name

email

##### 1.2.3.3.2 Type

🔹 VARCHAR

##### 1.2.3.3.3 Is Required

✅ Yes

##### 1.2.3.3.4 Is Primary Key

❌ No

##### 1.2.3.3.5 Is Unique

✅ Yes

##### 1.2.3.3.6 Index Type

UniqueIndex

##### 1.2.3.3.7 Size

255

##### 1.2.3.3.8 Constraints

- EMAIL_FORMAT

##### 1.2.3.3.9 Default Value



##### 1.2.3.3.10 Is Foreign Key

❌ No

##### 1.2.3.3.11 Precision

0

##### 1.2.3.3.12 Scale

0

#### 1.2.3.4.0 VARCHAR

##### 1.2.3.4.1 Name

firstName

##### 1.2.3.4.2 Type

🔹 VARCHAR

##### 1.2.3.4.3 Is Required

✅ Yes

##### 1.2.3.4.4 Is Primary Key

❌ No

##### 1.2.3.4.5 Is Unique

❌ No

##### 1.2.3.4.6 Index Type

Index

##### 1.2.3.4.7 Size

100

##### 1.2.3.4.8 Constraints

*No items available*

##### 1.2.3.4.9 Default Value



##### 1.2.3.4.10 Is Foreign Key

❌ No

##### 1.2.3.4.11 Precision

0

##### 1.2.3.4.12 Scale

0

#### 1.2.3.5.0 VARCHAR

##### 1.2.3.5.1 Name

lastName

##### 1.2.3.5.2 Type

🔹 VARCHAR

##### 1.2.3.5.3 Is Required

✅ Yes

##### 1.2.3.5.4 Is Primary Key

❌ No

##### 1.2.3.5.5 Is Unique

❌ No

##### 1.2.3.5.6 Index Type

Index

##### 1.2.3.5.7 Size

100

##### 1.2.3.5.8 Constraints

*No items available*

##### 1.2.3.5.9 Default Value



##### 1.2.3.5.10 Is Foreign Key

❌ No

##### 1.2.3.5.11 Precision

0

##### 1.2.3.5.12 Scale

0

#### 1.2.3.6.0 VARCHAR

##### 1.2.3.6.1 Name

identityProviderId

##### 1.2.3.6.2 Type

🔹 VARCHAR

##### 1.2.3.6.3 Is Required

✅ Yes

##### 1.2.3.6.4 Is Primary Key

❌ No

##### 1.2.3.6.5 Is Unique

✅ Yes

##### 1.2.3.6.6 Index Type

UniqueIndex

##### 1.2.3.6.7 Size

255

##### 1.2.3.6.8 Constraints

*No items available*

##### 1.2.3.6.9 Default Value



##### 1.2.3.6.10 Is Foreign Key

❌ No

##### 1.2.3.6.11 Precision

0

##### 1.2.3.6.12 Scale

0

#### 1.2.3.7.0 JSONB

##### 1.2.3.7.1 Name

notificationPreferences

##### 1.2.3.7.2 Type

🔹 JSONB

##### 1.2.3.7.3 Is Required

❌ No

##### 1.2.3.7.4 Is Primary Key

❌ No

##### 1.2.3.7.5 Is Unique

❌ No

##### 1.2.3.7.6 Index Type

None

##### 1.2.3.7.7 Size

0

##### 1.2.3.7.8 Constraints

*No items available*

##### 1.2.3.7.9 Default Value

{}

##### 1.2.3.7.10 Is Foreign Key

❌ No

##### 1.2.3.7.11 Precision

0

##### 1.2.3.7.12 Scale

0

#### 1.2.3.8.0 BOOLEAN

##### 1.2.3.8.1 Name

isActive

##### 1.2.3.8.2 Type

🔹 BOOLEAN

##### 1.2.3.8.3 Is Required

✅ Yes

##### 1.2.3.8.4 Is Primary Key

❌ No

##### 1.2.3.8.5 Is Unique

❌ No

##### 1.2.3.8.6 Index Type

Index

##### 1.2.3.8.7 Size

0

##### 1.2.3.8.8 Constraints

*No items available*

##### 1.2.3.8.9 Default Value

true

##### 1.2.3.8.10 Is Foreign Key

❌ No

##### 1.2.3.8.11 Precision

0

##### 1.2.3.8.12 Scale

0

#### 1.2.3.9.0 DateTimeOffset

##### 1.2.3.9.1 Name

createdAt

##### 1.2.3.9.2 Type

🔹 DateTimeOffset

##### 1.2.3.9.3 Is Required

✅ Yes

##### 1.2.3.9.4 Is Primary Key

❌ No

##### 1.2.3.9.5 Is Unique

❌ No

##### 1.2.3.9.6 Index Type

Index

##### 1.2.3.9.7 Size

0

##### 1.2.3.9.8 Constraints

*No items available*

##### 1.2.3.9.9 Default Value

CURRENT_TIMESTAMP

##### 1.2.3.9.10 Is Foreign Key

❌ No

##### 1.2.3.9.11 Precision

0

##### 1.2.3.9.12 Scale

0

#### 1.2.3.10.0 DateTimeOffset

##### 1.2.3.10.1 Name

updatedAt

##### 1.2.3.10.2 Type

🔹 DateTimeOffset

##### 1.2.3.10.3 Is Required

✅ Yes

##### 1.2.3.10.4 Is Primary Key

❌ No

##### 1.2.3.10.5 Is Unique

❌ No

##### 1.2.3.10.6 Index Type

None

##### 1.2.3.10.7 Size

0

##### 1.2.3.10.8 Constraints

*No items available*

##### 1.2.3.10.9 Default Value

CURRENT_TIMESTAMP

##### 1.2.3.10.10 Is Foreign Key

❌ No

##### 1.2.3.10.11 Precision

0

##### 1.2.3.10.12 Scale

0

### 1.2.4.0.0 Primary Keys

- userId

### 1.2.5.0.0 Unique Constraints

#### 1.2.5.1.0 UC_User_Email

##### 1.2.5.1.1 Name

UC_User_Email

##### 1.2.5.1.2 Columns

- email

#### 1.2.5.2.0 UC_User_IdentityProviderId

##### 1.2.5.2.1 Name

UC_User_IdentityProviderId

##### 1.2.5.2.2 Columns

- identityProviderId

### 1.2.6.0.0 Indexes

#### 1.2.6.1.0 BTree

##### 1.2.6.1.1 Name

IX_User_TenantId

##### 1.2.6.1.2 Columns

- tenantId

##### 1.2.6.1.3 Type

🔹 BTree

#### 1.2.6.2.0 BTree

##### 1.2.6.2.1 Name

IX_User_Tenant_Active_Name

##### 1.2.6.2.2 Columns

- tenantId
- isActive
- lastName
- firstName

##### 1.2.6.2.3 Type

🔹 BTree

## 1.3.0.0.0 Role

### 1.3.1.0.0 Name

Role

### 1.3.2.0.0 Description

Defines a set of permissions for users (e.g., Administrator, Operator). (REQ-1-011)

### 1.3.3.0.0 Attributes

#### 1.3.3.1.0 Guid

##### 1.3.3.1.1 Name

roleId

##### 1.3.3.1.2 Type

🔹 Guid

##### 1.3.3.1.3 Is Required

✅ Yes

##### 1.3.3.1.4 Is Primary Key

✅ Yes

##### 1.3.3.1.5 Is Unique

✅ Yes

##### 1.3.3.1.6 Index Type

UniqueIndex

##### 1.3.3.1.7 Size

0

##### 1.3.3.1.8 Constraints

*No items available*

##### 1.3.3.1.9 Default Value



##### 1.3.3.1.10 Is Foreign Key

❌ No

##### 1.3.3.1.11 Precision

0

##### 1.3.3.1.12 Scale

0

#### 1.3.3.2.0 VARCHAR

##### 1.3.3.2.1 Name

name

##### 1.3.3.2.2 Type

🔹 VARCHAR

##### 1.3.3.2.3 Is Required

✅ Yes

##### 1.3.3.2.4 Is Primary Key

❌ No

##### 1.3.3.2.5 Is Unique

✅ Yes

##### 1.3.3.2.6 Index Type

UniqueIndex

##### 1.3.3.2.7 Size

50

##### 1.3.3.2.8 Constraints

*No items available*

##### 1.3.3.2.9 Default Value



##### 1.3.3.2.10 Is Foreign Key

❌ No

##### 1.3.3.2.11 Precision

0

##### 1.3.3.2.12 Scale

0

#### 1.3.3.3.0 TEXT

##### 1.3.3.3.1 Name

description

##### 1.3.3.3.2 Type

🔹 TEXT

##### 1.3.3.3.3 Is Required

❌ No

##### 1.3.3.3.4 Is Primary Key

❌ No

##### 1.3.3.3.5 Is Unique

❌ No

##### 1.3.3.3.6 Index Type

None

##### 1.3.3.3.7 Size

0

##### 1.3.3.3.8 Constraints

*No items available*

##### 1.3.3.3.9 Default Value



##### 1.3.3.3.10 Is Foreign Key

❌ No

##### 1.3.3.3.11 Precision

0

##### 1.3.3.3.12 Scale

0

#### 1.3.3.4.0 BOOLEAN

##### 1.3.3.4.1 Name

isSystemRole

##### 1.3.3.4.2 Type

🔹 BOOLEAN

##### 1.3.3.4.3 Is Required

✅ Yes

##### 1.3.3.4.4 Is Primary Key

❌ No

##### 1.3.3.4.5 Is Unique

❌ No

##### 1.3.3.4.6 Index Type

Index

##### 1.3.3.4.7 Size

0

##### 1.3.3.4.8 Constraints

*No items available*

##### 1.3.3.4.9 Default Value

false

##### 1.3.3.4.10 Is Foreign Key

❌ No

##### 1.3.3.4.11 Precision

0

##### 1.3.3.4.12 Scale

0

### 1.3.4.0.0 Primary Keys

- roleId

### 1.3.5.0.0 Unique Constraints

- {'name': 'UC_Role_Name', 'columns': ['name']}

### 1.3.6.0.0 Indexes

- {'name': 'IX_Role_IsSystemRole', 'columns': ['isSystemRole'], 'type': 'BTree'}

## 1.4.0.0.0 UserRole

### 1.4.1.0.0 Name

UserRole

### 1.4.2.0.0 Description

Junction table to assign Roles to Users, enabling many-to-many relationships. (REQ-1-061)

### 1.4.3.0.0 Attributes

#### 1.4.3.1.0 Guid

##### 1.4.3.1.1 Name

userId

##### 1.4.3.1.2 Type

🔹 Guid

##### 1.4.3.1.3 Is Required

✅ Yes

##### 1.4.3.1.4 Is Primary Key

✅ Yes

##### 1.4.3.1.5 Is Unique

❌ No

##### 1.4.3.1.6 Index Type

Index

##### 1.4.3.1.7 Size

0

##### 1.4.3.1.8 Constraints

*No items available*

##### 1.4.3.1.9 Default Value



##### 1.4.3.1.10 Is Foreign Key

✅ Yes

##### 1.4.3.1.11 Precision

0

##### 1.4.3.1.12 Scale

0

#### 1.4.3.2.0 Guid

##### 1.4.3.2.1 Name

roleId

##### 1.4.3.2.2 Type

🔹 Guid

##### 1.4.3.2.3 Is Required

✅ Yes

##### 1.4.3.2.4 Is Primary Key

✅ Yes

##### 1.4.3.2.5 Is Unique

❌ No

##### 1.4.3.2.6 Index Type

Index

##### 1.4.3.2.7 Size

0

##### 1.4.3.2.8 Constraints

*No items available*

##### 1.4.3.2.9 Default Value



##### 1.4.3.2.10 Is Foreign Key

✅ Yes

##### 1.4.3.2.11 Precision

0

##### 1.4.3.2.12 Scale

0

#### 1.4.3.3.0 Guid

##### 1.4.3.3.1 Name

assetScopeId

##### 1.4.3.3.2 Type

🔹 Guid

##### 1.4.3.3.3 Is Required

❌ No

##### 1.4.3.3.4 Is Primary Key

❌ No

##### 1.4.3.3.5 Is Unique

❌ No

##### 1.4.3.3.6 Index Type

Index

##### 1.4.3.3.7 Size

0

##### 1.4.3.3.8 Constraints

*No items available*

##### 1.4.3.3.9 Default Value



##### 1.4.3.3.10 Is Foreign Key

✅ Yes

##### 1.4.3.3.11 Precision

0

##### 1.4.3.3.12 Scale

0

#### 1.4.3.4.0 DateTimeOffset

##### 1.4.3.4.1 Name

assignedAt

##### 1.4.3.4.2 Type

🔹 DateTimeOffset

##### 1.4.3.4.3 Is Required

✅ Yes

##### 1.4.3.4.4 Is Primary Key

❌ No

##### 1.4.3.4.5 Is Unique

❌ No

##### 1.4.3.4.6 Index Type

None

##### 1.4.3.4.7 Size

0

##### 1.4.3.4.8 Constraints

*No items available*

##### 1.4.3.4.9 Default Value

CURRENT_TIMESTAMP

##### 1.4.3.4.10 Is Foreign Key

❌ No

##### 1.4.3.4.11 Precision

0

##### 1.4.3.4.12 Scale

0

### 1.4.4.0.0 Primary Keys

- userId
- roleId

### 1.4.5.0.0 Unique Constraints

*No items available*

### 1.4.6.0.0 Indexes

#### 1.4.6.1.0 BTree

##### 1.4.6.1.1 Name

IX_UserRole_RoleId

##### 1.4.6.1.2 Columns

- roleId

##### 1.4.6.1.3 Type

🔹 BTree

#### 1.4.6.2.0 BTree

##### 1.4.6.2.1 Name

IX_UserRole_AssetScopeId

##### 1.4.6.2.2 Columns

- assetScopeId

##### 1.4.6.2.3 Type

🔹 BTree

### 1.4.7.0.0 Caching

| Property | Value |
|----------|-------|
| Strategy | Redis |
| Key | user_permissions:{userId} |
| Invalidation | On user-permission changes. |
| Comment | Caches resolved user permissions for the duration ... |

## 1.5.0.0.0 AuditLog

### 1.5.1.0.0 Name

AuditLog

### 1.5.2.0.0 Description

Stores a tamper-evident record of all significant user and system actions. (REQ-1-040, REQ-1-023)

### 1.5.3.0.0 Attributes

#### 1.5.3.1.0 BIGINT

##### 1.5.3.1.1 Name

auditLogId

##### 1.5.3.1.2 Type

🔹 BIGINT

##### 1.5.3.1.3 Is Required

✅ Yes

##### 1.5.3.1.4 Is Primary Key

✅ Yes

##### 1.5.3.1.5 Is Unique

✅ Yes

##### 1.5.3.1.6 Index Type

UniqueIndex

##### 1.5.3.1.7 Size

0

##### 1.5.3.1.8 Constraints

- AUTO_INCREMENT

##### 1.5.3.1.9 Default Value



##### 1.5.3.1.10 Is Foreign Key

❌ No

##### 1.5.3.1.11 Precision

0

##### 1.5.3.1.12 Scale

0

#### 1.5.3.2.0 Guid

##### 1.5.3.2.1 Name

tenantId

##### 1.5.3.2.2 Type

🔹 Guid

##### 1.5.3.2.3 Is Required

✅ Yes

##### 1.5.3.2.4 Is Primary Key

❌ No

##### 1.5.3.2.5 Is Unique

❌ No

##### 1.5.3.2.6 Index Type

Index

##### 1.5.3.2.7 Size

0

##### 1.5.3.2.8 Constraints

*No items available*

##### 1.5.3.2.9 Default Value



##### 1.5.3.2.10 Is Foreign Key

✅ Yes

##### 1.5.3.2.11 Precision

0

##### 1.5.3.2.12 Scale

0

#### 1.5.3.3.0 Guid

##### 1.5.3.3.1 Name

userId

##### 1.5.3.3.2 Type

🔹 Guid

##### 1.5.3.3.3 Is Required

❌ No

##### 1.5.3.3.4 Is Primary Key

❌ No

##### 1.5.3.3.5 Is Unique

❌ No

##### 1.5.3.3.6 Index Type

Index

##### 1.5.3.3.7 Size

0

##### 1.5.3.3.8 Constraints

*No items available*

##### 1.5.3.3.9 Default Value



##### 1.5.3.3.10 Is Foreign Key

✅ Yes

##### 1.5.3.3.11 Precision

0

##### 1.5.3.3.12 Scale

0

#### 1.5.3.4.0 DateTimeOffset

##### 1.5.3.4.1 Name

timestamp

##### 1.5.3.4.2 Type

🔹 DateTimeOffset

##### 1.5.3.4.3 Is Required

✅ Yes

##### 1.5.3.4.4 Is Primary Key

❌ No

##### 1.5.3.4.5 Is Unique

❌ No

##### 1.5.3.4.6 Index Type

Index

##### 1.5.3.4.7 Size

0

##### 1.5.3.4.8 Constraints

*No items available*

##### 1.5.3.4.9 Default Value

CURRENT_TIMESTAMP

##### 1.5.3.4.10 Is Foreign Key

❌ No

##### 1.5.3.4.11 Precision

0

##### 1.5.3.4.12 Scale

0

#### 1.5.3.5.0 VARCHAR

##### 1.5.3.5.1 Name

actionType

##### 1.5.3.5.2 Type

🔹 VARCHAR

##### 1.5.3.5.3 Is Required

✅ Yes

##### 1.5.3.5.4 Is Primary Key

❌ No

##### 1.5.3.5.5 Is Unique

❌ No

##### 1.5.3.5.6 Index Type

Index

##### 1.5.3.5.7 Size

100

##### 1.5.3.5.8 Constraints

*No items available*

##### 1.5.3.5.9 Default Value



##### 1.5.3.5.10 Is Foreign Key

❌ No

##### 1.5.3.5.11 Precision

0

##### 1.5.3.5.12 Scale

0

#### 1.5.3.6.0 VARCHAR

##### 1.5.3.6.1 Name

entityName

##### 1.5.3.6.2 Type

🔹 VARCHAR

##### 1.5.3.6.3 Is Required

❌ No

##### 1.5.3.6.4 Is Primary Key

❌ No

##### 1.5.3.6.5 Is Unique

❌ No

##### 1.5.3.6.6 Index Type

Index

##### 1.5.3.6.7 Size

100

##### 1.5.3.6.8 Constraints

*No items available*

##### 1.5.3.6.9 Default Value



##### 1.5.3.6.10 Is Foreign Key

❌ No

##### 1.5.3.6.11 Precision

0

##### 1.5.3.6.12 Scale

0

#### 1.5.3.7.0 VARCHAR

##### 1.5.3.7.1 Name

entityId

##### 1.5.3.7.2 Type

🔹 VARCHAR

##### 1.5.3.7.3 Is Required

❌ No

##### 1.5.3.7.4 Is Primary Key

❌ No

##### 1.5.3.7.5 Is Unique

❌ No

##### 1.5.3.7.6 Index Type

Index

##### 1.5.3.7.7 Size

255

##### 1.5.3.7.8 Constraints

*No items available*

##### 1.5.3.7.9 Default Value



##### 1.5.3.7.10 Is Foreign Key

❌ No

##### 1.5.3.7.11 Precision

0

##### 1.5.3.7.12 Scale

0

#### 1.5.3.8.0 JSONB

##### 1.5.3.8.1 Name

details

##### 1.5.3.8.2 Type

🔹 JSONB

##### 1.5.3.8.3 Is Required

❌ No

##### 1.5.3.8.4 Is Primary Key

❌ No

##### 1.5.3.8.5 Is Unique

❌ No

##### 1.5.3.8.6 Index Type

None

##### 1.5.3.8.7 Size

0

##### 1.5.3.8.8 Constraints

*No items available*

##### 1.5.3.8.9 Default Value

{}

##### 1.5.3.8.10 Is Foreign Key

❌ No

##### 1.5.3.8.11 Precision

0

##### 1.5.3.8.12 Scale

0

#### 1.5.3.9.0 VARCHAR

##### 1.5.3.9.1 Name

sourceIpAddress

##### 1.5.3.9.2 Type

🔹 VARCHAR

##### 1.5.3.9.3 Is Required

❌ No

##### 1.5.3.9.4 Is Primary Key

❌ No

##### 1.5.3.9.5 Is Unique

❌ No

##### 1.5.3.9.6 Index Type

None

##### 1.5.3.9.7 Size

45

##### 1.5.3.9.8 Constraints

*No items available*

##### 1.5.3.9.9 Default Value



##### 1.5.3.9.10 Is Foreign Key

❌ No

##### 1.5.3.9.11 Precision

0

##### 1.5.3.9.12 Scale

0

#### 1.5.3.10.0 VARCHAR

##### 1.5.3.10.1 Name

qldbDigest

##### 1.5.3.10.2 Type

🔹 VARCHAR

##### 1.5.3.10.3 Is Required

❌ No

##### 1.5.3.10.4 Is Primary Key

❌ No

##### 1.5.3.10.5 Is Unique

✅ Yes

##### 1.5.3.10.6 Index Type

UniqueIndex

##### 1.5.3.10.7 Size

255

##### 1.5.3.10.8 Constraints

*No items available*

##### 1.5.3.10.9 Default Value



##### 1.5.3.10.10 Is Foreign Key

❌ No

##### 1.5.3.10.11 Precision

0

##### 1.5.3.10.12 Scale

0

### 1.5.4.0.0 Primary Keys

- auditLogId

### 1.5.5.0.0 Unique Constraints

- {'name': 'UC_AuditLog_QldbDigest', 'columns': ['qldbDigest']}

### 1.5.6.0.0 Indexes

#### 1.5.6.1.0 BTree

##### 1.5.6.1.1 Name

IX_AuditLog_Tenant_Timestamp

##### 1.5.6.1.2 Columns

- tenantId
- timestamp

##### 1.5.6.1.3 Type

🔹 BTree

#### 1.5.6.2.0 BTree

##### 1.5.6.2.1 Name

IX_AuditLog_User_Timestamp

##### 1.5.6.2.2 Columns

- userId
- timestamp

##### 1.5.6.2.3 Type

🔹 BTree

#### 1.5.6.3.0 BTree

##### 1.5.6.3.1 Name

IX_AuditLog_Entity

##### 1.5.6.3.2 Columns

- entityName
- entityId

##### 1.5.6.3.3 Type

🔹 BTree

#### 1.5.6.4.0 GIN

##### 1.5.6.4.1 Name

IX_AuditLog_Details_GIN

##### 1.5.6.4.2 Columns

- details

##### 1.5.6.4.3 Type

🔹 GIN

### 1.5.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Range |
| Column | timestamp |
| Strategy | Monthly |
| Comment | Improves queries by date and simplifies data reten... |

## 1.6.0.0.0 OpcCoreClient

### 1.6.1.0.0 Name

OpcCoreClient

### 1.6.2.0.0 Description

Represents a deployed on-premise or edge client instance. (REQ-1-001, REQ-1-062)

### 1.6.3.0.0 Attributes

#### 1.6.3.1.0 Guid

##### 1.6.3.1.1 Name

opcCoreClientId

##### 1.6.3.1.2 Type

🔹 Guid

##### 1.6.3.1.3 Is Required

✅ Yes

##### 1.6.3.1.4 Is Primary Key

✅ Yes

##### 1.6.3.1.5 Is Unique

✅ Yes

##### 1.6.3.1.6 Index Type

UniqueIndex

##### 1.6.3.1.7 Size

0

##### 1.6.3.1.8 Constraints

*No items available*

##### 1.6.3.1.9 Default Value



##### 1.6.3.1.10 Is Foreign Key

❌ No

##### 1.6.3.1.11 Precision

0

##### 1.6.3.1.12 Scale

0

#### 1.6.3.2.0 Guid

##### 1.6.3.2.1 Name

tenantId

##### 1.6.3.2.2 Type

🔹 Guid

##### 1.6.3.2.3 Is Required

✅ Yes

##### 1.6.3.2.4 Is Primary Key

❌ No

##### 1.6.3.2.5 Is Unique

❌ No

##### 1.6.3.2.6 Index Type

Index

##### 1.6.3.2.7 Size

0

##### 1.6.3.2.8 Constraints

*No items available*

##### 1.6.3.2.9 Default Value



##### 1.6.3.2.10 Is Foreign Key

✅ Yes

##### 1.6.3.2.11 Precision

0

##### 1.6.3.2.12 Scale

0

#### 1.6.3.3.0 VARCHAR

##### 1.6.3.3.1 Name

name

##### 1.6.3.3.2 Type

🔹 VARCHAR

##### 1.6.3.3.3 Is Required

✅ Yes

##### 1.6.3.3.4 Is Primary Key

❌ No

##### 1.6.3.3.5 Is Unique

❌ No

##### 1.6.3.3.6 Index Type

Index

##### 1.6.3.3.7 Size

255

##### 1.6.3.3.8 Constraints

*No items available*

##### 1.6.3.3.9 Default Value



##### 1.6.3.3.10 Is Foreign Key

❌ No

##### 1.6.3.3.11 Precision

0

##### 1.6.3.3.12 Scale

0

#### 1.6.3.4.0 VARCHAR

##### 1.6.3.4.1 Name

status

##### 1.6.3.4.2 Type

🔹 VARCHAR

##### 1.6.3.4.3 Is Required

✅ Yes

##### 1.6.3.4.4 Is Primary Key

❌ No

##### 1.6.3.4.5 Is Unique

❌ No

##### 1.6.3.4.6 Index Type

Index

##### 1.6.3.4.7 Size

50

##### 1.6.3.4.8 Constraints

- ENUM('Online', 'Offline', 'Degraded')

##### 1.6.3.4.9 Default Value

Offline

##### 1.6.3.4.10 Is Foreign Key

❌ No

##### 1.6.3.4.11 Precision

0

##### 1.6.3.4.12 Scale

0

#### 1.6.3.5.0 VARCHAR

##### 1.6.3.5.1 Name

softwareVersion

##### 1.6.3.5.2 Type

🔹 VARCHAR

##### 1.6.3.5.3 Is Required

❌ No

##### 1.6.3.5.4 Is Primary Key

❌ No

##### 1.6.3.5.5 Is Unique

❌ No

##### 1.6.3.5.6 Index Type

None

##### 1.6.3.5.7 Size

50

##### 1.6.3.5.8 Constraints

*No items available*

##### 1.6.3.5.9 Default Value



##### 1.6.3.5.10 Is Foreign Key

❌ No

##### 1.6.3.5.11 Precision

0

##### 1.6.3.5.12 Scale

0

#### 1.6.3.6.0 DateTimeOffset

##### 1.6.3.6.1 Name

lastHeartbeat

##### 1.6.3.6.2 Type

🔹 DateTimeOffset

##### 1.6.3.6.3 Is Required

❌ No

##### 1.6.3.6.4 Is Primary Key

❌ No

##### 1.6.3.6.5 Is Unique

❌ No

##### 1.6.3.6.6 Index Type

Index

##### 1.6.3.6.7 Size

0

##### 1.6.3.6.8 Constraints

*No items available*

##### 1.6.3.6.9 Default Value



##### 1.6.3.6.10 Is Foreign Key

❌ No

##### 1.6.3.6.11 Precision

0

##### 1.6.3.6.12 Scale

0

#### 1.6.3.7.0 BOOLEAN

##### 1.6.3.7.1 Name

isDeleted

##### 1.6.3.7.2 Type

🔹 BOOLEAN

##### 1.6.3.7.3 Is Required

✅ Yes

##### 1.6.3.7.4 Is Primary Key

❌ No

##### 1.6.3.7.5 Is Unique

❌ No

##### 1.6.3.7.6 Index Type

Index

##### 1.6.3.7.7 Size

0

##### 1.6.3.7.8 Constraints

*No items available*

##### 1.6.3.7.9 Default Value

false

##### 1.6.3.7.10 Is Foreign Key

❌ No

##### 1.6.3.7.11 Precision

0

##### 1.6.3.7.12 Scale

0

#### 1.6.3.8.0 DateTimeOffset

##### 1.6.3.8.1 Name

createdAt

##### 1.6.3.8.2 Type

🔹 DateTimeOffset

##### 1.6.3.8.3 Is Required

✅ Yes

##### 1.6.3.8.4 Is Primary Key

❌ No

##### 1.6.3.8.5 Is Unique

❌ No

##### 1.6.3.8.6 Index Type

Index

##### 1.6.3.8.7 Size

0

##### 1.6.3.8.8 Constraints

*No items available*

##### 1.6.3.8.9 Default Value

CURRENT_TIMESTAMP

##### 1.6.3.8.10 Is Foreign Key

❌ No

##### 1.6.3.8.11 Precision

0

##### 1.6.3.8.12 Scale

0

#### 1.6.3.9.0 DateTimeOffset

##### 1.6.3.9.1 Name

updatedAt

##### 1.6.3.9.2 Type

🔹 DateTimeOffset

##### 1.6.3.9.3 Is Required

✅ Yes

##### 1.6.3.9.4 Is Primary Key

❌ No

##### 1.6.3.9.5 Is Unique

❌ No

##### 1.6.3.9.6 Index Type

None

##### 1.6.3.9.7 Size

0

##### 1.6.3.9.8 Constraints

*No items available*

##### 1.6.3.9.9 Default Value

CURRENT_TIMESTAMP

##### 1.6.3.9.10 Is Foreign Key

❌ No

##### 1.6.3.9.11 Precision

0

##### 1.6.3.9.12 Scale

0

### 1.6.4.0.0 Primary Keys

- opcCoreClientId

### 1.6.5.0.0 Unique Constraints

- {'name': 'UC_OpcCoreClient_Tenant_Name', 'columns': ['tenantId', 'name']}

### 1.6.6.0.0 Indexes

#### 1.6.6.1.0 BTree

##### 1.6.6.1.1 Name

IX_OpcCoreClient_Tenant_Status_Heartbeat

##### 1.6.6.1.2 Columns

- tenantId
- status
- lastHeartbeat DESC

##### 1.6.6.1.3 Type

🔹 BTree

#### 1.6.6.2.0 BTree

##### 1.6.6.2.1 Name

IX_OpcCoreClient_Tenant_IsDeleted

##### 1.6.6.2.2 Columns

- tenantId
- isDeleted

##### 1.6.6.2.3 Type

🔹 BTree

## 1.7.0.0.0 OpcServerConnection

### 1.7.1.0.0 Name

OpcServerConnection

### 1.7.2.0.0 Description

Stores configuration details for connecting to an industrial OPC server. (REQ-1-002)

### 1.7.3.0.0 Attributes

#### 1.7.3.1.0 Guid

##### 1.7.3.1.1 Name

opcServerConnectionId

##### 1.7.3.1.2 Type

🔹 Guid

##### 1.7.3.1.3 Is Required

✅ Yes

##### 1.7.3.1.4 Is Primary Key

✅ Yes

##### 1.7.3.1.5 Is Unique

✅ Yes

##### 1.7.3.1.6 Index Type

UniqueIndex

##### 1.7.3.1.7 Size

0

##### 1.7.3.1.8 Constraints

*No items available*

##### 1.7.3.1.9 Default Value



##### 1.7.3.1.10 Is Foreign Key

❌ No

##### 1.7.3.1.11 Precision

0

##### 1.7.3.1.12 Scale

0

#### 1.7.3.2.0 Guid

##### 1.7.3.2.1 Name

opcCoreClientId

##### 1.7.3.2.2 Type

🔹 Guid

##### 1.7.3.2.3 Is Required

✅ Yes

##### 1.7.3.2.4 Is Primary Key

❌ No

##### 1.7.3.2.5 Is Unique

❌ No

##### 1.7.3.2.6 Index Type

Index

##### 1.7.3.2.7 Size

0

##### 1.7.3.2.8 Constraints

*No items available*

##### 1.7.3.2.9 Default Value



##### 1.7.3.2.10 Is Foreign Key

✅ Yes

##### 1.7.3.2.11 Precision

0

##### 1.7.3.2.12 Scale

0

#### 1.7.3.3.0 VARCHAR

##### 1.7.3.3.1 Name

name

##### 1.7.3.3.2 Type

🔹 VARCHAR

##### 1.7.3.3.3 Is Required

✅ Yes

##### 1.7.3.3.4 Is Primary Key

❌ No

##### 1.7.3.3.5 Is Unique

❌ No

##### 1.7.3.3.6 Index Type

Index

##### 1.7.3.3.7 Size

255

##### 1.7.3.3.8 Constraints

*No items available*

##### 1.7.3.3.9 Default Value



##### 1.7.3.3.10 Is Foreign Key

❌ No

##### 1.7.3.3.11 Precision

0

##### 1.7.3.3.12 Scale

0

#### 1.7.3.4.0 VARCHAR

##### 1.7.3.4.1 Name

protocol

##### 1.7.3.4.2 Type

🔹 VARCHAR

##### 1.7.3.4.3 Is Required

✅ Yes

##### 1.7.3.4.4 Is Primary Key

❌ No

##### 1.7.3.4.5 Is Unique

❌ No

##### 1.7.3.4.6 Index Type

None

##### 1.7.3.4.7 Size

20

##### 1.7.3.4.8 Constraints

- ENUM('OPC-DA', 'OPC-UA', 'OPC-XML-DA')

##### 1.7.3.4.9 Default Value



##### 1.7.3.4.10 Is Foreign Key

❌ No

##### 1.7.3.4.11 Precision

0

##### 1.7.3.4.12 Scale

0

#### 1.7.3.5.0 VARCHAR

##### 1.7.3.5.1 Name

endpointUrl

##### 1.7.3.5.2 Type

🔹 VARCHAR

##### 1.7.3.5.3 Is Required

✅ Yes

##### 1.7.3.5.4 Is Primary Key

❌ No

##### 1.7.3.5.5 Is Unique

❌ No

##### 1.7.3.5.6 Index Type

None

##### 1.7.3.5.7 Size

512

##### 1.7.3.5.8 Constraints

*No items available*

##### 1.7.3.5.9 Default Value



##### 1.7.3.5.10 Is Foreign Key

❌ No

##### 1.7.3.5.11 Precision

0

##### 1.7.3.5.12 Scale

0

#### 1.7.3.6.0 JSONB

##### 1.7.3.6.1 Name

securityConfiguration

##### 1.7.3.6.2 Type

🔹 JSONB

##### 1.7.3.6.3 Is Required

❌ No

##### 1.7.3.6.4 Is Primary Key

❌ No

##### 1.7.3.6.5 Is Unique

❌ No

##### 1.7.3.6.6 Index Type

None

##### 1.7.3.6.7 Size

0

##### 1.7.3.6.8 Constraints

*No items available*

##### 1.7.3.6.9 Default Value

{}

##### 1.7.3.6.10 Is Foreign Key

❌ No

##### 1.7.3.6.11 Precision

0

##### 1.7.3.6.12 Scale

0

#### 1.7.3.7.0 BOOLEAN

##### 1.7.3.7.1 Name

isRedundantPair

##### 1.7.3.7.2 Type

🔹 BOOLEAN

##### 1.7.3.7.3 Is Required

✅ Yes

##### 1.7.3.7.4 Is Primary Key

❌ No

##### 1.7.3.7.5 Is Unique

❌ No

##### 1.7.3.7.6 Index Type

None

##### 1.7.3.7.7 Size

0

##### 1.7.3.7.8 Constraints

*No items available*

##### 1.7.3.7.9 Default Value

false

##### 1.7.3.7.10 Is Foreign Key

❌ No

##### 1.7.3.7.11 Precision

0

##### 1.7.3.7.12 Scale

0

#### 1.7.3.8.0 Guid

##### 1.7.3.8.1 Name

backupServerConnectionId

##### 1.7.3.8.2 Type

🔹 Guid

##### 1.7.3.8.3 Is Required

❌ No

##### 1.7.3.8.4 Is Primary Key

❌ No

##### 1.7.3.8.5 Is Unique

❌ No

##### 1.7.3.8.6 Index Type

Index

##### 1.7.3.8.7 Size

0

##### 1.7.3.8.8 Constraints

*No items available*

##### 1.7.3.8.9 Default Value



##### 1.7.3.8.10 Is Foreign Key

✅ Yes

##### 1.7.3.8.11 Precision

0

##### 1.7.3.8.12 Scale

0

### 1.7.4.0.0 Primary Keys

- opcServerConnectionId

### 1.7.5.0.0 Unique Constraints

- {'name': 'UC_OpcServerConnection_Client_Name', 'columns': ['opcCoreClientId', 'name']}

### 1.7.6.0.0 Indexes

- {'name': 'IX_OpcServerConnection_OpcCoreClientId', 'columns': ['opcCoreClientId'], 'type': 'BTree'}

## 1.8.0.0.0 Asset

### 1.8.1.0.0 Name

Asset

### 1.8.2.0.0 Description

Represents a physical or logical asset in a hierarchical structure (ISA-95). (REQ-1-031, REQ-1-046)

### 1.8.3.0.0 Attributes

#### 1.8.3.1.0 Guid

##### 1.8.3.1.1 Name

assetId

##### 1.8.3.1.2 Type

🔹 Guid

##### 1.8.3.1.3 Is Required

✅ Yes

##### 1.8.3.1.4 Is Primary Key

✅ Yes

##### 1.8.3.1.5 Is Unique

✅ Yes

##### 1.8.3.1.6 Index Type

UniqueIndex

##### 1.8.3.1.7 Size

0

##### 1.8.3.1.8 Constraints

*No items available*

##### 1.8.3.1.9 Default Value



##### 1.8.3.1.10 Is Foreign Key

❌ No

##### 1.8.3.1.11 Precision

0

##### 1.8.3.1.12 Scale

0

#### 1.8.3.2.0 Guid

##### 1.8.3.2.1 Name

tenantId

##### 1.8.3.2.2 Type

🔹 Guid

##### 1.8.3.2.3 Is Required

✅ Yes

##### 1.8.3.2.4 Is Primary Key

❌ No

##### 1.8.3.2.5 Is Unique

❌ No

##### 1.8.3.2.6 Index Type

Index

##### 1.8.3.2.7 Size

0

##### 1.8.3.2.8 Constraints

*No items available*

##### 1.8.3.2.9 Default Value



##### 1.8.3.2.10 Is Foreign Key

✅ Yes

##### 1.8.3.2.11 Precision

0

##### 1.8.3.2.12 Scale

0

#### 1.8.3.3.0 VARCHAR

##### 1.8.3.3.1 Name

name

##### 1.8.3.3.2 Type

🔹 VARCHAR

##### 1.8.3.3.3 Is Required

✅ Yes

##### 1.8.3.3.4 Is Primary Key

❌ No

##### 1.8.3.3.5 Is Unique

❌ No

##### 1.8.3.3.6 Index Type

Index

##### 1.8.3.3.7 Size

255

##### 1.8.3.3.8 Constraints

*No items available*

##### 1.8.3.3.9 Default Value



##### 1.8.3.3.10 Is Foreign Key

❌ No

##### 1.8.3.3.11 Precision

0

##### 1.8.3.3.12 Scale

0

#### 1.8.3.4.0 Guid

##### 1.8.3.4.1 Name

parentAssetId

##### 1.8.3.4.2 Type

🔹 Guid

##### 1.8.3.4.3 Is Required

❌ No

##### 1.8.3.4.4 Is Primary Key

❌ No

##### 1.8.3.4.5 Is Unique

❌ No

##### 1.8.3.4.6 Index Type

Index

##### 1.8.3.4.7 Size

0

##### 1.8.3.4.8 Constraints

*No items available*

##### 1.8.3.4.9 Default Value



##### 1.8.3.4.10 Is Foreign Key

✅ Yes

##### 1.8.3.4.11 Precision

0

##### 1.8.3.4.12 Scale

0

#### 1.8.3.5.0 Guid

##### 1.8.3.5.1 Name

assetTemplateId

##### 1.8.3.5.2 Type

🔹 Guid

##### 1.8.3.5.3 Is Required

❌ No

##### 1.8.3.5.4 Is Primary Key

❌ No

##### 1.8.3.5.5 Is Unique

❌ No

##### 1.8.3.5.6 Index Type

Index

##### 1.8.3.5.7 Size

0

##### 1.8.3.5.8 Constraints

*No items available*

##### 1.8.3.5.9 Default Value



##### 1.8.3.5.10 Is Foreign Key

✅ Yes

##### 1.8.3.5.11 Precision

0

##### 1.8.3.5.12 Scale

0

#### 1.8.3.6.0 JSONB

##### 1.8.3.6.1 Name

properties

##### 1.8.3.6.2 Type

🔹 JSONB

##### 1.8.3.6.3 Is Required

❌ No

##### 1.8.3.6.4 Is Primary Key

❌ No

##### 1.8.3.6.5 Is Unique

❌ No

##### 1.8.3.6.6 Index Type

None

##### 1.8.3.6.7 Size

0

##### 1.8.3.6.8 Constraints

*No items available*

##### 1.8.3.6.9 Default Value

{}

##### 1.8.3.6.10 Is Foreign Key

❌ No

##### 1.8.3.6.11 Precision

0

##### 1.8.3.6.12 Scale

0

#### 1.8.3.7.0 BOOLEAN

##### 1.8.3.7.1 Name

isDeleted

##### 1.8.3.7.2 Type

🔹 BOOLEAN

##### 1.8.3.7.3 Is Required

✅ Yes

##### 1.8.3.7.4 Is Primary Key

❌ No

##### 1.8.3.7.5 Is Unique

❌ No

##### 1.8.3.7.6 Index Type

Index

##### 1.8.3.7.7 Size

0

##### 1.8.3.7.8 Constraints

*No items available*

##### 1.8.3.7.9 Default Value

false

##### 1.8.3.7.10 Is Foreign Key

❌ No

##### 1.8.3.7.11 Precision

0

##### 1.8.3.7.12 Scale

0

#### 1.8.3.8.0 DateTimeOffset

##### 1.8.3.8.1 Name

createdAt

##### 1.8.3.8.2 Type

🔹 DateTimeOffset

##### 1.8.3.8.3 Is Required

✅ Yes

##### 1.8.3.8.4 Is Primary Key

❌ No

##### 1.8.3.8.5 Is Unique

❌ No

##### 1.8.3.8.6 Index Type

Index

##### 1.8.3.8.7 Size

0

##### 1.8.3.8.8 Constraints

*No items available*

##### 1.8.3.8.9 Default Value

CURRENT_TIMESTAMP

##### 1.8.3.8.10 Is Foreign Key

❌ No

##### 1.8.3.8.11 Precision

0

##### 1.8.3.8.12 Scale

0

### 1.8.4.0.0 Primary Keys

- assetId

### 1.8.5.0.0 Unique Constraints

- {'name': 'UC_Asset_Tenant_Name', 'columns': ['tenantId', 'name']}

### 1.8.6.0.0 Indexes

#### 1.8.6.1.0 BTree

##### 1.8.6.1.1 Name

IX_Asset_Tenant_Parent

##### 1.8.6.1.2 Columns

- tenantId
- parentAssetId

##### 1.8.6.1.3 Type

🔹 BTree

#### 1.8.6.2.0 BTree

##### 1.8.6.2.1 Name

IX_Asset_Tenant_IsDeleted

##### 1.8.6.2.2 Columns

- tenantId
- isDeleted

##### 1.8.6.2.3 Type

🔹 BTree

#### 1.8.6.3.0 BTree

##### 1.8.6.3.1 Name

IX_Asset_AssetTemplateId

##### 1.8.6.3.2 Columns

- assetTemplateId

##### 1.8.6.3.3 Type

🔹 BTree

### 1.8.7.0.0 Caching

| Property | Value |
|----------|-------|
| Strategy | Redis |
| Key | asset_hierarchy:{tenantId} |
| Invalidation | On CUD operations on the Asset table. |
| Comment | Caches the tree structure to avoid expensive recur... |

## 1.9.0.0.0 OpcTag

### 1.9.1.0.0 Name

OpcTag

### 1.9.2.0.0 Description

Represents a data point (tag) from an OPC server, mapped to an asset. (REQ-1-047)

### 1.9.3.0.0 Attributes

#### 1.9.3.1.0 Guid

##### 1.9.3.1.1 Name

opcTagId

##### 1.9.3.1.2 Type

🔹 Guid

##### 1.9.3.1.3 Is Required

✅ Yes

##### 1.9.3.1.4 Is Primary Key

✅ Yes

##### 1.9.3.1.5 Is Unique

✅ Yes

##### 1.9.3.1.6 Index Type

UniqueIndex

##### 1.9.3.1.7 Size

0

##### 1.9.3.1.8 Constraints

*No items available*

##### 1.9.3.1.9 Default Value



##### 1.9.3.1.10 Is Foreign Key

❌ No

##### 1.9.3.1.11 Precision

0

##### 1.9.3.1.12 Scale

0

#### 1.9.3.2.0 Guid

##### 1.9.3.2.1 Name

assetId

##### 1.9.3.2.2 Type

🔹 Guid

##### 1.9.3.2.3 Is Required

✅ Yes

##### 1.9.3.2.4 Is Primary Key

❌ No

##### 1.9.3.2.5 Is Unique

❌ No

##### 1.9.3.2.6 Index Type

Index

##### 1.9.3.2.7 Size

0

##### 1.9.3.2.8 Constraints

*No items available*

##### 1.9.3.2.9 Default Value



##### 1.9.3.2.10 Is Foreign Key

✅ Yes

##### 1.9.3.2.11 Precision

0

##### 1.9.3.2.12 Scale

0

#### 1.9.3.3.0 Guid

##### 1.9.3.3.1 Name

opcServerConnectionId

##### 1.9.3.3.2 Type

🔹 Guid

##### 1.9.3.3.3 Is Required

✅ Yes

##### 1.9.3.3.4 Is Primary Key

❌ No

##### 1.9.3.3.5 Is Unique

❌ No

##### 1.9.3.3.6 Index Type

Index

##### 1.9.3.3.7 Size

0

##### 1.9.3.3.8 Constraints

*No items available*

##### 1.9.3.3.9 Default Value



##### 1.9.3.3.10 Is Foreign Key

✅ Yes

##### 1.9.3.3.11 Precision

0

##### 1.9.3.3.12 Scale

0

#### 1.9.3.4.0 VARCHAR

##### 1.9.3.4.1 Name

name

##### 1.9.3.4.2 Type

🔹 VARCHAR

##### 1.9.3.4.3 Is Required

✅ Yes

##### 1.9.3.4.4 Is Primary Key

❌ No

##### 1.9.3.4.5 Is Unique

❌ No

##### 1.9.3.4.6 Index Type

Index

##### 1.9.3.4.7 Size

255

##### 1.9.3.4.8 Constraints

*No items available*

##### 1.9.3.4.9 Default Value



##### 1.9.3.4.10 Is Foreign Key

❌ No

##### 1.9.3.4.11 Precision

0

##### 1.9.3.4.12 Scale

0

#### 1.9.3.5.0 VARCHAR

##### 1.9.3.5.1 Name

nodeId

##### 1.9.3.5.2 Type

🔹 VARCHAR

##### 1.9.3.5.3 Is Required

✅ Yes

##### 1.9.3.5.4 Is Primary Key

❌ No

##### 1.9.3.5.5 Is Unique

❌ No

##### 1.9.3.5.6 Index Type

Index

##### 1.9.3.5.7 Size

512

##### 1.9.3.5.8 Constraints

*No items available*

##### 1.9.3.5.9 Default Value



##### 1.9.3.5.10 Is Foreign Key

❌ No

##### 1.9.3.5.11 Precision

0

##### 1.9.3.5.12 Scale

0

#### 1.9.3.6.0 VARCHAR

##### 1.9.3.6.1 Name

dataType

##### 1.9.3.6.2 Type

🔹 VARCHAR

##### 1.9.3.6.3 Is Required

❌ No

##### 1.9.3.6.4 Is Primary Key

❌ No

##### 1.9.3.6.5 Is Unique

❌ No

##### 1.9.3.6.6 Index Type

None

##### 1.9.3.6.7 Size

50

##### 1.9.3.6.8 Constraints

*No items available*

##### 1.9.3.6.9 Default Value



##### 1.9.3.6.10 Is Foreign Key

❌ No

##### 1.9.3.6.11 Precision

0

##### 1.9.3.6.12 Scale

0

#### 1.9.3.7.0 BOOLEAN

##### 1.9.3.7.1 Name

isWritable

##### 1.9.3.7.2 Type

🔹 BOOLEAN

##### 1.9.3.7.3 Is Required

✅ Yes

##### 1.9.3.7.4 Is Primary Key

❌ No

##### 1.9.3.7.5 Is Unique

❌ No

##### 1.9.3.7.6 Index Type

None

##### 1.9.3.7.7 Size

0

##### 1.9.3.7.8 Constraints

*No items available*

##### 1.9.3.7.9 Default Value

false

##### 1.9.3.7.10 Is Foreign Key

❌ No

##### 1.9.3.7.11 Precision

0

##### 1.9.3.7.12 Scale

0

#### 1.9.3.8.0 INT

##### 1.9.3.8.1 Name

subscriptionUpdateRateMs

##### 1.9.3.8.2 Type

🔹 INT

##### 1.9.3.8.3 Is Required

❌ No

##### 1.9.3.8.4 Is Primary Key

❌ No

##### 1.9.3.8.5 Is Unique

❌ No

##### 1.9.3.8.6 Index Type

None

##### 1.9.3.8.7 Size

0

##### 1.9.3.8.8 Constraints

*No items available*

##### 1.9.3.8.9 Default Value

1000

##### 1.9.3.8.10 Is Foreign Key

❌ No

##### 1.9.3.8.11 Precision

0

##### 1.9.3.8.12 Scale

0

### 1.9.4.0.0 Primary Keys

- opcTagId

### 1.9.5.0.0 Unique Constraints

- {'name': 'UC_OpcTag_Connection_NodeId', 'columns': ['opcServerConnectionId', 'nodeId']}

### 1.9.6.0.0 Indexes

- {'name': 'IX_OpcTag_AssetId', 'columns': ['assetId'], 'type': 'BTree'}

### 1.9.7.0.0 Caching

| Property | Value |
|----------|-------|
| Strategy | Redis |
| Key | opctag_config:{opcServerConnectionId} |
| Invalidation | On tag configuration changes. |
| Comment | Caches tag configuration which is read frequently ... |

## 1.10.0.0.0 TagDataPoint

### 1.10.1.0.0 Name

TagDataPoint

### 1.10.2.0.0 Description

Represents a single time-series data point for an OPC tag. Stored in TimescaleDB. (REQ-1-003)

### 1.10.3.0.0 Attributes

#### 1.10.3.1.0 DateTimeOffset

##### 1.10.3.1.1 Name

timestamp

##### 1.10.3.1.2 Type

🔹 DateTimeOffset

##### 1.10.3.1.3 Is Required

✅ Yes

##### 1.10.3.1.4 Is Primary Key

✅ Yes

##### 1.10.3.1.5 Is Unique

❌ No

##### 1.10.3.1.6 Index Type

Index

##### 1.10.3.1.7 Size

0

##### 1.10.3.1.8 Constraints

*No items available*

##### 1.10.3.1.9 Default Value



##### 1.10.3.1.10 Is Foreign Key

❌ No

##### 1.10.3.1.11 Precision

0

##### 1.10.3.1.12 Scale

0

#### 1.10.3.2.0 Guid

##### 1.10.3.2.1 Name

opcTagId

##### 1.10.3.2.2 Type

🔹 Guid

##### 1.10.3.2.3 Is Required

✅ Yes

##### 1.10.3.2.4 Is Primary Key

✅ Yes

##### 1.10.3.2.5 Is Unique

❌ No

##### 1.10.3.2.6 Index Type

Index

##### 1.10.3.2.7 Size

0

##### 1.10.3.2.8 Constraints

*No items available*

##### 1.10.3.2.9 Default Value



##### 1.10.3.2.10 Is Foreign Key

✅ Yes

##### 1.10.3.2.11 Precision

0

##### 1.10.3.2.12 Scale

0

#### 1.10.3.3.0 Guid

##### 1.10.3.3.1 Name

tenantId

##### 1.10.3.3.2 Type

🔹 Guid

##### 1.10.3.3.3 Is Required

✅ Yes

##### 1.10.3.3.4 Is Primary Key

❌ No

##### 1.10.3.3.5 Is Unique

❌ No

##### 1.10.3.3.6 Index Type

Index

##### 1.10.3.3.7 Size

0

##### 1.10.3.3.8 Constraints

*No items available*

##### 1.10.3.3.9 Default Value



##### 1.10.3.3.10 Is Foreign Key

❌ No

##### 1.10.3.3.11 Precision

0

##### 1.10.3.3.12 Scale

0

#### 1.10.3.4.0 Guid

##### 1.10.3.4.1 Name

assetId

##### 1.10.3.4.2 Type

🔹 Guid

##### 1.10.3.4.3 Is Required

✅ Yes

##### 1.10.3.4.4 Is Primary Key

❌ No

##### 1.10.3.4.5 Is Unique

❌ No

##### 1.10.3.4.6 Index Type

Index

##### 1.10.3.4.7 Size

0

##### 1.10.3.4.8 Constraints

*No items available*

##### 1.10.3.4.9 Default Value



##### 1.10.3.4.10 Is Foreign Key

❌ No

##### 1.10.3.4.11 Precision

0

##### 1.10.3.4.12 Scale

0

#### 1.10.3.5.0 DOUBLE PRECISION

##### 1.10.3.5.1 Name

value

##### 1.10.3.5.2 Type

🔹 DOUBLE PRECISION

##### 1.10.3.5.3 Is Required

✅ Yes

##### 1.10.3.5.4 Is Primary Key

❌ No

##### 1.10.3.5.5 Is Unique

❌ No

##### 1.10.3.5.6 Index Type

None

##### 1.10.3.5.7 Size

0

##### 1.10.3.5.8 Constraints

*No items available*

##### 1.10.3.5.9 Default Value



##### 1.10.3.5.10 Is Foreign Key

❌ No

##### 1.10.3.5.11 Precision

0

##### 1.10.3.5.12 Scale

0

#### 1.10.3.6.0 VARCHAR

##### 1.10.3.6.1 Name

quality

##### 1.10.3.6.2 Type

🔹 VARCHAR

##### 1.10.3.6.3 Is Required

✅ Yes

##### 1.10.3.6.4 Is Primary Key

❌ No

##### 1.10.3.6.5 Is Unique

❌ No

##### 1.10.3.6.6 Index Type

None

##### 1.10.3.6.7 Size

50

##### 1.10.3.6.8 Constraints

*No items available*

##### 1.10.3.6.9 Default Value



##### 1.10.3.6.10 Is Foreign Key

❌ No

##### 1.10.3.6.11 Precision

0

##### 1.10.3.6.12 Scale

0

### 1.10.4.0.0 Primary Keys

- opcTagId
- timestamp

### 1.10.5.0.0 Unique Constraints

*No items available*

### 1.10.6.0.0 Indexes

- {'name': 'IX_TagDataPoint_Tenant_Asset_Timestamp', 'columns': ['tenantId', 'assetId', 'timestamp DESC'], 'type': 'BTree'}

### 1.10.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Hypertable |
| Column | timestamp |
| Comment | Critical for time-series performance. |

### 1.10.8.0.0 Optimizations

- {'type': 'Continuous Aggregate', 'suggestion': 'Create materialized views for hourly/daily averages, min/max for fast reporting.', 'comment': 'Massively reduces query latency for dashboards.'}

## 1.11.0.0.0 Alarm

### 1.11.1.0.0 Name

Alarm

### 1.11.2.0.0 Description

Represents an alarm or event from an OPC A&C server. (REQ-1-035)

### 1.11.3.0.0 Attributes

#### 1.11.3.1.0 Guid

##### 1.11.3.1.1 Name

alarmId

##### 1.11.3.1.2 Type

🔹 Guid

##### 1.11.3.1.3 Is Required

✅ Yes

##### 1.11.3.1.4 Is Primary Key

✅ Yes

##### 1.11.3.1.5 Is Unique

✅ Yes

##### 1.11.3.1.6 Index Type

UniqueIndex

##### 1.11.3.1.7 Size

0

##### 1.11.3.1.8 Constraints

*No items available*

##### 1.11.3.1.9 Default Value



##### 1.11.3.1.10 Is Foreign Key

❌ No

##### 1.11.3.1.11 Precision

0

##### 1.11.3.1.12 Scale

0

#### 1.11.3.2.0 Guid

##### 1.11.3.2.1 Name

tenantId

##### 1.11.3.2.2 Type

🔹 Guid

##### 1.11.3.2.3 Is Required

✅ Yes

##### 1.11.3.2.4 Is Primary Key

❌ No

##### 1.11.3.2.5 Is Unique

❌ No

##### 1.11.3.2.6 Index Type

Index

##### 1.11.3.2.7 Size

0

##### 1.11.3.2.8 Constraints

*No items available*

##### 1.11.3.2.9 Default Value



##### 1.11.3.2.10 Is Foreign Key

❌ No

##### 1.11.3.2.11 Precision

0

##### 1.11.3.2.12 Scale

0

#### 1.11.3.3.0 Guid

##### 1.11.3.3.1 Name

assetId

##### 1.11.3.3.2 Type

🔹 Guid

##### 1.11.3.3.3 Is Required

✅ Yes

##### 1.11.3.3.4 Is Primary Key

❌ No

##### 1.11.3.3.5 Is Unique

❌ No

##### 1.11.3.3.6 Index Type

Index

##### 1.11.3.3.7 Size

0

##### 1.11.3.3.8 Constraints

*No items available*

##### 1.11.3.3.9 Default Value



##### 1.11.3.3.10 Is Foreign Key

❌ No

##### 1.11.3.3.11 Precision

0

##### 1.11.3.3.12 Scale

0

#### 1.11.3.4.0 Guid

##### 1.11.3.4.1 Name

opcTagId

##### 1.11.3.4.2 Type

🔹 Guid

##### 1.11.3.4.3 Is Required

✅ Yes

##### 1.11.3.4.4 Is Primary Key

❌ No

##### 1.11.3.4.5 Is Unique

❌ No

##### 1.11.3.4.6 Index Type

Index

##### 1.11.3.4.7 Size

0

##### 1.11.3.4.8 Constraints

*No items available*

##### 1.11.3.4.9 Default Value



##### 1.11.3.4.10 Is Foreign Key

✅ Yes

##### 1.11.3.4.11 Precision

0

##### 1.11.3.4.12 Scale

0

#### 1.11.3.5.0 VARCHAR

##### 1.11.3.5.1 Name

state

##### 1.11.3.5.2 Type

🔹 VARCHAR

##### 1.11.3.5.3 Is Required

✅ Yes

##### 1.11.3.5.4 Is Primary Key

❌ No

##### 1.11.3.5.5 Is Unique

❌ No

##### 1.11.3.5.6 Index Type

Index

##### 1.11.3.5.7 Size

50

##### 1.11.3.5.8 Constraints

- ENUM('Active', 'Acknowledged', 'Shelved', 'Cleared')

##### 1.11.3.5.9 Default Value

Active

##### 1.11.3.5.10 Is Foreign Key

❌ No

##### 1.11.3.5.11 Precision

0

##### 1.11.3.5.12 Scale

0

#### 1.11.3.6.0 INT

##### 1.11.3.6.1 Name

severity

##### 1.11.3.6.2 Type

🔹 INT

##### 1.11.3.6.3 Is Required

✅ Yes

##### 1.11.3.6.4 Is Primary Key

❌ No

##### 1.11.3.6.5 Is Unique

❌ No

##### 1.11.3.6.6 Index Type

Index

##### 1.11.3.6.7 Size

0

##### 1.11.3.6.8 Constraints

*No items available*

##### 1.11.3.6.9 Default Value

500

##### 1.11.3.6.10 Is Foreign Key

❌ No

##### 1.11.3.6.11 Precision

0

##### 1.11.3.6.12 Scale

0

#### 1.11.3.7.0 TEXT

##### 1.11.3.7.1 Name

message

##### 1.11.3.7.2 Type

🔹 TEXT

##### 1.11.3.7.3 Is Required

❌ No

##### 1.11.3.7.4 Is Primary Key

❌ No

##### 1.11.3.7.5 Is Unique

❌ No

##### 1.11.3.7.6 Index Type

None

##### 1.11.3.7.7 Size

0

##### 1.11.3.7.8 Constraints

*No items available*

##### 1.11.3.7.9 Default Value



##### 1.11.3.7.10 Is Foreign Key

❌ No

##### 1.11.3.7.11 Precision

0

##### 1.11.3.7.12 Scale

0

#### 1.11.3.8.0 DateTimeOffset

##### 1.11.3.8.1 Name

activeTimestamp

##### 1.11.3.8.2 Type

🔹 DateTimeOffset

##### 1.11.3.8.3 Is Required

✅ Yes

##### 1.11.3.8.4 Is Primary Key

❌ No

##### 1.11.3.8.5 Is Unique

❌ No

##### 1.11.3.8.6 Index Type

Index

##### 1.11.3.8.7 Size

0

##### 1.11.3.8.8 Constraints

*No items available*

##### 1.11.3.8.9 Default Value



##### 1.11.3.8.10 Is Foreign Key

❌ No

##### 1.11.3.8.11 Precision

0

##### 1.11.3.8.12 Scale

0

#### 1.11.3.9.0 DateTimeOffset

##### 1.11.3.9.1 Name

acknowledgedTimestamp

##### 1.11.3.9.2 Type

🔹 DateTimeOffset

##### 1.11.3.9.3 Is Required

❌ No

##### 1.11.3.9.4 Is Primary Key

❌ No

##### 1.11.3.9.5 Is Unique

❌ No

##### 1.11.3.9.6 Index Type

None

##### 1.11.3.9.7 Size

0

##### 1.11.3.9.8 Constraints

*No items available*

##### 1.11.3.9.9 Default Value



##### 1.11.3.9.10 Is Foreign Key

❌ No

##### 1.11.3.9.11 Precision

0

##### 1.11.3.9.12 Scale

0

#### 1.11.3.10.0 DateTimeOffset

##### 1.11.3.10.1 Name

shelvedUntilTimestamp

##### 1.11.3.10.2 Type

🔹 DateTimeOffset

##### 1.11.3.10.3 Is Required

❌ No

##### 1.11.3.10.4 Is Primary Key

❌ No

##### 1.11.3.10.5 Is Unique

❌ No

##### 1.11.3.10.6 Index Type

Index

##### 1.11.3.10.7 Size

0

##### 1.11.3.10.8 Constraints

*No items available*

##### 1.11.3.10.9 Default Value



##### 1.11.3.10.10 Is Foreign Key

❌ No

##### 1.11.3.10.11 Precision

0

##### 1.11.3.10.12 Scale

0

### 1.11.4.0.0 Primary Keys

- alarmId

### 1.11.5.0.0 Unique Constraints

*No items available*

### 1.11.6.0.0 Indexes

#### 1.11.6.1.0 BTree

##### 1.11.6.1.1 Name

IX_Alarm_OpcTagId

##### 1.11.6.1.2 Columns

- opcTagId

##### 1.11.6.1.3 Type

🔹 BTree

#### 1.11.6.2.0 BTree

##### 1.11.6.2.1 Name

IX_Alarm_Tenant_State_Severity_Timestamp

##### 1.11.6.2.2 Columns

- tenantId
- state
- severity DESC
- activeTimestamp DESC

##### 1.11.6.2.3 Type

🔹 BTree

#### 1.11.6.3.0 BTree

##### 1.11.6.3.1 Name

IX_Alarm_AssetId

##### 1.11.6.3.2 Columns

- assetId

##### 1.11.6.3.3 Type

🔹 BTree

## 1.12.0.0.0 AlarmHistory

### 1.12.1.0.0 Name

AlarmHistory

### 1.12.2.0.0 Description

Logs all state changes and actions for a given alarm. (REQ-1-036)

### 1.12.3.0.0 Attributes

#### 1.12.3.1.0 BIGINT

##### 1.12.3.1.1 Name

alarmHistoryId

##### 1.12.3.1.2 Type

🔹 BIGINT

##### 1.12.3.1.3 Is Required

✅ Yes

##### 1.12.3.1.4 Is Primary Key

✅ Yes

##### 1.12.3.1.5 Is Unique

✅ Yes

##### 1.12.3.1.6 Index Type

UniqueIndex

##### 1.12.3.1.7 Size

0

##### 1.12.3.1.8 Constraints

- AUTO_INCREMENT

##### 1.12.3.1.9 Default Value



##### 1.12.3.1.10 Is Foreign Key

❌ No

##### 1.12.3.1.11 Precision

0

##### 1.12.3.1.12 Scale

0

#### 1.12.3.2.0 Guid

##### 1.12.3.2.1 Name

alarmId

##### 1.12.3.2.2 Type

🔹 Guid

##### 1.12.3.2.3 Is Required

✅ Yes

##### 1.12.3.2.4 Is Primary Key

❌ No

##### 1.12.3.2.5 Is Unique

❌ No

##### 1.12.3.2.6 Index Type

Index

##### 1.12.3.2.7 Size

0

##### 1.12.3.2.8 Constraints

*No items available*

##### 1.12.3.2.9 Default Value



##### 1.12.3.2.10 Is Foreign Key

✅ Yes

##### 1.12.3.2.11 Precision

0

##### 1.12.3.2.12 Scale

0

#### 1.12.3.3.0 Guid

##### 1.12.3.3.1 Name

userId

##### 1.12.3.3.2 Type

🔹 Guid

##### 1.12.3.3.3 Is Required

✅ Yes

##### 1.12.3.3.4 Is Primary Key

❌ No

##### 1.12.3.3.5 Is Unique

❌ No

##### 1.12.3.3.6 Index Type

Index

##### 1.12.3.3.7 Size

0

##### 1.12.3.3.8 Constraints

*No items available*

##### 1.12.3.3.9 Default Value



##### 1.12.3.3.10 Is Foreign Key

✅ Yes

##### 1.12.3.3.11 Precision

0

##### 1.12.3.3.12 Scale

0

#### 1.12.3.4.0 VARCHAR

##### 1.12.3.4.1 Name

action

##### 1.12.3.4.2 Type

🔹 VARCHAR

##### 1.12.3.4.3 Is Required

✅ Yes

##### 1.12.3.4.4 Is Primary Key

❌ No

##### 1.12.3.4.5 Is Unique

❌ No

##### 1.12.3.4.6 Index Type

None

##### 1.12.3.4.7 Size

50

##### 1.12.3.4.8 Constraints

- ENUM('Acknowledge', 'Shelve', 'Unshelve', 'Suppress')

##### 1.12.3.4.9 Default Value



##### 1.12.3.4.10 Is Foreign Key

❌ No

##### 1.12.3.4.11 Precision

0

##### 1.12.3.4.12 Scale

0

#### 1.12.3.5.0 DateTimeOffset

##### 1.12.3.5.1 Name

timestamp

##### 1.12.3.5.2 Type

🔹 DateTimeOffset

##### 1.12.3.5.3 Is Required

✅ Yes

##### 1.12.3.5.4 Is Primary Key

❌ No

##### 1.12.3.5.5 Is Unique

❌ No

##### 1.12.3.5.6 Index Type

Index

##### 1.12.3.5.7 Size

0

##### 1.12.3.5.8 Constraints

*No items available*

##### 1.12.3.5.9 Default Value

CURRENT_TIMESTAMP

##### 1.12.3.5.10 Is Foreign Key

❌ No

##### 1.12.3.5.11 Precision

0

##### 1.12.3.5.12 Scale

0

#### 1.12.3.6.0 TEXT

##### 1.12.3.6.1 Name

comment

##### 1.12.3.6.2 Type

🔹 TEXT

##### 1.12.3.6.3 Is Required

❌ No

##### 1.12.3.6.4 Is Primary Key

❌ No

##### 1.12.3.6.5 Is Unique

❌ No

##### 1.12.3.6.6 Index Type

None

##### 1.12.3.6.7 Size

0

##### 1.12.3.6.8 Constraints

*No items available*

##### 1.12.3.6.9 Default Value



##### 1.12.3.6.10 Is Foreign Key

❌ No

##### 1.12.3.6.11 Precision

0

##### 1.12.3.6.12 Scale

0

### 1.12.4.0.0 Primary Keys

- alarmHistoryId

### 1.12.5.0.0 Unique Constraints

*No items available*

### 1.12.6.0.0 Indexes

#### 1.12.6.1.0 BTree

##### 1.12.6.1.1 Name

IX_AlarmHistory_Alarm_Timestamp

##### 1.12.6.1.2 Columns

- alarmId
- timestamp

##### 1.12.6.1.3 Type

🔹 BTree

#### 1.12.6.2.0 BTree

##### 1.12.6.2.1 Name

IX_AlarmHistory_User_Timestamp

##### 1.12.6.2.2 Columns

- userId
- timestamp

##### 1.12.6.2.3 Type

🔹 BTree

### 1.12.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Range |
| Column | timestamp |
| Strategy | Monthly |
| Comment | Manages growth of historical alarm event data. |

## 1.13.0.0.0 AiModel

### 1.13.1.0.0 Name

AiModel

### 1.13.2.0.0 Description

Represents a machine learning model available in the system. (REQ-1-049)

### 1.13.3.0.0 Attributes

#### 1.13.3.1.0 Guid

##### 1.13.3.1.1 Name

aiModelId

##### 1.13.3.1.2 Type

🔹 Guid

##### 1.13.3.1.3 Is Required

✅ Yes

##### 1.13.3.1.4 Is Primary Key

✅ Yes

##### 1.13.3.1.5 Is Unique

✅ Yes

##### 1.13.3.1.6 Index Type

UniqueIndex

##### 1.13.3.1.7 Size

0

##### 1.13.3.1.8 Constraints

*No items available*

##### 1.13.3.1.9 Default Value



##### 1.13.3.1.10 Is Foreign Key

❌ No

##### 1.13.3.1.11 Precision

0

##### 1.13.3.1.12 Scale

0

#### 1.13.3.2.0 Guid

##### 1.13.3.2.1 Name

tenantId

##### 1.13.3.2.2 Type

🔹 Guid

##### 1.13.3.2.3 Is Required

✅ Yes

##### 1.13.3.2.4 Is Primary Key

❌ No

##### 1.13.3.2.5 Is Unique

❌ No

##### 1.13.3.2.6 Index Type

Index

##### 1.13.3.2.7 Size

0

##### 1.13.3.2.8 Constraints

*No items available*

##### 1.13.3.2.9 Default Value



##### 1.13.3.2.10 Is Foreign Key

✅ Yes

##### 1.13.3.2.11 Precision

0

##### 1.13.3.2.12 Scale

0

#### 1.13.3.3.0 VARCHAR

##### 1.13.3.3.1 Name

name

##### 1.13.3.3.2 Type

🔹 VARCHAR

##### 1.13.3.3.3 Is Required

✅ Yes

##### 1.13.3.3.4 Is Primary Key

❌ No

##### 1.13.3.3.5 Is Unique

❌ No

##### 1.13.3.3.6 Index Type

Index

##### 1.13.3.3.7 Size

255

##### 1.13.3.3.8 Constraints

*No items available*

##### 1.13.3.3.9 Default Value



##### 1.13.3.3.10 Is Foreign Key

❌ No

##### 1.13.3.3.11 Precision

0

##### 1.13.3.3.12 Scale

0

#### 1.13.3.4.0 TEXT

##### 1.13.3.4.1 Name

description

##### 1.13.3.4.2 Type

🔹 TEXT

##### 1.13.3.4.3 Is Required

❌ No

##### 1.13.3.4.4 Is Primary Key

❌ No

##### 1.13.3.4.5 Is Unique

❌ No

##### 1.13.3.4.6 Index Type

None

##### 1.13.3.4.7 Size

0

##### 1.13.3.4.8 Constraints

*No items available*

##### 1.13.3.4.9 Default Value



##### 1.13.3.4.10 Is Foreign Key

❌ No

##### 1.13.3.4.11 Precision

0

##### 1.13.3.4.12 Scale

0

#### 1.13.3.5.0 VARCHAR

##### 1.13.3.5.1 Name

modelType

##### 1.13.3.5.2 Type

🔹 VARCHAR

##### 1.13.3.5.3 Is Required

✅ Yes

##### 1.13.3.5.4 Is Primary Key

❌ No

##### 1.13.3.5.5 Is Unique

❌ No

##### 1.13.3.5.6 Index Type

Index

##### 1.13.3.5.7 Size

100

##### 1.13.3.5.8 Constraints

- ENUM('PredictiveMaintenance', 'AnomalyDetection')

##### 1.13.3.5.9 Default Value



##### 1.13.3.5.10 Is Foreign Key

❌ No

##### 1.13.3.5.11 Precision

0

##### 1.13.3.5.12 Scale

0

#### 1.13.3.6.0 DateTimeOffset

##### 1.13.3.6.1 Name

createdAt

##### 1.13.3.6.2 Type

🔹 DateTimeOffset

##### 1.13.3.6.3 Is Required

✅ Yes

##### 1.13.3.6.4 Is Primary Key

❌ No

##### 1.13.3.6.5 Is Unique

❌ No

##### 1.13.3.6.6 Index Type

Index

##### 1.13.3.6.7 Size

0

##### 1.13.3.6.8 Constraints

*No items available*

##### 1.13.3.6.9 Default Value

CURRENT_TIMESTAMP

##### 1.13.3.6.10 Is Foreign Key

❌ No

##### 1.13.3.6.11 Precision

0

##### 1.13.3.6.12 Scale

0

### 1.13.4.0.0 Primary Keys

- aiModelId

### 1.13.5.0.0 Unique Constraints

- {'name': 'UC_AiModel_Tenant_Name', 'columns': ['tenantId', 'name']}

### 1.13.6.0.0 Indexes

- {'name': 'IX_AiModel_Tenant_ModelType', 'columns': ['tenantId', 'modelType'], 'type': 'BTree'}

## 1.14.0.0.0 AiModelVersion

### 1.14.1.0.0 Name

AiModelVersion

### 1.14.2.0.0 Description

Manages versions of an AI model, including its file and approval status. (REQ-1-050)

### 1.14.3.0.0 Attributes

#### 1.14.3.1.0 Guid

##### 1.14.3.1.1 Name

aiModelVersionId

##### 1.14.3.1.2 Type

🔹 Guid

##### 1.14.3.1.3 Is Required

✅ Yes

##### 1.14.3.1.4 Is Primary Key

✅ Yes

##### 1.14.3.1.5 Is Unique

✅ Yes

##### 1.14.3.1.6 Index Type

UniqueIndex

##### 1.14.3.1.7 Size

0

##### 1.14.3.1.8 Constraints

*No items available*

##### 1.14.3.1.9 Default Value



##### 1.14.3.1.10 Is Foreign Key

❌ No

##### 1.14.3.1.11 Precision

0

##### 1.14.3.1.12 Scale

0

#### 1.14.3.2.0 Guid

##### 1.14.3.2.1 Name

aiModelId

##### 1.14.3.2.2 Type

🔹 Guid

##### 1.14.3.2.3 Is Required

✅ Yes

##### 1.14.3.2.4 Is Primary Key

❌ No

##### 1.14.3.2.5 Is Unique

❌ No

##### 1.14.3.2.6 Index Type

Index

##### 1.14.3.2.7 Size

0

##### 1.14.3.2.8 Constraints

*No items available*

##### 1.14.3.2.9 Default Value



##### 1.14.3.2.10 Is Foreign Key

✅ Yes

##### 1.14.3.2.11 Precision

0

##### 1.14.3.2.12 Scale

0

#### 1.14.3.3.0 VARCHAR

##### 1.14.3.3.1 Name

version

##### 1.14.3.3.2 Type

🔹 VARCHAR

##### 1.14.3.3.3 Is Required

✅ Yes

##### 1.14.3.3.4 Is Primary Key

❌ No

##### 1.14.3.3.5 Is Unique

❌ No

##### 1.14.3.3.6 Index Type

Index

##### 1.14.3.3.7 Size

50

##### 1.14.3.3.8 Constraints

*No items available*

##### 1.14.3.3.9 Default Value



##### 1.14.3.3.10 Is Foreign Key

❌ No

##### 1.14.3.3.11 Precision

0

##### 1.14.3.3.12 Scale

0

#### 1.14.3.4.0 VARCHAR

##### 1.14.3.4.1 Name

status

##### 1.14.3.4.2 Type

🔹 VARCHAR

##### 1.14.3.4.3 Is Required

✅ Yes

##### 1.14.3.4.4 Is Primary Key

❌ No

##### 1.14.3.4.5 Is Unique

❌ No

##### 1.14.3.4.6 Index Type

Index

##### 1.14.3.4.7 Size

50

##### 1.14.3.4.8 Constraints

- ENUM('PendingApproval', 'Approved', 'Rejected', 'Archived')

##### 1.14.3.4.9 Default Value

PendingApproval

##### 1.14.3.4.10 Is Foreign Key

❌ No

##### 1.14.3.4.11 Precision

0

##### 1.14.3.4.12 Scale

0

#### 1.14.3.5.0 VARCHAR

##### 1.14.3.5.1 Name

storagePath

##### 1.14.3.5.2 Type

🔹 VARCHAR

##### 1.14.3.5.3 Is Required

✅ Yes

##### 1.14.3.5.4 Is Primary Key

❌ No

##### 1.14.3.5.5 Is Unique

❌ No

##### 1.14.3.5.6 Index Type

None

##### 1.14.3.5.7 Size

1,024

##### 1.14.3.5.8 Constraints

*No items available*

##### 1.14.3.5.9 Default Value



##### 1.14.3.5.10 Is Foreign Key

❌ No

##### 1.14.3.5.11 Precision

0

##### 1.14.3.5.12 Scale

0

#### 1.14.3.6.0 VARCHAR

##### 1.14.3.6.1 Name

modelFormat

##### 1.14.3.6.2 Type

🔹 VARCHAR

##### 1.14.3.6.3 Is Required

✅ Yes

##### 1.14.3.6.4 Is Primary Key

❌ No

##### 1.14.3.6.5 Is Unique

❌ No

##### 1.14.3.6.6 Index Type

None

##### 1.14.3.6.7 Size

20

##### 1.14.3.6.8 Constraints

- ENUM('ONNX')

##### 1.14.3.6.9 Default Value

ONNX

##### 1.14.3.6.10 Is Foreign Key

❌ No

##### 1.14.3.6.11 Precision

0

##### 1.14.3.6.12 Scale

0

#### 1.14.3.7.0 Guid

##### 1.14.3.7.1 Name

submittedByUserId

##### 1.14.3.7.2 Type

🔹 Guid

##### 1.14.3.7.3 Is Required

✅ Yes

##### 1.14.3.7.4 Is Primary Key

❌ No

##### 1.14.3.7.5 Is Unique

❌ No

##### 1.14.3.7.6 Index Type

Index

##### 1.14.3.7.7 Size

0

##### 1.14.3.7.8 Constraints

*No items available*

##### 1.14.3.7.9 Default Value



##### 1.14.3.7.10 Is Foreign Key

✅ Yes

##### 1.14.3.7.11 Precision

0

##### 1.14.3.7.12 Scale

0

#### 1.14.3.8.0 Guid

##### 1.14.3.8.1 Name

approvedByUserId

##### 1.14.3.8.2 Type

🔹 Guid

##### 1.14.3.8.3 Is Required

❌ No

##### 1.14.3.8.4 Is Primary Key

❌ No

##### 1.14.3.8.5 Is Unique

❌ No

##### 1.14.3.8.6 Index Type

Index

##### 1.14.3.8.7 Size

0

##### 1.14.3.8.8 Constraints

*No items available*

##### 1.14.3.8.9 Default Value



##### 1.14.3.8.10 Is Foreign Key

✅ Yes

##### 1.14.3.8.11 Precision

0

##### 1.14.3.8.12 Scale

0

#### 1.14.3.9.0 DateTimeOffset

##### 1.14.3.9.1 Name

createdAt

##### 1.14.3.9.2 Type

🔹 DateTimeOffset

##### 1.14.3.9.3 Is Required

✅ Yes

##### 1.14.3.9.4 Is Primary Key

❌ No

##### 1.14.3.9.5 Is Unique

❌ No

##### 1.14.3.9.6 Index Type

Index

##### 1.14.3.9.7 Size

0

##### 1.14.3.9.8 Constraints

*No items available*

##### 1.14.3.9.9 Default Value

CURRENT_TIMESTAMP

##### 1.14.3.9.10 Is Foreign Key

❌ No

##### 1.14.3.9.11 Precision

0

##### 1.14.3.9.12 Scale

0

### 1.14.4.0.0 Primary Keys

- aiModelVersionId

### 1.14.5.0.0 Unique Constraints

- {'name': 'UC_AiModelVersion_Model_Version', 'columns': ['aiModelId', 'version']}

### 1.14.6.0.0 Indexes

#### 1.14.6.1.0 BTree

##### 1.14.6.1.1 Name

IX_AiModelVersion_AiModelId

##### 1.14.6.1.2 Columns

- aiModelId

##### 1.14.6.1.3 Type

🔹 BTree

#### 1.14.6.2.0 BTree

##### 1.14.6.2.1 Name

IX_AiModelVersion_Status

##### 1.14.6.2.2 Columns

- status

##### 1.14.6.2.3 Type

🔹 BTree

## 1.15.0.0.0 ModelAssignment

### 1.15.1.0.0 Name

ModelAssignment

### 1.15.2.0.0 Description

Links a specific AI model version to an asset for execution. (REQ-1-014, REQ-1-056)

### 1.15.3.0.0 Attributes

#### 1.15.3.1.0 Guid

##### 1.15.3.1.1 Name

modelAssignmentId

##### 1.15.3.1.2 Type

🔹 Guid

##### 1.15.3.1.3 Is Required

✅ Yes

##### 1.15.3.1.4 Is Primary Key

✅ Yes

##### 1.15.3.1.5 Is Unique

✅ Yes

##### 1.15.3.1.6 Index Type

UniqueIndex

##### 1.15.3.1.7 Size

0

##### 1.15.3.1.8 Constraints

*No items available*

##### 1.15.3.1.9 Default Value



##### 1.15.3.1.10 Is Foreign Key

❌ No

##### 1.15.3.1.11 Precision

0

##### 1.15.3.1.12 Scale

0

#### 1.15.3.2.0 Guid

##### 1.15.3.2.1 Name

assetId

##### 1.15.3.2.2 Type

🔹 Guid

##### 1.15.3.2.3 Is Required

✅ Yes

##### 1.15.3.2.4 Is Primary Key

❌ No

##### 1.15.3.2.5 Is Unique

❌ No

##### 1.15.3.2.6 Index Type

Index

##### 1.15.3.2.7 Size

0

##### 1.15.3.2.8 Constraints

*No items available*

##### 1.15.3.2.9 Default Value



##### 1.15.3.2.10 Is Foreign Key

✅ Yes

##### 1.15.3.2.11 Precision

0

##### 1.15.3.2.12 Scale

0

#### 1.15.3.3.0 Guid

##### 1.15.3.3.1 Name

aiModelVersionId

##### 1.15.3.3.2 Type

🔹 Guid

##### 1.15.3.3.3 Is Required

✅ Yes

##### 1.15.3.3.4 Is Primary Key

❌ No

##### 1.15.3.3.5 Is Unique

❌ No

##### 1.15.3.3.6 Index Type

Index

##### 1.15.3.3.7 Size

0

##### 1.15.3.3.8 Constraints

*No items available*

##### 1.15.3.3.9 Default Value



##### 1.15.3.3.10 Is Foreign Key

✅ Yes

##### 1.15.3.3.11 Precision

0

##### 1.15.3.3.12 Scale

0

#### 1.15.3.4.0 Guid

##### 1.15.3.4.1 Name

opcCoreClientId

##### 1.15.3.4.2 Type

🔹 Guid

##### 1.15.3.4.3 Is Required

✅ Yes

##### 1.15.3.4.4 Is Primary Key

❌ No

##### 1.15.3.4.5 Is Unique

❌ No

##### 1.15.3.4.6 Index Type

Index

##### 1.15.3.4.7 Size

0

##### 1.15.3.4.8 Constraints

*No items available*

##### 1.15.3.4.9 Default Value



##### 1.15.3.4.10 Is Foreign Key

✅ Yes

##### 1.15.3.4.11 Precision

0

##### 1.15.3.4.12 Scale

0

#### 1.15.3.5.0 BOOLEAN

##### 1.15.3.5.1 Name

isActive

##### 1.15.3.5.2 Type

🔹 BOOLEAN

##### 1.15.3.5.3 Is Required

✅ Yes

##### 1.15.3.5.4 Is Primary Key

❌ No

##### 1.15.3.5.5 Is Unique

❌ No

##### 1.15.3.5.6 Index Type

Index

##### 1.15.3.5.7 Size

0

##### 1.15.3.5.8 Constraints

*No items available*

##### 1.15.3.5.9 Default Value

true

##### 1.15.3.5.10 Is Foreign Key

❌ No

##### 1.15.3.5.11 Precision

0

##### 1.15.3.5.12 Scale

0

#### 1.15.3.6.0 DateTimeOffset

##### 1.15.3.6.1 Name

assignedAt

##### 1.15.3.6.2 Type

🔹 DateTimeOffset

##### 1.15.3.6.3 Is Required

✅ Yes

##### 1.15.3.6.4 Is Primary Key

❌ No

##### 1.15.3.6.5 Is Unique

❌ No

##### 1.15.3.6.6 Index Type

None

##### 1.15.3.6.7 Size

0

##### 1.15.3.6.8 Constraints

*No items available*

##### 1.15.3.6.9 Default Value

CURRENT_TIMESTAMP

##### 1.15.3.6.10 Is Foreign Key

❌ No

##### 1.15.3.6.11 Precision

0

##### 1.15.3.6.12 Scale

0

### 1.15.4.0.0 Primary Keys

- modelAssignmentId

### 1.15.5.0.0 Unique Constraints

- {'name': 'UC_ModelAssignment_Asset_Version', 'columns': ['assetId', 'aiModelVersionId']}

### 1.15.6.0.0 Indexes

#### 1.15.6.1.0 BTree

##### 1.15.6.1.1 Name

IX_ModelAssignment_AssetId

##### 1.15.6.1.2 Columns

- assetId

##### 1.15.6.1.3 Type

🔹 BTree

#### 1.15.6.2.0 BTree

##### 1.15.6.2.1 Name

IX_ModelAssignment_AiModelVersionId

##### 1.15.6.2.2 Columns

- aiModelVersionId

##### 1.15.6.2.3 Type

🔹 BTree

#### 1.15.6.3.0 BTree

##### 1.15.6.3.1 Name

IX_ModelAssignment_OpcCoreClientId

##### 1.15.6.3.2 Columns

- opcCoreClientId

##### 1.15.6.3.3 Type

🔹 BTree

## 1.16.0.0.0 AnomalyEvent

### 1.16.1.0.0 Name

AnomalyEvent

### 1.16.2.0.0 Description

Records an anomaly detected by an AI model. (REQ-1-052)

### 1.16.3.0.0 Attributes

#### 1.16.3.1.0 Guid

##### 1.16.3.1.1 Name

anomalyEventId

##### 1.16.3.1.2 Type

🔹 Guid

##### 1.16.3.1.3 Is Required

✅ Yes

##### 1.16.3.1.4 Is Primary Key

✅ Yes

##### 1.16.3.1.5 Is Unique

✅ Yes

##### 1.16.3.1.6 Index Type

UniqueIndex

##### 1.16.3.1.7 Size

0

##### 1.16.3.1.8 Constraints

*No items available*

##### 1.16.3.1.9 Default Value



##### 1.16.3.1.10 Is Foreign Key

❌ No

##### 1.16.3.1.11 Precision

0

##### 1.16.3.1.12 Scale

0

#### 1.16.3.2.0 Guid

##### 1.16.3.2.1 Name

modelAssignmentId

##### 1.16.3.2.2 Type

🔹 Guid

##### 1.16.3.2.3 Is Required

✅ Yes

##### 1.16.3.2.4 Is Primary Key

❌ No

##### 1.16.3.2.5 Is Unique

❌ No

##### 1.16.3.2.6 Index Type

Index

##### 1.16.3.2.7 Size

0

##### 1.16.3.2.8 Constraints

*No items available*

##### 1.16.3.2.9 Default Value



##### 1.16.3.2.10 Is Foreign Key

✅ Yes

##### 1.16.3.2.11 Precision

0

##### 1.16.3.2.12 Scale

0

#### 1.16.3.3.0 DateTimeOffset

##### 1.16.3.3.1 Name

timestamp

##### 1.16.3.3.2 Type

🔹 DateTimeOffset

##### 1.16.3.3.3 Is Required

✅ Yes

##### 1.16.3.3.4 Is Primary Key

❌ No

##### 1.16.3.3.5 Is Unique

❌ No

##### 1.16.3.3.6 Index Type

Index

##### 1.16.3.3.7 Size

0

##### 1.16.3.3.8 Constraints

*No items available*

##### 1.16.3.3.9 Default Value



##### 1.16.3.3.10 Is Foreign Key

❌ No

##### 1.16.3.3.11 Precision

0

##### 1.16.3.3.12 Scale

0

#### 1.16.3.4.0 DOUBLE PRECISION

##### 1.16.3.4.1 Name

anomalyScore

##### 1.16.3.4.2 Type

🔹 DOUBLE PRECISION

##### 1.16.3.4.3 Is Required

✅ Yes

##### 1.16.3.4.4 Is Primary Key

❌ No

##### 1.16.3.4.5 Is Unique

❌ No

##### 1.16.3.4.6 Index Type

None

##### 1.16.3.4.7 Size

0

##### 1.16.3.4.8 Constraints

*No items available*

##### 1.16.3.4.9 Default Value



##### 1.16.3.4.10 Is Foreign Key

❌ No

##### 1.16.3.4.11 Precision

0

##### 1.16.3.4.12 Scale

0

#### 1.16.3.5.0 BOOLEAN

##### 1.16.3.5.1 Name

isTrueAnomaly

##### 1.16.3.5.2 Type

🔹 BOOLEAN

##### 1.16.3.5.3 Is Required

❌ No

##### 1.16.3.5.4 Is Primary Key

❌ No

##### 1.16.3.5.5 Is Unique

❌ No

##### 1.16.3.5.6 Index Type

Index

##### 1.16.3.5.7 Size

0

##### 1.16.3.5.8 Constraints

*No items available*

##### 1.16.3.5.9 Default Value



##### 1.16.3.5.10 Is Foreign Key

❌ No

##### 1.16.3.5.11 Precision

0

##### 1.16.3.5.12 Scale

0

#### 1.16.3.6.0 TEXT

##### 1.16.3.6.1 Name

feedbackComment

##### 1.16.3.6.2 Type

🔹 TEXT

##### 1.16.3.6.3 Is Required

❌ No

##### 1.16.3.6.4 Is Primary Key

❌ No

##### 1.16.3.6.5 Is Unique

❌ No

##### 1.16.3.6.6 Index Type

None

##### 1.16.3.6.7 Size

0

##### 1.16.3.6.8 Constraints

*No items available*

##### 1.16.3.6.9 Default Value



##### 1.16.3.6.10 Is Foreign Key

❌ No

##### 1.16.3.6.11 Precision

0

##### 1.16.3.6.12 Scale

0

### 1.16.4.0.0 Primary Keys

- anomalyEventId

### 1.16.5.0.0 Unique Constraints

*No items available*

### 1.16.6.0.0 Indexes

#### 1.16.6.1.0 BTree

##### 1.16.6.1.1 Name

IX_AnomalyEvent_Assignment_Timestamp

##### 1.16.6.1.2 Columns

- modelAssignmentId
- timestamp

##### 1.16.6.1.3 Type

🔹 BTree

#### 1.16.6.2.0 BTree

##### 1.16.6.2.1 Name

IX_AnomalyEvent_IsTrueAnomaly

##### 1.16.6.2.2 Columns

- isTrueAnomaly

##### 1.16.6.2.3 Type

🔹 BTree

### 1.16.7.0.0 Partitioning

| Property | Value |
|----------|-------|
| Type | Range |
| Column | timestamp |
| Strategy | Monthly |
| Comment | Improves query performance for analyzing anomaly t... |

## 1.17.0.0.0 License

### 1.17.1.0.0 Name

License

### 1.17.2.0.0 Description

Manages software licenses assigned to tenants. (REQ-1-063)

### 1.17.3.0.0 Attributes

#### 1.17.3.1.0 Guid

##### 1.17.3.1.1 Name

licenseId

##### 1.17.3.1.2 Type

🔹 Guid

##### 1.17.3.1.3 Is Required

✅ Yes

##### 1.17.3.1.4 Is Primary Key

✅ Yes

##### 1.17.3.1.5 Is Unique

✅ Yes

##### 1.17.3.1.6 Index Type

UniqueIndex

##### 1.17.3.1.7 Size

0

##### 1.17.3.1.8 Constraints

*No items available*

##### 1.17.3.1.9 Default Value



##### 1.17.3.1.10 Is Foreign Key

❌ No

##### 1.17.3.1.11 Precision

0

##### 1.17.3.1.12 Scale

0

#### 1.17.3.2.0 Guid

##### 1.17.3.2.1 Name

tenantId

##### 1.17.3.2.2 Type

🔹 Guid

##### 1.17.3.2.3 Is Required

✅ Yes

##### 1.17.3.2.4 Is Primary Key

❌ No

##### 1.17.3.2.5 Is Unique

✅ Yes

##### 1.17.3.2.6 Index Type

UniqueIndex

##### 1.17.3.2.7 Size

0

##### 1.17.3.2.8 Constraints

*No items available*

##### 1.17.3.2.9 Default Value



##### 1.17.3.2.10 Is Foreign Key

✅ Yes

##### 1.17.3.2.11 Precision

0

##### 1.17.3.2.12 Scale

0

#### 1.17.3.3.0 VARCHAR

##### 1.17.3.3.1 Name

licenseTier

##### 1.17.3.3.2 Type

🔹 VARCHAR

##### 1.17.3.3.3 Is Required

✅ Yes

##### 1.17.3.3.4 Is Primary Key

❌ No

##### 1.17.3.3.5 Is Unique

❌ No

##### 1.17.3.3.6 Index Type

Index

##### 1.17.3.3.7 Size

50

##### 1.17.3.3.8 Constraints

*No items available*

##### 1.17.3.3.9 Default Value

Silver

##### 1.17.3.3.10 Is Foreign Key

❌ No

##### 1.17.3.3.11 Precision

0

##### 1.17.3.3.12 Scale

0

#### 1.17.3.4.0 VARCHAR

##### 1.17.3.4.1 Name

licenseKey

##### 1.17.3.4.2 Type

🔹 VARCHAR

##### 1.17.3.4.3 Is Required

✅ Yes

##### 1.17.3.4.4 Is Primary Key

❌ No

##### 1.17.3.4.5 Is Unique

✅ Yes

##### 1.17.3.4.6 Index Type

UniqueIndex

##### 1.17.3.4.7 Size

255

##### 1.17.3.4.8 Constraints

*No items available*

##### 1.17.3.4.9 Default Value



##### 1.17.3.4.10 Is Foreign Key

❌ No

##### 1.17.3.4.11 Precision

0

##### 1.17.3.4.12 Scale

0

#### 1.17.3.5.0 DateTimeOffset

##### 1.17.3.5.1 Name

validFrom

##### 1.17.3.5.2 Type

🔹 DateTimeOffset

##### 1.17.3.5.3 Is Required

✅ Yes

##### 1.17.3.5.4 Is Primary Key

❌ No

##### 1.17.3.5.5 Is Unique

❌ No

##### 1.17.3.5.6 Index Type

None

##### 1.17.3.5.7 Size

0

##### 1.17.3.5.8 Constraints

*No items available*

##### 1.17.3.5.9 Default Value



##### 1.17.3.5.10 Is Foreign Key

❌ No

##### 1.17.3.5.11 Precision

0

##### 1.17.3.5.12 Scale

0

#### 1.17.3.6.0 DateTimeOffset

##### 1.17.3.6.1 Name

validTo

##### 1.17.3.6.2 Type

🔹 DateTimeOffset

##### 1.17.3.6.3 Is Required

✅ Yes

##### 1.17.3.6.4 Is Primary Key

❌ No

##### 1.17.3.6.5 Is Unique

❌ No

##### 1.17.3.6.6 Index Type

Index

##### 1.17.3.6.7 Size

0

##### 1.17.3.6.8 Constraints

*No items available*

##### 1.17.3.6.9 Default Value



##### 1.17.3.6.10 Is Foreign Key

❌ No

##### 1.17.3.6.11 Precision

0

##### 1.17.3.6.12 Scale

0

#### 1.17.3.7.0 INT

##### 1.17.3.7.1 Name

maxUsers

##### 1.17.3.7.2 Type

🔹 INT

##### 1.17.3.7.3 Is Required

❌ No

##### 1.17.3.7.4 Is Primary Key

❌ No

##### 1.17.3.7.5 Is Unique

❌ No

##### 1.17.3.7.6 Index Type

None

##### 1.17.3.7.7 Size

0

##### 1.17.3.7.8 Constraints

*No items available*

##### 1.17.3.7.9 Default Value



##### 1.17.3.7.10 Is Foreign Key

❌ No

##### 1.17.3.7.11 Precision

0

##### 1.17.3.7.12 Scale

0

#### 1.17.3.8.0 INT

##### 1.17.3.8.1 Name

maxClients

##### 1.17.3.8.2 Type

🔹 INT

##### 1.17.3.8.3 Is Required

❌ No

##### 1.17.3.8.4 Is Primary Key

❌ No

##### 1.17.3.8.5 Is Unique

❌ No

##### 1.17.3.8.6 Index Type

None

##### 1.17.3.8.7 Size

0

##### 1.17.3.8.8 Constraints

*No items available*

##### 1.17.3.8.9 Default Value



##### 1.17.3.8.10 Is Foreign Key

❌ No

##### 1.17.3.8.11 Precision

0

##### 1.17.3.8.12 Scale

0

### 1.17.4.0.0 Primary Keys

- licenseId

### 1.17.5.0.0 Unique Constraints

#### 1.17.5.1.0 UC_License_TenantId

##### 1.17.5.1.1 Name

UC_License_TenantId

##### 1.17.5.1.2 Columns

- tenantId

#### 1.17.5.2.0 UC_License_LicenseKey

##### 1.17.5.2.1 Name

UC_License_LicenseKey

##### 1.17.5.2.2 Columns

- licenseKey

### 1.17.6.0.0 Indexes

- {'name': 'IX_License_ValidTo', 'columns': ['validTo'], 'type': 'BTree'}

## 1.18.0.0.0 ReportTemplate

### 1.18.1.0.0 Name

ReportTemplate

### 1.18.2.0.0 Description

Defines the configuration for automated reports. (REQ-1-065)

### 1.18.3.0.0 Attributes

#### 1.18.3.1.0 Guid

##### 1.18.3.1.1 Name

reportTemplateId

##### 1.18.3.1.2 Type

🔹 Guid

##### 1.18.3.1.3 Is Required

✅ Yes

##### 1.18.3.1.4 Is Primary Key

✅ Yes

##### 1.18.3.1.5 Is Unique

✅ Yes

##### 1.18.3.1.6 Index Type

UniqueIndex

##### 1.18.3.1.7 Size

0

##### 1.18.3.1.8 Constraints

*No items available*

##### 1.18.3.1.9 Default Value



##### 1.18.3.1.10 Is Foreign Key

❌ No

##### 1.18.3.1.11 Precision

0

##### 1.18.3.1.12 Scale

0

#### 1.18.3.2.0 Guid

##### 1.18.3.2.1 Name

tenantId

##### 1.18.3.2.2 Type

🔹 Guid

##### 1.18.3.2.3 Is Required

✅ Yes

##### 1.18.3.2.4 Is Primary Key

❌ No

##### 1.18.3.2.5 Is Unique

❌ No

##### 1.18.3.2.6 Index Type

Index

##### 1.18.3.2.7 Size

0

##### 1.18.3.2.8 Constraints

*No items available*

##### 1.18.3.2.9 Default Value



##### 1.18.3.2.10 Is Foreign Key

✅ Yes

##### 1.18.3.2.11 Precision

0

##### 1.18.3.2.12 Scale

0

#### 1.18.3.3.0 VARCHAR

##### 1.18.3.3.1 Name

name

##### 1.18.3.3.2 Type

🔹 VARCHAR

##### 1.18.3.3.3 Is Required

✅ Yes

##### 1.18.3.3.4 Is Primary Key

❌ No

##### 1.18.3.3.5 Is Unique

❌ No

##### 1.18.3.3.6 Index Type

Index

##### 1.18.3.3.7 Size

255

##### 1.18.3.3.8 Constraints

*No items available*

##### 1.18.3.3.9 Default Value



##### 1.18.3.3.10 Is Foreign Key

❌ No

##### 1.18.3.3.11 Precision

0

##### 1.18.3.3.12 Scale

0

#### 1.18.3.4.0 JSONB

##### 1.18.3.4.1 Name

configuration

##### 1.18.3.4.2 Type

🔹 JSONB

##### 1.18.3.4.3 Is Required

✅ Yes

##### 1.18.3.4.4 Is Primary Key

❌ No

##### 1.18.3.4.5 Is Unique

❌ No

##### 1.18.3.4.6 Index Type

None

##### 1.18.3.4.7 Size

0

##### 1.18.3.4.8 Constraints

*No items available*

##### 1.18.3.4.9 Default Value

{}

##### 1.18.3.4.10 Is Foreign Key

❌ No

##### 1.18.3.4.11 Precision

0

##### 1.18.3.4.12 Scale

0

#### 1.18.3.5.0 VARCHAR

##### 1.18.3.5.1 Name

schedule

##### 1.18.3.5.2 Type

🔹 VARCHAR

##### 1.18.3.5.3 Is Required

❌ No

##### 1.18.3.5.4 Is Primary Key

❌ No

##### 1.18.3.5.5 Is Unique

❌ No

##### 1.18.3.5.6 Index Type

Index

##### 1.18.3.5.7 Size

100

##### 1.18.3.5.8 Constraints

- CRON_FORMAT

##### 1.18.3.5.9 Default Value



##### 1.18.3.5.10 Is Foreign Key

❌ No

##### 1.18.3.5.11 Precision

0

##### 1.18.3.5.12 Scale

0

#### 1.18.3.6.0 VARCHAR

##### 1.18.3.6.1 Name

outputFormat

##### 1.18.3.6.2 Type

🔹 VARCHAR

##### 1.18.3.6.3 Is Required

✅ Yes

##### 1.18.3.6.4 Is Primary Key

❌ No

##### 1.18.3.6.5 Is Unique

❌ No

##### 1.18.3.6.6 Index Type

None

##### 1.18.3.6.7 Size

20

##### 1.18.3.6.8 Constraints

- ENUM('PDF', 'HTML', 'CSV')

##### 1.18.3.6.9 Default Value

PDF

##### 1.18.3.6.10 Is Foreign Key

❌ No

##### 1.18.3.6.11 Precision

0

##### 1.18.3.6.12 Scale

0

#### 1.18.3.7.0 JSONB

##### 1.18.3.7.1 Name

distributionList

##### 1.18.3.7.2 Type

🔹 JSONB

##### 1.18.3.7.3 Is Required

❌ No

##### 1.18.3.7.4 Is Primary Key

❌ No

##### 1.18.3.7.5 Is Unique

❌ No

##### 1.18.3.7.6 Index Type

None

##### 1.18.3.7.7 Size

0

##### 1.18.3.7.8 Constraints

*No items available*

##### 1.18.3.7.9 Default Value

[]

##### 1.18.3.7.10 Is Foreign Key

❌ No

##### 1.18.3.7.11 Precision

0

##### 1.18.3.7.12 Scale

0

### 1.18.4.0.0 Primary Keys

- reportTemplateId

### 1.18.5.0.0 Unique Constraints

- {'name': 'UC_ReportTemplate_Tenant_Name', 'columns': ['tenantId', 'name']}

### 1.18.6.0.0 Indexes

#### 1.18.6.1.0 BTree

##### 1.18.6.1.1 Name

IX_ReportTemplate_TenantId

##### 1.18.6.1.2 Columns

- tenantId

##### 1.18.6.1.3 Type

🔹 BTree

#### 1.18.6.2.0 BTree

##### 1.18.6.2.1 Name

IX_ReportTemplate_Schedule

##### 1.18.6.2.2 Columns

- schedule

##### 1.18.6.2.3 Type

🔹 BTree

## 1.19.0.0.0 ApprovalRequest

### 1.19.1.0.0 Name

ApprovalRequest

### 1.19.2.0.0 Description

Tracks a request for a critical system change under an MOC workflow. (REQ-1-032)

### 1.19.3.0.0 Attributes

#### 1.19.3.1.0 Guid

##### 1.19.3.1.1 Name

approvalRequestId

##### 1.19.3.1.2 Type

🔹 Guid

##### 1.19.3.1.3 Is Required

✅ Yes

##### 1.19.3.1.4 Is Primary Key

✅ Yes

##### 1.19.3.1.5 Is Unique

✅ Yes

##### 1.19.3.1.6 Index Type

UniqueIndex

##### 1.19.3.1.7 Size

0

##### 1.19.3.1.8 Constraints

*No items available*

##### 1.19.3.1.9 Default Value



##### 1.19.3.1.10 Is Foreign Key

❌ No

##### 1.19.3.1.11 Precision

0

##### 1.19.3.1.12 Scale

0

#### 1.19.3.2.0 Guid

##### 1.19.3.2.1 Name

tenantId

##### 1.19.3.2.2 Type

🔹 Guid

##### 1.19.3.2.3 Is Required

✅ Yes

##### 1.19.3.2.4 Is Primary Key

❌ No

##### 1.19.3.2.5 Is Unique

❌ No

##### 1.19.3.2.6 Index Type

Index

##### 1.19.3.2.7 Size

0

##### 1.19.3.2.8 Constraints

*No items available*

##### 1.19.3.2.9 Default Value



##### 1.19.3.2.10 Is Foreign Key

✅ Yes

##### 1.19.3.2.11 Precision

0

##### 1.19.3.2.12 Scale

0

#### 1.19.3.3.0 VARCHAR

##### 1.19.3.3.1 Name

requestType

##### 1.19.3.3.2 Type

🔹 VARCHAR

##### 1.19.3.3.3 Is Required

✅ Yes

##### 1.19.3.3.4 Is Primary Key

❌ No

##### 1.19.3.3.5 Is Unique

❌ No

##### 1.19.3.3.6 Index Type

Index

##### 1.19.3.3.7 Size

100

##### 1.19.3.3.8 Constraints

- ENUM('DeployAiModel', 'ModifyAlarmPriority', 'ChangeSecurityPolicy')

##### 1.19.3.3.9 Default Value



##### 1.19.3.3.10 Is Foreign Key

❌ No

##### 1.19.3.3.11 Precision

0

##### 1.19.3.3.12 Scale

0

#### 1.19.3.4.0 VARCHAR

##### 1.19.3.4.1 Name

status

##### 1.19.3.4.2 Type

🔹 VARCHAR

##### 1.19.3.4.3 Is Required

✅ Yes

##### 1.19.3.4.4 Is Primary Key

❌ No

##### 1.19.3.4.5 Is Unique

❌ No

##### 1.19.3.4.6 Index Type

Index

##### 1.19.3.4.7 Size

50

##### 1.19.3.4.8 Constraints

- ENUM('Pending', 'Approved', 'Rejected')

##### 1.19.3.4.9 Default Value

Pending

##### 1.19.3.4.10 Is Foreign Key

❌ No

##### 1.19.3.4.11 Precision

0

##### 1.19.3.4.12 Scale

0

#### 1.19.3.5.0 Guid

##### 1.19.3.5.1 Name

requestedByUserId

##### 1.19.3.5.2 Type

🔹 Guid

##### 1.19.3.5.3 Is Required

✅ Yes

##### 1.19.3.5.4 Is Primary Key

❌ No

##### 1.19.3.5.5 Is Unique

❌ No

##### 1.19.3.5.6 Index Type

Index

##### 1.19.3.5.7 Size

0

##### 1.19.3.5.8 Constraints

*No items available*

##### 1.19.3.5.9 Default Value



##### 1.19.3.5.10 Is Foreign Key

✅ Yes

##### 1.19.3.5.11 Precision

0

##### 1.19.3.5.12 Scale

0

#### 1.19.3.6.0 Guid

##### 1.19.3.6.1 Name

approvedByUserId

##### 1.19.3.6.2 Type

🔹 Guid

##### 1.19.3.6.3 Is Required

❌ No

##### 1.19.3.6.4 Is Primary Key

❌ No

##### 1.19.3.6.5 Is Unique

❌ No

##### 1.19.3.6.6 Index Type

Index

##### 1.19.3.6.7 Size

0

##### 1.19.3.6.8 Constraints

*No items available*

##### 1.19.3.6.9 Default Value



##### 1.19.3.6.10 Is Foreign Key

✅ Yes

##### 1.19.3.6.11 Precision

0

##### 1.19.3.6.12 Scale

0

#### 1.19.3.7.0 JSONB

##### 1.19.3.7.1 Name

requestDetails

##### 1.19.3.7.2 Type

🔹 JSONB

##### 1.19.3.7.3 Is Required

✅ Yes

##### 1.19.3.7.4 Is Primary Key

❌ No

##### 1.19.3.7.5 Is Unique

❌ No

##### 1.19.3.7.6 Index Type

None

##### 1.19.3.7.7 Size

0

##### 1.19.3.7.8 Constraints

*No items available*

##### 1.19.3.7.9 Default Value

{}

##### 1.19.3.7.10 Is Foreign Key

❌ No

##### 1.19.3.7.11 Precision

0

##### 1.19.3.7.12 Scale

0

#### 1.19.3.8.0 DateTimeOffset

##### 1.19.3.8.1 Name

createdAt

##### 1.19.3.8.2 Type

🔹 DateTimeOffset

##### 1.19.3.8.3 Is Required

✅ Yes

##### 1.19.3.8.4 Is Primary Key

❌ No

##### 1.19.3.8.5 Is Unique

❌ No

##### 1.19.3.8.6 Index Type

Index

##### 1.19.3.8.7 Size

0

##### 1.19.3.8.8 Constraints

*No items available*

##### 1.19.3.8.9 Default Value

CURRENT_TIMESTAMP

##### 1.19.3.8.10 Is Foreign Key

❌ No

##### 1.19.3.8.11 Precision

0

##### 1.19.3.8.12 Scale

0

#### 1.19.3.9.0 DateTimeOffset

##### 1.19.3.9.1 Name

updatedAt

##### 1.19.3.9.2 Type

🔹 DateTimeOffset

##### 1.19.3.9.3 Is Required

✅ Yes

##### 1.19.3.9.4 Is Primary Key

❌ No

##### 1.19.3.9.5 Is Unique

❌ No

##### 1.19.3.9.6 Index Type

None

##### 1.19.3.9.7 Size

0

##### 1.19.3.9.8 Constraints

*No items available*

##### 1.19.3.9.9 Default Value

CURRENT_TIMESTAMP

##### 1.19.3.9.10 Is Foreign Key

❌ No

##### 1.19.3.9.11 Precision

0

##### 1.19.3.9.12 Scale

0

### 1.19.4.0.0 Primary Keys

- approvalRequestId

### 1.19.5.0.0 Unique Constraints

*No items available*

### 1.19.6.0.0 Indexes

#### 1.19.6.1.0 BTree

##### 1.19.6.1.1 Name

IX_ApprovalRequest_Tenant_Status_Type_Created

##### 1.19.6.1.2 Columns

- tenantId
- status
- requestType
- createdAt

##### 1.19.6.1.3 Type

🔹 BTree

#### 1.19.6.2.0 BTree

##### 1.19.6.2.1 Name

IX_ApprovalRequest_RequestedByUser

##### 1.19.6.2.2 Columns

- requestedByUserId

##### 1.19.6.2.3 Type

🔹 BTree

#### 1.19.6.3.0 BTree

##### 1.19.6.3.1 Name

IX_ApprovalRequest_ApprovedByUser

##### 1.19.6.3.2 Columns

- approvedByUserId

##### 1.19.6.3.3 Type

🔹 BTree

# 2.0.0.0.0 Relations

## 2.1.0.0.0 OneToMany

### 2.1.1.0.0 Name

TenantUsers

### 2.1.2.0.0 Id

REL_TENANT_USER_001

### 2.1.3.0.0 Source Entity

Tenant

### 2.1.4.0.0 Target Entity

User

### 2.1.5.0.0 Type

🔹 OneToMany

### 2.1.6.0.0 Source Multiplicity

1

### 2.1.7.0.0 Target Multiplicity

0..*

### 2.1.8.0.0 Cascade Delete

❌ No

### 2.1.9.0.0 Is Identifying

❌ No

### 2.1.10.0.0 On Delete

Restrict

### 2.1.11.0.0 On Update

Cascade

### 2.1.12.0.0 Join Table

#### 2.1.12.1.0 Name

N/A

#### 2.1.12.2.0 Columns

*No items available*

## 2.2.0.0.0 OneToMany

### 2.2.1.0.0 Name

TenantOpcCoreClients

### 2.2.2.0.0 Id

REL_TENANT_OPCCLIENT_001

### 2.2.3.0.0 Source Entity

Tenant

### 2.2.4.0.0 Target Entity

OpcCoreClient

### 2.2.5.0.0 Type

🔹 OneToMany

### 2.2.6.0.0 Source Multiplicity

1

### 2.2.7.0.0 Target Multiplicity

0..*

### 2.2.8.0.0 Cascade Delete

❌ No

### 2.2.9.0.0 Is Identifying

❌ No

### 2.2.10.0.0 On Delete

Restrict

### 2.2.11.0.0 On Update

Cascade

### 2.2.12.0.0 Join Table

#### 2.2.12.1.0 Name

N/A

#### 2.2.12.2.0 Columns

*No items available*

## 2.3.0.0.0 OneToMany

### 2.3.1.0.0 Name

TenantAssets

### 2.3.2.0.0 Id

REL_TENANT_ASSET_001

### 2.3.3.0.0 Source Entity

Tenant

### 2.3.4.0.0 Target Entity

Asset

### 2.3.5.0.0 Type

🔹 OneToMany

### 2.3.6.0.0 Source Multiplicity

1

### 2.3.7.0.0 Target Multiplicity

0..*

### 2.3.8.0.0 Cascade Delete

❌ No

### 2.3.9.0.0 Is Identifying

❌ No

### 2.3.10.0.0 On Delete

Restrict

### 2.3.11.0.0 On Update

Cascade

### 2.3.12.0.0 Join Table

#### 2.3.12.1.0 Name

N/A

#### 2.3.12.2.0 Columns

*No items available*

## 2.4.0.0.0 OneToMany

### 2.4.1.0.0 Name

TenantAuditLogs

### 2.4.2.0.0 Id

REL_TENANT_AUDITLOG_001

### 2.4.3.0.0 Source Entity

Tenant

### 2.4.4.0.0 Target Entity

AuditLog

### 2.4.5.0.0 Type

🔹 OneToMany

### 2.4.6.0.0 Source Multiplicity

1

### 2.4.7.0.0 Target Multiplicity

0..*

### 2.4.8.0.0 Cascade Delete

✅ Yes

### 2.4.9.0.0 Is Identifying

✅ Yes

### 2.4.10.0.0 On Delete

Cascade

### 2.4.11.0.0 On Update

Cascade

### 2.4.12.0.0 Join Table

#### 2.4.12.1.0 Name

N/A

#### 2.4.12.2.0 Columns

*No items available*

## 2.5.0.0.0 OneToMany

### 2.5.1.0.0 Name

UserAuditLogs

### 2.5.2.0.0 Id

REL_USER_AUDITLOG_001

### 2.5.3.0.0 Source Entity

User

### 2.5.4.0.0 Target Entity

AuditLog

### 2.5.5.0.0 Type

🔹 OneToMany

### 2.5.6.0.0 Source Multiplicity

1

### 2.5.7.0.0 Target Multiplicity

0..*

### 2.5.8.0.0 Cascade Delete

❌ No

### 2.5.9.0.0 Is Identifying

❌ No

### 2.5.10.0.0 On Delete

SetNull

### 2.5.11.0.0 On Update

Cascade

### 2.5.12.0.0 Join Table

#### 2.5.12.1.0 Name

N/A

#### 2.5.12.2.0 Columns

*No items available*

## 2.6.0.0.0 ManyToMany

### 2.6.1.0.0 Name

UserHasRoles

### 2.6.2.0.0 Id

REL_USER_ROLE_001

### 2.6.3.0.0 Source Entity

User

### 2.6.4.0.0 Target Entity

Role

### 2.6.5.0.0 Type

🔹 ManyToMany

### 2.6.6.0.0 Source Multiplicity

0..*

### 2.6.7.0.0 Target Multiplicity

0..*

### 2.6.8.0.0 Cascade Delete

❌ No

### 2.6.9.0.0 Is Identifying

❌ No

### 2.6.10.0.0 On Delete

Cascade

### 2.6.11.0.0 On Update

Cascade

### 2.6.12.0.0 Join Table

#### 2.6.12.1.0 Name

UserRole

#### 2.6.12.2.0 Columns

##### 2.6.12.2.1 Guid

###### 2.6.12.2.1.1 Name

userId

###### 2.6.12.2.1.2 Type

🔹 Guid

###### 2.6.12.2.1.3 References

User.userId

##### 2.6.12.2.2.0 Guid

###### 2.6.12.2.2.1 Name

roleId

###### 2.6.12.2.2.2 Type

🔹 Guid

###### 2.6.12.2.2.3 References

Role.roleId

##### 2.6.12.2.3.0 Guid

###### 2.6.12.2.3.1 Name

assetScopeId

###### 2.6.12.2.3.2 Type

🔹 Guid

###### 2.6.12.2.3.3 References

Asset.assetId

## 2.7.0.0.0.0 OneToMany

### 2.7.1.0.0.0 Name

ClientServerConnections

### 2.7.2.0.0.0 Id

REL_OPCCLIENT_OPCSERVER_001

### 2.7.3.0.0.0 Source Entity

OpcCoreClient

### 2.7.4.0.0.0 Target Entity

OpcServerConnection

### 2.7.5.0.0.0 Type

🔹 OneToMany

### 2.7.6.0.0.0 Source Multiplicity

1

### 2.7.7.0.0.0 Target Multiplicity

0..*

### 2.7.8.0.0.0 Cascade Delete

✅ Yes

### 2.7.9.0.0.0 Is Identifying

✅ Yes

### 2.7.10.0.0.0 On Delete

Cascade

### 2.7.11.0.0.0 On Update

Cascade

### 2.7.12.0.0.0 Join Table

#### 2.7.12.1.0.0 Name

N/A

#### 2.7.12.2.0.0 Columns

*No items available*

## 2.8.0.0.0.0 OneToOne

### 2.8.1.0.0.0 Name

OpcServerRedundancy

### 2.8.2.0.0.0 Id

REL_OPCSERVER_REDUNDANCY_001

### 2.8.3.0.0.0 Source Entity

OpcServerConnection

### 2.8.4.0.0.0 Target Entity

OpcServerConnection

### 2.8.5.0.0.0 Type

🔹 OneToOne

### 2.8.6.0.0.0 Source Multiplicity

0..1

### 2.8.7.0.0.0 Target Multiplicity

0..1

### 2.8.8.0.0.0 Cascade Delete

❌ No

### 2.8.9.0.0.0 Is Identifying

❌ No

### 2.8.10.0.0.0 On Delete

SetNull

### 2.8.11.0.0.0 On Update

Cascade

### 2.8.12.0.0.0 Join Table

#### 2.8.12.1.0.0 Name

N/A

#### 2.8.12.2.0.0 Columns

*No items available*

## 2.9.0.0.0.0 OneToMany

### 2.9.1.0.0.0 Name

AssetHierarchy

### 2.9.2.0.0.0 Id

REL_ASSET_HIERARCHY_001

### 2.9.3.0.0.0 Source Entity

Asset

### 2.9.4.0.0.0 Target Entity

Asset

### 2.9.5.0.0.0 Type

🔹 OneToMany

### 2.9.6.0.0.0 Source Multiplicity

0..1

### 2.9.7.0.0.0 Target Multiplicity

0..*

### 2.9.8.0.0.0 Cascade Delete

❌ No

### 2.9.9.0.0.0 Is Identifying

❌ No

### 2.9.10.0.0.0 On Delete

Restrict

### 2.9.11.0.0.0 On Update

Cascade

### 2.9.12.0.0.0 Join Table

#### 2.9.12.1.0.0 Name

N/A

#### 2.9.12.2.0.0 Columns

*No items available*

## 2.10.0.0.0.0 OneToMany

### 2.10.1.0.0.0 Name

AssetOpcTags

### 2.10.2.0.0.0 Id

REL_ASSET_OPCTAG_001

### 2.10.3.0.0.0 Source Entity

Asset

### 2.10.4.0.0.0 Target Entity

OpcTag

### 2.10.5.0.0.0 Type

🔹 OneToMany

### 2.10.6.0.0.0 Source Multiplicity

1

### 2.10.7.0.0.0 Target Multiplicity

0..*

### 2.10.8.0.0.0 Cascade Delete

✅ Yes

### 2.10.9.0.0.0 Is Identifying

✅ Yes

### 2.10.10.0.0.0 On Delete

Cascade

### 2.10.11.0.0.0 On Update

Cascade

### 2.10.12.0.0.0 Join Table

#### 2.10.12.1.0.0 Name

N/A

#### 2.10.12.2.0.0 Columns

*No items available*

## 2.11.0.0.0.0 OneToMany

### 2.11.1.0.0.0 Name

ConnectionOpcTags

### 2.11.2.0.0.0 Id

REL_OPCSERVER_OPCTAG_001

### 2.11.3.0.0.0 Source Entity

OpcServerConnection

### 2.11.4.0.0.0 Target Entity

OpcTag

### 2.11.5.0.0.0 Type

🔹 OneToMany

### 2.11.6.0.0.0 Source Multiplicity

1

### 2.11.7.0.0.0 Target Multiplicity

0..*

### 2.11.8.0.0.0 Cascade Delete

❌ No

### 2.11.9.0.0.0 Is Identifying

❌ No

### 2.11.10.0.0.0 On Delete

Restrict

### 2.11.11.0.0.0 On Update

Cascade

### 2.11.12.0.0.0 Join Table

#### 2.11.12.1.0.0 Name

N/A

#### 2.11.12.2.0.0 Columns

*No items available*

## 2.12.0.0.0.0 OneToMany

### 2.12.1.0.0.0 Name

OpcTagDataPoints

### 2.12.2.0.0.0 Id

REL_OPCTAG_DATAPOINT_001

### 2.12.3.0.0.0 Source Entity

OpcTag

### 2.12.4.0.0.0 Target Entity

TagDataPoint

### 2.12.5.0.0.0 Type

🔹 OneToMany

### 2.12.6.0.0.0 Source Multiplicity

1

### 2.12.7.0.0.0 Target Multiplicity

0..*

### 2.12.8.0.0.0 Cascade Delete

✅ Yes

### 2.12.9.0.0.0 Is Identifying

✅ Yes

### 2.12.10.0.0.0 On Delete

Cascade

### 2.12.11.0.0.0 On Update

Cascade

### 2.12.12.0.0.0 Join Table

#### 2.12.12.1.0.0 Name

N/A

#### 2.12.12.2.0.0 Columns

*No items available*

## 2.13.0.0.0.0 OneToMany

### 2.13.1.0.0.0 Name

OpcTagAlarms

### 2.13.2.0.0.0 Id

REL_OPCTAG_ALARM_001

### 2.13.3.0.0.0 Source Entity

OpcTag

### 2.13.4.0.0.0 Target Entity

Alarm

### 2.13.5.0.0.0 Type

🔹 OneToMany

### 2.13.6.0.0.0 Source Multiplicity

1

### 2.13.7.0.0.0 Target Multiplicity

0..*

### 2.13.8.0.0.0 Cascade Delete

✅ Yes

### 2.13.9.0.0.0 Is Identifying

❌ No

### 2.13.10.0.0.0 On Delete

Cascade

### 2.13.11.0.0.0 On Update

Cascade

### 2.13.12.0.0.0 Join Table

#### 2.13.12.1.0.0 Name

N/A

#### 2.13.12.2.0.0 Columns

*No items available*

## 2.14.0.0.0.0 OneToMany

### 2.14.1.0.0.0 Name

AlarmHistoryEntries

### 2.14.2.0.0.0 Id

REL_ALARM_ALARMHISTORY_001

### 2.14.3.0.0.0 Source Entity

Alarm

### 2.14.4.0.0.0 Target Entity

AlarmHistory

### 2.14.5.0.0.0 Type

🔹 OneToMany

### 2.14.6.0.0.0 Source Multiplicity

1

### 2.14.7.0.0.0 Target Multiplicity

1..*

### 2.14.8.0.0.0 Cascade Delete

✅ Yes

### 2.14.9.0.0.0 Is Identifying

✅ Yes

### 2.14.10.0.0.0 On Delete

Cascade

### 2.14.11.0.0.0 On Update

Cascade

### 2.14.12.0.0.0 Join Table

#### 2.14.12.1.0.0 Name

N/A

#### 2.14.12.2.0.0 Columns

*No items available*

## 2.15.0.0.0.0 OneToMany

### 2.15.1.0.0.0 Name

UserAlarmActions

### 2.15.2.0.0.0 Id

REL_USER_ALARMHISTORY_001

### 2.15.3.0.0.0 Source Entity

User

### 2.15.4.0.0.0 Target Entity

AlarmHistory

### 2.15.5.0.0.0 Type

🔹 OneToMany

### 2.15.6.0.0.0 Source Multiplicity

1

### 2.15.7.0.0.0 Target Multiplicity

0..*

### 2.15.8.0.0.0 Cascade Delete

❌ No

### 2.15.9.0.0.0 Is Identifying

❌ No

### 2.15.10.0.0.0 On Delete

Restrict

### 2.15.11.0.0.0 On Update

Cascade

### 2.15.12.0.0.0 Join Table

#### 2.15.12.1.0.0 Name

N/A

#### 2.15.12.2.0.0 Columns

*No items available*

## 2.16.0.0.0.0 OneToMany

### 2.16.1.0.0.0 Name

TenantAiModels

### 2.16.2.0.0.0 Id

REL_TENANT_AIMODEL_001

### 2.16.3.0.0.0 Source Entity

Tenant

### 2.16.4.0.0.0 Target Entity

AiModel

### 2.16.5.0.0.0 Type

🔹 OneToMany

### 2.16.6.0.0.0 Source Multiplicity

1

### 2.16.7.0.0.0 Target Multiplicity

0..*

### 2.16.8.0.0.0 Cascade Delete

❌ No

### 2.16.9.0.0.0 Is Identifying

❌ No

### 2.16.10.0.0.0 On Delete

Restrict

### 2.16.11.0.0.0 On Update

Cascade

### 2.16.12.0.0.0 Join Table

#### 2.16.12.1.0.0 Name

N/A

#### 2.16.12.2.0.0 Columns

*No items available*

## 2.17.0.0.0.0 OneToMany

### 2.17.1.0.0.0 Name

AiModelVersions

### 2.17.2.0.0.0 Id

REL_AIMODEL_VERSION_001

### 2.17.3.0.0.0 Source Entity

AiModel

### 2.17.4.0.0.0 Target Entity

AiModelVersion

### 2.17.5.0.0.0 Type

🔹 OneToMany

### 2.17.6.0.0.0 Source Multiplicity

1

### 2.17.7.0.0.0 Target Multiplicity

1..*

### 2.17.8.0.0.0 Cascade Delete

✅ Yes

### 2.17.9.0.0.0 Is Identifying

✅ Yes

### 2.17.10.0.0.0 On Delete

Cascade

### 2.17.11.0.0.0 On Update

Cascade

### 2.17.12.0.0.0 Join Table

#### 2.17.12.1.0.0 Name

N/A

#### 2.17.12.2.0.0 Columns

*No items available*

## 2.18.0.0.0.0 OneToMany

### 2.18.1.0.0.0 Name

UserSubmittedModelVersions

### 2.18.2.0.0.0 Id

REL_USER_SUBMITTEDMODEL_001

### 2.18.3.0.0.0 Source Entity

User

### 2.18.4.0.0.0 Target Entity

AiModelVersion

### 2.18.5.0.0.0 Type

🔹 OneToMany

### 2.18.6.0.0.0 Source Multiplicity

1

### 2.18.7.0.0.0 Target Multiplicity

0..*

### 2.18.8.0.0.0 Cascade Delete

❌ No

### 2.18.9.0.0.0 Is Identifying

❌ No

### 2.18.10.0.0.0 On Delete

SetNull

### 2.18.11.0.0.0 On Update

Cascade

### 2.18.12.0.0.0 Join Table

#### 2.18.12.1.0.0 Name

N/A

#### 2.18.12.2.0.0 Columns

*No items available*

## 2.19.0.0.0.0 OneToMany

### 2.19.1.0.0.0 Name

UserApprovedModelVersions

### 2.19.2.0.0.0 Id

REL_USER_APPROVEDMODEL_001

### 2.19.3.0.0.0 Source Entity

User

### 2.19.4.0.0.0 Target Entity

AiModelVersion

### 2.19.5.0.0.0 Type

🔹 OneToMany

### 2.19.6.0.0.0 Source Multiplicity

1

### 2.19.7.0.0.0 Target Multiplicity

0..*

### 2.19.8.0.0.0 Cascade Delete

❌ No

### 2.19.9.0.0.0 Is Identifying

❌ No

### 2.19.10.0.0.0 On Delete

SetNull

### 2.19.11.0.0.0 On Update

Cascade

### 2.19.12.0.0.0 Join Table

#### 2.19.12.1.0.0 Name

N/A

#### 2.19.12.2.0.0 Columns

*No items available*

## 2.20.0.0.0.0 OneToMany

### 2.20.1.0.0.0 Name

AssetModelAssignments

### 2.20.2.0.0.0 Id

REL_ASSET_MODELASSIGN_001

### 2.20.3.0.0.0 Source Entity

Asset

### 2.20.4.0.0.0 Target Entity

ModelAssignment

### 2.20.5.0.0.0 Type

🔹 OneToMany

### 2.20.6.0.0.0 Source Multiplicity

1

### 2.20.7.0.0.0 Target Multiplicity

0..*

### 2.20.8.0.0.0 Cascade Delete

✅ Yes

### 2.20.9.0.0.0 Is Identifying

❌ No

### 2.20.10.0.0.0 On Delete

Cascade

### 2.20.11.0.0.0 On Update

Cascade

### 2.20.12.0.0.0 Join Table

#### 2.20.12.1.0.0 Name

N/A

#### 2.20.12.2.0.0 Columns

*No items available*

## 2.21.0.0.0.0 OneToMany

### 2.21.1.0.0.0 Name

VersionModelAssignments

### 2.21.2.0.0.0 Id

REL_VERSION_MODELASSIGN_001

### 2.21.3.0.0.0 Source Entity

AiModelVersion

### 2.21.4.0.0.0 Target Entity

ModelAssignment

### 2.21.5.0.0.0 Type

🔹 OneToMany

### 2.21.6.0.0.0 Source Multiplicity

1

### 2.21.7.0.0.0 Target Multiplicity

0..*

### 2.21.8.0.0.0 Cascade Delete

❌ No

### 2.21.9.0.0.0 Is Identifying

❌ No

### 2.21.10.0.0.0 On Delete

Restrict

### 2.21.11.0.0.0 On Update

Cascade

### 2.21.12.0.0.0 Join Table

#### 2.21.12.1.0.0 Name

N/A

#### 2.21.12.2.0.0 Columns

*No items available*

## 2.22.0.0.0.0 OneToMany

### 2.22.1.0.0.0 Name

ClientModelAssignments

### 2.22.2.0.0.0 Id

REL_OPCCLIENT_MODELASSIGN_001

### 2.22.3.0.0.0 Source Entity

OpcCoreClient

### 2.22.4.0.0.0 Target Entity

ModelAssignment

### 2.22.5.0.0.0 Type

🔹 OneToMany

### 2.22.6.0.0.0 Source Multiplicity

1

### 2.22.7.0.0.0 Target Multiplicity

0..*

### 2.22.8.0.0.0 Cascade Delete

❌ No

### 2.22.9.0.0.0 Is Identifying

❌ No

### 2.22.10.0.0.0 On Delete

Restrict

### 2.22.11.0.0.0 On Update

Cascade

### 2.22.12.0.0.0 Join Table

#### 2.22.12.1.0.0 Name

N/A

#### 2.22.12.2.0.0 Columns

*No items available*

## 2.23.0.0.0.0 OneToMany

### 2.23.1.0.0.0 Name

AssignmentAnomalyEvents

### 2.23.2.0.0.0 Id

REL_ASSIGNMENT_ANOMALY_001

### 2.23.3.0.0.0 Source Entity

ModelAssignment

### 2.23.4.0.0.0 Target Entity

AnomalyEvent

### 2.23.5.0.0.0 Type

🔹 OneToMany

### 2.23.6.0.0.0 Source Multiplicity

1

### 2.23.7.0.0.0 Target Multiplicity

0..*

### 2.23.8.0.0.0 Cascade Delete

✅ Yes

### 2.23.9.0.0.0 Is Identifying

✅ Yes

### 2.23.10.0.0.0 On Delete

Cascade

### 2.23.11.0.0.0 On Update

Cascade

### 2.23.12.0.0.0 Join Table

#### 2.23.12.1.0.0 Name

N/A

#### 2.23.12.2.0.0 Columns

*No items available*

## 2.24.0.0.0.0 OneToOne

### 2.24.1.0.0.0 Name

TenantLicense

### 2.24.2.0.0.0 Id

REL_TENANT_LICENSE_001

### 2.24.3.0.0.0 Source Entity

Tenant

### 2.24.4.0.0.0 Target Entity

License

### 2.24.5.0.0.0 Type

🔹 OneToOne

### 2.24.6.0.0.0 Source Multiplicity

1

### 2.24.7.0.0.0 Target Multiplicity

1

### 2.24.8.0.0.0 Cascade Delete

✅ Yes

### 2.24.9.0.0.0 Is Identifying

✅ Yes

### 2.24.10.0.0.0 On Delete

Cascade

### 2.24.11.0.0.0 On Update

Cascade

### 2.24.12.0.0.0 Join Table

#### 2.24.12.1.0.0 Name

N/A

#### 2.24.12.2.0.0 Columns

*No items available*

## 2.25.0.0.0.0 OneToMany

### 2.25.1.0.0.0 Name

TenantReportTemplates

### 2.25.2.0.0.0 Id

REL_TENANT_REPORTTEMPLATE_001

### 2.25.3.0.0.0 Source Entity

Tenant

### 2.25.4.0.0.0 Target Entity

ReportTemplate

### 2.25.5.0.0.0 Type

🔹 OneToMany

### 2.25.6.0.0.0 Source Multiplicity

1

### 2.25.7.0.0.0 Target Multiplicity

0..*

### 2.25.8.0.0.0 Cascade Delete

✅ Yes

### 2.25.9.0.0.0 Is Identifying

❌ No

### 2.25.10.0.0.0 On Delete

Cascade

### 2.25.11.0.0.0 On Update

Cascade

### 2.25.12.0.0.0 Join Table

#### 2.25.12.1.0.0 Name

N/A

#### 2.25.12.2.0.0 Columns

*No items available*

## 2.26.0.0.0.0 OneToMany

### 2.26.1.0.0.0 Name

TenantApprovalRequests

### 2.26.2.0.0.0 Id

REL_TENANT_APPROVAL_001

### 2.26.3.0.0.0 Source Entity

Tenant

### 2.26.4.0.0.0 Target Entity

ApprovalRequest

### 2.26.5.0.0.0 Type

🔹 OneToMany

### 2.26.6.0.0.0 Source Multiplicity

1

### 2.26.7.0.0.0 Target Multiplicity

0..*

### 2.26.8.0.0.0 Cascade Delete

✅ Yes

### 2.26.9.0.0.0 Is Identifying

❌ No

### 2.26.10.0.0.0 On Delete

Cascade

### 2.26.11.0.0.0 On Update

Cascade

### 2.26.12.0.0.0 Join Table

#### 2.26.12.1.0.0 Name

N/A

#### 2.26.12.2.0.0 Columns

*No items available*

## 2.27.0.0.0.0 OneToMany

### 2.27.1.0.0.0 Name

UserRequestedApprovals

### 2.27.2.0.0.0 Id

REL_USER_REQUESTEDAPPROVAL_001

### 2.27.3.0.0.0 Source Entity

User

### 2.27.4.0.0.0 Target Entity

ApprovalRequest

### 2.27.5.0.0.0 Type

🔹 OneToMany

### 2.27.6.0.0.0 Source Multiplicity

1

### 2.27.7.0.0.0 Target Multiplicity

0..*

### 2.27.8.0.0.0 Cascade Delete

❌ No

### 2.27.9.0.0.0 Is Identifying

❌ No

### 2.27.10.0.0.0 On Delete

Restrict

### 2.27.11.0.0.0 On Update

Cascade

### 2.27.12.0.0.0 Join Table

#### 2.27.12.1.0.0 Name

N/A

#### 2.27.12.2.0.0 Columns

*No items available*

## 2.28.0.0.0.0 OneToMany

### 2.28.1.0.0.0 Name

UserApprovedApprovals

### 2.28.2.0.0.0 Id

REL_USER_APPROVEDAPPROVAL_001

### 2.28.3.0.0.0 Source Entity

User

### 2.28.4.0.0.0 Target Entity

ApprovalRequest

### 2.28.5.0.0.0 Type

🔹 OneToMany

### 2.28.6.0.0.0 Source Multiplicity

1

### 2.28.7.0.0.0 Target Multiplicity

0..*

### 2.28.8.0.0.0 Cascade Delete

❌ No

### 2.28.9.0.0.0 Is Identifying

❌ No

### 2.28.10.0.0.0 On Delete

SetNull

### 2.28.11.0.0.0 On Update

Cascade

### 2.28.12.0.0.0 Join Table

#### 2.28.12.1.0.0 Name

N/A

#### 2.28.12.2.0.0 Columns

*No items available*

## 2.29.0.0.0.0 OneToMany

### 2.29.1.0.0.0 Name

AssetRoleScope

### 2.29.2.0.0.0 Id

REL_ASSET_ROLESCOPE_001

### 2.29.3.0.0.0 Source Entity

Asset

### 2.29.4.0.0.0 Target Entity

UserRole

### 2.29.5.0.0.0 Type

🔹 OneToMany

### 2.29.6.0.0.0 Source Multiplicity

1

### 2.29.7.0.0.0 Target Multiplicity

0..*

### 2.29.8.0.0.0 Cascade Delete

❌ No

### 2.29.9.0.0.0 Is Identifying

❌ No

### 2.29.10.0.0.0 On Delete

SetNull

### 2.29.11.0.0.0 On Update

Cascade

### 2.29.12.0.0.0 Join Table

#### 2.29.12.1.0.0 Name

N/A

#### 2.29.12.2.0.0 Columns

*No items available*

## 2.30.0.0.0.0 OneToMany

### 2.30.1.0.0.0 Name

TenantTagDataPoints_Denormalized

### 2.30.2.0.0.0 Id

REL_TENANT_DATAPOINT_DENORM_001

### 2.30.3.0.0.0 Source Entity

Tenant

### 2.30.4.0.0.0 Target Entity

TagDataPoint

### 2.30.5.0.0.0 Type

🔹 OneToMany

### 2.30.6.0.0.0 Source Multiplicity

1

### 2.30.7.0.0.0 Target Multiplicity

0..*

### 2.30.8.0.0.0 Cascade Delete

❌ No

### 2.30.9.0.0.0 Is Identifying

❌ No

### 2.30.10.0.0.0 On Delete

Restrict

### 2.30.11.0.0.0 On Update

Cascade

### 2.30.12.0.0.0 Join Table

#### 2.30.12.1.0.0 Name

N/A

#### 2.30.12.2.0.0 Columns

*No items available*

## 2.31.0.0.0.0 OneToMany

### 2.31.1.0.0.0 Name

AssetTagDataPoints_Denormalized

### 2.31.2.0.0.0 Id

REL_ASSET_DATAPOINT_DENORM_001

### 2.31.3.0.0.0 Source Entity

Asset

### 2.31.4.0.0.0 Target Entity

TagDataPoint

### 2.31.5.0.0.0 Type

🔹 OneToMany

### 2.31.6.0.0.0 Source Multiplicity

1

### 2.31.7.0.0.0 Target Multiplicity

0..*

### 2.31.8.0.0.0 Cascade Delete

❌ No

### 2.31.9.0.0.0 Is Identifying

❌ No

### 2.31.10.0.0.0 On Delete

Restrict

### 2.31.11.0.0.0 On Update

Cascade

### 2.31.12.0.0.0 Join Table

#### 2.31.12.1.0.0 Name

N/A

#### 2.31.12.2.0.0 Columns

*No items available*

## 2.32.0.0.0.0 OneToMany

### 2.32.1.0.0.0 Name

TenantAlarms_Denormalized

### 2.32.2.0.0.0 Id

REL_TENANT_ALARM_DENORM_001

### 2.32.3.0.0.0 Source Entity

Tenant

### 2.32.4.0.0.0 Target Entity

Alarm

### 2.32.5.0.0.0 Type

🔹 OneToMany

### 2.32.6.0.0.0 Source Multiplicity

1

### 2.32.7.0.0.0 Target Multiplicity

0..*

### 2.32.8.0.0.0 Cascade Delete

❌ No

### 2.32.9.0.0.0 Is Identifying

❌ No

### 2.32.10.0.0.0 On Delete

Restrict

### 2.32.11.0.0.0 On Update

Cascade

### 2.32.12.0.0.0 Join Table

#### 2.32.12.1.0.0 Name

N/A

#### 2.32.12.2.0.0 Columns

*No items available*

## 2.33.0.0.0.0 OneToMany

### 2.33.1.0.0.0 Name

AssetAlarms_Denormalized

### 2.33.2.0.0.0 Id

REL_ASSET_ALARM_DENORM_001

### 2.33.3.0.0.0 Source Entity

Asset

### 2.33.4.0.0.0 Target Entity

Alarm

### 2.33.5.0.0.0 Type

🔹 OneToMany

### 2.33.6.0.0.0 Source Multiplicity

1

### 2.33.7.0.0.0 Target Multiplicity

0..*

### 2.33.8.0.0.0 Cascade Delete

❌ No

### 2.33.9.0.0.0 Is Identifying

❌ No

### 2.33.10.0.0.0 On Delete

SetNull

### 2.33.11.0.0.0 On Update

Cascade

### 2.33.12.0.0.0 Join Table

#### 2.33.12.1.0.0 Name

N/A

#### 2.33.12.2.0.0 Columns

*No items available*

