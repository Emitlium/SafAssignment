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

        // --- Constanty a IWebDriver ---
        protected IWebDriver prohlizec;
        protected const string url = "https://teams.microsoft.com/";
        protected const string jmeno = "dlp.automation3@safeticadlptesting.onmicrosoft.com";
        protected const string jmenoId = "i0116";
        protected const string prihlaseniTlacitkoDalsi = "idSIButton9";
        protected const string heslo = "Password.dlp";
        protected const string hesloId = "i0118";
        protected const string prihlaseniTlacitkoNe = "idBtn_Back";
        protected const string menuChatId = "app-bar-86fcd49b-61a2-4701-b771-54728cd291fb";
        protected const string automationChatTitle = "DLP Automation3";

        // --- Metody ---
        public IWebElement WaitForElementToBeVisible(By by, int vteriny = 30)
        {
            // vytvoří WebDriverWait, který čeká než se zobrazí element na stránce
            var waitVisible = new WebDriverWait(prohlizec, TimeSpan.FromSeconds(vteriny));     
            return waitVisible.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public void PrihlaseniDal(string prihlaseniTlacitkoDalsi)
        {
            IWebElement pokracuj = WaitForElementToBeVisible(By.Id(prihlaseniTlacitkoDalsi));
            pokracuj.Click();
        }

        // --- OneTime SetuP&TearDown - běží před testy a po testech ---
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

        // --- SetUp&TearDown - běží před a po každém testu ---
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
            prohlizec.Manage().Window.Maximize();
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
            PrihlaseniDal(prihlaseniTlacitkoDalsi);
        }

        [Test, Order(3)]
        public void T03_ZadatHeslo()
        {
            // pocka na pole hesla, vyplni, overi vyplneni
            IWebElement poleHeslo = WaitForElementToBeVisible(By.Id(hesloId));
            poleHeslo.SendKeys(heslo);
            Assert.AreEqual(heslo, poleHeslo.GetAttribute("value"));

            // pocka na zobrazeni tlacitka, klikne (posune nás k otravné tabulce)
            PrihlaseniDal(prihlaseniTlacitkoDalsi);
            PrihlaseniDal(prihlaseniTlacitkoNe);
        }

        [Test, Order(4)]
        public void T04_LocateAutomaTion()
        {
            IWebElement closeButton = WaitForElementToBeVisible(By.XPath("//button[@type='button' and @role='button' and @title='Zavřít']"));
            closeButton.Click();

            IWebElement menuChat = WaitForElementToBeVisible(By.Id(menuChatId));
            menuChat.Click();

            //switch na Frame 0, jinak to na te strance nenajde ten selector
            prohlizec.SwitchTo().Frame(0);

            IWebElement pripojSoubory = WaitForElementToBeVisible(By.CssSelector(".fui-Flex:nth-child(2) .ui-toolbar__item:nth-child(2) .ui-icon > .jr"));
            pripojSoubory.Click();

            IWebElement pripojSouboryDrive = WaitForElementToBeVisible(By.CssSelector(".ui-menu__itemwrapper:nth-child(1) .fui-StyledText"));
            pripojSouboryDrive.Click();

            // zábava...ted potrebuju vybrat soubory, ale xpath a selector zase nefunguje. neni to v default frame, v 0, a 1 mi hazi chybu ze neexistuje :)

            //prohlizec.SwitchTo().DefaultContent();
            //prohlizec.SwitchTo().Frame(1);
            //prohlizec.SwitchTo().Frame(prohlizec.FindElement(By.XPath("//iframe[@aria-label='Otevírání výběru souborů']")));
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
