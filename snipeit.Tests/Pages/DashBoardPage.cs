using Microsoft.Playwright;

namespace snipeit.Tests.Pages
{
    public class DashBoardPage
    {
        private readonly IPage _page;
        public DashBoardPage(IPage page)
        {
            _page = page;
        }

        // Locators: how to find each element on the dashboard screen
        private ILocator DashboardTitle => _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" });
        private ILocator CreateNewMenu => _page.Locator("a.dropdown-toggle", new() { HasText = "Create New" });
        private ILocator CreateAssetLink => _page.Locator("ul.dropdown-menu a[href$='/hardware/create']");

        public async Task WaitUntilLoadedAsync()
        {
            await Assertions.Expect(DashboardTitle).ToBeVisibleAsync();

        }
        public async Task OpenCreateAssetAsync()
        {
            await CreateNewMenu.ClickAsync();
            await CreateAssetLink.ClickAsync();
        }
    }
}

