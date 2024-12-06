//=============================================================================
//D The EventInterface class.
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

namespace Delcam.Plugins.Events
{
    /// <summary>
    /// An interface for classes that can support handling of XML based events.
    /// </summary>
    interface EventInterface
    {
        /// <summary>
        /// Handle the event, which will be an XML snippet.
        /// </summary>
        /// <param name="xml_event">An XML snippet contaning the event information.</param>
        void HandleEvents(string xml_event);
    }
}
