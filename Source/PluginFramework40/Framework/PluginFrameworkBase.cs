//=============================================================================
//D The PluginFrameworkBase class.
//
// ----------------------------------------------------------------------------
// Copyright 2011 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC  Who When     What
// ----- --- -------- ---------------------------------------------------------
// 93978 PSL 10/11/11 Written.
// 99355 PSL 23/07/12 Allow derived classes to access m_services.
// 99896 PSL 29/08/12 Don't assume registry paths won't contain "\\" chars.
// 99968 PSL 31/08/12 Don't assume path won't be null.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Win32;
using PowerMILL;
using System.Text.RegularExpressions;
using Delcam.Plugins.Localisation;
using Delcam.Plugins.Events;
using Delcam.Plugins.Utils;

namespace Delcam.Plugins.Framework
{
    /// <summary>
    /// The abstract base class for all plugins using the framework.  This class provides a default implementation of the
    /// IPluginCommunicationsInterface interface functions, and handles events and localisation functionality. 
    /// </summary>

    public abstract class PluginFrameworkBase : IPluginCommunicationsInterface
    {
        // Private members
        private string m_token;
        private PluginServices m_services;
        private IntPtr? m_parent_window_hwnd = null;
        private EventManager m_event_utils;

        // Public abstract properties
        //=============================================================================
        /// <summary>
        /// Returns the name of the plugin assembly.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract string PluginAssemblyName { get; }

        //=============================================================================
        /// <summary>
        /// Returns the plugin's GUID.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public abstract Guid PluginGuid { get;  } 

        // C'tor
        //=============================================================================
        /// <summary>
        /// 
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public PluginFrameworkBase()
        {
        }

        // Virtual functions - may be overriden by dervied classed, but should always call down
        //=============================================================================
        /// <summary>
        /// Called to do an initial setup of the locale, and setup the translation manager.  This 
        /// function may be overriden by dervied classed, but should always call down to this implementation.
        /// </summary>
        /// <param name="locale"></param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public virtual void setup_locale(string locale)
        {
            // Setup the translator
            string translation_files_path = PluginInstallPath + "/Localisation";
            TranslationManager.Instance(PluginGuid).set_locale_from_iso_string(locale);
            TranslationManager.Instance(PluginGuid).TranslationProvider = new XMLTranslationProvider(translation_files_path);
        }

        // Virtual functions - may be overriden by dervied classed, but should always call down
        //=============================================================================
        /// <summary>
        /// Called to setup the framework. This function may be overriden by dervied classed, but should always call 
        /// down to this implementation.
        /// </summary>
        /// <param name="token">The token supplied when the plugin is first initialised.</param>
        /// <param name="services">The services interface supplied when the plugin is first initialised.</param>
        /// <param name="parent_window_hwnd">The parent HWND, supplied when the plugin is first initialised.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public virtual void setup_framework(string token, PluginServices services, int parent_window_hwnd)
        {
            // Cache the arguments
            m_token = token;
            m_services = services;
            m_parent_window_hwnd = new IntPtr(parent_window_hwnd);

            // Setup the event utilities class
            m_event_utils = new EventManager(this);

            // Setup the translator to hear about locale changes
            m_event_utils.Subscribe(new EventSubscription("LocaleChanged", TranslationManager.Instance(PluginGuid).locale_changed));
        }
        
        //=============================================================================
        /// <summary>
        /// Called to shutdown the framework.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public virtual void shutdown_framework()
        {
            m_services = null;

            // Do a full garbage collect - this is required to sweep away any remaining COM objects that have
            // been created on our behalf, as they'll stop PM shutting down
            GC.Collect();
        }

        // Interface functions
#region PluginCommunicationsInterface Members

        //=============================================================================
        /// <summary>
        /// Returns the token that was supplied when the plugin was first initialised.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Token
        {
	        get { return m_token; }
        }

        //=============================================================================
        /// <summary>
        /// Returns the services interface that was supplied when the plugin was first initialised.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        // 99355 PSL 23/07/12 Added protected set.
        //-----------------------------------------------------------------------------
        public PluginServices Services
        {
	    get { return m_services; }
            protected set { m_services = value; }
        }

        //=============================================================================
        /// <summary>
        /// Returns the parent HWND, supplied when the plugin is first initialised.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public IntPtr? ParentWindow
        {
            get { return m_parent_window_hwnd; }
        }

        //=============================================================================
        /// <summary>
        /// Returns an instance of the event manager.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public EventManager EventUtils
        {
            get { return m_event_utils; }
        }

