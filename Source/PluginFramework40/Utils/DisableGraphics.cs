//=============================================================================
//D The DisableGraphics class.
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
using Delcam.Plugins.Framework;

namespace Delcam.Plugins.Utils
{
    /// <summary>
    /// An IDisposable class to temporarily disable normal graphical updates while the class is within scope.
    /// </summary>
    public class DisableGraphics : IDisposable
    {
        // Private data members
        private IPluginCommunicationsInterface m_comms;

        //=============================================================================
        /// <summary>
        /// The DisableGraphics c'tor.
        /// </summary>
        /// <param name="comms">The IPluginCommunicationsInterface interface reference, allowing commands to be issued.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public DisableGraphics(IPluginCommunicationsInterface comms)
        {
            // Cache the info we need to issue commands when we're disposed
            m_comms = comms;

            // Sanity check
            if (m_comms.Services == null) 
            {
                // We can't do anything if we can't issue commands...
                return;
            }

            // Disable the graphics
            m_comms.Services.InsertCommand(m_comms.Token, "GRAPHICS LOCK");
        }

        #region IDisposable Members

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
        public void Dispose()
        {
            // Enable the graphics
            m_comms.Services.InsertCommand(m_comms.Token, "GRAPHICS UNLOCK");
        }

        #endregion
    }
}
