# 1 Overview

## 1.1 Diagram Id

SEQ-EH-002

## 1.2 Name

Microservice Circuit Breaker Trip

## 1.3 Description

The Query Service makes a gRPC call to a downstream service (e.g., Asset Service) which is slow or unresponsive. After several consecutive failures, the circuit breaker in the Query Service's client trips to an 'Open' state, causing subsequent calls to fail fast without making a network request. After a timeout, it allows a single trial request to test if the dependency has recovered.

## 1.4 Type

🔹 ErrorHandling

## 1.5 Purpose

To prevent cascading failures and improve system resilience by isolating failing dependencies, allowing the calling service to degrade gracefully instead of failing completely. This is a key tactic for achieving high availability (REQ-NFR-002).

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

Rare

## 1.9 Participants

- REPO-SVC-DQR
- REPO-SVC-AST

## 1.10 Key Interactions

- The Query Service repeatedly calls the Asset Service, and the calls time out or fail.
- The failure count exceeds the configured threshold in the circuit breaker policy (e.g., 5 consecutive failures).
- The circuit breaker transitions to the 'Open' state.
- Subsequent calls from the Query Service to the Asset Service immediately fail locally with an exception, without a network call.
- After the 'Open' duration expires (e.g., 30 seconds), the circuit transitions to 'Half-Open'.
- The next call is allowed through. If it succeeds, the circuit closes; if it fails, it re-opens.

## 1.11 Triggers

- A downstream microservice dependency becomes unhealthy, consistently failing or timing out on requests.

## 1.12 Outcomes

- The calling service is protected from consuming resources (threads, memory) on a failing dependency.
- The overall system remains more stable and responsive.
- The failing dependency is given time to recover without being overloaded by continuous requests.

## 1.13 Business Rules

- Circuit breakers should be implemented on all critical synchronous inter-service communication paths.

## 1.14 Error Scenarios

- The dependency does not recover, and the circuit remains open, causing a partial loss of functionality.
- A 'flapping' scenario where the dependency is intermittently available, causing the circuit to open and close rapidly.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-EH-002

## 2.2 Name

Implementation: Microservice Circuit Breaker Trip and Recovery

## 2.3 Description

A detailed technical sequence showing the state transitions of a Polly-based circuit breaker in a .NET gRPC client (Query Service) when a downstream dependency (Asset Service) fails and subsequently recovers. It covers the transition from Closed -> Open -> Half-Open -> Closed, including fail-fast behavior and monitoring integration.

## 2.4 Participants

### 2.4.1 Service

#### 2.4.1.1 Repository Id

REPO-SVC-DQR

#### 2.4.1.2 Display Name

Query Service

#### 2.4.1.3 Type

🔹 Service

#### 2.4.1.4 Technology

.NET 8, gRPC Client, Polly v8.x

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | <<Client (with Breaker)>> |

### 2.4.2.0 Service

#### 2.4.2.1 Repository Id

REPO-SVC-AST

#### 2.4.2.2 Display Name

Asset Service

#### 2.4.2.3 Type

🔹 Service

#### 2.4.2.4 Technology

.NET 8, gRPC Server

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #FF4500 |
| Stereotype | <<Dependency>> |

## 2.5.0.0 Interactions

### 2.5.1.0 Synchronous Request-Response

#### 2.5.1.1 Source Id

REPO-SVC-DQR

#### 2.5.1.2 Target Id

REPO-SVC-AST

#### 2.5.1.3 Message

gRPC Call: GetAssetDetails(request) [Loop 1..N]

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Synchronous Request-Response

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

Error: gRPC StatusCode.DeadlineExceeded

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | asset.v1.AssetService/GetAssetDetails |
| Parameters | GetAssetDetailsRequest protobuf message |
| Authentication | Handled by K8s network policy or service mesh. |
| Error Handling | Call timeout configured (e.g., 500ms). Each failur... |
| Performance | N/A (Failing call) |

### 2.5.2.0 Internal State Transition

#### 2.5.2.1 Source Id

REPO-SVC-DQR

#### 2.5.2.2 Target Id

REPO-SVC-DQR

#### 2.5.2.3 Message

