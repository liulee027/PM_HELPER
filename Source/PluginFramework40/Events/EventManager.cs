//=============================================================================
//D The EventManager class.
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
using System.Xml;
using System.IO;
using System.Windows;
using Delcam.Plugins.Framework;

namespace Delcam.Plugins.Events
{
    //=============================================================================
    /// <summary>
    /// An event manager, which can subscribe to PowerMILL events, parse incomming XML events, 
    /// call the relevant delegates, and unsubscribe from event notification.
    /// </summary>
    //
    // History.
    // DICC  Who When     What
    // ----- --- -------- ---------------------------------------------------------
    // 93978 PSL 10/11/11 Written.
    //-----------------------------------------------------------------------------
    public class EventManager
    {
        List<EventSubscription> m_event_subscriptions = new List<EventSubscription>();
        IPluginCommunicationsInterface m_comms;

        //=============================================================================
        /// <summary>
        /// Event manager c'tor.
        /// </summary>
        /// <param name="comms">The PowerMILL communications interface</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public EventManager(IPluginCommunicationsInterface comms)
        {
            m_comms = comms;
        }

        //=============================================================================
        /// <summary>
        /// Subscribe to an event, given a subscription object that encapulates the request.
        /// </summary>
        /// <param name="sub">The subscription object, which contains the event name, any filtering requirements, and the event delegate function.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void Subscribe(EventSubscription sub)
        {
            // Add the subscription to our list
            m_event_subscriptions.Add(sub);

            // Ask PowerMILL to subscribe to this event
            int id;
            if (sub.Filtered)
            {
                id = m_comms.Services.SubscribeToFilteredEvent(m_comms.Token, sub.EventName, sub.FilterField, sub.FilterValue);
            }
            else
            {
                id = m_comms.Services.SubscribeToEvent(m_comms.Token, sub.EventName);
            }

            // Store the subscription id
            sub.SubscriptionID = id;
        }

        //=============================================================================
        /// <summary>
        /// Unsubscribe from the specified event
        /// </summary>
        /// <param name="sub">The event subscription object that was used when subscribing.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void Unsubscribe(EventSubscription sub)
        {
            // Add the subscription from our list
            m_event_subscriptions.Remove(sub);

            // Ask PowerMILL to cancel the subscription
            m_comms.Services.UnsubscribeFromEvent(m_comms.Token, sub.SubscriptionID);
        }

        //=============================================================================
        /// <summary>
        /// Called when PowerMILL broadcasts an event.  The XML event data will be parsed, and 
        /// passed to all delegate functions that are associated with this event.
        /// </summary>
        /// <param name="xml_event">The XML snippet containg event data.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void HandleEvent(string xml_event)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;

            XmlDocument doc = new XmlDocument();
            doc.Load(XmlReader.Create(new StringReader(xml_event), settings));

            // Get the root node
            XmlNode root = null;
            try
            {
                // Obtain the root node
                root = doc.DocumentElement;
            }
            catch (Exception)
            {
                // Just ignore it
                return;
            }

            // Get the name of the event
            string event_name = root.Name;

            // Find the event subscription ID
            int subscription_id = -1;
            XmlAttributeCollection attributes = root.Attributes;
            foreach (XmlAttribute attribute in attributes)
            {
                if (attribute.Name == "subscription_id")
                {
                    subscription_id = XmlConvert.ToInt32(attribute.Value);
                    break;
                }
            }
            if (subscription_id < 0)
            {
                // Don't know who to pass it on to
                return;
            }

            // Get the list of arguments
            Dictionary<string, string> event_arguments = new Dictionary<string, string>();                
            foreach (XmlNode child in root.ChildNodes)
            {
                event_arguments.Add(child.Name, child.InnerText);
            }

            // See if there's an event to handle this
            fire_event(event_name, event_arguments, subscription_id);            
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
        private void fire_event(string event_name, Dictionary<string, string> event_arguments, int subscription_id)
        {
            foreach (EventSubscription sub in m_event_subscriptions)
            {               
                if (sub.SubscriptionID == subscription_id)
                {
                    EventCallback callback = sub.CallbackFunction;
                    if (callback != null)
                    {
                        callback(event_name, event_arguments);
                    }
                }
            }
        }
    }
}
