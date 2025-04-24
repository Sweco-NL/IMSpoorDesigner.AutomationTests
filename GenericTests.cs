using System;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.AutomationElements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using FlaUI.Core.Input;
using System.Runtime.Versioning;
using System.IO;
using System.Reflection;
using FlaUI.Core.Tools;

namespace IMSpoorDesigner.AutomationTests
{


    [TestClass]
    public class GenericTests
    {

        private static readonly string AppPath = ConfigHelper.GetApplicationPath();
        private static string GetAppFolder
        {
            get
            {
                var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(baseDir, "..\\..\\..\\TestFiles\\");
            }
        }
        [SupportedOSPlatform("windows")]

        private void OpenImxFile(Application app, Window mainWindow, string fileName)
        {
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
            txtInput.Text = fileName;
            txtInput.Patterns.Value.Pattern.SetValue(fileName);
            var openBtn = dialogWindow.FindFirstChild(cf => cf.ByName("Open")).AsButton();
            openBtn.Click();


        }

        [TestMethod]
        [SupportedOSPlatform("windows")]
        public void TestOpenIMXAndShowOnMap()
        {
            using var app = Application.Launch(AppPath);
            using var automation = new UIA3Automation();
            var mainWindow = app.GetMainWindow(automation);
            OpenImxFile(app, mainWindow, "2021-11-17_1243_Halfweg.xml");
          
            //assert//
            var treeView = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ImxTreeview"));
            Thread.Sleep(2000);
            var treeItem = treeView.FindFirstChild(cf => cf.ByName("IMSpoor.TreeviewManager.TreeItemViewModels.SituationTreeItemViewModel"));
            Assert.IsNotNull(treeItem,"There is no treeviewCreated, IMX not loaded!");
            

            //app.Close();

            // Optionally verify some UI change after clicking the button.
            //var label = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("YourLabelId")).AsLabel();
            // Assert.AreEqual("Expected Text", label.Text, "Label text did not update as expected!");
        }
    }
}