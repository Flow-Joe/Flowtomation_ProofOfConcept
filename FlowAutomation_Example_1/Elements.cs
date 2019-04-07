using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowAutomation_Example_1
{
    public static class Elements
    {
        /* All elements are placed here using selectors as an example, when creating your automation framework I strongly suggest looking into a POM approach.
           If you're trying to run this without understanding automation, you can easily get an element selector by right clicking the element and selecting 'Inspect Element'.
           Once the browser DevTools has loaded you can right click the highlighted element within the DevTools, click 'Copy' and select 'Copy Selector'.
           These are based on the UnitedKingdom Flow Website and may differ for each region - This has NOT been checked */

        //Sign In Elements
        public static string SignIn = "#header-signin-link";
        public static string SignIn_UserName = "#i0116";
        public static string SignIn_Password = "#i0118";
        public static string SignIn_ConfirmButton = "#idSIButton9";
        public static string SignIn_IgnoreStaySignedInButton = "#idBtn_Back";

        //Flow HomePage
        public static string WorkLessDoMore_Title = "#content-container > main > section.context-homepage-hero.b-lazy.b-error > div:nth-child(1) > div > div > h1";
        public static string SideBar = "#react-flow-appsidebar";
        public static string SideBarButtons = "ms-Button";

        //New Flow Page
        public static string CreateAFlowFromBlank_Title = "#content-container > section > landing > div > flow-new-getting-started-hero-section > div > div > div.column.medium-offset-2.medium-10.large-offset-1.large-4 > h1";
    }
}
