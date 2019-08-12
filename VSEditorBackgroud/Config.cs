using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace VSEditorBackgroud
{
    [Serializable]
    public class Config
    {
        public ImageConfig BackgroundImage;
        private static Config _Config = null;
        [XmlAttribute]
        public double LayerOpacity;
        private static object staticSyncRoot = new object();

        public Config()
        {
            this.LayerOpacity = 0.5;
            this.BackgroundImage = new ImageConfig();
        }

        public static Config CreatePure()
        {
            Config config = new Config();
            return config;
        }

        public static Config CurrentConfig
        {
            get
            {
                lock (staticSyncRoot)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config), new XmlRootAttribute("ImageConfig"));
                    bool IsCreate = false;
                    if (_Config == null)
                    {
                        if (File.Exists(ConfigConsts.ConfigPath))
                        {
                            try
                            {
                                using (StreamReader reader = new StreamReader(ConfigConsts.ConfigPath))
                                {
                                    _Config = (Config)serializer.Deserialize(reader);
                                }
                                IsCreate = true;
                            }
                            catch { }
                        }
                        if (!IsCreate)
                        {
                            _Config = CreatePure();
                            try
                            {
                                using (StreamWriter writer = new StreamWriter(ConfigConsts.ConfigPath, false, ConfigConsts.NoBomUTF8))
                                {
                                    serializer.Serialize(writer, _Config, ConfigConsts.VoidNamespaceMapping);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                return _Config;
            }
        }

        [Serializable]
        public class ImageConfig
        {
            [XmlAttribute]
            public double Opacity = 1.0;
            public string Uri = "\t";
        }
    }
}
