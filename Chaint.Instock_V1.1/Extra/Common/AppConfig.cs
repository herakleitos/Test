using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;


namespace Common
{
          //class AppConfig
          //  {
          //  }


    public class AppConfigElement : ConfigurationElement
    {
        #region Constructors
        static AppConfigElement()
        {
            s_propName = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
                );

            //s_propDesc = new ConfigurationProperty(
            //    "desc",
            //    typeof(string),
            //    null,
            //    ConfigurationPropertyOptions.None
            //    ); 
           

            //s_propSequence = new ConfigurationProperty(
            //    "sequence",
            //    typeof(int),
            //    0,
            //    ConfigurationPropertyOptions.IsRequired
            //    );



            _SQLServer = new ConfigurationProperty(
               "SQLServer",
               typeof(string),
               null,
               ConfigurationPropertyOptions.None
               );



            _SQLDataBase = new ConfigurationProperty(
               "SQLDataBase",
               typeof(string),
               null,
               ConfigurationPropertyOptions.None
               );



            _SQLUserID = new ConfigurationProperty(
               "SQLUserID",
               typeof(string),
               null,
               ConfigurationPropertyOptions.None
               );




            _SQLPassword = new ConfigurationProperty(
               "SQLPassword",
               typeof(string),
               null,
               ConfigurationPropertyOptions.None
               );
             
             

            s_properties = new ConfigurationPropertyCollection();

            s_properties.Add(s_propName);
            //s_properties.Add(s_propDesc);
            ////s_properties.Add(s_propState);
            //s_properties.Add(s_propSequence);
            s_properties.Add(_SQLServer);
            s_properties.Add(_SQLDataBase);
            s_properties.Add(_SQLUserID);
            s_properties.Add(_SQLPassword);


        }
        #endregion

        #region Fields
        private static ConfigurationPropertyCollection s_properties;
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return s_properties;
            }
        }

        private static ConfigurationProperty s_propName;
          public string Name
        {
            get { return (string)base[s_propName]; }
            set { base[s_propName] = value; }
        }

        //private static ConfigurationProperty s_propDesc;
        //  public string Description
        //{
        //    get { return (string)base[s_propDesc]; }
        //    set { base[s_propDesc] = value; }
        //}
  
        //private static ConfigurationProperty s_propSequence;
        // public int Sequence
        //{
        //    get { return (int)base[s_propSequence]; }
        //    set { base[s_propSequence] = value; }
        //}








        private static ConfigurationProperty _SQLServer;
        public string SQLServer
        {
            
            get { return (string)base[_SQLServer]; }
            set { base[_SQLServer] = value; }
            
        }

        private static ConfigurationProperty _SQLDataBase;
        public string SQLDataBase
        {
            get { return (string)base[_SQLDataBase]; }
            set { base[_SQLDataBase] = value; }
        }

        private static ConfigurationProperty _SQLUserID;
        public string SQLUserID
        {
            get { return (string)base[_SQLUserID]; }
            set { base[_SQLUserID] = value; }
        }

        private static ConfigurationProperty _SQLPassword;
        public string SQLPassword
        {
            get { return (string)base[_SQLPassword]; }
            set { base[_SQLPassword] = value; }
        }

























        #endregion

    
    } 


    public class AppConfigCollection : ConfigurationElementCollection
    {
        #region Constructor
        public AppConfigCollection()
        {
        }
        #endregion

        #region Properties
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        protected override string ElementName
        {
            get
            {
                return "AppUtils";
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return new ConfigurationPropertyCollection();
            }
        }
        #endregion

        #region Indexers
        public AppConfigElement this[int index]
        {
            get
            {
                return (AppConfigElement)base.BaseGet(index);
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public AppConfigElement this[string name]
        {
            get
            {
                return (AppConfigElement)base.BaseGet(name);
            }
        }
        #endregion

        #region Methods
        public void Add(AppConfigElement item)
        {
            base.BaseAdd(item);
        }

        public void Remove(AppConfigElement item)
        {
            base.BaseRemove(item);
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }
        #endregion

        #region Overrides
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as AppConfigElement).Name;
        }
        #endregion
    }

    public class AppConfigSection : ConfigurationSection
    {
        #region Constructors
        static AppConfigSection()
        {
            s_propName = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
                );

            s_propArticles = new ConfigurationProperty(
                "",
                typeof(AppConfigCollection),
                null,
                ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsDefaultCollection
                );

            s_propKeyValues = new ConfigurationProperty(
             "AppSettings",
             typeof(KeyValueConfigurationCollection),
             null,
             ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsDefaultCollection
             );



            s_properties = new ConfigurationPropertyCollection();

            s_properties.Add(s_propName);
            s_properties.Add(s_propArticles);
            s_properties.Add(s_propKeyValues);
        }
        #endregion

        #region Fields
        private static ConfigurationPropertyCollection s_properties;
        private static ConfigurationProperty s_propName;
        private static ConfigurationProperty s_propArticles;

        private static ConfigurationProperty s_propKeyValues;
        #endregion

        #region Properties
        public string Name
        {
            get { return (string)base[s_propName]; }
            set { base[s_propName] = value; }
        }

        public AppConfigCollection Articles
        {
            get { return (AppConfigCollection)base[s_propArticles]; }
        }


        public KeyValueConfigurationCollection AppSettings
        {
            get { return (KeyValueConfigurationCollection)base[s_propKeyValues]; }
        }


        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return s_properties;
            }
        }
        #endregion
    }
}
