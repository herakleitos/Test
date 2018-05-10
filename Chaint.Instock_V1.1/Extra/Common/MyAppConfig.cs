using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Data;
using System.Reflection;
using System.Security;
using Microsoft.Win32;
using System.Windows.Forms;
namespace Common
{  
  
    /// <summary>
    ///   Profile class that utilizes an XML-formatted .config file to retrieve and save its data. </summary>
    /// <remarks>
    ///   Config files are used by Windows and Web apps to store application-specific configuration information.
    ///   The System.Configuration namespace contains a variety of classes that may be used to retrieve the data
    ///   from config files; however there is no provision for writing to such files.  The reason: they're only 
    ///   meant to be read by the program, not written.  For this reason, I initially considered not writing a 
    ///   Profile class for config files.  Instead, I created a separate <see cref="Xml" /> class that stores 
    ///   profile data in its own XML format, meant for a separate file.  Although that is the preferred choice, 
    ///   there may still be some developers who, for whatever reason, need a way to write to config files at 
    ///   run-time.  If you're one of those, this class is for you.
    ///   <para> 
    ///   By default this class formats the data inside the config file as follows.  
    ///   (Notice that XML elements cannot contain spaces so this class converts them to underscores.) </para> 
    ///   <code> 
    ///   &lt;configuration&gt;
    ///     &lt;configSections&gt; 
    ///       &lt;sectionGroup name="profile"&gt;
    ///         &lt;section name="A_Section" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null" /&gt;
    ///         &lt;section name="Another_Section" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null" /&gt;
    ///       &lt;/sectionGroup&gt;
    ///     &lt;/configSections&gt;
    ///     &lt;appSettings&gt;
    ///       &lt;add key="App Entry" value="App Value" /&gt;
    ///     &lt;/appSettings&gt;
    ///     &lt;profile&gt;
    ///       &lt;A_Section&gt;
    ///         &lt;add key="An Entry" value="Some Value" /&gt;
    ///         &lt;add key="Another Entry" value="Another Value" /&gt;
    ///       &lt;/A_Section&gt;
    ///       &lt;Another_Section&gt;
    ///         &lt;add key="This is cool" value="True" /&gt;
    ///       &lt;/Another_Section&gt;
    ///     &lt;/profile&gt;
    ///   &lt;/configuration&gt;
    ///   </code>
    ///   <para> 
    ///   If you wanted to read the value of "A_Section/An Entry" using the System.Configuration classes, you'd do it using the following code: </para>
    ///   <code> 
    ///   NameValueCollection section = (NameValueCollection)ConfigurationSettings.GetConfig("profile/A_Section");
    ///   string value = section["An Entry"];
    ///   </code>
    ///   <para> 
    ///   One thing to keep in mind is that .NET caches the config data as it reads it, so any subsequent 
    ///   updates to it on the file will not be seen by the System.Configuration classes, at least for Windows apps.
    ///   The Config class, however, has no such problem since the data is read from the file every time,
    ///   unless <see cref="XmlBased.Buffering" /> is enabled.
    ///   The equivalent of the above code would look like this: </para>
    ///   <code> 
    ///   Config config = new Config();
    ///   string value = config.GetValue("A Section", "An Entry", null);
    ///   </code> 
    ///   <para> 
    ///   As a bonus, you may use the Config class to access the "appSettings" section by clearing the
    ///   GroupName property.  Here's an example: </para>
    ///   <code> 
    ///   Config config = new Config();
    ///   config.GroupName = null;  // don't use a section group
    ///   ...
    ///   string value = config.GetValue("appSettings", "App Entry", null);
    ///   config.SetValue("appSettings", "Update Date", DateTime.Today);
    ///   </code>
    ///   </remarks>
    public class MyAppConfig : XmlBased
    {
        // Fields
        private string m_groupName = "profile";
        private const string SECTION_TYPE = "System.Configuration.NameValueSectionHandler, System, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null";

        /// <summary>
        ///   Initializes a new instance of the Config class by setting the <see cref="Profile.Name" /> to <see cref="Profile.DefaultName" />. </summary>
        public MyAppConfig()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the Config class by setting the <see cref="Profile.Name" /> to the given file name. </summary>
        /// <param name="fileName">
        ///   The name of the Config file to initialize the <see cref="Profile.Name" /> property with. </param>
        public MyAppConfig(string fileName)
            :
            base(fileName)
        {
            _DefaultName = fileName;
        }

        /// <summary>
        ///   Initializes a new instance of the Config class based on another Config object. </summary>
        /// <param name="config">
        ///   The Config object whose properties and events are used to initialize the object being constructed. </param>
        public MyAppConfig(MyAppConfig config)
            :
            base(config)
        {
            m_groupName = config.m_groupName;
        }

        /// <summary>
        ///   Gets the default name for the Config file. </summary>
        /// <remarks>
        ///   For Windows apps, this property returns the name of the executable plus .config ("program.exe.config").
        ///   For Web apps, this property returns the full path of the <i>web.config</i> file.
        ///   This property is used to set the <see cref="Profile.Name" /> property inside the default constructor.</remarks>
        public override string DefaultName
        {
            get
            {
                if (_DefaultName == "")
                    _DefaultName = DefaultNameWithoutExtension + ".config";
                return _DefaultName;
            }
           
          
        } 
        private string _DefaultName = "";


        /// <summary>
        ///   Retrieves a copy of itself. </summary>
        /// <returns>
        ///   The return value is a copy of itself as an object. </returns>
        /// <seealso cref="Profile.CloneReadOnly" />
        public override object Clone()
        {
            return new MyAppConfig(this);
        }

        /// <summary>
        ///   Gets or sets the name of the element under which all sections should be located. </summary>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="XmlException">
        ///   The value being set contains an namespace prefix (eg, <b>prefix:</b>whatever). </exception>
        /// <remarks>
        ///   By default this property is set to "profile".  This means that the sections come as
        ///   descendants of "configuration\profile".  However, this property may be set to null so that
        ///   all sections can be placed directly under "configuration".  This is useful for reading/writing
        ///   the popular "appSettings" section, which may also be retrieved via the System.Configuration.ConfigurationSettings.AppSettings property.
        ///   <para>The <see cref="Profile.Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without changing this property.  After the property has been changed, 
        ///   the <see cref="Profile.Changed" /> event is raised.</para> </remarks>
        public string GroupName
        {
            get
            {
                return m_groupName;
            }
            set
            {
                VerifyNotReadOnly();
                if (m_groupName == value)
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "GroupName", value))
                    return;

                m_groupName = value;
                if (m_groupName != null)
                {
                    m_groupName = m_groupName.Replace(' ', '_');

                    if (m_groupName.IndexOf(':') >= 0)
                        throw new XmlException("GroupName may not contain a namespace prefix.");
                }

