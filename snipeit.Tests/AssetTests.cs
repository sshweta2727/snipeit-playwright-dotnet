using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using snipeit.Tests.Pages;
using System.Text.RegularExpressions;

namespace snipeit.Tests
{
    public class AssetTests : PageTest
    {
        [Test]
        public async Task CreateAsset_CheckedOutToRandomUser()
        {
            var loginPage = new LoginPage(Page);
            var dashboardPage = new DashBoardPage(Page);
            var createAssetPage = new CreateAsset(Page);
            var assetsListPage = new AssetsListPage(Page);

            // Unique asset tag for this run, e.g. SK-20261015213045
            var assetTag = $"Sweta's Macbook Pro-{DateTime.Now:yyyyMMddHHmmss}";

            // Login to snipeit page first
            await loginPage.GotoAsync();
            await loginPage.LoginAsync("admin", "password");

            // Verify dashboard then open Create New > Asset
            await dashboardPage.WaitUntilLoadedAsync();
            await dashboardPage.OpenCreateAssetAsync();

            // Check: Create Asset form is open
            await Expect(Page).ToHaveURLAsync(new Regex("/hardware/create"));

            // Fill in the asset details
            await createAssetPage.SelectCompanyField("Beier LLC");
            await createAssetPage.EnterAssetTag(assetTag);
            await createAssetPage.SelectModel("Macbook Pro 13\"");
            await createAssetPage.SelectStatus("Ready to Deploy");

            // Check out to a random user, and remember who it was
            var selectedUser = await createAssetPage.SelectRandomUser();
            TestContext.Out.WriteLine($"Selected user: {selectedUser}");
            // Save the asset
            await createAssetPage.ClickSave();

            // Check: success message mentions our asset tag
            var successMessage = Page.Locator(".alert-success");
            await Expect(successMessage).ToContainTextAsync("created successfully");
            await Expect(successMessage).ToContainTextAsync(assetTag);

            // Find the asset in the assets list
            await assetsListPage.SearchAsync(assetTag);

            // Check: our asset appears in the results
            await Expect(Page.GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true })).ToBeVisibleAsync();

            // Open the asset page from the list
            await assetsListPage.OpenAssetAsync(assetTag);

            // Check: we're on the asset's own page, e.g. /hardware/2609
            await Expect(Page).ToHaveURLAsync(new Regex(@"/hardware/\d+"));

            // await Page.PauseAsync();
        }
    }
}