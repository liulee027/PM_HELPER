//=============================================================================
//D The PluginFrameworkNoPowerMILL class.
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
using Delcam.Plugins.Localisation;
using Delcam.Plugins.Events;

namespace Delcam.Plugins.Framework
{
    /// <summary>
    /// This class is intended to be used when the plugin is being hosted outside of PowerMILL for
    /// testing purposes.  
    /// </summary>
    public class PluginFrameworkNoPowerMILL : IPluginCommunicationsInterface
    {
        // private data members
        private Guid m_guid;
        private string m_assembly_name;

        //=============================================================================
        /// <summary>
        /// PluginFrameworkNoPowerMILL c'tor.
        /// </summary>
        /// <param name="guid">The guid of the plugin.</param>
        /// <param name="plugin_assembly_name">The name of the plugin's assembly.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public PluginFrameworkNoPowerMILL(Guid guid, string plugin_assembly_name)
        {
            // Cache the guid
            m_guid = guid;
            m_assembly_name = plugin_assembly_name;

            // Setup the translator
            string translation_files_path = PluginInstallPath + "/Localisation";
            //TranslationManager.Instance.set_locale_from_iso_string("fr-FR");
            TranslationManager.Instance(PluginGuid).TranslationProvider = new XMLTranslationProvider(translation_files_path);  
        }

        #region IPluginCommunicationsInterface Members

        //=============================================================================
        /// <summary>
        /// Flag used to
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public bool ConnectedToPowerMILL
        {
            get { return false; }
        }

        //=============================================================================
        /// <summary>
        /// Returns the token that was given to the plugin when it was first initilaised.  This token is 
        /// required whenever the plugin uses the plugins services interface to communicate with PowerMILL.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Token
        {
            get { throw new NotImplementedException("This property shouldn't be accesed when not connected to PowerMILL"); }
        }

        /// <summary>
        /// Returns a reference to the plugin services interface, which is used to communicate with PowerMILL.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        public PowerMILL.PluginServices Services
        {
            get { throw new NotImplementedException("This property shouldn't be accesed when not connected to PowerMILL"); }
        }

        //=============================================================================
        /// <summary>
        /// Returns PowerMILL's main window's HWND.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public IntPtr? ParentWindow
        {
            get { throw new NotImplementedException("This property shouldn't be accesed when not connected to PowerMILL"); }
        }

        //=============================================================================
        /// <summary>
        /// Returns a reference to the event manager, which can be used to subscribe to events.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public EventManager EventUtils
        {
            get { throw new NotImplementedException("This property shouldn't be accesed when not connected to PowerMILL"); }
        }

        //=============================================================================
        /// <summary>
        /// Returns a reference to the translation manager.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public TranslationManager TranslationUtils
        {
            get { return TranslationManager.Instance(PluginGuid); }
        }

        //=============================================================================
        /// <summary>
        /// Returns the assembly name of the plugin.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string PluginAssemblyName
        {
            get
            {
                return m_assembly_name;
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns the CLSID (a GUID) of the plugin.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public Guid PluginGuid
        {
            get
            {
                return m_guid;
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns the install path for the plugin, which is the path to the assembly dll.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string PluginInstallPath
        {
            get
            {
                return PluginFrameworkBase.plugin_install_path(m_guid);
            }
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to issue a PowerMILL command.  Do not call if PowerMILL is busy.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="command">The command to be issued.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void issue_command(string command)
        {
            throw new NotImplementedException("This function shouldn't be called when not connected to PowerMILL");
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to insert a PowerMILL command into the command queue.  
        /// This function shouldn't be called if PowerMILL is busy unless this is in response to a plugin command.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="command">The command to be inserted.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void insert_command(string command)
        {
            throw new NotImplementedException("This function shouldn't be called when not connected to PowerMILL");
        }

        //=============================================================================
        /// <summary>
        /// Returns a string parameter.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the parameter</param>
        /// <returns>Returns a string parameter.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string get_string_par(string par_name)
        {
            throw new NotImplementedException("This function shouldn't be called when not connected to PowerMILL");
        }

        //=============================================================================
        /// <summary>
        /// Returns a double parameter.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the parameter</param>
        /// <returns>Returns a double parameter.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public double get_real_par(string par_name)
        {
            throw new NotImplementedException("This function shouldn't be called when not connected to PowerMILL");
        }

        //=============================================================================
        /// <summary>
        /// Returns a int parameter.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the parameter</param>
        /// <returns>Returns an int parameter.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public int get_int_par(string par_name)
        {
            throw new NotImplementedException("This function shouldn't be called when not connected to PowerMILL");
        }

        //=============================================================================
        /// <summary>
        /// Returns a string representation of the required information, if it exists.
        /// Do not use if ConnectedToPowerMILL == false.
        /// </summary>
        /// <param name="par_name">The name of the information to be requested</param>
        /// <returns>A string containing the required information.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string get_pmill_info(string info_name)
        {
            throw new NotImplementedException("This function shouldn't be called when not connected to PowerMILL");
        }

        #endregion
    }
}
