# Distributed AI Token Limiter in C# using Redis and ASP.NET Core

A lightweight distributed token governance layer for AI/LLM workloads built with:
- C#
- ASP.NET Core
- Redis
- StackExchange.Redis

---
# Overview

Traditional API rate limiting strategies are generally request-based.

That model works reasonably well for standard REST APIs, but becomes insufficient for Large Language Model (LLM) systems where infrastructure cost is primarily driven by token consumption rather than request count.

This project implements a distributed token limiter designed specifically for AI workloads.

The implementation focuses on:
- distributed token accounting
- centralized governance
- low operational overhead
- Redis-based atomic counters
- pre-inference enforcement
- horizontally scalable API infrastructure

---

# Why Token-Based Limiting

In AI systems, two requests may have completely different infrastructure impact depending on:
- prompt size
- context window size
- retrieval augmented generation (RAG)
- conversation history
- generated response length
- model selection

Example:

| User | Requests | Tokens Per Request | Total Tokens |
|---|---|---|---|
| User A | 10 | 200 | 2,000 |
| User B | 10 | 50,000 | 500,000 |

Traditional request-per-minute limits do not capture actual AI infrastructure consumption.

This implementation focuses on token-aware governance instead.
# Architecture

The solution consists of two primary components:

## Token Consumption Service

Responsible for:
- token accounting
- limit evaluation
- Redis interaction
- enforcement policy retrieval

## ASP.NET Core Enforcement Filter

Responsible for:
- request interception
- onboarding validation
- token eligibility enforcement
- early request rejection

# Installation

Install the NuGet package:

```bash
dotnet add package nucleotidz.token.limiter
```

---

# Configuration

Add the following Redis configuration section inside `appsettings.json`.

```json
"Redis": {
  "EndPoints": "localhost",
  "Port": 6379,
  "UserName": "",
  "Password": ""
}
```

---

# Dependency Injection

Register the token limiter services inside `Program.cs`.

```csharp
builder.Services.AddAITokenLimiter(
    builder.Configuration,
    nucleotidz.token.limiter.configuration.LimiterType.FixedWindow);

builder.Services.AddAITokenLimiterFilter();
```

---

# Protecting APIs

Apply the `AITokenRateLimiter` filter on APIs that should enforce token governance.

```csharp
[HttpGet("get")]
[ServiceFilter(typeof(AITokenRateLimiter))]
public async Task<IActionResult> Get(
    [FromHeader(Name = "x-ai-client-key")] string clientkey)
{
    return Ok("Request successful");
}
```

The limiter expects the client identifier to be passed through:

```text
x-ai-client-key
```

---

# User / Client Onboarding

Before a client can consume AI tokens, it must first be onboarded with:
- token limit
- enforcement window

Use `IOnboardService` for onboarding.

Example:

```csharp
[HttpPost("onboard")]
public async Task<IActionResult> Onboard(
    TokenLimitModel tokenLimitModel)
{
    await onboardService.OnBoardAsync(tokenLimitModel);

    return Ok("User onboarded successfully.");
}
```

Example payload:

```json
{
  "user": "client-001",
  "limit": 100000,
  "window": "01:00:00"
}
```

---

# Consuming Tokens

After every successful AI / Agent response, consume the generated tokens using `ITokenLimiter`.

```csharp
await tokenLimiter.ConsumeAsync(clientkey, tokens);
```

This updates the distributed runtime token counter stored in Redis.

---

# Example Request Flow

```text
Client Request
      ↓
ASP.NET Core API
      ↓
AITokenRateLimiter Filter
      ↓
Redis Validation
      ↓
Allowed / Rejected
      ↓
AI Agent / LLM Execution
      ↓
Consume Tokens
```

---

# HTTP Responses

| Status Code | Meaning |
|---|---|
| 200 | Request allowed |
| 406 | Client not onboarded |
| 429 | Token limit exhausted |

---

# Supported Limiter Types

Currently supported:
- FixedWindow

Example:

```csharp
LimiterType.FixedWindow
```

Future versions may include:
- Sliding Window
- Token Bucket
- Leaky Bucket
- Distributed Burst Policies
