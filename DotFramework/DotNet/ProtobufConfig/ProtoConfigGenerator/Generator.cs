using Dot.Context;
using Dot.TemplateEngine;
using Dot.Tool.Proto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Dot.Tool.ProtoGenerator
{
    public static class Generator
    {
        public static void GenerateParser(ProtoConfig protoConfig, ProtoGroup protoGroup, string outputDir, string templateContent)
        {
            StringContext context = new StringContext();
            context.Add("protoConfig", protoConfig);

            List<string> assemblies = new List<string>()
            {
                typeof(ProtoConfig).Assembly.Location,
            };

            string outputFilePath = $"{outputDir}/{protoGroup.Name}_Parser.cs";
            context.AddOrUpdate("protoGroup", protoGroup);
            string outputContent = Engine.Execute(context, templateContent, assemblies.ToArray());
            File.WriteAllText(outputFilePath, outputContent);
            context.Remove("protoGroup");
        }

        public static void GenerateRecognizer(ProtoConfig protoConfig,string outputDir,string templateDir,TemplatePathData pathData)
        {
            string templateFullPath = $"{templateDir}/{pathData.Client}";
            if (!File.Exists(templateFullPath))
            {
                Console.WriteLine($"Template is Null.path = {templateFullPath}", Color.Red);
                return;
            }
            string templateContent = File.ReadAllText(templateFullPath);
            Generator.GenerateRecognizer(protoConfig, protoConfig.C2SGroup, outputDir, templateContent);

            templateFullPath = $"{templateDir}/{pathData.Server}";
            if (!File.Exists(templateFullPath))
            {
                Console.WriteLine($"Template is Null.path = {templateFullPath}", Color.Red);
                return;
            }
            templateContent = File.ReadAllText(templateFullPath);
            Generator.GenerateRecognizer(protoConfig, protoConfig.S2CGroup, outputDir, templateContent);
        }

        private static void GenerateRecognizer(ProtoConfig protoConfig, ProtoGroup protoGroup, string outputDir,string templateContent)
        {
            StringContext context = new StringContext();
            context.Add("protoConfig", protoConfig);

            List<string> assemblies = new List<string>()
            {
                typeof(ProtoConfig).Assembly.Location,
            };

            string outputFilePath = $"{outputDir}/{protoGroup.Name}.cs";
            context.AddOrUpdate("protoGroup", protoGroup);
            string outputContent = Engine.Execute(context, templateContent, assemblies.ToArray());
            File.WriteAllText(outputFilePath, outputContent);
            context.Remove("protoGroup");
        }
    }
}
