namespace Dot.Core.Util
{
    public static class BitUtil
    {
        public static byte SetBit(byte input,int index,bool enable)
        {
            if (enable)
            {
                return (byte)(input | (1 << index));
            }
            else
            {
                return (byte)(input & (~(1 << index)));
            }
        }

        public static byte GetBit(byte input,int index)
        {
            return (byte)(input & (1 << index));
        }

        public static bool IsEnable(byte input ,int index)
        {
            return GetBit(input, index) > 0;
        }

        public static int SetBit(int input,int index,bool enable)
        {
            if(enable)
            {
                return input | (1 << index);
            }else
            {
                return input & (~(1 << index));
            }
        }

        public static int GetBit(int input,int index)
        {
            return input & (1 << index);
        }

        public static bool IsEnable(int input,int index)
        {
            return GetBit(input, index) > 0;
        }
    }
}
