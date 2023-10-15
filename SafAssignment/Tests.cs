using NUnit.Framework;              // [SetUp, TearDown, ... atd]
using OpenQA.Selenium;              // IWebDriver, IWebElement
using OpenQA.Selenium.Chrome;       // ChromeDriver
using OpenQA.Selenium.Interactions; // Actions

namespace SafAssignment
{
    public class CommonTest : Base
    {
        /* Tady bude:
         * Vytvořte třídu CommonTest – base třída pro všechny testy. Součástí této třídy musí být:
         *      Metody [SetUp], [TearDown]
         *          Metoda [Setup] zaloguje test, který se spouští 
         *          Metoda [TearDown] zaloguje výsledek testu
         */

        // --- OneTime SetuP&TearDown - běží před testy a po testech ---

        private int i = 1;
        public static List<string> logs = new List<string>();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            LoggingOfTests.WriteLogsToFile(logs, "PokusneLogy.txt");
            File.Exists("PokusneLogy.txt");
            driver.Quit();
        }

        // --- SetUp&TearDown - běží před a po každém testu ---

        [SetUp] public void SetUp()
        {
            // tady bude metoda která zaloguje test, který je spouštěn
            LoggingOfTests.LogStartOfTest(logs, i);
        }

        [TearDown] public void TearDown()
        {
            // tady bude metoda, která zaloguje výsledek testu
            LoggingOfTests.LogEndOfTest(logs, i);
            i++;
        }
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
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseUrl);
        }

        [Test, Order(2)]
        public void T02_ZadatJmeno()
        {
            // pocka na pole jmena, vyplni, overi vyplneni
            IWebElement poleJmeno = WaitForElementToBeVisible(By.Id(id_name));
            poleJmeno.SendKeys(input_name);
            Assert.AreEqual(input_name, poleJmeno.GetAttribute("value"));

            // pocka na zobrazeni tlacitka, klikne (posune nás k heslu)

            PrihlaseniDal(id_loginNext);
        }

        [Test, Order(3)]
        public void T03_ZadatHeslo()
        {
            // pocka na pole hesla, vyplni, overi vyplneni
            IWebElement poleHeslo = WaitForElementToBeVisible(By.Id(id_password));
            poleHeslo.SendKeys(input_password);
            Assert.AreEqual(input_password, poleHeslo.GetAttribute("value"));

            // pocka na zobrazeni tlacitka, klikne (posune nás k otravné tabulce)
            PrihlaseniDal(id_loginNext);
            PrihlaseniDal(id_loginNextNo);
        }

        [Test, Order(4)]
        public void T04_LocateAutomaTion()
        {
            IWebElement closeButton = WaitForElementToBeVisible(By.XPath(xPath_blockingPopup));
            closeButton.Click();

            IWebElement menuChat = WaitForElementToBeVisible(By.Id(id_menuChat));
            menuChat.Click();

            // overeni ze jsme na spravnem miste
            Thread.Sleep(4000);                                       // chvile na nacteni stranky
            string currentUrl = driver.Url;                           // opravdova url
            Assert.AreEqual(chatUrl, currentUrl);                     // overeni
        }

        [Test, Order(5)]
        public void T05_LocateDrive()
        {
            driver.SwitchTo().DefaultContent(); 
            driver.SwitchTo().Frame(0);

            IWebElement pripojSoubory = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFiles));
            pripojSoubory.Click();

            IWebElement pripojSouboryDrive = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFilesFromDrive));
            pripojSouboryDrive.Click();
        }
        
        [Test, Order(6)]
        public void T06_SelectExcelFile()
        {
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);          // 0 frame
            Thread.Sleep(2000);
            driver.SwitchTo().Frame(0);          // 0 frame nested v predchozim 0 frame + čekání

            IWebElement vyberExcel = WaitForElementToBeVisible(By.XPath(xPath_findExcelByText));
            vyberExcel.Click();
            IWebElement pripojit = WaitForElementToBeVisible(By.XPath(xPath_buttonAddFileByText));
            pripojit.Click();
        }

        [Test, Order(7)]
        public void T07_SendExcelFile()
        {
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);

            IWebElement odeslat = WaitForElementToBeVisible(By.CssSelector(cssSelector_sendChatContents));
            odeslat.Click();
            Thread.Sleep(3000);
         }

        [Test, Order(8)]
        public void T08_SelectFile()
        {
            // zbavit se otravneho popupu ktery zmizi jakmile clovek presune mysku na obrazovku
            Actions actions = new Actions(driver);
            IWebElement placeholderElement = driver.FindElement(By.TagName("body"));
            actions.MoveToElement(placeholderElement).Perform();

            IWebElement napisText = WaitForElementToBeVisible(By.CssSelector(cssSelector_chatWindow));
            napisText.SendKeys("Test!!");
            Thread.Sleep(1000);

            IWebElement odeslat = WaitForElementToBeVisible(By.CssSelector(cssSelector_sendChatContents));
            odeslat.Click();
            Thread.Sleep(1000);
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
