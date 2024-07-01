using Microsoft.Playwright;

namespace Playwright_Specflow.src.Pages;
public class LoginPage 
{
    private readonly IPage _page;

    public LoginPage(Hooks.Hooks page){
        _page = page._page;
    }

    // Define locators as private variables
    private readonly string usernameInput = "#user-name";
    private readonly string passwordInput = "#password";
    private readonly string loginButton = "#login-button";
    private readonly string errorMessage = "[data-test=\"error\"]";
    private readonly string inventoryHeader = ".product_label"; // or whatever locator represents a successful login

    // Navigate to the login page
    public async Task NavigateAsync()
    {
        await _page.GotoAsync("https://www.saucedemo.com/v1/index.html");
    }

    // Interact with the login page
    public async Task LoginAsync(string username, string password)
    {
        await _page.FillAsync(usernameInput, username);
        await _page.FillAsync(passwordInput, password);
        await _page.ClickAsync(loginButton);
    }

    // Check for error message
    public async Task<string> GetErrorMessageAsync()
    {
        return await _page.TextContentAsync(errorMessage);
    }

    // Check for successful login
    public async Task<string> GetInventoryHeaderAsync()
    {
        return await _page.TextContentAsync(inventoryHeader);
    }
}
