namespace DotEngine.Config.Ndb
{
    public enum NDBFieldType : int
    {
        Null = 0,

        Int = 'i',
        IntArray = 'I',
        
        Float = 'f',
        FloatArray = 'F',
        
        Long = 'l',
        LongArray = 'L',
        
        Bool = 'b',
        BoolArray = 'B',
        
        String = 's',
        StringArray = 'S',
    }

    public class NDBField
    {
        public NDBFieldType type;
        public string name;
        public int offset;
    }
}
