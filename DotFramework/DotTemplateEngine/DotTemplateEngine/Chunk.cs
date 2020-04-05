namespace Dot.TemplateEngine
{
    public enum TokenType
    {
        None = 0,
        Using,
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
}
