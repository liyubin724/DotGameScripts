using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DotTool.PBConfig
{
    public static class ProtoConfigUtil
    {
        public static ProtoConfig ReadConfig(string path, bool createIfNot = false)
        {
            ProtoConfig config = null;
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProtoConfig));
                config = xmlSerializer.Deserialize(File.OpenRead(path)) as ProtoConfig;
            }

            if (config == null && createIfNot)
            {
                config = new ProtoConfig();
            }
            return config;
        }

        public static void WriterConfig(string path, ProtoConfig config)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProtoConfig));
            StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
            xmlSerializer.Serialize(writer, config);
        }
    }
}
