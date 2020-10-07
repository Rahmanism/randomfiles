using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RandomFiles
{
    class FileList
    {
        List<FileItem> files;
        string sourcePath;

        public FileList(string sourceFolder)
        {
            sourcePath = sourceFolder;
            files = new List<FileItem>();
        }

        public void LoadFilesList()
        {
            files = new List<FileItem>();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException(
                    "The source destination does not exist.");
            }

            GetChildren(sourcePath);

            if (files.Count < 1)
            {
                throw new Exception("No files in the source!");
            }
        }

        private void GetChildren(string dir)
        {
            try
            {
                foreach (string f in Directory.GetFiles(dir))
                {
                    var info = new FileInfo(f);
                    files.Add(new FileItem
                    {
                        Path = f,
                        Size = info.Length
                    });
                }
                foreach (string d in Directory.GetDirectories(dir))
                {
                    GetChildren(d);
                }
            }
            catch {}
        }

        /// <summary>
        /// Gets a list of files randomly.
        /// The total size of the generated list will less than
        /// or equal to the size.
        /// </summary>
        /// <param name="size">Size of the generated list in MB</param>
        /// <returns></returns>
        public List<FileItem> GetRandomFileList(long size = 1024)
        {
            var selectedFiles = new List<FileItem>();
            if (files.Count < 1)
            {
                LoadFilesList();
            }

            var random = new Random();
            long totalSelectedSize = 0;
            var maxSize = size * 1048576; // to change it to bytes
            var maxTries = files.Count;
            var tries = 0;

            while (selectedFiles.Count < files.Count &&
                totalSelectedSize <= maxSize &&
                tries <= maxTries)
            {
                int itemIndex = random.Next(files.Count);
                FileItem item = files[itemIndex];
                if (selectedFiles.Any(i => i.Path == item.Path))
                {
                    continue;
                }
                tries++;
                if ((totalSelectedSize + item.Size) <= maxSize)
                {
                    selectedFiles.Add(item);
                    totalSelectedSize += item.Size;
                }
            }

            return selectedFiles;
        }
    }
}
