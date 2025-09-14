# 1 Entities

## 1.1 Tenant

### 1.1.1 Name

Tenant

### 1.1.2 Description

Represents a customer organization, providing data isolation in a multi-tenant architecture. REQ-1-024, REQ-1-025.

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

#### 1.1.3.3.0 BOOLEAN

##### 1.1.3.3.1 Name

isActive

##### 1.1.3.3.2 Type

🔹 BOOLEAN

##### 1.1.3.3.3 Is Required

✅ Yes

##### 1.1.3.3.4 Is Primary Key

❌ No

##### 1.1.3.3.5 Is Unique

❌ No

##### 1.1.3.3.6 Index Type

Index

##### 1.1.3.3.7 Size

0

##### 1.1.3.3.8 Constraints

*No items available*

##### 1.1.3.3.9 Default Value

true

##### 1.1.3.3.10 Is Foreign Key

❌ No

##### 1.1.3.3.11 Precision

0

##### 1.1.3.3.12 Scale

0

#### 1.1.3.4.0 VARCHAR

##### 1.1.3.4.1 Name

dataResidencyRegion

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

*No items available*

##### 1.1.3.4.9 Default Value



##### 1.1.3.4.10 Is Foreign Key

❌ No

##### 1.1.3.4.11 Precision

0

##### 1.1.3.4.12 Scale

0

#### 1.1.3.5.0 DateTimeOffset

##### 1.1.3.5.1 Name

createdAt

##### 1.1.3.5.2 Type

🔹 DateTimeOffset

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

CURRENT_TIMESTAMP

##### 1.1.3.5.10 Is Foreign Key

❌ No

##### 1.1.3.5.11 Precision

0

##### 1.1.3.5.12 Scale

0

#### 1.1.3.6.0 DateTimeOffset

##### 1.1.3.6.1 Name

updatedAt

##### 1.1.3.6.2 Type

🔹 DateTimeOffset

##### 1.1.3.6.3 Is Required

✅ Yes

##### 1.1.3.6.4 Is Primary Key

❌ No

##### 1.1.3.6.5 Is Unique

❌ No

##### 1.1.3.6.6 Index Type

None

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

#### 1.1.3.7.0 BOOLEAN

##### 1.1.3.7.1 Name

isDeleted

##### 1.1.3.7.2 Type

🔹 BOOLEAN

##### 1.1.3.7.3 Is Required

✅ Yes

##### 1.1.3.7.4 Is Primary Key

❌ No

##### 1.1.3.7.5 Is Unique

❌ No

##### 1.1.3.7.6 Index Type

Index

##### 1.1.3.7.7 Size

0

##### 1.1.3.7.8 Constraints

*No items available*

##### 1.1.3.7.9 Default Value

false

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

- {'name': 'IX_Tenant_Active_NotDeleted', 'columns': ['isActive', 'isDeleted'], 'type': 'BTree'}

## 1.2.0.0.0 User

### 1.2.1.0.0 Name

User

### 1.2.2.0.0 Description

Represents a system user with authentication details and profile information. REQ-1-011, REQ-1-080.

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

identityProviderId

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

*No items available*

##### 1.2.3.3.9 Default Value



##### 1.2.3.3.10 Is Foreign Key

❌ No

##### 1.2.3.3.11 Precision

0

##### 1.2.3.3.12 Scale

0

#### 1.2.3.4.0 VARCHAR

##### 1.2.3.4.1 Name

email

##### 1.2.3.4.2 Type

🔹 VARCHAR

##### 1.2.3.4.3 Is Required

✅ Yes

##### 1.2.3.4.4 Is Primary Key

❌ No

##### 1.2.3.4.5 Is Unique

✅ Yes

##### 1.2.3.4.6 Index Type

UniqueIndex

##### 1.2.3.4.7 Size

255

##### 1.2.3.4.8 Constraints

- EMAIL_FORMAT

##### 1.2.3.4.9 Default Value



##### 1.2.3.4.10 Is Foreign Key

❌ No

##### 1.2.3.4.11 Precision

0

##### 1.2.3.4.12 Scale

0

#### 1.2.3.5.0 VARCHAR

##### 1.2.3.5.1 Name

firstName

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

lastName

##### 1.2.3.6.2 Type

🔹 VARCHAR

##### 1.2.3.6.3 Is Required

✅ Yes

##### 1.2.3.6.4 Is Primary Key

❌ No

##### 1.2.3.6.5 Is Unique

❌ No

##### 1.2.3.6.6 Index Type

Index

##### 1.2.3.6.7 Size

100

##### 1.2.3.6.8 Constraints

*No items available*

##### 1.2.3.6.9 Default Value



##### 1.2.3.6.10 Is Foreign Key

❌ No

##### 1.2.3.6.11 Precision

0

##### 1.2.3.6.12 Scale

0

#### 1.2.3.7.0 BOOLEAN

##### 1.2.3.7.1 Name

isActive

##### 1.2.3.7.2 Type

🔹 BOOLEAN

##### 1.2.3.7.3 Is Required

✅ Yes

##### 1.2.3.7.4 Is Primary Key

❌ No

##### 1.2.3.7.5 Is Unique

❌ No

##### 1.2.3.7.6 Index Type

Index

##### 1.2.3.7.7 Size

0

##### 1.2.3.7.8 Constraints

*No items available*

##### 1.2.3.7.9 Default Value

true

##### 1.2.3.7.10 Is Foreign Key

❌ No

##### 1.2.3.7.11 Precision

0

##### 1.2.3.7.12 Scale

0

#### 1.2.3.8.0 DateTimeOffset

##### 1.2.3.8.1 Name

createdAt

##### 1.2.3.8.2 Type

🔹 DateTimeOffset

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

CURRENT_TIMESTAMP

##### 1.2.3.8.10 Is Foreign Key

❌ No

##### 1.2.3.8.11 Precision

0

##### 1.2.3.8.12 Scale

0

#### 1.2.3.9.0 DateTimeOffset

##### 1.2.3.9.1 Name

updatedAt

##### 1.2.3.9.2 Type

🔹 DateTimeOffset

##### 1.2.3.9.3 Is Required

✅ Yes

##### 1.2.3.9.4 Is Primary Key

❌ No

##### 1.2.3.9.5 Is Unique

❌ No

##### 1.2.3.9.6 Index Type

None

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

#### 1.2.3.10.0 BOOLEAN

##### 1.2.3.10.1 Name

isDeleted

##### 1.2.3.10.2 Type

🔹 BOOLEAN

##### 1.2.3.10.3 Is Required

✅ Yes

##### 1.2.3.10.4 Is Primary Key

❌ No

##### 1.2.3.10.5 Is Unique

❌ No

##### 1.2.3.10.6 Index Type

Index

##### 1.2.3.10.7 Size

0

##### 1.2.3.10.8 Constraints

*No items available*

##### 1.2.3.10.9 Default Value

false

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

IX_User_FullName

##### 1.2.6.2.2 Columns

- lastName
- firstName

##### 1.2.6.2.3 Type

🔹 BTree

#### 1.2.6.3.0 BTree

##### 1.2.6.3.1 Name

IX_User_Active_NotDeleted

##### 1.2.6.3.2 Columns

- isActive
- isDeleted

##### 1.2.6.3.3 Type

🔹 BTree

#### 1.2.6.4.0 BTree

##### 1.2.6.4.1 Name

IX_User_TenantActiveDeletedName

##### 1.2.6.4.2 Columns

- tenantId
- isActive
- isDeleted
- lastName
- firstName

##### 1.2.6.4.3 Type

🔹 BTree

## 1.3.0.0.0 Role

### 1.3.1.0.0 Name

Role

### 1.3.2.0.0 Description

Defines a set of permissions for users (e.g., Administrator, Engineer). REQ-1-011.

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

Junction table to link Users to Roles, establishing a many-to-many relationship with asset-level scope. REQ-1-061.

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

✅ Yes

##### 1.4.3.3.4 Is Primary Key

✅ Yes

##### 1.4.3.3.5 Is Unique

❌ No

##### 1.4.3.3.6 Index Type

Index

##### 1.4.3.3.7 Size

0

##### 1.4.3.3.8 Constraints

*No items available*

##### 1.4.3.3.9 Default Value

00000000-0000-0000-0000-000000000000

##### 1.4.3.3.10 Is Foreign Key

✅ Yes

##### 1.4.3.3.11 Precision

0

##### 1.4.3.3.12 Scale

0

### 1.4.4.0.0 Primary Keys

- userId
- roleId
- assetScopeId

### 1.4.5.0.0 Unique Constraints

*No items available*

### 1.4.6.0.0 Indexes

#### 1.4.6.1.0 BTree

##### 1.4.6.1.1 Name

IX_UserRole_UserId

##### 1.4.6.1.2 Columns

- userId

##### 1.4.6.1.3 Type

🔹 BTree

#### 1.4.6.2.0 BTree

##### 1.4.6.2.1 Name

IX_UserRole_RoleId

##### 1.4.6.2.2 Columns

- roleId

##### 1.4.6.2.3 Type

🔹 BTree

#### 1.4.6.3.0 BTree

##### 1.4.6.3.1 Name

IX_UserRole_AssetScopeId

##### 1.4.6.3.2 Columns

- assetScopeId

##### 1.4.6.3.3 Type

🔹 BTree

## 1.5.0.0.0 OpcCoreClient

### 1.5.1.0.0 Name

OpcCoreClient

### 1.5.2.0.0 Description

Represents a deployed instance of the on-premise/edge client application. REQ-1-001, REQ-1-062.

### 1.5.3.0.0 Attributes

#### 1.5.3.1.0 Guid

##### 1.5.3.1.1 Name

opcCoreClientId

##### 1.5.3.1.2 Type

🔹 Guid

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

*No items available*

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

#### 1.5.3.3.0 VARCHAR

##### 1.5.3.3.1 Name

name

##### 1.5.3.3.2 Type

🔹 VARCHAR

##### 1.5.3.3.3 Is Required

✅ Yes

##### 1.5.3.3.4 Is Primary Key

❌ No

##### 1.5.3.3.5 Is Unique

❌ No

##### 1.5.3.3.6 Index Type

Index

##### 1.5.3.3.7 Size

255

##### 1.5.3.3.8 Constraints

*No items available*

