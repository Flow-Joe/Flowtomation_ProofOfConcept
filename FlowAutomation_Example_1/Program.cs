using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Script.Serialization;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FlowAutomation_Example_1
{
    // Follow FlowJoe and his journey via the links below
    // Blog: https://www.flowjoe.io/
    // Twitter: https://www.twitter.com/flow_joe_
    // Youtube: https://www.youtube.com/channel/UCv4gsiyRjfB9NlvPPBd2a3g
    // This is a proof-of-concept for passing a Selenium based test result via a REST Post call throw a Microsoft Flow to provide a notification on the Flow Mobile Application
    class Program
    {
        private static IWebDriver _driver;
        private static WebDriverWait _wait;
        private static string userName = "YOURFLOWUSERNAME"; // Your Flow Username
        private static string password = "YOURFLOWPASSWORD"; // Your Flow Password

        static void Main(string[] args)
        {
            CreateDriver(); // This method creates a Chrome driver
            ConnectToMicrosoftFlow(userName, password); // This method launches the Flow website, signs in and verifies you're on the Flow homepage

            RunCreateNewBlankFlowTest(); // Test 1 clicks 'My Flows', Selects the New button, Selects 'Create from blank' then verifies the 'Create from blank' button appears on the new-flow landing page

            PostResultsToFlow(1,0); // As this is a proof of concept, providing the code reaches here I will pass in '1' for a test passing.
        }

        private static void RunCreateNewBlankFlowTest()
        {
            SelectMenuButton(Buttons.MyFlows);

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name(Buttons.New))); // Wait for New button to exist
            _driver.FindElement(By.Name(Buttons.New)).Click(); // Click New Button

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Name(Buttons.CreateFromBlank))); // Wait for the Create from Blank button to exist
            _driver.FindElement(By.Name(Buttons.CreateFromBlank)).Click(); // Click Create from Blank button

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(Elements.CreateAFlowFromBlank_Title))); // Waits for title to exist
            Assert.IsTrue(_driver.FindElement(By.CssSelector(Elements.CreateAFlowFromBlank_Title)).Text.Equals("Create a flow from blank")); // Verifies you're on the New Flow page
        }

        private static void PostResultsToFlow(int testsPassed, int testsFailed)
        {
            // This method posts the results being sent through as params to the url provided when creating a flow receiver

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("YOUR_FLOW_POST_URL");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = new JavaScriptSerializer().Serialize(new
                {
                    TestsPassed = testsPassed,
                    TestsFailed = testsFailed,
                    TotalTests = testsPassed+testsFailed
                });

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }

        private static void SelectMenuButton(string menuButton)
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(Elements.SideBar))); // We wait for the side bar to exist
            var sideBar = _driver.FindElement(By.CssSelector(Elements.SideBar)); // We find the sidebar element
            var menuItems = sideBar.FindElements(By.ClassName(Elements.SideBarButtons)); // We get a list of all buttons (We do this as Microsoft Dynamically names these)

            foreach (var menuItem in menuItems) // We cycle through each menu item
            {
                if (!menuItem.Text.Contains(menuButton)) continue; // If the menu item contains the button we want we proceed
                menuItem.Click(); // We click the menu button we want
                break;

            }
        }

        private static void CreateDriver()
        {
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)); // This launches Chrome from the execution directory nuget package
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20)); // This allows us to wait for elements to appear for a maximum of 20 seconds

            _driver.Manage().Window.Maximize(); // This maximizes the Chrome window
        }

        private static void ConnectToMicrosoftFlow(string userName, string password)
        {
            _driver.Navigate().GoToUrl("https://flow.microsoft.com/en-us/");

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(Elements.SignIn))); // This waits until our expected outcome has occured.
            _driver.FindElement(By.CssSelector(Elements.SignIn)).Click(); // We click the SignIn button

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(Elements.SignIn_UserName)));
            _driver.FindElement(By.CssSelector(Elements.SignIn_UserName)).SendKeys(userName); // This sends your username to the username field on the Microsoft Online Sign-In Website
            _driver.FindElement(By.CssSelector(Elements.SignIn_ConfirmButton)).Click(); // This clicks the next button

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector(Elements.SignIn_Password)));
            _driver.FindElement(By.CssSelector(Elements.SignIn_Password)).SendKeys(password); // This sends your password to the password field on the Microsoft Online Sign-In Website
            _driver.FindElement(By.CssSelector(Elements.SignIn_ConfirmButton)).Click(); // This clicks the signin button

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(Elements.SignIn_IgnoreStaySignedInButton)));
            _driver.FindElement(By.CssSelector(Elements.SignIn_IgnoreStaySignedInButton)).Click(); // Clicks 'No' when asked if you want to remain signed in

            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector(Elements.WorkLessDoMore_Title)));
            Assert.IsTrue(_driver.FindElement(By.CssSelector(Elements.WorkLessDoMore_Title)).Text.Equals("Work less, do more.")); // Verifies you're on the Flow homepage by checking the WLDM text
        }
    }
}
