//=============================================================================
//D The PluginFrameworkWithoutPanes class.
//
// ----------------------------------------------------------------------------
// Copyright 2011 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC  Who When     What
// ----- --- -------- ---------------------------------------------------------
// 93978 PSL 10/11/11 Written.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Delcam.Plugins.Utils;

namespace Delcam.Plugins.Framework
{
    /// <summary>
    /// This class is the base class for any plugins that do not require support for panes, and implements
    /// the minimum required IPowerMILLPlugin interface.
    /// </summary>
    public abstract class PluginFrameworkWithoutPanes : PluginFrameworkBase, PowerMILL.IPowerMILLPlugin
    {
        // Abstracts the derived class will need to implement
        //=============================================================================
        /// <summary>
        /// Returns the name of the plugin, which should be translated to the current locale.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract string PluginName { get; }

        //=============================================================================
        /// <summary>
        /// Returns the author/publisher of the plugin.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract string PluginAuthor { get; }

        //=============================================================================
        /// <summary>
        /// Returns a description of the plugin, which should be translated to the current locale.
        /// </summary>       
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract string PluginDescription { get; }

        //=============================================================================
        /// <summary>
        /// Returns the path to the plugin's icon within the resources.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract string PluginIconPath { get; }

        //=============================================================================
        /// <summary>
        /// Returns the version of the plugin.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract Version PluginVersion { get; }

        //=============================================================================
        /// <summary>
        ///  Returns the minimum PowerMILL version required to run the plugin.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract Version PowerMILLVersion { get; }

        //=============================================================================
        /// <summary>
        /// Returns true if the plugin has an option form that can be displayed.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract bool PluginHasOptions { get; }

        // Virtuals the derived class may need to override if they need non-default
        // behaviour
        //=============================================================================
        /// <summary>
        /// Called when a PowerMILL project is being saved or loaded, giving the plugin the opportunity to save/retrieve data.
        /// </summary>
        /// <param name="Path">The path to the plugin's directory with the project directory.</param>
        /// <param name="Saving">true if the project is being saved, false if it's being loaded.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public virtual void SerializePluginProjectData(string Path, bool Saving)
        {
        }

        //=============================================================================
        /// <summary>
        /// Called to display the options form.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public virtual void DisplayOptionsForm()
        {
        }

        //=============================================================================
        /// <summary>
        /// Called when the plugin needs to process a command.
        /// </summary>
        /// <param name="Command">The command to be processed.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public virtual void ProcessPluginCommand(string Command)
        {
        }
        
        // Implementation of the plugin interface
        #region IPowerMILLPlugin Members

        //=============================================================================
        /// <summary>
        /// Returns the author/publisher of the plugin.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Author
        {
            get { return PluginAuthor; }
        }

        //=============================================================================
        /// <summary>
        /// Returns a description of the plugin, which should be translated to the current locale.
        /// </summary>    
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Description
        {
            get { return PluginDescription; }
        }

        //=============================================================================
        /// <summary>
        /// Called to display the options form.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void DisplayOptions()
        {
            DisplayOptionsForm();
        }

        //=============================================================================
        /// <summary>
        /// Returns true if the plugin has an option form that can be displayed.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public bool HasOptions
        {
            get { return PluginHasOptions; }
        }

        //=============================================================================
        /// <summary>
        /// Called before the plugin has been initialised, and provides the current locale.  The plugin must
        /// translate into this locale when reponding with the name and description of the plugin.
        /// </summary>
        /// <param name="locale"></param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void PreInitialise(string locale)
        {
            try
            {
                setup_locale(locale);
            }
            catch
            {
            }
        }

        //=============================================================================
        /// <summary>
        /// Called to initialise the plugin.  The arguments should be cached for later use.
        /// </summary>
        /// <param name="Token">This token is required whenever communicating with PowerMILL.</param>
        /// <param name="pServices">An interface pointer to the services object, by which the plugin will communicate with PowerMILL.</param>
        /// <param name="ParentWindow">The HWND of the parent window, so dialogs can be correctly parented.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void Initialise(string Token, PowerMILL.PluginServices pServices, int ParentWindow)
        {
            try
            {
                setup_framework(Token, pServices, ParentWindow);
            }
            catch
            {
            }
        }

        //=============================================================================
        /// <summary>        
        /// Returns the minimum PowerMILL version required to run the plugin.
        /// </summary>
        /// <param name="pMajor">The major version number.</param>
        /// <param name="pMinor">The minor version number.</param>
        /// <param name="pIssue">The issue/build number.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void MinPowerMILLVersion(out int pMajor, out int pMinor, out int pIssue)
        {
            Version pm_version = PowerMILLVersion;
            pMajor = pm_version.Major;
            pMinor = pm_version.Minor;
            pIssue = pm_version.Build;
        }

        //=============================================================================
        /// <summary>
        /// Returns the name of the plugin, which should be translated to the current locale.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Name
        {
            get { return PluginName; }
        }

        //=============================================================================
        /// <summary>
        /// Called to obtain the plugin's icon as a byte array of pixels.  Icons must be 32 x 32.
        /// </summary>
        /// <param name="pFormat">The pixel format, i.e. order of RGB pixels and possibly alpha channel.</param>
        /// <param name="pPixelData">The byte array.</param>
        /// <param name="pWidth">The width of the icon, which should be 32 pixels.</param>
        /// <param name="pHeight">The height of the icon, which should be 32 pixels.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void PluginIconBitmap(out PowerMILL.PluginBitmapFormat pFormat, out byte[] pPixelData, out int pWidth, out int pHeight)
        {
            BitmapUtils.get_bitmap(PluginAssemblyName, PluginIconPath, out pFormat, out pPixelData, out pWidth, out pHeight); 
        }

        //=============================================================================
        /// <summary>
        /// Called to process an event that was previously subscribed to.
        /// </summary>
        /// <param name="EventData">The xml snippet for the event data.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void ProcessEvent(string EventData)
        {
            EventUtils.HandleEvent(EventData);
        }

        //=============================================================================
        /// <summary>
        /// Called to process a plugin command.
        /// </summary>
        /// <param name="Command">The command to be processed.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void ProcessCommand(string Command)
        {
            ProcessPluginCommand(Command);
        }

        //=============================================================================
        /// <summary>
        /// Called when PowerMILL is saving or loading data, giving the plugin opportunity to add or retrieve data from the project directory.
        /// </summary>
        /// <param name="Path">The path to the plugin's directory within the project directory.</param>
        /// <param name="Saving">true if the project is being saved, and false if it's being loaded.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void SerializeProjectData(string Path, bool Saving)
        {
            SerializePluginProjectData(Path, Saving);
        }

        //=============================================================================
        /// <summary>
        /// Called when a plugin is being terminated.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void Uninitialise()
        {
            shutdown_framework();
        }

        //=============================================================================
        /// <summary>
        /// Called to obtain the plugin version.
        /// </summary>
        /// <param name="pMajor">The major version number.</param>
        /// <param name="pMinor">The minor version number.</param>
        /// <param name="pIssue">The issue/build number.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void Version(out int pMajor, out int pMinor, out int pIssue)
        {
            Version plugin_version = PluginVersion;
            pMajor = plugin_version.Major;
            pMinor = plugin_version.Minor;
            pIssue = plugin_version.Build;
        }

        #endregion
    }
}
