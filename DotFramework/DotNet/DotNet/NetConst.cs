using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Net
{
    public enum MessageWriterType
    {
        Json = 0,
        ProtoBuf = 1,
    }

    public enum MessageCryptoType
    {
        None = 0,
        AES,
    }

    public enum MessageCompressorType
    {
        None = 0,
        Snappy,
    }

    public static class NetConst
    {
    }
}
