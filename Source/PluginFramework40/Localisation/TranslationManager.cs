//=============================================================================
//D The TranslationManager class.
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
using System.Windows;
using System.Globalization;
using System.Threading;

namespace Delcam.Plugins.Localisation
{
    //=============================================================================
    /// <summary>
    /// The translation manager class.  Each plugin has it's own instance of the plugin manager.  The tranlsation manager
    /// does not actually do any translation, but rather has a 'translation provider' to do the work.
    /// </summary>
    //
    // History.
    // DICC  Who When     What
    // ----- --- -------- ---------------------------------------------------------
    // 93978 PSL 10/11/11 Written.
    //-----------------------------------------------------------------------------
    public class TranslationManager
    {
        // Private data members
        private static Dictionary<Guid, TranslationManager> m_translation_managers = new Dictionary<Guid, TranslationManager>();
        private ITranslationProvider m_translation_provider;
        private bool m_use_check_lang = false;

        // Events
        public event EventHandler LanguageChanged;

        //=============================================================================
        /// <summary>
        /// Gets or sets whether the 'Check' language should be used.  This basically sets the flag on the underlying
        /// translation provider.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public bool UseCheckLanguage { 
            get
            {
                return m_use_check_lang;
            }
            set
            {
                m_use_check_lang = value;
                ITranslationProvider itp = TranslationProvider;
                if (itp != null)
                {
                    itp.UseCheckLanguage = m_use_check_lang;
                }
            }
        }

        //=============================================================================
        /// <summary>
        /// Gets or sets the current language.  An event is fired when the language changes.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if( value != Thread.CurrentThread.CurrentUICulture)
                {
                    Thread.CurrentThread.CurrentUICulture = value;
                    OnLanguageChanged();
                }
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns a list of the languages supported by the current translation provider.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public IEnumerable<CultureInfo> Languages
        {
            get
            {
               if( TranslationProvider != null)
               {
                   return TranslationProvider.Languages;
               }
               return Enumerable.Empty<CultureInfo>();
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns an instance of the translation manager for the specified plugin.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static TranslationManager Instance(Guid guid)
        {
            // Create a translation manager if it doesn't already exist
            if (!m_translation_managers.ContainsKey(guid))
            {
                m_translation_managers[guid] = new TranslationManager();
            }

            // Return the translation manager
            return m_translation_managers[guid];
        }

        //=============================================================================
        /// <summary>
        /// Returns the translation provider.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public ITranslationProvider TranslationProvider
        {
            get
            {
                return m_translation_provider;
            }
            set
            {
                m_translation_provider = value;
                m_translation_provider.UseCheckLanguage = UseCheckLanguage;
            }
        }        

        //=============================================================================
        /// <summary>
        /// Called to translate a given key using the current translation provider
        /// </summary>
        /// <param name="key">The key or phrase to translate.</param>
        /// <returns>The translated phrase.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Translate(string key)
        {
            if( TranslationProvider!= null)
            {
                string translatedValue = TranslationProvider.Translate(key);
                if( translatedValue != null)
                {
                    return translatedValue;
                }
            }
            return string.Format("!{0}!", key);
        }

        //=============================================================================
        /// <summary>
        /// Sets the locale given an ISO string, e.g. en-GB.
        /// </summary>
        /// <param name="locale">The ISO formatted locale in the format "xx-XX" specifying the language and Country, e.g. "en-GB".</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void set_locale_from_iso_string(string locale)
        {
            if (locale == "check")
            {
                UseCheckLanguage = true;
                CurrentLanguage = new CultureInfo("en-GB");
            }
            else
            {
                UseCheckLanguage = false;
                CultureInfo ci;
                try
                {
                    ci = new CultureInfo(locale);
                }
                catch
                {
                    // Unknown culture
                    ci = new CultureInfo("en-GB");
                }
                CurrentLanguage = ci;
            }
        }

        //=============================================================================
        /// <summary>
        /// An event callback function, which parses the "LocaleChanged" event for changes and calls the relevant 
        /// function to modify the current locale accordingly.
        /// </summary>
        /// <param name="event_name"></param>
        /// <param name="event_arguments"></param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public void locale_changed(string event_name, Dictionary<string, string> event_arguments)
        {
            string locale = event_arguments["Locale"];
            set_locale_from_iso_string(locale);
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
        private void OnLanguageChanged()
        {
            if (LanguageChanged != null)
            {
                LanguageChanged(this, EventArgs.Empty);
            }
        }
    }
}