##### 1.5.3.3.9 Default Value



##### 1.5.3.3.10 Is Foreign Key

❌ No

##### 1.5.3.3.11 Precision

0

##### 1.5.3.3.12 Scale

0

#### 1.5.3.4.0 VARCHAR

##### 1.5.3.4.1 Name

status

##### 1.5.3.4.2 Type

🔹 VARCHAR

##### 1.5.3.4.3 Is Required

✅ Yes

##### 1.5.3.4.4 Is Primary Key

❌ No

##### 1.5.3.4.5 Is Unique

❌ No

##### 1.5.3.4.6 Index Type

Index

##### 1.5.3.4.7 Size

50

##### 1.5.3.4.8 Constraints

- ENUM('Online', 'Offline', 'Degraded', 'Unregistered')

##### 1.5.3.4.9 Default Value

Unregistered

##### 1.5.3.4.10 Is Foreign Key

❌ No

##### 1.5.3.4.11 Precision

0

##### 1.5.3.4.12 Scale

0

#### 1.5.3.5.0 VARCHAR

##### 1.5.3.5.1 Name

version

##### 1.5.3.5.2 Type

🔹 VARCHAR

##### 1.5.3.5.3 Is Required

❌ No

##### 1.5.3.5.4 Is Primary Key

❌ No

##### 1.5.3.5.5 Is Unique

❌ No

##### 1.5.3.5.6 Index Type

None

##### 1.5.3.5.7 Size

50

##### 1.5.3.5.8 Constraints

*No items available*

##### 1.5.3.5.9 Default Value



##### 1.5.3.5.10 Is Foreign Key

❌ No

##### 1.5.3.5.11 Precision

0

##### 1.5.3.5.12 Scale

0

#### 1.5.3.6.0 DateTimeOffset

##### 1.5.3.6.1 Name

lastSeenAt

##### 1.5.3.6.2 Type

🔹 DateTimeOffset

##### 1.5.3.6.3 Is Required

❌ No

##### 1.5.3.6.4 Is Primary Key

❌ No

##### 1.5.3.6.5 Is Unique

❌ No

##### 1.5.3.6.6 Index Type

Index

##### 1.5.3.6.7 Size

0

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

registrationToken

##### 1.5.3.7.2 Type

🔹 VARCHAR

##### 1.5.3.7.3 Is Required

❌ No

##### 1.5.3.7.4 Is Primary Key

❌ No

##### 1.5.3.7.5 Is Unique

✅ Yes

##### 1.5.3.7.6 Index Type

UniqueIndex

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

#### 1.5.3.8.0 VARCHAR

##### 1.5.3.8.1 Name

clientCertificateThumbprint

##### 1.5.3.8.2 Type

🔹 VARCHAR

##### 1.5.3.8.3 Is Required

❌ No

##### 1.5.3.8.4 Is Primary Key

❌ No

##### 1.5.3.8.5 Is Unique

✅ Yes

##### 1.5.3.8.6 Index Type

UniqueIndex

##### 1.5.3.8.7 Size

255

##### 1.5.3.8.8 Constraints

*No items available*

##### 1.5.3.8.9 Default Value



##### 1.5.3.8.10 Is Foreign Key

❌ No

##### 1.5.3.8.11 Precision

0

##### 1.5.3.8.12 Scale

0

#### 1.5.3.9.0 DateTimeOffset

##### 1.5.3.9.1 Name

createdAt

##### 1.5.3.9.2 Type

🔹 DateTimeOffset

##### 1.5.3.9.3 Is Required

✅ Yes

##### 1.5.3.9.4 Is Primary Key

❌ No

##### 1.5.3.9.5 Is Unique

❌ No

##### 1.5.3.9.6 Index Type

Index

##### 1.5.3.9.7 Size

0

##### 1.5.3.9.8 Constraints

*No items available*

##### 1.5.3.9.9 Default Value

CURRENT_TIMESTAMP

##### 1.5.3.9.10 Is Foreign Key

❌ No

##### 1.5.3.9.11 Precision

0

##### 1.5.3.9.12 Scale

0

#### 1.5.3.10.0 DateTimeOffset

##### 1.5.3.10.1 Name

updatedAt

##### 1.5.3.10.2 Type

🔹 DateTimeOffset

##### 1.5.3.10.3 Is Required

✅ Yes

##### 1.5.3.10.4 Is Primary Key

❌ No

##### 1.5.3.10.5 Is Unique

❌ No

##### 1.5.3.10.6 Index Type

None

##### 1.5.3.10.7 Size

0

##### 1.5.3.10.8 Constraints

*No items available*

##### 1.5.3.10.9 Default Value

CURRENT_TIMESTAMP

##### 1.5.3.10.10 Is Foreign Key

❌ No

##### 1.5.3.10.11 Precision

0

##### 1.5.3.10.12 Scale

0

### 1.5.4.0.0 Primary Keys

- opcCoreClientId

### 1.5.5.0.0 Unique Constraints

#### 1.5.5.1.0 UC_OpcCoreClient_TenantName

##### 1.5.5.1.1 Name

UC_OpcCoreClient_TenantName

##### 1.5.5.1.2 Columns

- tenantId
- name

#### 1.5.5.2.0 UC_OpcCoreClient_RegToken

##### 1.5.5.2.1 Name

UC_OpcCoreClient_RegToken

##### 1.5.5.2.2 Columns

- registrationToken

#### 1.5.5.3.0 UC_OpcCoreClient_Thumbprint

##### 1.5.5.3.1 Name

UC_OpcCoreClient_Thumbprint

##### 1.5.5.3.2 Columns

- clientCertificateThumbprint

### 1.5.6.0.0 Indexes

#### 1.5.6.1.0 BTree

##### 1.5.6.1.1 Name

IX_OpcCoreClient_TenantId

##### 1.5.6.1.2 Columns

- tenantId

##### 1.5.6.1.3 Type

🔹 BTree

#### 1.5.6.2.0 BTree

##### 1.5.6.2.1 Name

IX_OpcCoreClient_Status

##### 1.5.6.2.2 Columns

- status

##### 1.5.6.2.3 Type

🔹 BTree

#### 1.5.6.3.0 BTree

##### 1.5.6.3.1 Name

IX_OpcCoreClient_TenantStatus

##### 1.5.6.3.2 Columns

- tenantId
- status

##### 1.5.6.3.3 Type

🔹 BTree

## 1.6.0.0.0 Asset

### 1.6.1.0.0 Name

Asset

### 1.6.2.0.0 Description

Represents a physical or logical asset in the plant hierarchy (e.g., Site, Line, Machine). REQ-1-031, REQ-1-046.

### 1.6.3.0.0 Attributes

#### 1.6.3.1.0 Guid

##### 1.6.3.1.1 Name

assetId

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

#### 1.6.3.3.0 Guid

##### 1.6.3.3.1 Name

parentAssetId

##### 1.6.3.3.2 Type

🔹 Guid

##### 1.6.3.3.3 Is Required

❌ No

##### 1.6.3.3.4 Is Primary Key

❌ No

##### 1.6.3.3.5 Is Unique

❌ No

##### 1.6.3.3.6 Index Type

Index

##### 1.6.3.3.7 Size

0

##### 1.6.3.3.8 Constraints

*No items available*

##### 1.6.3.3.9 Default Value



##### 1.6.3.3.10 Is Foreign Key

✅ Yes

##### 1.6.3.3.11 Precision

0

##### 1.6.3.3.12 Scale

0

#### 1.6.3.4.0 VARCHAR

##### 1.6.3.4.1 Name

name

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

255

##### 1.6.3.4.8 Constraints

*No items available*

##### 1.6.3.4.9 Default Value



##### 1.6.3.4.10 Is Foreign Key

❌ No

##### 1.6.3.4.11 Precision

0

##### 1.6.3.4.12 Scale

0

#### 1.6.3.5.0 TEXT

##### 1.6.3.5.1 Name

description

##### 1.6.3.5.2 Type

🔹 TEXT

##### 1.6.3.5.3 Is Required

❌ No

##### 1.6.3.5.4 Is Primary Key

❌ No

##### 1.6.3.5.5 Is Unique

❌ No

##### 1.6.3.5.6 Index Type

None

##### 1.6.3.5.7 Size

0

##### 1.6.3.5.8 Constraints

*No items available*

##### 1.6.3.5.9 Default Value



##### 1.6.3.5.10 Is Foreign Key

❌ No

##### 1.6.3.5.11 Precision

0

##### 1.6.3.5.12 Scale

0

#### 1.6.3.6.0 Guid

##### 1.6.3.6.1 Name

assetTemplateId

##### 1.6.3.6.2 Type

🔹 Guid

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

✅ Yes

##### 1.6.3.6.11 Precision

0

##### 1.6.3.6.12 Scale

0

#### 1.6.3.7.0 JSONB

##### 1.6.3.7.1 Name

metadata

##### 1.6.3.7.2 Type

🔹 JSONB

##### 1.6.3.7.3 Is Required

❌ No

##### 1.6.3.7.4 Is Primary Key

❌ No

##### 1.6.3.7.5 Is Unique

❌ No

##### 1.6.3.7.6 Index Type

None

##### 1.6.3.7.7 Size

0

##### 1.6.3.7.8 Constraints

*No items available*

##### 1.6.3.7.9 Default Value



##### 1.6.3.7.10 Is Foreign Key

❌ No

##### 1.6.3.7.11 Precision

0

##### 1.6.3.7.12 Scale

0

#### 1.6.3.8.0 VARCHAR

##### 1.6.3.8.1 Name

materializedPath

##### 1.6.3.8.2 Type

🔹 VARCHAR

##### 1.6.3.8.3 Is Required

❌ No

##### 1.6.3.8.4 Is Primary Key

❌ No

##### 1.6.3.8.5 Is Unique

❌ No

##### 1.6.3.8.6 Index Type

Index

##### 1.6.3.8.7 Size

1,024

##### 1.6.3.8.8 Constraints

*No items available*

##### 1.6.3.8.9 Default Value



##### 1.6.3.8.10 Is Foreign Key

❌ No

##### 1.6.3.8.11 Precision

0

##### 1.6.3.8.12 Scale

0

#### 1.6.3.9.0 DateTimeOffset

##### 1.6.3.9.1 Name

