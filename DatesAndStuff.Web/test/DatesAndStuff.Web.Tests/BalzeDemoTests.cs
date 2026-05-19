using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using FluentAssertions;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        // elinditjuk a bongeszot
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    [TearDown]
    public void Teardown()
    {
        // bezarjuk a bongeszot
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void CheckFlights_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // megnyitjuk az oldalt
        driver.Navigate().GoToUrl("https://blazedemo.com/");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        
        // beallitjuk az indulasi helyet Mexico Cityre
        var fromPortDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("fromPort")));
        var fromPortSelect = new SelectElement(fromPortDropdown);
        fromPortSelect.SelectByText("Mexico City");
        
        // beallitjuk az erkezesi helyet Dublinra
        var toPortDropdown = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("toPort")));
        var toPortSelect = new SelectElement(toPortDropdown);
        toPortSelect.SelectByText("Dublin");

        // megnyomjuk a kereses gombot
        var findFlightsButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
        findFlightsButton.Click();
        
        // megszamoljuk az eredmenyeket a kovi oldalon
        // megvarjuk hogy a tablazat toltodjon be
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table.table tbody")));

        // lekerjuk az osszes sort a tablazatbol
        var flightRows = driver.FindElements(By.CssSelector("table.table tbody tr"));

        // Assert
        // ellenorizzuk hogya sorok szama >= 4
        flightRows.Count.Should().BeGreaterThanOrEqualTo(3, "there should be 3 flights between Mexico City and Dublin");
    }
}