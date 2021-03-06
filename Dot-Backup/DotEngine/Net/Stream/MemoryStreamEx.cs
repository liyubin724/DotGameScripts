﻿using System;
using System.IO;
using System.Net;

namespace Dot.Net.Stream
{
    public class MemoryStreamEx : MemoryStream
    {
        public bool ReadInt(int startIndex,out int value)
        {
            value = 0;
            if(Length >= startIndex+sizeof(int))
            {
                byte[] intBytes = new byte[sizeof(int)];

                Seek(startIndex, SeekOrigin.Begin);
                Read(intBytes, 0, intBytes.Length);
                value = BitConverter.ToInt32(intBytes, 0);

                return true;
            }
            return false;
        }

        public bool ReadByte(int startIndex,out byte value)
        {
            value = 0;
            if(Length>=startIndex+sizeof(byte))
            {
                value = (byte)ReadByte();
                return true;
            }
            return false;
        }

        public void Clear()
        {
            SetLength(0);
        }
    }
}
