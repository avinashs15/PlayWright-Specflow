using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Playwright_Specflow.src.Pages;

public class CheckoutPage
{
    private readonly IPage _page;
    public CheckoutPage(Hooks.Hooks page)
    {
        _page = page._page;
    }

    // Define locators as private variables
    private readonly string firstNameInput = "#first-name";
    private readonly string lastNameInput = "#last-name";
    private readonly string zipCodeInput = "#postal-code";
    private readonly string continueButton = ".cart_button";
    private readonly string finishButton = "text='FINISH'";

    

    // Enter user details during checkout
    public async Task EnterUserDetailsAsync(string firstName, string lastName, string zipCode)
    {
        await _page.FillAsync(firstNameInput, firstName);
        await _page.FillAsync(lastNameInput, lastName);
        await _page.FillAsync(zipCodeInput, zipCode);
        await _page.ClickAsync(continueButton);
        await CompleteCheckoutAsync();
    }

    // Complete the checkout process
    public async Task CompleteCheckoutAsync()
    {
        await _page.ClickAsync(finishButton);
    }

    // Check if the checkout is complete
    public async Task<bool> IsCheckoutCompleteAsync()
    {
        // Implement logic to check if checkout is complete, e.g., by verifying success message or next page state
        // Example:
        var successMessage = await _page.TextContentAsync(".complete-header");
        return successMessage.Contains("THANK YOU");
    }
}
