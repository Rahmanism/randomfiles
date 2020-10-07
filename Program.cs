﻿using System;
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
            "--delete"
        };

        static void Main(string[] args)
        {
            var output = new Output();

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

            var f = new FileList(source);
            f.LoadFilesList();

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

                fileOps.CopyFiles(selectedFiles, destination);
                output.Show("Copying is done.");
                return;
            }
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
    }
}
