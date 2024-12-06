//=============================================================================
//D The PluginFrameworkWithPanes class.
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
using Delcam.Plugins.Utils;
using Delcam.Plugins.Events;
using System.Windows;

namespace Delcam.Plugins.Framework
{
    /// <summary>
    /// This class is the base class for any plugins that require support for panes, and implements
    /// the IPowerMILLPlugin and PowerMILL.IPowerMILLPluginPane interfaces.
    /// </summary>
    public abstract class PluginFrameworkWithPanes : PluginFrameworkWithoutPanes, PowerMILL.IPowerMILLPluginPane
    {
        // Private data members
        private List<PaneDefinition> m_panes = new List<PaneDefinition>();
        private bool m_ok_to_register = true;

        //=============================================================================
        /// <summary>
        /// The PluginFrameworkWithPanes c'tor.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public PluginFrameworkWithPanes()
        {
        }

        // Abstract functions the derived class must implement
        //=============================================================================
        /// <summary>
        /// Called to ask the derived class to register any panes that it requires.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        protected abstract void register_panes();

        // Functions required by the derived class
        //=============================================================================
        /// <summary>
        /// Called to register panes.
        /// </summary>
        /// <param name="pane">The pane definition to be registered.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        protected void register_pane(PaneDefinition pane)
        {
            if (m_ok_to_register)
            {
                m_panes.Add(pane);
            }
            else
            {
                throw new Exception("Can't register new panes after PowerMILL has requested the total number of panes");
            }
        }

        // Private management functions
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
        private void uninitialise_panes()
        {
            foreach (PaneDefinition pd in m_panes)
            {
                pd.Uninitialise();
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
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public override void setup_framework(string token, PowerMILL.PluginServices services, int parent_window_hwnd)
        {
            base.setup_framework(token, services, parent_window_hwnd);
            register_panes();
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
        public override void shutdown_framework()
        {
            uninitialise_panes();
            base.shutdown_framework();
        } 

        // Implementation of IPowerMILLPluginPane        
        #region IPowerMILLPluginPane Members

        //=============================================================================
        /// <summary>
        /// Called to initialise the pane before displaying it.
        /// </summary>
        /// <param name="PaneIndex">The index of the pane being refered to.</param>
        /// <param name="ParentWindow">The HWND of the parent that the pane should use for rendering content.</param>
        /// <param name="Width">The width of the pane.</param>
        /// <param name="Height">The height of the pane.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void InitialisePane(int PaneIndex, int ParentWindow, int Width, int Height)
        {
            if (PaneIndex < m_panes.Count)
            {
                m_panes[PaneIndex].initialise_pane(ParentWindow, Width, Height);
            }
            else
            {
                throw new Exception("InitialisePane: Index out of range");
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns the number of panes that the plugin supports.  Panes cannot be registered after this
        /// function has been called.
        /// </summary>
        /// <returns>Returns the number of panes that the plugin supports</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public int NumberOfPanes()
        {
            m_ok_to_register = false;
            return m_panes.Count;
        }

        //=============================================================================
        /// <summary>
        /// Returns the height of the specified pane.
        /// </summary>
        /// <param name="PaneIndex">The pane being referenced.</param>
        /// <returns>Returns the height of the specified pane.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public int PaneHeight(int PaneIndex)
        {
            if (PaneIndex < m_panes.Count)
            {
                return m_panes[PaneIndex].PaneHeight;
            }
            else
            {
                throw new Exception("PaneHeight: Index out of range");
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns the minimum width of the specified pane.
        /// </summary>
        /// <param name="PaneIndex">The pane being referenced.</param>
        /// <returns>Returns the minimum width of the specified pane.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public int MinimumPaneWidth(int PaneIndex)
        {
            if (PaneIndex < m_panes.Count)
            {
                return m_panes[PaneIndex].MinimumPaneWidth;
            }
            else
            {
                throw new Exception("PaneHeight: Index out of range");
            }
        }

        //=============================================================================
        /// <summary>
        /// Called to inform the plugin that the specified pane has been resized.
        /// </summary>
        /// <param name="PaneIndex">The pane being referenced.</param>
        /// <param name="Width">The new width of the pane.</param>
        /// <param name="Height">The new height of the pane, which should remain constant.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void PaneResized(int PaneIndex, int Width, int Height)
        {
            if (PaneIndex < m_panes.Count)
            {
                m_panes[PaneIndex].pane_resized(Width, Height);
            }
            else
            {
                throw new Exception("PaneResized: Index out of range");
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns the title of the pane, which should be translated into the current locale.
        /// </summary>
        /// <param name="PaneIndex">The pane being referenced.</param>
        /// <returns>The tranlsated title of the pane.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string PaneTitle(int PaneIndex)
        {
            if (PaneIndex < m_panes.Count)
            {
                return m_panes[PaneIndex].PaneTitle;
            }
            else
            {
                throw new Exception("PaneTitle: Index out of range");
            }
        }

        //=============================================================================
        /// <summary>
        /// Called to obtain the pane icon.
        /// </summary>
        /// <param name="PaneIndex">The pane being referenced.</param>
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
        public void PaneTitleBitmap(int PaneIndex, out PowerMILL.PluginBitmapFormat pFormat, out byte[] pPixelData, out int pWidth, out int pHeight)
        {
            if (PaneIndex < m_panes.Count)
            {
                string title_bitmap = m_panes[PaneIndex].PaneTitleBitmap;
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
