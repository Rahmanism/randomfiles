using System;
using System.Collections.Generic;
using System.IO;

namespace RandomFiles
{
    class FileOperations
    {
        public int DoneFilesCount { get; private set; }
        public string CurrentFile { get; private set; }
        public long DoneFilesSize { get; private set; }

        public string Source { get; set; }
        public bool KeepEmptyFolders { get; set; }

        public FileOperations()
        {
            DoneFilesCount = 0;
            CurrentFile = "";
            DoneFilesSize = 0;
        }

        public void DeleteFiles(List<FileItem> files)
        {
            DoneFilesCount = 0;
            DoneFilesSize = 0;
            var output = new Output();

            files.ForEach((Action<FileItem>)(item =>
            {
                try
                {
                    CurrentFile = item.Path;
                    File.Delete(item.Path);
                    if (!KeepEmptyFolders)
                    {
                        var dir = Path.GetDirectoryName(item.Path);
                        while (dir != Source)
                        {
                            bool empty =
                                Directory.GetFiles(
                                    dir, "",
                                    SearchOption.AllDirectories).Length < 1;
                            if (empty)
                            {
                                Directory.Delete(dir, true);
                            }
                            dir = Path.GetFullPath(Path.Combine(dir, ".."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    output.Error($"File: {Path.GetFileName(item.Path)} - " +
                        ex.Message);
                }
                finally
                {
                    DoneFilesCount++;
                    DoneFilesSize += item.Size;
                }
            }));
        }

        /// <summary>
        /// Copies the files to the destination
        /// </summary>
        /// <param name="files">List of files to be copied</param>
        /// <param name="destination">The destination path</param>
        /// <param name="sameFolder">
        /// Copy all files in the same folder if true
        /// Or make the folder structure based on source path of files if false.
        /// </param>
        /// <param name="sourcePath">
        /// If sameFolder is false, this path will be used as root for making
        /// folder structure in the destination.
        /// </param>
        public void CopyFiles(
            List<FileItem> files,
            string destination,
            bool sameFolder,
            string sourcePath = "")
        {
            DoneFilesCount = 0;
            DoneFilesSize = 0;
            var output = new Output();

            files.ForEach((Action<FileItem>)(item =>
            {
                try
                {
                    CurrentFile = item.Path;

                    string destDir = destination;

                    if (!sameFolder)
                    {
                        string relativeDestDir = Path.GetRelativePath(
                            sourcePath, Path.GetDirectoryName(item.Path));

                        destDir = Path.Combine(destination, relativeDestDir);

                        if (!Directory.Exists(destDir))
                        {
                            Directory.CreateDirectory(destDir);
                        }
                    }

                    File.Copy(item.Path,
                        Path.Combine(
                            destDir,
                            Path.GetFileName(item.Path)));
                }
                catch (Exception ex)
                {
                    output.Error($"File: {Path.GetFileName(item.Path)} - " +
                        ex.Message);
                }
                finally
                {
                    DoneFilesCount++;
                    DoneFilesSize += item.Size;
                }
            }));
        }
    }
}
