//=============================================================================
//D The TabDefinition class.
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
using System.Windows;
using System.Windows.Interop;

namespace Delcam.Plugins.Framework
{
    public class TabDefinition
    {
        private HwndSource m_hwnd_source;
        private FrameworkElement m_root_element;
        private int m_minimum_height;
        private string m_title;
        private string m_title_bitmap;

        //=============================================================================
        /// <summary>
        /// C'tor.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public TabDefinition(FrameworkElement root_element, int min_height, string title, string bitmap)
        {
            m_root_element = root_element;
            m_minimum_height = min_height;
            m_title = title;
            m_title_bitmap = bitmap;
        }

        //=============================================================================
        /// <summary>
        /// Initialise the pane, and create the HwndSource to hold the WPF content.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public void initialise_tab(int ParentWindow, int Width, int Height)
        {
            // Build the parameters required for the HwndSource object
            HwndSourceParameters sourceParams = new HwndSourceParameters("PowerMILLPlugin");
            sourceParams.PositionX = 0;
            sourceParams.PositionY = 0;
            sourceParams.Height = Height;
            sourceParams.Width = Width;
            sourceParams.ParentWindow = new IntPtr(ParentWindow);
            sourceParams.WindowStyle = 0x10000000 | 0x40000000; // WS_VISIBLE | WS_CHILD;   

            // Create the HwndSource object
            m_hwnd_source = new HwndSource(sourceParams);

            // Set the root visual
            m_hwnd_source.RootVisual = m_root_element;

            // Set the size of the Hwnd
            m_hwnd_source.SizeToContent = SizeToContent.WidthAndHeight;

            // Set the size of the control
            m_root_element.Width = Width;
            m_root_element.Height = Height;

            // Add a message hook - this is a workaround so hear about WM_CHAR messages
            m_hwnd_source.AddHook(child_hwnd_source_hook);
        }

        //=============================================================================
        /// <summary>
        /// Called when the tab is resized.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ------ --- -------- --------------------------------------------------------
        // <nnnn>  PSL 17/07/12 Written.
        //-----------------------------------------------------------------------------
        public void tab_resized(int Width, int Height)
        {
            // Resize the root element
            m_root_element.Width = Width;
            m_root_element.Height = Height;
        }

        //=============================================================================
        /// <summary>
        /// Get the minimum tab height.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public int MinimumTabHeight
        {
            get
            {
                return m_minimum_height;
            }
        }

        //=============================================================================
        /// <summary>
        /// Get the tab title.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public string TabTitle
        {
            get
            {
                return m_title;
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns the path of the tab icon in the resources.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public string TabTitleBitmap
        {
            get
            {
                return m_title_bitmap;
            }
        }

        //=============================================================================
        /// <summary>
        /// Uninitialise the tab.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public void Uninitialise()
        {
            m_root_element.Visibility = Visibility.Hidden;
            m_root_element = null;
            m_hwnd_source = null;
        }

        //=============================================================================
        /// <summary>
        /// Get the root FrameworkElement for the pane.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        public FrameworkElement RootElement { get { return m_root_element; } }

        //=============================================================================
        /// <summary>
        /// Message callback to work around MFC bug.
        /// </summary>
        //
        // History.
        // DICC   Who When     What
        // ------ --- -------- --------------------------------------------------------
        //  99199 PSL 02/11/12 Written.
        //-----------------------------------------------------------------------------
        private IntPtr child_hwnd_source_hook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // The standard message loop for MFC dialogs includes a call to the IsDialogMessage() function. 
            // This function actually handles certain keys, and thus returns TRUE to PreTranslateMessage, meaning
            // that WM_CHAR messages are never dispatched, and we don't hear about them.  The upshot is that
            // Textboxes etc. in managed apps when working with native code and interop don't recieve characters.
            // The workaround is to listen out for WM_GETDLGCODE, which is posted by IsDialogMessage() - in response,
            // we can shout 'Hey, we want to hear about chars!', so WM_CHAR messages do still get dispatched....
            if (msg == 0x0087) // WM_GETDLGCODE
            {
                //<nnn>AnotherTab.Tmp(msg.ToString());

                handled = true;
                return new IntPtr(0x0004); // DLGC_WANTALLKEYS          0x0080); // DLGC_WANTCHARS
            }

            return new IntPtr(0);
        }
    }
}
