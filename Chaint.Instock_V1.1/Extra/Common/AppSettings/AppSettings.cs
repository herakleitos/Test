using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
namespace CTWH.Common.AppSettings
{
    public class AppSettings : AppSettingsBase
    {
        #region Data members
        // used to seed the base constructor
        private const string APP_DATA_FOLDER = "SharedSettingsDemo";
        private const string APP_DATA_FILENAME = "Settings.xml";
        private const string FILE_COMMENT = "Demo Shared User Seettings";
        // actual data
        protected string m_name = "john";
        protected int m_age = 52;
        protected bool m_old = true;
        #endregion Data members

        #region Properties
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set name
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set age
        /// </summary>
        public int Age
        {
            get { return m_age; }
            set { m_age = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set old
        /// </summary>
        public bool Old
        {
            get { return m_old; }
            set { m_old = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set data as XElement object
        /// </summary>
        public override XElement XElement
        {
            get
            {
                return new XElement(m_settingsKeyName,
                                    new XElement("Name", Name),
                                    new XElement("Age", Age.ToString()),
                                    new XElement("Old", Old.ToString()));
            }
            set
            {
                Name = value.Element("Name").Value;
                Age = Convert.ToInt32(value.Element("Age").Value);
                Old = Convert.ToBoolean(value.Element("Old").Value);
            }
        }
        #endregion Properties

        #region Constructors
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isDefault"></param>
        /// <param name="value"></param>
        public AppSettings(string settingsKeyName, XElement element)
            : base(APP_DATA_FOLDER, APP_DATA_FILENAME, settingsKeyName, FILE_COMMENT, element)
        {
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isDefault"></param>
        /// <param name="value"></param>
        public AppSettings(string appDataFolder, string appDataFilename, string settingsKeyName, string fileComment, XElement element)
            : base(appDataFolder, appDataFilename, settingsKeyName, fileComment, element)
        {
        }
        #endregion Constructors

    }
}
