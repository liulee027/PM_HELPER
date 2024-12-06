//=============================================================================
//D Utilities to assist with installation.
//
// ----------------------------------------------------------------------------
// Copyright 2012 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC  Who When     What
// ----- --- -------- ---------------------------------------------------------
// 95167 PSL 23/01/12 Written.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

// Note: http://stackoverflow.com/questions/239465/how-to-register-a-net-ccw-with-regasm-from-a-visual-studio-2008-setup-project

// Another note: http://resnikb.wordpress.com/2009/05/21/installing-the-same-managed-addin-in-32bit-and-64bit-autodesk-inventor/

namespace Delcam.Plugins.Installation
{
    public static class InstallationUtils
    {
        // PowerMILL's component category - used by PowerMILL to discover plugins
        static private string s_plugin_component_category = "{311B0135-1826-4A8C-98DE-F313289F815E}";

        // Define the two registration options
        private enum RegistrationType
        {
            Register,
            Unregister
        }

        //=============================================================================
        /// <summary>
        /// Method called as a custom action when the plugin is installed.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 95167 PSL 23/01/12 Written.
        //-----------------------------------------------------------------------------
        public static void Install(string plugin_guid, string dll_path)
        {
            // Register the .NET assembly, with the codebase option so the path is
            // saved in the registry
            RegAsm("/codebase", dll_path);

            // Register the plugin component category
            RegisterCOMCategory(plugin_guid, RegistrationType.Register);
        }

        //=============================================================================
        /// <summary>
        /// Method called as a custom action when the plugin is uninstalled.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 95167 PSL 23/01/12 Written.
        //-----------------------------------------------------------------------------
        public static void Uninstall(string plugin_guid, string dll_path)
        {
            // Unregister the plugin component category
            RegisterCOMCategory(plugin_guid, RegistrationType.Unregister);

            // Unregister the .NET assembly
            RegAsm("/u", dll_path);
        }

        //=============================================================================
        /// <summary>
        /// Method to register an assembly with both x86 and x64 registry hives.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 95167 PSL 23/01/12 Written.
        //-----------------------------------------------------------------------------
        private static void RegAsm(string parameters, string dll_path)
        {
            // RuntimeEnvironment.GetRuntimeDirectory() returns something like
            //    C:\Windows\Microsoft.NET\Framework64\v2.0.50727\
            // We need to get to the
            //    C:\Windows\Microsoft.NET
            // part in order to create 32 and 64 bit paths
            string net_base = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), @"..\.."));

            // Create paths to 32bit and 64bit versions of regasm.exe
            string[] to_exec = new[]
	        {
	            string.Concat(net_base, "\\Framework\\", RuntimeEnvironment.GetSystemVersion(), "\\regasm.exe"),
	            string.Concat(net_base, "\\Framework64\\", RuntimeEnvironment.GetSystemVersion(), "\\regasm.exe")
	        };

            foreach (string path in to_exec)
            {
                // Skip the executables that do not exist
                // This most likely happens on an x86 machine when processing the path to an x64 regasm
                if (!File.Exists(path))
                    continue;

                // Build the argument string
                string args = string.Format("\"{0}\" {1}", dll_path, parameters);

                // Launch the process
                LaunchProcess(path, args);
            }
        }

        //=============================================================================
        /// <summary>
        /// Method to add PowerMILL's plugin component category to the plugin in both 
        /// x86 and x64 registry hives.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 95167 PSL 23/01/12 Written.
        //-----------------------------------------------------------------------------
        private static void RegisterCOMCategory(string plugin_guid, RegistrationType register)
        {
            // Build the registry key we wish to add
            string reg_key = "HKCR\\CLSID\\" + plugin_guid + "\\Implemented Categories\\" 
                + s_plugin_component_category;

            // Build the path
            string path = Environment.SystemDirectory + "\\reg.exe";

            // Loop over the platforms we wish to add the key to
            string[] platforms = new[] { "32", "64" };
            foreach (string platform in platforms)
            {
                // Build the arguments
                string args = (register == RegistrationType.Register) ? "ADD" : "DELETE";
                args += " \"" + reg_key + "\" /f";

                // Build platform args
                string platform_arg = " /reg:" + platform;

                // Launch the process
                int code = LaunchProcess(path, args + platform_arg);

                // Check for errors on XP - /reg option doesn't exist
                if (code != 0)
                {
                    // Repeat the operation without the /reg:32 option
                    code = LaunchProcess(path, args);

                    // If we're on XP, then there's no point doing this for x64
                    return;
                }
            }
        }

        //=============================================================================
        /// <summary>
        /// Method to lauch a process given the path and argument string.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 95167 PSL 23/01/12 Written.
        //-----------------------------------------------------------------------------
        private static int LaunchProcess(string path, string arguments)
        {
            // Initialise the exit code
            int exit_code = 0;

            // Create a process object, and initialise it
            Process process = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    FileName = path,
                    Arguments = arguments
                }
            };

            // Start the process, wait for it to exit, then tidy up (via using)
            using (process)
            {
                process.Start();
                process.WaitForExit();
                exit_code = process.ExitCode;
            }

            // Return the captured exit code
            return exit_code;
        }
    }
}
