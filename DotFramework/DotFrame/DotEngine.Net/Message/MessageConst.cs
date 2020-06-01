namespace Dot.Net.Message
{
    public enum MessageErrorCode
    {
        Reader_ReadSerialNumberError = 100,
        Reader_ReadFlagTypeError,
        Reader_ReadMessageIDError,
        Reader_CompareMessageDataLengthError,
        Reader_CompareSerialNumberError,

        Reader_CompareCryptoTypeError,
    }

    public class MessageConst
    {
        public static readonly int MESSAGE_MIN_LENGTH = 0;

        public static readonly int MESSAGE_CRYPTO_FLAG_INDEX = 0;
        public static readonly int MESSAGE_COMPRESSOR_FLAG_INDEX = 1;

        static MessageConst()
        {
            MESSAGE_MIN_LENGTH = sizeof(int) //Total Length
                + sizeof(byte) //Serial Number
                + sizeof(byte) //compression and crypt
                + sizeof(int); //Message ID
        }
    }
}
