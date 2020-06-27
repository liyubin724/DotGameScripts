using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.FS
{
    public enum FileSystemMode
    {
        Read,
        Create,
        Update,
    }

    public enum FileSystemResultCode
    {
        Success = 0,

        UnknownError = -1,
        ModeNotExistError = -2,

        ContentFileNotExistError = -100,
        ContentHasBeenOpenedError = -101,
        ContentOpenError = -102,

        ChunkByteLengthError = -200,
        ChunkPathSizeError = -201,
        ChunkByteTooLongError = -202,
        ChunkDataByteLengthError = -203,
        ChunkDataCountError = -204,

        FragmentByteLengthError = -300,
        FragmentDataByteLengthError = -301,
        FragmentDataCountError = -302,

        IndexFileNotExistError = -400,
        IndexStreamError = -401,
    }

    public class FileSystem
    {
        public string Name { get; private set; }
        public FileSystemMode Mode { get; private set; }

        public string ContentFilePath { get; private set; }
        public string IndexFilePath { get; private set; }

        private FileContent content = null;
        private FileChunk chunk = null;
        private FileFragment fragment = null;

        public FileSystem(string name)
        {
            Name = name;
        }

        public FileSystemResultCode Open(FileSystemMode mode, string contentPath, string indexPath)
        {
            Mode = mode;
            ContentFilePath = contentPath;
            IndexFilePath = indexPath;

            if (Mode != FileSystemMode.Create && !File.Exists(ContentFilePath))
            {
                return FileSystemResultCode.ContentFileNotExistError;
            }
            if (Mode != FileSystemMode.Create && !File.Exists(IndexFilePath))
            {
                return FileSystemResultCode.IndexFileNotExistError;
            }

            content = new FileContent();
            chunk = new FileChunk();

            FileSystemResultCode resultCode = FileSystemResultCode.Success;
            resultCode = content.OpenContent(ContentFilePath, Mode);
            if (resultCode != FileSystemResultCode.Success)
            {
                return resultCode;
            }

            if (Mode != FileSystemMode.Read)
            {
                fragment = new FileFragment();
            }

            if (Mode != FileSystemMode.Create)
            {
                FileStream indexStream = null;
                try
                {
                    indexStream = new FileStream(IndexFilePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    resultCode = chunk.ReadFromStream(indexStream);
                    if (resultCode == FileSystemResultCode.Success && fragment != null)
                    {
                        resultCode = fragment.ReadFromStream(indexStream);
                    }
                } catch
                {
                    resultCode = FileSystemResultCode.IndexFileNotExistError;
                }
                finally
                {
                    if (indexStream != null)
                    {
                        indexStream.Close();
                    }
                }

            }

            return resultCode;
        }

        public bool HasFile(string filePath)
        {
            return chunk.Exist(filePath);
        }

        public byte[] GetFile(string filePath)
        {
            ChunkData chunkData = chunk.Get(filePath);
            if(chunkData == null)
            {
                return new byte[0];
            }

            return content.Read(chunkData.StartPosition, chunkData.ContentLength);
        }

        public void AddFile(string filePath, byte[] fileBytes)
        {
            
        }

        public void DeleteFile(string filePath)
        {
            ChunkData chunkData = chunk.Get(filePath);
            if (chunkData != null)
            {
                fragment.Add(chunkData.StartPosition, chunkData.UsageSize);
            }
        }

        public FileSystemResultCode Save()
        {

        }

    }
}
