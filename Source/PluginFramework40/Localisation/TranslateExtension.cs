//=============================================================================
//D The TranslateExtension class.
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
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace Delcam.Plugins.Localisation
{
    /// <summary>
    /// The Translate Markup extension returns a binding to a TranslationData
    /// that provides a translated resource of the specified key
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        #region Private Members

        private string m_key;

        #endregion

        #region Construction

        //=============================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public TranslateExtension(string key)
        {
            m_key = key;
        }

        #endregion

        [ConstructorArgument("key")]
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
        public string Key
        {
            get { return m_key; }
            set { m_key = value;}
        }

        //=============================================================================
        /// <summary>
        /// See <see cref="MarkupExtension.ProvideValue" />
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            ITranslationSupporter its = get_supporter(serviceProvider);
            if (its != null && its.Translator != null)
            {
                var binding = new Binding("Value")
                      {
                          Source = new TranslationData(its.Translator, m_key)
                      };
                return binding.ProvideValue(serviceProvider);
            }
            return null;
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
        private ITranslationSupporter get_supporter(IServiceProvider serviceProvider)
        {
            // The IServiceProvider is a very generic interface, but we're only interested in getting the 
            // IProvideValueTarget service, as this is what's used for XAML markup stuff
            IProvideValueTarget ipvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (ipvt != null)
            {
                // Get the target object, and check if it's a framework element, i.e. a control of some sort
                FrameworkElement fe = ipvt.TargetObject as FrameworkElement;
                if (fe != null)
                {
                    // We need to work our way up the heirarchy looking for any element that supports our
                    // ITranslationSupporter interface
                    ITranslationSupporter its = fe as ITranslationSupporter;
                    if (its != null) return its;

                    // Check the parent
                    FrameworkElement parent = fe.Parent as FrameworkElement;
                    while (parent != null)
                    {
                        fe = parent;
                        its = fe as ITranslationSupporter;
                        if (its != null) return its;
                        parent = fe.Parent as FrameworkElement;
                    }
                }
            }

            return null;
        }
    }
}
