using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using NUnit.Framework;
using System.IO;
using System.Diagnostics;

namespace UserInteractionsTutorial
{
    public class InteractionDemo
    {
        private ChromeDriver _driver;
        private Actions _actions;
        private WebDriverWait _wait;

        [Test]
        public void DragDropExample1()
        {
            _driver.Navigate().GoToUrl("http://jqueryui.com/droppable/");
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("demo-frame")));

            IWebElement sourceElement = _driver.FindElement(By.Id("draggable"));
            IWebElement targetElement = _driver.FindElement(By.Id("droppable"));
            _actions.DragAndDrop(sourceElement, targetElement).Perform(); //source, target.

            Assert.AreEqual("Dropped!", targetElement.Text);
        }


        [Test]
        public void DragDropExample2()
        {
            _driver.Navigate().GoToUrl("http://jqueryui.com/droppable/");
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("demo-frame")));

            IWebElement sourceElement = _driver.FindElement(By.Id("draggable"));
            IWebElement targetElement = _driver.FindElement(By.Id("droppable"));

            //The same: _actions.DragAndDrop(sourceElement, targetElement).Perform();
            var drag = _actions
                .ClickAndHold(sourceElement)
                .MoveToElement(targetElement)
                .Release(targetElement)
                .Build();

            drag.Perform();

            Assert.AreEqual("Dropped!", targetElement.Text);
        }

        [Test]
        public void DragDropQuiz()
        {
            _driver.Navigate().GoToUrl("http://www.pureexample.com/jquery-ui/basic-droppable.html");
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("ExampleFrame-94")));

            IWebElement dragSource = _driver.FindElement(By.XPath("//*[@class='square ui-draggable']"));
            IWebElement dropTarget = _driver.FindElement(By.XPath("//*[@class='squaredotted ui-droppable']"));
            _actions.DragAndDrop(dragSource, dropTarget).Perform();

            IWebElement dropText = _driver.FindElement(By.Id("info"));
            Assert.AreEqual("dropped!", dropText.Text);
        }

        [Test]
        public void Resizable()
        {
            _driver.Navigate().GoToUrl("http://jqueryui.com/resizable/");
            _driver.Manage().Window.Maximize();
            _wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("demo-frame")));

            IWebElement cornerResizable = _driver.FindElement(By.XPath("//*[@class='ui-resizable-handle ui-resizable-se ui-icon ui-icon-gripsmall-diagonal-se']"));
            _actions.ClickAndHold(cornerResizable).MoveByOffset(300, 200).Perform();

            Assert.IsTrue(_driver.FindElement(By.XPath("//*[@id='resizable']")).Displayed);
            TestContext.Out.WriteLine("Matcheables");
        }

        [Test]
        public void OpenNetworkTabUsingGoogleChrome()
        {
            _driver.Navigate().GoToUrl("www.google.com");
            _actions.KeyDown(Keys.Control).KeyDown(Keys.Shift).SendKeys("i").Perform(); //ctrl + shift + i

            _actions.KeyUp(Keys.Control).KeyUp(Keys.Shift).Perform();
            // _driver.Navigate().GoToUrl("");
        }

        [Test]
        public void DragDropQuizHtml5()
        {
            _driver.Navigate().GoToUrl("http://the-internet.herokuapp.com/drag_and_drop");
            _driver.Manage().Window.Maximize();
            var source = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("column-a")));

            var jsFile = File.ReadAllText(@"C:\Users\Leonime\Desktop\drag_and_drop_helper.js"); //Select the path
            IJavaScriptExecutor js = _driver as IJavaScriptExecutor; //interface

            js.ExecuteScript(jsFile + "$('#column-a').simulateDragDrop({ dropTarget: '#column-b'});");

            var compareJS = _driver.FindElement(By.XPath("//*[@id='column-b']/header"));
            Assert.AreEqual("A", compareJS.Text);
        }
        
        [Test]
        public void DrawCanvas()
        {
            _driver.Navigate().GoToUrl("https://www.compendiumdev.co.uk/selenium/gui_user_interactions.html");
            _driver.Manage().Window.Maximize();

            //bring the page into focus to reduce intermitency
            _driver.FindElement(By.TagName("html")).Click();

            var canvas = _driver.FindElement(By.Id("canvas"));
           // Debug.WriteLine();
        }

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _actions = new Actions(_driver);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15)); //Time to wait
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Close();
            _driver.Quit();
        }
        
    }
}
