using System;
using System.Collections.Generic;
using System.IO;

namespace RandomFiles
{
    class FileOperations
    {
        private int numberOfDoneFiles;
        private string currentFile;

        public int NumberOfDoneFiles { get => numberOfDoneFiles; }
        public string CurrentFile { get => currentFile; }

        public FileOperations()
        {
            numberOfDoneFiles = 0;
            currentFile = "";
        }

        public void DeleteFiles(List<FileItem> files)
        {
            numberOfDoneFiles = 0;
            var output = new Output();

            files.ForEach(item =>
            {
                try
                {
                    currentFile = item.Path;
                    File.Delete(item.Path);
                }
                catch (Exception ex)
                {
                    output.Error($"File: {Path.GetFileName(item.Path)} - " +
                        ex.Message);
                }
                finally
                {
                    numberOfDoneFiles++;
                }

            });
        }

        public void CopyFiles(List<FileItem> files, string destination)
        {
            numberOfDoneFiles = 0;
            var output = new Output();

            files.ForEach(item =>
            {
                try
                {
                    currentFile = item.Path;
                    File.Copy(item.Path,
                        Path.Combine(
                            destination,
                            Path.GetFileName(item.Path)));
                }
                catch (Exception ex)
                {
                    output.Error($"File: {Path.GetFileName(item.Path)} - " +
                        ex.Message);
                }
                finally
                {
                    numberOfDoneFiles++;
                }
            });
        }
    }
}
