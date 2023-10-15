using NUnit.Framework;              // [SetUp, TearDown, ... atd]
using OpenQA.Selenium;              // IWebDriver, IWebElement
using OpenQA.Selenium.Chrome;       // ChromeDriver
using OpenQA.Selenium.Interactions; // Actions
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

        // *** Constanty a IWebDriver ***
        // Driver
        protected IWebDriver driver;

        // URL
        protected const string baseUrl = "https://teams.microsoft.com/";
        protected const string chatUrl = "https://teams.microsoft.com/_#/conversations/48:notes?ctx=chat";

        // Prihlaseni
        protected const string input_name = "dlp.automation3@safeticadlptesting.onmicrosoft.com";
        protected const string id_name = "i0116";
        protected const string id_loginNext = "idSIButton9";
        protected const string input_password = "Password.dlp";
        protected const string id_password = "i0118";
        protected const string id_loginNextNo = "idBtn_Back";

        // Menu -> Chat
        protected const string xPath_blockingPopup = "//button[@type='button' and @role='button' and @title='Zavřít']";
        protected const string id_menuChat = "app-bar-86fcd49b-61a2-4701-b771-54728cd291fb";

        // Pripoj soubory
        protected const string cssSelector_addFiles = ".fui-Flex:nth-child(2) .ui-toolbar__item:nth-child(2) .ui-icon > .jr";
        protected const string cssSelector_addFilesFromDrive = ".ui-menu__itemwrapper:nth-child(1) .fui-StyledText";

        // Soubory
        protected const string xPath_findExcelByText = "//*[contains(text(),'ExcelFile.xlsx')]";
        protected const string xPath_findPresentationByText = "//*[contains(text(),'Presentation.xlsx')]";
        protected const string xPath_buttonAddFileByText = "//*[contains(text(),'Připojit')]";

        // Text
        protected const string cssSelector_chatWindow = ".ck-placeholder";
        protected string chatText = "Test";

        // Chat -> Odesílání
        protected const string cssSelector_sendChatContents = ".ms-FocusZone:nth-child(3) .ui-icon > .jq";


        // *** Metody ***
        // WaitForElementToBeVisible čeká na zobrazení elementu na stránce (30 vteřin, jinak Timeout)
        public IWebElement WaitForElementToBeVisible(By by, int vteriny = 30)
        {
            var waitVisible = new WebDriverWait(driver, TimeSpan.FromSeconds(vteriny));     
            return waitVisible.Until(ExpectedConditions.ElementIsVisible(by));
        }

        //

        public void ElementAndClick(By element)
        {
            IWebElement passedElement = WaitForElementToBeVisible(element);
            passedElement.Click();
        }
        // priklad uziti:
        // id ->            ElementAndClick(By.Id("tady-bude-nejaky-string"));
        // Xpath ->         ElementAndClick(ByXPath("tady-bude-nejaky-string"));
        // CssSelector ->   ElementAndClick(By.CssSelector("tady-bude-nejaky-string"));

        public void PrihlaseniDal(string prihlaseniTlacitkoDalsi)
        {
            IWebElement pokracuj = WaitForElementToBeVisible(By.Id(prihlaseniTlacitkoDalsi));
            pokracuj.Click();
        }

        // --- OneTime SetuP&TearDown - běží před testy a po testech ---
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            driver = new ChromeDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            driver.Quit();
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
            /*default -> frame0 -> frame 0*/
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
