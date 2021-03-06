﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RandomFiles
{
    class Help
    {
        public static string MainHelp()
        {
            string mainHelpText = @"
RandomFiles
The goal is to copy random files from a given path till it reaches the specified size to a destination.
Another function is to delete random files in the given path till it reaches the specified size.


Syntax:

To copy:
    randomfiles <source_folder> [destination] [--size nn] [--type <type1,type2...>] [--same-folder]

You can set the type of files by file extension using --type switch.
Example:
    randomfiles music d:\ --size 500 --type mp3,ogg

--same-folder:
    If you use this switch, all files will be copied in the same folder at the destination regardless of their folder structure in the source.
    By default the directory structre will be created at the destination.



To delete:
    randomfiles <source_folder> [--size nn] --delete [--keep-empty-folders]

--keep-empty-folders:
    It will keeps the empty folders after deleting all files in a folder.
    By default if a folder becomes empty after deleting its files, the folder will be deleted.


- The default size is 1024.
- The size is in MB.
- The default destination is current folder.
            ";

            return mainHelpText;
        }
    }
}
