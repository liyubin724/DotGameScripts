//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.IO;
//using System.Runtime.CompilerServices;
//using System.Text;

//namespace DotEngine.FileSystem
//{
//    public enum FileChunkResultCode
//    {
//        Success = 0,
       
//        ChunkReadFileNotExist = -1,
//        ChunkReadFileError = -2,
        


//        ChunkSaveFilePathError = -10,
//        ChunkSaveFileError = -11,

//        IndexByteCountError = -100,
//        IndexPathError = -101,
//        IndexPathRepeatError = -102,

//        FragmentByteCountError = -200,
//    }

//    public class FileChunk
//    {
//        private FileUsedMode usedMode = FileUsedMode.ReadWrite;

//        private string filePath = null;

//        private List<FileFragment> fragments = null;
//        private Dictionary<string, FileFragment> contentFragmentDic = null;
        
//        public FileChunk(FileUsedMode mode) 
//        {
//            usedMode = mode;
//            contentFragmentDic = new Dictionary<string, FileFragment>();
//            if(usedMode == FileUsedMode.Write || usedMode == FileUsedMode.ReadWrite)
//            {
//                fragments = new List<FileFragment>();

//            }
//        }

//        public unsafe FileChunkResultCode ReadChunkFromFile(string filePath)
//        {
//            if(string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
//            {
//                return FileChunkResultCode.ChunkReadFileNotExist;
//            }
//            this.filePath = filePath;

//            FileChunkResultCode resultCode = FileChunkResultCode.Success;
//            FileStream fileStream = null;
//            try
//            {
//                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
//                byte[] dataBytes = new byte[FileFragment.LENGTH];
//                while (fileStream.Read(dataBytes, 0, dataBytes.Length) != FileFragment.LENGTH)
//                {
//                    fixed(byte* b = &dataBytes[0])
//                    {
//                        int pathLen = *((int*)b);
//                        string path = null;
//                        if(pathLen>0)
//                        {
//                            path = Encoding.UTF8.GetString(dataBytes, sizeof(int), pathLen);
//                        }else if(usedMode == FileUsedMode.Read)
//                        {
//                            continue;
//                        }
//                        long start = *((long*)(b + sizeof(int) + FileFragment.LENGTH));
//                        long length = *((long*)(b + sizeof(int) + FileFragment.LENGTH+sizeof(long)));
//                        long size = *((long*)(b + sizeof(int) + FileFragment.LENGTH + sizeof(long)*2));

//                        FileFragment fragment = new FileFragment()
//                        {
//                            Path = path,
//                            Start = start,
//                            Length = length,
//                            Size = size,
//                        };
//                        fragments.Add(fragment);
//                        if(!string.IsNullOrEmpty(path))
//                        {
//                            contentFragmentDic.Add(path, fragment);
//                        }
//                    }
//                }
//                fileStream.Close();
//            }catch
//            {
//                if(fileStream!=null)
//                {
//                    fileStream.Close();
//                }
//                resultCode = FileChunkResultCode.ChunkReadFileError;
//            }
//            return resultCode;
//        }

//        public FileChunkResultCode SaveToFile()
//        {
//            if(usedMode == FileUsedMode.Read)
//            {
//                return FileChunkResultCode.ChunkReadFileError;
//            }
//            if(string.IsNullOrEmpty(filePath))
//            {
//                return FileChunkResultCode.ChunkSaveFilePathError;
//            }

//            FileChunkResultCode resultCode = FileChunkResultCode.Success;

//            FileStream fileStream = null;
//            try
//            {
//                fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
//                if(fragments.Count>0)
//                {
//                    for(int i =0;i<fragments.Count;++i)
//                    {
//                        FileFragment fragment = fragments[i];
//                        byte[] pathBytes = new byte[128];
//                        int pathLen = 0;
//                          if(!string.IsNullOrEmpty(fragment.Path))
//                        {
//                            pathLen = Encoding.UTF8.GetBytes(fragment.Path, 0, fragment.Path.Length, pathBytes, 0);
//                        }
//                        byte[] pathLenBytes = BitConverter.GetBytes(pathLen);
//                        byte[] startBytes = BitConverter.GetBytes(fragment.Start);
//                        byte[] lengthBytes = BitConverter.GetBytes(fragment.Length);
//                        byte[] sizeBytes = BitConverter.GetBytes(fragment.Size);