        //=============================================================================
        /// <summary>
        /// Returns an instance of the translation manager.
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
        /// Convenience function to issue a PowerMILL command.
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
            int response = m_services.DoCommand(m_token, command);
            if (response != 0)
            {
                throw new Exception("Failed to issue the command: " + command);
            }
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to insert a PowerMILL command.
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
            int response = m_services.InsertCommand(m_token, command);
            if (response != 0)
            {
                throw new Exception("Failed to queue the command: " + command);
            }
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to get a string parameter.  This function works even if commands are being echoed to the command window.
        /// </summary>
        /// <param name="par_name">The parameter name.</param>
        /// <returns>The string parameter value.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string get_string_par(string par_name)
        {
            object response;
            int error = m_services.DoCommandEx(m_token, "PRINT PAR '" + par_name + "'", out response);
            if (error != 0)
            {
                throw new Exception("Failed to obtain parameter value: " + par_name);
            }
            return process_string_response(response);
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to get a real (double) parameter.  This function works even if commands are being echoed to the command window.
        /// </summary>
        /// <param name="par_name">The parameter name.</param>
        /// <returns>The real (double) parameter value.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public double get_real_par(string par_name)
        {
            object response;
            int error = m_services.DoCommandEx(m_token, "PRINT PAR '" + par_name + "'", out response);
            if (error != 0)
            {
                throw new Exception("Failed to obtain parameter value: " + par_name);
            }
            return process_real_response(response);
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to get an int parameter.  This function works even if commands are being echoed to the command window.
        /// </summary>
        /// <param name="par_name">The parameter name.</param>
        /// <returns>The int parameter value.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public int get_int_par(string par_name)
        {
            object response;
            int error = m_services.DoCommandEx(m_token, "PRINT PAR '" + par_name + "'", out response);
            if (error != 0)
            {
                throw new Exception("Failed to obtain parameter value: " + par_name);
            }
            return process_int_response(response);
        }

        //=============================================================================
        /// <summary>
        /// Convenience function to obtain information from PowerMILL.
        /// </summary>
        /// <param name="info_name">The name of the information to be obtained.</param>
        /// <returns>The string-based information.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string get_pmill_info(string info_name)
        {
            if (m_services == null)
            {
                return "";
            }
            // The following will throw an exception if info name isn't recognised by PowerMILL
            return m_services.RequestInformation(info_name);
        }

        //=============================================================================
        /// <summary>
        /// The install path of the plugin.
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
                return plugin_install_path(PluginGuid);
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns true if the plugin is connected to PowerMILL.  Returns false if the plugin is
        /// being hosted outside of PowerMILL.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public bool ConnectedToPowerMILL
        {
            get
            {
                // Have we been setup correctly?
                return m_token != null && m_token.Length > 0 && m_services != null && m_parent_window_hwnd != null;
            }
        }

#endregion

        // Utility functions
        //=============================================================================
        /// <summary>
        /// Utility function to obtain the install path of a plugin given a GUID.  This inspects the registry for the 'CodeBase' path value.
        /// </summary>
        /// <param name="guid">The GUID of the plugin</param>
        /// <returns>The plugin install path.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        // 99896 PSL 29/08/12 Don't assume registry paths won't contain "\\" chars.
        // 99968 PSL 31/08/12 Don't assume path won't be null.
        //-----------------------------------------------------------------------------
        static public string plugin_install_path(Guid guid)
        {
            string guid_string = guid.ToString();

            // Get the path to the dll
            string dll_path = (string)Registry.GetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{" + guid_string + @"}\InprocServer32",
                "CodeBase",
                "");

            // Tidy up the string
            if (dll_path != null)
            {
                dll_path = dll_path.Replace('\\', '/');
                int last = dll_path.LastIndexOf("/");
                if (last > 1)
                {
                    string root_path = dll_path.Substring(0, last);
                    root_path = root_path.Replace("file:///", "");
                    return root_path;
                }
            }
            
            // Fail...
            return null;            
        }

        static Regex regex1 = new Regex(@"\s*Process Command : \[[^\]]*]\s*\(([^)]*)\)(.*)");
        static Regex regex2 = new Regex(@"\s*\(([^)]*)\)(.*)");
        //=============================================================================
        /// <summary>
        /// 
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        private bool process_response(object response, out string response_type, out string value)
        {
            // If echo commands is on, we expect a response along these lines for failure:
            //
            // Process Command : [print par 'tool.name'\r]
            //
            // (ERROR) Invalid name
            //
            // or this for success:
            //
            // Process Command : [print par 'tool.name'\r]
            //
            // (STRING) Fred

            string res = response as string;
            if (res != null)
            {
                // See if echo commands is on
                Match match = regex1.Match(res);
                if (match.Success)
                {
                    Capture capture = match.Groups[1];
                    response_type = capture.Value.Trim();
                    capture = match.Groups[2];
                    value = capture.Value.Trim();
                    return true;
                }

                // That failed, so try again assuming echo commands is off
                match = regex2.Match(res);
                if (match.Success)
                {
                    Capture capture = match.Groups[1];
                    response_type = capture.Value.Trim();
                    capture = match.Groups[2];
                    value = capture.Value.Trim();
                    return true;
                }
            }

            response_type = "";
            value = "";
            return false;
        }

        //=============================================================================
        /// <summary>
        /// 
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        private string process_string_response(object response)
        {
            string response_type;
            string value;
            if (process_response(response, out response_type, out value))
            {
                if (response_type.ToLower() == "string")
                {
                    return value;
                }
            }
            throw new Exception("Failed to read a 'string' from PowerMILL's response");
        }

        //=============================================================================
        /// <summary>
        /// 
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        private double process_real_response(object response)
        {
            string response_type;
            string value;
            if (process_response(response, out response_type, out value))
            {
                if (response_type.ToLower() == "real")
                {
                    return double.Parse(value);
                }
            }
            throw new Exception("Failed to read a 'real' from PowerMILL's response");
        }

        //=============================================================================
        /// <summary>
        /// 
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        private int process_int_response(object response)
        {
            string response_type;
            string value;
            if (process_response(response, out response_type, out value))
            {
                if (response_type.ToLower() == "int")
                {
                    return int.Parse(value);
                }
            }
            throw new Exception("Failed to read an 'int' from PowerMILL's response");
        }
    }
}
