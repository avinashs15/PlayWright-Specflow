using TechTalk.SpecFlow;
using Microsoft.Playwright;
using System.Threading.Tasks;
using NUnit.Framework;
using Playwright_Specflow.src.Pages;

namespace Playwright_Specflow.src.Steps;

[Binding]
public class CheckOutSteps
{
    private readonly LoginPage _loginPage;
    private readonly ShoppingCartPage _cartPage;
    private readonly CheckoutPage _checkoutPage;

    public CheckOutSteps(ShoppingCartPage shoppingCartPage, CheckoutPage checkoutPage, LoginPage loginPage)
    {
        _cartPage = shoppingCartPage;
        _checkoutPage = checkoutPage;
        _loginPage = loginPage;
    }


    [When(@"I add (.*) to the cart from inventory page")]
    public async Task WhenIAddProductToTheCart(string product)
    {
        await _cartPage.AddProductToCartAsync(product);
    }

    [When(@"I proceed to checkout")]
    public async Task WhenIProceedToCheckout()
    {
        await _cartPage.ProceedToCheckoutAsync();
    }

    [When(@"I enter user details:")]
    public async Task WhenIEnterUserDetails(Table table)
    {
        var userDetails = table.Rows[0];
        var firstName = userDetails["First Name"];
        var lastName = userDetails["Last Name"];
        var zipCode = userDetails["Zip Code"];
        await _checkoutPage.EnterUserDetailsAsync(firstName, lastName, zipCode);
    }

    [Then(@"I should complete the checkout successfully")]
    public async Task ThenIShouldCompleteTheCheckoutSuccessfully()
    {
        Assert.IsTrue(await _checkoutPage.IsCheckoutCompleteAsync(), "Checkout should be completed successfully.");
    }
}
