using System;
using System.Collections.Generic;
using System.IO;

namespace DotEngine.FS
{
    public class FragmentData
    {
        public const int Size = sizeof(long) + sizeof(int);

        public long StartPosition { get; set; }
        public int UsageSize { get; set; }
    }

    public class FileFragment
    {
        private List<FragmentData> sortedStartFragments;
        public FileFragment()
        {
            sortedStartFragments = new List<FragmentData>();
        }

        public unsafe FileSystemResultCode ReadFromStream(Stream stream)
        {
            byte[] bytes = new byte[FragmentData.Size];
            if (stream.Read(bytes, 0, sizeof(int)) != sizeof(int))
            {
                return FileSystemResultCode.FragmentByteLengthError;
            }

            int len = 0;
            fixed (byte* b = &bytes[0])
            {
                len = *((int*)b);
            }

            for (int i = 0; i < len; ++i)
            {
                if(stream.Read(bytes,0,bytes.Length)!=bytes.Length)
                {
                    return FileSystemResultCode.FragmentDataByteLengthError;
                }

                fixed(byte *c = &bytes[0])
                {
                    long start = *((long*)c);
                    long size = *((long*)(c + sizeof(long)));

                    FragmentData data = new FragmentData() { StartPosition = start, UsageSize = size };
                    sortedStartFragments.Add(data);
                }
            }

            if(sortedStartFragments.Count!=len)
            {
                return FileSystemResultCode.FragmentDataCountError;
            }
            return FileSystemResultCode.Success;
        }

        public void WriteToStream(Stream stream)
        {
            int len = sortedStartFragments.Count;
            byte[] lenBytes = BitConverter.GetBytes(len);
            stream.Write(lenBytes, 0, lenBytes.Length);

            foreach (var fragment in sortedStartFragments)
            {
                byte[] startBytes = BitConverter.GetBytes(fragment.StartPosition);
                byte[] sizeBytes = BitConverter.GetBytes(fragment.UsageSize);
                stream.Write(startBytes, 0, startBytes.Length);
                stream.Write(sizeBytes, 0, sizeBytes.Length);
            }
        }

        public void Add(long start,int size)
        {

        }

        public void Get(int length)
        {

        }


    }
}
