# PolicyPilot BDD Automation

A .NET 10 test automation sample for insurance quote APIs, using C#, Playwright API testing, xUnit, and Reqnroll/Gherkin.

## What It Covers

- API tests for quote creation using Playwright's API request client.
- Gherkin scenarios generated and run through Reqnroll with xUnit.
- GitHub Actions CI with TRX test reports published by `dorny/test-reporter`.

Browser-based UI automation is not implemented yet. `UiBaseUrl` is reserved for that work.

## Prerequisites

- .NET 10 SDK
- Network access to the configured MockAPI project

## Configuration

Set the API host in `Config/appsetting.json`. The service posts to `/api/v1/quotes` relative to `ApiBaseUrl`.

The MockAPI `quotes` resource is expected to provide these fields:

| Field | Expected type |
| --- | --- |
| `id` | String/Object ID |
| `customerId` | String |
| `registrationNumber` | String |
| `status` | String |
| `coverType` | String |
| `premium` | Number greater than zero |

Use your own MockAPI host if you do not want tests writing to the configured shared resource.

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

The tests create quote records in MockAPI on each run. The generic MockAPI resource does not enforce insurance validation; the invalid-registration scenario documents that it accepts the request.

## Continuous Integration

`.github/workflows/dotnet.yml` runs the full test suite on pushes, pull requests, and manual dispatch. It targets Ubuntu 24.04 and .NET 10, writes a TRX report, and publishes the results to GitHub with `dorny/test-reporter`.
