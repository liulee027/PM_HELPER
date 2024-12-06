//=============================================================================
//D The XMLTranslationProvider class.
//
// ----------------------------------------------------------------------------
// Copyright 2011 Delcam plc., Birmingham, UK
// ----------------------------------------------------------------------------
//
// History.
// DICC  Who When     What
// ----- --- -------- ---------------------------------------------------------
// 93978 PSL 10/11/11 Written.
// 95005 PSL 12/01/12 Skip files that don't end in exactly .xml.
// 96285 PSL 28/03/12 Skip subsequent files with the same locale.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Windows;

namespace Delcam.Plugins.Localisation
{
    /// <summary>
    /// A translation provider, implementing the ITranslationProvider interface.
    /// </summary>
    public class XMLTranslationProvider : ITranslationProvider
    {
        // Private member data
        private Dictionary<string, XmlNode> m_language_files = new Dictionary<string, XmlNode>();
        private XmlNode m_current_dictionary_root;
        private Dictionary<string, string> m_current_dictionary = new Dictionary<string, string>();
        private static Dictionary<string, string> s_untranslated = new Dictionary<string, string>();

        //=============================================================================
        /// <summary>
        /// The XMLTranslationProvider c'tor.
        /// </summary>
        /// <param name="localisation_files_dir">The path to the localisation files.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        // 95005 PSL 12/01/12 Skip files that don't end in exactly .xml.
        // 96285 PSL 28/03/12 Skip subsequent files with the same locale.
        //-----------------------------------------------------------------------------
        public XMLTranslationProvider(string localisation_files_dir)
        {
            // Search the path for all translation files
            string[] files = null;
            try
            {
                files = System.IO.Directory.GetFiles(localisation_files_dir, "*.xml");
            }
            catch
            {
            }
            if (files != null)
            {
                int count = files.Count();
                foreach (string file in files)
                {
                    // Skip wierd stuff we wouldn't of expected to get in, e.g. something.xml~ or somthing.xml.old
                    if (file.Substring(file.Length - 4, 4) != ".xml")
                    {
                        continue;
                    }

                    // Load the language translation file
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    if (doc != null)
                    {
                        XmlNode root = doc.SubNode("languagedocument");
                        if (root != null)
                        {
                            XmlNode lang_node = root.Attributes.GetNamedItem("language");
                            if (lang_node != null && lang_node.InnerText.Length > 0 && !m_language_files.ContainsKey(lang_node.InnerText))
                            {
                                m_language_files.Add(lang_node.InnerText, root);
                            }
                        }
                    }
                }
            }
        }

        #region ITranslationProvider Members
        
        //=============================================================================
        /// <summary>
        /// The main translate function.
        /// </summary>
        /// <param name="phrase">The key or phrase to translate.</param>
        /// <returns>The translated phrase.</returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Translate(string phrase)
        {
            XmlNode root = select_langauge_node();
            if (root == null)
            {
                // Try to revert to the English translation
                root = select_langauge_node("en");
                if (root == null)
                {
                    // Failed to find any translation at all because no suitable file
                    return string.Format(">>{0}<<", phrase);
                }
            }

            return Translate(phrase, root);
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
        public List<CultureInfo> Languages
        {
            get
            {
                List<string> keys = m_language_files.Keys.ToList();
                List<CultureInfo> lci = new List<CultureInfo>();
                foreach (string key in keys)
                {
                    lci.Add(new CultureInfo("key"));
                }
                return lci;
            }
        }

        //=============================================================================
        /// <summary>
        /// Gets or sets whether the 'Check' language should be used. 
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public bool UseCheckLanguage
        {
            get;
            set;
        }

        #endregion

        //=============================================================================
        /// <summary>
        /// Called to translate the given phrase using the specified XML document node
        /// </summary>
        /// <param name="phrase">The key or phrase to be translated</param>
        /// <param name="root">The current XML document containing translated phrases.</param>
        /// <returns></returns>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public string Translate(string phrase, XmlNode root)
        {
            // Rebuild the dictionary, if the language has changed
            rebuild_dictionary(root);

            // Attempt the translation
            if (m_current_dictionary.ContainsKey(phrase))
            {
                // Return the translated phrase
                return m_current_dictionary[phrase];
            }
            else
            {
                // No translation found
                s_untranslated[phrase] = string.Format("\n  <message>\n    <english>{0}</english>\n    <translation>{0}</translation>\n  </message>", phrase);
                return string.Format(">{0}<", phrase);
            }
        }

        //=============================================================================
        /// <summary>
        /// Returns a string representing all the untranslated phrases encountered as an XML document.
        /// </summary>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static string UntranslatedPhrases
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (string s in s_untranslated.Values.ToList())
                {
                    result.Append(s);
                }
                return result.ToString();
            }
        }

