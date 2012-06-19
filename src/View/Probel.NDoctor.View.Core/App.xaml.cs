﻿/*
    This file is part of NDoctor.

    NDoctor is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    NDoctor is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with NDoctor.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace Probel.NDoctor.View.Core
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Threading;

    using log4net;

    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Core.View;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using MySplashScreen = Probel.NDoctor.View.Core.View.SplashScreen;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Fields

        private static RibbonData ribbonData = new RibbonData();

        #endregion Fields

        #region Constructors

        public App()
        {
#if DEBUG
            //Hook the console to the application to have logging features
            AllocConsole();
#endif
            this.Logger = LogManager.GetLogger(typeof(App));
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Language);

            PluginContext.Configuration.ComponentLogginEnabled = bool.Parse(ConfigurationManager.AppSettings["ComponentLogginEnabled"]);

            var main = new MainWindow();
            var splash = new MySplashScreen();

            try { splash.ShowDialog(); }
            catch (Exception ex)
            {
                this.Logger.Error("An error occured in the spashscreen", ex);
                throw ex;
            }

            if (!splash.IsOnError) main.Show();
            else Application.Current.Shutdown();
        }
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the ribbon data.
        /// </summary>
        public static RibbonData RibbonData
        {
            get { return ribbonData; }
        }

        /// <summary>
        /// Gets a value indicating whether the uncaught exception are handled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if uncaught exceptions are handled; otherwise, <c>false</c>.
        /// </value>
        private bool DoHandle
        {
            get
            {
                var value = ConfigurationManager.AppSettings["HandleUncaughtException"];
                bool result;

                if (bool.TryParse(value, out result)) return result;
                else return false;
            }
        }

        private ILog Logger
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the DispatcherUnhandledException event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            this.Logger.Fatal("An uncaught exception was thrown", e.Exception);

            MessageBox.Show(e.Exception.Message, Messages.Title_Fatal, MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = this.DoHandle;
        }

        #endregion Methods
    }
}