                RaiseChangeEvent(false, ProfileChangeType.Other, null, "GroupName", value);
            }
        }

        /// <summary>
        ///   Gets whether we have a valid GroupName. </summary>
        private bool HasGroupName
        {
            get
            {
                return m_groupName != null && m_groupName != "";
            }
        }

        /// <summary>
        ///   Gets the name of the GroupName plus a slash or an empty string is HasGroupName is false. </summary>
        /// <remarks>
        ///   This property helps us when retrieving sections. </remarks>
        private string GroupNameSlash
        {
            get
            {
                return (HasGroupName ? (m_groupName + "/") : "");
            }
        }

        /// <summary>
        ///   Retrieves whether we don't have a valid GroupName and a given section is 
        ///   equal to "appSettings". </summary>
        /// <remarks>
        ///   This method helps us determine whether we need to deal with the "configuration\configSections" element. </remarks>
        private bool IsAppSettings(string section)
        {
            return !HasGroupName && section != null && section == "appSettings";
        }

        /// <summary>
        ///   Verifies the given section name is not null and trims it. </summary>
        /// <param name="section">
        ///   The section name to verify and adjust. </param>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method first calls <see cref="Profile.VerifyAndAdjustSection">Profile.VerifyAndAdjustSection</see> 
        ///   and then replaces any spaces in the section with underscores.  This is needed 
        ///   because XML element names may not contain spaces.  </remarks>
        /// <seealso cref="Profile.VerifyAndAdjustEntry" />
        protected override void VerifyAndAdjustSection(ref string section)
        {
            base.VerifyAndAdjustSection(ref section);
            if (section.IndexOf(' ') >= 0)
                section = section.Replace(' ', '_');
        }

        /// <summary>
        ///   Sets the value for an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry where the value will be set. </param>
        /// <param name="value">
        ///   The value to set. If it's null, the entry is removed. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.Name" /> is null or empty, 
        ///   <see cref="Profile.ReadOnly" /> is true, or
        ///   the resulting XML document is invalid. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file or
        ///	  the resulting XML document would not be well formed. </exception>
        /// <remarks>
        ///   If the Config file does not exist, it is created.
        ///   The <see cref="Profile.Changing" /> event is raised before setting the value.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without setting the value.  After the value has been set, 
        ///   the <see cref="Profile.Changed" /> event is raised.
        ///   <para>
        ///   Note: If <see cref="XmlBased.Buffering" /> is enabled, the value is not actually written to the
        ///   Config file until the buffer is flushed (or closed). </para></remarks>
        /// <seealso cref="GetValue" />
        public override void SetValue(string section, string entry, object value)
        {
            // If the value is null, remove the entry
            if (value == null)
            {
                RemoveEntry(section, entry);
                return;
            }

            VerifyNotReadOnly();
            VerifyName();
            VerifyAndAdjustSection(ref section);
            VerifyAndAdjustEntry(ref entry);

            if (!RaiseChangeEvent(true, ProfileChangeType.SetValue, section, entry, value))
                return;

            bool hasGroupName = HasGroupName;
            bool isAppSettings = IsAppSettings(section);

            // If the file does not exist, use the writer to quickly create it
            if ((m_buffer == null || m_buffer.IsEmpty) && !File.Exists(Name))
            {
                XmlTextWriter writer = null;

                // If there's a buffer, write to it without creating the file
                if (m_buffer == null)
                    writer = new XmlTextWriter(Name, Encoding);
                else
                    writer = new XmlTextWriter(new MemoryStream(), Encoding);

                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();

                writer.WriteStartElement("configuration");
                if (!isAppSettings)
                {
                    writer.WriteStartElement("configSections");
                    if (hasGroupName)
                    {
                        writer.WriteStartElement("sectionGroup");
                        writer.WriteAttributeString("name", null, m_groupName);
                    }
                    writer.WriteStartElement("section");
                    writer.WriteAttributeString("name", null, section);
                    writer.WriteAttributeString("type", null, SECTION_TYPE);
                    writer.WriteEndElement();

                    if (hasGroupName)
                        writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                if (hasGroupName)
                    writer.WriteStartElement(m_groupName);
                writer.WriteStartElement(section);
                writer.WriteStartElement("add");
                writer.WriteAttributeString("key", null, entry);
                writer.WriteAttributeString("value", null, value.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                if (hasGroupName)
                    writer.WriteEndElement();
                writer.WriteEndElement();

                if (m_buffer != null)
                    m_buffer.Load(writer);
                writer.Close();

                RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
                return;
            }

            // The file exists, edit it

            XmlDocument doc = GetXmlDocument();
            XmlElement root = doc.DocumentElement;

            XmlAttribute attribute = null;
            XmlNode sectionNode = null;

            // Check if we need to deal with the configSections element
            if (!isAppSettings)
            {
                // Get the configSections element and add it if it's not there
                XmlNode sectionsNode = root.SelectSingleNode("configSections");
                if (sectionsNode == null)
                    sectionsNode = root.AppendChild(doc.CreateElement("configSections"));

                XmlNode sectionGroupNode = sectionsNode;
                if (hasGroupName)
                {
                    // Get the sectionGroup element and add it if it's not there
                    sectionGroupNode = sectionsNode.SelectSingleNode("sectionGroup[@name=\"" + m_groupName + "\"]");
                    if (sectionGroupNode == null)
                    {
                        XmlElement element = doc.CreateElement("sectionGroup");
                        attribute = doc.CreateAttribute("name");
                        attribute.Value = m_groupName;
                        element.Attributes.Append(attribute);
                        sectionGroupNode = sectionsNode.AppendChild(element);
                    }
                }

                // Get the section element and add it if it's not there
                sectionNode = sectionGroupNode.SelectSingleNode("section[@name=\"" + section + "\"]");
                if (sectionNode == null)
                {
                    XmlElement element = doc.CreateElement("section");
                    attribute = doc.CreateAttribute("name");
                    attribute.Value = section;
                    element.Attributes.Append(attribute);

                    sectionNode = sectionGroupNode.AppendChild(element);
                }

                // Update the type attribute
                attribute = doc.CreateAttribute("type");
                attribute.Value = SECTION_TYPE;
                sectionNode.Attributes.Append(attribute);
            }

            // Get the element with the sectionGroup name and add it if it's not there
            XmlNode groupNode = root;
            if (hasGroupName)
            {
                groupNode = root.SelectSingleNode(m_groupName);
                if (groupNode == null)
                    groupNode = root.AppendChild(doc.CreateElement(m_groupName));
            }

            // Get the element with the section name and add it if it's not there
            sectionNode = groupNode.SelectSingleNode(section);
            if (sectionNode == null)
                sectionNode = groupNode.AppendChild(doc.CreateElement(section));

            // Get the 'add' element and add it if it's not there
            XmlNode entryNode = sectionNode.SelectSingleNode("add[@key=\"" + entry + "\"]");
            if (entryNode == null)
            {
                XmlElement element = doc.CreateElement("add");
                attribute = doc.CreateAttribute("key");
                attribute.Value = entry;
                element.Attributes.Append(attribute);

                entryNode = sectionNode.AppendChild(element);
            }

            // Update the value attribute
            attribute = doc.CreateAttribute("value");
            attribute.Value = value.ToString();
            entryNode.Attributes.Append(attribute);

            // Save the file
            Save(doc);
            RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
        }

        /// <summary>
        ///   Retrieves the value of an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value is the entry's value, or null if the entry does not exist. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file. </exception>
        /// <exception cref="NullReferenceException">
        ///   'value' attribute is missing from the entry node. </exception>
        /// <seealso cref="SetValue" />
        /// <seealso cref="Profile.HasEntry" />
        public override object GetValue(string section, string entry)
        {
            VerifyAndAdjustSection(ref section);
            VerifyAndAdjustEntry(ref entry);

            try
            {
                XmlDocument doc = GetXmlDocument();
                XmlElement root = doc.DocumentElement;

                XmlNode entryNode = root.SelectSingleNode(GroupNameSlash + section + "/add[@key=\"" + entry + "\"]");
                return entryNode.Attributes["value"].Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///   Removes an entry from a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty or
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file or
        ///	  the resulting XML document would not be well formed. </exception>
        /// <remarks>
        ///   The <see cref="Profile.Changing" /> event is raised before removing the entry.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without removing the entry.  After the entry has been removed, 
        ///   the <see cref="Profile.Changed" /> event is raised. 
        ///   <para>
        ///   Note: If <see cref="XmlBased.Buffering" /> is enabled, the entry is not removed from the
        ///   Config file until the buffer is flushed (or closed). </para></remarks>
        /// <seealso cref="RemoveSection" />
        public override void RemoveEntry(string section, string entry)
        {
            VerifyNotReadOnly();
            VerifyAndAdjustSection(ref section);
            VerifyAndAdjustEntry(ref entry);

            // Verify the document exists
            XmlDocument doc = GetXmlDocument();
            if (doc == null)
                return;

            // Get the entry's node, if it exists
            XmlElement root = doc.DocumentElement;
            XmlNode entryNode = root.SelectSingleNode(GroupNameSlash + section + "/add[@key=\"" + entry + "\"]");
            if (entryNode == null)
                return;

            if (!RaiseChangeEvent(true, ProfileChangeType.RemoveEntry, section, entry, null))
                return;

            entryNode.ParentNode.RemoveChild(entryNode);
            Save(doc);
            RaiseChangeEvent(false, ProfileChangeType.RemoveEntry, section, entry, null);
        }

        /// <summary>
        ///   Removes a section. </summary>
        /// <param name="section">
        ///   The name of the section to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty or
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file or
        ///	  the resulting XML document would not be well formed. </exception>
        /// <remarks>
        ///   The <see cref="Profile.Changing" /> event is raised before removing the section.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without removing the section.  After the section has been removed, 
        ///   the <see cref="Profile.Changed" /> event is raised.
        ///   <para>
        ///   Note: If <see cref="XmlBased.Buffering" /> is enabled, the section is not removed from the
        ///   Config file until the buffer is flushed (or closed). </para></remarks>
        /// <seealso cref="RemoveEntry" />
        public override void RemoveSection(string section)
        {
            VerifyNotReadOnly();
            VerifyAndAdjustSection(ref section);

            // Verify the document exists
            XmlDocument doc = GetXmlDocument();
            if (doc == null)
                return;

            // Get the root node, if it exists
            XmlElement root = doc.DocumentElement;
            if (root == null)
                return;

            // Get the section's node, if it exists
            XmlNode sectionNode = root.SelectSingleNode(GroupNameSlash + section);
            if (sectionNode == null)
                return;

            if (!RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, null, null))
                return;

            sectionNode.ParentNode.RemoveChild(sectionNode);

            // Delete the configSections entry also			
            if (!IsAppSettings(section))
            {
                sectionNode = root.SelectSingleNode("configSections/" + (HasGroupName ? ("sectionGroup[@name=\"" + m_groupName + "\"]") : "") + "/section[@name=\"" + section + "\"]");
                if (sectionNode == null)
                    return;

                sectionNode.ParentNode.RemoveChild(sectionNode);
            }

            Save(doc);
            RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, null, null);
        }

        /// <summary>
        ///   Retrieves the names of all the entries inside a section. </summary>
        /// <param name="section">
        ///   The name of the section holding the entries. </param>
        /// <returns>
        ///   If the section exists, the return value is an array with the names of its entries; 
        ///   otherwise it's null. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file. </exception>
        /// <seealso cref="Profile.HasEntry" />
        /// <seealso cref="GetSectionNames" />
        public override string[] GetEntryNames(string section)
        {
            // Verify the section exists
            if (!HasSection(section))
                return null;

            VerifyAndAdjustSection(ref section);
            XmlDocument doc = GetXmlDocument();
            XmlElement root = doc.DocumentElement;

            // Get the entry nodes
            XmlNodeList entryNodes = root.SelectNodes(GroupNameSlash + section + "/add[@key]");
            if (entryNodes == null)
                return null;

            // Add all entry names to the string array			
            string[] entries = new string[entryNodes.Count];
            int i = 0;

            foreach (XmlNode node in entryNodes)
                entries[i++] = node.Attributes["key"].Value;

            return entries;
        }

        /// <summary>
        ///   Retrieves the names of all the sections. </summary>
        /// <returns>
        ///   If the Config file exists, the return value is an array with the names of all the sections;
        ///   otherwise it's null. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file. </exception>
        /// <seealso cref="Profile.HasSection" />
        /// <seealso cref="GetEntryNames" />
        public override string[] GetSectionNames()
        {
            // Verify the document exists
            XmlDocument doc = GetXmlDocument();
            if (doc == null)
                return null;

            // Get the root node, if it exists
            XmlElement root = doc.DocumentElement;
            if (root == null)
                return null;

            // Get the group node
            XmlNode groupNode = (HasGroupName ? root.SelectSingleNode(m_groupName) : root);
            if (groupNode == null)
                return null;

            // Get the section nodes
            XmlNodeList sectionNodes = groupNode.ChildNodes;
            if (sectionNodes == null)
                return null;

            // Add all section names to the string array			
            string[] sections = new string[sectionNodes.Count];
            int i = 0;

            foreach (XmlNode node in sectionNodes)
                sections[i++] = node.Name;

            return sections;
        }
    }




    /// <summary>
    ///   Abstract base class for all XML-based Profile classes. </summary>
    /// <remarks>
    ///   This class provides common methods and properties for the XML-based Profile classes 
    ///   (<see cref="Xml" />, <see cref="Config" />). </remarks>
    public abstract class XmlBased : Profile
    {
        private Encoding m_encoding = Encoding.UTF8;
        internal XmlBuffer m_buffer;

        /// <summary>
        ///   Initializes a new instance of the XmlBased class by setting the <see cref="Profile.Name" /> to <see cref="Profile.DefaultName" />. </summary>
        protected XmlBased()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the XmlBased class by setting the <see cref="Profile.Name" /> to the given file name. </summary>
        /// <param name="fileName">
        ///   The name of the file to initialize the <see cref="Profile.Name" /> property with. </param>
        protected XmlBased(string fileName)
            :
            base(fileName)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the XmlBased class based on another XmlBased object. </summary>
        /// <param name="profile">
        ///   The XmlBased profile object whose properties and events are used to initialize the object being constructed. </param>
        protected XmlBased(XmlBased profile)
            :
            base(profile)
        {
            m_encoding = profile.Encoding;
        }

        /// <summary>
        ///   Retrieves an XmlDocument object based on the <see cref="Profile.Name" /> of the file. </summary>
        /// <returns>
        ///   If <see cref="Buffering" /> is not enabled, the return value is the XmlDocument object loaded with the file, 
        ///   or null if the file does not exist. If <see cref="Buffering" /> is enabled, the return value is an 
        ///   XmlDocument object, which will be loaded with the file if it already exists.</returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file. </exception>
        protected XmlDocument GetXmlDocument()
        {
            if (m_buffer != null)
                return m_buffer.XmlDocument;

            VerifyName();
            if (!File.Exists(Name))
                return null;

            XmlDocument doc = new XmlDocument();
            doc.Load(Name);
            return doc;
        }

        /// <summary>
        ///   Saves any changes pending on an XmlDocument object, unless <see cref="Buffering" /> is enabled. </summary>
        /// <exception cref="XmlException">
        ///	  The resulting XML document would not be well formed. </exception>
        /// <remarks>
        ///   If <see cref="Buffering" /> is enabled, this method sets the <see cref="XmlBuffer.NeedsFlushing" /> property to true 
        ///   and the changes are not saved until the buffer is flushed (or closed).  If the Buffer is not active
        ///   the contents of the XmlDocument object are saved to the file. </remarks>
        protected void Save(XmlDocument doc)
        {
            if (m_buffer != null)
                m_buffer.m_needsFlushing = true;
            else
                doc.Save(Name);

        }

        /// <summary>
        ///   Activates buffering on this XML-based profile object, if not already active. </summary>
        /// <param name="lockFile">
        ///   If true, the file is locked when the buffer is activated so that no other processes can write to it.  
        ///   If false, other processes can continue writing to it and the actual contents of the file can get 
        ///   out of synch with the contents of the buffer. </param>
        /// <returns>
        ///   The return value is an <see cref="XmlBuffer" /> object that may be used to control the buffer used
        ///   to read/write values from this XmlBased profile.  </returns>
        /// <exception cref="InvalidOperationException">
        ///	  Attempting to lock the file  and the name is null or empty. </exception>
        /// <exception cref="SecurityException">
        ///	  Attempting to lock the file without the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  Attempting to lock the file and ReadWrite access is not permitted by the operating system. </exception>
        /// <remarks>
        ///   <i>Buffering</i> is the caching of an <see cref="XmlDocument" /> object so that subsequent reads or writes
        ///   are all done through it.  This dramatically increases the performance of those operations, but it requires
        ///   that the buffer is flushed (or closed) to commit any changes done to the underlying file.
        ///   <para>
        ///   The XmlBuffer object is created and attached to this XmlBased profile object, if not already present.
        ///   If it is already attached, the same object is returned in subsequent calls, until the object is closed. </para>
        ///   <para>
        ///   Since the XmlBuffer class implements <see cref="IDisposable" />, the <c>using</c> keyword in C# can be 
        ///   used to conveniently create the buffer, write to it, and then automatically flush it (when it's disposed).  
        ///   Here's an example:
        ///   <code> 
        ///   using (profile.Buffer(true))
        ///   {
        ///      profile.SetValue("A Section", "An Entry", "A Value");
        ///      profile.SetValue("A Section", "Another Entry", "Another Value");
        ///      ...
        ///   }
        ///   </code></para></remarks>
        /// <seealso cref="XmlBuffer" />
        /// <seealso cref="Buffering" />
        public XmlBuffer Buffer(bool lockFile)
        {
            if (m_buffer == null)
                m_buffer = new XmlBuffer(this, lockFile);
            return m_buffer;
        }

        /// <summary>
        ///   Activates <i>locked</i> buffering on this XML-based profile object, if not already active. </summary>
        /// <returns>
        ///   The return value is an <see cref="XmlBuffer" /> object that may be used to control the buffer used
        ///   to read/write values from this XmlBased profile.  </returns>
        /// <exception cref="InvalidOperationException">
        ///	  Attempting to lock the file  and the name is null or empty. </exception>
        /// <exception cref="SecurityException">
        ///	  Attempting to lock the file without the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  Attempting to lock the file and ReadWrite access is not permitted by the operating system. </exception>
        /// <remarks>
        ///   <i>Buffering</i> refers to the caching of an <see cref="XmlDocument" /> object so that subsequent reads or writes
        ///   are all done through it.  This dramatically increases the performance of those operations, but it requires
        ///   that the buffer is flushed (or closed) to commit any changes done to the underlying file.
        ///   <para>
        ///   The XmlBuffer object is created and attached to this XmlBased profile object, if not already present.
        ///   If it is already attached, the same object is returned in subsequent calls, until the object is closed. </para>
        ///   <para>
        ///   If the buffer is created, the underlying file (if any) is locked so that no other processes 
        ///   can write to it. This is equivalent to calling Buffer(true). </para>
        ///   <para>
        ///   Since the XmlBuffer class implements <see cref="IDisposable" />, the <c>using</c> keyword in C# can be 
        ///   used to conveniently create the buffer, write to it, and then automatically flush it (when it's disposed).  
        ///   Here's an example:
        ///   <code> 
        ///   using (profile.Buffer())
        ///   {
        ///      profile.SetValue("A Section", "An Entry", "A Value");
        ///      profile.SetValue("A Section", "Another Entry", "Another Value");
        ///      ...
        ///   }
        ///   </code></para></remarks>
        /// <seealso cref="XmlBuffer" />
        /// <seealso cref="Buffering" />
        public XmlBuffer Buffer()
        {
            return Buffer(true);
        }

        /// <summary>
        ///   Gets whether buffering is active or not. </summary>
        /// <remarks>
        ///   <i>Buffering</i> is the caching of an <see cref="XmlDocument" /> object so that subsequent reads or writes
        ///   are all done through it.  This dramatically increases the performance of those operations, but it requires
        ///   that the buffer is flushed (or closed) to commit any changes done to the underlying file.
        ///   <para>
        ///   This property may be used to determine if the buffer is active without actually activating it.  
        ///   The <see cref="Buffer" /> method activates the buffer, which then needs to be flushed (or closed) to update the file. </para></remarks>
        /// <seealso cref="Buffer" />
        /// <seealso cref="XmlBuffer" />
        public bool Buffering
        {
            get
            {
                return m_buffer != null;
            }
        }

        /// <summary>
        ///   Gets or sets the encoding, to be used if the file is created. </summary>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <remarks>
        ///   By default this property is set to <see cref="System.Text.Encoding.UTF8">Encoding.UTF8</see>, but it is only 
        ///   used when the file is not found and needs to be created to write the value. 
        ///   If the file exists, the existing encoding is used and this value is ignored. 
        ///   The <see cref="Profile.Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without changing this property.  After the property has been changed, 
        ///   the <see cref="Profile.Changed" /> event is raised. </remarks>
        public Encoding Encoding
        {
            get
            {
                return m_encoding;
            }
            set
            {
                VerifyNotReadOnly();
                if (m_encoding == value)
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "Encoding", value))
                    return;

                m_encoding = value;
                RaiseChangeEvent(false, ProfileChangeType.Other, null, "Encoding", value);
            }
        }
    }
     

    /// <summary>
    ///   Buffer class for all <see cref="XmlBased" /> Profile classes. </summary>
    /// <remarks>
    ///   This class provides buffering functionality for the <see cref="XmlBased" /> classes.
    ///   <i>Buffering</i> refers to the caching of an <see cref="XmlDocument" /> object so that subsequent reads or writes
    ///   are all done through it.  This dramatically increases the performance of those operations, but it requires
    ///   that the buffer is flushed (or closed) to commit any changes done to the underlying file. 
    ///   <para>
    ///   Since an XmlBased object can only have one buffer attached to it at a time, this class may not
    ///   be instanciated directly.  Instead, use the <see cref="XmlBased.Buffer" /> method of the profile object. </para></remarks>
    /// <seealso cref="XmlBased.Buffer" />
    public class XmlBuffer : IDisposable
    {
        private XmlBased m_profile;
        private XmlDocument m_doc;
        private FileStream m_file;
        internal bool m_needsFlushing;

        /// <summary>
        ///   Initializes a new instance of the XmlBuffer class and optionally locks the file. </summary>
        /// <param name="profile">
        ///   The XmlBased object to associate with the buffer and to assign this object to. </param>
        /// <param name="lockFile">
        ///   If true and the file exists, the file is locked to prevent other processes from writing to it
        ///   until the buffer is closed. </param>
        /// <exception cref="InvalidOperationException">
        ///	  Attempting to lock the file  and the name is null or empty. </exception>
        /// <exception cref="SecurityException">
        ///	  Attempting to lock the file without the required permission. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  Attempting to lock the file and ReadWrite access is not permitted by the operating system. </exception>
        internal XmlBuffer(XmlBased profile, bool lockFile)
        {
            m_profile = profile;

            if (lockFile)
            {
                m_profile.VerifyName();
                if (File.Exists(m_profile.Name))
                    m_file = new FileStream(m_profile.Name, FileMode.Open, m_profile.ReadOnly ? FileAccess.Read : FileAccess.ReadWrite, FileShare.Read);
            }
        }

        /// <summary>
        ///   Loads the XmlDocument object with the contents of an XmlTextWriter object. </summary>
        /// <param name="writer">
        ///   The XmlTextWriter object to load the XmlDocument with. </param>
        /// <remarks>
        ///   This method is used to load the buffer with new data. </remarks>
        internal void Load(XmlTextWriter writer)
        {
            writer.Flush();
            writer.BaseStream.Position = 0;
            m_doc.Load(writer.BaseStream);

            m_needsFlushing = true;
        }

        /// <summary>
        ///   Gets the XmlDocument object associated with this buffer, based on the profile's Name. </summary>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="XmlException">
        ///	  Parse error in the XML being loaded from the file. </exception>
        internal XmlDocument XmlDocument
        {
            get
            {
                if (m_doc == null)
                {
                    m_doc = new XmlDocument();

                    if (m_file != null)
                    {
                        m_file.Position = 0;
                        m_doc.Load(m_file);
                    }
                    else
                    {
                        m_profile.VerifyName();
                        if (File.Exists(m_profile.Name))
                            m_doc.Load(m_profile.Name);
                    }
                }
                return m_doc;
            }
        }

        /// <summary>
        ///   Gets whether the buffer's XmlDocument object is empty. </summary>
        internal bool IsEmpty
        {
            get
            {
                return XmlDocument.InnerXml == String.Empty;
            }
        }

        /// <summary>
        ///   Gets whether changes have been made to the XmlDocument object that require
        ///   the buffer to be flushed so that the file gets updated. </summary>
        /// <remarks>
        ///   This property returns true when the XmlDocument object has been changed and the 
        ///   <see cref="Flush" /> (or <see cref="Close" />) method needs to be called to 
        ///   update the file. </remarks>
        /// <seealso cref="Flush" />
        /// <seealso cref="Close" />
        public bool NeedsFlushing
        {
            get
            {
                return m_needsFlushing;
            }
        }

        /// <summary>
        ///   Gets whether the file associated with the buffer's profile is locked. </summary>
        /// <remarks>
        ///   This property returns true when this object has been created with the <i>lockFile</i> parameter set to true,
        ///   provided the file exists.  When locked, other processes will not be allowed to write to the profile's
        ///   file until the buffer is closed. </remarks>
        /// <seealso cref="Close" />
        public bool Locked
        {
            get
            {
                return m_file != null;
            }
        }

        /// <summary>
        ///   Writes the contents of the XmlDocument object to the file associated with this buffer's profile. </summary>
        /// <remarks>
        ///   This method may be used to explictly commit any changes made to the <see cref="XmlBased" /> profile from the time 
        ///   the buffer was last flushed or created.  It writes the contents of the XmlDocument object to the profile's file.
        ///   When the buffer is being closed (with <see cref="Close" /> or <see cref="Dispose" />) this method is 
        ///   called if <see cref="NeedsFlushing" /> is true. After the buffer is closed, this method may not be called. </remarks>
        /// <exception cref="InvalidOperationException">
        ///   This object is closed. </exception>
        /// <seealso cref="Close" />
        /// <seealso cref="Reset" />
        public void Flush()
        {
            if (m_profile == null)
                throw new InvalidOperationException("Cannot flush an XmlBuffer object that has been closed.");

            if (m_doc == null)
                return;

            if (m_file == null)
                m_doc.Save(m_profile.Name);
            else
            {
                m_file.SetLength(0);
                m_doc.Save(m_file);
            }

            m_needsFlushing = false;
        }

        /// <summary>
        ///   Resets the buffer by discarding its XmlDocument object. </summary>
        /// <remarks>
        ///   This method may be used to rollback any changes made to the <see cref="XmlBased" /> profile from the time 
        ///   the buffer was last flushed or created. After the buffer is closed, this method may not be called. </remarks>
        /// <exception cref="InvalidOperationException">
        ///   This object is closed. </exception>
        /// <seealso cref="Flush" />
        /// <seealso cref="Close" />
        public void Reset()
        {
            if (m_profile == null)
                throw new InvalidOperationException("Cannot reset an XmlBuffer object that has been closed.");

            m_doc = null;
            m_needsFlushing = false;
        }

        /// <summary>
        ///   Closes the buffer by flushing the contents of its XmlDocument object (if necessary) and dettaching itself 
        ///   from its <see cref="XmlBased" /> profile. </summary>
        /// <remarks>
        ///   This method may be used to explictly deactivate the <see cref="XmlBased" /> profile buffer. 
        ///   This means that the buffer is flushed (if <see cref="NeedsFlushing" /> is true) and it gets 
        ///   dettached from the profile. The <see cref="Dispose" /> method automatically calls this method. </remarks>
        /// <seealso cref="Flush" />
        /// <seealso cref="Dispose" />
        public void Close()
        {
            if (m_profile == null)
                return;

            if (m_needsFlushing)
                Flush();

            m_doc = null;

            if (m_file != null)
            {
                m_file.Close();
                m_file = null;
            }

            if (m_profile != null)
                m_profile.m_buffer = null;
            m_profile = null;
        }

        /// <summary>
        ///   Disposes of this object's resources by closing the buffer. </summary>
        /// <remarks>
        ///   This method calls <see cref="Close" />, which flushes the buffer and dettaches it from the profile. </remarks>
        /// <seealso cref="Close" />
        /// <seealso cref="Flush" />
        public void Dispose()
        {
            Close();
        }
    }


    /// <summary>
    ///   Abstract base class for all Profile classes in this namespace. </summary>
    /// <remarks>
    ///   This class contains fields and methods which are common for all the derived Profile classes. 
    ///   It fully implements most of the methods and properties of its base interfaces so that 
    ///   derived classes don't have to. </remarks>
    public abstract class Profile : IProfile
    {
        // Fields
        private string m_name;
        private bool m_readOnly;

        /// <summary>
        ///   Event used to notify that the profile is about to be changed. </summary>
        /// <seealso cref="Changed" />
        public event ProfileChangingHandler Changing;

        /// <summary>
        ///   Event used to notify that the profile has been changed. </summary>
        /// <seealso cref="Changing" />
        public event ProfileChangedHandler Changed;

        /// <summary>
        ///   Initializes a new instance of the Profile class by setting the <see cref="Name" /> to <see cref="DefaultName" />. </summary>
        protected Profile()
        {
            m_name = DefaultName;
        }

        /// <summary>
        ///   Initializes a new instance of the Profile class by setting the <see cref="Name" /> to a value. </summary>
        /// <param name="name">
        ///   The name to initialize the <see cref="Name" /> property with. </param>
        protected Profile(string name)
        {
            m_name = name;
        }

        /// <summary>
        ///   Initializes a new instance of the Profile class based on another Profile object. </summary>
        /// <param name="profile">
        ///   The Profile object whose properties and events are used to initialize the object being constructed. </param>
        protected Profile(Profile profile)
        {
            m_name = profile.m_name;
            m_readOnly = profile.m_readOnly;
            Changing = profile.Changing;
            Changed = profile.Changed;
        }

        /// <summary>
        ///   Gets or sets the name associated with the profile. </summary>
        /// <exception cref="NullReferenceException">
        ///   Setting this property to null. </exception>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if ReadOnly is true. </exception>
        /// <remarks>
        ///   This is usually the name of the file where the data is stored. 
        ///   The <see cref="Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this property 
        ///   returns immediately without being changed.  After the property is changed, 
        ///   the <see cref="Changed" /> event is raised. </remarks>
        /// <seealso cref="DefaultName" />
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                VerifyNotReadOnly();
                if (m_name == value.Trim())
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.Name, null, null, value))
                    return;

                m_name = value.Trim();
                RaiseChangeEvent(false, ProfileChangeType.Name, null, null, value);
            }
        }

        /// <summary>
        ///   Gets or sets whether the profile is read-only or not. </summary>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if it's already true. </exception>
        /// <remarks>
        ///   A read-only profile does not allow any operations that alter sections,
        ///   entries, or values, such as <see cref="SetValue" /> or <see cref="RemoveEntry" />.  
        ///   Once a profile has been marked read-only, it may no longer go back; 
        ///   attempting to do so causes an InvalidOperationException to be raised.
        ///   The <see cref="Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this property 
        ///   returns immediately without being changed.  After the property is changed, 
        ///   the <see cref="Changed" /> event is raised. </remarks>
        /// <seealso cref="CloneReadOnly" />
        /// <seealso cref="IReadOnlyProfile" />
        public bool ReadOnly
        {
            get
            {
                return m_readOnly;
            }

            set
            {
                VerifyNotReadOnly();
                if (m_readOnly == value)
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.ReadOnly, null, null, value))
                    return;

                m_readOnly = value;
                RaiseChangeEvent(false, ProfileChangeType.ReadOnly, null, null, value);
            }
        }

        /// <summary>
        ///   Gets the name associated with the profile by default. </summary>
        /// <remarks>
        ///   This property needs to be implemented by derived classes.  
        ///   See <see cref="IProfile.DefaultName">IProfile.DefaultName</see> for additional remarks. </remarks>
        /// <seealso cref="Name" />
        public abstract string DefaultName
        {
            get;
        }

        /// <summary>
        ///   Retrieves a copy of itself. </summary>
        /// <returns>
        ///   The return value is a copy of itself as an object. </returns>
        /// <remarks>
        ///   This method needs to be implemented by derived classes. </remarks>
        /// <seealso cref="CloneReadOnly" />
        public abstract object Clone();

        /// <summary>
        ///   Sets the value for an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry where the value will be set. </param>
        /// <param name="value">
        ///   The value to set. If it's null, the entry should be removed. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true or
        ///   <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise.
        ///   See <see cref="IProfile.SetValue">IProfile.SetValue</see> for additional remarks. </remarks>
        /// <seealso cref="GetValue" />
        public abstract void SetValue(string section, string entry, object value);

        /// <summary>
        ///   Retrieves the value of an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value is the entry's value, or null if the entry does not exist. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise. </remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public abstract object GetValue(string section, string entry);

        /// <summary>
        ///   Retrieves the string value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value is the entry's value converted to a string, or the given default value if the entry does not exist. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method calls <c>GetValue(section, entry)</c> of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public virtual string GetValue(string section, string entry, string defaultValue)
        {
            object value = GetValue(section, entry);
            return (value == null ? defaultValue : value.ToString());
        }

        /// <summary>
        ///   Retrieves the integer value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value is the entry's value converted to an integer.  If the value
        ///   cannot be converted, the return value is 0.  If the entry does not exist, the
        ///   given default value is returned. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method calls <c>GetValue(section, entry)</c> of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public virtual int GetValue(string section, string entry, int defaultValue)
        {
            object value = GetValue(section, entry);
            if (value == null)
                return defaultValue;

            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///   Retrieves the double value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value is the entry's value converted to a double.  If the value
        ///   cannot be converted, the return value is 0.  If the entry does not exist, the
        ///   given default value is returned. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method calls <c>GetValue(section, entry)</c> of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public virtual double GetValue(string section, string entry, double defaultValue)
        {
            object value = GetValue(section, entry);
            if (value == null)
                return defaultValue;

            try
            {
                return Convert.ToDouble(value);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///   Retrieves the bool value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value is the entry's value converted to a bool.  If the value
        ///   cannot be converted, the return value is <c>false</c>.  If the entry does not exist, the
        ///   given default value is returned. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   Note: Boolean values are stored as "True" or "False". 
        ///   <para>
        ///   This method calls <c>GetValue(section, entry)</c> of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </para></remarks>
        /// <seealso cref="SetValue" />
        /// <seealso cref="HasEntry" />
        public virtual bool GetValue(string section, string entry, bool defaultValue)
        {
            object value = GetValue(section, entry);
            if (value == null)
                return defaultValue;

            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///   Determines if an entry exists inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to be checked for existence. </param>
        /// <returns>
        ///   If the entry exists inside the section, the return value is true; otherwise false. </returns>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method calls GetEntryNames of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </remarks>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        public virtual bool HasEntry(string section, string entry)
        {
            string[] entries = GetEntryNames(section);

            if (entries == null)
                return false;

            VerifyAndAdjustEntry(ref entry);
            return Array.IndexOf(entries, entry) >= 0;
        }

        /// <summary>
        ///   Determines if a section exists. </summary>
        /// <param name="section">
        ///   The name of the section to be checked for existence. </param>
        /// <returns>
        ///   If the section exists, the return value is true; otherwise false. </returns>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        public virtual bool HasSection(string section)
        {
            string[] sections = GetSectionNames();

            if (sections == null)
                return false;

            VerifyAndAdjustSection(ref section);
            return Array.IndexOf(sections, section) >= 0;
        }

        /// <summary>
        ///   Removes an entry from a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise.
        ///   See <see cref="IProfile.RemoveEntry">IProfile.RemoveEntry</see> for additional remarks. </remarks>
        /// <seealso cref="RemoveSection" />
        public abstract void RemoveEntry(string section, string entry);

        /// <summary>
        ///   Removes a section. </summary>
        /// <param name="section">
        ///   The name of the section to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise.
        ///   See <see cref="IProfile.RemoveSection">IProfile.RemoveSection</see> for additional remarks. </remarks>
        /// <seealso cref="RemoveEntry" />
        public abstract void RemoveSection(string section);

        /// <summary>
        ///   Retrieves the names of all the entries inside a section. </summary>
        /// <param name="section">
        ///   The name of the section holding the entries. </param>
        /// <returns>
        ///   If the section exists, the return value should be an array with the names of its entries; 
        ///   otherwise null. </returns>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what other exceptions derived versions may raise. </remarks>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        public abstract string[] GetEntryNames(string section);

        /// <summary>
        ///   Retrieves the names of all the sections. </summary>
        /// <returns>
        ///   The return value should be an array with the names of all the sections. </returns>
        /// <remarks>
        ///   This method needs to be implemented by derived classes.  Check the 
        ///   documentation to see what exceptions derived versions may raise. </remarks>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        public abstract string[] GetSectionNames();

        /// <summary>
        ///   Retrieves a copy of itself and makes it read-only. </summary>
        /// <returns>
        ///   The return value is a copy of itself as a IReadOnlyProfile object. </returns>
        /// <remarks>
        ///   This method serves as a convenient way to pass a read-only copy of the profile to methods 
        ///   that are not allowed to modify it. </remarks>
        /// <seealso cref="ReadOnly" />
        public virtual IReadOnlyProfile CloneReadOnly()
        {
            Profile profile = (Profile)Clone();
            profile.m_readOnly = true;

            return profile;
        }

        /// <summary>
        ///   Retrieves a DataSet object containing every section, entry, and value in the profile. </summary>
        /// <returns>
        ///   If the profile exists, the return value is a DataSet object representing the profile; otherwise it's null. </returns>
        /// <exception cref="InvalidOperationException">
        ///	  <see cref="Profile.Name" /> is null or empty. </exception>
        /// <remarks>
        ///   The returned DataSet will be named using the <see cref="Name" /> property.  
        ///   It will contain one table for each section, and each entry will be represented by a column inside the table.
        ///   Each table will contain only one row where the values will stored corresponding to each column (entry). 
        ///   <para>
        ///   This method serves as a convenient way to extract the profile's data to this generic medium known as the DataSet.  
        ///   This allows it to be moved to many different places, including a different type of profile object 
        ///   (eg., INI to XML conversion). </para>
        ///   <para>
        ///   This method calls GetSectionNames, GetEntryNames, and GetValue of the derived class, so check the 
        ///   documentation to see what other exceptions may be raised. </para></remarks>
        /// <seealso cref="SetDataSet" />
        public virtual DataSet GetDataSet()
        {
            VerifyName();

            string[] sections = GetSectionNames();
            if (sections == null)
                return null;

            DataSet ds = new DataSet(Name);

            // Add a table for each section
            foreach (string section in sections)
            {
                DataTable table = ds.Tables.Add(section);

                // Retrieve the column names and values
                string[] entries = GetEntryNames(section);
                DataColumn[] columns = new DataColumn[entries.Length];
                object[] values = new object[entries.Length];

                int i = 0;
                foreach (string entry in entries)
                {
                    object value = GetValue(section, entry);

                    columns[i] = new DataColumn(entry, value.GetType());
                    values[i++] = value;
                }

                // Add the columns and values to the table
                table.Columns.AddRange(columns);
                table.Rows.Add(values);
            }

            return ds;
        }

        /// <summary>
        ///   Writes the data of every table from a DataSet into this profile. </summary>
        /// <param name="ds">
        ///   The DataSet object containing the data to be set. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true or
        ///   <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   ds is null. </exception>
        /// <remarks>
        ///   Each table in the DataSet represents a section of the profile.  
        ///   Each column of each table represents an entry.  And for each column, the corresponding value
        ///   of the first row is the value to be passed to <see cref="SetValue" />.  
        ///   Note that only the first row is imported; additional rows are ignored.
        ///   <para>
        ///   This method serves as a convenient way to take any data inside a generic DataSet and 
        ///   write it to any of the available profiles. </para>
        ///   <para>
        ///   This method calls SetValue of the derived class, so check its 
        ///   documentation to see what other exceptions may be raised. </para></remarks>
        /// <seealso cref="GetDataSet" />
        public virtual void SetDataSet(DataSet ds)
        {
            if (ds == null)
                throw new ArgumentNullException("ds");

            // Create a section for each table
            foreach (DataTable table in ds.Tables)
            {
                string section = table.TableName;
                DataRowCollection rows = table.Rows;
                if (rows.Count == 0)
                    continue;

                // Loop through each column and add it as entry with value of the first row				
                foreach (DataColumn column in table.Columns)
                {
                    string entry = column.ColumnName;
                    object value = rows[0][column];

                    SetValue(section, entry, value);
                }
            }
        }

        /// <summary>
        ///   Gets the name of the file to be used as the default, without the profile-specific extension. </summary>
        /// <remarks>
        ///   This property is used by file-based Profile implementations 
        ///   when composing the DefaultName.  These implementations take the value returned by this
        ///   property and add their own specific extension (.ini, .xml, .config, etc.).
        ///   <para>
        ///   For Windows applications, this property returns the full path of the executable.  
        ///   For Web applications, this returns the full path of the web.config file without 
        ///   the .config extension.  </para></remarks>
        /// <seealso cref="DefaultName" />
        protected string DefaultNameWithoutExtension
        {
            get
            {
                try
                {
                    string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    return file.Substring(0, file.LastIndexOf('.'));
                }
                catch
                {
                    return "profile";  // if all else fails
                }
            }
        }

        /// <summary>
        ///   Verifies the given section name is not null and trims it. </summary>
        /// <param name="section">
        ///   The section name to verify and adjust. </param>
        /// <exception cref="ArgumentNullException">
        ///   section is null. </exception>
        /// <remarks>
        ///   This method may be used by derived classes to make sure that a valid
        ///   section name has been passed, and to make any necessary adjustments to it
        ///   before passing it to the corresponding APIs. </remarks>
        /// <seealso cref="VerifyAndAdjustEntry" />
        protected virtual void VerifyAndAdjustSection(ref string section)
        {
            if (section == null)
                throw new ArgumentNullException("section");

            section = section.Trim();
        }

        /// <summary>
        ///   Verifies the given entry name is not null and trims it. </summary>
        /// <param name="entry">
        ///   The entry name to verify and adjust. </param>
        /// <remarks>
        ///   This method may be used by derived classes to make sure that a valid
        ///   entry name has been passed, and to make any necessary adjustments to it
        ///   before passing it to the corresponding APIs. </remarks>
        /// <exception cref="ArgumentNullException">
        ///   entry is null. </exception>
        /// <seealso cref="VerifyAndAdjustSection" />
        protected virtual void VerifyAndAdjustEntry(ref string entry)
        {
            if (entry == null)
                throw new ArgumentNullException("entry");

            entry = entry.Trim();
        }

        /// <summary>
        ///   Verifies the Name property is not empty or null. </summary>
        /// <remarks>
        ///   This method may be used by derived classes to make sure that the 
        ///   APIs are working with a valid Name (file name) </remarks>
        /// <exception cref="InvalidOperationException">
        ///   name is empty or null. </exception>
        /// <seealso cref="Name" />
        protected internal virtual void VerifyName()
        {
            if (m_name == null || m_name == "")
                throw new InvalidOperationException("Operation not allowed because Name property is null or empty.");
        }

        /// <summary>
        ///   Verifies the ReadOnly property is not true. </summary>
        /// <remarks>
        ///   This method may be used by derived classes as a convenient way to 
        ///   validate that modifications to the profile can be made. </remarks>
        /// <exception cref="InvalidOperationException">
        ///   ReadOnly is true. </exception>
        /// <seealso cref="ReadOnly" />
        protected internal virtual void VerifyNotReadOnly()
        {
            if (m_readOnly)
                throw new InvalidOperationException("Operation not allowed because ReadOnly property is true.");
        }

        /// <summary>
        ///   Raises either the Changing or Changed event. </summary>
        /// <param name="changing">
        ///   If true, the Changing event is raised otherwise it's Changed. </param>
        /// <param name="changeType">
        ///   The type of change being made. </param>
        /// <param name="section">
        ///   The name of the section that was involved in the change or null if not applicable. </param>
        /// <param name="entry">
        ///   The name of the entry that was involved in the change or null if not applicable. 
        ///   If changeType is equal to Other, entry is the name of the property involved in the change.</param>
        /// <param name="value">
        ///   The value that was changed or null if not applicable. </param>
        /// <returns>
        ///   The return value is based on the event raised.  If the Changing event was raised, 
        ///   the return value is the opposite of ProfileChangingArgs.Cancel; otherwise it's true.</returns>
        /// <remarks>
        ///   This method may be used by derived classes as a convenient alternative to calling 
        ///   OnChanging and OnChanged.  For example, a typical call to OnChanging would require
        ///   four lines of code, which this method reduces to two. </remarks>
        /// <seealso cref="Changing" />
        /// <seealso cref="Changed" />
        /// <seealso cref="OnChanging" />
        /// <seealso cref="OnChanged" />
        protected bool RaiseChangeEvent(bool changing, ProfileChangeType changeType, string section, string entry, object value)
        {
            if (changing)
            {
                // Don't even bother if there are no handlers.
                if (Changing == null)
                    return true;

                ProfileChangingArgs e = new ProfileChangingArgs(changeType, section, entry, value);
                OnChanging(e);
                return !e.Cancel;
            }

            // Don't even bother if there are no handlers.
            if (Changed != null)
                OnChanged(new ProfileChangedArgs(changeType, section, entry, value));
            return true;
        }

        /// <summary>
        ///   Raises the Changing event. </summary>
        /// <param name="e">
        ///   The arguments object associated with the Changing event. </param>
        /// <remarks>
        ///   This method should be invoked prior to making a change to the profile so that the
        ///   Changing event is raised, giving a chance to the handlers to prevent the change from
        ///   happening (by setting e.Cancel to true). This method calls each individual handler 
        ///   associated with the Changing event and checks the resulting e.Cancel flag.  
        ///   If it's true, it stops and does not call of any remaining handlers since the change 
        ///   needs to be prevented anyway. </remarks>
        /// <seealso cref="Changing" />
        /// <seealso cref="OnChanged" />
        protected virtual void OnChanging(ProfileChangingArgs e)
        {
            if (Changing == null)
                return;

            foreach (ProfileChangingHandler handler in Changing.GetInvocationList())
            {
                handler(this, e);

                // If a particular handler cancels the event, stop
                if (e.Cancel)
                    break;
            }
        }

        /// <summary>
        ///   Raises the Changed event. </summary>
        /// <param name="e">
        ///   The arguments object associated with the Changed event. </param>
        /// <remarks>
        ///   This method should be invoked after a change to the profile has been made so that the
        ///   Changed event is raised, giving a chance to the handlers to be notified of the change. </remarks>
        /// <seealso cref="Changed" />
        /// <seealso cref="OnChanging" />
        protected virtual void OnChanged(ProfileChangedArgs e)
        {
            if (Changed != null)
                Changed(this, e);
        }

        /// <summary>
        ///   Runs a test to verify this object is working as expected. </summary>
        /// <param name="cleanup">
        ///   If true, the modifications made to the profile are cleaned up as the final part of the test. 
        ///   If false, the modifications are not removed thus allowing them to be examined. </param>
        /// <remarks>
        ///   This method tests most of the funcionality of a profile object to ensure
        ///   accuracy and consistency.  All profile classes should behave identically when calling this method. 
        ///   If the test fails, an Exception is raised detailing the problem.  </remarks>
        /// <exception cref="Exception">
        ///   The test failed. </exception>
        public virtual void Test(bool cleanup)
        {
            string task = "";
            try
            {
                string section = "Profile Test";

                task = "initializing the profile -- cleaning up the '" + section + "' section";

                RemoveSection(section);

                task = "getting the sections and their count";

                string[] sections = GetSectionNames();
                int sectionCount = (sections == null ? 0 : sections.Length);
                bool haveSections = sectionCount > 1;

                task = "adding some valid entries to the '" + section + "' section";

                SetValue(section, "Text entry", "123 abc");
                SetValue(section, "Blank entry", "");
                SetValue(section, "Null entry", null);  // nothing will be added
                SetValue(section, "  Entry with leading and trailing spaces  ", "The spaces should be trimmed from the entry");
                SetValue(section, "Integer entry", 2 * 8 + 1);
                SetValue(section, "Long entry", 1234567890123456789);
                SetValue(section, "Double entry", 2 * 8 + 1.95);
                SetValue(section, "DateTime entry", DateTime.Today);
                SetValue(section, "Boolean entry", haveSections);

                task = "adding a null entry to the '" + section + "' section";

                try
                {
                    SetValue(section, null, "123 abc");
                    throw new Exception("Passing a null entry was allowed for SetValue");
                }
                catch (ArgumentNullException)
                {
                }

                task = "retrieving a null section";

                try
                {
                    GetValue(null, "Test");
                    throw new Exception("Passing a null section was allowed for GetValue");
                }
                catch (ArgumentNullException)
                {
                }

                task = "getting the number of entries and their count";

                int expectedEntries = 8;
                string[] entries = GetEntryNames(section);

                task = "verifying the number of entries is " + expectedEntries;

                if (entries.Length != expectedEntries)
                    throw new Exception("Incorrect number of entries found: " + entries.Length);

                task = "checking the values for the entries added";

                string strValue = GetValue(section, "Text entry", "");
                if (strValue != "123 abc")
                    throw new Exception("Incorrect string value found for the Text entry: '" + strValue + "'");

                int nValue = GetValue(section, "Text entry", 321);
                if (nValue != 0)
                    throw new Exception("Incorrect integer value found for the Text entry: " + nValue);

                strValue = GetValue(section, "Blank entry", "invalid");
                if (strValue != "")
                    throw new Exception("Incorrect string value found for the Blank entry: '" + strValue + "'");

                object value = GetValue(section, "Blank entry");
                if (value == null)
                    throw new Exception("Incorrect null value found for the Blank entry");

                nValue = GetValue(section, "Blank entry", 321);
                if (nValue != 0)
                    throw new Exception("Incorrect integer value found for the Blank entry: " + nValue);

                bool bValue = GetValue(section, "Blank entry", true);
                if (bValue != false)
                    throw new Exception("Incorrect bool value found for the Blank entry: " + bValue);

                strValue = GetValue(section, "Null entry", "");
                if (strValue != "")
                    throw new Exception("Incorrect string value found for the Null entry: '" + strValue + "'");

                value = GetValue(section, "Null entry");
                if (value != null)
                    throw new Exception("Incorrect object value found for the Blank entry: '" + value + "'");

                strValue = GetValue(section, "  Entry with leading and trailing spaces  ", "");
                if (strValue != "The spaces should be trimmed from the entry")
                    throw new Exception("Incorrect string value found for the Entry with leading and trailing spaces: '" + strValue + "'");

                if (!HasEntry(section, "Entry with leading and trailing spaces"))
                    throw new Exception("The Entry with leading and trailing spaces (trimmed) was not found");

                nValue = GetValue(section, "Integer entry", 0);
                if (nValue != 17)
                    throw new Exception("Incorrect integer value found for the Integer entry: " + nValue);

                double dValue = GetValue(section, "Integer entry", 0.0);
                if (dValue != 17)
                    throw new Exception("Incorrect double value found for the Integer entry: " + dValue);

                long lValue = Convert.ToInt64(GetValue(section, "Long entry"));
                if (lValue != 1234567890123456789)
                    throw new Exception("Incorrect long value found for the Long entry: " + lValue);

                strValue = GetValue(section, "Long entry", "");
                if (strValue != "1234567890123456789")
                    throw new Exception("Incorrect string value found for the Long entry: '" + strValue + "'");

                dValue = GetValue(section, "Double entry", 0.0);
                if (dValue != 17.95)
                    throw new Exception("Incorrect double value found for the Double entry: " + dValue);

                nValue = GetValue(section, "Double entry", 321);
                if (nValue != 0)
                    throw new Exception("Incorrect integer value found for the Double entry: " + nValue);

                strValue = GetValue(section, "DateTime entry", "");
                if (strValue != DateTime.Today.ToString())
                    throw new Exception("Incorrect string value found for the DateTime entry: '" + strValue + "'");

                DateTime today = DateTime.Parse(strValue);
                if (today != DateTime.Today)
                    throw new Exception("The DateTime value is not today's date: '" + strValue + "'");

                bValue = GetValue(section, "Boolean entry", !haveSections);
                if (bValue != haveSections)
                    throw new Exception("Incorrect bool value found for the Boolean entry: " + bValue);

                strValue = GetValue(section, "Boolean entry", "");
                if (strValue != haveSections.ToString())
                    throw new Exception("Incorrect string value found for the Boolean entry: '" + strValue + "'");

                value = GetValue(section, "Nonexistent entry");
                if (value != null)
                    throw new Exception("Incorrect value found for the Nonexistent entry: '" + value + "'");

                strValue = GetValue(section, "Nonexistent entry", "Some Default");
                if (strValue != "Some Default")
                    throw new Exception("Incorrect default value found for the Nonexistent entry: '" + strValue + "'");

                task = "creating a ReadOnly clone of the object";

                IReadOnlyProfile roProfile = CloneReadOnly();

                if (!roProfile.HasSection(section))
                    throw new Exception("The section is missing from the cloned read-only profile");

                dValue = roProfile.GetValue(section, "Double entry", 0.0);
                if (dValue != 17.95)
                    throw new Exception("Incorrect double value in the cloned object: " + dValue);

                task = "checking if ReadOnly clone can be hacked to allow writing";

                try
                {
                    ((IProfile)roProfile).ReadOnly = false;
                    throw new Exception("Changing of the ReadOnly flag was allowed on the cloned read-only profile");
                }
                catch (InvalidOperationException)
                {
                }

                try
                {
                    // Test if a read-only profile can be hacked by casting
                    ((IProfile)roProfile).SetValue(section, "Entry which should not be written", "This should not happen");
                    throw new Exception("SetValue did not throw an InvalidOperationException when writing to the cloned read-only profile");
                }
                catch (InvalidOperationException)
                {
                }

                //	task = "checking the DataSet methods";

                //	DataSet ds = GetDataSet();
                //	Profile copy = (Profile)Clone();
                //	copy.Name = Name + "2";
                //	copy.SetDataSet(ds);					

                if (!cleanup)
                    return;

                task = "deleting the entries just added";

                RemoveEntry(section, "Text entry");
                RemoveEntry(section, "Blank entry");
                RemoveEntry(section, "  Entry with leading and trailing spaces  ");
                RemoveEntry(section, "Integer entry");
                RemoveEntry(section, "Long entry");
                RemoveEntry(section, "Double entry");
                RemoveEntry(section, "DateTime entry");
                RemoveEntry(section, "Boolean entry");

                task = "deleting a nonexistent entry";

                RemoveEntry(section, "Null entry");

                task = "verifying all entries were deleted";

                entries = GetEntryNames(section);

                if (entries.Length != 0)
                    throw new Exception("Incorrect number of entries still found: " + entries.Length);

                task = "deleting the section";

                RemoveSection(section);

                task = "verifying the section was deleted";

                int sectionCount2 = GetSectionNames().Length;

                if (sectionCount != sectionCount2)
                    throw new Exception("Incorrect number of sections found after deleting: " + sectionCount2);

                entries = GetEntryNames(section);

                if (entries != null)
                    throw new Exception("The section was apparently not deleted since GetEntryNames did not return null");
            }
            catch (Exception ex)
            {
                throw new Exception("Test Failed while " + task, ex);
            }
        }
    }



    /// <summary>
    ///   Profile class that utilizes the Windows Registry to retrieve and save its data. </summary>
    /// <remarks>
    ///   By default class this class uses the HKEY_CURRENT_USER root key,
    ///   and sets its default subkey based on the CompanyName and ProductName properties of
    ///   the Application object.  For the Demo application, the Company name is set to
    ///   "AMS" and the product is "ProfileDemo".  So the entire path looks like this:
    /// 
    ///   <code>HKEY_CURRENT_USER\Software\AMS\ProfileDemo</code>
    /// 
    ///   Each section is then created as a subkey of this location on the registry. </remarks>
    public class Registry : Profile
    {
        // Fields
        private RegistryKey m_rootKey = Microsoft.Win32.Registry.CurrentUser;

        /// <summary>
        ///   Initializes a new instance of the Registry class by setting the <see cref="Profile.Name" /> to <see cref="Profile.DefaultName" />. </summary>
        public Registry()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the Registry class by setting the <see cref="RootKey" /> and/or <see cref="Profile.Name" />. </summary>
        /// <param name="rootKey">
        ///   If not null, this is used to initialize the <see cref="RootKey" /> property. </param>
        /// <param name="subKeyName">
        ///   If not null, this is used to initialize the <see cref="Profile.Name" /> property. </param>
        public Registry(RegistryKey rootKey, string subKeyName)
            :
            base("")
        {
            if (rootKey != null)
                m_rootKey = rootKey;
            if (subKeyName != null)
                Name = subKeyName;
        }

        /// <summary>
        ///   Initializes a new instance of the Registry class based on another Registry object. </summary>
        /// <param name="reg">
        ///   The Registry object whose properties and events are used to initialize the object being constructed. </param>
        public Registry(Registry reg)
            :
            base(reg)
        {
            m_rootKey = reg.m_rootKey;
        }

        /// <summary>
        ///   Gets the default name sub-key registry path. </summary>
        /// <exception cref="InvalidOperationException">
        ///   Application.CompanyName or Application.ProductName are empty.</exception>
        /// <remarks>
        ///   This is set to "Software\\" + Application.CompanyName + "\\" + Application.ProductName. </remarks>
        public override string DefaultName
        {
            get
            {
                //if (Application.CompanyName == "" || Application.ProductName == "")
                //    throw new InvalidOperationException("Application.CompanyName and/or Application.ProductName are empty and they're needed for the DefaultName.");

                //return "Software\\" + Application.CompanyName + "\\" + Application.ProductName;
                 return "Software\\" + "Chaint" + "\\" + "ChaintApp";
              
            }
        }

        /// <summary>
        ///   Retrieves a copy of itself. </summary>
        /// <returns>
        ///   The return value is a copy of itself as an object. </returns>
        /// <seealso cref="Profile.CloneReadOnly" />
        public override object Clone()
        {
            return new Registry(this);
        }

        /// <summary>
        ///   Gets or sets the root RegistryKey object to use as the base for the <see cref="Profile.Name" />. </summary>
        /// <exception cref="InvalidOperationException">
        ///   Setting this property if <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <remarks>
        ///   By default, this property is set to Microsoft.Win32.Registry.CurrentUser. 
        ///   The <see cref="Profile.Changing" /> event is raised before changing this property.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without changing this property.  After the property has been changed, 
        ///   the <see cref="Profile.Changed" /> event is raised. </remarks>
        public RegistryKey RootKey
        {
            get
            {
                return m_rootKey;
            }
            set
            {
                VerifyNotReadOnly();
                if (m_rootKey == value)
                    return;

                if (!RaiseChangeEvent(true, ProfileChangeType.Other, null, "RootKey", value))
                    return;

                m_rootKey = value;
                RaiseChangeEvent(false, ProfileChangeType.Other, null, "RootKey", value);
            }
        }

        /// <summary>
        ///   Retrieves a RegistryKey object for the given section. </summary>
        /// <param name="section">
        ///   The name of the section to retrieve the key for. </param>
        /// <param name="create">
        ///   If true, the key is created if necessary; otherwise it is just opened. </param>
        /// <param name="writable">
        ///   If true the key is opened with write access; otherwise it is opened read-only. </param>
        /// <returns>
        ///   The return value is a RegistryKey object representing the section's subkey. </returns>
        /// <exception cref="ArgumentException">
        ///	  The length of <i>Name\section</i> is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(create, currentKey) or RegistryPermission.SetInclude(open, currentKey) access. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  The registry key cannot be written to (for example, it was opened as an unwritable key) </exception>
        /// <remarks>
        ///   This method returns a key for the equivalent path: <see cref="RootKey" /> + "\\" + <see cref="Profile.Name" /> + "\\" + section </remarks>
        protected RegistryKey GetSubKey(string section, bool create, bool writable)
        {
            VerifyName();

            string keyName = Name + "\\" + section;

            if (create)
                return m_rootKey.CreateSubKey(keyName);
            return m_rootKey.OpenSubKey(keyName, writable);
        }

        /// <summary>
        ///   Sets the value for an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry where the value will be set. </param>
        /// <param name="value">
        ///   The value to set. If it's null, the entry is removed. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true or
        ///   <see cref="Profile.Name" /> is null or empty. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null, or 
        ///	  the length of <i>Name\section</i> is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(create, currentKey) or RegistryPermission.SetInclude(open, currentKey) access. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  The registry key cannot be written to (for example, it was opened as an unwritable key) </exception>
        /// <remarks>
        ///   If either the subkey, section, or entry does not exist, it is created.
        ///   The <see cref="Profile.Changing" /> event is raised before setting the value.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without setting the value.  After the value has been set, 
        ///   the <see cref="Profile.Changed" /> event is raised. </remarks>
        /// <seealso cref="GetValue" />
        public override void SetValue(string section, string entry, object value)
        {
            // If the value is null, remove the entry
            if (value == null)
            {
                RemoveEntry(section, entry);
                return;
            }

            VerifyNotReadOnly();
            VerifyAndAdjustSection(ref section);
            VerifyAndAdjustEntry(ref entry);

            if (!RaiseChangeEvent(true, ProfileChangeType.SetValue, section, entry, value))
                return;

            using (RegistryKey subKey = GetSubKey(section, true, true))
                subKey.SetValue(entry, value);

            RaiseChangeEvent(false, ProfileChangeType.SetValue, section, entry, value);
        }

        /// <summary>
        ///   Retrieves the value of an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value is the entry's value, or null if the entry does not exist. </returns>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null, or 
        ///	  the length of <i>Name\section</i> is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(delete, currentKey) access. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  The registry key cannot be written to (for example, it was opened as an unwritable key) </exception>
        /// <seealso cref="SetValue" />
        /// <seealso cref="Profile.HasEntry" />
        public override object GetValue(string section, string entry)
        {
            VerifyAndAdjustSection(ref section);
            VerifyAndAdjustEntry(ref entry);

            using (RegistryKey subKey = GetSubKey(section, false, false))
                return (subKey == null ? null : subKey.GetValue(entry));
        }

        /// <summary>
        ///   Removes an entry from a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   Either section or entry is null, or 
        ///	  the length of <i>Name\section</i> is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(create, currentKey) or RegistryPermission.SetInclude(open, currentKey) access. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  The registry key cannot be written to (for example, it was opened as an unwritable key) </exception>
        /// <remarks>
        ///   The <see cref="Profile.Changing" /> event is raised before removing the entry.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without removing the entry.  After the entry has been removed, 
        ///   the <see cref="Profile.Changed" /> event is raised. </remarks>
        /// <seealso cref="RemoveSection" />
        public override void RemoveEntry(string section, string entry)
        {
            VerifyNotReadOnly();
            VerifyAndAdjustSection(ref section);
            VerifyAndAdjustEntry(ref entry);

            using (RegistryKey subKey = GetSubKey(section, false, true))
            {
                if (subKey != null && subKey.GetValue(entry) != null)
                {
                    if (!RaiseChangeEvent(true, ProfileChangeType.RemoveEntry, section, entry, null))
                        return;

                    subKey.DeleteValue(entry, false);
                    RaiseChangeEvent(false, ProfileChangeType.RemoveEntry, section, entry, null);
                }
            }
        }

        /// <summary>
        ///   Removes a section. </summary>
        /// <param name="section">
        ///   The name of the section to remove. </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="Profile.ReadOnly" /> is true. </exception>
        /// <exception cref="ArgumentNullException">
        ///   section is null or 
        ///	  the length of <i>Name\section</i> is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(create, currentKey) or RegistryPermission.SetInclude(open, currentKey) access. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  The registry key cannot be written to (for example, it was opened as an unwritable key) </exception>
        /// <remarks>
        ///   The <see cref="Profile.Changing" /> event is raised before removing the section.  
        ///   If its <see cref="ProfileChangingArgs.Cancel" /> property is set to true, this method 
        ///   returns immediately without removing the section.  After the section has been removed, 
        ///   the <see cref="Profile.Changed" /> event is raised. </remarks>
        /// <seealso cref="RemoveEntry" />
        public override void RemoveSection(string section)
        {
            VerifyNotReadOnly();
            VerifyName();
            VerifyAndAdjustSection(ref section);

            using (RegistryKey key = m_rootKey.OpenSubKey(Name, true))
            {
                if (key != null && HasSection(section))
                {
                    if (!RaiseChangeEvent(true, ProfileChangeType.RemoveSection, section, null, null))
                        return;

                    key.DeleteSubKeyTree(section);
                    RaiseChangeEvent(false, ProfileChangeType.RemoveSection, section, null, null);
                }
            }
        }

        /// <summary>
        ///   Retrieves the names of all the entries inside a section. </summary>
        /// <param name="section">
        ///   The name of the section holding the entries. </param>
        /// <exception cref="ArgumentNullException">
        ///   section is null or 
        ///	  the length of <i>Name\section</i> is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(delete, currentKey) access. </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///	  The registry key cannot be written to (for example, it was opened as an unwritable key) </exception>
        /// <returns>
        ///   If the section exists, the return value is an array with the names of its entries; 
        ///   otherwise it's null. </returns>
        /// <seealso cref="Profile.HasEntry" />
        /// <seealso cref="GetSectionNames" />
        public override string[] GetEntryNames(string section)
        {
            VerifyAndAdjustSection(ref section);

            using (RegistryKey subKey = GetSubKey(section, false, false))
            {
                if (subKey == null)
                    return null;

                return subKey.GetValueNames();
            }
        }

        /// <summary>
        ///   Retrieves the names of all the sections. </summary>
        /// <returns>
        ///   If the XML file exists, the return value is an array with the names of all the sections;
        ///   otherwise it's null. </returns>
        /// <exception cref="ArgumentNullException">
        ///	  The length of the section is longer than 255 characters. </exception>
        /// <exception cref="SecurityException">
        ///	  The user does not have RegistryPermission.SetInclude(delete, currentKey) access. </exception>
        /// <seealso cref="Profile.HasSection" />
        /// <seealso cref="GetEntryNames" />
        public override string[] GetSectionNames()
        {
            VerifyName();

            using (RegistryKey key = m_rootKey.OpenSubKey(Name))
            {
                if (key == null)
                    return null;
                return key.GetSubKeyNames();
            }
        }
    }




    #region interface

    /// <summary>
    ///   Base interface for all profile classes in this namespace.
    ///   It represents a read-only profile. </summary>
    /// <seealso cref="IProfile" />
    /// <seealso cref="Profile" />
    public interface IReadOnlyProfile : ICloneable
    {
        /// <summary>
        ///   Gets the name associated with the profile. </summary>
        /// <remarks>
        ///   This should be the name of the file where the data is stored, or something equivalent. </remarks>
        string Name
        {
            get;
        }

        /// <summary>
        ///   Retrieves the value of an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <returns>
        ///   The return value should be the entry's value, or null if the entry does not exist. </returns>
        /// <seealso cref="HasEntry" />
        object GetValue(string section, string entry);

        /// <summary>
        ///   Retrieves the value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value should be the entry's value converted to a string, or the given default value if the entry does not exist. </returns>
        /// <seealso cref="HasEntry" />
        string GetValue(string section, string entry, string defaultValue);

        /// <summary>
        ///   Retrieves the value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value should be the entry's value converted to an integer.  If the value
        ///   cannot be converted, the return value should be 0.  If the entry does not exist, the
        ///   given default value should be returned. </returns>
        /// <seealso cref="HasEntry" />
        int GetValue(string section, string entry, int defaultValue);

        /// <summary>
        ///   Retrieves the value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value should be the entry's value converted to a double.  If the value
        ///   cannot be converted, the return value should be 0.  If the entry does not exist, the
        ///   given default value should be returned. </returns>
        /// <seealso cref="HasEntry" />
        double GetValue(string section, string entry, double defaultValue);

        /// <summary>
        ///   Retrieves the value of an entry inside a section, or a default value if the entry does not exist. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry with the value. </param>
        /// <param name="entry">
        ///   The name of the entry where the value is stored. </param>
        /// <param name="defaultValue">
        ///   The value to return if the entry (or section) does not exist. </param>
        /// <returns>
        ///   The return value should be the entry's value converted to a bool.  If the value
        ///   cannot be converted, the return value should be <c>false</c>.  If the entry does not exist, the
        ///   given default value should be returned. </returns>
        /// <remarks>
        ///   Note: Boolean values are stored as "True" or "False". </remarks>
        /// <seealso cref="HasEntry" />
        bool GetValue(string section, string entry, bool defaultValue);

        /// <summary>
        ///   Determines if an entry exists inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to be checked for existence. </param>
        /// <returns>
        ///   If the entry exists inside the section, the return value should be true; otherwise false. </returns>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        bool HasEntry(string section, string entry);

        /// <summary>
        ///   Determines if a section exists. </summary>
        /// <param name="section">
        ///   The name of the section to be checked for existence. </param>
        /// <returns>
        ///   If the section exists, the return value should be true; otherwise false. </returns>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        bool HasSection(string section);

        /// <summary>
        ///   Retrieves the names of all the entries inside a section. </summary>
        /// <param name="section">
        ///   The name of the section holding the entries. </param>
        /// <returns>
        ///   If the section exists, the return value should be an array with the names of its entries; 
        ///   otherwise it should be null. </returns>
        /// <seealso cref="HasEntry" />
        /// <seealso cref="GetSectionNames" />
        string[] GetEntryNames(string section);

        /// <summary>
        ///   Retrieves the names of all the sections. </summary>
        /// <returns>
        ///   The return value should be an array with the names of all the sections. </returns>
        /// <seealso cref="HasSection" />
        /// <seealso cref="GetEntryNames" />
        string[] GetSectionNames();

        /// <summary>
        ///   Retrieves a DataSet object containing every section, entry, and value in the profile. </summary>
        /// <returns>
        ///   If the profile exists, the return value should be a DataSet object representing the profile; otherwise it's null. </returns>
        /// <remarks>
        ///   The returned DataSet should be named using the <see cref="Name" /> property.  
        ///   It should contain one table for each section, and each entry should be represented by a column inside the table.
        ///   Each table should contain only one row where the values will be stored corresponding to each column (entry). 
        ///   <para>
        ///   This method serves as a convenient way to extract the profile's data to this generic medium known as the DataSet.  
        ///   This allows it to be moved to many different places, including a different type of profile object 
        ///   (eg., INI to XML conversion). </para>
        /// </remarks>
        DataSet GetDataSet();
    }

    /// <summary>
    ///   Interface implemented by all profile classes in this namespace.
    ///   It represents a normal profile. </summary>
    /// <remarks>
    ///   This interface takes the members of IReadOnlyProfile (its base interface) and adds
    ///   to it the rest of the members, which allow modifications to the profile.  
    ///   Altogether, this represents a complete profile object. </remarks>
    /// <seealso cref="IReadOnlyProfile" />
    /// <seealso cref="Profile" />
    public interface IProfile : IReadOnlyProfile
    {
        /// <summary>
        ///   Gets or sets the name associated with the profile. </summary>
        /// <remarks>
        ///   This should be the name of the file where the data is stored, or something equivalent.
        ///   When setting this property, the <see cref="ReadOnly" /> property should be checked and if true, an InvalidOperationException should be raised.
        ///   The <see cref="Changing" /> and <see cref="Changed" /> events should be raised before and after this property is changed. </remarks>
        /// <seealso cref="DefaultName" />
        new string Name
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets the name associated with the profile by default. </summary>
        /// <remarks>
        ///   This is used to set the default Name of the profile and it is typically based on 
        ///   the name of the executable plus some extension. </remarks>
        /// <seealso cref="Name" />
        string DefaultName
        {
            get;
        }

        /// <summary>
        ///   Gets or sets whether the profile is read-only or not. </summary>
        /// <remarks>
        ///   A read-only profile should not allow any operations that alter sections,
        ///   entries, or values, such as <see cref="SetValue" /> or <see cref="RemoveEntry" />.  
        ///   Once a profile has been marked read-only, it should be allowed to go back; 
        ///   attempting to do so should cause an InvalidOperationException to be raised.
        ///   The <see cref="Changing" /> and <see cref="Changed" /> events should be raised before 
        ///   and after this property is changed. </remarks>
        /// <seealso cref="CloneReadOnly" />
        /// <seealso cref="IReadOnlyProfile" />
        bool ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        ///   Sets the value for an entry inside a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry where the value will be set. </param>
        /// <param name="value">
        ///   The value to set. If it's null, the entry should be removed. </param>
        /// <remarks>
        ///   This method should check the <see cref="ReadOnly" /> property and throw an InvalidOperationException if it's true.
        ///   It should also raise the <see cref="Changing" /> and <see cref="Changed" /> events before and after the value is set. </remarks>
        /// <seealso cref="IReadOnlyProfile.GetValue" />
        void SetValue(string section, string entry, object value);

        /// <summary>
        ///   Removes an entry from a section. </summary>
        /// <param name="section">
        ///   The name of the section that holds the entry. </param>
        /// <param name="entry">
        ///   The name of the entry to remove. </param>
        /// <remarks>
        ///   This method should check the <see cref="ReadOnly" /> property and throw an InvalidOperationException if it's true.
        ///   It should also raise the <see cref="Changing" /> and <see cref="Changed" /> events before and after the entry is removed. </remarks>
        /// <seealso cref="RemoveSection" />
        void RemoveEntry(string section, string entry);

        /// <summary>
        ///   Removes a section. </summary>
        /// <param name="section">
        ///   The name of the section to remove. </param>
        /// <remarks>
        ///   This method should check the <see cref="ReadOnly" /> property and throw an InvalidOperationException if it's true.
        ///   It should also raise the <see cref="Changing" /> and <see cref="Changed" /> events before and after the section is removed. </remarks>
        /// <seealso cref="RemoveEntry" />
        void RemoveSection(string section);

        /// <summary>
        ///   Writes the data of every table from a DataSet into this profile. </summary>
        /// <param name="ds">
        ///   The DataSet object containing the data to be set. </param>
        /// <remarks>
        ///   Each table in the DataSet should be used to represent a section of the profile.  
        ///   Each column of each table should represent an entry.  And for each column, the corresponding value
        ///   of the first row is the value that should be passed to <see cref="SetValue" />.  
        ///   <para>
        ///   This method serves as a convenient way to take any data inside a generic DataSet and 
        ///   write it to any of the available profiles. </para></remarks>
        /// <seealso cref="IReadOnlyProfile.GetDataSet" />
        void SetDataSet(DataSet ds);

        /// <summary>
        ///   Creates a copy of itself and makes it read-only. </summary>
        /// <returns>
        ///   The return value should be a copy of itself as an IReadOnlyProfile object. </returns>
        /// <remarks>
        ///   This method is meant as a convenient way to pass a read-only copy of the profile to methods 
        ///   that are not allowed to modify it. </remarks>
        /// <seealso cref="ReadOnly" />
        IReadOnlyProfile CloneReadOnly();

        /// <summary>
        ///   Event that should be raised just before the profile is to be changed to allow the change to be canceled. </summary>
        /// <seealso cref="Changed" />
        event ProfileChangingHandler Changing;

        /// <summary>
        ///   Event that should be raised right after the profile has been changed. </summary>
        /// <seealso cref="Changing" />
        event ProfileChangedHandler Changed;
    }

    #endregion

    #region    Event
    /// <summary>
    ///   Types of changes that may be made to a Profile object. </summary>
    /// <remarks>
    ///   A variable of this type is passed inside the ProfileChangedArgs object 
    ///   for the <see cref="Profile.Changing" /> and <see cref="Profile.Changed" /> events </remarks>
    /// <seealso cref="ProfileChangedArgs" />
    public enum ProfileChangeType
    {
        /// <summary> 
        ///   The change refers to the <see cref="Profile.Name" /> property. </summary>		
        /// <remarks> 
        ///   <see cref="ProfileChangedArgs.Value" /> will contain the new name. </remarks>
        Name,

        /// <summary> 
        ///   The change refers to the <see cref="Profile.ReadOnly" /> property. </summary>		
        /// <remarks> 
        ///   <see cref="ProfileChangedArgs.Value" /> will be true. </remarks>
        ReadOnly,

        /// <summary> 
        ///   The change refers to the <see cref="Profile.SetValue" /> method. </summary>		
        /// <remarks> 
        ///   <see cref="ProfileChangedArgs.Section" />,  <see cref="ProfileChangedArgs.Entry" />, 
        ///   and <see cref="ProfileChangedArgs.Value" /> will be set to the same values passed 
        ///   to the SetValue method. </remarks>
        SetValue,

        /// <summary> 
        ///   The change refers to the <see cref="Profile.RemoveEntry" /> method. </summary>		
        /// <remarks> 
        ///   <see cref="ProfileChangedArgs.Section" /> and <see cref="ProfileChangedArgs.Entry" /> 
        ///   will be set to the same values passed to the RemoveEntry method. </remarks>
        RemoveEntry,

        /// <summary> 
        ///   The change refers to the <see cref="Profile.RemoveSection" /> method. </summary>		
        /// <remarks> 
        ///   <see cref="ProfileChangedArgs.Section" /> will contain the name of the section passed to the RemoveSection method. </remarks>
        RemoveSection,

        /// <summary> 
        ///   The change refers to method or property specific to the Profile class. </summary>		
        /// <remarks> 
        ///   <see cref="ProfileChangedArgs.Entry" /> will contain the name of the  method or property.
        ///   <see cref="ProfileChangedArgs.Value" /> will contain the new value. </remarks>
        Other
    }

    /// <summary>
    ///   EventArgs class to be passed as the second parameter of a <see cref="Profile.Changed" /> event handler. </summary>
    /// <remarks>
    ///   This class provides all the information relevant to the change made to the Profile.
    ///   It is also used as a convenient base class for the ProfileChangingArgs class which is passed 
    ///   as the second parameter of the <see cref="Profile.Changing" /> event handler. </remarks>
    /// <seealso cref="ProfileChangingArgs" />
    public class ProfileChangedArgs : EventArgs
    {
        // Fields
        private readonly ProfileChangeType m_changeType;
        private readonly string m_section;
        private readonly string m_entry;
        private readonly object m_value;

        /// <summary>
        ///   Initializes a new instance of the ProfileChangedArgs class by initializing all of its properties. </summary>
        /// <param name="changeType">
        ///   The type of change made to the profile. </param>
        /// <param name="section">
        ///   The name of the section involved in the change or null. </param>
        /// <param name="entry">
        ///   The name of the entry involved in the change, or if changeType is set to Other, the name of the method/property that was changed. </param>
        /// <param name="value">
        ///   The new value for the entry or method/property, based on the value of changeType. </param>
        /// <seealso cref="ProfileChangeType" />
        public ProfileChangedArgs(ProfileChangeType changeType, string section, string entry, object value)
        {
            m_changeType = changeType;
            m_section = section;
            m_entry = entry;
            m_value = value;
        }

        /// <summary>
        ///   Gets the type of change that raised the event. </summary>
        public ProfileChangeType ChangeType
        {
            get
            {
                return m_changeType;
            }
        }

        /// <summary>
        ///   Gets the name of the section involved in the change, or null if not applicable. </summary>
        public string Section
        {
            get
            {
                return m_section;
            }
        }

        /// <summary>
        ///   Gets the name of the entry involved in the change, or null if not applicable. </summary>
        /// <remarks> 
        ///   If <see cref="ChangeType" /> is set to Other, this property holds the name of the 
        ///   method/property that was changed. </remarks>
        public string Entry
        {
            get
            {
                return m_entry;
            }
        }

        /// <summary>
        ///   Gets the new value for the entry or method/property, based on the value of <see cref="ChangeType" />. </summary>
        public object Value
        {
            get
            {
                return m_value;
            }
        }
    }

    /// <summary>
    ///   EventArgs class to be passed as the second parameter of a <see cref="Profile.Changing" /> event handler. </summary>
    /// <remarks>
    ///   This class provides all the information relevant to the change about to be made to the Profile.
    ///   Besides the properties of ProfileChangedArgs, it adds the Cancel property which allows the 
    ///   event handler to prevent the change from happening. </remarks>
    /// <seealso cref="ProfileChangedArgs" />
    public class ProfileChangingArgs : ProfileChangedArgs
    {
        private bool m_cancel;

        /// <summary>
        ///   Initializes a new instance of the ProfileChangingArgs class by initializing all of its properties. </summary>
        /// <param name="changeType">
        ///   The type of change to be made to the profile. </param>
        /// <param name="section">
        ///   The name of the section involved in the change or null. </param>
        /// <param name="entry">
        ///   The name of the entry involved in the change, or if changeType is set to Other, the name of the method/property that was changed. </param>
        /// <param name="value">
        ///   The new value for the entry or method/property, based on the value of changeType. </param>
        /// <seealso cref="ProfileChangeType" />
        public ProfileChangingArgs(ProfileChangeType changeType, string section, string entry, object value)
            :
            base(changeType, section, entry, value)
        {
        }

        /// <summary>
        ///   Gets or sets whether the change about to the made should be canceled or not. </summary>
        /// <remarks> 
        ///   By default this property is set to false, meaning that the change is allowed.  </remarks>
        public bool Cancel
        {
            get
            {
                return m_cancel;
            }
            set
            {
                m_cancel = value;
            }
        }
    }

    /// <summary>
    ///   Definition of the <see cref="Profile.Changing" /> event handler. </summary>
    /// <remarks>
    ///   This definition complies with the .NET Framework's standard for event handlers.
    ///   The sender is always set to the Profile object that raised the event. </remarks>
    public delegate void ProfileChangingHandler(object sender, ProfileChangingArgs e);

    /// <summary>
    ///   Definition of the <see cref="Profile.Changed" /> event handler. </summary>
    /// <remarks>
    ///   This definition complies with the .NET Framework's standard for event handlers.
    ///   The sender is always set to the Profile object that raised the event. </remarks>
    public delegate void ProfileChangedHandler(object sender, ProfileChangedArgs e);

    #endregion

}