createdAt

##### 1.6.3.9.2 Type

🔹 DateTimeOffset

##### 1.6.3.9.3 Is Required

✅ Yes

##### 1.6.3.9.4 Is Primary Key

❌ No

##### 1.6.3.9.5 Is Unique

❌ No

##### 1.6.3.9.6 Index Type

Index

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

#### 1.6.3.10.0 DateTimeOffset

##### 1.6.3.10.1 Name

updatedAt

##### 1.6.3.10.2 Type

🔹 DateTimeOffset

##### 1.6.3.10.3 Is Required

✅ Yes

##### 1.6.3.10.4 Is Primary Key

❌ No

##### 1.6.3.10.5 Is Unique

❌ No

##### 1.6.3.10.6 Index Type

None

##### 1.6.3.10.7 Size

0

##### 1.6.3.10.8 Constraints

*No items available*

##### 1.6.3.10.9 Default Value

CURRENT_TIMESTAMP

##### 1.6.3.10.10 Is Foreign Key

❌ No

##### 1.6.3.10.11 Precision

0

##### 1.6.3.10.12 Scale

0

#### 1.6.3.11.0 BOOLEAN

##### 1.6.3.11.1 Name

isDeleted

##### 1.6.3.11.2 Type

🔹 BOOLEAN

##### 1.6.3.11.3 Is Required

✅ Yes

##### 1.6.3.11.4 Is Primary Key

❌ No

##### 1.6.3.11.5 Is Unique

❌ No

##### 1.6.3.11.6 Index Type

Index

##### 1.6.3.11.7 Size

0

##### 1.6.3.11.8 Constraints

*No items available*

##### 1.6.3.11.9 Default Value

false

##### 1.6.3.11.10 Is Foreign Key

❌ No

##### 1.6.3.11.11 Precision

0

##### 1.6.3.11.12 Scale

0

### 1.6.4.0.0 Primary Keys

- assetId

### 1.6.5.0.0 Unique Constraints

- {'name': 'UC_Asset_HierarchyName', 'columns': ['tenantId', 'parentAssetId', 'name']}

### 1.6.6.0.0 Indexes

#### 1.6.6.1.0 BTree

##### 1.6.6.1.1 Name

IX_Asset_TenantId

##### 1.6.6.1.2 Columns

- tenantId

##### 1.6.6.1.3 Type

🔹 BTree

#### 1.6.6.2.0 BTree

##### 1.6.6.2.1 Name

IX_Asset_ParentAssetId

##### 1.6.6.2.2 Columns

- parentAssetId

##### 1.6.6.2.3 Type

🔹 BTree

#### 1.6.6.3.0 BTree

##### 1.6.6.3.1 Name

IX_Asset_AssetTemplateId

##### 1.6.6.3.2 Columns

- assetTemplateId

##### 1.6.6.3.3 Type

🔹 BTree

#### 1.6.6.4.0 GIN

##### 1.6.6.4.1 Name

IX_Asset_Metadata_GIN

##### 1.6.6.4.2 Columns

- metadata

##### 1.6.6.4.3 Type

🔹 GIN

#### 1.6.6.5.0 BTree

##### 1.6.6.5.1 Name

IX_Asset_MaterializedPath

##### 1.6.6.5.2 Columns

- materializedPath

##### 1.6.6.5.3 Type

🔹 BTree

## 1.7.0.0.0 AssetTemplate

### 1.7.1.0.0 Name

AssetTemplate

### 1.7.2.0.0 Description

Defines a standard structure and properties for a type of asset. REQ-1-048.

### 1.7.3.0.0 Attributes

#### 1.7.3.1.0 Guid

##### 1.7.3.1.1 Name

assetTemplateId

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

tenantId

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

#### 1.7.3.4.0 TEXT

##### 1.7.3.4.1 Name

description

##### 1.7.3.4.2 Type

🔹 TEXT

##### 1.7.3.4.3 Is Required

❌ No

##### 1.7.3.4.4 Is Primary Key

❌ No

##### 1.7.3.4.5 Is Unique

❌ No

##### 1.7.3.4.6 Index Type

None

##### 1.7.3.4.7 Size

0

##### 1.7.3.4.8 Constraints

*No items available*

##### 1.7.3.4.9 Default Value



##### 1.7.3.4.10 Is Foreign Key

❌ No

##### 1.7.3.4.11 Precision

0

##### 1.7.3.4.12 Scale

0

#### 1.7.3.5.0 JSONB

##### 1.7.3.5.1 Name

propertyDefinitions

##### 1.7.3.5.2 Type

🔹 JSONB

##### 1.7.3.5.3 Is Required

✅ Yes

##### 1.7.3.5.4 Is Primary Key

❌ No

##### 1.7.3.5.5 Is Unique

❌ No

##### 1.7.3.5.6 Index Type

None

##### 1.7.3.5.7 Size

0

##### 1.7.3.5.8 Constraints

*No items available*

##### 1.7.3.5.9 Default Value

{}

##### 1.7.3.5.10 Is Foreign Key

❌ No

##### 1.7.3.5.11 Precision

0

##### 1.7.3.5.12 Scale

0

#### 1.7.3.6.0 BOOLEAN

##### 1.7.3.6.1 Name

isDeleted

##### 1.7.3.6.2 Type

🔹 BOOLEAN

##### 1.7.3.6.3 Is Required

✅ Yes

##### 1.7.3.6.4 Is Primary Key

❌ No

##### 1.7.3.6.5 Is Unique

❌ No

##### 1.7.3.6.6 Index Type

Index

##### 1.7.3.6.7 Size

0

##### 1.7.3.6.8 Constraints

*No items available*

##### 1.7.3.6.9 Default Value

false

##### 1.7.3.6.10 Is Foreign Key

❌ No

##### 1.7.3.6.11 Precision

0

##### 1.7.3.6.12 Scale

0

### 1.7.4.0.0 Primary Keys

- assetTemplateId

### 1.7.5.0.0 Unique Constraints

- {'name': 'UC_AssetTemplate_TenantName', 'columns': ['tenantId', 'name']}

### 1.7.6.0.0 Indexes

- {'name': 'IX_AssetTemplate_TenantId', 'columns': ['tenantId'], 'type': 'BTree'}

## 1.8.0.0.0 OpcTagMapping

### 1.8.1.0.0 Name

OpcTagMapping

### 1.8.2.0.0 Description

Maps a raw OPC tag from a data source to a specific property of an Asset. REQ-1-047.

### 1.8.3.0.0 Attributes

#### 1.8.3.1.0 Guid

##### 1.8.3.1.1 Name

opcTagMappingId

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

assetId

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

#### 1.8.3.3.0 Guid

##### 1.8.3.3.1 Name

opcCoreClientId

##### 1.8.3.3.2 Type

🔹 Guid

##### 1.8.3.3.3 Is Required

✅ Yes

##### 1.8.3.3.4 Is Primary Key

❌ No

##### 1.8.3.3.5 Is Unique

❌ No

##### 1.8.3.3.6 Index Type

Index

##### 1.8.3.3.7 Size

0

##### 1.8.3.3.8 Constraints

*No items available*

##### 1.8.3.3.9 Default Value



##### 1.8.3.3.10 Is Foreign Key

✅ Yes

##### 1.8.3.3.11 Precision

0

##### 1.8.3.3.12 Scale

0

#### 1.8.3.4.0 VARCHAR

##### 1.8.3.4.1 Name

opcServerEndpoint

##### 1.8.3.4.2 Type

🔹 VARCHAR

##### 1.8.3.4.3 Is Required

✅ Yes

##### 1.8.3.4.4 Is Primary Key

❌ No

##### 1.8.3.4.5 Is Unique

❌ No

##### 1.8.3.4.6 Index Type

None

##### 1.8.3.4.7 Size

512

##### 1.8.3.4.8 Constraints

*No items available*

##### 1.8.3.4.9 Default Value



##### 1.8.3.4.10 Is Foreign Key

❌ No

##### 1.8.3.4.11 Precision

0

##### 1.8.3.4.12 Scale

0

#### 1.8.3.5.0 VARCHAR

##### 1.8.3.5.1 Name

opcTagNodeId

##### 1.8.3.5.2 Type

🔹 VARCHAR

##### 1.8.3.5.3 Is Required

✅ Yes

##### 1.8.3.5.4 Is Primary Key

❌ No

##### 1.8.3.5.5 Is Unique

❌ No

##### 1.8.3.5.6 Index Type

Index

##### 1.8.3.5.7 Size

512

##### 1.8.3.5.8 Constraints

*No items available*

##### 1.8.3.5.9 Default Value



##### 1.8.3.5.10 Is Foreign Key

❌ No

##### 1.8.3.5.11 Precision

0

##### 1.8.3.5.12 Scale

0

#### 1.8.3.6.0 VARCHAR

##### 1.8.3.6.1 Name

assetPropertyName

##### 1.8.3.6.2 Type

🔹 VARCHAR

##### 1.8.3.6.3 Is Required

✅ Yes

##### 1.8.3.6.4 Is Primary Key

❌ No

##### 1.8.3.6.5 Is Unique

❌ No

##### 1.8.3.6.6 Index Type

Index

##### 1.8.3.6.7 Size

255

##### 1.8.3.6.8 Constraints

*No items available*

##### 1.8.3.6.9 Default Value



##### 1.8.3.6.10 Is Foreign Key

❌ No

##### 1.8.3.6.11 Precision

0

##### 1.8.3.6.12 Scale

0

### 1.8.4.0.0 Primary Keys

- opcTagMappingId

### 1.8.5.0.0 Unique Constraints

- {'name': 'UC_OpcTagMapping_AssetProperty', 'columns': ['assetId', 'assetPropertyName']}

### 1.8.6.0.0 Indexes

#### 1.8.6.1.0 BTree

##### 1.8.6.1.1 Name

IX_OpcTagMapping_AssetId

##### 1.8.6.1.2 Columns

- assetId

##### 1.8.6.1.3 Type

🔹 BTree

#### 1.8.6.2.0 BTree

##### 1.8.6.2.1 Name

IX_OpcTagMapping_OpcCoreClientId

##### 1.8.6.2.2 Columns

- opcCoreClientId

##### 1.8.6.2.3 Type

🔹 BTree

#### 1.8.6.3.0 BTree

