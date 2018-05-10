using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
namespace CTWH.Common.AppSettings
{
    public class AppSettingsBase
    {
        #region Data members
        protected System.Environment.SpecialFolder m_specialFolder = Environment.SpecialFolder.CommonApplicationData;
        protected bool m_isDefault = true;
        protected string m_fileName = "";
        protected string m_dataFilePath = "";
        protected string m_settingsFileComment = "";
        protected string m_settingsKeyName = "";
        protected XElement m_defaultSettings = null;

        #endregion Data members

        #region Properties
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the special folder where the settings file will be stored
        /// </summary>
        public System.Environment.SpecialFolder SpecialFolder
        {
            get { return m_specialFolder; }
            set { m_specialFolder = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the flag that indicates whether this is a "default" settings object
        /// </summary>
        public bool IsDefault
        {
            get { return m_isDefault; }
            set { m_isDefault = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the settings filename 
        /// </summary>
        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the comments placed in the settings file (optional)
        /// </summary>
        public string SettingsFileComment
        {
            get { return m_settingsFileComment; }
            set { m_settingsFileComment = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the key name for the user settings section
        /// </summary>
        public string SettingsKeyName
        {
            get { return m_settingsKeyName; }
            set { m_settingsKeyName = value; }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the base class' data. This will throw an exception that reminds you 
        /// that you have to override it (because properties cannot be made abstract).
        /// </summary>
        public virtual XElement XElement
        {
            get { throw new Exception("You must provide your own XElement property."); }
            set { throw new Exception("You must provide your own XElement property."); }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get/set the default settings data (as an XElement object)
        /// </summary>
        public XElement DefaultSettings
        {
            get { return m_defaultSettings; }
            set { m_defaultSettings = value; }
        }
        #endregion Properties

        #region Constructors
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Constructor - since we don't want the programmer to instantiate this class, 
        /// this constructor is protected (thanks Naveenth).
        /// </summary>
        /// <param name="appFolder">The name of the application settings folder</param>
        /// <param name="fileName">The name of the data file</param>
        protected AppSettingsBase(string appFolder, string fileName, string settingsKeyName,
                               string fileComment, XElement defaultSettings)
        {
            m_defaultSettings = defaultSettings;
            m_isDefault = (m_defaultSettings == null);
            m_fileName = fileName;
            m_settingsKeyName = settingsKeyName;
            m_settingsFileComment = fileComment;
            m_dataFilePath = CreateAppDataFolder(appFolder);
            if (!IsDefault && m_defaultSettings != null)
            {
                XElement = m_defaultSettings;
            }
        }
        #endregion Constructors

        #region Virtual methods
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Resets the user settings to the default settings
        /// </summary>
        /// <param name="defaultData">The data to be set</param>
        public virtual void Reset()
        {
            if (!IsDefault && m_defaultSettings != null)
            {
                XElement = m_defaultSettings;
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Loads the user settings, and then the default settings.
        /// </summary>
        public virtual void Load()
        {
            bool loaded = false;
            string fileName = System.IO.Path.Combine(m_dataFilePath, m_fileName);
            if (File.Exists(fileName))
            {
                try
                {
                    XDocument doc = XDocument.Load(fileName);
                    var settings = doc.Descendants(SettingsKeyName);
                    if (settings.Count() > 0)
                    {
                        foreach (XElement element in settings)
                        {
                            XElement = element;
                            loaded = true;
                            // just in case we have more than one, only take the first one
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (!loaded && !IsDefault && m_defaultSettings != null)
            {
                Reset();
            }
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Saves the user settings and the default settings
        /// </summary>
        public virtual void Save()
        {
            if (!IsDefault)
            {
                string fileName = System.IO.Path.Combine(m_dataFilePath, m_fileName);
                try
                {
                    XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                                                  new XComment(SettingsFileComment));
                    var root = new XElement("ROOT", XElement);
                    doc.Add(root);
                    doc.Save(fileName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Sets the user settings to the current default settings
        /// </summary>
        public virtual void SetAsDefaults(ref XElement element)
        {
            if (!IsDefault)
            {
                element = XElement;
            }
        }
        #endregion Virtual methods

        #region Utility methods
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Casts the specified integer to the appropriate enum ordinal. If all else fails, 
        /// the enum will be returned as the specified default ordinal.
        /// </summary>
        /// <param name="value">The integer value representing an enumeration element</param>
        /// <param name="deafaultValue">The default enumertion to be used if the specified "value" does not exist in the enumeration definition</param>
        /// <returns></returns>
        public static T IntToEnum<T>(int value, T defaultValue)
        {
            T enumValue = (Enum.IsDefined(typeof(T), value)) ? (T)(object)value : defaultValue;
            return enumValue;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Create (if necessary) the specified application data folder. This method 
        /// only creates the root folder, and will throw an exception if more than one 
        /// folder is specified.  For instance, "\MyApp" is valid, but 
        /// "\MyApp\MySubFolder" is not.
        /// </summary>
        /// <param name="folderName">A single folder name (can have a bcakslash at either or both ends).</param>
        /// <returns>The fully qualified path that was created (or that already exists)</returns>
        protected string CreateAppDataFolder(string folderName)
        {
            string appDataPath = "";
            string dataFilePath = "";

            folderName = folderName.Trim();
            if (folderName != "")
            {
                try
                {
                    // Set the directory where the file will come from.  The folder name 
                    // returned will be different between XP and Vista. Under XP, the default 
                    // folder name is "C:\Documents and Settings\All Users\Application Data\[folderName]"
                    // while under Vista, the folder name is "C:\Program Data\[folderName]".
                    appDataPath = System.Environment.GetFolderPath(this.SpecialFolder);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (folderName.Contains("\\"))
                {
                    string[] path = folderName.Split('\\');
                    int folderCount = 0;
                    int folderIndex = -1;
                    for (int i = 0; i < path.Length; i++)
                    {
                        string folder = path[i];
                        if (folder != "")
                        {
                            if (folderIndex == -1)
                            {
                                folderIndex = i;
                            }
                            folderCount++;
                        }
                    }
                    if (folderCount != 1)
                    {
                        throw new Exception("Invalid folder name specified (this function only creates the root app data folder for the application).");
                    }
                    folderName = path[folderIndex];
                }
            }
            if (folderName == "")
            {
                throw new Exception("Processed folder name resulted in an empty string.");
            }
            try
            {
                dataFilePath = System.IO.Path.Combine(appDataPath, folderName);
                if (!Directory.Exists(dataFilePath))
                {
                    Directory.CreateDirectory(dataFilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataFilePath;
        }
        #endregion Utility methods

    }
}
