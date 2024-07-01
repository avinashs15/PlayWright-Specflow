using Microsoft.Playwright;

namespace Playwright_Specflow.src.Pages;
public class ShoppingCartPage
{
    private readonly IPage _page;
    public ShoppingCartPage(Hooks.Hooks page)
    {
        _page = page._page;
    }
    
    private readonly string _cartButtonSelector = "#shopping_cart_container";
    private readonly string _checkoutButtonSelector = "[class=\"btn_action checkout_button\"]";


    public async Task AddProductToCartAsync(string productName)
    {
        var productButtonSelector = $"//div[text()='{productName}']/../../following-sibling::div/button[contains(@class, 'btn_inventory')]";
        await _page.ClickAsync(productButtonSelector);
    }

    public async Task ProceedToCheckoutAsync()
    {
        await _page.ClickAsync(_cartButtonSelector);
        await _page.ClickAsync(_checkoutButtonSelector);
    }
}
