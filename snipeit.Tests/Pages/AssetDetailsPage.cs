using Microsoft.Playwright;

namespace snipeit.Tests.Pages
{
    public class AssetDetailsPage
    {
        private readonly IPage _page;

        public AssetDetailsPage(IPage page)
        {
            _page = page;
        }

        // Elements the test will verify
        public ILocator PageHeading => _page.Locator("li.breadcrumb-item.active");
        public ILocator StatusBox => _page.Locator("div.well").First;
        public ILocator CheckedOutUser => StatusBox.Locator("a[href*='/users/']");
        public ILocator CompanyLink(string companyName) => _page.GetByRole(AriaRole.Link, new() { Name = companyName, Exact = true });
        private ILocator HistoryTab => _page.Locator("a[href$='#history']");
        public ILocator HistoryRow(string action) => _page.Locator("#history tbody tr", new() { HasText = action });

        // Actions
        public async Task OpenHistoryTab()
        {
            await HistoryTab.ClickAsync();
        }
    }
}