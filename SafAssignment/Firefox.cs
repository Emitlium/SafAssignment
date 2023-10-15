using NUnit.Framework;              // [SetUp, TearDown, ... atd]
using OpenQA.Selenium;              // IWebDriver, IWebElement
using OpenQA.Selenium.Firefox;      // FirefoxDriver
using OpenQA.Selenium.Interactions; // Actions

namespace SafAssignment
{
    [TestFixture]
    public class TeamsFirefoxTest : CommonTest
    {
        [OneTimeSetUp]
        public void FirefoxOneTimeSetUp()
        {
            currentBrowser = "Firefox";
            driver = new FirefoxDriver();
        }

        [OneTimeTearDown]
        public void FireFoxOneTimeTearDown()
        {
            LoggingOfTests.WriteLogsToFile(firefoxLogs, "FirefoxLogs.txt");
            driver.Quit();
        }

        [Test]
        public void Firefox_T01_NacistStranku()
        {
            // maximalizuje okno a otevře url
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(baseUrl);
            LoggingOfTests.DuringTesting(chromeLogs, " -> Zadali jsme URL pro Teams.");
        }

        [Test]
        public void Firefox_T02_ZadatJmeno()
        {
            // pocka na pole jmena, vyplni, overi vyplneni
            IWebElement poleJmeno = WaitForElementToBeVisible(By.Id(id_name));
            poleJmeno.SendKeys(input_name);
            Assert.AreEqual(input_name, poleJmeno.GetAttribute("value"));
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Zadali jsme jméno.");


            // pocka na zobrazeni tlacitka, klikne (posune nás k heslu)
            PrihlaseniDal(id_loginNext);
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Posouváme se dál, budeme zadávat heslo.");

        }

        [Test]
        public void Firefox_T03_ZadatHeslo()
        {
            // pocka na pole hesla, vyplni, overi vyplneni
            IWebElement poleHeslo = WaitForElementToBeVisible(By.Id(id_password));
            poleHeslo.SendKeys(input_password);
            Assert.AreEqual(input_password, poleHeslo.GetAttribute("value"));
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Zadali jsme heslo.");

            // pocka na zobrazeni tlacitka, klikne (posune nás k otravné tabulce)
            PrihlaseniDal(id_loginNext);
            PrihlaseniDal(id_loginNextNo);
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Posouváme se dál, budeme přesměrování do Teams.");
        }

        [Test]
        public void Firefox_T04_LocateAutomaTion()
        {
            // vypne popup
            IWebElement closeButton = WaitForElementToBeVisible(By.XPath(xPath_blockingPopup));
            closeButton.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Vypnuto vyskakovací okno v pravém dolním rohu.");


            // najde a klikne na chat
            IWebElement menuChat = WaitForElementToBeVisible(By.Id(id_menuChat));
            menuChat.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Jsme v Chatu.");


            // overeni ze jsme na spravnem miste
            Thread.Sleep(4000);                                       // chvile na nacteni stranky
            string currentUrl = driver.Url;                           // opravdova url
            Assert.AreEqual(chatUrl, currentUrl);                     // overeni
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Máme očekávané URL chatu.");
            Thread.Sleep(2000);
        }

        [Test]
        public void Firefox_T05_LocateDrive()
        {
            // frames default -> prvni iframe
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(5000);

            // najde a klikne na pridavani souboru
            IWebElement addFiles = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFiles));
            addFiles.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Klikli jsme na přidávání souboru.");


            // ze zobrazeneho vyberu dvou najde a klikne na pridavani z Drive
            IWebElement addFilesFromDrive = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFilesFromDrive));
            addFilesFromDrive.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> V přidávání souboru jsme zvolili Drive.");
        }

        [Test]
        public void Firefox_T06_SelectFiles()
        {
            // frames default -> prvni iframe -> prvni vnoreny iframe
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);          // 0 frame
            Thread.Sleep(2000);                  // chvili cekame, jinak to tady spadne
            driver.SwitchTo().Frame(0);          // 0 frame nested v predchozim 0 frame + čekání

            // najde a klikne na excel
            IWebElement selectExcel = WaitForElementToBeVisible(By.XPath(xPath_findExcelByText));
            selectExcel.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Vybrali jsme Excel.");

            IWebElement clickOn_Pridat = WaitForElementToBeVisible(By.XPath(xPath_buttonAddFileByText));
            clickOn_Pridat.Click();

            Thread.Sleep(3000);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);          // 0 frame
            Thread.Sleep(2000);                  // chvili cekame, jinak to tady spadne
            IWebElement addFiles = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFiles));
            addFiles.Click();
            IWebElement addFilesFromDrive = WaitForElementToBeVisible(By.CssSelector(cssSelector_addFilesFromDrive));
            addFilesFromDrive.Click();

            // najde a klikne na přidat
            Thread.Sleep(3000);
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);          // 0 frame
            Thread.Sleep(2000);                  // chvili cekame, jinak to tady spadne
            driver.SwitchTo().Frame(0);
            IWebElement selectPresent = WaitForElementToBeVisible(By.XPath(xPath_findPresentationByText));
            selectPresent.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Vybrali jsme Prezentaci.");

            // klikne na přidat
            clickOn_Pridat.Click();
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Přidali jsme Excel a Prezentaci.");
        }

        [Test]
        public void Firefox_T07_SendFiles()
        {
            // frames
            driver.SwitchTo().DefaultContent();
            driver.SwitchTo().Frame(0);
            Thread.Sleep(1000);                 // chvili cekame

            // odešleme Excel
            IWebElement clickOnSendChat = WaitForElementToBeVisible(By.CssSelector(cssSelector_sendChatContents));
            clickOnSendChat.Click();
            Thread.Sleep(3000);                 // chvili cekame
            LoggingOfTests.DuringTesting(firefoxLogs, " -> Odeslali jsme Excel do chatu.");
        }

        [Test]
        public void WriteTextThrice()
        {
            // zbavit se otravneho popupu ktery zmizi jakmile clovek presune mysku na obrazovku
            Thread.Sleep(1000);
            Actions actions = new Actions(driver);
            IWebElement placeholderElement = driver.FindElement(By.TagName("body"));
            actions.MoveToElement(placeholderElement).Perform();

            // napíšeme do chatu text
            IWebElement napisText = WaitForElementToBeVisible(By.CssSelector(cssSelector_chatWindow));
            for (int counter = 1; counter < 4; counter++)
            {
                napisText.SendKeys($"Firefox Test #{counter}!!");
                Thread.Sleep(1000);
                LoggingOfTests.DuringTesting(firefoxLogs, " -> Napsali jsme text v chatovém okně.");

                // odešleme text v chatu
                IWebElement odeslat = WaitForElementToBeVisible(By.CssSelector(cssSelector_sendChatContents));
                odeslat.Click();
                Thread.Sleep(1000);
                LoggingOfTests.DuringTesting(firefoxLogs, " -> Odeslali jsme text do chatu.");
            }
        }
    }
}
