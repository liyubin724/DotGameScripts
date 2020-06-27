using DotEngine.FS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateFS();
            UpdateForAddFS();
            ReadAllFile();
        }

        static void UpdateForAddFS()
        {
            string dir = @"D:\CompanySpace\ScorpionClient\trunk\Client\Assets\Script";
            string[] files = (from file in Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories) select file.Replace("\\", "/")).ToArray();

            string contentFilePath = "D:/content.fs";
            string indexFilePath = "D:/index.fs";

            FileSystem fs = new FileSystem("Create");
            fs.Open(FileSystemMode.Update, contentFilePath, indexFilePath);

            foreach (var file in files)
            {
                byte[] bytes = File.ReadAllBytes(file);
                fs.AddFile(Path.GetFileName(file), bytes);
            }

            fs.Close();
        }

        static void UpdateForDeleteFS()
        {
            string contentFilePath = "D:/content.fs";
            string indexFilePath = "D:/index.fs";

            FileSystem fs = new FileSystem("Update");
            fs.Open(FileSystemMode.Update, contentFilePath, indexFilePath);

            string[] files = fs.GetAllFile();
            foreach (var f in files)
            {
                fs.DeleteFile(f);
                break;
            }
            fs.Close();

        }

        static void ReadAllFile()
        {
            string contentFilePath = "D:/content.fs";
            string indexFilePath = "D:/index.fs";

            FileSystem fs = new FileSystem("Create");
            fs.Open(FileSystemMode.Read, contentFilePath, indexFilePath);

            string[] files = fs.GetAllFile();
            foreach(var f in files )
            {
                byte[] bytes = fs.GetFile(f);
                File.WriteAllBytes("D:/test/" + f, bytes);
            }
            fs.Close();
        }

        static void CreateFS()
        {
            string dir = @"D:\WorkSpace\fs";
            string[] files = (from file in Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories) select file.Replace("\\", "/")).ToArray();

            string contentFilePath = "D:/content.fs";
            string indexFilePath = "D:/index.fs";

            FileSystem fs = new FileSystem("Create");
            fs.Open(FileSystemMode.Create, contentFilePath, indexFilePath);

            foreach(var file in files)
            {
                byte[] bytes = File.ReadAllBytes(file);
                fs.AddFile(Path.GetFileName(file), bytes);
            }

            string debugFile = fs.ToString();
            File.WriteAllText("D:/output.txt", debugFile);

            fs.Close();

        }
    }
}
