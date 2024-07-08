# Playwright-SpecFlow

This repository contains a test automation framework using Playwright with SpecFlow for BDD testing. It is designed to test the [SauceDemo](https://www.saucedemo.com/v1/index.html) application.

## Table of Contents

- [Setup](#setup)
- [Page Object Model](#page-object-model)
- [SpecFlow How-To](#specflow-how-to)
- [Running Tests](#running-tests)
  - [Locally](#locally)
  - [Azure Pipelines](#azure-pipelines)
  - [GitHub Actions](#github-actions)

## Setup

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/) (for Playwright)
- [SpecFlow](https://specflow.org/)
- [Playwright](https://playwright.dev/)

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/avinashs15/PlayWright-Specflow.git
    cd PlayWright-Specflow
    ```

2. Install Playwright:
    ```sh
    npx playwright install //skip if you want to execute the tests on the installed browsers
    ```

3. Restore .NET dependencies:
    ```sh
    dotnet restore
    ```

## Page Object Model

The Page Object Model (POM) is a design pattern that creates an object repository for web UI elements. This repository uses POM for better maintainability and reusability.

### Example

Here's an example of a Page Object class for the login page:

```csharp
using Microsoft.Playwright;

namespace PlaywrightSpecFlow.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public ILocator UsernameField => _page.Locator("#user-name");
        public ILocator PasswordField => _page.Locator("#password");
        public ILocator LoginButton => _page.Locator("#login-button");

        public async Task Login(string username, string password)
        {
            await UsernameField.FillAsync(username);
            await PasswordField.FillAsync(password);
            await LoginButton.ClickAsync();
        }
    }
}
```

## SpecFlow How-To

SpecFlow is used for Behavior-Driven Development (BDD). It allows you to define tests in a human-readable format using Gherkin syntax.

### Example

Here’s an example of a SpecFlow feature file and step definition:

#### Feature File: `Login.feature`

```gherkin
Feature: Login

  Scenario: Successful login with valid credentials
    Given I am on the login page
    When I login with valid credentials
    Then I should see the products page
```


```csharp
using Microsoft.Playwright;
using TechTalk.SpecFlow;
using PlaywrightSpecFlow.Pages;

namespace PlaywrightSpecFlow.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly IPage _page;
        private readonly LoginPage _loginPage;

        public LoginSteps(IPage page)
        {
            _page = page;
            _loginPage = new LoginPage(_page);
        }

        [Given(@"I am on the login page")]
        public async Task GivenIAmOnTheLoginPage()
        {
            await _page.GotoAsync("https://www.saucedemo.com/v1/index.html");
        }

        [When(@"I login with valid credentials")]
        public async Task WhenILoginWithValidCredentials()
        {
            await _loginPage.Login("standard_user", "secret_sauce");
        }

        [Then(@"I should see the products page")]
        public async Task ThenIShouldSeeTheProductsPage()
        {
            var productTitle = _page.Locator(".title");
            var isVisible = await productTitle.IsVisibleAsync();
            Assert.IsTrue(isVisible, "Products page is not visible.");
        }
    }
}
```

### Azure Pipelines

To set up Azure Pipelines, follow these steps:

1. Create a new pipeline in Azure DevOps and select your repository.
2. Use the following `azure-pipelines.yml` file for your pipeline configuration:

```yaml
trigger:
- none # change it to main/ branch name if needed

pool:
  vmImage: 'ubuntu-latest'

steps:
- task: UseDotNet@2
  inputs:
    packageType: 'sdk'
    version: '6.x'
    installationPath: $(Agent.ToolsDirectory)/dotnet

- script: |
    npm install -g npx
    npx playwright install
  displayName: 'Install Playwright'

- script: |
    dotnet restore
  displayName: 'Restore .NET dependencies'

- script: |
    dotnet build --configuration Release --no-restore
  displayName: 'Build solution'

- script: |
    dotnet test --configuration Release --no-build --logger "trx;LogFileName=test_results.trx"
  displayName: 'Run tests'

- task: PublishTestResults@2
  inputs:
    testRunner: 'VSTest'
    testResultsFiles: '**/*.trx'
    searchFolder: '$(System.DefaultWorkingDirectory)'
  displayName: 'Publish test results'


## GitHub Actions

To set up GitHub Actions, create a workflow file in `.github/workflows/ci.yml` in your repository with the following content:

```yaml
name: CI

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout repository
      uses: actions/checkout@v2

    - name: Setup .NET Core
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '6.x'  # Replace with your desired .NET Core version

    - name: Install Node.js and Playwright
      run: |
        curl -fsSL https://deb.nodesource.com/setup_16.x | sudo -E bash -
        sudo apt-get install -y nodejs
        npm install -g npx
        npx playwright install

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release --no-restore

    - name: Test
      run: dotnet test --configuration Release --no-build --verbosity normal --logger "trx;LogFileName=test_results.trx"

    - name: Publish Test Results
      uses: actions/upload-artifact@v2
      with:
        name: TestResults
        path: '**/TestResults/*.trx'