//                        fileStream.Write(pathLenBytes, 0, pathLenBytes.Length);
//                        fileStream.Write(pathBytes, 0, pathBytes.Length);
//                        fileStream.Write(startBytes, 0, startBytes.Length);
//                        fileStream.Write(lengthBytes, 0, lengthBytes.Length);
//                        fileStream.Write(sizeBytes, 0, sizeBytes.Length);
//                    }
//                    fileStream.Flush();
//                }
//                fileStream.Close();
//            }catch
//            {
//                if(fileStream!=null)
//                {
//                    fileStream.Close();
//                }
//                resultCode = FileChunkResultCode.ChunkSaveFileError;
//            }
//            return resultCode;
//        }

//        internal FileFragment GetContent(string path)
//        {
//            if(contentFragmentDic.TryGetValue(path,out FileFragment data))
//            {
//                return data;
//            }
//            return null;
//        }

//        internal FileFragment GetEmpty(long length)
//        {
//            long size = GetSize(length);

//        }

//        internal void RemoveContent(string path)
//        {
//            if (contentFragmentDic.TryGetValue(path, out FileFragment data))
//            {
//                contentFragmentDic.Remove(path);
//                data.Path = null;
//                data.Length = 0;
//            }
//        }

//        private long GetSize(long length)
//        {
//            long result = length;
//            while (true)
//            {
//                if (result % 4 == 0)
//                {
//                    break;
//                }
//                ++result;
//            }
//            return result;
//        }

//        //---------------
//        private void AddFragment(long start, long size)
//        {
//            int insertIndex = BinarySearchFragmentIndex(0, fragmentDatas.Count - 1, start);
//            FragmentData preFragmentData = insertIndex == 0 ? null : fragmentDatas[insertIndex - 1];
//            FragmentData nextFragmentData = insertIndex == fragmentDatas.Count ? null : fragmentDatas[insertIndex];
//            bool isMergePre = false;
//            if(preFragmentData!=null && preFragmentData.Start+preFragmentData.Size == start)
//            {
//                isMergePre = true;
//                preFragmentData.Size += size;
//            }
//            bool isMergeNext = false;
//            if(nextFragmentData!=null && start+size == nextFragmentData.Start)
//            {
//                isMergeNext = true;
//                if(isMergePre)
//                {
//                    preFragmentData.Size += nextFragmentData.Size;
//                    fragmentDatas.RemoveAt(insertIndex);
//                }else
//                {
//                    nextFragmentData.Start -= size;
//                }
//            }
//            if(!isMergeNext && !isMergePre)
//            {
//                FragmentData fragment = new FragmentData()
//                {
//                    Start = start,
//                    Size = size,
//                };
//                fragmentDatas.Insert(insertIndex, fragment);
//            }
//        }

//        private int BinarySearchFragmentIndex(int low,int high,long value)
//        {
//            FragmentData lowData = fragmentDatas[low];
//            if(lowData.Start>value)
//            {
//                return low;
//            }

//            FragmentData highData = fragmentDatas[high];
//            if(highData.Start<value)
//            {
//                return high+1;
//            }

//            int mid = (low + high) / 2;
//            if(mid == low)
//            {
//                return mid + 1;
//            }
//            FragmentData midData = fragmentDatas[mid];

//            if(midData.Start>value)
//            {
//                return BinarySearchFragmentIndex(low, mid,value);
//            }else if(midData.Start < value)
//            {
//                return BinarySearchFragmentIndex(mid, high, value);
//            }
//            else
//            {
//                return mid;
//            }
//        }

        

//    }
//}
