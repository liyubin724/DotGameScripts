using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dot.Tool.ProtoGenerator
{
    [Serializable]
    [XmlRoot("templates")]
    public class TemplateConfig
    {
        [XmlElement("template")]
        public List<TemplateData> Templates { get; set; }

        public TemplateData GetTemplate(LanguageType language)
        {
            if(Templates!=null)
            {
                foreach(var template in Templates)
                {
                    if(template.Language == language)
                    {
                        return template;
                    }
                }
            }
            return null;
        }
    }

    [Serializable]
    public class TemplateData
    {
        [XmlAttribute("language")]
        public LanguageType Language { get; set; }

        [XmlElement("recognizer")]
        public TemplatePathData Recognizer { get; set; }
        [XmlElement("parser")]
        public TemplatePathData Parser { get; set; }
    }

    public class TemplatePathData
    {
        [XmlAttribute("client")]
        public string Client { get; set; }
        [XmlAttribute("server")]
        public string Server { get; set; }
    }
}
