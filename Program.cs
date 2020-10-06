using System;
using System.IO;
using System.Linq;

namespace RandomFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new Output();

            output.Show( "RandomFiles" );
            output.Show( "----------------" );

            if (args.Length == 0) {
                output.Show( "Error: You didn't set the source folder." );
                return;
            }

            if (args.Contains("--help") || args.Contains("-h")) {
                Console.WriteLine( Help.MainHelp() );
                return;
            }

            int size = 1024;
            string currentPath = Path.GetFullPath( "." );
            string source = args[0];

            //if (Path.ex)

            string destination = currentPath;

            Console.WriteLine( $"current path: {currentPath}." );



            Console.WriteLine( $"arg: {args[0]}" );


        }
    }
}
