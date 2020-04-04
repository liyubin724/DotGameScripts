using System;
using System.Collections.Generic;
using System.Text;

namespace Dot.TemplateEngine
{
    public enum TokenType
    {
        None = 0,
        Code,
        Eval,
        Text,
    }

    public class Chunk
    {
        public TokenType Type { get; private set; }
        public string Text { get; private set; }

        public Chunk(TokenType type, string text)
        {
            Type = type;
            Text = text;
        }
    }

    public static class ChunkParser
    {
        public static List<Chunk> Parser(string snippet)
        {
            if(string.IsNullOrEmpty(snippet))
            {
                throw new TemplateFormatException("Snippet is empty");
            }

            List<Chunk> chunks = new List<Chunk>();

            StringBuilder chunkSB = new StringBuilder();

            TokenType tokenType = TokenType.None;
            for(int i =0;i<snippet.Length;)
            {


                if(snippet[i] == '<')
                {
                    if(i+1>=snippet.Length)
                    {
                        throw new TemplateFormatException("");
                    }else if(snippet[i+1] == '%')
                    {
                        if(tokenType != TokenType.None)
                        {
                            throw new TemplateFormatException("");
                        }else
                        {
                            if(chunkSB.Length>0)
                            {
                                chunks.Add(new Chunk(TokenType.Text, chunkSB.ToString()));
                            }

                        }
                    }else
                    {

                    }
                }else if(snippet[i] == '>')
                {

                }else
                {

                }
            }
            
        }
    }
}
