using NUnit.Framework;              // [SetUp, TearDown, ... atd]
using OpenQA.Selenium;              // IWebDriver, IWebElement
using OpenQA.Selenium.Chrome;       // ChromeDriver
using OpenQA.Selenium.Interactions; // Actions


namespace SafAssignment
{
    [TestFixture]
    public class TeamsChromeTest : CommonTest
    {
        [OneTimeSetUp]
        public void ChromeOneTimeSetUp()
        {
            currentBrowser = "Chrome";
            driver = new ChromeDriver();
        }

        [Test]
        public void Chrome_T01_NacistStranku()
        {
            // maximalizuje okno a otevře url
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseUrl);
            LoggingOfTests.DuringTesting(chromeLogs, " -> Zadali jsme URL pro Teams.");
        }

        [Test]
        public void Chrome_T02_ZadatJmeno()
        {
            // pocka na pole jmena, vyplni, overi vyplneni
            IWebElement poleJmeno = WaitForElementToBeVisible(By.Id(id_name));
            poleJmeno.SendKeys(input_name);
            Assert.AreEqual(input_name, poleJmeno.GetAttribute("value"));
            LoggingOfTests.DuringTesting(chromeLogs, " -> Zadali jsme jméno.");


            // pocka na zobrazeni tlacitka, klikne (posune nás k heslu)
            PrihlaseniDal(id_loginNext);
            LoggingOfTests.DuringTesting(chromeLogs, " -> Posouváme se dál, budeme zadávat heslo.");
        }

        [Test]
        public void Chrome_T03_ZadatHeslo()
        {
            // pocka na pole hesla, vyplni, overi vyplneni
            IWebElement poleHeslo = WaitForElementToBeVisible(By.Id(id_password));
            poleHeslo.SendKeys(input_password);
            Assert.AreEqual(input_password, poleHeslo.GetAttribute("value"));
            LoggingOfTests.DuringTesting(chromeLogs, " -> Zadali jsme heslo.");

            // pocka na zobrazeni tlacitka, klikne (posune nás k otravné tabulce)
            PrihlaseniDal(id_loginNext);
            PrihlaseniDal(id_loginNextNo);
            LoggingOfTests.DuringTesting(chromeLogs, " -> Posouváme se dál, budeme přesměrování do Teams.");
        }

        [Test]
        public void Chrome_T04_LocateAutomaTion()
        {
            // vypne popup
            IWebElement closeButton = WaitForElementToBeVisible(By.XPath(xPath_blockingPopup));
            closeButton.Click();
            LoggingOfTests.DuringTesting(chromeLogs, " -> Vypnuto vyskakovací okno v pravém dolním rohu.");


            // najde a klikne na chat
            IWebElement menuChat = WaitForElementToBeVisible(By.Id(id_menuChat));
            menuChat.Click();
            LoggingOfTests.DuringTesting(chromeLogs, " -> Jsme v Chatu.");


            // overeni ze jsme na spravnem miste
            Thread.Sleep(4000);                                       // chvile na nacteni stranky
            string currentUrl = driver.Url;                           // opravdova url
            Assert.AreEqual(chatUrl, currentUrl);                     // overeni
            LoggingOfTests.DuringTesting(chromeLogs, " -> Máme očekávané URL chatu.");
        }

        [Test]
        public void Chrome_T05_LocateDrive()
        {
            // frames default -> prvni iframe
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);

            // najde a klikne na pridavani souboru
            IWebElement addFiles = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFiles));
            addFiles.Click();
            LoggingOfTests.DuringTesting(chromeLogs, " -> Klikli jsme na přidávání souboru.");


            // ze zobrazeneho vyberu dvou najde a klikne na pridavani z Drive
            IWebElement addFilesFromDrive = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFilesFromDrive));
            addFilesFromDrive.Click();
            LoggingOfTests.DuringTesting(chromeLogs, " -> V přidávání souboru jsme zvolili Drive.");
        }

        [Test]
        public void Chrome_T06_SelectExcelFile()
        {
            LoggingOfTests.DuringTesting(chromeLogs, " -> Pokud je tohle poslední záznam, spadlo to na frames");

            // frames default -> prvni iframe -> prvni vnoreny iframe
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);          // 0 frame
            Thread.Sleep(2000);                  // chvili cekame, jinak to tady spadne
            driver.SwitchTo().Frame(0);          // 0 frame nested v predchozim 0 frame + čekání

            // najde a klikne na excel
            IWebElement selectExcel = WaitForElementToBeVisible(By.XPath(xPath_findExcelByText));
            selectExcel.Click();
            LoggingOfTests.DuringTesting(chromeLogs, " -> Vybrali jsme Excel.");


            // najde a klikne na přidat
            IWebElement clickOn_Pridat = WaitForElementToBeVisible(By.XPath(xPath_buttonAddFileByText));
            clickOn_Pridat.Click();
            LoggingOfTests.DuringTesting(chromeLogs, " -> Přidali jsme Excel.");
        }

        [Test]
        public void Chrome_T07_SendExcelFile()
        {
            LoggingOfTests.DuringTesting(chromeLogs, " -> Pokud je tohle poslední záznam, spadlo to na frames x2");
            // frames
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);                 // chvili cekame

            // odešleme Excel
            IWebElement clickOnSendChat = WaitForElementToBeVisible(By.CssSelector(cssSelector_sendChatContents));
            clickOnSendChat.Click();
            Thread.Sleep(3000);                 // chvili cekame
            LoggingOfTests.DuringTesting(chromeLogs, " -> Odeslali jsme Excel do chatu.");
        }

        [Test]
        public void Chrome_T08_WriteText()
        {
            // zbavit se otravneho popupu ktery zmizi jakmile clovek presune mysku na obrazovku
            Thread.Sleep(1000);
            Actions actions = new Actions(driver);
            IWebElement placeholderElement = driver.FindElement(By.TagName("body"));
            actions.MoveToElement(placeholderElement).Perform();

            // napíšeme do chatu text
            IWebElement napisText = WaitForElementToBeVisible(By.CssSelector(cssSelector_chatWindow));
            napisText.SendKeys("Test!!");
            Thread.Sleep(2000);
            LoggingOfTests.DuringTesting(chromeLogs, " -> Napsali jsme text v chatovém okně.");

            // odešleme text v chatu
            IWebElement odeslat = WaitForElementToBeVisible(By.CssSelector(cssSelector_sendChatContents));
            odeslat.Click();
            Thread.Sleep(1000);
            LoggingOfTests.DuringTesting(chromeLogs, " -> Odeslali jsme text do chatu.");
        }

        [OneTimeTearDown]
        public void ChromeOneTimeTearDown()
        {
            LoggingOfTests.WriteLogsToFile(chromeLogs, "ChromeLogs.txt");
            driver.Quit();
        }
    }
}
