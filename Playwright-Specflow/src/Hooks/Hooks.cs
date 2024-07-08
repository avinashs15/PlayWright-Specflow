using Microsoft.Playwright;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(5)]

namespace Playwright_Specflow.src.Hooks
{
    [Binding]
    public class Hooks
    {
        public IBrowser? _browser;
        public IBrowserContext? _context;
        public IPage? _page;

        [BeforeTestRun]
        public static  void BeforeTestRun()
        {
            Console.WriteLine("BeforeTestRun");
        }


        [BeforeScenario]
        public async Task BeforeScenario()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                Channel = "chrome"

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