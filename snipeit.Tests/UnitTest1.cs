using System.Text.RegularExpressions;
using Microsoft.Playwright.NUnit;
namespace snipeit.Tests
{
    public class LoginTests : PageTest
    {
        [Test]
        public async Task LoginPage_TestOpen()
        {
            await Page.GotoAsync("https://demo.snipeitapp.com/");
            await Expect(Page).ToHaveTitleAsync(new Regex("Snipe-IT Asset Management"));
        }

        [Test]
        public async Task Login_withCredentials()
        {
            await Page.GotoAsync("https://demo.snipeitapp.com/");
            await Page.GetByPlaceholder("Username").FillAsync("testuser");
            await Page.Locator("#password-field").FillAsync("password");
            await Page.Locator("#remember").CheckAsync();
            await Expect(Page.Locator("#remember")).ToBeCheckedAsync();
            await Page.Locator("button[type='submit']").ClickAsync();
            await Expect(Page).Not.ToHaveURLAsync(new Regex("/login"));


        }
    }
}