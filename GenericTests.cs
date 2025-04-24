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
            var openMenuItem = menu.FindFirstDescendant(cf => cf.ByName("Open Folder")).AsMenuItem();
            openMenuItem?.Click();

            Thread.Sleep(2000);

            var dialogWindow = mainWindow.ModalWindows[0]; // Get the first modal dialog (File Open dialog)

            var filePathTextBox = dialogWindow.FindFirstDescendant(cf => cf.ByAutomationId("1152")).AsComboBox(); // "1148" is a common AutomationId for file dialogs
            var txtInput = filePathTextBox.FindFirstChild().As<TextBox>();
            var openBtn = dialogWindow.FindFirstChild(cf => cf.ByName("Select Folder")).AsButton();
            openBtn.Click();


        }

        [TestMethod]
        [SupportedOSPlatform("windows")]
        public void TestOpenIMXAndShowOnMap()
        {
            using var app = Application.Launch(AppPath);
            using var automation = new UIA3Automation();
            var mainWindow = app.GetMainWindow(automation);
            OpenImxFile(app, mainWindow, "ModelStation_TC9l");
          
            //assert//
            var treeView = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("ImxTreeview"));
            Thread.Sleep(12000);
            var treeItem = treeView.FindFirstChild(cf => cf.ByName("IMSpoor.TreeviewManager.TreeItemViewModels.SituationTreeItemViewModel"));
            
            Assert.IsNotNull(treeItem,"There is no treeviewCreated, IMX not loaded!");

            TestRenderSSPOnMap(app, mainWindow);

            Thread.Sleep(2000);
            app.Close();
            //app.Close();

            // Optionally verify some UI change after clicking the button.
            //var label = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("YourLabelId")).AsLabel();
            // Assert.AreEqual("Expected Text", label.Text, "Label text did not update as expected!");
        }

        [SupportedOSPlatform("windows")]
        public void TestRenderSSPOnMap(Application app, Window mainWindow)
        {
            var menu = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("mainRibbon")).AsMenu();
            var analysis = menu.FindFirstDescendant(cf => cf.ByAutomationId("AnalysisRibbonTab")).AsTabItem();
            var analysisButton = analysis.FindFirstDescendant(cf => cf.ByClassName("RibbonTabHeader"));
            Assert.IsNotNull(analysisButton, "Analysis tab not found!");
            Thread.Sleep(2000);
            analysisButton?.Click();

            var sspList = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("StaticSpeedProfile")).AsListBox();
            Thread.Sleep(1000);
            var sspBasic = sspList.FindFirstDescendant(cf => cf.ByName("Show SSP Basic Speed")).AsButton();
            var sspCat0 = sspList.FindFirstDescendant(cf => cf.ByName(" Show SSP Category 0")).AsButton();
            var sspCat1 = sspList.FindFirstDescendant(cf => cf.ByName(" Show SSP Category 1")).AsButton();
            var sspCat2 = sspList.FindFirstDescendant(cf => cf.ByName(" Show SSP Category 2")).AsButton();

            sspBasic.Click();
            Thread.Sleep(3000);
            sspBasic.Click();

            Thread.Sleep(1000);
            sspCat0.Click();
            Thread.Sleep(1000);
            sspCat0.Click();

            Thread.Sleep(1000);
            sspCat1.Click();
            Thread.Sleep(1000);
            sspCat1.Click();

            Thread.Sleep(1000);
            sspCat2.Click();
            Thread.Sleep(1000);
            sspCat2.Click();
        }
    }
}