using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

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

        [SetUp] public void SetUp() { }

        [TearDown] public void TearDown() { }

    }



    public class TeamsChromeTest
    {
        /* Tady bude:
         * Vytvořte třídu TeamsChromeTest. Tato třída musí obsahovat:
         *      Test na odeslání souboru z OneDrive
         *      Test na napsání zprávy do chatu 
         */

        private const string url = "https://teams.microsoft.com/";
        private const string jmeno = "dlp.automation3@safeticadlptesting.onmicrosoft.com";
        private const string heslo = "Password.dlp";

        readonly IWebDriver prohlizec = new ChromeDriver();

        [Test, Order(1)]
        public void T01_NacistStranku()
        {
            prohlizec.Navigate().GoToUrl(url);
        }

        [Test, Order(2)]
        public void T02_ZadatJmeno()
        {
            IWebElement poleJmeno = prohlizec.FindElement(By.Id("i0116")); // id pole prihlasovaciho jmena
            poleJmeno.Click();
            poleJmeno.SendKeys(jmeno);
        }

        [Test, Order(3)]
        public void T03_PokracovaZJmena()
        {
            IWebElement dalsi = prohlizec.FindElement(By.Id("idSIButton9")); // id tlacitka dalsi
            dalsi.Click();
        }

        [Test, Order(4)]
        public void T04_ZadatHeslo()
        {
            IWebElement poleHeslo = prohlizec.FindElement(By.Id("i0118")); // id pole hesla
            poleHeslo.Click();
            poleHeslo.SendKeys(heslo);
        }

        [Test, Order(5)]
        public void T05_PokracovaZJmena()
        {
            IWebElement dalsi = prohlizec.FindElement(By.Id("idSIButton9")); // id tlacitka dalsi
            dalsi.Click();
        }


        [Test, Order(99)]
        public void T99_ZavriChrome()
        {
            prohlizec.Quit();
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
