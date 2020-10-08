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

        static void Main(string[] args)
        {
            var output = new Output();
            args = ArgsToLower(args);

            output.Show("RandomFiles");
            output.Show("----------------\n");

            if (args.Contains("--help") || args.Contains("-h"))
            {
                output.Show(Help.MainHelp());
                return;
            }

            if (args.Length == 0)
            {
                output.Error("You didn't set the source folder.");
                return;
            }

            string source = args[0];
            if (!Directory.Exists(source))
            {
                output.Error("The source folder does not exist.");
                return;
            }
            source = Path.GetFullPath(source);

            // size in MB
            long size = GetSize(args, output);

            string[] types = GetTypes(args, output);
            var f = new FileList(source);
            f.LoadFilesList(types);

            List<FileItem> selectedFiles = f.GetRandomFileList(size);

            var fileOps = new FileOperations();

            // DELETE
            if (args.Contains("--delete"))
            {
                fileOps.DeleteFiles(selectedFiles);
                output.Show("Delete is done.");
                return;
            }
            // COPY
            else
            {
                string destination;
                destination = (ARG_SWITCHES.Contains(args[1]))
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
                return;
            }
        }

        private static string[] ArgsToLower(string[] args)
        {
            for (int i =0; i < args.Length; i++)
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
        /// <param name="args"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        private static string[] GetTypes(string[] args, Output output)
        {
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
