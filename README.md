# Snipe-IT Automation Project using Playwright and .NET 10

Built with .NET 10, Playwright for .NET 1.63 and NUnit.

## What the test does

`AssetTests.CreateAsset_CheckedOutToRandomUser` covers the full flow:

1. Logs in to the Snipe-IT demo as admin
2. Creates a new Macbook Pro 13" asset with status "Ready to Deploy", checked out to a random user
3. Searches for the new asset in the assets list
4. Opens the asset page from the list and checks the asset tag, model, company, status and assigned user
5. Opens the History tab and checks the "create new" and "checkout" entries

There are also two smaller login tests in `LoginTests`.

## Project structure

```
snipeit.Automation/
├── test.runsettings          # browser, headed/headless and speed settings
└── snipeit.Tests/
    ├── Config/
    │   └── TestSettings.cs   # URL and login settings
    ├── Pages/                # page objects, one per screen
    │   ├── LoginPage.cs
    │   ├── DashBoardPage.cs
    │   ├── CreateAssetPage.cs
    │   ├── AssetsListPage.cs
    │   └── AssetDetailsPage.cs
    ├── LoginTests.cs
    └── AssetTests.cs
```

I used the Page Object Model so each screen's locators and actions live in one place. The tests only describe the steps and the expected results.

## How to run

### Prerequisites

- .NET 10 SDK
- PowerShell 7 (needed for the Playwright browser install script)

### Steps

1. Clone the repo and open a terminal in the repo folder
2. Build the project:
```
   dotnet build
```
3. Install the Playwright browser:
```
   pwsh snipeit.Tests/bin/Debug/net10.0/playwright.ps1 install chromium
```
4. Run the tests:
```
   dotnet test --settings test.runsettings
```

Or in Visual Studio: Test > Configure Run Settings > Select Solution Wide runsettings File > `test.runsettings`, then Test Explorer > Run All.

## Run settings

`test.runsettings` currently runs the browser visibly and slowed down (1 second between actions), so it's easy to watch what the test is doing. To run fast and in the background, change:

```xml
<Headless>true</Headless>
<SlowMo>0</SlowMo>
```

## Configuration

The site URL and login details are in `Config/TestSettings.cs`. By default they use the public demo (admin / password). To run against another Snipe-IT site, set these environment variables:

- `SNIPEIT_URL`
- `SNIPEIT_USER`
- `SNIPEIT_PASSWORD`

## Notes on test data

- The demo site is shared by many people and resets regularly, so the test creates its own data each run instead of relying on existing records.
- The asset tag includes a timestamp so it's unique on every run.
- The random user is picked from the first page of users that loads in the dropdown. The chosen user's name is saved and checked later on the asset page and in History. The dropdown shows names as "Last First (username)" and the asset page shows "First Last", so the test compares each part of the name.
- The company ("Bruen LLC") comes from the demo's sample data. If the demo regenerates its data and this company disappears, update the `company` value at the top of the test.

## Things I'd add with more time

- Run the tests in a CI pipeline (e.g. GitHub Actions) on each push
- Pick the company randomly as well, so the test doesn't depend on any fixed sample data
- Screenshots and Playwright traces on failure
- Clean up created assets after the test