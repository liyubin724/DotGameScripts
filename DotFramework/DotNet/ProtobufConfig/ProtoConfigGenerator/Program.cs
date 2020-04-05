using CommandLine;
using Dot.Tool.Proto;
using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Console = Colorful.Console;

namespace Dot.Tool.ProtoGenerator
{
    public enum LanguageType
    {
        CSharp,
        Lua,
        CPlusPlus,
    }

    public enum PlatformType
    {
        Client,
        Server,
    }

    public class GeneratorOption
    {
        [Option('c',"input-config",Required =true,HelpText ="Proto配置文件的路径")]
        public string InputConfigPath { get; set; }
        [Option('t', "input-template", Required = true, HelpText = "模板配置文件路径")]
        public string InputTemplatePath { get; set; }
        [Option('o', "output", Required = true, HelpText = "生成文件输出目录")]
        public string OutputDir { get; set; }
        [Option('l', "language", Required = true, HelpText = "输出语言，只前只可选择CSharp")]
        public LanguageType Language { get; set; }
        [Option('p', "platform", Required = true, HelpText = "默认输出平台，可选择Client,Server")]
        public PlatformType PlatformType { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<GeneratorOption>(args).WithParsed((option)=>
            {
                ProtoConfig protoConfig = ReadConfig<ProtoConfig>(option.InputConfigPath);
                if(protoConfig == null)
                {
                    Console.WriteLine("ProtoConfig Error", Color.Red);
                    return;
                }
               

                TemplateConfig templateConfig = ReadConfig<TemplateConfig>(option.InputTemplatePath);
                if (templateConfig == null)
                {
                    Console.WriteLine("TemplateConfig Error", Color.Red);
                    return;
                }

                string templateDir = Path.GetDirectoryName(option.InputTemplatePath);
                TemplateData template = templateConfig.GetTemplate(option.Language);
                if(template == null)
                {
                    Console.WriteLine("TemplateData Error", Color.Red);
                    return;
                }

                Generator.GenerateRecognizer(protoConfig, option.OutputDir, templateDir,template.Recognizer);

                string parserTemplateFullPath = string.Empty;
                ProtoGroup protoGroup = null;
                if(option.PlatformType == PlatformType.Server)
                {
                    parserTemplateFullPath = $"{templateDir}/{template.Parser.Server}";
                    protoGroup = protoConfig.C2SGroup;
                }
                else
                {
                    parserTemplateFullPath = $"{templateDir}/{template.Parser.Client}";
                    protoGroup = protoConfig.S2CGroup;
                }

                if(!File.Exists(parserTemplateFullPath))
                {
                    Console.WriteLine("parserTemplateFullPath Error", Color.Red);
                    return;
                }
                string parserTemplateContent = File.ReadAllText(parserTemplateFullPath);

                Generator.GenerateParser(protoConfig, protoGroup, option.OutputDir, parserTemplateContent);

            });
            if (result.Tag == ParserResultType.NotParsed)
            {
                Console.WriteLine("The parameter in option is error");
            }
            Console.ReadKey();
        }

        static T ReadConfig<T>(string configPath) where T:class
        {
            if (string.IsNullOrEmpty(configPath) || !File.Exists(configPath))
            {
                Console.WriteLine("the file of config is not found!", Color.Red);
                return default;
            }

            T config = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                config = (T)xmlSerializer.Deserialize(File.OpenRead(configPath));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Read config({configPath}) caused some error.message={e.Message}.", Color.Red);
                return default;
            }
            return config;
        }
    }
}