        //=============================================================================
        /// <summary>
        /// Outputs an XML document containing all the untranslated phrases encountered.  The name will be based on
        /// the specified locale, e.g. will be of the form en-GB.xml.untranslated.
        /// </summary>
        /// <param name="plugin_install_path">The install path for the plugin.</param>
        /// <param name="locale">The locale in ISO form, e.g. "en-GB".</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public static void OutputUntranslatedPhrases(string plugin_install_path, string locale)
        {
            // Build the XML file
            StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<languagedocument language=\"");
            sb.Append(locale);
            sb.Append("\">");
            sb.Append(XMLTranslationProvider.UntranslatedPhrases);
            sb.Append("\n</languagedocument>\n");

            // Write it out to a file
            string translation_files_path = plugin_install_path + "/Localisation/" + locale + ".xml.untranslated";
            using (System.IO.StreamWriter stream_writer = new System.IO.StreamWriter(translation_files_path))
            {
                stream_writer.Write(sb.ToString());
            }
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
        private void rebuild_dictionary(XmlNode root)
        {
            // Rebuild the dictionary, if the language has changed
            if (root != m_current_dictionary_root)
            {
                m_current_dictionary_root = root;
                m_current_dictionary = new Dictionary<string, string>();
                string duplicates = "";
                foreach (XmlNode message_node in root.ChildNodes)
                {
                    //XmlNode message_node = root.SubNode("message");
                    if (message_node.Name == "message")
                    {
                        // Find the two sub nodes
                        XmlNode english_node = message_node.SubNode("english");
                        XmlNode translation_node = message_node.SubNode("translation");
                        if (english_node != null && translation_node != null)
                        {
                            if (m_current_dictionary.ContainsKey(english_node.InnerText))
                            {
                                duplicates += english_node.InnerText + "\n";
                            }
                            else 
                            {
                                m_current_dictionary[english_node.InnerText] = translation_node.InnerText;
                            }
                        }
                    }
                }

                // Report any duplicated
                if (duplicates.Length > 0) 
                {
                    MessageBox.Show(string.Format("The following duplicates were found in the translation file:\n\n{0}", duplicates));
                }
            }
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
        private XmlNode select_langauge_node()
        {
            // Get the current thread culture
            CultureInfo ci = Thread.CurrentThread.CurrentUICulture;

            // See if we can find a match
            return select_langauge_node(ci.Name);            
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
        private XmlNode select_langauge_node(string culture)
        {
            List<string> keys = m_language_files.Keys.ToList();

            // Do we need to override the language with the 'check' language for testing?
            if (UseCheckLanguage)
            {
                string check = "check";
                if (keys.Contains(check))
                {
                    return m_language_files[check];
                }
                else
                {
                    return null;
                }
            }

            if (culture.Length == 5 && culture[2]=='-')
            {
                // Attempt to find an exact match (i.e. language and culture)
                if (keys.Contains(culture))
                {
                    return m_language_files[culture];
                }
                
                // Attempt to find any document with the same language
                string language = "" + culture[0] + culture[1];
                return select_langauge_node(language);
            }
            else if (culture.Length == 2)
            {
                // Attempt to find any document with the same language
                string match = keys.Find(s => s.Substring(0, 2) == culture);
                if (match != null)
                {
                    return m_language_files[match];
                }
            }

            // No matches found
            return null;            
        }
    }

    public static class XmlNodeUtils 
    {
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
        public static XmlNode SubNode(this XmlNode node, string child_node_name)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == child_node_name) return child;
            }
            return null;
        }
    }
}
