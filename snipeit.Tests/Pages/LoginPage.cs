using Microsoft.Playwright;
using snipeit.Tests.Config;

namespace snipeit.Tests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        public LoginPage(IPage page)
        {
            _page = page;
        }

        // Locators: how to find each element on the login screen
        private ILocator UsernameInput => _page.GetByPlaceholder("Username");
        private ILocator PasswordInput => _page.Locator("#password-field");
        private ILocator RememberMeCheckbox => _page.Locator("#remember");
        private ILocator LoginButton => _page.Locator("button[type='submit']");

        public async Task GotoAsync()
        {
            await _page.GotoAsync(TestSettings.BaseUrl);

        }

        public async Task LoginAsync(string username, string password)
        {
            await UsernameInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await RememberMeCheckbox.ClickAsync();
            await LoginButton.ClickAsync();
        }
        }
}

