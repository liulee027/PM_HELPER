//=============================================================================
//D The TranslationData class.
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
using System.ComponentModel;

namespace Delcam.Plugins.Localisation
{
    public class TranslationData : IWeakEventListener, INotifyPropertyChanged
    {
        #region Private Members

        private string m_key;
        TranslationManager m_tm;

        #endregion

        //=============================================================================
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationData"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        //
        // History.
        // DICC  Who When     What
        // ----- --- -------- ---------------------------------------------------------
        // 93978 PSL 10/11/11 Written.
        //-----------------------------------------------------------------------------
        public TranslationData(TranslationManager tm, string key)
        {
            m_tm = tm;
            m_key = key;
            LanguageChangedEventManager.AddListener(m_tm, this);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="TranslationData"/> is reclaimed by garbage collection.
        /// </summary>
        ~TranslationData()
        {
            Dispose(false);
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
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Code to dispose the managed resources of the class
                LanguageChangedEventManager.RemoveListener(m_tm, this);
            }
            // Code to dispose the un-managed resources of the class
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
        public object Value
        {
            get
            {
                return m_tm.Translate(m_key);
            }
        }

        #region IWeakEventListener Members

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
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(LanguageChangedEventManager))
            {
                OnLanguageChanged(sender, e);
                return true;
            }
            return false;
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
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs("Value"));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
