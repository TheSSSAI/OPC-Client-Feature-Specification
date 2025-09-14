# 1 Strategies

## 1.1 Retry

### 1.1.1 Type

🔹 Retry

### 1.1.2 Configuration

#### 1.1.2.1 Description

Handles transient network and service issues for both Edge-to-Cloud and Cloud-to-Cloud communication. Edge clients use an aggressive, long-running retry to ensure eventual data delivery, leveraging their on-disk buffer.

#### 1.1.2.2 Policies

##### 1.1.2.2.1 Scope

###### 1.1.2.2.1.1 Scope

OPC Core Client (to Central Management Plane)

###### 1.1.2.2.1.2 Strategy

Exponential Backoff with Jitter

###### 1.1.2.2.1.3 Max Attempts

-1

###### 1.1.2.2.1.4 Initial Interval

5s

###### 1.1.2.2.1.5 Max Interval

5m

###### 1.1.2.2.1.6 Timeout Per Attempt

30s

###### 1.1.2.2.1.7 Error Handling Rules

- NetworkConnectivityError
- GrpcServiceUnavailable
- MqttConnectionLost

##### 1.1.2.2.2.0 Scope

###### 1.1.2.2.2.1 Scope

Cloud Microservices (Inter-service & Database)

###### 1.1.2.2.2.2 Strategy

Fixed Interval

###### 1.1.2.2.2.3 Max Attempts

3

###### 1.1.2.2.2.4 Interval

250ms

###### 1.1.2.2.2.5 Timeout Per Attempt

2s

###### 1.1.2.2.2.6 Error Handling Rules

- DatabaseTransientError
- RedisConnectionError
- InternalGrpcTransientError

## 1.2.0.0.0.0 CircuitBreaker

### 1.2.1.0.0.0 Type

🔹 CircuitBreaker

### 1.2.2.0.0.0 Configuration

#### 1.2.2.1.0.0 Description

Protects cloud microservices from cascading failures during synchronous inter-service calls (gRPC/REST). Prevents a failing service from overwhelming its callers. (REQ-1-085)

#### 1.2.2.2.0.0 Scope

Cloud Microservices (Inter-service gRPC/REST)

#### 1.2.2.3.0.0 Failure Threshold

5

#### 1.2.2.4.0.0 Open Duration

30s

#### 1.2.2.5.0.0 Half Open Actions

1

#### 1.2.2.6.0.0 Error Handling Rules

- ServiceUnavailableError
- Http5xxError
- RequestTimeoutError

## 1.3.0.0.0.0 Fallback

### 1.3.1.0.0.0 Type

🔹 Fallback

### 1.3.2.0.0.0 Configuration

#### 1.3.2.1.0.0 Description

Ensures graceful degradation for data query operations in the Central Management Plane. When the primary database (TimescaleDB) is unavailable, it serves recently cached data from Redis. (REQ-1-026)

#### 1.3.2.2.0.0 Scope

Query & Analytics Service

#### 1.3.2.3.0.0 Fallback Response

ServeStaleCacheFromRedis

#### 1.3.2.4.0.0 Error Handling Rules

- DatabaseUnavailableError
- CircuitBreakerOpen

## 1.4.0.0.0.0 DeadLetter

### 1.4.1.0.0.0 Type

🔹 DeadLetter

### 1.4.2.0.0.0 Configuration

#### 1.4.2.1.0.0 Description

Captures and alerts on non-retriable, critical errors that require immediate manual intervention. The primary action is to log with CRITICAL severity and trigger a high-priority alert. (REQ-1-079, REQ-1-090)

#### 1.4.2.2.0.0 Action

LogAndTriggerPagerDutyAlert

#### 1.4.2.3.0.0 Error Handling Rules

- InvalidConfigurationError
- AuthenticationError
- BufferFullError
- MessageDeserializationError

# 2.0.0.0.0.0 Monitoring

## 2.1.0.0.0.0 Error Types

- NetworkConnectivityError
- GrpcServiceUnavailable
- MqttConnectionLost
- DatabaseTransientError
- RedisConnectionError
- ServiceUnavailableError
- Http5xxError
- RequestTimeoutError
- DatabaseUnavailableError
- CircuitBreakerOpen
- InvalidConfigurationError
- AuthenticationError
- BufferFullError
- MessageDeserializationError

## 2.2.0.0.0.0 Alerting

As per REQ-1-090, Alertmanager is configured to trigger critical alerts for specific error conditions. Key triggers include: CircuitBreakerOpen events, DeadLetter queue errors (e.g., BufferFullError), and sustained OPC Core Client offline status (via Prometheus heartbeat monitoring). Alerts are routed to on-call personnel via PagerDuty and to a shared engineering channel in Slack. All errors are logged in a structured JSON format to a centralized OpenSearch cluster with a correlation ID for distributed tracing and analysis in Grafana.

