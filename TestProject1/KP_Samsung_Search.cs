using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;


namespace UITest
{
    class Tests
    {
        String test_url = "https://www.kupujemprodajem.com/";

        public IWebDriver driver;
        

        [SetUp]
        public void start_Browser()
        {
            //Initialize Chrome 
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void test_Search()
        {
            //Set url
            driver.Url = test_url;

            //Close pop-up window
            IWebElement closeWindow = driver.FindElement(By.XPath("//div[@class= 'kpBoxCloseButton' ]"));
            closeWindow.Click();

            // Enter term for search
            IWebElement searchText = driver.FindElement(By.CssSelector("[name = 'data[keywords]']"));
            searchText.SendKeys("Samsung");

            // Wait for suggestion list to be visible and select category 'Mobilni telefoni > Samsung'
            WebDriverWait wait = new(driver, TimeSpan.FromSeconds(3));
            IWebElement suggestionsList = wait.Until(e => e.FindElement(By.XPath("//*[@id='autocompleteHolder']/div/div/ul/li[2]")));
            searchText.SendKeys(Keys.ArrowDown);
            searchText.SendKeys(Keys.ArrowDown);
            searchText.SendKeys(Keys.Enter);

            // Wait for results to load and select sorting criteria to be visible; select popularity descending
            IWebElement resultsLoad = wait.Until(e => e.FindElement(By.XPath("//*[@id='middleCol']/div[4]/ul/li[2]")));
            IWebElement sortingCriteria = wait.Until(e => e.FindElement(By.XPath("//*[@id='orderSecondSelection']/div/div[1]/div[1]/span[2]")));
            sortingCriteria.Click();
            IWebElement selectSorting = driver.FindElement(By.CssSelector("div[data-value = 'view_count desc']"));
            selectSorting.Click();
            IWebElement waitForSearch = wait.Until(e => e.FindElement(By.XPath("//*[@id= 'orderSecondSelection' ]/div/div[1]/div[3]/span")));
            IWebElement confirmSortingSelection = driver.FindElement(By.CssSelector("input[class = 'searchButton secondarySearchButton']"));
            confirmSortingSelection.Click();


            // Fetch a list of views for all elements and add them to a list
            IList<IWebElement> viewsCount = driver.FindElements(By.ClassName("view-count"));
            List<string> views = new List<string>();

            foreach (IWebElement element in viewsCount)
            {
                views.Add(element.Text);
            }

            // Add all elements to a list as ints
            List<int> viewsInt = views.Select(x => int.Parse(x)).ToList();

            // Sort elements of list 
            List<int> viewsSorted = viewsInt.OrderByDescending(x => x).ToList();
            int len = viewsSorted.Count;
            Console.WriteLine(len);
            
            // Check if elements of unsorted list fetched from sorted results are similar to sorted list
            for (int i = 0; i < len; i++)
            {
                if (viewsInt[i] == viewsSorted[i])
                {
                    Console.WriteLine("Test has passed!");
                }

            }
        }

        [TearDown]
            public void close_Browser()
            {
                // Close web browser
                driver.Quit();
            }
        
    }
}