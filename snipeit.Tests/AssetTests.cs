using System.Text.RegularExpressions;
using snipeit.Tests.Pages;
using Microsoft.Playwright.NUnit;
namespace snipeit.Tests
{
    public class AssetTests : PageTest
    {
        [Test]
        public async Task CreateAsset_CheckedOutToRandomUser()
        {
            var loginPage = new LoginPage(Page);
            var dashboardPage = new DashBoardPage(Page);

       // Login to snipeit page first
            await loginPage.GotoAsync();
            await loginPage.LoginAsync("admin", "password");

       // verify dashboard then create asset

            await dashboardPage.WaitUntilLoadedAsync();
            await dashboardPage.OpenCreateAssetAsync();

            // Check: Create Asset form is open

            await Expect(Page).ToHaveURLAsync(new Regex("/hardware/create"));


        }
    }
}
