using snipeit.Tests.Config;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using snipeit.Tests.Pages;
using System.Text.RegularExpressions;

namespace snipeit.Tests
{
    public class AssetTests : PageTest
    {
        // "Anderson Gerson (username) - #12345"  ->  ["Anderson", "Gerson"]
        private static string[] GetNameParts(string userDropdownText)
        {
            var fullName = userDropdownText.Split(" (")[0];
            return fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        [Test]
        public async Task CreateAsset_CheckedOutToRandomUser()
        {
            var loginPage = new LoginPage(Page);
            var dashboardPage = new DashBoardPage(Page);
            var createAssetPage = new CreateAssetPage(Page);
            var assetsListPage = new AssetsListPage(Page);
            var assetDetailsPage = new AssetDetailsPage(Page);

            // Test data
            var assetTag = $"Sweta's Macbook Pro-{DateTime.Now:yyyyMMddHHmmss}";
            var company = "Bruen LLC";
            var model = "Macbook Pro 13\"";
            var status = "Ready to Deploy";

            // Login to snipeit page first
            await loginPage.GotoAsync();
            await loginPage.LoginAsync(TestSettings.Username, TestSettings.Password);

            // Verify dashboard then open Create New > Asset
            await dashboardPage.WaitUntilLoadedAsync();
            await dashboardPage.OpenCreateAssetAsync();

            // Check: Create Asset form is open
            await Expect(Page).ToHaveURLAsync(new Regex("/hardware/create"));

            // Fill in the asset details
            await createAssetPage.SelectCompanyField(company);
            await createAssetPage.EnterAssetTag(assetTag);
            await createAssetPage.SelectModel(model);
            await createAssetPage.SelectStatus(status);

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

            // Validate the asset details match what we entered
            await Expect(assetDetailsPage.PageHeading).ToContainTextAsync(assetTag);
            await Expect(assetDetailsPage.PageHeading).ToContainTextAsync(model);
            await Expect(assetDetailsPage.StatusBox).ToContainTextAsync(status);
            await Expect(assetDetailsPage.StatusBox).ToContainTextAsync("Deployed");
            await Expect(assetDetailsPage.CompanyLink(company)).ToBeVisibleAsync();

            // Validate it's checked out to the random user we picked
            foreach (var namePart in GetNameParts(selectedUser))
            {
                await Expect(assetDetailsPage.CheckedOutUser).ToContainTextAsync(namePart);
            }

            // Open the History tab, and check it's really showing
            await assetDetailsPage.OpenHistoryTab();
            await Expect(assetDetailsPage.HistoryRow("checkout")).ToBeVisibleAsync();

            // Check: "create new" entry for our asset
            var createRow = assetDetailsPage.HistoryRow("create new");
            await Expect(createRow).ToContainTextAsync(assetTag);
            await Expect(createRow).ToContainTextAsync("Admin");

            // Check: "checkout" entry for our asset, to our random user
            var checkoutRow = assetDetailsPage.HistoryRow("checkout");
            await Expect(checkoutRow).ToContainTextAsync(assetTag);
            foreach (var namePart in GetNameParts(selectedUser))
            {
                await Expect(checkoutRow).ToContainTextAsync(namePart);
            }

            // TEMP: pause to look at the History tab (remove before commit)
            // await Page.PauseAsync();
        }
    }
}