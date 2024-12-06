//=============================================================================
//D The DisableDialogMessages class.
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
    /// An IDisposable class to temporarily disable dialog messages and errors while the class is within scope.
    /// </summary>
    public class DisableDialogMessages : IDisposable
    {
        // Private data members
        private IPluginCommunicationsInterface m_comms;
        private bool m_enable_errors = false;
        private bool m_enable_messages = false;

        //=============================================================================
        /// <summary>
        /// An enum to define the type of messages to disable.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public enum MessageType
        {
            Errors,
            Messages,
            Both
        }

        //=============================================================================
        /// <summary>
        /// DisableDialogMessages c'tor, disabling both types of message.
        /// </summary>
        /// <param name="comms">The IPluginCommunicationsInterface interface reference, allowing commands to be issued.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public DisableDialogMessages(IPluginCommunicationsInterface comms)
            : this(comms, MessageType.Both)
        {
        }

        //=============================================================================
        /// <summary>
        /// DisableDialogMessages c'tor, allowing the type of message to be disabled to be specified.
        /// </summary>
        /// <param name="comms">The IPluginCommunicationsInterface interface reference, allowing commands to be issued.</param>
        /// <param name="type">The type of message to disable.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public DisableDialogMessages(IPluginCommunicationsInterface comms, MessageType type)
        {
            // Cache the info we need to issue commands when we're disposed
            m_comms = comms;

            // Sanity check
            if (m_comms.Services == null) 
            {
                // We can't do anything if we can't issue commands...
                return;
            }

            // Do we wish errors to be temporarily disabled?
            if (type == MessageType.Errors || type == MessageType.Both)
            {
                // Check if messages are currently enabled
                if (m_comms.Services.RequestInformation("ErrorsDisplayed") == "true")
                {
                    // Mark the fact that we need to enable errors when we're disposed
                    m_enable_errors = true;

                    // Disable errors
                    m_comms.Services.InsertCommand(m_comms.Token, "DIALOGS ERROR OFF");
                }
            }

            // Do we wish messages to be temporarily disabled?
            if (type == MessageType.Messages || type == MessageType.Both)
            {
                // Check if messages are currently enabled
                if (m_comms.Services.RequestInformation("MessagesDisplayed") == "true")
                {
                    // Mark the fact that we need to enable errors when we're disposed
                    m_enable_messages = true;

                    // Disable errors
                    m_comms.Services.InsertCommand(m_comms.Token, "DIALOGS MESSAGE OFF");
                }
            }
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
            // Do we need to be enable errors?
            if (m_enable_errors)
            {
                m_comms.Services.InsertCommand(m_comms.Token, "DIALOGS ERROR ON");                
            }

            // Do we need to be enable messages?
            if (m_enable_messages)
            {
                m_comms.Services.InsertCommand(m_comms.Token, "DIALOGS MESSAGE ON");                
            }
        }

        #endregion
    }
}
