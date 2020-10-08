using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void LoadFilesList(string[] types)
        {
            files = new List<FileItem>();

            if (!Directory.Exists(sourcePath))
            {
                throw new DirectoryNotFoundException(
                    "The source destination does not exist.");
            }

            GetChildren(sourcePath, types);

            if (files.Count < 1)
            {
                throw new Exception("No files in the source!");
            }
        }

        /// <summary>
        /// Fills the "files" with the files of a folder with search pattern.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="pattern"></param>
        private void GetFilesOfDir(string dir, string pattern)
        {
            foreach (string f in Directory.GetFiles(
                dir, pattern, SearchOption.AllDirectories))
            {
                var info = new FileInfo(f);
                files.Add(new FileItem
                {
                    Path = f,
                    Size = info.Length
                });
            }
        }

        /// <summary>
        /// Fills the "files" of the given directory.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="types">Some file exts like mp3, jpg, etc.</param>
        private void GetChildren(string dir, string[] types)
        {
            try
            {
                if (types.Length > 0)
                {
                    foreach (var type in types)
                    {
                        GetFilesOfDir(dir, $"*.{type}");
                    }
                }
                else
                {
                    GetFilesOfDir(dir, "");
                }
            }
            catch { }
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
                throw new Exception("No file is selected.");
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
