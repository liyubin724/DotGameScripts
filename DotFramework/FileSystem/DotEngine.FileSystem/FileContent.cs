//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DotEngine.FileSystem
//{
//    public enum FileUsedMode
//    {
//        Read = 0,
//        Write ,
//        ReadWrite,
//    }

//    public class FileContent
//    {
//        private FileUsedMode usedMode = FileUsedMode.ReadWrite;
//        private FileChunk fileChunk = null;
//        private FileStream contentStream = null;

//        public FileContent(FileUsedMode mode)
//        {
//            usedMode = mode;
//        }

//        public void OpenFile(string contentFilePath,string chunkFilePath)
//        {

//        }

//        public void CreateFile(string contentFilePath,string chunkFilePath)
//        {

//        }

//        public void OpenOrCreateFile(string contentFilePath,string chunkFilePath)
//        {

//        }

//        public int Count()
//        {

//        }

//        public bool IsExist(string path)
//        {

//        }

//        public bool Write(string path,byte[] bytes)
//        {

//        }

//        public byte[] Read(string path)
//        {

//        }

//        public bool Delete(string path)
//        {

//        }

//        public bool Rename(string oldPath,string newPath)
//        {

//        }
       
//        public bool Save()
//        {

//        }

//        public bool SaveTo()
//        {

//        }

//        public void Dispose()
//        {

//        }



//    }
//}
