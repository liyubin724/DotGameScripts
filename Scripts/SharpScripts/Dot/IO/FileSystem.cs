﻿using Dot.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using Desc = System.ComponentModel.DescriptionAttribute;
using Dot.Core.Util;

namespace Dot.Core.IO
{
    public enum FileSystemState
    {
        None = 0,
        Open,
        Create,
        Update,
    }

    public enum FileSystemErrorCode
    {
        None = 0,
        [Desc("State error")]
        StateError,
        [Desc("File can't found")]
        FilePathNotExistError,
        [Desc("Path is empty")]
        FilePathEmptyError,
        [Desc("Read index failed")]
        ReadIndexError,
        [Desc("Save Index to disk caused a error")]
        SaveIndexError,
        [Desc("Open data exception")]
        DataOpenError,
        [Desc("Create data exception")]
        DataCreateError,
        [Desc("File not saved in index")]
        NotExistInIndexError,
        [Desc("Read byte from data cased a error")]
        ReadFromDataError,
        [Desc("The bytes of the file is Null")]
        FileBytesNullError,
        [Desc("The hashCode is repeated,plz change the file name")]
        HashCodeRepeatError,
    }

    public class FileSystem
    {
        private string dataPath = null;
        private string indexPath = null;
        private string fragmentPath = null;

        private FileStream dataFileStream = null;

        private Dictionary<uint, FileIndex> indexDic = new Dictionary<uint, FileIndex>();
        private FileFragment fragment = null;

        private FileSystemState state = FileSystemState.None;
        private bool isChanged = false;

        public FileSystemErrorCode ErrorCode { get; private set; } = FileSystemErrorCode.None;
        
        public FileSystem(string dataPath, string indexPath, string fragmentPath)
        {
            this.dataPath = dataPath;
            this.indexPath = indexPath;
            this.fragmentPath = fragmentPath;
        }

