using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RandomFiles
{
    class Program
    {
        const long DEFAULT_SIZE = 1024;
        static readonly string[] ARG_SWITCHES =
        {
            "-h",
            "--help",
            "--size",
            "--delete",
            "--type",
            "--same-folder"
        };

        static string[] args;

        static void Main(string[] mainArgs)
        {
            args = ArgsToLower(mainArgs);
            Greeting();

            if (!ReadyToContinue())
                return;

            string source = args[0];
            source = Path.GetFullPath(source);

            List<FileItem> selectedFiles =
                GetSelectedFiles(source);

            // DELETE
            if (args.Contains("--delete"))
            {
                DoDelete(selectedFiles);
            }
            // COPY
            else
            {
                DoCopy(source, selectedFiles);
            }
        }

        /// <summary>
        /// Gets the list of files.
        /// </summary>
        /// <param name="source">Source path</param>
        /// <returns>
        /// Gets the list of files to operate on.
        /// </returns>
        private static List<FileItem> GetSelectedFiles(string source)
        {
            var output = new Output();

            // size in MB
            long size = GetSize(args, output);

            string[] types = GetTypes();
            var f = new FileList(source);
            f.LoadFilesList(types);

            return f.GetRandomFileList(size);
        }

        /// <summary>
        /// Checks if program should continue to main operations
        /// or stop because of lacking some args or just showing help.
        /// </summary>
        /// <returns>
        /// True if there's no problem to continue
        /// False if it has help switch, or there's no source path.
        /// </returns>
        private static bool ReadyToContinue()
        {
            var output = new Output();

            if (args.Contains("--help") || args.Contains("-h"))
            {
                output.Show(Help.MainHelp());
                return false;
            }

            if (args.Length == 0)
            {
                output.Error("You didn't set the source folder.");
                return false;
            }

            if (!Directory.Exists(args[0]))
            {
                output.Error("The source folder does not exist.");
                return false;
            }

            return true;
        }

        private static void DoDelete(List<FileItem> selectedFiles)
        {
            var output = new Output();
            var fileOps = new FileOperations();
            fileOps.DeleteFiles(selectedFiles);
            output.Show("Delete is done.");
        }

        private static void DoCopy(string source, List<FileItem> selectedFiles)
        {
            var output = new Output();
            var fileOps = new FileOperations();
            string destination = (ARG_SWITCHES.Contains(args[1]))
                ? Path.GetFullPath(".") : args[1];

            if (!Directory.Exists(destination))
            {
                output.Error("The destination folder does not exist.");
                return;
            }

            if (source == destination)
            {
                output.Error("Source and destination are the same.");
                return;
            }

            bool sameFolder = args.Contains("--same-folder");

            fileOps.CopyFiles(
                selectedFiles, destination, sameFolder, source);
            output.Show("Copying is done.");
        }

        private static void Greeting()
        {
            var output = new Output();
            output.Show("RandomFiles");
            output.Show("----------------\n");
        }

        private static string[] ArgsToLower(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToLower();
            }

            return args;
        }

        /// <summary>
        /// Gets the size from args or returns the default value
        /// </summary>
        /// <param name="args"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private static long GetSize(string[] args, Output output)
        {
            long size = DEFAULT_SIZE;
            int sizeParamIndex = Array.IndexOf(args, "--size");
            if (sizeParamIndex >= 0)
            {
                try
                {
                    size = long.Parse(args[sizeParamIndex + 1]);
                }
                catch (Exception ex)
                {
                    output.Error(ex.Message);
                    throw new Exception("The size is not given properly.");
                }
            }

            return size;
        }

        /// <summary>
        /// Gets the file extensions from args.
        /// </summary>
        /// <returns>
        /// Array of string that includes file extensions
        /// or an array with zero length.
        /// </returns>
        private static string[] GetTypes()
        {
            var output = new Output();

            int typesParamIndex = Array.IndexOf(args, "--type");
            if (typesParamIndex >= 0)
            {
                try
                {
                    var types = args[typesParamIndex + 1].Split(",");
                    return types;
                }
                catch (Exception ex)
                {
                    output.Error(ex.Message);
                    throw new Exception("We have a problem with types!");
                }
            }
            else
            {
                return new string[0];
            }
        }
    }
}
