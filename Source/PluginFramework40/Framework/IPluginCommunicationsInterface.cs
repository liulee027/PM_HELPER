//=============================================================================
//D The IPluginCommunicationsInterface interface.
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
using PowerMILL;
using Delcam.Plugins.Events;
using Delcam.Plugins.Localisation;

namespace Delcam.Plugins.Framework
{
    //=============================================================================
    /// <summary>
    /// An interface that's common to all of the plugin framwork base classes, exposing general infomration
    /// about the plugin and some convenience functions.
    /// </summary>
    //
    // History.
    // DICC  Who When     What
    // ----- --- -------- ---------------------------------------------------------
    // 93978 PSL 10/11/11 Written.
    //-----------------------------------------------------------------------------
    public interface IPluginCommunicationsInterface
    {
        /// <summary>
        /// Flag used to inform users of this interface if the plugin is actually connected to PowerMILL
        /// </summary>
        bool ConnectedToPowerMILL { get; }

        // Communications properties - do not use if ConnectedToPowerMILL == false
        /// <summary>
        /// Returns the token that was given to the plugin when it was first initilaised.  This token is 
        /// required whenever the plugin uses the plugins services interface to communicate with PowerMILL.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Returns a reference to the plugin services interface, which is used to communicate with PowerMILL.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        PluginServices Services { get; }

        /// <summary>
        /// Returns PowerMILL's main window's HWND.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        IntPtr? ParentWindow { get; }

        /// <summary>
        /// Returns a reference to the event manager, which can be used to subscribe to events.
        /// </summary>
        EventManager EventUtils { get; }

        // Plugin properties
        /// <summary>
        /// Returns the assembly name of the plugin.
        /// </summary>
        string PluginAssemblyName { get; }

        /// <summary>
        /// Returns the CLSID (a GUID) of the plugin.
        /// </summary>
        Guid PluginGuid { get; }

        /// <summary>
        /// Returns the install path for the plugin, which is the path to the assembly dll.
        /// </summary>
        string PluginInstallPath { get; }

        // Translation
        /// <summary>
        /// Returns a reference to the translation manager.
        /// </summary>
        TranslationManager TranslationUtils { get; }

        // Convenience functions - these all throw an exception if things don't 
        // go smoothly, so must be used in a try {} catch {} block...
        //
        // Again, do not use if ConnectedToPowerMILL == false
        /// <summary>
        /// Convenience function to issue a PowerMILL command.  Do not call if PowerMILL is busy.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="command">The command to be issued.</param>
        void issue_command(string command);

        /// <summary>
        /// Convenience function to insert a PowerMILL command into the command queue.  
        /// This function shouldn't be called if PowerMILL is busy unless this is in response to a plugin command.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="command">The command to be inserted.</param>
        void insert_command(string command);

        /// <summary>
        /// Returns a string parameter.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the parameter</param>
        /// <returns>Returns a string parameter.</returns>
        string get_string_par(string par_name);

        /// <summary>
        /// Returns a double parameter.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the parameter</param>
        /// <returns>Returns a double parameter.</returns>
        double get_real_par(string par_name);

        /// <summary>
        /// Returns a int parameter.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the parameter</param>
        /// <returns>Returns an int parameter.</returns>
        int get_int_par(string par_name);

        /// <summary>
        /// Returns a string representation of the required information, if it exists.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the information to be requested</param>
        /// <returns>A string containing the required information.</returns>
        string get_pmill_info(string info_name);
    }
}
