//=============================================================================
//D The ITranslationProvider interface.
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
using System.Globalization;
using System.Linq;
using System.Text;

namespace Delcam.Plugins.Localisation
{
    public interface ITranslationProvider
    {
        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string Translate(string key);

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        /// <value>The available languages.</value>
        List<CultureInfo> Languages { get; }

        /// <summary>
        /// Gets or sets whether the 'Check' language should be used.
        /// </summary>
        bool UseCheckLanguage { set; get; }
    }
}
