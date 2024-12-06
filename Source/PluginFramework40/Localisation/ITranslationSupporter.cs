//=============================================================================
//D The ITranslationSupporter interface.
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

namespace Delcam.Plugins.Localisation
{
    public interface ITranslationSupporter
    {
        /// <summary>
        /// Gets the translation manager.
        /// </summary>
        TranslationManager Translator { get; }
    }
}
