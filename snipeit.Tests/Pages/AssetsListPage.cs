using Microsoft.Playwright;

namespace snipeit.Tests.Pages
{
    public class AssetsListPage
    {
        private readonly IPage _page;

        public AssetsListPage(IPage page)
        {
            _page = page;
        }

        // Locators
        private ILocator SearchBox => _page.Locator("input.search-input");
        private ILocator AssetLink(string assetTag) => _page.GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true });

        // Actions
        public async Task SearchAsync(string text)
        {
            await SearchBox.FillAsync(text);
            await SearchBox.PressAsync("Enter");
        }

        public async Task OpenAssetAsync(string assetTag)
        {
            await AssetLink(assetTag).ClickAsync();
        }
    }
}