##### 1.8.6.3.1 Name

IX_OpcTagMapping_ClientEndpointNode

##### 1.8.6.3.2 Columns

- opcCoreClientId
- opcServerEndpoint
- opcTagNodeId

##### 1.8.6.3.3 Type

🔹 BTree

## 1.9.0.0.0 AiModel

### 1.9.1.0.0 Name

AiModel

### 1.9.2.0.0 Description

Represents an AI/ML model available in the system for tasks like predictive maintenance. REQ-1-004.

### 1.9.3.0.0 Attributes

#### 1.9.3.1.0 Guid

##### 1.9.3.1.1 Name

aiModelId

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

tenantId

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

#### 1.9.3.3.0 VARCHAR

##### 1.9.3.3.1 Name

name

##### 1.9.3.3.2 Type

🔹 VARCHAR

##### 1.9.3.3.3 Is Required

✅ Yes

##### 1.9.3.3.4 Is Primary Key

❌ No

##### 1.9.3.3.5 Is Unique

❌ No

##### 1.9.3.3.6 Index Type

Index

##### 1.9.3.3.7 Size

255

##### 1.9.3.3.8 Constraints

*No items available*

##### 1.9.3.3.9 Default Value



##### 1.9.3.3.10 Is Foreign Key

❌ No

##### 1.9.3.3.11 Precision

0

##### 1.9.3.3.12 Scale

0

#### 1.9.3.4.0 TEXT

##### 1.9.3.4.1 Name

description

##### 1.9.3.4.2 Type

🔹 TEXT

##### 1.9.3.4.3 Is Required

❌ No

##### 1.9.3.4.4 Is Primary Key

❌ No

##### 1.9.3.4.5 Is Unique

❌ No

##### 1.9.3.4.6 Index Type

None

##### 1.9.3.4.7 Size

0

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

modelType

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

100

##### 1.9.3.5.8 Constraints

- ENUM('PredictiveMaintenance', 'AnomalyDetection', 'NLQ')

##### 1.9.3.5.9 Default Value



##### 1.9.3.5.10 Is Foreign Key

❌ No

##### 1.9.3.5.11 Precision

0

##### 1.9.3.5.12 Scale

0

#### 1.9.3.6.0 DateTimeOffset

##### 1.9.3.6.1 Name

createdAt

##### 1.9.3.6.2 Type

🔹 DateTimeOffset

##### 1.9.3.6.3 Is Required

✅ Yes

##### 1.9.3.6.4 Is Primary Key

❌ No

##### 1.9.3.6.5 Is Unique

❌ No

##### 1.9.3.6.6 Index Type

Index

##### 1.9.3.6.7 Size

0

##### 1.9.3.6.8 Constraints

*No items available*

##### 1.9.3.6.9 Default Value

CURRENT_TIMESTAMP

##### 1.9.3.6.10 Is Foreign Key

❌ No

##### 1.9.3.6.11 Precision

0

##### 1.9.3.6.12 Scale

0

#### 1.9.3.7.0 BOOLEAN

##### 1.9.3.7.1 Name

isDeleted

##### 1.9.3.7.2 Type

🔹 BOOLEAN

##### 1.9.3.7.3 Is Required

✅ Yes

##### 1.9.3.7.4 Is Primary Key

❌ No

##### 1.9.3.7.5 Is Unique

❌ No

##### 1.9.3.7.6 Index Type

Index

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

### 1.9.4.0.0 Primary Keys

- aiModelId

### 1.9.5.0.0 Unique Constraints

- {'name': 'UC_AiModel_TenantName', 'columns': ['tenantId', 'name']}

### 1.9.6.0.0 Indexes

#### 1.9.6.1.0 BTree

##### 1.9.6.1.1 Name

IX_AiModel_TenantId

##### 1.9.6.1.2 Columns

- tenantId

##### 1.9.6.1.3 Type

🔹 BTree

#### 1.9.6.2.0 BTree

##### 1.9.6.2.1 Name

IX_AiModel_ModelType

##### 1.9.6.2.2 Columns

- modelType

##### 1.9.6.2.3 Type

🔹 BTree

## 1.10.0.0.0 AiModelVersion

### 1.10.1.0.0 Name

AiModelVersion

### 1.10.2.0.0 Description

Represents a specific version of an AI Model, including its file and approval status. REQ-1-049, REQ-1-050.

### 1.10.3.0.0 Attributes

#### 1.10.3.1.0 Guid

##### 1.10.3.1.1 Name

aiModelVersionId

##### 1.10.3.1.2 Type

🔹 Guid

##### 1.10.3.1.3 Is Required

✅ Yes

##### 1.10.3.1.4 Is Primary Key

✅ Yes

##### 1.10.3.1.5 Is Unique

✅ Yes

##### 1.10.3.1.6 Index Type

UniqueIndex

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

aiModelId

##### 1.10.3.2.2 Type

🔹 Guid

##### 1.10.3.2.3 Is Required

✅ Yes

##### 1.10.3.2.4 Is Primary Key

❌ No

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

#### 1.10.3.3.0 VARCHAR

##### 1.10.3.3.1 Name

version

##### 1.10.3.3.2 Type

🔹 VARCHAR

##### 1.10.3.3.3 Is Required

✅ Yes

##### 1.10.3.3.4 Is Primary Key

❌ No

##### 1.10.3.3.5 Is Unique

❌ No

##### 1.10.3.3.6 Index Type

Index

##### 1.10.3.3.7 Size

50

##### 1.10.3.3.8 Constraints

*No items available*

##### 1.10.3.3.9 Default Value



##### 1.10.3.3.10 Is Foreign Key

❌ No

##### 1.10.3.3.11 Precision

0

##### 1.10.3.3.12 Scale

0

#### 1.10.3.4.0 VARCHAR

##### 1.10.3.4.1 Name

status

##### 1.10.3.4.2 Type

🔹 VARCHAR

##### 1.10.3.4.3 Is Required

✅ Yes

##### 1.10.3.4.4 Is Primary Key

❌ No

##### 1.10.3.4.5 Is Unique

❌ No

##### 1.10.3.4.6 Index Type

Index

##### 1.10.3.4.7 Size

50

##### 1.10.3.4.8 Constraints

- ENUM('Submitted', 'Approved', 'Rejected', 'Archived')

##### 1.10.3.4.9 Default Value

Submitted

##### 1.10.3.4.10 Is Foreign Key

❌ No

##### 1.10.3.4.11 Precision

0

##### 1.10.3.4.12 Scale

0

#### 1.10.3.5.0 VARCHAR

##### 1.10.3.5.1 Name

modelStoragePath

##### 1.10.3.5.2 Type

🔹 VARCHAR

##### 1.10.3.5.3 Is Required

✅ Yes

##### 1.10.3.5.4 Is Primary Key

❌ No

##### 1.10.3.5.5 Is Unique

❌ No

##### 1.10.3.5.6 Index Type

None

##### 1.10.3.5.7 Size

1,024

##### 1.10.3.5.8 Constraints

*No items available*

##### 1.10.3.5.9 Default Value



##### 1.10.3.5.10 Is Foreign Key

❌ No

##### 1.10.3.5.11 Precision

0

##### 1.10.3.5.12 Scale

0

#### 1.10.3.6.0 Guid

##### 1.10.3.6.1 Name

submittedByUserId

##### 1.10.3.6.2 Type

🔹 Guid

##### 1.10.3.6.3 Is Required

✅ Yes

##### 1.10.3.6.4 Is Primary Key

❌ No

##### 1.10.3.6.5 Is Unique

❌ No

##### 1.10.3.6.6 Index Type

Index

##### 1.10.3.6.7 Size

0

##### 1.10.3.6.8 Constraints

*No items available*

##### 1.10.3.6.9 Default Value



##### 1.10.3.6.10 Is Foreign Key

✅ Yes

##### 1.10.3.6.11 Precision

0

##### 1.10.3.6.12 Scale

0

#### 1.10.3.7.0 Guid

##### 1.10.3.7.1 Name

approvedByUserId

##### 1.10.3.7.2 Type

🔹 Guid

##### 1.10.3.7.3 Is Required

❌ No

##### 1.10.3.7.4 Is Primary Key

❌ No

##### 1.10.3.7.5 Is Unique

❌ No

##### 1.10.3.7.6 Index Type

Index

##### 1.10.3.7.7 Size

0

##### 1.10.3.7.8 Constraints

*No items available*

##### 1.10.3.7.9 Default Value



##### 1.10.3.7.10 Is Foreign Key

✅ Yes

##### 1.10.3.7.11 Precision

0

##### 1.10.3.7.12 Scale

0

#### 1.10.3.8.0 JSONB

##### 1.10.3.8.1 Name

performanceMetrics

##### 1.10.3.8.2 Type

🔹 JSONB

##### 1.10.3.8.3 Is Required

❌ No

##### 1.10.3.8.4 Is Primary Key

❌ No

##### 1.10.3.8.5 Is Unique

❌ No

##### 1.10.3.8.6 Index Type

None

##### 1.10.3.8.7 Size

0

##### 1.10.3.8.8 Constraints

*No items available*

##### 1.10.3.8.9 Default Value



##### 1.10.3.8.10 Is Foreign Key

❌ No

##### 1.10.3.8.11 Precision

0

##### 1.10.3.8.12 Scale

0

#### 1.10.3.9.0 DateTimeOffset

##### 1.10.3.9.1 Name

createdAt

##### 1.10.3.9.2 Type

🔹 DateTimeOffset

##### 1.10.3.9.3 Is Required

✅ Yes

##### 1.10.3.9.4 Is Primary Key

❌ No

##### 1.10.3.9.5 Is Unique

❌ No

##### 1.10.3.9.6 Index Type

Index

##### 1.10.3.9.7 Size

0

##### 1.10.3.9.8 Constraints

*No items available*

##### 1.10.3.9.9 Default Value

CURRENT_TIMESTAMP

##### 1.10.3.9.10 Is Foreign Key

❌ No

##### 1.10.3.9.11 Precision

0

##### 1.10.3.9.12 Scale

0

### 1.10.4.0.0 Primary Keys

- aiModelVersionId

### 1.10.5.0.0 Unique Constraints

- {'name': 'UC_AiModelVersion_ModelVersion', 'columns': ['aiModelId', 'version']}

