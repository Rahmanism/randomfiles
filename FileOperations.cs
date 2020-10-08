using System;
using System.Collections.Generic;
using System.IO;

namespace RandomFiles
{
    class FileOperations
    {
        public int NumberOfDoneFiles { get; private set; }
        public string CurrentFile { get; private set; }

        public FileOperations()
        {
            NumberOfDoneFiles = 0;
            CurrentFile = "";
        }

        public void DeleteFiles(List<FileItem> files)
        {
            NumberOfDoneFiles = 0;
            var output = new Output();

            files.ForEach((Action<FileItem>)(item =>
            {
                try
                {
                    CurrentFile = item.Path;
                    File.Delete(item.Path);
                }
                catch (Exception ex)
                {
                    output.Error($"File: {Path.GetFileName(item.Path)} - " +
                        ex.Message);
                }
                finally
                {
                    NumberOfDoneFiles++;
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
            NumberOfDoneFiles = 0;
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
                    NumberOfDoneFiles++;
                }
            }));
        }
    }
}
