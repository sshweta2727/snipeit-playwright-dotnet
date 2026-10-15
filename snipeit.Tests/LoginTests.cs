using System.Text.RegularExpressions;
using snipeit.Tests.Pages;
using Microsoft.Playwright.NUnit;
namespace snipeit.Tests
{
    public class LoginTests : PageTest
    {
        [Test]
        public async Task LoginPage_TestOpen()
        {
            var loginPage = new LoginPage(Page);

            await loginPage.GotoAsync();
            await Expect(Page).ToHaveTitleAsync(new Regex("Snipe-IT Asset Management"));
        }

        [Test]
        public async Task Login_withCredentials()
        {
            var loginPage = new LoginPage(Page);

            await loginPage.GotoAsync();
            await loginPage.LoginAsync("testuser", "password");
            await Expect(Page).Not.ToHaveURLAsync(new Regex("/login"));


        }
    }
}