### 1.10.6.0.0 Indexes

#### 1.10.6.1.0 BTree

##### 1.10.6.1.1 Name

IX_AiModelVersion_AiModelId

##### 1.10.6.1.2 Columns

- aiModelId

##### 1.10.6.1.3 Type

🔹 BTree

#### 1.10.6.2.0 BTree

##### 1.10.6.2.1 Name

IX_AiModelVersion_Status

##### 1.10.6.2.2 Columns

- status

##### 1.10.6.2.3 Type

🔹 BTree

#### 1.10.6.3.0 BTree

##### 1.10.6.3.1 Name

IX_AiModelVersion_SubmittedBy

##### 1.10.6.3.2 Columns

- submittedByUserId

##### 1.10.6.3.3 Type

🔹 BTree

#### 1.10.6.4.0 BTree

##### 1.10.6.4.1 Name

IX_AiModelVersion_ModelStatusCreatedDesc

##### 1.10.6.4.2 Columns

- aiModelId
- status
- createdAt

##### 1.10.6.4.3 Type

🔹 BTree

##### 1.10.6.4.4 Order

- ASC
- ASC
- DESC

## 1.11.0.0.0 AiModelAssignment

### 1.11.1.0.0 Name

AiModelAssignment

### 1.11.2.0.0 Description

Links an approved AI Model Version to a specific Asset for execution. REQ-1-014, REQ-1-056.

### 1.11.3.0.0 Attributes

#### 1.11.3.1.0 Guid

##### 1.11.3.1.1 Name

aiModelAssignmentId

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

assetId

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

✅ Yes

##### 1.11.3.2.11 Precision

0

##### 1.11.3.2.12 Scale

0

#### 1.11.3.3.0 Guid

##### 1.11.3.3.1 Name

aiModelVersionId

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

✅ Yes

##### 1.11.3.3.11 Precision

0

##### 1.11.3.3.12 Scale

0

#### 1.11.3.4.0 BOOLEAN

##### 1.11.3.4.1 Name

isActive

##### 1.11.3.4.2 Type

🔹 BOOLEAN

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

true

##### 1.11.3.4.10 Is Foreign Key

❌ No

##### 1.11.3.4.11 Precision

0

##### 1.11.3.4.12 Scale

0

#### 1.11.3.5.0 JSONB

##### 1.11.3.5.1 Name

configuration

##### 1.11.3.5.2 Type

🔹 JSONB

##### 1.11.3.5.3 Is Required

❌ No

##### 1.11.3.5.4 Is Primary Key

❌ No

##### 1.11.3.5.5 Is Unique

❌ No

##### 1.11.3.5.6 Index Type

None

##### 1.11.3.5.7 Size

0

##### 1.11.3.5.8 Constraints

*No items available*

##### 1.11.3.5.9 Default Value



##### 1.11.3.5.10 Is Foreign Key

❌ No

##### 1.11.3.5.11 Precision

0

##### 1.11.3.5.12 Scale

0

#### 1.11.3.6.0 BOOLEAN

##### 1.11.3.6.1 Name

isDeleted

##### 1.11.3.6.2 Type

🔹 BOOLEAN

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

false

##### 1.11.3.6.10 Is Foreign Key

❌ No

##### 1.11.3.6.11 Precision

0

##### 1.11.3.6.12 Scale

0

### 1.11.4.0.0 Primary Keys

- aiModelAssignmentId

### 1.11.5.0.0 Unique Constraints

- {'name': 'UC_AiModelAssignment_AssetVersion', 'columns': ['assetId', 'aiModelVersionId']}

### 1.11.6.0.0 Indexes

#### 1.11.6.1.0 BTree

##### 1.11.6.1.1 Name

IX_AiModelAssignment_AssetId

##### 1.11.6.1.2 Columns

- assetId

##### 1.11.6.1.3 Type

🔹 BTree

#### 1.11.6.2.0 BTree

##### 1.11.6.2.1 Name

IX_AiModelAssignment_AiModelVersionId

##### 1.11.6.2.2 Columns

- aiModelVersionId

##### 1.11.6.2.3 Type

🔹 BTree

#### 1.11.6.3.0 BTree

##### 1.11.6.3.1 Name

IX_AiModelAssignment_AssetActiveDeleted

##### 1.11.6.3.2 Columns

- assetId
- isActive
- isDeleted

##### 1.11.6.3.3 Type

🔹 BTree

## 1.12.0.0.0 AuditLog

### 1.12.1.0.0 Name

AuditLog

### 1.12.2.0.0 Description

Stores a tamper-evident record of all significant user and system actions. REQ-1-023, REQ-1-040.

### 1.12.3.0.0 Partitioning

| Property | Value |
|----------|-------|
| Strategy | Range |
| Column | timestamp |
| Interval | Monthly |

### 1.12.4.0.0 Attributes

#### 1.12.4.1.0 BIGINT

##### 1.12.4.1.1 Name

auditLogId

##### 1.12.4.1.2 Type

🔹 BIGINT

##### 1.12.4.1.3 Is Required

✅ Yes

##### 1.12.4.1.4 Is Primary Key

✅ Yes

##### 1.12.4.1.5 Is Unique

✅ Yes

##### 1.12.4.1.6 Index Type

UniqueIndex

##### 1.12.4.1.7 Size

0

##### 1.12.4.1.8 Constraints

- AUTO_INCREMENT

##### 1.12.4.1.9 Default Value



##### 1.12.4.1.10 Is Foreign Key

❌ No

##### 1.12.4.1.11 Precision

0

##### 1.12.4.1.12 Scale

0

#### 1.12.4.2.0 Guid

##### 1.12.4.2.1 Name

tenantId

##### 1.12.4.2.2 Type

🔹 Guid

##### 1.12.4.2.3 Is Required

✅ Yes

##### 1.12.4.2.4 Is Primary Key

❌ No

##### 1.12.4.2.5 Is Unique

❌ No

##### 1.12.4.2.6 Index Type

Index

##### 1.12.4.2.7 Size

0

##### 1.12.4.2.8 Constraints

*No items available*

##### 1.12.4.2.9 Default Value



##### 1.12.4.2.10 Is Foreign Key

✅ Yes

##### 1.12.4.2.11 Precision

0

##### 1.12.4.2.12 Scale

0

#### 1.12.4.3.0 Guid

##### 1.12.4.3.1 Name

userId

##### 1.12.4.3.2 Type

🔹 Guid

##### 1.12.4.3.3 Is Required

❌ No

##### 1.12.4.3.4 Is Primary Key

❌ No

##### 1.12.4.3.5 Is Unique

❌ No

##### 1.12.4.3.6 Index Type

Index

##### 1.12.4.3.7 Size

0

##### 1.12.4.3.8 Constraints

*No items available*

##### 1.12.4.3.9 Default Value



##### 1.12.4.3.10 Is Foreign Key

✅ Yes

##### 1.12.4.3.11 Precision

0

##### 1.12.4.3.12 Scale

0

#### 1.12.4.4.0 VARCHAR

##### 1.12.4.4.1 Name

userIdentifier

##### 1.12.4.4.2 Type

🔹 VARCHAR

##### 1.12.4.4.3 Is Required

❌ No

##### 1.12.4.4.4 Is Primary Key

❌ No

##### 1.12.4.4.5 Is Unique

❌ No

##### 1.12.4.4.6 Index Type

None

##### 1.12.4.4.7 Size

255

##### 1.12.4.4.8 Constraints

*No items available*

##### 1.12.4.4.9 Default Value



##### 1.12.4.4.10 Is Foreign Key

❌ No

##### 1.12.4.4.11 Precision

0

##### 1.12.4.4.12 Scale

0

#### 1.12.4.5.0 DateTimeOffset

##### 1.12.4.5.1 Name

timestamp

##### 1.12.4.5.2 Type

🔹 DateTimeOffset

##### 1.12.4.5.3 Is Required

✅ Yes

##### 1.12.4.5.4 Is Primary Key

❌ No

##### 1.12.4.5.5 Is Unique

❌ No

##### 1.12.4.5.6 Index Type

Index

##### 1.12.4.5.7 Size

0

##### 1.12.4.5.8 Constraints

*No items available*

##### 1.12.4.5.9 Default Value

CURRENT_TIMESTAMP

##### 1.12.4.5.10 Is Foreign Key

❌ No

##### 1.12.4.5.11 Precision

0

##### 1.12.4.5.12 Scale

0

#### 1.12.4.6.0 VARCHAR

##### 1.12.4.6.1 Name

sourceIpAddress

##### 1.12.4.6.2 Type

🔹 VARCHAR

##### 1.12.4.6.3 Is Required

❌ No

##### 1.12.4.6.4 Is Primary Key

❌ No

##### 1.12.4.6.5 Is Unique

❌ No

##### 1.12.4.6.6 Index Type

None

##### 1.12.4.6.7 Size

45

##### 1.12.4.6.8 Constraints

*No items available*

##### 1.12.4.6.9 Default Value



##### 1.12.4.6.10 Is Foreign Key

❌ No

##### 1.12.4.6.11 Precision

0

##### 1.12.4.6.12 Scale

0

#### 1.12.4.7.0 VARCHAR

##### 1.12.4.7.1 Name

action

##### 1.12.4.7.2 Type

🔹 VARCHAR

##### 1.12.4.7.3 Is Required

✅ Yes

##### 1.12.4.7.4 Is Primary Key

❌ No

##### 1.12.4.7.5 Is Unique

❌ No

##### 1.12.4.7.6 Index Type

Index

##### 1.12.4.7.7 Size

255

##### 1.12.4.7.8 Constraints

*No items available*

##### 1.12.4.7.9 Default Value



##### 1.12.4.7.10 Is Foreign Key

❌ No

##### 1.12.4.7.11 Precision

0

##### 1.12.4.7.12 Scale

0

#### 1.12.4.8.0 VARCHAR

##### 1.12.4.8.1 Name

entityName

##### 1.12.4.8.2 Type

🔹 VARCHAR

##### 1.12.4.8.3 Is Required

❌ No

##### 1.12.4.8.4 Is Primary Key

❌ No

##### 1.12.4.8.5 Is Unique

❌ No

##### 1.12.4.8.6 Index Type

Index

##### 1.12.4.8.7 Size

