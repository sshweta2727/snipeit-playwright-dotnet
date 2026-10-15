using Microsoft.Playwright.NUnit;
using snipeit.Tests.Config;
using snipeit.Tests.Pages;
using System.Text.RegularExpressions;
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
            await loginPage.LoginAsync(TestSettings.Username, TestSettings.Password);
            await Expect(Page).Not.ToHaveURLAsync(new Regex("/login"));


        }
    }
}