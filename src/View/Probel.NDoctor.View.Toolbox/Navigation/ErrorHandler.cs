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
namespace Probel.NDoctor.View.Toolbox.Navigation
{
    using System;
    using System.Text;
    using System.Windows;

    using log4net;

    using Probel.Helpers.Assertion;
    using Probel.Helpers.Strings;
    using Probel.Mvvm.Gui;
    using Probel.NDoctor.Domain.Components;
    using Probel.NDoctor.View.Toolbox;
    using Probel.NDoctor.View.Toolbox.Controls;
    using Probel.NDoctor.View.Toolbox.Helpers;
    using Probel.NDoctor.View.Toolbox.Properties;
    using Probel.NDoctor.View.Toolbox.ViewModel;

    /// <summary>
    /// This class provides error handling methods
    /// </summary>
    public class ErrorHandler : IErrorHandler
    {
        #region Fields

        private readonly IStatusWriter StatusWriter;
        private readonly WindowManager WindowManager = new WindowManager();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandler"/> class.
        /// </summary>
        /// <param name="callerType">Type of the caller.</param>
        internal ErrorHandler(object caller, IStatusWriter statusWriter)
        {
            this.Logger = LogManager.GetLogger(caller.GetType());
            this.StatusWriter = statusWriter;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public ILog Logger
        {
            get;
            private set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void Error(Exception ex)
        {
            this.Error(ex, ex.Message);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Error(Exception ex, string format, params object[] args)
        {
            this.HandleError(false, ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorSilently(Exception ex, string format, params object[] args)
        {
            this.HandleError(false, ex, format, args);
        }

        /// <summary>
        /// Handles the fatal error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log.</param>
        public void Fatal(Exception ex)
        {
            this.Fatal(ex, ex.Message);
        }

        /// <summary>
        /// Handles the fatal error, log it and shows a message box with the error.
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Fatal(Exception ex, string format, params object[] args)
        {
            this.HandleFatal(ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and shows a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void Warning(Exception ex, string format, params object[] args)
        {
            this.HandleWarning(false, ex, format, args);
        }

        /// <summary>
        /// Handles the error, log it and DOESN'T show a message box with the error.
        /// This error is showed as a warning
        /// </summary>
        /// <param name="ex">The exception to log..</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarningSilently(Exception ex, string format, params object[] args)
        {
            this.HandleWarning(true, ex, format, args);
        }

        private void HandleError(bool silent, Exception ex, string format, params object[] args)
        {
            //Logs only error, not authorisation errors...
            if (!(ex is AuthorisationException)) { this.Logger.Error(format.FormatWith(args), ex); }

            if (ex is AssertionException) //Any AssertionException highlight a important issue, it FATAL
            {
                WindowManager.Show<ExceptionViewModel>();
            }
            else if (!silent) //Check if the error should be shown in the foreground.
            {
                if (ex.GetType() == typeof(AuthorisationException))
                {
                    MessageBox.Show(Messages.Msg_ErrorAuthorisation, Messages.Title_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(format.FormatWith(args), Messages.Title_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                    //WindowManager.Show<ExceptionViewModel>();
                }
            }
            this.WriteErrorInStatus();
        }

        private void HandleFatal(Exception ex, string format, params object[] args)
        {
            this.Logger.Fatal(format.FormatWith(args), ex);
            this.WindowManager.Show<ExceptionViewModel>();
        }

        private void HandleWarning(bool silent, Exception ex, string format, params object[] args)
        {
            this.Logger.Warn(format.FormatWith(args), ex);
            if (!silent)
            {
                if (ex.GetType() == typeof(AuthorisationException))
                {
                    MessageBox.Show(Messages.Msg_ErrorAuthorisation, Messages.Title_Error, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, Messages.Title_Warning, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            this.WriteWarningInStatus(string.Format(format, args));
        }

        private void WriteErrorInStatus()
        {
            if (this.StatusWriter != null)
            {
                StatusWriter.WriteStatus(StatusType.Error, Messages.Msg_ErrorOccured);
            }
            else { this.Logger.Warn("No StatusWriter is configured"); }
        }

        private void WriteWarningInStatus(string message)
        {
            if (StatusWriter != null)
            {
                StatusWriter.WriteStatus(StatusType.Warning, message);
            }
            else { this.Logger.Warn("No StatusWriter is configured"); }
        }

        #endregion Methods
    }
}