100

##### 1.12.4.8.8 Constraints

*No items available*

##### 1.12.4.8.9 Default Value



##### 1.12.4.8.10 Is Foreign Key

❌ No

##### 1.12.4.8.11 Precision

0

##### 1.12.4.8.12 Scale

0

#### 1.12.4.9.0 VARCHAR

##### 1.12.4.9.1 Name

entityId

##### 1.12.4.9.2 Type

🔹 VARCHAR

##### 1.12.4.9.3 Is Required

❌ No

##### 1.12.4.9.4 Is Primary Key

❌ No

##### 1.12.4.9.5 Is Unique

❌ No

##### 1.12.4.9.6 Index Type

Index

##### 1.12.4.9.7 Size

255

##### 1.12.4.9.8 Constraints

*No items available*

##### 1.12.4.9.9 Default Value



##### 1.12.4.9.10 Is Foreign Key

❌ No

##### 1.12.4.9.11 Precision

0

##### 1.12.4.9.12 Scale

0

#### 1.12.4.10.0 JSONB

##### 1.12.4.10.1 Name

details

##### 1.12.4.10.2 Type

🔹 JSONB

##### 1.12.4.10.3 Is Required

❌ No

##### 1.12.4.10.4 Is Primary Key

❌ No

##### 1.12.4.10.5 Is Unique

❌ No

##### 1.12.4.10.6 Index Type

None

##### 1.12.4.10.7 Size

0

##### 1.12.4.10.8 Constraints

*No items available*

##### 1.12.4.10.9 Default Value



##### 1.12.4.10.10 Is Foreign Key

❌ No

##### 1.12.4.10.11 Precision

0

##### 1.12.4.10.12 Scale

0

#### 1.12.4.11.0 VARCHAR

##### 1.12.4.11.1 Name

qldbAnchorHash

##### 1.12.4.11.2 Type

🔹 VARCHAR

##### 1.12.4.11.3 Is Required

❌ No

##### 1.12.4.11.4 Is Primary Key

❌ No

##### 1.12.4.11.5 Is Unique

❌ No

##### 1.12.4.11.6 Index Type

Index

##### 1.12.4.11.7 Size

255

##### 1.12.4.11.8 Constraints

*No items available*

##### 1.12.4.11.9 Default Value



##### 1.12.4.11.10 Is Foreign Key

❌ No

##### 1.12.4.11.11 Precision

0

##### 1.12.4.11.12 Scale

0

### 1.12.5.0.0 Primary Keys

- auditLogId

### 1.12.6.0.0 Unique Constraints

*No items available*

### 1.12.7.0.0 Indexes

#### 1.12.7.1.0 BTree

##### 1.12.7.1.1 Name

IX_AuditLog_TenantTimestampDesc

##### 1.12.7.1.2 Columns

- tenantId
- timestamp

##### 1.12.7.1.3 Type

🔹 BTree

##### 1.12.7.1.4 Order

- ASC
- DESC

#### 1.12.7.2.0 BTree

##### 1.12.7.2.1 Name

IX_AuditLog_UserId

##### 1.12.7.2.2 Columns

- userId

##### 1.12.7.2.3 Type

🔹 BTree

#### 1.12.7.3.0 BTree

##### 1.12.7.3.1 Name

IX_AuditLog_Entity

##### 1.12.7.3.2 Columns

- entityName
- entityId

##### 1.12.7.3.3 Type

🔹 BTree

## 1.13.0.0.0 DataRetentionPolicy

### 1.13.1.0.0 Name

DataRetentionPolicy

### 1.13.2.0.0 Description

Configurable policies for retaining different types of data (e.g., time-series, audit logs). REQ-1-088.

### 1.13.3.0.0 Attributes

#### 1.13.3.1.0 Guid

##### 1.13.3.1.1 Name

dataRetentionPolicyId

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

dataType

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

100

##### 1.13.3.3.8 Constraints

- ENUM('RawTimeSeries', 'AggregatedTimeSeries', 'AuditLog', 'AlarmEvent')

##### 1.13.3.3.9 Default Value



##### 1.13.3.3.10 Is Foreign Key

❌ No

##### 1.13.3.3.11 Precision

0

##### 1.13.3.3.12 Scale

0

#### 1.13.3.4.0 INT

##### 1.13.3.4.1 Name

retentionPeriodDays

##### 1.13.3.4.2 Type

🔹 INT

##### 1.13.3.4.3 Is Required

✅ Yes

##### 1.13.3.4.4 Is Primary Key

❌ No

##### 1.13.3.4.5 Is Unique

❌ No

##### 1.13.3.4.6 Index Type

None

##### 1.13.3.4.7 Size

0

##### 1.13.3.4.8 Constraints

- POSITIVE_VALUE

##### 1.13.3.4.9 Default Value



##### 1.13.3.4.10 Is Foreign Key

❌ No

##### 1.13.3.4.11 Precision

0

##### 1.13.3.4.12 Scale

0

#### 1.13.3.5.0 BOOLEAN

##### 1.13.3.5.1 Name

isActive

##### 1.13.3.5.2 Type

🔹 BOOLEAN

##### 1.13.3.5.3 Is Required

✅ Yes

##### 1.13.3.5.4 Is Primary Key

❌ No

##### 1.13.3.5.5 Is Unique

❌ No

##### 1.13.3.5.6 Index Type

Index

##### 1.13.3.5.7 Size

0

##### 1.13.3.5.8 Constraints

*No items available*

##### 1.13.3.5.9 Default Value

true

##### 1.13.3.5.10 Is Foreign Key

❌ No

##### 1.13.3.5.11 Precision

0

##### 1.13.3.5.12 Scale

0

### 1.13.4.0.0 Primary Keys

- dataRetentionPolicyId

### 1.13.5.0.0 Unique Constraints

- {'name': 'UC_DataRetentionPolicy_TenantDataType', 'columns': ['tenantId', 'dataType']}

### 1.13.6.0.0 Indexes

- {'name': 'IX_DataRetentionPolicy_TenantId', 'columns': ['tenantId'], 'type': 'BTree'}

## 1.14.0.0.0 NotificationPreference

### 1.14.1.0.0 Name

NotificationPreference

### 1.14.2.0.0 Description

Stores user-specific preferences for receiving notifications. REQ-1-066.

### 1.14.3.0.0 Attributes

#### 1.14.3.1.0 Guid

##### 1.14.3.1.1 Name

notificationPreferenceId

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

userId

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

notificationCategory

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

100

##### 1.14.3.3.8 Constraints

- ENUM('CriticalAlarm', 'SystemUpdate', 'ReportGenerated')

##### 1.14.3.3.9 Default Value



##### 1.14.3.3.10 Is Foreign Key

❌ No

##### 1.14.3.3.11 Precision

0

##### 1.14.3.3.12 Scale

0

#### 1.14.3.4.0 VARCHAR

##### 1.14.3.4.1 Name

channel

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

- ENUM('Email', 'SMS', 'InApp')

##### 1.14.3.4.9 Default Value



##### 1.14.3.4.10 Is Foreign Key

❌ No

##### 1.14.3.4.11 Precision

0

##### 1.14.3.4.12 Scale

0

#### 1.14.3.5.0 BOOLEAN

##### 1.14.3.5.1 Name

isEnabled

##### 1.14.3.5.2 Type

🔹 BOOLEAN

##### 1.14.3.5.3 Is Required

✅ Yes

##### 1.14.3.5.4 Is Primary Key

❌ No

##### 1.14.3.5.5 Is Unique

❌ No

##### 1.14.3.5.6 Index Type

Index

##### 1.14.3.5.7 Size

0

##### 1.14.3.5.8 Constraints

*No items available*

##### 1.14.3.5.9 Default Value

true

##### 1.14.3.5.10 Is Foreign Key

❌ No

##### 1.14.3.5.11 Precision

0

##### 1.14.3.5.12 Scale

0

### 1.14.4.0.0 Primary Keys

- notificationPreferenceId

### 1.14.5.0.0 Unique Constraints

- {'name': 'UC_NotificationPreference_UserPref', 'columns': ['userId', 'notificationCategory', 'channel']}

### 1.14.6.0.0 Indexes

- {'name': 'IX_NotificationPreference_UserId', 'columns': ['userId'], 'type': 'BTree'}

## 1.15.0.0.0 TimeSeriesData

### 1.15.1.0.0 Name

TimeSeriesData

### 1.15.2.0.0 Description

Stores high-frequency time-series data from OPC tags. Implemented as a TimescaleDB hypertable for performance. REQ-1-075.

### 1.15.3.0.0 Partitioning

#### 1.15.3.1.0 Strategy

Hypertable

#### 1.15.3.2.0 Time Column

timestamp

#### 1.15.3.3.0 Space Columns

- opcTagMappingId

### 1.15.4.0.0 Attributes

#### 1.15.4.1.0 DateTimeOffset

##### 1.15.4.1.1 Name

timestamp

##### 1.15.4.1.2 Type

🔹 DateTimeOffset

##### 1.15.4.1.3 Is Required

✅ Yes

##### 1.15.4.1.4 Is Primary Key

✅ Yes

##### 1.15.4.1.5 Is Unique

❌ No

##### 1.15.4.1.6 Index Type

Index

#### 1.15.4.2.0 Guid

##### 1.15.4.2.1 Name

opcTagMappingId

##### 1.15.4.2.2 Type

🔹 Guid

##### 1.15.4.2.3 Is Required

✅ Yes

##### 1.15.4.2.4 Is Primary Key

✅ Yes

##### 1.15.4.2.5 Is Unique

❌ No

##### 1.15.4.2.6 Index Type

Index

##### 1.15.4.2.7 Is Foreign Key

✅ Yes

#### 1.15.4.3.0 DOUBLE PRECISION

##### 1.15.4.3.1 Name

value

##### 1.15.4.3.2 Type

🔹 DOUBLE PRECISION

##### 1.15.4.3.3 Is Required

✅ Yes

##### 1.15.4.3.4 Is Primary Key

❌ No

##### 1.15.4.3.5 Is Unique

❌ No

#### 1.15.4.4.0 INT

##### 1.15.4.4.1 Name

quality

##### 1.15.4.4.2 Type

🔹 INT

##### 1.15.4.4.3 Is Required

✅ Yes

