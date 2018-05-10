using System;
using System.Collections.Generic;
using System.Text;

namespace CTWH.Common.AppSettings
{
    public class ChaintSettingsManager
    {
        private AppSettings m_userSettings = null;
		private AppSettings m_defaultSettings = null;

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the user settings object
		/// </summary>
		public AppSettings UserSettings
		{
			get { return m_userSettings; }
		}
		//--------------------------------------------------------------------------------
		/// <summary>
		/// Get the default settings object
		/// </summary>
		public AppSettings DefaultSettings
		{
			get { return m_defaultSettings; }
		}

		//--------------------------------------------------------------------------------
		/// <summary>
		/// Constructor
		/// </summary>
        public ChaintSettingsManager()
		{
			// create the default settings first
			m_defaultSettings = new AppSettings("DEMO_DEFAULT_SETTINGS", null);
			m_defaultSettings.Load();
			// so we can pass them to the user settings for initial setup
			m_userSettings = new AppSettings("DEMO_USER_SETTINGS", m_defaultSettings.XElement);
			m_userSettings.Load();
		}
    }
}
