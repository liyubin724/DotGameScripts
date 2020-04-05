using Dot.Context;
using Dot.TemplateEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotTemplateEngineConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string tePath = @"D:\WorkSpace\DotGameProject\DotGameScripts\DotFramework\DotTemplateEngine\test.te.txt";
            string templateContent = File.ReadAllText(tePath);
            List<string> assemblies = new List<string>()
            {
                typeof(StringContext).Assembly.Location,
                typeof(StringBuilder).Assembly.Location,
                typeof(List<string>).Assembly.Location,
            };

            StringContext context = new StringContext();
            context.Add("names", new List<string>()
            {
                "AA",
                "BB",
                "CC",
            });

            string content = Engine.GenerateCode(templateContent);
            string result = Engine.Execute(context, templateContent, assemblies.ToArray());
            
            File.WriteAllText("D:\\t.cs", result);

            Console.ReadKey();
        }
    }
}