##### 1.15.4.4.4 Is Primary Key

❌ No

##### 1.15.4.4.5 Is Unique

❌ No

### 1.15.5.0.0 Primary Keys

- timestamp
- opcTagMappingId

### 1.15.6.0.0 Unique Constraints

*No items available*

### 1.15.7.0.0 Indexes

*No items available*

## 1.16.0.0.0 EffectiveUserPermissions

### 1.16.1.0.0 Name

EffectiveUserPermissions

### 1.16.2.0.0 Description

A materialized view that pre-calculates and flattens user permissions across the asset hierarchy to accelerate authorization checks. REQ-1-061.

### 1.16.3.0.0 Is Materialized View

✅ Yes

### 1.16.4.0.0 Attributes

#### 1.16.4.1.0 Guid

##### 1.16.4.1.1 Name

userId

##### 1.16.4.1.2 Type

🔹 Guid

##### 1.16.4.1.3 Is Required

✅ Yes

##### 1.16.4.1.4 Is Primary Key

✅ Yes

##### 1.16.4.1.5 Is Unique

❌ No

##### 1.16.4.1.6 Is Foreign Key

✅ Yes

#### 1.16.4.2.0 Guid

##### 1.16.4.2.1 Name

assetId

##### 1.16.4.2.2 Type

🔹 Guid

##### 1.16.4.2.3 Is Required

✅ Yes

##### 1.16.4.2.4 Is Primary Key

✅ Yes

##### 1.16.4.2.5 Is Unique

❌ No

##### 1.16.4.2.6 Is Foreign Key

✅ Yes

#### 1.16.4.3.0 JSONB

##### 1.16.4.3.1 Name

permissionSet

##### 1.16.4.3.2 Type

🔹 JSONB

##### 1.16.4.3.3 Is Required

✅ Yes

##### 1.16.4.3.4 Is Primary Key

❌ No

##### 1.16.4.3.5 Is Unique

❌ No

### 1.16.5.0.0 Primary Keys

- userId
- assetId

### 1.16.6.0.0 Unique Constraints

*No items available*

### 1.16.7.0.0 Indexes

- {'name': 'IX_EffectivePerms_AssetId', 'columns': ['assetId'], 'type': 'BTree'}

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

✅ Yes

### 2.1.9.0.0 Is Identifying

❌ No

### 2.1.10.0.0 On Delete

Cascade

### 2.1.11.0.0 On Update

Cascade

### 2.1.12.0.0 Join Table

#### 2.1.12.1.0 Name

ForeignKeyInUser

#### 2.1.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

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

✅ Yes

### 2.2.9.0.0 Is Identifying

❌ No

### 2.2.10.0.0 On Delete

Cascade

### 2.2.11.0.0 On Update

Cascade

### 2.2.12.0.0 Join Table

#### 2.2.12.1.0 Name

ForeignKeyInOpcCoreClient

#### 2.2.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

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

✅ Yes

### 2.3.9.0.0 Is Identifying

❌ No

### 2.3.10.0.0 On Delete

Cascade

### 2.3.11.0.0 On Update

Cascade

### 2.3.12.0.0 Join Table

#### 2.3.12.1.0 Name

ForeignKeyInAsset

#### 2.3.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

## 2.4.0.0.0 OneToMany

### 2.4.1.0.0 Name

TenantAssetTemplates

### 2.4.2.0.0 Id

REL_TENANT_ASSETTEMPLATE_001

### 2.4.3.0.0 Source Entity

Tenant

### 2.4.4.0.0 Target Entity

AssetTemplate

### 2.4.5.0.0 Type

🔹 OneToMany

### 2.4.6.0.0 Source Multiplicity

1

### 2.4.7.0.0 Target Multiplicity

0..*

### 2.4.8.0.0 Cascade Delete

✅ Yes

### 2.4.9.0.0 Is Identifying

❌ No

### 2.4.10.0.0 On Delete

Cascade

### 2.4.11.0.0 On Update

Cascade

### 2.4.12.0.0 Join Table

#### 2.4.12.1.0 Name

ForeignKeyInAssetTemplate

#### 2.4.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

## 2.5.0.0.0 OneToMany

### 2.5.1.0.0 Name

TenantAiModels

### 2.5.2.0.0 Id

REL_TENANT_AIMODEL_001

### 2.5.3.0.0 Source Entity

Tenant

### 2.5.4.0.0 Target Entity

AiModel

### 2.5.5.0.0 Type

🔹 OneToMany

### 2.5.6.0.0 Source Multiplicity

1

### 2.5.7.0.0 Target Multiplicity

0..*

### 2.5.8.0.0 Cascade Delete

✅ Yes

### 2.5.9.0.0 Is Identifying

❌ No

### 2.5.10.0.0 On Delete

Cascade

### 2.5.11.0.0 On Update

Cascade

### 2.5.12.0.0 Join Table

#### 2.5.12.1.0 Name

ForeignKeyInAiModel

#### 2.5.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

## 2.6.0.0.0 OneToMany

### 2.6.1.0.0 Name

TenantAuditLogs

### 2.6.2.0.0 Id

REL_TENANT_AUDITLOG_001

### 2.6.3.0.0 Source Entity

Tenant

### 2.6.4.0.0 Target Entity

AuditLog

### 2.6.5.0.0 Type

🔹 OneToMany

### 2.6.6.0.0 Source Multiplicity

1

### 2.6.7.0.0 Target Multiplicity

0..*

### 2.6.8.0.0 Cascade Delete

✅ Yes

### 2.6.9.0.0 Is Identifying

❌ No

### 2.6.10.0.0 On Delete

Cascade

### 2.6.11.0.0 On Update

Cascade

### 2.6.12.0.0 Join Table

#### 2.6.12.1.0 Name

ForeignKeyInAuditLog

#### 2.6.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

## 2.7.0.0.0 OneToMany

### 2.7.1.0.0 Name

TenantDataRetentionPolicies

### 2.7.2.0.0 Id

REL_TENANT_DATARETPOL_001

### 2.7.3.0.0 Source Entity

Tenant

### 2.7.4.0.0 Target Entity

DataRetentionPolicy

### 2.7.5.0.0 Type

🔹 OneToMany

### 2.7.6.0.0 Source Multiplicity

1

### 2.7.7.0.0 Target Multiplicity

0..*

### 2.7.8.0.0 Cascade Delete

✅ Yes

### 2.7.9.0.0 Is Identifying

❌ No

### 2.7.10.0.0 On Delete

Cascade

### 2.7.11.0.0 On Update

Cascade

### 2.7.12.0.0 Join Table

#### 2.7.12.1.0 Name

ForeignKeyInDataRetentionPolicy

#### 2.7.12.2.0 Columns

- {'name': 'tenantId', 'type': 'Guid', 'references': 'Tenant.tenantId'}

## 2.8.0.0.0 OneToMany

### 2.8.1.0.0 Name

UserRoleAssignments

### 2.8.2.0.0 Id

REL_USER_USERROLE_001

### 2.8.3.0.0 Source Entity

User

### 2.8.4.0.0 Target Entity

UserRole

### 2.8.5.0.0 Type

🔹 OneToMany

### 2.8.6.0.0 Source Multiplicity

1

### 2.8.7.0.0 Target Multiplicity

0..*

### 2.8.8.0.0 Cascade Delete

✅ Yes

### 2.8.9.0.0 Is Identifying

✅ Yes

### 2.8.10.0.0 On Delete

Cascade

### 2.8.11.0.0 On Update

Cascade

### 2.8.12.0.0 Join Table

#### 2.8.12.1.0 Name

ForeignKeyInUserRole

#### 2.8.12.2.0 Columns

- {'name': 'userId', 'type': 'Guid', 'references': 'User.userId'}

## 2.9.0.0.0 OneToMany

### 2.9.1.0.0 Name

RoleUserAssignments

### 2.9.2.0.0 Id

REL_ROLE_USERROLE_001

### 2.9.3.0.0 Source Entity

Role

### 2.9.4.0.0 Target Entity

UserRole

### 2.9.5.0.0 Type

🔹 OneToMany

### 2.9.6.0.0 Source Multiplicity

1

### 2.9.7.0.0 Target Multiplicity

0..*

### 2.9.8.0.0 Cascade Delete

✅ Yes

### 2.9.9.0.0 Is Identifying

✅ Yes

### 2.9.10.0.0 On Delete

Cascade

### 2.9.11.0.0 On Update

Cascade

### 2.9.12.0.0 Join Table

#### 2.9.12.1.0 Name

ForeignKeyInUserRole

#### 2.9.12.2.0 Columns

- {'name': 'roleId', 'type': 'Guid', 'references': 'Role.roleId'}

## 2.10.0.0.0 OneToMany

### 2.10.1.0.0 Name

AssetRoleScope

### 2.10.2.0.0 Id

REL_ASSET_USERROLE_001

### 2.10.3.0.0 Source Entity

Asset

### 2.10.4.0.0 Target Entity

UserRole

### 2.10.5.0.0 Type

🔹 OneToMany

### 2.10.6.0.0 Source Multiplicity

1

### 2.10.7.0.0 Target Multiplicity

0..*

### 2.10.8.0.0 Cascade Delete

✅ Yes

### 2.10.9.0.0 Is Identifying

✅ Yes

### 2.10.10.0.0 On Delete

Cascade

### 2.10.11.0.0 On Update

Cascade

### 2.10.12.0.0 Join Table

#### 2.10.12.1.0 Name

ForeignKeyInUserRole

#### 2.10.12.2.0 Columns

- {'name': 'assetScopeId', 'type': 'Guid', 'references': 'Asset.assetId'}

## 2.11.0.0.0 OneToMany

### 2.11.1.0.0 Name

UserNotificationPreferences

### 2.11.2.0.0 Id

REL_USER_NOTIFPREF_001

### 2.11.3.0.0 Source Entity

User

### 2.11.4.0.0 Target Entity

NotificationPreference

### 2.11.5.0.0 Type

🔹 OneToMany

### 2.11.6.0.0 Source Multiplicity

1

### 2.11.7.0.0 Target Multiplicity

0..*

### 2.11.8.0.0 Cascade Delete

✅ Yes

### 2.11.9.0.0 Is Identifying

❌ No

### 2.11.10.0.0 On Delete

