﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace NestIn
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidNestInPkgString)]
    [ProvideOptionPage(typeof(NestInOptionsPage), "NestIn", "NestIn Options", 0, 0, true)]
    public sealed class NestInPackage : Package
    {

        public DefaultSelectionModeOptions DefaultSelectionMode
        {
            get
            {
                NestInOptionsPage page = (NestInOptionsPage)GetDialogPage(typeof(NestInOptionsPage));
                return page.DefaultSelectedItem;
            }
        }
        public bool ShouldPromptForDefaultRoot
        {
            get
            {
                NestInOptionsPage page = (NestInOptionsPage)GetDialogPage(typeof(NestInOptionsPage));
                return page.ShouldPromptForDefaultRoot;
            }
        }

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public NestInPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID nestInCommandId = new CommandID(GuidList.guidNestInCmdSet, (int)PkgCmdIDList.cmdidMyCommand);
                MenuCommand nestIn = new MenuCommand(NestInCallback, nestInCommandId);
                mcs.AddCommand(nestIn);

                CommandID unNestCommandId = new CommandID(GuidList.guidNestInCmdSet, (int)PkgCmdIDList.cmdidMyCommand2);
                MenuCommand unNestIn = new MenuCommand(UnNestCallback, unNestCommandId);
                mcs.AddCommand(unNestIn);
            }
        }
        #endregion

        private static T GetService<T>() where T : class
        {
            return (T)ServiceProvider.GlobalProvider.GetService(typeof(T));
            //var componentModelService = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            //return componentModelService.GetService<T>();
        }

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void NestInCallback(object sender, EventArgs e)
        {
            var rootSelector = new RootSelector();
            var envDte = GetService<DTE>();
            new Worker(envDte, rootSelector, DefaultSelectionMode, ShouldPromptForDefaultRoot).Nest();
            rootSelector.Dispose();
        }


        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void UnNestCallback(object sender, EventArgs e)
        {
            var rootSelector = new RootSelector();
            var envDte = GetService<DTE>();
            new Worker(envDte, rootSelector, DefaultSelectionMode, ShouldPromptForDefaultRoot).UnNest();
            rootSelector.Dispose();
        }

    }
}
