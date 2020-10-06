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
                output.Error( "You didn't set the source folder." );
                return;
            }

            if (args.Contains("--help") || args.Contains("-h")) {
                output.Show( Help.MainHelp() );
                return;
            }

            int size = 1024;
            string currentPath = Path.GetFullPath( "." );
            string source = args[0];

            if ( !Directory.Exists(source) ) {
                output.Error( "The source destination does not exist." );
                return;
            }

            output.Show( Path.GetFullPath( source ) );

            string destination = currentPath;

            Console.WriteLine( $"current path: {currentPath}." );



            Console.WriteLine( $"arg: {args[0]}" );


        }
    }
}
