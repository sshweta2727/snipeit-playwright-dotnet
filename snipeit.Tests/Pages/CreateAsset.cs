using Microsoft.Playwright;

namespace snipeit.Tests.Pages
{
    public class CreateAsset
    {
        private readonly IPage _page;
        public CreateAsset(IPage page)
        {
            _page = page;
        }

        // Locators: how to find each element on the Create New Asset screen
        private ILocator Company => _page.Locator("#select2-company_id_select-container");
        private ILocator AssetTag => _page.Locator("input[name= 'asset_tags[1]']");
        private ILocator Serial => _page.Locator("#serial_1");
        private ILocator Model => _page.Locator("#select2-model_select_id-container");
        private ILocator Status=> _page.Locator("#select2-status_select_id-container");
        private ILocator DropdownSearchBox => _page.Locator(".select2-container--open .select2-search__field");
        private ILocator DropdownOption(string text) => _page.Locator(".select2-results__option", new() { HasText = text });
        private ILocator User => _page.Locator("#select2-assigned_user_select-container");
        private ILocator UserOptions => _page.Locator(".select2-container--open .select2-results__option:not(.loading-results):not(.select2-results__message)");
        private ILocator SaveButton => _page.Locator("#submit_button");

        // Actions

        public async Task SelectCompanyField(string companyName)
        {
            await SelectFromDropdown(Company, companyName);
        }
        public async Task EnterAssetTag(string assetTag)
        {
            await AssetTag.FillAsync(assetTag);
        }
        public async Task SelectModel(string modelName)
        {
            await SelectFromDropdown(Model, modelName);
        }

        public async Task SelectStatus(string statusName)
        {
            await SelectFromDropdown(Status, statusName);
        }

        // Helper: works for any Select2 dropdown on this page
        private async Task SelectFromDropdown(ILocator dropdown, string text)
        {
            await dropdown.ClickAsync();
            await DropdownSearchBox.FillAsync(text);
            await DropdownOption(text).First.ClickAsync();
        }
        public async Task<string> SelectRandomUser()
        {
            await User.ClickAsync();
            await UserOptions.First.WaitForAsync();

            var count = await UserOptions.CountAsync();
            var randomIndex = Random.Shared.Next(count);
            var randomUser = UserOptions.Nth(randomIndex);

            var userName = (await randomUser.InnerTextAsync()).Trim();
            await randomUser.ClickAsync();

            return userName;
        }
        public async Task ClickSave()
        {
            await SaveButton.ClickAsync();
        }

    }
}

