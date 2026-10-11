# PolicyPilot BDD Automation

A .NET 10 test automation sample for insurance quote APIs, using C#, Playwright API testing, xUnit, and Reqnroll/Gherkin.

## What It Covers

- API tests for quote creation using Playwright's API request client.
- Gherkin scenarios generated and run through Reqnroll with xUnit.
- GitHub Actions CI with TRX test reports published by `dorny/test-reporter`.

Browser-based UI automation is not implemented yet. `UiBaseUrl` is reserved for that work.

## Tracing Test Data

If a value is not shown in a feature or table, trace how the request is built before concluding that the value is missing. Follow the object through the step definition, test-data helper, any nested helper or factory, and the service call. Check what the final request contains immediately before it is sent.

In this project, quote data flows from the feature to `QuoteSteps`, then from `QuoteTestData` into a `QuoteRequest`, which `QuoteService` sends to the API. When investigating a field such as a country code, check each point in that chain for a default or later assignment.

## Step Definition Patterns

Reqnroll step bindings can use regular expressions or Cucumber Expressions. For example, both styles can capture a quoted policy status:

```csharp
[When(@"Policy Status ""(.*)""")]
```

```csharp
[When("Policy Status {string}")]
```

## Prerequisites

- .NET 10 SDK
- Network access to the configured MockAPI project

## Configuration

By default, tests use a local HTTP test server, so they do not depend on a remote API or shared test data. Set the `API_BASE_URL` environment variable to opt in to testing against an external API; when it is not set, the local test server is used. The service posts to `/api/v1/quotes` relative to `ApiBaseUrl`.

The MockAPI `quotes` resource is expected to provide these fields:

| Field | Expected type |
| --- | --- |
| `id` | String/Object ID |
| `customerId` | String |
| `registrationNumber` | String |
| `status` | String |
| `coverType` | String |
| `premium` | Number greater than zero |

The configured MockAPI host is available for manual use by setting `API_BASE_URL`. Tests never write to it by default.

## Run Tests

From the repository root:

```powershell
dotnet restore
dotnet test --configuration Release
```

Run only one test group:

```powershell
dotnet test --filter FullyQualifiedName~QuoteApiTests
dotnet test --filter FullyQualifiedName~QuoteAPIFeature
```

The local test server returns deterministic quote responses and accepts deletion of quotes created during tests.

## Continuous Integration

`.github/workflows/dotnet.yml` runs the full test suite on pushes and pull requests, and supports manual dispatch with a GitHub Environment and an optional `dotnet test --filter` expression. CI uses the local test server by default. To test against an external API, set its `API_BASE_URL` variable in the selected GitHub Environment; add separate environments for separate API targets. The workflow targets Ubuntu 24.04 and .NET 10, writes a TRX report, and publishes results with `dorny/test-reporter`.

For example, use `FullyQualifiedName~QuoteApiTests` to run only the API test class, or `FullyQualifiedName~QuoteAPIFeature` to run only the Reqnroll feature tests.
