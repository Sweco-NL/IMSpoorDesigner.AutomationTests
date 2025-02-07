using System;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using FlaUI.Core.Input;

[TestClass]
public class UiTests
{
    private const string AppPath = @"D:\Projecten\Development - IMX\IMSpoorDesigner\IMSpoorDesigner.Application\bin\x64\Debug\net8.0-windows10.0.19041.0\Application.IMSpoorDesigner.exe"; // Update this with your app's path.

    [TestMethod]
    public void Test1 ()
    {
        Assert.IsFalse(true);
    }

    [TestMethod]
    public void TestButtonClick()
    {
        using (var app = Application.Launch(AppPath))
        {
            using (var automation = new UIA3Automation())
            {
                // Get the main window of the application.
                var mainWindow = app.GetMainWindow(automation);

             
                var menu = mainWindow.FindFirstDescendant(cf => cf.ByClassName("RibbonApplicationMenu")).AsMenu();
                var menuBtn = menu.FindFirstDescendant(cf => cf.ByClassName("RibbonToggleButton"));
                menuBtn?.Click();
                var openMenuItem = menu.FindFirstDescendant(cf => cf.ByName("Open ...")).AsMenuItem();
                openMenuItem?.Click();
                
                Thread.Sleep(2000);

                var dialogWindow = mainWindow.ModalWindows[0]; // Get the first modal dialog (File Open dialog)
                
                var filePathTextBox = dialogWindow.FindFirstDescendant(cf => cf.ByAutomationId("1148")).AsComboBox(); // "1148" is a common AutomationId for file dialogs
                var txtInput = filePathTextBox.FindFirstChild().As<TextBox>();
                txtInput.Focus();
                //filePathTextBox.Text = string.Empty;

                txtInput.Text = @"D:\Data\IMX\2021-11-17_1243_Halfweg.xml";
                txtInput.Patterns.Value.Pattern.SetValue(@"D:\Data\IMX\2021-11-17_1243_Halfweg.xml");

                var openBtn = dialogWindow.FindFirstChild(cf => cf.ByName("Open")).AsButton();
                openBtn.Click();


                // Optionally verify some UI change after clicking the button.
                //var label = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("YourLabelId")).AsLabel();
                // Assert.AreEqual("Expected Text", label.Text, "Label text did not update as expected!");
            }
        }
    }
}