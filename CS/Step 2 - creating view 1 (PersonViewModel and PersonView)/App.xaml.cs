﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;
using DX_MVVM.Helpers;

namespace DX_MVVM {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
#if !SILVERLIGHT
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            ApplicationThemeHelper.ApplicationThemeName = "Office2007Silver";
            MainWindow = new DXWindow() { Title = "How to use the DevExpress components with MVVM pattern", Width = 525, Height = 350 };
            MainWindow.Content = new MainPage();
            MainWindow.DataContext = PersonViewModelCreator.DesignPersonViewModel;
            MainWindow.Show();
        }
#else
        public App() {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            ThemeManager.ApplicationThemeName = "Office2007Silver";
            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e) {
            MainPage mainPage = new MainPage();
            mainPage.DataContext = PersonViewModelCreator.DesignPersonViewModel;
            this.RootVisual = mainPage;
        }

        private void Application_Exit(object sender, EventArgs e) {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e) {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if(!System.Diagnostics.Debugger.IsAttached) {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e) {
            try {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            } catch(Exception) {
            }
        }
#endif
    }
}
