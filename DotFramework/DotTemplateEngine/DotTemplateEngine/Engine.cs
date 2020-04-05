using Dot.Context;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Dot.TemplateEngine
{
    public static class ChunkParser
    {
        public static List<Chunk> Parse(string snippet)
        {
            if (string.IsNullOrEmpty(snippet))
            {
                throw new TemplateFormatException("Snippet is empty");
            }

            List<Chunk> chunks = new List<Chunk>();

            StringBuilder chunkSB = new StringBuilder();
            int offset = 0;
            while (offset < snippet.Length)
            {
                int startIndex = snippet.IndexOf("<%", offset);
                int endIndex = snippet.IndexOf("%>", offset);
                if (startIndex < 0 && endIndex < 0)
                {
                    if (offset < snippet.Length)
                    {
                        Chunk chunk = new Chunk(TokenType.Text, snippet.Substring(offset));
                        chunks.Add(chunk);
                    }
                    offset = snippet.Length;
                }
                else if (startIndex >= 0 && endIndex < 0)
                {
                    throw new TemplateFormatException("");
                }
                else if (endIndex >= 0 && startIndex < 0)
                {
                    throw new TemplateFormatException("");
                }
                else if (startIndex >= 0 && endIndex >= 0)
                {
                    if (endIndex <= startIndex)
                    {
                        throw new TemplateFormatException("");
                    }
                    else
                    {
                        if(startIndex>offset)
                        {
                            Chunk chunk = new Chunk(TokenType.Text, snippet.Substring(offset, startIndex- offset));
                            chunks.Add(chunk);
                        }

                        int index = startIndex + 2;
                        if (index >= snippet.Length)
                        {
                            throw new TemplateFormatException("");
                        }
                        else
                        {
                            char symbolChar = snippet[index];
                            if (symbolChar == '=')
                            {
                                Chunk chunk = new Chunk(TokenType.Eval, snippet.Substring(index + 1, endIndex-index-1));
                                chunks.Add(chunk);
                            }else if(symbolChar == '+')
                            {
                                Chunk chunk = new Chunk(TokenType.Using, snippet.Substring(index + 1, endIndex - index - 1));
                                chunks.Add(chunk);
                            }
                            else
                            {
                                Chunk chunk = new Chunk(TokenType.Code, snippet.Substring(index, endIndex-index));
                                chunks.Add(chunk);
                            }

                            offset = endIndex + 2;
                        }
                    }
                }
            }

            if (chunks.Count == 0)
            {
                throw new TemplateFormatException("");
            }
            return chunks;
        }
    }

    public class Engine
    {
        private static string[] DefaultUsing = new string[]
        {
            "using Dot.Context;",
            "using System.Text;",
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
            Assembly assembly = CompileCode(assemblies, code);
            Type type = assembly.GetType("TemplateRunner");
            MethodInfo mInfo = type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public);
            object result = mInfo.Invoke(null, new object[] { context });
            return result.ToString();
        }

        public static Assembly CompileCode(string[] assemblies,string code)
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

        public static string GenerateCode(string template)
        {
            List<string> usingList = new List<string>(DefaultUsing);

            List<Chunk> chunks = ChunkParser.Parse(template);
            StringBuilder scriptSB = new StringBuilder();
            scriptSB.AppendLine(ScriptStart);

            foreach (var chunk in chunks)
            {
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
                    scriptSB.AppendLine($"sb.Append(\"{EscapeSpecialCharacterToLiteral(chunk.Text)}\");");
                }else if(chunk.Type == TokenType.Using)
                {
                    if(usingList.IndexOf(chunk.Text)<0)
                    {
                        usingList.Add(chunk.Text);
                    }
                }
            }

            scriptSB.AppendLine(ScriptEnd);
            
            if(usingList.Count>0)
            {
                scriptSB.Insert(0, string.Join("\r\n", usingList.ToArray())+"\r\n");
            }

            return scriptSB.ToString();
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
