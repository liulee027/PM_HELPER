//=============================================================================
//D The EventSubscription class.
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
    /// The delegate for event callbacks.
    /// </summary>
    /// <param name="event_name">The name of the event that was subscribed to.</param>
    /// <param name="event_arguments">A dictionary of key/values, which conatins the list of event fields and corresponding data.</param>
    public delegate void EventCallback(string event_name, Dictionary<string, string> event_arguments);

    //=============================================================================
    /// <summary>
    /// A class that encapsulates an event subscription, including teh event name, filtering, and delegate callbacks.
    /// </summary>
    //
    // History.
    // DICC  Who When     What
    // ----- --- -------- ---------------------------------------------------------
    // 93978 PSL 10/11/11 Written.
    //-----------------------------------------------------------------------------
    public class EventSubscription
    {
        private string m_event_name;
        private string m_filter_field = "";
        private string m_filter_value = "";
        private EventCallback m_callback_function = null;
        private int m_event_subscription_id;

        //=============================================================================
        /// <summary>
        /// Constructs an event subscription object without filtering.
        /// </summary>
        /// <param name="event_name">The name of the event being subscribed to.</param>
        /// <param name="callback">The function delgate that will be called when a matching event occurs, with all the event data.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public EventSubscription(string event_name, EventCallback callback)
        {
            m_event_name = event_name;
            m_callback_function = callback;
        }
        //=============================================================================
        /// <summary>
        /// Constructs an event subscription object without filtering.
        /// </summary>
        /// <param name="event_name">The name of the event being subscribed to.</param>
        /// <param name="filter_field">The name of the filter field.</param>
        /// <param name="filter_value">The data within the specified field that must match the event.</param>
        /// <param name="callback">The function delgate that will be called when a matching event occurs, with all the event data.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public EventSubscription(string event_name, string filter_field, string filter_value, EventCallback callback)
        {
            m_event_name = event_name;
            m_filter_field = filter_field;
            m_filter_value = filter_value;
            m_callback_function = callback;
        }

        //=============================================================================
        /// <summary>
        /// The name of the event.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string EventName { get { return m_event_name; } }
        //=============================================================================
        /// <summary>
        /// The name of the filter field, if filtering.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string FilterField { get { return m_filter_field; } }
        //=============================================================================
        /// <summary>
        /// The filter value.  Only events whose filter field matches this value will be broadcast by PowerMILL.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string FilterValue { get { return m_filter_value; } }
        //=============================================================================
        /// <summary>
        /// The delegate that if called when a matching event is broadcast.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public EventCallback CallbackFunction { get { return m_callback_function; } }
        //=============================================================================
        /// <summary>
        /// Returns true if a filter has been specified.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public bool Filtered { get { return m_filter_field.Length > 0; } }
        //=============================================================================
        /// <summary>
        /// The subscription ID that PowerMILL returned when the event was actually registered.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public int SubscriptionID 
        { 
            get { return m_event_subscription_id; } 
            set { m_event_subscription_id = value; } 
        }
    }
}