        public bool Open()
        {
            if(state != FileSystemState.None)
            {
                ErrorCode = FileSystemErrorCode.StateError;
                return false;
            }
            state = FileSystemState.Open;
            if (string.IsNullOrEmpty(dataPath)||string.IsNullOrEmpty(indexPath)||string.IsNullOrEmpty(fragmentPath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return false;
            }
            if(!File.Exists(dataPath) || !File.Exists(indexPath))
            {
                ErrorCode = FileSystemErrorCode.FilePathNotExistError;
                return false;
            }

            if(!ReadFileIndex())
            {
                ErrorCode = FileSystemErrorCode.ReadIndexError;
                return false;
            }

            try
            {
                dataFileStream = new FileStream(dataPath, FileMode.Open, FileAccess.Read);
            }catch
            {
                Close();
                ErrorCode = FileSystemErrorCode.DataOpenError;
                return false;
            }

            return true;
        }

        public bool Create()
        {
            if(state!= FileSystemState.None)
            {
                ErrorCode = FileSystemErrorCode.StateError;
                return false;
            }
            state = FileSystemState.Create;
            if (string.IsNullOrEmpty(dataPath) || string.IsNullOrEmpty(indexPath) || string.IsNullOrEmpty(fragmentPath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return false;
            }
            if(File.Exists(dataPath))
            {
                File.Delete(dataPath);
            }
            if(File.Exists(indexPath))
            {
                File.Delete(indexPath);
            }
            if(File.Exists(fragmentPath))
            {
                File.Delete(fragmentPath);
            }

            try
            {
                dataFileStream = new FileStream(dataPath, FileMode.Create, FileAccess.Write);
            }catch
            {
                Close();
                ErrorCode = FileSystemErrorCode.DataCreateError;
                return false;
            }

            return true;
        }

        public bool Update()
        {
            if(state != FileSystemState.None)
            {
                ErrorCode = FileSystemErrorCode.StateError;
                return false;
            }
            state = FileSystemState.Update;
            if (string.IsNullOrEmpty(dataPath) || string.IsNullOrEmpty(indexPath) || string.IsNullOrEmpty(fragmentPath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return false;
            }
            if (!File.Exists(dataPath) || !File.Exists(indexPath))
            {
                ErrorCode = FileSystemErrorCode.FilePathNotExistError;
                return false;
            }
            if (!ReadFileIndex())
            {
                ErrorCode = FileSystemErrorCode.ReadIndexError;
                return false;
            }

            fragment = new FileFragment();
            fragment.ReadFragment(fragmentPath);

            try
            {
                dataFileStream = new FileStream(dataPath, FileMode.Open, FileAccess.Write);
            }
            catch
            {
                Close();
                ErrorCode = FileSystemErrorCode.DataOpenError;
                return false;
            }
            return true;
        }

        public bool Exist(string filePath)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            string lowerFilePath = filePath.ToLower();
            uint hashcode = StringUtil.GetHashByTime33(lowerFilePath);
            if(indexDic.ContainsKey(hashcode))
            {
                return true;
            }
            return false;
        }

        public int Count()
        {
            return indexDic.Count;
        }

        public byte[] Read(string filePath)
        {
            if(state!= FileSystemState.Open)
            {
                ErrorCode = FileSystemErrorCode.StateError;
                return null;
            }

            if(string.IsNullOrEmpty(filePath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return null;
            }
            string lowerFilePath = filePath.ToLower();
            uint hashcode = StringUtil.GetHashByTime33(lowerFilePath);
            if(indexDic.TryGetValue(hashcode,out FileIndex indexData))
            {
                byte[] bytes = new byte[indexData.length];
                try
                {
                    dataFileStream.Seek(indexData.start, SeekOrigin.Begin);
                    int len = dataFileStream.Read(bytes, 0, (int)indexData.length);
                    return bytes;
                }
                catch
                {
                    ErrorCode = FileSystemErrorCode.ReadFromDataError;
                }
            }else
            {
                ErrorCode = FileSystemErrorCode.NotExistInIndexError;
            }
            return null;
        }

        public bool Write(string filePath, byte[] fileBytes)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return false;
            }

            if(fileBytes == null || fileBytes.Length == 0)
            {
                ErrorCode = FileSystemErrorCode.FileBytesNullError;
                return false;
            }

            if(state == FileSystemState.Create || state == FileSystemState.Update)
            {
                string lowerFilePath = filePath.ToLower();
                uint hashcode = StringUtil.GetHashByTime33(lowerFilePath);
                if (indexDic.TryGetValue(hashcode, out FileIndex indexData))
                {
                    Delete(hashcode);
                }
                return Write(hashcode, fileBytes);
            }else
            {
                ErrorCode = FileSystemErrorCode.StateError;
            }
            return false;
        }

        private bool Write(uint filePath,byte[] fileBytes)
        {
            FileIndex indexData = new FileIndex();
            indexData.length = fileBytes.Length;
            indexData.size = GetFileSize(fileBytes.Length);
            FileFragmentData fragmentData = null;
            if(state == FileSystemState.Update)
            {
                fragmentData = fragment.GetFragment(indexData.size);
            }
            if(fragmentData == null)
            {
                dataFileStream.Seek(0, SeekOrigin.End);
                indexData.start = (int)dataFileStream.Position;
            }else
            {
                indexData.start = fragmentData.start;
                if(fragmentData.size == indexData.size)
                {
                    fragment.DeleteFragment(fragmentData);
                }else
                {
                    fragmentData.start += indexData.size;
                    fragmentData.size -= indexData.size;
                }
            }
            dataFileStream.Seek(indexData.start, SeekOrigin.Begin);
            dataFileStream.Write(fileBytes, 0, fileBytes.Length);
            for(int i=0;i<indexData.size - indexData.length;i++)
            {
                dataFileStream.WriteByte(0);
            }

            indexDic.Add(filePath, indexData);
            isChanged = true;
            return true;
        }

        public bool Delete(string filePath)
        {
            if (state != FileSystemState.Update)
            {
                ErrorCode = FileSystemErrorCode.StateError;
                return false;
            }

            if(string.IsNullOrEmpty(filePath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return false;
            }

            string lowerFilePath = filePath.ToLower();
            uint hashcode = StringUtil.GetHashByTime33(lowerFilePath);
            return Delete(hashcode);
        }

        private bool Delete(uint filePath)
        {
            if (indexDic.TryGetValue(filePath, out FileIndex indexData))
            {
                fragment.AddFragment(indexData);
                indexDic.Remove(filePath);
                isChanged = true;

                return true;
            }else
            {
                ErrorCode = FileSystemErrorCode.NotExistInIndexError;
            }
            return false;
        }

        public bool Rename(string oldFilePath, string newFilePath)
        {
            if (string.IsNullOrEmpty(oldFilePath) || string.IsNullOrEmpty(newFilePath))
            {
                ErrorCode = FileSystemErrorCode.FilePathEmptyError;
                return false;
            }
            if(state == FileSystemState.Update)
            {
                string oldLowerFilePath = oldFilePath.ToLower();
                uint hashcode = StringUtil.GetHashByTime33(oldLowerFilePath);
                if (indexDic.TryGetValue(hashcode, out FileIndex indexData))
                {
                    string newLowerFilePath = newFilePath.ToLower();
                    uint nHashcode = StringUtil.GetHashByTime33(newLowerFilePath);
                    if (indexDic.ContainsKey(nHashcode))
                    {
                        ErrorCode = FileSystemErrorCode.HashCodeRepeatError;
                        return false;
                    }

                    indexDic.Remove(hashcode);
                    indexDic.Add(nHashcode, indexData);

                    isChanged = true;

                    return true;
                }else
                {
                    ErrorCode = FileSystemErrorCode.NotExistInIndexError;
                    return false;
                }
            }else
            {
                ErrorCode = FileSystemErrorCode.StateError;
            }
            
            return false;
        }

        public bool Save()
        {
            if(state == FileSystemState.Create || state == FileSystemState.Update)
            {
                if (isChanged)
                {
                    isChanged = false;
                    bool result = SaveFileIndex();
                    if (result && state == FileSystemState.Update)
                    {
                        result = fragment.SaveFragment(fragmentPath);
                    }
                    return result;
                }
                return true;
            }else
            {
                ErrorCode = FileSystemErrorCode.StateError;
                return false;
            }
        }

        public void Close()
        {
            if(isChanged)
            {
                Save();
            }

            if(dataFileStream!=null)
            {
                dataFileStream.Close();
                dataFileStream = null;
            }
            indexDic.Clear();
            fragment = null;
            state = FileSystemState.None;
            
        }

        public string GetErrorMessage()
        {
            return Util.EnumUtil.GetEnumDescription(ErrorCode);
        }

        private unsafe bool ReadFileIndex()
        {
            int indexSize = sizeof(uint) * 4;
            byte[] buffer = new byte[indexSize];
            FileStream indexFS = null;
            bool result = true;
            try
            {
                indexFS = new FileStream(indexPath, FileMode.Open, FileAccess.Read);
                while(true)
                {
                    int size = indexFS.Read(buffer, 0, indexSize);
                    if(size<=0)
                    {
                        break;
                    }else if(size!=indexSize)
                    {
                        indexDic = null;
                        result = false;
                        break;
                    }

                    FileIndex index = new FileIndex();
                    uint filePath = 0;
                    fixed (byte* b = &buffer[0])
                    {
                        filePath = *((uint*)b);
                    }
                    int byteOffset = sizeof(uint);
                    fixed (byte* b = &buffer[byteOffset])
                    {
                        index.start = *((int*)b);
                    }
                    byteOffset += sizeof(int);
                    fixed (byte* b = &buffer[byteOffset])
                    {
                        index.length = *((int*)b);
                    }
                    byteOffset += sizeof(int);
                    fixed (byte* b = &buffer[byteOffset])
                    {
                        index.size = *((int*)b);
                    }
                    indexDic.Add(filePath, index);
                }
            }catch(Exception e)
            {
                result = false;
                ErrorCode = FileSystemErrorCode.ReadIndexError;
            }finally
            {
                if(indexFS!=null)
                {
                    indexFS.Close();
                    indexFS = null;
                }
            }

            return result;
        }
        
        private bool SaveFileIndex()
        {
            if(state!= FileSystemState.Create && state != FileSystemState.Update)
            {
                return false;
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(indexPath, FileMode.Create, FileAccess.Write);
                foreach (var kvp in indexDic)
                {
                    byte[] pathBytes = BitConverter.GetBytes(kvp.Key);
                    fs.Write(pathBytes, 0, pathBytes.Length);
                    byte[] startBytes = BitConverter.GetBytes(kvp.Value.start);
                    fs.Write(startBytes, 0, startBytes.Length);
                    byte[] lenBytes = BitConverter.GetBytes(kvp.Value.length);
                    fs.Write(lenBytes, 0, lenBytes.Length);
                    byte[] sizeBytes = BitConverter.GetBytes(kvp.Value.size);
                    fs.Write(sizeBytes, 0, sizeBytes.Length);
                }
                fs.Flush();
                fs.Close();
                fs = null;
            }
            catch
            {
                ErrorCode = FileSystemErrorCode.SaveIndexError;
                if (fs != null)
                {
                    fs.Close();
                    fs = null;
                }
                return false;
            }
            return true;
        }

        private int GetFileSize(int len)
        {
            int result = len;
            while(true)
            {
                if(result%4==0)
                {
                    break;
                }
                ++result;
            }
            return result;
        }
    }
}
