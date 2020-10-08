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
                    this.CurrentFile = item.Path;
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

        public void CopyFiles(List<FileItem> files, string destination)
        {
            NumberOfDoneFiles = 0;
            var output = new Output();

            files.ForEach((Action<FileItem>)(item =>
            {
                try
                {
                    this.CurrentFile = item.Path;
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
                    NumberOfDoneFiles++;
                }
            }));
        }
    }
}
