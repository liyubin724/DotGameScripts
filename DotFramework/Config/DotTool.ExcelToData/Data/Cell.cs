namespace DotTool.ETD.Data
{
    public class Cell
    {
        private int row;
        private int col;
        private string value;

        public int Row { get => row; }
        public int Col { get => col; }

        public Cell(int r,int c,string v)
        {
            this.row = r;
            this.col = c;
            this.value = v;
        }

        public string GetValue(Field field)
        {
            return string.IsNullOrEmpty(value) ? field.DefaultValue : value;
        }

        public override string ToString()
        {
            return $"<row:{row},col:{col},value:{(string.IsNullOrEmpty(value) ? "" : value)}>";
        }
    }
}
