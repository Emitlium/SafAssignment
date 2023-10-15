using OpenQA.Selenium;              // IWebDriver, IWebElement
using OpenQA.Selenium.Support.UI;   // WebDriverWait
using SeleniumExtras.WaitHelpers;   // ExpectedConditions

namespace SafAssignment
{
    public class Base
    {
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

        // InteractWithElement nahradí opakované zapisování vyhledání
        public void InteractWithVisibleElement(By element)
        {
            WaitForElementToBeVisible(element);
            /*  
             *  priklad uziti:
             *   id ->            InteractWithVisibleElement(By.Id("tady-bude-nejaky-string"));
             *   Xpath ->         InteractWithVisibleElement(ByXPath("tady-bude-nejaky-string"));
             *   CssSelector ->   InteractWithVisibleElement(By.CssSelector("tady-bude-nejaky-string"));
             *   ---> na její konec můžeme dodat .Click()
            */
        }

        public void PrihlaseniDal(string prihlaseniTlacitkoDalsi)
        {
            IWebElement pokracuj = WaitForElementToBeVisible(By.Id(prihlaseniTlacitkoDalsi));
            pokracuj.Click();
        }
    }
}