Internal: Failure threshold (e.g., 5) breached. Circuit Breaker transitions to OPEN state.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Internal State Transition

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal/In-Memory |
| Method | Polly.CircuitBreaker.OnBreak |
| Parameters | Consecutive failure count, last exception. |
| Authentication | N/A |
| Error Handling | Polly policy triggers state change. Log event with... |
| Performance | State change is sub-millisecond. |

### 2.5.3.0 Local Method Call

#### 2.5.3.1 Source Id

REPO-SVC-DQR

#### 2.5.3.2 Target Id

REPO-SVC-DQR

#### 2.5.3.3 Message

Application attempts gRPC call: GetAssetDetails(request)

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Local Method Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

Immediate Exception: Polly.CircuitBreaker.BrokenCircuitException

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal/In-Memory |
| Method | Polly.CircuitBreaker.ExecuteAsync |
| Parameters | The gRPC call delegate. |
| Authentication | N/A |
| Error Handling | Caller must catch BrokenCircuitException and imple... |
| Performance | Fails fast (<1ms). |

### 2.5.4.0 Internal State Transition

#### 2.5.4.1 Source Id

REPO-SVC-DQR

#### 2.5.4.2 Target Id

REPO-SVC-DQR

#### 2.5.4.3 Message

Internal: Open duration (e.g., 30s) expired. Circuit Breaker transitions to HALF-OPEN state.

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Internal State Transition

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message



#### 2.5.4.8 Has Return

❌ No

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal/In-Memory |
| Method | Polly.CircuitBreaker.OnHalfOpen |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | Polly policy timer triggers state change. The next... |
| Performance | State change is sub-millisecond. |

### 2.5.5.0 Synchronous Request-Response

#### 2.5.5.1 Source Id

REPO-SVC-DQR

#### 2.5.5.2 Target Id

REPO-SVC-AST

#### 2.5.5.3 Message

gRPC Trial Call: GetAssetDetails(request) [Half-Open State]

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 Synchronous Request-Response

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

Success: AssetDetailsResponse

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | asset.v1.AssetService/GetAssetDetails |
| Parameters | GetAssetDetailsRequest protobuf message |
| Authentication | Handled by K8s network policy or service mesh. |
| Error Handling | The success or failure of this single call determi... |
| Performance | Normal response time (e.g., <50ms). |

### 2.5.6.0 Internal State Transition

#### 2.5.6.1 Source Id

REPO-SVC-DQR

#### 2.5.6.2 Target Id

REPO-SVC-DQR

#### 2.5.6.3 Message

Internal: Trial call succeeded. Circuit Breaker transitions to CLOSED state.

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 Internal State Transition

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message



#### 2.5.6.8 Has Return

❌ No

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal/In-Memory |
| Method | Polly.CircuitBreaker.OnReset |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | Polly policy resets the consecutive failure count.... |
| Performance | State change is sub-millisecond. |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Circuit Breaker is configured via application settings (e.g., FailureThreshold=5, DurationOfBreak=30s) to allow for environment-specific tuning.

#### 2.6.1.2 Position

TopLeft

#### 2.6.1.3 Participant Id

REPO-SVC-DQR

#### 2.6.1.4 Sequence Number

0

### 2.6.2.0 Content

#### 2.6.2.1 Content

The Asset Service is experiencing a transient issue like a pod restart, high load, or temporary loss of DB connectivity, which later resolves.

#### 2.6.2.2 Position

TopRight

#### 2.6.2.3 Participant Id

REPO-SVC-AST

#### 2.6.2.4 Sequence Number

0

### 2.6.3.0 Content

#### 2.6.3.1 Content

If the Trial Call at step 5 had failed, the breaker would have immediately returned to the OPEN state for another 30s period without resetting the failure count.

#### 2.6.3.2 Position

Bottom

#### 2.6.3.3 Participant Id

*Not specified*

#### 2.6.3.4 Sequence Number

5

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Since this is internal traffic within a Kubernetes... |
| Performance Targets | Fail-fast latency for calls made when the circuit ... |
| Error Handling Strategy | The primary strategy is the Circuit Breaker patter... |
| Testing Considerations | Implement integration tests using a fault-injectio... |
| Monitoring Requirements | The Query Service must expose Prometheus metrics f... |
| Deployment Considerations | Circuit breaker settings (failure threshold, break... |

