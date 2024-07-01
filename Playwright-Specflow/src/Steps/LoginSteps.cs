using TechTalk.SpecFlow;
using Microsoft.Playwright;
using System.Threading.Tasks;
using NUnit.Framework;
using Playwright_Specflow.src.Pages;

namespace Playwright_Specflow.src.Steps;

[Binding]
public class LoginSteps
{
    private readonly LoginPage _loginPage;

    public LoginSteps(LoginPage loginPage)
    {
        _loginPage = loginPage;
    }

    [Given(@"I am on the Sauce Labs login page")]
    public async Task GivenIAmOnTheSauceLabsLoginPage()
    {
        await _loginPage.NavigateAsync();
    }

    [When(@"I log in with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenILogInWithUsernameAndPassword(string username, string password)
    {
        await _loginPage.LoginAsync(username, password);
    }


    [Given(@"I am logged in with valid credentials")]
    public async Task GivenIAmLoggedInWithValidCredentials()
    {
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync("standard_user", "secret_sauce");
    }

    [Then(@"I should be logged in successfully")]
    public async Task ThenIShouldBeLoggedInSuccessfully()
    {
        var inventoryHeader = await _loginPage.GetInventoryHeaderAsync();
        Assert.IsNotNull(inventoryHeader, "The inventory header should not be null, indicating a successful login.");
    }

    [Then(@"I should see an error message ""(.*)""")]
    public async Task ThenIShouldSeeAnErrorMessage(string expectedErrorMessage)
    {
        var actualErrorMessage = await _loginPage.GetErrorMessageAsync();
        StringAssert.Contains(expectedErrorMessage, actualErrorMessage);
    }
}