Cascade

### 2.11.11.0.0 On Update

Cascade

### 2.11.12.0.0 Join Table

#### 2.11.12.1.0 Name

ForeignKeyInNotificationPreference

#### 2.11.12.2.0 Columns

- {'name': 'userId', 'type': 'Guid', 'references': 'User.userId'}

## 2.12.0.0.0 OneToMany

### 2.12.1.0.0 Name

UserSubmittedAiModelVersions

### 2.12.2.0.0 Id

REL_USER_AIMODELVERSION_SUBMIT_001

### 2.12.3.0.0 Source Entity

User

### 2.12.4.0.0 Target Entity

AiModelVersion

### 2.12.5.0.0 Type

🔹 OneToMany

### 2.12.6.0.0 Source Multiplicity

1

### 2.12.7.0.0 Target Multiplicity

0..*

### 2.12.8.0.0 Cascade Delete

❌ No

### 2.12.9.0.0 Is Identifying

❌ No

### 2.12.10.0.0 On Delete

SetNull

### 2.12.11.0.0 On Update

Cascade

### 2.12.12.0.0 Join Table

#### 2.12.12.1.0 Name

ForeignKeyInAiModelVersion

#### 2.12.12.2.0 Columns

- {'name': 'submittedByUserId', 'type': 'Guid', 'references': 'User.userId'}

## 2.13.0.0.0 OneToMany

### 2.13.1.0.0 Name

UserApprovedAiModelVersions

### 2.13.2.0.0 Id

REL_USER_AIMODELVERSION_APPROVE_001

### 2.13.3.0.0 Source Entity

User

### 2.13.4.0.0 Target Entity

AiModelVersion

### 2.13.5.0.0 Type

🔹 OneToMany

### 2.13.6.0.0 Source Multiplicity

0..1

### 2.13.7.0.0 Target Multiplicity

0..*

### 2.13.8.0.0 Cascade Delete

❌ No

### 2.13.9.0.0 Is Identifying

❌ No

### 2.13.10.0.0 On Delete

SetNull

### 2.13.11.0.0 On Update

Cascade

### 2.13.12.0.0 Join Table

#### 2.13.12.1.0 Name

ForeignKeyInAiModelVersion

#### 2.13.12.2.0 Columns

- {'name': 'approvedByUserId', 'type': 'Guid', 'references': 'User.userId'}

## 2.14.0.0.0 OneToMany

### 2.14.1.0.0 Name

UserAuditLogs

### 2.14.2.0.0 Id

REL_USER_AUDITLOG_001

### 2.14.3.0.0 Source Entity

User

### 2.14.4.0.0 Target Entity

AuditLog

### 2.14.5.0.0 Type

🔹 OneToMany

### 2.14.6.0.0 Source Multiplicity

0..1

### 2.14.7.0.0 Target Multiplicity

0..*

### 2.14.8.0.0 Cascade Delete

❌ No

### 2.14.9.0.0 Is Identifying

❌ No

### 2.14.10.0.0 On Delete

SetNull

### 2.14.11.0.0 On Update

Cascade

### 2.14.12.0.0 Join Table

#### 2.14.12.1.0 Name

ForeignKeyInAuditLog

#### 2.14.12.2.0 Columns

- {'name': 'userId', 'type': 'Guid', 'references': 'User.userId'}

## 2.15.0.0.0 OneToMany

### 2.15.1.0.0 Name

AssetHierarchy

### 2.15.2.0.0 Id

REL_ASSET_ASSET_PARENT_001

### 2.15.3.0.0 Source Entity

Asset

### 2.15.4.0.0 Target Entity

Asset

### 2.15.5.0.0 Type

🔹 OneToMany

### 2.15.6.0.0 Source Multiplicity

0..1

### 2.15.7.0.0 Target Multiplicity

0..*

### 2.15.8.0.0 Cascade Delete

❌ No

### 2.15.9.0.0 Is Identifying

❌ No

### 2.15.10.0.0 On Delete

SetNull

### 2.15.11.0.0 On Update

Cascade

### 2.15.12.0.0 Join Table

#### 2.15.12.1.0 Name

ForeignKeyInAsset

#### 2.15.12.2.0 Columns

- {'name': 'parentAssetId', 'type': 'Guid', 'references': 'Asset.assetId'}

## 2.16.0.0.0 OneToMany

### 2.16.1.0.0 Name

AssetTemplateInstances

### 2.16.2.0.0 Id

REL_ASSETTEMPLATE_ASSET_001

### 2.16.3.0.0 Source Entity

AssetTemplate

### 2.16.4.0.0 Target Entity

Asset

### 2.16.5.0.0 Type

🔹 OneToMany

### 2.16.6.0.0 Source Multiplicity

1

### 2.16.7.0.0 Target Multiplicity

0..*

### 2.16.8.0.0 Cascade Delete

❌ No

### 2.16.9.0.0 Is Identifying

❌ No

### 2.16.10.0.0 On Delete

SetNull

### 2.16.11.0.0 On Update

Cascade

### 2.16.12.0.0 Join Table

#### 2.16.12.1.0 Name

ForeignKeyInAsset

#### 2.16.12.2.0 Columns

- {'name': 'assetTemplateId', 'type': 'Guid', 'references': 'AssetTemplate.assetTemplateId'}

## 2.17.0.0.0 OneToMany

### 2.17.1.0.0 Name

AssetTagMappings

### 2.17.2.0.0 Id

REL_ASSET_OPCTAGMAP_001

### 2.17.3.0.0 Source Entity

Asset

### 2.17.4.0.0 Target Entity

OpcTagMapping

### 2.17.5.0.0 Type

🔹 OneToMany

### 2.17.6.0.0 Source Multiplicity

1

### 2.17.7.0.0 Target Multiplicity

0..*

### 2.17.8.0.0 Cascade Delete

✅ Yes

### 2.17.9.0.0 Is Identifying

❌ No

### 2.17.10.0.0 On Delete

Cascade

### 2.17.11.0.0 On Update

Cascade

### 2.17.12.0.0 Join Table

#### 2.17.12.1.0 Name

ForeignKeyInOpcTagMapping

#### 2.17.12.2.0 Columns

- {'name': 'assetId', 'type': 'Guid', 'references': 'Asset.assetId'}

## 2.18.0.0.0 OneToMany

### 2.18.1.0.0 Name

ClientTagMappings

### 2.18.2.0.0 Id

REL_OPCCLIENT_OPCTAGMAP_001

### 2.18.3.0.0 Source Entity

OpcCoreClient

### 2.18.4.0.0 Target Entity

OpcTagMapping

### 2.18.5.0.0 Type

🔹 OneToMany

### 2.18.6.0.0 Source Multiplicity

1

### 2.18.7.0.0 Target Multiplicity

0..*

### 2.18.8.0.0 Cascade Delete

✅ Yes

### 2.18.9.0.0 Is Identifying

❌ No

### 2.18.10.0.0 On Delete

Cascade

### 2.18.11.0.0 On Update

Cascade

### 2.18.12.0.0 Join Table

#### 2.18.12.1.0 Name

ForeignKeyInOpcTagMapping

#### 2.18.12.2.0 Columns

- {'name': 'opcCoreClientId', 'type': 'Guid', 'references': 'OpcCoreClient.opcCoreClientId'}

## 2.19.0.0.0 OneToMany

### 2.19.1.0.0 Name

AssetModelAssignments

### 2.19.2.0.0 Id

REL_ASSET_AIMODELASSIGN_001

### 2.19.3.0.0 Source Entity

Asset

### 2.19.4.0.0 Target Entity

AiModelAssignment

### 2.19.5.0.0 Type

🔹 OneToMany

### 2.19.6.0.0 Source Multiplicity

1

### 2.19.7.0.0 Target Multiplicity

0..*

### 2.19.8.0.0 Cascade Delete

✅ Yes

### 2.19.9.0.0 Is Identifying

❌ No

### 2.19.10.0.0 On Delete

Cascade

### 2.19.11.0.0 On Update

Cascade

### 2.19.12.0.0 Join Table

#### 2.19.12.1.0 Name

ForeignKeyInAiModelAssignment

#### 2.19.12.2.0 Columns

- {'name': 'assetId', 'type': 'Guid', 'references': 'Asset.assetId'}

## 2.20.0.0.0 OneToMany

### 2.20.1.0.0 Name

ModelVersions

### 2.20.2.0.0 Id

REL_AIMODEL_AIMODELVERSION_001

### 2.20.3.0.0 Source Entity

AiModel

### 2.20.4.0.0 Target Entity

AiModelVersion

### 2.20.5.0.0 Type

🔹 OneToMany

### 2.20.6.0.0 Source Multiplicity

1

### 2.20.7.0.0 Target Multiplicity

0..*

### 2.20.8.0.0 Cascade Delete

✅ Yes

### 2.20.9.0.0 Is Identifying

❌ No

### 2.20.10.0.0 On Delete

Cascade

### 2.20.11.0.0 On Update

Cascade

### 2.20.12.0.0 Join Table

#### 2.20.12.1.0 Name

ForeignKeyInAiModelVersion

#### 2.20.12.2.0 Columns

- {'name': 'aiModelId', 'type': 'Guid', 'references': 'AiModel.aiModelId'}

## 2.21.0.0.0 OneToMany

### 2.21.1.0.0 Name

VersionModelAssignments

### 2.21.2.0.0 Id

REL_AIMODELVERSION_AIMODELASSIGN_001

### 2.21.3.0.0 Source Entity

AiModelVersion

### 2.21.4.0.0 Target Entity

AiModelAssignment

### 2.21.5.0.0 Type

🔹 OneToMany

### 2.21.6.0.0 Source Multiplicity

1

### 2.21.7.0.0 Target Multiplicity

0..*

### 2.21.8.0.0 Cascade Delete

✅ Yes

### 2.21.9.0.0 Is Identifying

❌ No

### 2.21.10.0.0 On Delete

Cascade

### 2.21.11.0.0 On Update

Cascade

### 2.21.12.0.0 Join Table

#### 2.21.12.1.0 Name

ForeignKeyInAiModelAssignment

#### 2.21.12.2.0 Columns

- {'name': 'aiModelVersionId', 'type': 'Guid', 'references': 'AiModelVersion.aiModelVersionId'}

