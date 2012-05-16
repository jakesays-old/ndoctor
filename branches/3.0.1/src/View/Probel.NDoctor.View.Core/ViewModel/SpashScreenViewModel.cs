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
namespace Probel.NDoctor.View.Core.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Input;

    using Probel.Helpers.Strings;
    using Probel.Mvvm.DataBinding;
    using Probel.NDoctor.Domain.DAL.Cfg;
    using Probel.NDoctor.View.Core.Model;
    using Probel.NDoctor.View.Core.Properties;
    using Probel.NDoctor.View.Plugins;
    using Probel.NDoctor.View.Plugins.Configuration;
    using Probel.NDoctor.View.Plugins.Helpers;
    using Probel.NDoctor.View.Plugins.MenuData;

    using StructureMap;

    public class SpashScreenViewModel : BaseViewModel
    {
        #region Fields

        private CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
        private uint progress;
        private string status;
        private string title;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpashScreenViewModel"/> class.
        /// </summary>
        public SpashScreenViewModel()
            : base()
        {
        }

        #endregion Constructors

        #region Events

        public event EventHandler Failed;

        public event EventHandler Loaded;

        #endregion Events

        #region Properties

        public uint Progress
        {
            get
            {
                return (this.progress > 100)
                    ? 100
                    : this.progress;
            }
            set
            {
                if (value > 100) return;
                this.progress = value;
                this.OnPropertyChanged(() => Progress);
            }
        }

        public string Status
        {
            get { return this.status; }
            set
            {
                this.status = value;
                this.OnPropertyChanged(() => Status);
            }
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged(() => Title);
            }
        }

        private bool CreateDatabase
        {
            get
            {
                var result = ConfigurationManager.AppSettings["CreateDatabase"];
                var createDatabase = false;
                if (bool.TryParse(result, out createDatabase)) return createDatabase;
                else return false;
            }
        }

        #endregion Properties

        #region Methods

        public void Start()
        {
            this.Logger.InfoFormat("Current culture: '{0}'", this.cultureInfo.ToString());
            var thread = new BackgroundWorker();
            thread.DoWork += (sender, e) =>
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;

                    this.BuildApplicationMenu();
                    this.BuildHomeMenu();

                    var version = Assembly.GetExecutingAssembly().GetName().Version;
                    this.Title = string.Format("nDoctor BETA {0}.{1}.{2}", version.Major, version.Minor, version.Build);

                    this.Status = Messages.Msg_ConfiguringNHibernate;
                    this.ConfigureNHibernate();
                    this.Progress = 33;

                    this.Status = Messages.Msg_ConfiguringStructureMap;
                    this.ConfigureStructureMap();
                    this.Progress = 66;

                    this.Status = Messages.Msg_ConfiguringPlugins;
                    this.ConfigurePlugins();
                    this.Progress = 100;
                    this.Logger.Info("Configuration done.");

                }
                catch (Exception ex)
                {
                    this.Logger.Error("An error occured in the spashscreen", ex);

                    MessageBox.Show(Messages.Msg_FatalErrorOccured.FormatWith(ex.Message)
                        , Messages.Title_Error
                        , MessageBoxButton.OK
                        , MessageBoxImage.Error);
                    this.OnFailed();
                }
            };
            thread.RunWorkerCompleted += (sender, e) => this.OnLoaded();
            thread.RunWorkerAsync();
        }

        private void BuildApplicationMenu()
        {
            #region Quit menu
            var shutdownMenu = new RibbonControlData(Messages.Menu_Exit, "", Commands.Shutdown) { Order = 999 };
            PluginContext.Host.AddToApplicationMenu(shutdownMenu);

            PluginContext.Host.AddToApplicationMenu(new RibbonSeparatorData(6));
            #endregion

            #region Home menu
            ICommand homeCommand = new RelayCommand(() =>
            {
                PluginContext.Host.NavigateToStartPage();
            });
            var homeMenu = new RibbonControlData(Messages.Menu_Home, "/Images/Home.png", homeCommand) { Order = 1 };

            PluginContext.Host.AddToApplicationMenu(homeMenu);
            PluginContext.Host.AddToApplicationMenu(new RibbonSeparatorData(2));
            #endregion
        }

        private void BuildHomeMenu()
        {
            var cmd = new RelayCommand(() => MessageBox.Show("Hello World!"));

            var groups = new ObservableCollection<RibbonGroupData>();
            groups.Add(new RibbonGroupData(Messages.Title_Tools));
            groups.Add(new RibbonGroupData(Messages.Title_Managers));
            groups.Add(new RibbonGroupData(Messages.Title_GlobalTools));

            PluginContext.Host.AddTab(new RibbonTabData(Messages.Title_Home, groups));
        }

        private void ConfigureNHibernate()
        {
            this.Logger.Info("Configuring nHibernate...");
            var path = ConfigurationManager.AppSettings["Database"];
            if (!File.Exists(path) && !this.CreateDatabase) throw new FileNotFoundException(Messages.Msg_ErrorDatabaseNotFound);

            this.Logger.DebugFormat("Database path: {0}", path);
            this.LogDatabaseCreation();
            new DAL().ConfigureUsingFile(path, this.CreateDatabase);
        }

        private void ConfigurePlugins()
        {
            this.Logger.Info("Configuring plugins...");
            var loader = ObjectFactory.GetInstance<IPluginLoader>();
            var container = new PluginContainer(PluginContext.Host, loader);
            container.LoadPlugins();
        }

        private void ConfigureStructureMap()
        {
            string configFile = "Plugins.config.xml";
            string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Plugins\");
            string xPath = @"/Plugins/Plugin";

            this.Logger.Info("Configuring StructureMap...");
            ObjectFactory.Configure(x =>
             {
                 x.For<IPluginLoader>().Add<MefPluginLoader>();

                 x.For<IPluginConfigurationLoader>().Add<XmlPluginConfigurationLoader>()
                     .Ctor<string>("configPath").Is(c => configPath)
                     .Ctor<string>("configFile").Is(c => configFile)
                     .Ctor<string>("xPath").Is(c => xPath);
             });
        }

        private void LogDatabaseCreation()
        {
            if (this.CreateDatabase) { this.Logger.Warn("Creation of a new database. Old data is deleted"); }
            else { this.Logger.Debug("Don't create a new database"); }
        }

        private void OnFailed()
        {
            if (this.Failed != null)
                this.Failed(this, EventArgs.Empty);
        }

        private void OnLoaded()
        {
            if (this.Loaded != null)
                this.Loaded(this, EventArgs.Empty);
        }

        #endregion Methods
    }
}