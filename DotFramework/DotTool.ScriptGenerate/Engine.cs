using DotEngine.Context;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotTool.ScriptGenerate
{
    public class Engine
    {
        private static string[] DefaultUsing = new string[]
        {
            "using Dot.Context;",
            "using System.Text;",
        };

        private static string[] DefaultAssemblies = new string[]
        {
            typeof(StringContext).Assembly.Location,
            typeof(StringBuilder).Assembly.Location,
        };

        private static string ScriptStart =
@"public static class TemplateRunner {
    public static string Run(StringContext context){
        StringBuilder sb = new StringBuilder();";

        private static string ScriptEnd =
@"            return sb.ToString();
    }
}";

        public static string Execute(StringContext context,string template, string[] assemblies)
        {
            string code = GenerateCode(template);
            if(string.IsNullOrEmpty(code))
            {
                return null;
            }
            List<string> assemblyList = new List<string>(DefaultAssemblies);
            if(assemblies!=null && assemblies.Length>0)
            {
                assemblyList.AddRange(assemblies);
            }

            Assembly assembly = CompileCode(assemblyList.Distinct().ToArray(), code);
            if(assembly == null)
            {
                return null;
            }
            Type type = assembly.GetType("TemplateRunner");
            MethodInfo mInfo = type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public);
            object result = mInfo.Invoke(null, new object[] { context });
            return result?.ToString();
        }

        private static Assembly CompileCode(string[] assemblies,string code)
        {
            CodeDomProvider provider = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.AddRange(assemblies);

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            CompilerResults cr =provider.CompileAssemblyFromSource(parameters, code);
            if(cr.Errors.HasErrors)
            {
                foreach(var error in cr.Errors)
                {
                    Console.WriteLine(error.ToString());
                }
                return null;
            }else
            {
                return cr.CompiledAssembly;
            }
        }

        private static string GenerateCode(string template)
        {
            List<string> usingList = new List<string>(DefaultUsing);

            Chunk[] chunks = ParseTemplate(template);

            StringBuilder scriptSB = new StringBuilder();
            scriptSB.AppendLine(ScriptStart);
            for(int i =0;i<chunks.Length;++i)
            {
                var chunk = chunks[i];
                if (chunk.Type == TokenType.Code)
                {
                    scriptSB.AppendLine(chunk.Text);
                }
                else if (chunk.Type == TokenType.Eval)
                {
                    scriptSB.AppendLine($"sb.Append(({chunk.Text}).ToString());");
                }
                else if (chunk.Type == TokenType.Text)
                {
                    string text = chunk.Text;
                    if(i > 0 && chunks[i-1].Type == TokenType.Code &&text.StartsWith("\r\n"))
                    {
                        text = text.Substring(2);
                    }
                    scriptSB.AppendLine($"sb.Append(\"{EscapeSpecialCharacterToLiteral(text)}\");");
                }
                else if (chunk.Type == TokenType.Using)
                {
                    usingList.Add(chunk.Text);
                }
                else if (chunk.Type == TokenType.Ignore)
                {
                    continue;
                }
            }

            scriptSB.AppendLine(ScriptEnd);
            
            if(usingList.Count>0)
            {
                scriptSB.Insert(0, string.Join("\r\n", usingList.Distinct().ToArray())+"\r\n");
            }

            return scriptSB.ToString();
        }

        private static Chunk[] ParseTemplate(string templateContent)
        {
            if (string.IsNullOrEmpty(templateContent))
            {
                throw new TemplateFormatException("The content is empty.");
            }

            List<Chunk> chunks = new List<Chunk>();

            StringBuilder chunkSB = new StringBuilder();
            int offset = 0;
            while (offset < templateContent.Length)
            {
                int startIndex = templateContent.IndexOf("<%", offset);
                int endIndex = templateContent.IndexOf("%>", offset);
                if (startIndex < 0 && endIndex < 0)
                {
                    if (offset < templateContent.Length)
                    {
                        Chunk chunk = new Chunk(TokenType.Text, templateContent.Substring(offset));
                        chunks.Add(chunk);
                    }
                    offset = templateContent.Length;
                }
                else if (startIndex >= 0 && endIndex < 0)
                {
                    throw new TemplateFormatException("The symbol of (\"%>) is not found");
                }
                else if (endIndex >= 0 && startIndex < 0)
                {
                    throw new TemplateFormatException("The symbol of (\"<%) is not found");
                }
                else if (startIndex >= 0 && endIndex >= 0)
                {
                    if (endIndex <= startIndex+2)
                    {
                        throw new TemplateFormatException("the format is error");
                    }
                    else
                    {
                        if (startIndex > offset)
                        {
                            Chunk chunk = new Chunk(TokenType.Text, templateContent.Substring(offset, startIndex - offset));
                            chunks.Add(chunk);
                        }

                        int index = startIndex + 2;
                        if (index >= templateContent.Length)
                        {
                            throw new TemplateFormatException("");
                        }
                        else
                        {
                            char symbolChar = templateContent[index];
                            if (symbolChar == '=')
                            {
                                Chunk chunk = new Chunk(TokenType.Eval, templateContent.Substring(index + 1, endIndex - index - 1));
                                chunks.Add(chunk);
                            }
                            else if (symbolChar == '+')
                            {
                                Chunk chunk = new Chunk(TokenType.Using, templateContent.Substring(index + 1, endIndex - index - 1));
                                chunks.Add(chunk);
                            }
                            else if (symbolChar == '-')
                            {
                                Chunk chunk = new Chunk(TokenType.Ignore, templateContent.Substring(index + 1, endIndex - index - 1));
                                chunks.Add(chunk);
                            }
                            else
                            {
                                Chunk chunk = new Chunk(TokenType.Code, templateContent.Substring(index, endIndex - index));
                                chunks.Add(chunk);
                            }

                            offset = endIndex + 2;
                        }
                    }
                }
            }

            if (chunks.Count == 0)
            {
                throw new TemplateFormatException("The format is error");
            }
            return chunks.ToArray();
        }

        private static string EscapeSpecialCharacterToLiteral(string input)
        {
            return input.Replace("\\", @"\\")
                    .Replace("\'", @"\'")
                    .Replace("\"", @"\""")
                    .Replace("\n", @"\n")
                    .Replace("\t", @"\t")
                    .Replace("\r", @"\r")
                    .Replace("\b", @"\b")
                    .Replace("\f", @"\f")
                    .Replace("\a", @"\a")
                    .Replace("\v", @"\v")
                    .Replace("\0", @"\0");
        }

    }
}
