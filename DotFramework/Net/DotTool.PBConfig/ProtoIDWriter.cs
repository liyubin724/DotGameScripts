using DotEngine.Context;
using DotTool.ScriptGenerate;
using System.Collections.Generic;
using System.IO;

namespace DotTool.PBConfig
{
    public class ProtoIDWriter
    {
        public static void CreateProtoID(string outputDir,string spaceName,ProtoGroup protoGroup,string templateContent)
        {
            StringContext context = new StringContext();
            context.Add("spaceName", spaceName);
            context.Add("protoGroup", protoGroup);

            List<string> assemblies = new List<string>()
            {
                typeof(ProtoConfig).Assembly.Location,
            };

            string outputFilePath = $"{outputDir}/{protoGroup.Name}_IDs.cs";
            string outputContent = TemplateEngine.Execute(context, templateContent, assemblies.ToArray());
            File.WriteAllText(outputFilePath, outputContent);
        }
    }
}
