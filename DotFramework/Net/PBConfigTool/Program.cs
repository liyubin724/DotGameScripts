using CommandLine;
using DotTool.PBConfig;
using System.IO;

namespace PBConfigTool
{
    public enum PlatformType
    {
        Client = 0,
        Server,
        All,
    }

    class Options
    {
        [Option('c',"config-path",Required =true,HelpText ="")]
        public string ConfigFilePath { get; set; }

        [Option('o',"output-dir",Required =true,HelpText ="")]
        public string OutputDir { get; set; }

        [Option('t',"template-path",Required =true,HelpText ="")]
        public string TemplateFilePath { get; set; }

        [Option('p',"platform",Required =false,HelpText ="")]
        public PlatformType Platform { get; set; } = PlatformType.All;
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        static void RunOptions(Options options)
        {
            if (!File.Exists(options.ConfigFilePath) || !File.Exists(options.TemplateFilePath))
            {
                return;
            }
            if (!Directory.Exists(options.OutputDir))
            {
                Directory.CreateDirectory(options.OutputDir);
            }
            string templateContent = File.ReadAllText(options.TemplateFilePath);
            if (string.IsNullOrEmpty(templateContent))
            {
                return;
            }

            ProtoConfig protoConfig = ProtoConfigUtil.ReadConfig(options.ConfigFilePath);
            if(protoConfig == null)
            {
                return;
            }

            if(options.Platform == PlatformType.Client || options.Platform == PlatformType.All)
            {
                ProtoIDWriter.CreateProtoID(options.OutputDir, protoConfig.SpaceName, protoConfig.C2SGroup, templateContent);
            }
            if(options.Platform == PlatformType.Server || options.Platform == PlatformType.All)
            {
                ProtoIDWriter.CreateProtoID(options.OutputDir, protoConfig.SpaceName, protoConfig.S2CGroup, templateContent);
            }
        }
    }
}
