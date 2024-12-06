//=============================================================================
//D The PluginFrameworkWithPanes class.
//
// ----------------------------------------------------------------------------
// Copyright 2012 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC   Who When     What
// ------ --- -------- --------------------------------------------------------
//  99199 PSL 02/11/12 Written.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Delcam.Plugins.Utils;
using Delcam.Plugins.Events;

namespace Delcam.Plugins.Framework
{
    /// <summary>
    /// This class is the base class for any plugins that require support for panes and tabs, and implements
    /// the IPowerMILLPlugin, PowerMILL.IPowerMILLPluginPane and PowerMILL.IPowerMILLPluginTab interfaces.
    /// </summary>
    public abstract class PluginFrameworkWithPanesAndTabs : PluginFrameworkWithPanes, PowerMILL.IPowerMILLPluginTab
    {
        // Private data members
        private List<TabDefinition> m_tabs = new List<TabDefinition>();
        private bool m_ok_to_register = true;

        //=============================================================================
        /// <summary>
        /// The PluginFrameworkWithPanes c'tor.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public PluginFrameworkWithPanesAndTabs()
        {
        }

        // Abstract functions the derived class must implement

        //=============================================================================
        /// <summary>
        /// Called to ask the derived class to register any tabs that it requires.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        protected abstract void register_tabs();

        // Functions required by the derived class

        //=============================================================================
        /// <summary>
        /// Called to register tabs.
        /// </summary>
        /// <param name="pane">The tab definition to be registered.</param>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        protected void register_tab(TabDefinition tab)
        {
            if (m_ok_to_register)
            {
                m_tabs.Add(tab);
            }
            else
            {
                throw new Exception("Can't register new tabs after PowerMILL has requested the total number of tabs");
            }
        }

        // Private management functions
        //=============================================================================
        /// <summary>
        /// 
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        private void uninitialise_tabs()
        {
            foreach (TabDefinition tab in m_tabs)
            {
                tab.Uninitialise();
            }
        }

        // Overrides from PluginFrameworkBase
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
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public override void setup_framework(string token, PowerMILL.PluginServices services, int parent_window_hwnd)
        {
            base.setup_framework(token, services, parent_window_hwnd);
            register_tabs();
        }

        //=============================================================================
        /// <summary>
        /// Called to shutdown the framework.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public override void shutdown_framework()
        {
            uninitialise_tabs();
            base.shutdown_framework();
        } 

        // Implementation of IPowerMILLPluginTabs    
        #region IPowerMILLPluginTab Members
        
        //=============================================================================
        /// <summary>
        /// Called to initialise the tab before displaying it.
        /// </summary>
        /// <param name="TabIndex">The index of the tab being refered to.</param>
        /// <param name="ParentWindow">The HWND of the parent that the tab should use for rendering content.</param>
        /// <param name="Width">The width of the tab.</param>
        /// <param name="Height">The height of the tab.</param>
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public void  InitialiseTab(int TabIndex, int ParentWindow, int Width, int Height)
        {
            if (TabIndex < m_tabs.Count)
            {
                m_tabs[TabIndex].initialise_tab(ParentWindow, Width, Height);
            }
            else
            {
                throw new Exception("InitialiseTab: Index out of range");
            }
        }        
        
        //=============================================================================
        /// <summary>
        /// Returns the minimum height of the specified pane.
        /// </summary>
        /// <param name="TabIndex">The tab being referenced.</param>
        /// <returns>Returns the minimum height of the specified tab.</returns>
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public int  MinimumTabHeight(int TabIndex)
        {
            if (TabIndex < m_tabs.Count)
            {
                return m_tabs[TabIndex].MinimumTabHeight;
            }
            else
            {
                throw new Exception("MinimumTabHeight: Index out of range");
            }
        }
                
        //=============================================================================
        /// <summary>
        /// Returns the number of tabs that the plugin supports.  Tabs cannot be registered after this
        /// function has been called.
        /// </summary>
        /// <returns>Returns the number of tabs that the plugin supports</returns>       
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public int  NumberOfTabs()
        {
            m_ok_to_register = false;
            return m_tabs.Count;
        }        
        
        //=============================================================================
        /// <summary>=
        /// Called to inform the plugin that the specified tab has been resized.
        /// </summary>
        /// <param name="TabIndex">The tab being referenced.</param>
        /// <param name="Width">The new width of the tab.</param>
        /// <param name="Height">The new height of the tab.</param>
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public void  TabResized(int TabIndex, int Width, int Height)
        {
            if (TabIndex < m_tabs.Count)
            {
                m_tabs[TabIndex].tab_resized(Width, Height);
            }
            else
            {
                throw new Exception("TabResized: Index out of range");
            }
        }        
        
        //=============================================================================
        /// <summary>        
        /// Returns the title of the tab, which should be translated into the current locale.
        /// </summary>
        /// <param name="TabIndex">The tab being referenced.</param>
        /// <returns>The tranlsated title of the tab.</returns>
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public string  TabTitle(int TabIndex)
        {
            if (TabIndex < m_tabs.Count)
            {
                return m_tabs[TabIndex].TabTitle;
            }
            else
            {
                throw new Exception("TabTitle: Index out of range");
            }
        }        
        
        //=============================================================================
        /// <summary>        
        /// Called to obtain the tab icon.
        /// </summary>
        /// <param name="TabIndex">The tab being referenced.</param>
        /// <param name="pFormat">The pixel format, i.e. order of RGB pixels and possibly alpha channel.</param>
        /// <param name="pPixelData">The byte array.</param>
        /// <param name="pWidth">The width of the icon, which should be 32 pixels.</param>
        /// <param name="pHeight">The height of the icon, which should be 32 pixels.</param>
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public void  TabTitleBitmap(int TabIndex, out PowerMILL.PluginBitmapFormat pFormat, out byte[] pPixelData, out int pWidth, out int pHeight)
        {
            if (TabIndex < m_tabs.Count)
            {
                string title_bitmap = m_tabs[TabIndex].TabTitleBitmap;
                BitmapUtils.get_bitmap(PluginAssemblyName, title_bitmap, out pFormat, out pPixelData, out pWidth, out pHeight);
            }
            else
            {
                pFormat = PowerMILL.PluginBitmapFormat.PMILL_BITMAP_FORMAT_FAIL;
                pPixelData = null;
                pWidth = 0;
                pHeight = 0;
                throw new Exception("PaneTitleBitmap: Index out of range");
            }
        }

        #endregion
    }
}

