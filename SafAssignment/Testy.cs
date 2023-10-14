using NUnit.Framework;              // [SetUp, TearDown, ... atd]
using OpenQA.Selenium;              // IWebDriver, IWebElement
using OpenQA.Selenium.Chrome;       // ChromeDriver
using OpenQA.Selenium.Support.UI;   // WebDriverWait
using SeleniumExtras.WaitHelpers;   // ExpectedConditions

namespace SafAssignment
{
    public class CommonTest
    {
        /* Tady bude:
         * Vytvořte třídu CommonTest – base třída pro všechny testy. Součástí této třídy musí být:
         *      Metody [SetUp], [TearDown]
         *          Metoda [Setup] zaloguje test, který se spouští 
         *          Metoda [TearDown] zaloguje výsledek testu
         */

        // Constanty a IWebDriver
        protected IWebDriver prohlizec;
        protected const string url = "https://teams.microsoft.com/";
        protected const string jmeno = "dlp.automation3@safeticadlptesting.onmicrosoft.com";
        protected const string jmenoId = "i0116";
        protected const string prihlaseniTlacitkoDalsi = "idSIButton9";
        protected const string heslo = "Password.dlp";
        protected const string hesloId = "i0118";
        protected const string prihlaseniTlacitkoNe = "idBtn_Back";

        // Metody
        
        public IWebElement WaitForElementToBeVisible(By by, int vteriny = 30)
        {
            var waitVisible = new WebDriverWait(prohlizec, TimeSpan.FromSeconds(vteriny));     // vytvoří WebDriverWait, který čeká než se zobrazí element na stránce
            return waitVisible.Until(ExpectedConditions.ElementIsVisible(by));
        }

        // OneTime SetuP&TearDown - běží před testy a po testech
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            prohlizec = new ChromeDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            prohlizec.Quit();
        }

        // SetUp&TearDown - běží před a po každém testu
        /*
        [SetUp] public void SetUp()
        {
            // tady bude metoda která zaloguje test, který je spouštěn
        }

        [TearDown] public void TearDown()
        {
            // tady bude metoda, která zaloguje výsledek testu
        }
        */
    }

    public class TeamsChromeTest : CommonTest
    {
        /* Tady bude:
         * Vytvořte třídu TeamsChromeTest. Tato třída musí obsahovat:
         *      Test na odeslání souboru z OneDrive
         *      Test na napsání zprávy do chatu 
         */

        [Test, Order(1)]
        public void T01_NacistStranku()
        {
            prohlizec.Navigate().GoToUrl(url);
        }

        [Test, Order(2)]
        public void T02_ZadatJmeno()
        {
            // pocka na pole jmena, vyplni, overi vyplneni
            IWebElement poleJmeno = WaitForElementToBeVisible(By.Id(jmenoId));
            poleJmeno.SendKeys(jmeno);
            Assert.AreEqual(jmeno, poleJmeno.GetAttribute("value"));

            // pocka na zobrazeni tlacitka, klikne (posune nás k heslu)
            IWebElement prihlaseniDalsi = WaitForElementToBeVisible(By.Id(prihlaseniTlacitkoDalsi));
            prihlaseniDalsi.Click();
        }

        [Test, Order(3)]
        public void T04_ZadatHeslo()
        {
            // pocka na pole hesla, vyplni, overi vyplneni
            IWebElement poleHeslo = WaitForElementToBeVisible(By.Id(hesloId));
            poleHeslo.SendKeys(heslo);
            Assert.AreEqual(heslo, poleHeslo.GetAttribute("value"));

            // pocka na zobrazeni tlacitka, klikne (posune nás k otravné tabulce)
            IWebElement prihlaseniNe = WaitForElementToBeVisible(By.Id(prihlaseniTlacitkoNe));
            prihlaseniNe.Click();
        }
    }

    public class TeamsFirefoxTest
    {
        /* Tady bude:
         * Vytvořte třídu TeamsFirefoxTest. Tato třída musí obsahovat:
         *      Test na odeslání dvou souborů najednou z OneDrive
         *      Test na napsání tří zpráv do chatu
         */
    }
}
