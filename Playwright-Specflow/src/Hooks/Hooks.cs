using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Playwright_Specflow.src.Hooks
{
    [Binding]
    public class Hooks
    {
        public IBrowser _browser;
        public IBrowserContext _context;
        public IPage _page;

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { 
                Headless = false,
                Channel ="chrome" 
            
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            await _context.CloseAsync();
            await _browser.CloseAsync();
        }
    }
}